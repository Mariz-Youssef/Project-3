using AutoMapper;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Requests;
using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Responses;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Interfaces;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Repositories;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;


namespace ClinicManagementSystem.backend.Features.DepartmentFeature.Services
{
    /// <summary>
    /// Provides department business operations.
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        private readonly IMapper _mapper;

        private readonly IApplicationDbContext _context;


        public DepartmentService(IDepartmentRepository departmentRepository,IApplicationDbContext context, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _context = context;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<DepartmentResponseDto> CreateAsync(CreateDepartmentRequestDto request, CancellationToken cancellationToken = default)
        {
            // Validate the request parameter to ensure it is not null
            ArgumentNullException.ThrowIfNull(request);

            string normalizedName = request.Name.Trim().ToLowerInvariant();

            // Check if a department with the same name already exists
            var departmentExists = await _departmentRepository.ExistsAsync(
                department => department.Name.ToLower() == normalizedName,
                cancellationToken);

            // If a department with the same name exists, throw a ConflictException
            if (departmentExists)
                throw new ConflictException($"Department '{request.Name}' already exists.");

            // Map the request DTO to the Department entity
            Department department = _mapper.Map<Department>(request);

            // Add the new department to the repository
            await _departmentRepository.AddAsync(department, cancellationToken);

            // Save changes to the database
        //  await _saveChanges.SaveChangesAsync(cancellationToken);

          await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DepartmentResponseDto>(department);

        }

        /// <inheritdoc/>
        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            // Validate the department ID to ensure it is greater than zero
            ValidateDepartmentId(id);
            // Retrieve the department by its ID or throw a NotFoundException if it does not exist
            Department department = await GetDepartmentOrThrowAsync(id, cancellationToken);

            // Delete the department from the repository by soft-deleting
            _departmentRepository.Delete(department);

            //  await _saveChanges.SaveChangesAsync(cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            // Validate the department ID to ensure it is greater than zero
            ValidateDepartmentId(id);

            return await _departmentRepository.ExistsAsync(id, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<PagedResult<DepartmentResponseDto>> GetAllAsync(PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            // check if pagination parameters is null and throw an ArgumentNullException if it is
            ArgumentNullException.ThrowIfNull(pagination);

            // Create a query to retrieve all departments, ordered by name
            IQueryable<Department> query = _departmentRepository.Query()
                .OrderBy(d => d.Name);

            // Get the paged result of the query based on the provided pagination parameters
            PagedResult<Department> pagedDepartments = await query.ToPagedResultAsync(pagination, cancellationToken);

            // Map the paged result of departments to a paged result of DepartmentResponseDto and return it
            return new PagedResult<DepartmentResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<DepartmentResponseDto>>(
            pagedDepartments.Items),

                pagination = pagedDepartments.pagination
            };

        }

        /// <inheritdoc/>
        public async Task<DepartmentDetailsResponseDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            // Validate the department ID to ensure it is greater than zero
            ValidateDepartmentId(id);

            //Gets the department details by id from the repository
            DepartmentDetailsResponseDto? department = await _departmentRepository.GetDetailsAsync(id, cancellationToken);

            if (department is null)
            {
                throw new NotFoundException($"Department with ID '{id}' was not found.");
            }

            return department;
        }

        /// <inheritdoc/>
        public async Task<PagedResult<DepartmentResponseDto>> SearchAsync(string searchTerm, PaginationParameters pagination, CancellationToken cancellationToken = default)
        {
            // check if pagination parameters is null and throw an ArgumentNullException if it is
            ArgumentNullException.ThrowIfNull(pagination);

            // Validate that the search term is not null or whitespace.
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw new BadRequestException("Search term cannot be empty.");
            }

            string normalizedSearch = searchTerm.Trim().ToLowerInvariant();

            // Create a query to search for departments whose names contain the normalized search term, ignoring case
            IQueryable<Department> query = _departmentRepository.Query()
                .Where(d => d.Name.ToLower().Contains(normalizedSearch))
                .OrderBy(d => d.Name);

            // Get the paged result of the query based on the provided pagination parameters
            PagedResult<Department> pagedDepartments = await query.ToPagedResultAsync(pagination, cancellationToken);

            // Map the paged result of departments to a paged result of DepartmentResponseDto and return it
            return new PagedResult<DepartmentResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<DepartmentResponseDto>>(pagedDepartments.Items),
                pagination = pagedDepartments.pagination
            };

        }

        /// <inheritdoc/>
        public async Task<DepartmentResponseDto> UpdateAsync(int id, UpdateDepartmentRequestDto request, CancellationToken cancellationToken = default)
        {
            // Validate the department ID to ensure it is greater than zero
            ValidateDepartmentId(id);

            ArgumentNullException.ThrowIfNull(request);

            Department department = await GetDepartmentOrThrowAsync(id, cancellationToken);

            string normalizedName = request.Name.Trim().ToLowerInvariant();

            // Check if a department with the same name already exists, excluding the current department being updated
            bool departmentExists = await _departmentRepository.ExistsAsync(d => d.Id != id && d.Name.ToLower() == normalizedName, cancellationToken);

            if (departmentExists)
            {
                throw new ConflictException($"Department '{request.Name}' already exists.");
            }

            _mapper.Map(request, department);

            _departmentRepository.Update(department);

            //   await _saveChanges.SaveChangesAsync(cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<DepartmentResponseDto>(department);
        }


        /// <summary>
        /// Retrieves a department by its identifier or throws a <see cref="NotFoundException"/>
        /// if the department does not exist.
        /// </summary>
        /// <param name="id">
        /// Department identifier.
        /// </param>
        /// <param name="cancellationToken">
        /// Cancellation token.
        /// </param>
        /// <returns>
        /// The existing department.
        /// </returns>
        /// <exception cref="NotFoundException">
        /// Thrown when the department is not found.
        /// </exception>
        private async Task<Department> GetDepartmentOrThrowAsync(int id, CancellationToken cancellationToken)
        {
            Department? department = await _departmentRepository.GetByIdAsync(id, cancellationToken);

            if (department is null)
            {
                throw new NotFoundException ($"Department with ID '{id}' was not found.");
            }

            return department;
        }

        /// <summary>
        /// Validates the department ID to ensure it is greater than zero.
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="BadRequestException"></exception>
        private static void ValidateDepartmentId(int id)
        {
            if (id <= 0)
            {
                throw new BadRequestException("Department ID must be greater than zero.");

            }
        }
    }
}

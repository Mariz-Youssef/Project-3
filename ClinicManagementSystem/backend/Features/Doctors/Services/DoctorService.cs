using AutoMapper;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Authentication.Interfaces;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Interfaces;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Requests;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Responses;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.backend.Features.Doctors.Services
{
    public class DoctorService: IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthService _userManager;

        public DoctorService(
            IDoctorRepository doctorRepository,
            IDepartmentRepository departmentRepository,
            IApplicationDbContext context,
            IMapper mapper)
        {
            _doctorRepository = doctorRepository;
            _departmentRepository = departmentRepository;
            _context = context;
            _mapper = mapper;
        }
        public async Task<PagedResult<DoctorResponse>> GetAllAsync(PaginationParameters pagination,CancellationToken cancellationToken = default)
        {
            var query = _doctorRepository.GetAllWithDetails();
            var pagedDoctors = await query.ToPagedResultAsync(
                pagination,
                cancellationToken);

            return new PagedResult<DoctorResponse>
            {
                Items = _mapper.Map<IReadOnlyList<DoctorResponse>>(pagedDoctors.Items),
                pagination = pagedDoctors.pagination
            };
        }
        public async Task<DoctorResponse?> GetByIdAsync(int id,CancellationToken cancellationToken = default)
        {
            var doctor = await _doctorRepository.GetByIdWithDetailsAsync(id, cancellationToken);

            if (doctor == null)
                throw new Exception("Doctor not found"); //Not found exception

            return _mapper.Map<DoctorResponse>(doctor);
        }
        public async Task<DoctorResponse> CreateAsync(CreateDoctorRequest request, CancellationToken cancellationToken = default)
        {
            // check if user exists
           // var user = await _userManager.FindByIdAsync(request.UserId.ToString());

            //if (user == null)
            //{
            //    throw new NotFoundException("User not found.");
            //}

            // Check department exists
            if (!await _departmentRepository.ExistsAsync(request.DepartmentId, cancellationToken))
            {
                throw new Exception("Department not found.");
            }

            // Check user isn't already a doctor
            if (await _doctorRepository.UserAlreadyAssignedAsync(
                    request.UserId,
                    cancellationToken))
            {
                throw new Exception("This user is already assigned to a doctor."); // bad request
            }

            // Check license uniqueness
            if (await _doctorRepository.LicenseExistsAsync(
                    request.LicenseNumber,
                    cancellationToken))
            {
                throw new Exception("License number already exists.");
            }

            var doctor = _mapper.Map<Doctor>(request);
            await _doctorRepository.AddAsync(doctor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var createdDoctor = await _doctorRepository.GetByIdWithDetailsAsync(doctor.Id, cancellationToken);

            return _mapper.Map<DoctorResponse>(createdDoctor);
        }
        public async Task<DoctorResponse> UpdateAsync(int id,UpdateDoctorRequest request,CancellationToken cancellationToken = default)
        {
            var doctor = await _doctorRepository
                .GetByIdAsync(id, cancellationToken);

            if (doctor == null)
                throw new Exception("Doctor not found."); //not found exception

            if (!await _departmentRepository.ExistsAsync(request.DepartmentId, cancellationToken))
            {
                throw new Exception("Department not found."); //not found exception
            }

            if (doctor.LicenseNumber != request.LicenseNumber &&
                await _doctorRepository.LicenseExistsAsync(request.LicenseNumber,cancellationToken))
            {
                throw new Exception("License number already exists."); //bad req
            }

            _mapper.Map(request, doctor);
            _doctorRepository.Update(doctor);

            await _context.SaveChangesAsync(cancellationToken);

            var updatedDoctor = await _doctorRepository.GetByIdWithDetailsAsync(id, cancellationToken);

            return _mapper.Map<DoctorResponse>(updatedDoctor);
        }

        public async Task DeleteAsync(int id,CancellationToken cancellationToken = default)
        {
            var doctor = await _doctorRepository
                .GetByIdAsync(id, cancellationToken);

            if (doctor == null)
                throw new Exception("Doctor not found."); //not found

            _doctorRepository.Delete(doctor);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedResult<DoctorResponse>> GetByDepartmentAsync(int departmentId, PaginationParameters pagination,CancellationToken cancellationToken = default)
        {
            var query = _doctorRepository.GetByDepartment(departmentId);
            var pagedDoctors = await query.ToPagedResultAsync(pagination,cancellationToken);

            return new PagedResult<DoctorResponse>
            {
                Items = _mapper.Map<IReadOnlyList<DoctorResponse>>(pagedDoctors.Items),
                pagination = pagedDoctors.pagination
            };
        }

        public async Task<PagedResult<DoctorResponse>> GetBySpecializationAsync(string specialization, PaginationParameters pagination,CancellationToken cancellationToken = default)
        {
            var query = _doctorRepository.GetBySpecialization(specialization);
            var pagedDoctors = await query.ToPagedResultAsync(pagination,cancellationToken);

            return new PagedResult<DoctorResponse>
            {
                Items = _mapper.Map<IReadOnlyList<DoctorResponse>>(pagedDoctors.Items),
                pagination = pagedDoctors.pagination
            };
        }

    }
}

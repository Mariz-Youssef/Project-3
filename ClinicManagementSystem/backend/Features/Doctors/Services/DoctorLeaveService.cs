using AutoMapper;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.Leaves;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Doctors.Services
{
    public class DoctorLeaveService : IDoctorLeaveService
    {
        private readonly IDoctorLeaveRepository _leaveRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DoctorLeaveService(
            IDoctorLeaveRepository leaveRepository,
            IDoctorRepository doctorRepository,
            IApplicationDbContext context,
            IMapper mapper)
        {
            _leaveRepository = leaveRepository;
            _doctorRepository = doctorRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<LeaveResponse>> GetByDoctorAsync(int doctorId,PaginationParameters pagination,CancellationToken cancellationToken = default)
        {
            if (!await _doctorRepository.ExistsAsync(doctorId, cancellationToken))
                throw new NotFoundException("Doctor not found.");

            var query = _leaveRepository
                .GetByDoctor(doctorId)
                .OrderBy(l => l.StartDate);

            var paged = await query.ToPagedResultAsync(pagination,cancellationToken);

            return new PagedResult<LeaveResponse>
            {
                Items = _mapper.Map<IReadOnlyList<LeaveResponse>>(paged.Items),
                pagination = paged.pagination
            };
        }

        public async Task<LeaveResponse?> GetByIdAsync(int doctorId,int id,CancellationToken cancellationToken = default)
        {
            var leave = await _leaveRepository.GetByIdWithDetailsAsync(id,cancellationToken);

            if (leave == null || leave.DoctorId != doctorId)
                throw new NotFoundException("Leave not found.");

            return _mapper.Map<LeaveResponse>(leave);
        }

        public async Task<LeaveResponse> CreateAsync(int doctorId,CreateLeaveRequest request,CancellationToken cancellationToken = default)
        {
            if (!await _doctorRepository.ExistsAsync(doctorId, cancellationToken))
                throw new NotFoundException("Doctor not found.");

            if (request.StartDate > request.EndDate)
                throw new ValidationException("Start date cannot be after end date.");

            if (await _leaveRepository.HasOverlappingLeaveAsync(
                    doctorId,
                    request.StartDate,
                    request.EndDate,
                    null,
                    cancellationToken))
            {
                throw new ConflictException("The leave overlaps with an existing leave.");
            }

            var entity = _mapper.Map<DoctorLeave>(request);

            entity.DoctorId = doctorId;

            await _leaveRepository.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            var created = await _leaveRepository.GetByIdWithDetailsAsync(entity.Id,cancellationToken);

            return _mapper.Map<LeaveResponse>(created);
        }

        public async Task<LeaveResponse> UpdateAsync(int doctorId,int id,UpdateLeaveRequest request,CancellationToken cancellationToken = default)
        {
            var entity = await _leaveRepository.GetByIdAsync(id,cancellationToken);

            if (entity == null || entity.DoctorId != doctorId)
                throw new NotFoundException("Leave not found.");

            if (request.StartDate > request.EndDate)
                throw new ValidationException("Start date cannot be after end date.");

            if (await _leaveRepository.HasOverlappingLeaveAsync(
                    doctorId,
                    request.StartDate,
                    request.EndDate,
                    id,
                    cancellationToken))
            {
                throw new ConflictException("The leave overlaps with an existing leave.");
            }

            _mapper.Map(request, entity);
            _leaveRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var updated = await _leaveRepository.GetByIdWithDetailsAsync(id,cancellationToken);

            return _mapper.Map<LeaveResponse>(updated);
        }

        public async Task DeleteAsync(
            int doctorId,
            int id,
            CancellationToken cancellationToken = default)
        {
            var entity = await _leaveRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (entity == null || entity.DoctorId != doctorId)
                throw new Exception("Leave not found.");

            _leaveRepository.Delete(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

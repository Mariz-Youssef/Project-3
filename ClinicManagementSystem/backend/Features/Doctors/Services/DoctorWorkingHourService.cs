using AutoMapper;
using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Pagination;
using ClinicManagementSystem.backend.Features.Doctors.DTOs.WorkingHours;
using ClinicManagementSystem.backend.Features.Doctors.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Interfaces;

namespace ClinicManagementSystem.backend.Features.Doctors.Services
{
    public class DoctorWorkingHourService : IDoctorWorkingHourService
    {
        private readonly IDoctorWorkingHourRepository _workingHourRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DoctorWorkingHourService(
            IDoctorWorkingHourRepository workingHourRepository,
            IDoctorRepository doctorRepository,
            IApplicationDbContext context,
            IMapper mapper)
        {
            _workingHourRepository = workingHourRepository;
            _doctorRepository = doctorRepository;
            _context = context;
            _mapper = mapper;
        }
        public async Task<PagedResult<WorkingHourResponse>> GetByDoctorAsync(int doctorId,PaginationParameters pagination,CancellationToken cancellationToken = default)
        {
            if (!await _doctorRepository.ExistsAsync(doctorId, cancellationToken))
                throw new NotFoundException("Doctor not found.");

            var query = _workingHourRepository.GetByDoctor(doctorId).OrderBy(x => x.DayOfWeek);

            var pagedResult =await query.ToPagedResultAsync(pagination,cancellationToken);

            return new PagedResult<WorkingHourResponse>
            {
                Items = _mapper.Map<IReadOnlyList<WorkingHourResponse>>(pagedResult.Items),
                pagination = pagedResult.pagination
            };
        }
        public async Task<WorkingHourResponse?> GetByIdAsync(int id,CancellationToken cancellationToken = default)
        {
            var workingHour =await _workingHourRepository.GetByIdWithDetailsAsync(id,cancellationToken);

            if (workingHour == null)
                throw new NotFoundException("Working hour not found.");

            return _mapper.Map<WorkingHourResponse>(workingHour);
        }
        public async Task<WorkingHourResponse> CreateAsync(int doctorId,CreateWorkingHourRequest request,CancellationToken cancellationToken = default)
        {
            if (!await _doctorRepository.ExistsAsync(doctorId,cancellationToken))
            {
                throw new NotFoundException("Doctor not found.");
            }

            if (await _workingHourRepository.ExistsForDayAsync(doctorId,request.DayOfWeek,null,cancellationToken))
            {
                throw new ConflictException("The doctor already has working hours for this day.");
            }

            var entity =_mapper.Map<DoctorWorkingHour>(request);
            entity.DoctorId = doctorId;
            await _workingHourRepository.AddAsync(entity,cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var created =await _workingHourRepository.GetByIdWithDetailsAsync(entity.Id,cancellationToken);
            return _mapper.Map<WorkingHourResponse>(created);
        }
        //we need the record id because a doctor may have more than one record in the table
        public async Task<WorkingHourResponse> UpdateAsync(int doctorId,int id,UpdateWorkingHourRequest request,CancellationToken cancellationToken = default)
        {
            var entity =await _workingHourRepository.GetByIdAsync(id,cancellationToken);

            if (entity == null || entity.DoctorId != doctorId)
                throw new NotFoundException("Working hour not found.");

            if (await _workingHourRepository.ExistsForDayAsync(doctorId,request.DayOfWeek,id,cancellationToken))
            {
                throw new ConflictException("The doctor already has working hours for this day.");
            }

            _mapper.Map(request, entity);
            _workingHourRepository.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var updated = await _workingHourRepository.GetByIdWithDetailsAsync(id,cancellationToken);
            return _mapper.Map<WorkingHourResponse>(updated);
        }
        public async Task DeleteAsync(int doctorId, int id,CancellationToken cancellationToken = default)
        {
            var entity = await _workingHourRepository.GetByIdAsync(id,cancellationToken);

            if (entity == null || entity.DoctorId != doctorId)
                throw new NotFoundException("Working hour not found.");

            _workingHourRepository.Delete(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}

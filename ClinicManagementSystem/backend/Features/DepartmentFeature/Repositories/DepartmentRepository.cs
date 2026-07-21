using ClinicManagementSystem.backend.Data;
using ClinicManagementSystem.backend.Features.DepartmentFeature.DTOs.Responses;
using ClinicManagementSystem.backend.Features.DepartmentFeature.Interfaces;
using ClinicManagementSystem.backend.Models;
using ClinicManagementSystem.backend.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;


namespace ClinicManagementSystem.backend.Features.DepartmentFeature.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {

        // inject the DbContext into the repository
        /// <summary>
        /// Initializes a new instance of the <see cref="DepartmentRepository"/> class.
        /// </summary>
        /// <param name="context">
        /// Application database context.
        /// </param>
        public DepartmentRepository(ApplicationDbContext context)
           : base(context)
        {
        }


        /// <summary>
        /// Gets the details of a department by its ID and include the doctors count in this department.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<DepartmentDetailsResponseDto?> GetDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(department => department.Id == id)
                .Select(department => new DepartmentDetailsResponseDto
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    DoctorsCount = department.Doctors.Count()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}

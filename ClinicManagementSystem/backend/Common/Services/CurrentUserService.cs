using ClinicManagementSystem.backend.Common.Exceptions.CustomExceptions;
using ClinicManagementSystem.backend.Common.Services.Interfaces;
using System.Security.Claims;

namespace ClinicManagementSystem.backend.Common.Services
{
    /// <summary>
    /// Provides information about the authenticated user.
    /// </summary>
    public class CurrentUserService:ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal User =>
            _httpContextAccessor.HttpContext?.User
            ?? throw new UnauthorizedException("User is not authenticated.");

        public bool IsAuthenticated =>
            User.Identity?.IsAuthenticated == true;

        public int UserId
        {
            get
            {
                string? value = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(value, out int id))
                {
                    throw new UnauthorizedException("User identifier was not found.");
                }

                return id;
            }
        }

        public string? UserName => User.FindFirstValue(ClaimTypes.Name);


        public string? Email => User.FindFirstValue(ClaimTypes.Email);


        
        public IReadOnlyList<string> Roles =>
            User.Claims
                .Where(claim => claim.Type == ClaimTypes.Role)
                .Select(claim => claim.Value)
                .ToList();


        public bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }

    }
}

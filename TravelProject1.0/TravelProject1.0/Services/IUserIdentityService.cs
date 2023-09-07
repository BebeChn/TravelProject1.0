using System.Security.Claims;

namespace TravelProject1._0.Services
{
    public interface IUserIdentityService
    {
        int GetUserId();
    }
    public class UserIdentityService : IUserIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            Claim user = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (user != null && int.TryParse(user.Value, out int id))
            {
                return id;
            }
            throw new InvalidOperationException("User ID not found or invalid.");
        }
    }
}

using System.Security.Claims;
using TravelProject1._0.Models;

namespace TravelProject1._0.Services
{
    public interface IUserSearchService
    {
        List<User> SearchUsers(string query);
    }
    public class UserSearchService : IUserSearchService
    {
   
        private readonly TravelProjectAzureContext _context;

        public UserSearchService(TravelProjectAzureContext context)
        {
            _context = context;
        }

        public List<User> SearchUsers(string query)
        {
            var searchResults = _context.Users.Where(u => u.Name.Contains(query)).ToList();
            return searchResults;
        }
    }
}

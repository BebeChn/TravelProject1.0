using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelProject1._0.Areas.Admin.Models.DTO;
using TravelProject1._0.Models;

namespace TravelProject1._0.Services
{
    public interface IUserSearchService
    {
        List<SearchUserDTO> SearchUsers(string query);
    }
    public class UserSearchService : IUserSearchService
    {
   
        private readonly TravelProjectAzureContext _context;

        public UserSearchService(TravelProjectAzureContext context)
        {
            _context = context;
        }

        public List<SearchUserDTO> SearchUsers(string query)
        {
            var searchResults = _context.Users
                .Where(u => u.UserId.ToString().Contains(query) ||
                            u.Name.Contains(query) ||
                            //u.Email.Contains(query) ||
                            u.Gender.Contains(query) ||
                            u.Birthday.Value.ToString().Contains(query)
                            )
                .Select(u => new SearchUserDTO
                {
                    Name = u.Name,
                    Gender = u.Gender,
                    Birthday = u.Birthday.Value.ToString("yyyy-MM-dd"),
                    Email = u.Email,
                    Phone = u.Phone,
                    UserId = u.UserId,
                }).ToList();
            
            return searchResults;
        }






    }
}

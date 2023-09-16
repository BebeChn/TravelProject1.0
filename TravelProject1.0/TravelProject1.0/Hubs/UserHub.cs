using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelProject1._0.Models;

namespace TravelProject1._0.Hubs
{
    public class UserHub: Hub
    {
        private readonly TravelProjectAzureContext _db;

        public UserHub(TravelProjectAzureContext context)
        {
            _db = context;
        }


        [Authorize]
        public async Task UserCartCount()
        {
            var userId = Convert.ToInt32(Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var count = await _db.Carts.AsNoTracking().CountAsync(c => c.UserId == userId);
            await Clients.Caller.SendAsync("ReviceCartCount", count);
        }

    }
}

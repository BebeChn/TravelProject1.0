using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models.ViewModel;
using TravelProject1._0.Models;
using TravelProject1._0.Services;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/Collect/[action]")]
    public class CollectApiController : Controller
    {
        private readonly TravelProjectAzureContext _context;
        private readonly IUserIdentityService _userIdentityService;
        public CollectApiController(TravelProjectAzureContext context, IUserIdentityService userIdentityService)
        {
            _context = context;
            _userIdentityService = userIdentityService;
        }

        [HttpGet]
        public async Task<List<CollectProductDto>> GetCollect()
        {
            int userId = _userIdentityService.GetUserId();
            return await _context.CollectTables.Where(c => c.UserId == userId).Select(c => new CollectProductDto
            {
                CollectId = c.CollectId,
                ProductId = c.ProductId,
                UserId = userId,
                CollectImage = c.CollectImage,
                CollectName = c.CollectName,
            }).ToListAsync();
        }

        [HttpPost]
        [Authorize]
        public async Task<bool> PostCollect([FromBody] PostCollectViewModel collect)
        {
            int userId = _userIdentityService.GetUserId();
            try
            {
                await _context.CollectTables.AddAsync(new CollectTable
                {
                    ProductId = collect.ProductId,
                    UserId = userId,
                    CollectImage = collect.CollectImage,
                    CollectName = collect.CollectName,
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<bool> DeleteCollect([FromBody] DeleteCollectViewModel model)
        {
            int userId = _userIdentityService.GetUserId();
            try
            {
                var uid = await _context.CollectTables.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == model.ProductId);
                if (uid == null) return false;

                _context.CollectTables.Remove(uid);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<int[]> GetCollectOfProductId()
        {
            return await _context.CollectTables.Where(x => x.UserId == _userIdentityService.GetUserId()).Select(x => x.ProductId).ToArrayAsync();
        }
    }
}

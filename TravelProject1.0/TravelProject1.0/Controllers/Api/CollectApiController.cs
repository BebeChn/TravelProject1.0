using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelProject1._0.Models.ViewModel;
using TravelProject1._0.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Collections.Concurrent;
using TravelProject1._0.Services;
using Microsoft.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/CollectApi/[Action]")]
    public class CollectApiController : Controller
    {

        private readonly TravelProjectAzureContext _context;
      
        private readonly IUserIdentityService _userIdentityService;
        public CollectApiController(TravelProjectAzureContext context,IUserIdentityService userIdentityService)
        {
            _context = context;
            _userIdentityService = userIdentityService;
        }
        [HttpGet]
        public async Task<ActionResult<List<CollectProductDTO>>> GetCollect()
        { 
            int userId = _userIdentityService.GetUserId();
            if (_context.CollectTables == null)
            {
                return NotFound();
            }
            var collects = await _context.CollectTables.Where(c => c.UserId == userId).Select(c => new CollectProductDTO
            {
                 CollectId = c.CollectId,
                 ProductId = c.ProductId,
                 UserId = userId,
                 CollectImage = c.CollectImage,
                 CollectName = c.CollectName,
            }).ToListAsync();
            if (collects == null || collects.Count == 0)
            {
                return NotFound();
            }
            return collects;
            }
            [Authorize]
            [HttpPost]
            public bool PostCollect([FromBody] PostCollectViewModel collect)
            {
            int userId = _userIdentityService.GetUserId();
            //var user = _context.CollectTables.FirstOrDefaultAsync(c => c.ProductId == collect.ProductId);
            //if (user != null)
            //{

            //    return BadRequest("已加入收藏夾");
            //}
            try
                {

                    // 創建收藏夾實體
                    CollectTable newCollect = new CollectTable
                    {
                        ProductId = collect.ProductId,
                        UserId = userId,
                        CollectImage = collect.CollectImage,
                        CollectName = collect.CollectName,
                    };

                    // 添加收藏到資料庫

                    _context.CollectTables.Add(newCollect);

                    _context.SaveChanges();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                return true;
            }
            [Authorize]
            [HttpDelete]
            public async Task<IActionResult> DeleteCollect([FromBody] DeleteCollectViewModel model)
            {
            int userId = _userIdentityService.GetUserId();
            var uid = await _context.CollectTables.FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == model.ProductId);
                //HttpContext.Response.Cookies.Append("userID", userId.ToString());
                if (uid == null)
                {
                    return NotFound(new { Message = "無法將商品從收藏移除" });
                }
                try
                {
                    _context.CollectTables.Remove(uid);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
                return Ok(new { Message = "商品已從收藏夾中移除" });
            }
        [HttpGet]
        [Authorize]
        public async Task<int[]> GetCollectOfProductId()
        {
            return await _context.CollectTables.Where(x => x.UserId == _userIdentityService.GetUserId()).Select(x=> x.ProductId).ToArrayAsync();
        }

    }
}

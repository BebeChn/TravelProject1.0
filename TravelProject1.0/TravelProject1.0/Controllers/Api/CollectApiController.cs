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

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/CollectApi/[Action]")]
    public class CollectApiController : Controller
    {

        private readonly TravelProjectAzureContext _context;
        private readonly IUserIdentityService _userIdentityService;

        public CollectApiController(TravelProjectAzureContext context, IUserIdentityService userIdentityService)

        {
            _context = context;
            _userIdentityService = userIdentityService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostCollect([FromBody]PostCollectViewModel collect)
        {
            //int userId = _userIdentityService.GetUserId();
            //HttpContext.Response.Cookies.Append("userID", userId.ToString());
            try
            {

                // 創建收藏夾實體
                CollectTable newCollect = new CollectTable
                {
                    ProductId = collect.ProductId,
                    UserId = collect.UserId,
                };

                // 添加收藏到資料庫

                _context.CollectTables.Add(newCollect);

                _context.SaveChanges();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(new { ErrorMessage = "收藏失败" });
            }
            return Ok();
        }
        [Authorize]
        [HttpDelete]
        public async Task<string> DeleteCollect(int id)
        {
            if (_context.CollectTables == null)
            {
                return "刪除失敗";
            }
            var collector = await _context.CollectTables.FindAsync(id);
            if (collector == null)
            {
                return "刪除失敗";
            }
            try
            {
                _context.CollectTables.Remove(collector);
                await _context.SaveChangesAsync();
            }
            catch
            {
                return "刪除關聯失敗";
            }
            return "刪除成功";
        }

    }
}

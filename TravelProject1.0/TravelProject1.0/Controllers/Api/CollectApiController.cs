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

namespace TravelProject1._0.Controllers.Api
{
    public class CollectApiController : Controller
    {

        private readonly TravelProjectAzureContext _context;
        private readonly IUserIdentityService _userIdentityService;

        public CollectApiController( TravelProjectAzureContext context,IUserIdentityService userIdentityService)

        {         
            _context = context;
            _userIdentityService = userIdentityService;
        }

        [HttpPost]
        public async Task<bool> PostCollect(PostCollectViewModel collect)
        {
            try
            {
                int userId = _userIdentityService.GetUserId();
                // 創建收藏夾實體
                CollectTable newCollect = new CollectTable
                {
                    ProductId=collect.ProductId,
                    UserId = userId,
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
        [HttpDelete]
        public async Task<bool> DeleteCollect(PostCollectViewModel collect)
        {
            try
            {
                int userId = _userIdentityService.GetUserId();
                // 創建收藏夾實體
                CollectTable newCollect = new CollectTable
                {
                    ProductId = collect.ProductId,
                    UserId = userId,
                };

                // 添加收藏到資料庫

                _context.CollectTables.Remove(newCollect);

                _context.SaveChanges();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}

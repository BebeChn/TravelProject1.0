using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models.ViewModel;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfirmTheOrderApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _dbContext;
        public ConfirmTheOrderApiController(TravelProjectAzureContext dbContext)
        {
            _dbContext = dbContext;
        }

        //取得聯絡人資訊
        [HttpGet]
        public async Task<IQueryable<ConfirmTheOrderDTO>> GetUser()
        {
            Claim user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            string? idu = user.Value;
            int id = Convert.ToInt32(idu);

            return _dbContext.Users.Where(u => u.UserId == id).Select(u => new ConfirmTheOrderDTO
            {
                UserId = id,
                Name = u.Name,
                Phone = u.Phone,
                Email = u.Email,
                Points = u.Points
            });
        }

        //取得訂單資訊
        [HttpGet]
        [Route("{orderId}")]
        public async Task<IQueryable<ConfirmTheOrderViewModel>> GetOrders(int orderId)
        {
            Claim user = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            string? idu = user.Value;
            int id = Convert.ToInt32(idu);

            return _dbContext.Orders.Include(o => o.OrderDetails).Where(o => o.UserId == id && o.OrderId == orderId).Select(o => new ConfirmTheOrderViewModel
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                ConfirmTheOrderDetails = o.OrderDetails.Select(od => new ConfirmTheOrderDetailDTO
                {
                    Odname = od.Odname,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                })
            });
        }
    }
}

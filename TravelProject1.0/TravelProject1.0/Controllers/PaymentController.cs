using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers
{
    public class PaymentController : Controller
    {
        private readonly TravelProjectAzureContext _db;
        private readonly IUserIdentityService _userIdentityService;

        public PaymentController(TravelProjectAzureContext db, IUserIdentityService userIdentity)
        {
            _db = db;
            _userIdentityService = userIdentity;
        }

        public IActionResult Index(PaymentDTO payment)
        {
            int userId = _userIdentityService.GetUserId();

            //新增到資料庫
            var data = new Order()
            {
                OrderDate = DateTime.Now,
                UserId = userId,
                OrderDetails = payment.detailDTOs.Select(x => new OrderDetail
                {
                    Odname = x.PlanName,
                    PlanId = x.PlanId,
                    Quantity = (short?)x.Quantity,
                    UnitPrice = x.UnitPrice,
                }).ToList(),
            };
            var user = _db.Users.Find(userId);
            if (user == null) throw new Exception();
            user.Points -= payment.Points;
            _db.Orders.Add(data);
            _db.SaveChanges();

            return View();
        }
    }
}

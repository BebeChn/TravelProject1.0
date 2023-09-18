using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Specialized;
using System.Net.Mime;
using System.Text;
using System.Web;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models.Payment;
using TravelProject1._0.Services;

namespace TravelProject1._0.Controllers
{
    public class PaymentController : Controller
    {
        private readonly TravelProjectAzureContext _db;
        private readonly IUserIdentityService _userIdentityService;
        private readonly IConfiguration _configuration;
        public PaymentController(TravelProjectAzureContext db, IUserIdentityService userIdentity, IConfiguration configuration)
        {
            _db = db;
            _userIdentityService = userIdentity;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Pay([FromBody] PaymentDto payment)
        {
            int userId = _userIdentityService.GetUserId();

            var subTotal = (int?)payment.detailDTOs.Sum(x => x.UnitPrice);
            var discount = payment.Points / 300;
            var total = subTotal - discount;

            //新增到資料庫
            var data = new Order()
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalPrice = total,
                NewPoint = total / 300,
                OrderDetails = payment.detailDTOs.Select(x => new OrderDetail
                {
                    PlanId = x.PlanId,
                    Odname = x.PlanName,
                    Quantity = (short?)x.Quantity,
                    UnitPrice = x.UnitPrice,
                    UseDate = DateTime.Now.AddDays(7),
                }).ToList(),
            };

            var user = _db.Users.Find(userId);
            if (user == null) throw new Exception();
            user.Points -= payment.Points;
            _db.Orders.Add(data);
            _db.SaveChanges();

            var oId = _db.Orders.Where(x => x.UserId == userId).Max(x => x.OrderId);

            var itemName = _db.OrderDetails.Include(x => x.Order).Where(x => x.Order.UserId == userId && x.OrderId == oId).Select(x => x.Odname);

            string productName = string.Join(Environment.NewLine, itemName);

            //金流
            string version = "2.0";

            // 目前時間轉換 +08:00, 防止傳入時間或Server時間時區不同造成錯誤
            DateTimeOffset taipeiStandardTimeOffset = DateTimeOffset.Now.ToOffset(new TimeSpan(8, 0, 0));

            TradeInfo tradeInfo = new TradeInfo()
            {
                // * 商店代號
                MerchantID = _configuration.GetValue<string>("Payment:MerchantID"),
                // * 回傳格式
                RespondType = "String",
                // * TimeStamp
                TimeStamp = taipeiStandardTimeOffset.ToUnixTimeSeconds().ToString(),
                // * 串接程式版本
                Version = version,
                // * 商店訂單編號
                //MerchantOrderNo = $"T{DateTime.Now.ToString("yyyyMMddHHmm")}",
                MerchantOrderNo = _db.Orders.Where(x => x.UserId == userId).Max(x => x.OrderId).ToString(),
                // * 訂單金額
                Amt = total.Value,
                // * 商品資訊
                ItemDesc = productName,
                // 繳費有效期限(適用於非即時交易)
                ExpireDate = null,
                // 支付完成 返回商店網址
                ReturnURL = _configuration.GetValue<string>("Payment:ReturnURL"),
                // 支付通知網址
                NotifyURL = _configuration.GetValue<string>("Payment:NotifyURL"),
                // 商店取號網址
                CustomerURL = _configuration.GetValue<string>("Payment:CustomerURL"),
                // 支付取消 返回商店網址
                ClientBackURL = null,
                // * 付款人電子信箱
                Email = user.Email,
                // 付款人電子信箱 是否開放修改(1=可修改 0=不可修改)
                EmailModify = 0,
                // 商店備註
                OrderComment = null,
                // 信用卡 一次付清啟用(1=啟用、0或者未有此參數=不啟用)
                CREDIT = 1,
                // WEBATM啟用(1=啟用、0或者未有此參數，即代表不開啟)
                WEBATM = 0,
                // ATM 轉帳啟用(1=啟用、0或者未有此參數，即代表不開啟)
                VACC = 0,
                // 超商代碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 30 元或超過 2 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                CVS = 0,
                // 超商條碼繳費啟用(1=啟用、0或者未有此參數，即代表不開啟)(當該筆訂單金額小於 20 元或超過 4 萬元時，即使此參數設定為啟用，MPG 付款頁面仍不會顯示此支付方式選項。)
                BARCODE = 0
            };

            var inputModel = new SpgatewayInputModel
            {
                MerchantID = _configuration.GetValue<string>("Payment:MerchantID"),
                Version = version
            };

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> tradeData = LambdaUtil.ModelToKeyValuePairList<TradeInfo>(tradeInfo);
            // 將List<KeyValuePair<string, string>> 轉換為 key1=Value1&key2=Value2&key3=Value3...
            var tradeQueryPara = string.Join("&", tradeData.Select(x => $"{x.Key}={x.Value}"));
            // AES 加密
            inputModel.TradeInfo = CryptoUtil.EncryptAESHex(tradeQueryPara, _configuration.GetValue<string>("Payment:HashKey"), _configuration.GetValue<string>("Payment:HashIV"));
            // SHA256 加密
            inputModel.TradeSha = CryptoUtil.EncryptSHA256($"HashKey={_configuration.GetValue<string>("Payment:HashKey")}&{inputModel.TradeInfo}&HashIV={_configuration.GetValue<string>("Payment:HashIV")}");

            // 將model 轉換為List<KeyValuePair<string, string>>, null值不轉
            List<KeyValuePair<string, string>> postData = LambdaUtil.ModelToKeyValuePairList<SpgatewayInputModel>(inputModel);

            StringBuilder s = new StringBuilder();
            s.AppendFormat("<form name='form' action='{0}' method='post'>", _configuration.GetValue<string>("Payment:AuthUrl"));
            foreach (KeyValuePair<string, string> item in postData)
            {
                s.AppendFormat("<input type='hidden' name='{0}' value='{1}' />", item.Key, item.Value);
            }

            s.Append("</form>");
            return Content(s.ToString(), MediaTypeNames.Text.Html);
        }

        [HttpPost]
        public IActionResult PayReturn([FromForm] PayreturnDTO dto)
        {
            if (dto.Status == "SUCCESS")
            {
                var decryptTradeInfo = CryptoUtil.DecryptAESHex(dto.TradeInfo, _configuration.GetValue<string>("Payment:HashKey"),
                    _configuration.GetValue<string>("Payment:HashIV"));

                // 取得回傳參數(ex:key1=value1&key2=value2),儲存為NameValueCollection
                NameValueCollection decryptTradeCollection = HttpUtility.ParseQueryString(decryptTradeInfo);
                SpgatewayOutputDataModel convertModel = LambdaUtil.DictionaryToObject<SpgatewayOutputDataModel>(decryptTradeCollection.AllKeys.ToDictionary(k => k, k => decryptTradeCollection[k]));

                var no = Convert.ToInt32(convertModel.MerchantOrderNo);
                var od = _db.Orders.FirstOrDefault(x => x.OrderId == no);
                var user = _db.Users.FirstOrDefault(u => u.UserId == od.UserId);
                var orderDetail = _db.OrderDetails.FirstOrDefault(o => o.OrderId == no);
                var cartItem = _db.Carts.FirstOrDefault(c => c.PlanId == orderDetail.PlanId && c.CartName == orderDetail.Odname && c.CartPrice == orderDetail.UnitPrice && c.CartQuantity == orderDetail.Quantity);

                bool isCartItem = cartItem != null;

                if (od == null) return View("Fail");

                user.Points += od.NewPoint;
                od.Status = "success";

                if (isCartItem)
                {
                    _db.Carts.Remove(cartItem);
                }

                _db.SaveChanges();

                ViewBag.Info = new
                {
                    merchantID = convertModel.MerchantID,
                    merchantOrderNo = convertModel.MerchantOrderNo,
                    tradeNo = convertModel.TradeNo,
                    amt = convertModel.Amt,
                    status = convertModel.Status,
                    payTime = convertModel.PayTime
                };

                return View("Success");
            }
            else
            {
                return View("Fail");
            }
        }
    }
}
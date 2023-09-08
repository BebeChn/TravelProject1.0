using AspNetCoreHero.ToastNotification.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NToastNotify.Helpers;
using NuGet.Packaging;
using System.Collections.Immutable;
using System.Reflection.Metadata.Ecma335;
using TravelProject1._0.Models;
using TravelProject1._0.Areas.Admin.Models.ChartViewModel.HotelChartDTO;
namespace TravelProject1._0.Areas.Admin.Controllers.Api.AnalyzeAPI
{
	[Area("Admin")]
	[Route("api/SaleChartApi/{action}")]
	[ApiController]
	public class SaleChartApiController : ControllerBase
	{
		private TravelProjectAzureContext _db;

		public SaleChartApiController(TravelProjectAzureContext db)
		{
			_db = db;
		}

		[HttpGet]
		public async Task<IEnumerable<object>> GetYearSales()
		{




			return null;
		}

		//[HttpGet]
		//public async Task<object> GetHotelSales()
		//{
		//	string date = DateTime.Now.AddMonths(-12).ToString().Substring(0, 7).Replace("/", "-");
		//	var hotelProductIdList = await _db.Products.Where(p => p.Id == 2).Select(p => p.ProductId).ToListAsync();
		//	var sales = new List<object>();
		//	Dictionary<string, decimal> monthlySales = new Dictionary<string, decimal>();

		//	foreach (var hotelId in hotelProductIdList)
		//	{
		//		var planId = await _db.Plans.Where(p => p.ProductId == hotelId).Select(p => p.PlanId).ToListAsync();
		//		foreach (var plan in planId)
		//		{
		//			var orderDetailId = await _db.OrderDetails.Where(o => o.PlanId == plan).Select(o => o.Id).ToListAsync();

		//			sales.Add(orderDetailId);
		//			sales.AddRange(orderDetailId);
		//		}
		//	}


		//	var sales = _db.OrderDetails.Where(o => o.Plan == plan);

		//	return sales;
		//}

		[HttpGet]
		public async Task<IEnumerable<GetSalesDTO>> GetSales(int categoryId)
		{
			DateTime thirteenMonths = DateTime.Now.AddMonths(-13);
			DateTime nowDateTime = DateTime.Now;
			var monthSales = new Dictionary<string, decimal>();
			var hotelIds = await _db.Products.AsNoTracking().Where(p => p.Id == categoryId).Select(p => p.ProductId).ToListAsync();
			foreach (var hotelId in hotelIds)
			{
				var planIds = await _db.Plans.AsNoTracking().Where(p => p.ProductId == hotelId).Select(p => p.PlanId).ToListAsync();

				foreach (var planId in planIds)
				{
					var orderDetails = await _db.OrderDetails.AsNoTracking()
						.Where(o => o.PlanId == planId && o.Order.OrderDate >= thirteenMonths &&  o.Order.OrderDate <= nowDateTime)
						.Select(o => new { 
							o.Order.OrderDate, 
							o.UnitPrice ,
							o.Quantity
						}).ToListAsync();

					foreach (var orderDetail in orderDetails)
					{
						string monthKey = orderDetail.OrderDate.ToString().Substring(0,6);

						if (!monthSales.ContainsKey(monthKey))
						{
							monthSales[monthKey] = Convert.ToDecimal(orderDetail.UnitPrice * orderDetail.Quantity);
						}
						else
						{
							monthSales[monthKey] += Convert.ToDecimal(orderDetail.UnitPrice * orderDetail.Quantity);
						}
					}
				}
			}

			var sortedMonthlySales = monthSales.OrderBy(kv => kv.Key).ToList();

			var result = sortedMonthlySales.Select(kv => new GetSalesDTO
			{
				Name = kv.Key,
				y = kv.Value
			}).ToList();

			return result;
		}

		[HttpGet]
		public async Task<IEnumerable<GetSalesDTO>> GetAirplaneSales()
		{
			return await GetSales(1);
		}

		[HttpGet]
		public async Task<IEnumerable<GetSalesDTO>> GetHotelSales()
		{
			return await GetSales(2);
		}

		[HttpGet]
		public async Task<IEnumerable<GetSalesDTO>> GetTransportationSales()
		{
			return await GetSales(3);
		}

		[HttpGet]
		public async Task<IEnumerable<GetSalesDTO>> GetAttractionsSales()
		{
			return await GetSales(4);
		}
	}
}

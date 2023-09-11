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
using TravelProject1._0.Areas.Admin.Models.ChartViewModel.SaleChartDTO;
using TravelProject1._0.Models.DTO;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Linq;

namespace TravelProject1._0.Areas.Admin.Controllers.Api.AnalyzeAPI
{
	[Area("Admin")]
	[Route("api/SaleChart/{action}")]
	[ApiController]
	public class SaleChartController : ControllerBase
	{
		private TravelProjectAzureContext _db;

		public SaleChartController(TravelProjectAzureContext db)
		{
			_db = db;
		}

		[NonAction]
		public Dictionary<string, decimal> AddThirteenYear()
		{
			var result = new Dictionary<string, decimal>();
			DateTime endDateTime = DateTime.Now.AddMonths(1);
			DateTime thirteenMonth = DateTime.Now.AddMonths(-13);
			for (var startMonth = thirteenMonth; startMonth < endDateTime; startMonth = startMonth.AddMonths(1))
			{
				result[startMonth.ToString("yyyy-MM")] = 0;
			}
			return result;
			}

		[NonAction]
		public Dictionary<string, decimal> GetTicketsSale(Dictionary<string, decimal> dictionary, int categotyId)
		{
			DateTime dateTimeNow = DateTime.Now;
			DateTime thirteenMonth = DateTime.Now.AddMonths(-13);
			//var orderDetails = _db.Products.AsNoTracking().Where(p => p.Id == categotyId && p.ProductId == p.Plans.)
			var producrIds = _db.Products.AsNoTracking().Where(p => p.Id == categotyId).Select(p => p.ProductId).ToList();
			foreach (var productid in producrIds)
			{
				var planIds = _db.Plans.Where(p => p.ProductId == productid).Select(p => p.PlanId).ToList();
				foreach (var planId in planIds)
				{
					var orderDetails = _db.OrderDetails.Where(o => o.PlanId == planId &&
					o.Order.OrderDate > thirteenMonth && o.Order.OrderDate <= dateTimeNow)
						.Select(o => new
						{
							Date = o.Order.OrderDate,
							Price = o.UnitPrice,
							Quantity = o.Quantity
						}).ToList();
					foreach (var orderDetail in orderDetails)
					{
						if (!dictionary.ContainsKey(orderDetail.Date.Value.ToString("yyyy-MM")))
						{
							dictionary[orderDetail.Date.Value.ToString("yyyy-MM")] = Convert.ToDecimal((orderDetail.Price * orderDetail.Quantity) / 10000);
						}
						else
						{
							dictionary[orderDetail.Date.Value.ToString("yyyy-MM")] += Convert.ToDecimal((orderDetail.Price * orderDetail.Quantity) / 10000);
						}
					}
				}
			}
			return dictionary;
		}

		//[NonAction]
		//public async Task<AllTicktesSaleDTO> GetAllTicktesThirteenMonthSale2()
		//{
		//	var airPlane = AddThirteenYear();
		//	var hotel = AddThirteenYear();
		//	var transportation = AddThirteenYear();
		//	var attractions = AddThirteenYear();

		//	var airOneYearSale = GetTicketsSale(airPlane, 1);
		//	var hotelOneYearSale = GetTicketsSale(hotel, 2);
		//	var transportationOneYearSale = GetTicketsSale(transportation, 3);
		//	var attractionsOneYearSale = GetTicketsSale(attractions, 4);

		//	decimal oneYearSale = 0;
		//	var orderDetails = await _db.OrderDetails.Select(o => new
		//	{
		//		o.UnitPrice,
		//		o.Quantity
		//	}).ToListAsync();
		//	foreach (var orderDetail in orderDetails)
		//	{
		//		oneYearSale += Convert.ToDecimal((orderDetail.UnitPrice * orderDetail.Quantity) / 10000);
		//	}

		//	AllTicktesSaleDTO atsDTO = new AllTicktesSaleDTO()
		//	{
		//		OneyearSale = oneYearSale,
		//		Airplane = airOneYearSale,
		//		Hotel = hotelOneYearSale,
		//		Transportation = transportationOneYearSale,
		//		Attractions = attractionsOneYearSale,
		//	};

		//	return atsDTO;
		//}

		[HttpGet]
		public async Task<AllTicktesSaleDTO> GetCurrentYearAllTicktesSales()
		{
			decimal oneYearSale = await _db.OrderDetails.SumAsync(o => (decimal)(o.UnitPrice * o.Quantity) / 10000);

			var orderDetailsList = _db.OrderDetails.AsNoTracking().Include(p => p.Plan).ThenInclude(p => p.Product)
				.Where(x=> EF.Functions.DateDiffYear(x.Order.OrderDate,DateTime.Now) <= 0 && EF.Functions.DateDiffYear(x.Order.OrderDate, DateTime.Now)>=0)
				.GroupBy(x => x.Plan.Product.Id)
				.Select(x => new { 
					Category = x.Key, 
					Details = x.Select(y => new 
					{ 
					OrderDate = y.Order.OrderDate.Value.ToString("yyyy-MM"),
					y.UnitPrice,
					y.Quantity 
				})}).ToList();

			var orderDetailsDicts = orderDetailsList.ToDictionary(x => x.Category, x => {
				int currentYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
				var dic = new Dictionary<string, decimal>();
                for (var i = 1; i < 13 ; i++)
                {
					var currentMonth = new DateTime(currentYear, i, 1).ToString("yyyy-MM"); 
					dic.Add(currentMonth, x.Details.Where(x=>x.OrderDate == currentMonth).Sum(y => Convert.ToDecimal(y.UnitPrice * y.Quantity) / 10000));
                }
				return dic;
            });

			var result = new AllTicktesSaleDTO()
			{
				OneyearSale = oneYearSale,
				Airplane = orderDetailsDicts[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Hotel = orderDetailsDicts[2].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Transportation = orderDetailsDicts[3].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Attractions = orderDetailsDicts[4].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				AirplaneOneYearSale = orderDetailsDicts[1],
				HotelOneYearSale = orderDetailsDicts[2],
				TransportationOneYearSale = orderDetailsDicts[3],
				AttractionsOneYearSale = orderDetailsDicts[4]
			};

			return result;
		}


		//[HttpGet]
		//public async Task<AllTicktesSaleDTO> GetTicktes()
		//{
		//	decimal oneYearSale = await _db.OrderDetails.SumAsync(o => (decimal)(o.UnitPrice * o.Quantity) / 10000);

		//	var result = await _db.OrderDetails.AsNoTracking().Include(od => od.Plan).ThenInclude(od => od.Product)
		//		.Where(od => EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) < 13 && EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) >= 0)
		//		.GroupBy(od => od.Plan.Product.Id)
		//		.Select(g => new
		//		{
		//			CategoryId = g.Key,
		//			Details = g.Select(g => new
		//			{
		//				OrderDate = g.Order.OrderDate.Value.ToString("yyyy-MM"),
		//				g.UnitPrice,
		//				g.Quantity
		//			})
		//		})
		//		.ToDictionaryAsync(d => d.CategoryId, d =>
		//		{
		//			var date = DateTime.Now;
		//			var dic = new Dictionary<string,decimal>();
		//			for (int i = 13; i >= 0; --i)
		//			{
		//				var currenMonth = date.AddMonths(-i).ToString("yyyy-MM");
		//				dic.Add(currenMonth, d.Details.Where(d => d.OrderDate == currenMonth).Sum(y => Convert.ToDecimal(y.UnitPrice * y.Quantity) / 10000));
		//			}
		//			return dic;
		//		});
		//	AllTicktesSaleDTO atsDTO = new AllTicktesSaleDTO()
		//	{
		//		OneyearSale = oneYearSale,
		//		Airplane = result[1],
		//		Hotel = result[2],
		//		Transportation = result[3],
		//		Attractions = result[4],
		//	};
		//	var qqq = result[1].Select(r => new HighChart3DGraphDTO
		//	{
		//		Name = r.Key,
		//		y = r.Value
		//	}).ToList();

		//	return atsDTO;
		//}

		//[HttpGet]
		//public async Task<IEnumerable<HighChart3DGraphDTO>> GetAirplaneSales()
		//{
		//	return await GetSales(1);
		//}

		//[HttpGet]
		//public async Task<IEnumerable<HighChart3DGraphDTO>> GetHotelSales()
		//{
		//	return await GetSales(2);
		//}

		//[HttpGet]
		//public async Task<IEnumerable<HighChart3DGraphDTO>> GetTransportationSales()
		//{
		//	return await GetSales(3);
		//}

		//[HttpGet]
		//public async Task<IEnumerable<HighChart3DGraphDTO>> GetAttractionsSales()
		//{
		//	return await GetSales(4);
		//}

		//public async Task<IEnumerable<HighChart3DGraphDTO>> GetSales(int categoryId)
		//{
		//	DateTime thirteenMonths = DateTime.Now.AddMonths(-13);
		//	DateTime nowDateTime = DateTime.Now;
		//	var monthSales = new Dictionary<string, decimal>();
		//	var hotelIds = await _db.Products.AsNoTracking().Where(p => p.Id == categoryId).Select(p => p.ProductId).ToListAsync();
		//	foreach (var hotelId in hotelIds)
		//	{
		//		var planIds = await _db.Plans.AsNoTracking().Where(p => p.ProductId == hotelId).Select(p => p.PlanId).ToListAsync();

		//		foreach (var planId in planIds)
		//		{
		//			var orderDetails = await _db.OrderDetails.AsNoTracking()
		//				.Where(o => o.PlanId == planId && o.Order.OrderDate >= thirteenMonths && o.Order.OrderDate <= nowDateTime)
		//				.Select(o => new
		//				{
		//					o.Order.OrderDate,
		//					o.UnitPrice,
		//					o.Quantity
		//				}).ToListAsync();

		//			foreach (var orderDetail in orderDetails)
		//			{
		//				string monthKey = orderDetail.OrderDate.Value.ToString("yyyy-MM");

		//				if (!monthSales.ContainsKey(monthKey))
		//				{
		//					monthSales[monthKey] = Convert.ToDecimal(orderDetail.UnitPrice * orderDetail.Quantity);
		//				}
		//				else
		//				{
		//					monthSales[monthKey] += Convert.ToDecimal(orderDetail.UnitPrice * orderDetail.Quantity);
		//				}
		//			}
		//		}
		//	}

		//	var result = monthSales.OrderBy(kv => kv.Key).Select(kv => new HighChart3DGraphDTO
		//	{
		//		Name = kv.Key,
		//		y = kv.Value
		//	}).ToList();

		//	return result;
		//}
	}
}

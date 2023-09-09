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

		//最開始的程式碼，後來想到或許還有其他方式可以執行
		//public List<string> GetThirteenMonth()
		//{
		//	var oneYear = new List<string>();
		//	DateTime dateTimeNow = DateTime.Now;
		//	DateTime thirteenMonth = DateTime.Now.AddMonths(-13);
		//	for (var startMonth = thirteenMonth; startMonth < dateTimeNow; startMonth = startMonth.AddMonths(1))
		//	{
		//		oneYear.Add(startMonth.ToString("Y"));
		//	}
		//	return oneYear;
		//}

		public Dictionary<string, decimal> AddThirteenYear()
		{
			var result = new Dictionary<string, decimal>();
			DateTime dateTimeNow = DateTime.Now;
			DateTime thirteenMonth = DateTime.Now.AddMonths(-13);
			for (var startMonth = thirteenMonth; startMonth < dateTimeNow; startMonth = startMonth.AddMonths(1))
			{
				result[startMonth.ToString("Y")] = 0;
			}
			return result;
		}

		public Dictionary<string,decimal> GetTicketsSale(Dictionary<string,decimal> dictionary,int categotyId)
		{
			var producrIds =  _db.Products.AsNoTracking().Where(p => p.Id == categotyId).Select(p => p.ProductId).ToList();
			foreach (var productid in producrIds)
			{ 
				var planIds =  _db.Plans.Where(p => p.ProductId == productid).Select(p => p.PlanId).ToList();
				foreach (var planId in planIds)
				{
					var orderDetails =  _db.OrderDetails.Where(o => o.PlanId == planId).Select(o => new { 
						Date = o.Order.OrderDate,
						Price = o.UnitPrice,
						Quantity = o.Quantity
					}).ToList();
					foreach (var orderDetail in orderDetails)
					{
						//DateTime oderDate = orderDetail.Date;
						//string nowdate = date.ToString("");
						if (!dictionary.ContainsKey(orderDetail.Date.Value.ToString("Y")))
						{
							dictionary[orderDetail.Date.Value.ToString("Y")] = Convert.ToDecimal(orderDetail.Price * orderDetail.Quantity);
						}
						else
						{
							dictionary[orderDetail.Date.Value.ToString("Y")] += Convert.ToDecimal(orderDetail.Price * orderDetail.Quantity);
						}
					}
				}
 			}

			return dictionary;
		}

		[HttpGet]
		public async Task<AllTicktesSaleDTO> GetAllTicktesThirteenMonthSale()
		{
			var airPlane = AddThirteenYear();
			var hotel = AddThirteenYear();
			var transportation = AddThirteenYear();
			var attractions = AddThirteenYear();

			var airOneYearSale = GetTicketsSale(airPlane,1);
			var hotelOneYearSale = GetTicketsSale(hotel,2);
			var transportationOneYearSale = GetTicketsSale(transportation,3);
			var attractionsOneYearSale = GetTicketsSale(attractions,4);

			AllTicktesSaleDTO atsDTO = new AllTicktesSaleDTO()
			{
				Airplane = airOneYearSale,
				Hotel = hotelOneYearSale,
				Transportation = transportationOneYearSale,
				Attractions = attractionsOneYearSale,
			};

			return atsDTO;
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

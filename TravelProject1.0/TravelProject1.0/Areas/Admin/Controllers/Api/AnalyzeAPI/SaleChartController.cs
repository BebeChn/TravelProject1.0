﻿using AspNetCoreHero.ToastNotification.Helpers;
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
		//decimal lastYearSale = await _db.OrderDetails.Where(od => EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) <= 0 &&
		//	EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) >= 0).SumAsync(o => (decimal)(o.UnitPrice * o.Quantity) / 10000);

		[HttpGet]
		public async Task<AllTicktesSaleDTO> GetCurrentYearAllTicktesSales()
		{
			decimal currentYearSale = await _db.OrderDetails
				.Where(od => EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) >= 0).
								SumAsync(od => (decimal)(od.UnitPrice * od.Quantity) / 10000);

			var resultList = await _db.OrderDetails.AsNoTracking().Include(od => od.Plan).ThenInclude(od => od.Product)
				.Where(od => EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) <= 0 &&
							EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) >= 0)
				.GroupBy(od => od.Plan.Product.Id)
				.Select(od => new
				{
					CategoryId = od.Key,
					Details = od.Select(od => new
					{
						OrderDate = od.Order.OrderDate.Value.ToString("yyyy-MM"),
						od.UnitPrice,
						od.Quantity
					})
				})
				.ToDictionaryAsync(od => od.CategoryId, od =>
				//				od.Details.ToDictionary(
				//	od => od.OrderDate,
				//	od => Convert.ToDecimal(od.UnitPrice * od.Quantity))
				//.DefaultIfEmpty()
				//.ToDictionary(
				//	od => od.Key,
				//	od => od.Value
				//	));
			{
				int currentYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
				var dic = new Dictionary<string, decimal>();
				for (var i = 1; i < 13; i++)
				{
					var currentYearMonth = new DateTime(currentYear, i, 1).ToString("yyyy-MM");
					dic.Add(currentYearMonth, od.Details.Where(od => od.OrderDate == currentYearMonth).Sum(od => Convert.ToDecimal(od.UnitPrice * od.Quantity) / 10000));
				}
				return dic;
			});

			var result = new AllTicktesSaleDTO()
			{
				OneyearSale = currentYearSale,
				Airplane = resultList[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Hotel = resultList[2].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Transportation = resultList[3].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Attractions = resultList[4].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				AirplaneOneYearSale = resultList[1],
				HotelOneYearSale = resultList[2],
				TransportationOneYearSale = resultList[3],
				AttractionsOneYearSale = resultList[4]
			};

			return result;
		}

		[HttpGet]
		public async Task<AllTicktesSaleDTO> GetLastYearAllTicktesSales()
		{
			decimal lastYearSale = await _db.OrderDetails
				.Where(od => EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) <= 1 &&
								EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) >= 1).
								SumAsync(od => (decimal)(od.UnitPrice * od.Quantity) / 10000);

			var resultList = _db.OrderDetails.AsNoTracking().Include(od => od.Plan).ThenInclude(od => od.Product)
				.Where(od => EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) <= 1 &&
							EF.Functions.DateDiffYear(od.Order.OrderDate, DateTime.Now) >= 1)
				.GroupBy(od => od.Plan.Product.Id)
				.Select(od => new
				{
					CategoryId = od.Key,
					Details = od.Select(od => new
					{
						OrderDate = od.Order.OrderDate.Value.ToString("yyyy-MM"),
						od.UnitPrice,
						od.Quantity
					})
				})
				.ToDictionary(od => od.CategoryId, od =>
				{
					int lastYear = Convert.ToInt32(DateTime.Now.AddYears(-1).ToString("yyyy"));
					var dic = new Dictionary<string, decimal>();
					for (var i = 1; i < 13; i++)
					{
						var lastYearMonth = new DateTime(lastYear, i, 1).ToString("yyyy-MM");
						dic.Add(lastYearMonth, od.Details.Where(od => od.OrderDate == lastYearMonth).Sum(od => Convert.ToDecimal(od.UnitPrice * od.Quantity) / 10000));
					}
					return dic;
				});

			var result = new AllTicktesSaleDTO()
			{
				OneyearSale = lastYearSale,
				Airplane = resultList[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Hotel = resultList[2].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Transportation = resultList[3].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Attractions = resultList[4].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				AirplaneOneYearSale = resultList[1],
				HotelOneYearSale = resultList[2],
				TransportationOneYearSale = resultList[3],
				AttractionsOneYearSale = resultList[4]
			};

			return result;
		}
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
		//{
		//	{
		//		Name = kv.Key,
		//		y = kv.Value
		//	}).ToList();

		//	return result;
		//}
		public async Task<object> GetCurrentMonthSale()
		{
			var aaa = _db.OrderDetails.AsNoTracking().Include(od => od.Plan).ThenInclude(od => od.Product)
				.Where(od => EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) >= 0)
				.GroupBy(od => od.Plan.Product.Id)
				.Select(od => new
				{
					CategoryId = od.Key,
					Details = od.Select(od => new
					{
						OrderDate = od.Order.OrderDate.Value.ToString("yyyy-MM"),
						od.UnitPrice,
						od.Quantity
					})
				})
				.ToDictionaryAsync(od => od.CategoryId, od =>
				{
					int lastYear = Convert.ToInt32(DateTime.Now.ToString("yyyy-MM"));
					var dic = new Dictionary<string, decimal>();
					for (var i = 1; i < 13; i++)
					{
						var lastYearMonth = new DateTime(lastYear, i, 1).ToString("yyyy-MM");
						dic.Add(lastYearMonth, od.Details.Where(od => od.OrderDate == lastYearMonth).Sum(od => Convert.ToDecimal(od.UnitPrice * od.Quantity) / 10000));
					}
					return dic;
				});
			
			

			return null;
		}
	}
}

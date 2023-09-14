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

		//今年度銷售數據
		[HttpGet]
		public async Task<AllTicktesSaleDTO> AllTicktesCurrentYearSale()
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
		//今年度銷售數據

		//前年度銷售數據
		[HttpGet]
		public async Task<AllTicktesSaleDTO> AllTicktesLastYearSale()
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
				.ToDictionaryAsync(od => od.CategoryId, od =>
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
				Airplane = resultList.Result[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Hotel = resultList.Result[2].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Transportation = resultList.Result[3].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				Attractions = resultList.Result[4].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				AirplaneOneYearSale = resultList.Result[1],
				HotelOneYearSale = resultList.Result[2],
				TransportationOneYearSale = resultList.Result[3],
				AttractionsOneYearSale = resultList.Result[4]
			};

			return result;
		}
		//前年度銷售數據

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

		//當週銷售數據
		[HttpGet]
		public async Task<AllTicktesSale> AllTicktesCurrentWeekSale()
		{
			decimal weekSale = await _db.OrderDetails
				.Where(od => EF.Functions.DateDiffWeek(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffWeek(od.Order.OrderDate, DateTime.Now) >= 0).
								SumAsync(od => (decimal)(od.UnitPrice * od.Quantity) / 10000);

			var resultList = await _db.OrderDetails.AsNoTracking().Include(od => od.Plan).ThenInclude(od => od.Product)
				.Where(od => EF.Functions.DateDiffWeek(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffWeek(od.Order.OrderDate, DateTime.Now) >= 0)
				.GroupBy(od => od.Plan.Product.Id)
				.Select(od => new
				{
					CategoryId = od.Key,
					Details = od.Select(od => new
					{
						OrderDate = od.Order.OrderDate.Value.ToString("MM-dd"),
						od.UnitPrice,
						od.Quantity
					})
				})
				.ToDictionaryAsync(od => od.CategoryId, od =>
				{
					var currentDate = DateTime.Now;
					var currentWeek = (DayOfWeek.Monday - currentDate.DayOfWeek) % 7;

					var dic = new Dictionary<string, decimal>();
					for (var i = 0; i < 7; i++)
					{
						var finalDate = currentDate.AddDays(currentWeek + i).ToString("MM-dd");
						dic.Add(finalDate, od.Details.Where(od => od.OrderDate == finalDate)
							.Sum(od => Convert.ToDecimal(od.UnitPrice * od.Quantity) / 10000));
					}
					return dic;
				});

			var result = new AllTicktesSale()
			{
				SaleTotal = weekSale,
				AirplaneSale = resultList[1],
				HotelSale = resultList[2],
				TransportationSale = resultList[3],
				AttractionsSale = resultList[4],
			};

			return result;
		}
		//當週銷售數據

		//當月銷售數據
		[HttpGet]
		public async Task<AllTicktesSale> AllTicktesCurrentMonthSale()
		{
			decimal monthSale = await _db.OrderDetails
				.Where(od => EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) >= 0).
								SumAsync(od => (decimal)(od.UnitPrice * od.Quantity) / 10000);

			var resultList = _db.OrderDetails.AsNoTracking().Include(od => od.Plan).ThenInclude(od => od.Product)
				.Where(od => EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) >= 0)
				.GroupBy(od => od.Plan.Product.Id)
				.Select(od => new
				{
					CategoryId = od.Key,
					Details = od.Select(od => new
					{
						OrderDate = od.Order.OrderDate.Value.ToString("MM-dd"),
						od.UnitPrice,
						od.Quantity
					})
				})
				.ToDictionaryAsync(od => od.CategoryId, od =>
				{
					int currentYear = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
					int currenMonth = Convert.ToInt32(DateTime.Now.ToString("MM"));
					var dic = new Dictionary<string, decimal>();
					for (var i = 1; i < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + 1; i++)
					{
						var currentMonthAndDay = new DateTime(currentYear, currenMonth, i).ToString("MM-dd");
						dic.Add(currentMonthAndDay, od.Details.Where(od => od.OrderDate == currentMonthAndDay).Sum(od => Convert.ToDecimal(od.UnitPrice * od.Quantity) / 10000));
					}
					return dic;
				});

			var result = new AllTicktesSale()
			{
				SaleTotal = monthSale,
				AirplaneSale = resultList.Result[1],
				HotelSale = resultList.Result[2],
				TransportationSale = resultList.Result[3],
				AttractionsSale = resultList.Result[4],
			};
			return result;
		}
		//當月銷售數據

		//當週銷售Top10
		[HttpGet]
		public async Task<AllTicktesTop10Sales> AllTicktesCurrentWeekTop10()
		{
			var resultList = await _db.OrderDetails.AsNoTracking()
				.Where(od => EF.Functions.DateDiffWeek(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffWeek(od.Order.OrderDate, DateTime.Now) >= 0)
				.GroupBy(od => od.Plan.Product.Id)
				.Select(od => new
				{
					CategoryId = od.Key,
					Details = od.Select(od => new
					{
						Name = od.Plan.Name,
						Data = od.Quantity.Value
					})
				}).ToDictionaryAsync(od => od.CategoryId, od =>
				{
					var dic = new Dictionary<string, int>();
					foreach (var orderDetail in od.Details)
					{
						if (!dic.ContainsKey(orderDetail.Name))
						{
							dic[orderDetail.Name] = (int)orderDetail.Data;
						}
						else
						{
							dic[orderDetail.Name] += (int)orderDetail.Data;
						}
					}
					return dic;
				});
			var result = new AllTicktesTop10Sales
			{
				AirplaneSale = resultList[1].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				HotelSale = resultList[2].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				TransportationSale = resultList[3].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				AttractionsSale = resultList[4].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList()
			};
			return result;
		}
		//當週銷售Top10

		//當月銷售Top10
		[HttpGet]
		public async Task<AllTicktesTop10Sales> AllTicktesCurrentMonthTop10()
		{
			var resultList = await _db.OrderDetails.AsNoTracking()
				.Where(od => EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffMonth(od.Order.OrderDate, DateTime.Now) >= 0)
				.GroupBy(od => od.Plan.Product.Id)
				.Select(od => new
				{
					CategoryId = od.Key,
					Details = od.Select(od => new
					{
						Name = od.Plan.Name,
						Data = od.Quantity.Value
					})
				}).ToDictionaryAsync(od => od.CategoryId, od =>
				{
					var dic = new Dictionary<string, int>();
					foreach (var orderDetail in od.Details)
					{
						if (!dic.ContainsKey(orderDetail.Name))
						{
							dic[orderDetail.Name] = (int)orderDetail.Data;
						}
						else
						{
							dic[orderDetail.Name] += (int)orderDetail.Data;
						}
					}
					return dic;
				});
			var result = new AllTicktesTop10Sales
			{
				AirplaneSale = resultList[1].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				HotelSale = resultList[2].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				TransportationSale = resultList[3].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList(),
				AttractionsSale = resultList[4].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList()
			};
			return result;
		}
		//當月銷售Top10
	}
}

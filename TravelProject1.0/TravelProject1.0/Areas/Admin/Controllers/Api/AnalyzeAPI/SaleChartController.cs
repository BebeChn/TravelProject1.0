using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using TravelProject1._0.Models;
using TravelProject1._0.Areas.Admin.Models.ChartViewModel.HotelChartDTO;
using TravelProject1._0.Areas.Admin.Models.ChartViewModel.SaleChartDTO;
using NuGet.Packaging;
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
				Airplane = resultList.ContainsKey(1) ? resultList[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
				Hotel = resultList.ContainsKey(2) ? resultList[2].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
				Transportation = resultList.ContainsKey(3) ? resultList[3].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
				Attractions = resultList.ContainsKey(4) ? resultList[4].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
				AirplaneOneYearSale = resultList.ContainsKey(1) ? resultList[1] : null,
				HotelOneYearSale = resultList.ContainsKey(1) ? resultList[1] : null,
				TransportationOneYearSale = resultList.ContainsKey(1) ? resultList[1] : null,
				AttractionsOneYearSale = resultList.ContainsKey(1) ? resultList[1] : null,
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

			var spareDic = new Dictionary<string, decimal>();
			var resultList = await _db.OrderDetails.AsNoTracking().Include(od => od.Plan).ThenInclude(od => od.Product)
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
						if (spareDic.ContainsKey(lastYearMonth))
						{
							spareDic.Add(lastYearMonth, 0);
						}
						dic.Add(lastYearMonth, od.Details.Where(od => od.OrderDate == lastYearMonth).Sum(od => Convert.ToDecimal(od.UnitPrice * od.Quantity) / 10000));
					}
					return dic;
				});

			var result = new AllTicktesSaleDTO()
			{
				OneyearSale = lastYearSale,
				Airplane = resultList.ContainsKey(1) ? resultList[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
				Hotel = resultList.ContainsKey(2) ? resultList[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
				Transportation = resultList.ContainsKey(3) ? resultList[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
				Attractions = resultList.ContainsKey(4) ? resultList[1].OrderBy(r => r.Key).Select(r => new HighChart3DGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
				AirplaneOneYearSale = resultList.ContainsKey(1) ? resultList[1] : spareDic,
				HotelOneYearSale = resultList.ContainsKey(2) ? resultList[2] : spareDic,
				TransportationOneYearSale = resultList.ContainsKey(3) ? resultList[3] : spareDic,
				AttractionsOneYearSale = resultList.ContainsKey(4) ? resultList[4] : spareDic,
			};

			return result;
		}
		//前年度銷售數據

		//當週銷售數據
		[HttpGet]
		public async Task<AllTicktesSale> AllTicktesCurrentWeekSale()
		{
			decimal weekSale = await _db.OrderDetails
				.Where(od => EF.Functions.DateDiffWeek(od.Order.OrderDate, DateTime.Now) <= 0 &&
								EF.Functions.DateDiffWeek(od.Order.OrderDate, DateTime.Now) >= 0).
								SumAsync(od => (decimal)(od.UnitPrice * od.Quantity) / 10000);
			//備用字典，以防沒有值
			var spareDic = new Dictionary<string,decimal>();
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
						if (!spareDic.ContainsKey(finalDate))
						{
							spareDic.Add(finalDate, 0);
						}
						dic.Add(finalDate, od.Details.Where(od => od.OrderDate == finalDate)
							.Sum(od => Convert.ToDecimal(od.UnitPrice * od.Quantity) / 10000));
					}
					return dic;
				});

			var result = new AllTicktesSale()
			{
				SaleTotal = weekSale,
				AirplaneSale =  resultList.ContainsKey(1) ? resultList[1] : spareDic,
				HotelSale = resultList.ContainsKey(2) ? resultList[2] : spareDic,
				TransportationSale = resultList.ContainsKey(3) ? resultList[3] : spareDic,
				AttractionsSale = resultList.ContainsKey(4) ? resultList[4] : spareDic,
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

			var spareDic = new Dictionary<string, decimal>();
			var resultList = await _db.OrderDetails.AsNoTracking().Include(od => od.Plan).ThenInclude(od => od.Product)
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
						if (!spareDic.ContainsKey(currentMonthAndDay))
						{
							spareDic.Add(currentMonthAndDay,0);
						}
						dic.Add(currentMonthAndDay, od.Details.Where(od => od.OrderDate == currentMonthAndDay).Sum(od => Convert.ToDecimal(od.UnitPrice * od.Quantity) / 10000));
					}
					return dic;
				});

			var result = new AllTicktesSale()
			{
				SaleTotal = monthSale,
				AirplaneSale = resultList.ContainsKey(1) ? resultList[1] : spareDic,
				HotelSale = resultList.ContainsKey(2) ? resultList[2] : spareDic,
				TransportationSale = resultList.ContainsKey(3) ? resultList[3] : spareDic,
				AttractionsSale = resultList.ContainsKey(4) ? resultList[4] : spareDic,
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
				AirplaneSale = resultList.ContainsKey(1) ? 
				resultList[1].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,

				HotelSale = resultList.ContainsKey(2) ?
				resultList[2].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,

				TransportationSale = resultList.ContainsKey(3) ?
				resultList[3].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,

				AttractionsSale = resultList.ContainsKey(4) ?
				resultList[4].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
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
				AirplaneSale = resultList.ContainsKey(1) ?
				resultList[1].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,

				HotelSale = resultList.ContainsKey(2) ?
				resultList[2].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,

				TransportationSale = resultList.ContainsKey(3) ?
				resultList[3].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,

				AttractionsSale = resultList.ContainsKey(4) ?
				resultList[4].OrderByDescending(r => r.Value).Take(10).Select(r => new HighChartBarGraphDTO { Name = r.Key, y = r.Value }).ToList() : null,
			};
			return result;
		}
		//當月銷售Top10
	}
}

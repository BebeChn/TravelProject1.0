using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Security.Permissions;
using TravelProject1._0.Areas.Admin.Models.ChartViewModel.HotelChartDTO;
using TravelProject1._0.Areas.Admin.Models.ChartViewModel.UserChartDTO;
using TravelProject1._0.Models;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
	[Area("Admin")]
	[Route("api/UserChart/{action}")]
	[ApiController]
	public class UserChartController : ControllerBase
	{
		private readonly TravelProjectAzureContext _db;

		public UserChartController(TravelProjectAzureContext travelProjectAzureContext)
		{
			_db = travelProjectAzureContext;
		}

		List<string> ageGroups = new List<string>
			{
				"18-22歲",
				"23-27歲",
				"28-32歲",
				"33-37歲",
				"38-42歲",
				"43-47歲",
				"48-52歲",
				"53-57歲",
				"58-62歲",
				"63-67歲",
				"68-72歲",
				"73-77歲",
				"78-82歲",
				"83-87歲",
				"88-92歲",
				"93-97歲"
			};

		private Dictionary<string, int> AgeGroupDictionart(List<string> ageGroups)
		{
			return ageGroups.ToDictionary(group => group, _ => 0);
		}

		private string GetAgeGroup(int? age)
		{
			string str = "未分類群組";
			foreach (var ageGroup in ageGroups)
			{
				var range = ageGroup.Split("-");
				var minAge = int.Parse(range[0].Replace("歲", ""));
				var maxAge = int.Parse(range[1].Replace("歲", ""));
				if (age >= minAge && age <= maxAge)
				{
					return ageGroup;
				}
			}
			return str;
		}

		[HttpGet]
		public async Task<GetUsersAnalyzeDTO> GetUsers()
		{
			var users = _db.Users.Select(u => new
			{
				UserId = u.UserId,
				Gender = u.Gender,
				Age = u.Age
			}).ToList();
			var orders = _db.Orders.Select(o => o.UserId).ToList();

			var Male = AgeGroupDictionart(ageGroups);
			var Female = AgeGroupDictionart(ageGroups);
			var payingMemberAgeGroup = AgeGroupDictionart(ageGroups);
			var nonPayingMemberAgeGroup = AgeGroupDictionart(ageGroups);

			//foreach (var user in users)
			//{
			//	switch (user.Age)
			//	{
			//		case int age when age >= 18 && age <= 22:
			//			if (user.Gender == "F")
			//			{
			//				Female["18-22歲"]++;
			//			}
			//			else
			//			{
			//				Male["18-22歲"]++;
			//			}
			//			if (orders.Any(order => order.UserId == user.UserId))
			//			{
			//				payingMemberAgeGroup["18-22歲"]++;
			//			}
			//			else
			//			{
			//				nonPayingMemberAgeGroup["18-22歲"]++;
			//			}
			//			break;
			foreach (var user in users)
			{
				var ageGroup = GetAgeGroup(user.Age);
				var isPayingMember = orders.Any(order => order == user.UserId);

				if (user.Gender == "F")
				{
					if (!Female.ContainsKey(ageGroup))
					{
						Female[ageGroup] = 0;
					}
					Female[ageGroup]++;
				}
				else
				{
					if (!Male.ContainsKey(ageGroup))
					{
						Male[ageGroup] = 0;
					}
					Male[ageGroup]++;
				}

				if (isPayingMember)
				{
					if (!payingMemberAgeGroup.ContainsKey(ageGroup))
					{
						payingMemberAgeGroup[ageGroup] = 0;
					}
					payingMemberAgeGroup[ageGroup]++;
				}
				else
				{
					if (!nonPayingMemberAgeGroup.ContainsKey(ageGroup))
					{
						nonPayingMemberAgeGroup[ageGroup] = 0;
					}
					nonPayingMemberAgeGroup[ageGroup]++;
				}
			}

			GetUsersAnalyzeDTO uaDTO = new GetUsersAnalyzeDTO();
			uaDTO.TotalMember = users.Count();
			uaDTO.PayingMemberAgeGroup = payingMemberAgeGroup;
			uaDTO.NonPayingMemberAgeGroup = nonPayingMemberAgeGroup;
			uaDTO.Male = Male;
			uaDTO.Female = Female;
			return uaDTO;
		}

		[HttpGet]
		public async Task<IEnumerable<HighChart3DGraph>> GetUserGender()
		{
			return _db.Users.AsNoTracking().Where(x => !string.IsNullOrEmpty(x.Gender))
			.Select(x => x.Gender).GroupBy(x => x).Select(x => new HighChart3DGraph
			{
				Name = x.Key == "F" ? "女性" : "男性",
				y = x.Count()
			});

			//var female = new Dictionary<string, int>();
			//var male = new Dictionary<string, int>();
			//var users = _db.Users.Select(u => u.Gender).ToList();

			//foreach (var user in users)
			//{
			//	if (user.Gender == "F")
			//	{
			//		if (female.ContainsKey(user.Gender))
			//		{
			//			female[user.Gender]++;
			//		}
			//		else
			//		{
			//			female[user.Gender] = 1;
			//		}
			//	}
			//	else
			//	{
			//		if (male.ContainsKey(user.Gender))
			//		{
			//			male[user.Gender]++;
			//		}
			//		else
			//		{
			//			male[user.Gender] = 1;
			//		}
			//	}
			//}

			//GetUserGenderDTO guDTO = new GetUserGenderDTO();
			//guDTO.Female = female;
			//guDTO.Male = male;
			//return guDTO;
		}

		[HttpGet]
		public async Task<IEnumerable<HighChart3DGraph>> GetUserAgeGroup()
		{
			var ageGroups = new List<HighChart3DGraph>();
			int minAge = 18;
			int maxAge = 97;
			int rangeAge = 5;

			for (int startAge = minAge; startAge < maxAge; startAge += rangeAge)
			{
				int endAge = startAge + rangeAge - 1;

				var ageGroup = _db.Users.AsNoTracking()
					.Where(u => u.Age.HasValue && u.Age >= startAge && u.Age <= endAge)
					.Select(u => u.Age)
					.ToList()
					.GroupBy(age => new
					{
						StartAge = startAge,
						EndAge = endAge
					})
					.Select(group => new HighChart3DGraph
					{
						Name = $"{group.Key.StartAge}-{group.Key.EndAge}歲",
						y = group.Count()
					})
					.ToList();

				ageGroups.AddRange(ageGroup);
			}

			return ageGroups;
		}

		[HttpGet]
		public async Task<IEnumerable<HighChart3DGraph>> GetIsPayAndNoPaying()
		{
			var userIds = await _db.Users.AsNoTracking().Include(u => u.Orders).Select(u => u.UserId).ToListAsync();
			var orderUserIds = await _db.Orders.AsNoTracking().Select(o => o.UserId).ToListAsync();
			var paying = userIds.Intersect(orderUserIds);
			var noPaying = userIds.Except(orderUserIds);

			var result = new List<HighChart3DGraph>
			{
				new HighChart3DGraph
				{
					Name = "已消費會員",
					y = paying.Count(),
				},
				new HighChart3DGraph
				{
					Name = "未消費會員",
					y = noPaying.Count(),
				}
			};

			return result;
		}
	}
}
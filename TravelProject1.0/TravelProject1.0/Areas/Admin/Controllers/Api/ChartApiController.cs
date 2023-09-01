using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using TravelProject1._0.Areas.Admin.Models.ChartViewModel;
using TravelProject1._0.Models;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
	[Area("Admin")]
	[Route("api/ChartApi/{action}")]
	[ApiController]
	public class ChartApiController : ControllerBase
	{
		private readonly TravelProjectAzureContext _db;
		public ChartApiController(TravelProjectAzureContext travelProjectAzureContext)
		{
			_db = travelProjectAzureContext;
		}

		[HttpGet]
		public async Task<UsersAnalyzeDTO> GetUsers()
		{
			var users = _db.Users.ToList();

			Dictionary<string,int> payingMemberAgeGroup = new Dictionary<string,int>
			{
				{ "18-22歲", 0 },
				{ "23-27歲", 0 },
				{ "28-32歲", 0 },
				{ "33-37歲", 0 },
				{ "38-42歲", 0 },
				{ "43-47歲", 0 },
				{ "48-52歲", 0 },
				{ "53-57歲", 0 },
				{ "58-62歲", 0 },
				{ "63-67歲", 0 },
				{ "68-72歲", 0 },
				{ "73-77歲", 0 },
				{ "78-82歲", 0 },
				{ "83-87歲", 0 },
				{ "88-92歲", 0 },
				{ "93-97歲", 0 }
			};

			Dictionary<string, int> nonPayingMemberAgeGroup = new Dictionary<string, int>
			{
				{ "18-22歲", 0 },
				{ "23-27歲", 0 },
				{ "28-32歲", 0 },
				{ "33-37歲", 0 },
				{ "38-42歲", 0 },
				{ "43-47歲", 0 },
				{ "48-52歲", 0 },
				{ "53-57歲", 0 },
				{ "58-62歲", 0 },
				{ "63-67歲", 0 },
				{ "68-72歲", 0 },
				{ "73-77歲", 0 },
				{ "78-82歲", 0 },
				{ "83-87歲", 0 },
				{ "88-92歲", 0 },
				{ "93-97歲", 0 }
			};

			foreach (var user in users)
			{
				switch (user.Age)
				{
					case int age when age >= 18 && age <= 22:
						if (user.Orders != null)
						{
							nonPayingMemberAgeGroup["18-22歲"]++;
						}
						else {
							payingMemberAgeGroup["18-22歲"]++;
						}
						break;
					case int age when age >= 23 && age <= 27:
						if (user.Orders != null)
						{
							nonPayingMemberAgeGroup["23-27歲"]++;
						}
						else
						{
							payingMemberAgeGroup["23-27歲"]++;
						}
						break;
					case int age when age >= 28 && age <= 32:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["28-32歲"]++;
						}
						else {
							nonPayingMemberAgeGroup["28-32歲"]++;
						}
						break;
					case int age when age >= 33 && age <= 37:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["33-37歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["33-37歲"]++;
						}
						break;
					case int age when age >= 38 && age <= 42:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["38-42歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["33-37歲"]++;
						}
						break;
					case int age when age >= 43 && age <= 47:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["43-47歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["43-47歲"]++;
						}
						break;
					case int age when age >= 48 && age <= 52:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["48-52歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["48-52歲"]++;
						}
						break;
					case int age when age >= 53 && age <= 57:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["53-57歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["53-57歲"]++;
						}
						break;
					case int age when age >= 58 && age <= 62:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["58-62歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["58-62歲"]++;
						}
						break;
					case int age when age >= 63 && age <= 67:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["63-67歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["63-67歲"]++;
						}
						break;
					case int age when age >= 68 && age <= 72:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["68-72歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["68-72歲"]++;
						}
						break;
					case int age when age >= 73 && age <= 77:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["73-77歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["73-77歲"]++;
						}
						break;
					case int age when age >= 78 && age <= 82:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["78-82歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["78-82歲"]++;
						}
						break;
					case int age when age >= 83 && age <= 87:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["83-87歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["83-87歲"]++;
						}
						break;
					case int age when age >= 88 && age <= 92:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["88-92歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["88-92歲"]++;
						}
						break;
					case int age when age >= 93 && age <= 97:
						if (user.Orders != null)
						{
							payingMemberAgeGroup["93-97歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["93-97歲"]++;
						}
						break;
					default: break;
				}
			}
			UsersAnalyzeDTO uaDTO = new UsersAnalyzeDTO();
			uaDTO.TotalMember = users.Count();
			uaDTO.PayingMemberAgeGroup = payingMemberAgeGroup;
			uaDTO.NonPayingMemberAgeGroup = nonPayingMemberAgeGroup;
			return uaDTO;
		}
	}
}

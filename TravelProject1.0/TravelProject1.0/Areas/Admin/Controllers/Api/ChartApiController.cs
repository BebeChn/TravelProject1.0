﻿using Microsoft.AspNetCore.Http;
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
			var orders = _db.Orders.ToList();

			Dictionary<string,int> Female = new Dictionary<string, int>()
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

			Dictionary<string,int> Male = new Dictionary<string, int>()
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
						if (user.Gender == "F")
						{
							Female["18-22歲"]++;
						}
						else
						{
							Male["18-22歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["18-22歲"]++;
						}
						else {
							nonPayingMemberAgeGroup["18-22歲"]++;
						}
						break;
					case int age when age >= 23 && age <= 27:
						if (user.Gender == "F")
						{
							Female["23-27歲"]++;
						}
						else
						{
							Male["23-27歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["23-27歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["23-27歲"]++;
						}
						break;
					case int age when age >= 28 && age <= 32:
						if (user.Gender == "F")
						{
							Female["28-32歲"]++;
						}
						else
						{
							Male["28-32歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["28-32歲"]++;
						}
						else {
							nonPayingMemberAgeGroup["28-32歲"]++;
						}
						break;
					case int age when age >= 33 && age <= 37:
						if (user.Gender == "F")
						{
							Female["33-37歲"]++;
						}
						else
						{
							Male["33-37歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["33-37歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["33-37歲"]++;
						}
						break;
					case int age when age >= 38 && age <= 42:
						if (user.Gender == "F")
						{
							Female["38-42歲"]++;
						}
						else
						{
							Male["38-42歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["38-42歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["33-37歲"]++;
						}
						break;
					case int age when age >= 43 && age <= 47:
						if (user.Gender == "F")
						{
							Female["43-47歲"]++;
						}
						else
						{
							Male["43-47歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["43-47歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["43-47歲"]++;
						}
						break;
					case int age when age >= 48 && age <= 52:
						if (user.Gender == "F")
						{
							Female["48-52歲"]++;
						}
						else
						{
							Male["48-52歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["48-52歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["48-52歲"]++;
						}
						break;
					case int age when age >= 53 && age <= 57:
						if (user.Gender == "F")
						{
							Female["53-57歲"]++;
						}
						else
						{
							Male["53-57歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["53-57歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["53-57歲"]++;
						}
						break;
					case int age when age >= 58 && age <= 62:
						if (user.Gender == "F")
						{
							Female["58-62歲"]++;
						}
						else
						{
							Male["58-62歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["58-62歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["58-62歲"]++;
						}
						break;
					case int age when age >= 63 && age <= 67:
						if (user.Gender == "F")
						{
							Female["63-67歲"]++;
						}
						else
						{
							Male["63-67歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["63-67歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["63-67歲"]++;
						}
						break;
					case int age when age >= 68 && age <= 72:
						if (user.Gender == "F")
						{
							Female["68-72歲"]++;
						}
						else
						{
							Male["68-72歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["68-72歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["68-72歲"]++;
						}
						break;
					case int age when age >= 73 && age <= 77:
						if (user.Gender == "F")
						{
							Female["73-77歲"]++;
						}
						else
						{
							Male["73-77歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["73-77歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["73-77歲"]++;
						}
						break;
					case int age when age >= 78 && age <= 82:
						if (user.Gender == "F")
						{
							Female["78-82歲"]++;
						}
						else
						{
							Male["78-82歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["78-82歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["78-82歲"]++;
						}
						break;
					case int age when age >= 83 && age <= 87:
						if (user.Gender == "F")
						{
							Female["83-87歲"]++;
						}
						else
						{
							Male["83-87歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["83-87歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["83-87歲"]++;
						}
						break;
					case int age when age >= 88 && age <= 92:
						if (user.Gender == "F")
						{
							Female["88-92歲"]++;
						}
						else
						{
							Male["88-92歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
						{
							payingMemberAgeGroup["88-92歲"]++;
						}
						else
						{
							nonPayingMemberAgeGroup["88-92歲"]++;
						}
						break;
					case int age when age >= 93 && age <= 97:
						if (user.Gender == "F")
						{
							Female["93-97歲"]++;
						}
						else
						{
							Male["93-97歲"]++;
						}
						if (orders.Any(order => order.UserId == user.UserId))
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
			uaDTO.Male = Male;
			uaDTO.Female = Female;
			return uaDTO;
		}
	}
}

﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Areas.Admin.Controllers.Api
{
	[Area("Admin")]
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class SuperApiController : ControllerBase
	{
		private readonly TravelProjectAzureContext _db;

		public SuperApiController(TravelProjectAzureContext db)
		{
			_db = db;
		}
		[HttpGet]
		public async Task<IEnumerable<SuperDTO>> SuperGET()
		{
			return _db.Admins.Select(x => new SuperDTO
			{
				Id = x.Id,
				Name = x.Name,
				Describe = x.Describe,
				Account = x.Account,
				CreateDate = x.CreateDate.GetValueOrDefault().ToString("u"),
				LoginDate = x.LoginDate.GetValueOrDefault().ToString("u")
			});
		}

		//[HttpPost]
		//public async Task<IEnumerable<SuperDTO>> SuperSearch(SuperDTO SuperDTO)
		//{
		//	return _db.Admins.Where(y =>
		//	y.Id == SuperDTO.Id ||
		//	y.Name.Contains(SuperDTO.Name) ||
		//	y.Describe.Contains(SuperDTO.Describe) ||
		//	y.Account.Contains(SuperDTO.Account)
		//	|| y.CreateDate == (SuperDTO.CreateDate) ||
		//	y.LoginDate == (SuperDTO.LoginDate)
		//	).Select(x => new SuperDTO
		//	{
		//		Id = x.Id,
		//		Name = x.Name,
		//		Describe = x.Describe,
		//		Account = x.Account,
		//		CreateDate = x.CreateDate,
		//		LoginDate = x.LoginDate
		//	});
		//}
	}
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

		[HttpPost]
		public async Task<IEnumerable<SuperFilterDTO>> SuperSearch(SuperFilterDTO SuperFilterDTO)
		{
			IEnumerable<TravelProject1._0.Models.Admin> query = _db.Admins;
			if (SuperFilterDTO.Id!=0 || SuperFilterDTO.RoleId!=0)
			{
				query = query.Where(y =>
					y.Id == SuperFilterDTO.Id ||
					//y.RoleId == SuperFilterDTO.RoleId ||
					y.Name.Contains(SuperFilterDTO.Name) ||
					y.Account.Contains(SuperFilterDTO.Account) ||
					y.Password.Contains(SuperFilterDTO.Password) ||
					y.Role.Contains(SuperFilterDTO.Role) ||
					y.Describe.Contains(SuperFilterDTO.Describe) ||
					y.Account.Contains(SuperFilterDTO.Account)
				);
			}
			return query.Select(x => new SuperFilterDTO
			{
				Id = x.Id,
				Name = x.Name,
				Describe = x.Describe,
				Account = x.Account,
				CreateDate=x.CreateDate.GetValueOrDefault().ToString("u"),
				LoginDate = x.LoginDate.GetValueOrDefault().ToString("u")
			});
		}
	}
}

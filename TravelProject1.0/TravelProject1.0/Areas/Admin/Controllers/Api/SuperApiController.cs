using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TravelProject1._0.Models.DTO;
using TravelProject1._0.Models;


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
		public async Task<IEnumerable<SuperFilterDTO>> SuperGET()
		{
			return _db.Admins.Select(x => new SuperFilterDTO
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

		[HttpPost]
		public async Task<string?> SuperPost(SuperDTO SuperDTO)
		{

			if (_db.Admins == null)
			{
				return null;
			}

			TravelProject1._0.Models.Admin add = new TravelProject1._0.Models.Admin
			{
				Id = SuperDTO.Id,
				Name = SuperDTO.Name,
				Account = SuperDTO.Account,
				Password = SuperDTO.Password,
				Role = SuperDTO.Role,
				Describe = SuperDTO.Describe,
				CreateDate=SuperDTO.CreateDate,
				LoginDate = SuperDTO.LoginDate
			};

			_db.Admins.Add(add);
			await _db.SaveChangesAsync();

			return "新增員工記錄成功";
		}
	}
}

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

		//搜尋
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

		//新增
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

		// DELETE: api/Employees/5				//刪除資料
		[HttpDelete("{id}")]
		public async Task<string?> SuperDel(int id)
		{
			if (_db.Admins == null)
			{
				return "刪除員工記錄失敗";
			}
			var admin = await _db.Admins.FindAsync(id);
			if (admin == null)
			{
				return "刪除員工記錄失敗";
			}

			try
			{
				_db.Admins.Remove(admin);
				await _db.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				return "刪除員工關聯記錄失敗";
			}
			return "刪除員工記錄成功";
		}

		// PUT: api/Employees/5					//更新資料，依自己設的DTO模型
		[HttpPut("{id}")]
		public async Task<string> PutSuper(int id, SuperDTO SuperDTO)
		{
			if (id != SuperDTO.Id)
			{
				return "更新員工記錄失敗";
			}

			TravelProject1._0.Models.Admin Sup = await _db.Admins.FindAsync(id);

			Sup.Describe = SuperDTO.Describe;
			Sup.Name = SuperDTO.Name;
			Sup.Account = SuperDTO.Account;
			//Sup.Password = SuperDTO.Password;
			//Sup.CreateDate = SuperDTO.CreateDate;
			//Sup.LoginDate = SuperDTO.LoginDate;
			//Sup.Role = SuperDTO.Role;

			_db.Entry(Sup).State = EntityState.Modified; //存回DTO

			try
			{
				await _db.SaveChangesAsync();//存回資料庫
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AdminExists(id))
				{
					return "更新員工記錄失敗";
				}
				else
				{
					throw;
				}
			}
			return "更新員工記錄成功";
		}

		private bool AdminExists(int id)
		{
			return (_db.Admins?.Any(e => e.Id == id)).GetValueOrDefault();
		}
	}
}

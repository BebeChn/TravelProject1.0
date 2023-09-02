using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
	[Route("api/[controller]/[action]/{id?}")]
	[ApiController]
	public class BookPlanApiController : ControllerBase
	{
		private readonly TravelProjectAzureContext _context;

		public BookPlanApiController(TravelProjectAzureContext context)
		{
			_context = context;
		}
		public IEnumerable<BookPlanDTO> GetBook(int id)
		{
			return _context.Products.Where(b => b.ProductId == id).Select(b => new BookPlanDTO
			{
				ProductName = b.ProductName,
				MainDescribe = b.MainDescribe,
				ProductId = b.ProductId,
				ShortDescribe = b.ShortDescribe,
				SubDescribe = b.SubDescribe,
				Img = b.Img,
				//Longgitude=b.Longgitude,
				//Latitude=b.Latitude
			});
		}

		public IEnumerable<BookPlanDTO> GetBookss(int id)
		{
			return _context.Plans.Where(c => c.ProductId == id).Select(c => new BookPlanDTO
			{
				Name = c.Name,
				Describe = c.Describe,
				PlanId = c.PlanId,
				PlanImg = c.PlanImg,
				PlanPrice= c.PlanPrice,
			});
		}

	}
}

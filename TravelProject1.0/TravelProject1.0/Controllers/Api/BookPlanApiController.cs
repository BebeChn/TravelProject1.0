using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
	[Route("api/[controller]/[action]")]
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
			return _context.Products.Where(b => b.Id == id).Select(b => new BookPlanDTO
			{
				ProductName = b.ProductName,
				MainDescribe = b.MainDescribe,
				ProductId = b.ProductId,
				ShortDescribe = b.ShortDescribe,
				SubDescribe = b.SubDescribe,
			});
		}

		public IEnumerable<BookPlanDTO> GetBookss()
		{
			return _context.Plans.Where(c => c.ProductId == 21).Select(c => new BookPlanDTO
			{
				Name = c.Name,
				Describe = c.Describe,
				PlanId = c.PlanId,
			});
		}

	}
}

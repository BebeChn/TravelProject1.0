using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public IEnumerable<BookPlanDTO> GetBook() 
		{
			return _context.Products.Where(b => b.Id == 21).Select(b => new BookPlanDTO
			{
				ProductName = b.ProductName,
				Price = b.Price,
				MainDescribe = b.MainDescribe,
				ProductId = b.ProductId,
				ShortDescribe = b.ShortDescribe,
				SubDescribe = b.SubDescribe,

			});
		}
	}
}

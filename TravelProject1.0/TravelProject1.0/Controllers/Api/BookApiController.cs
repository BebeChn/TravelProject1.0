using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProject1._0.Models;
using TravelProject1._0.Models.DTO;

namespace TravelProject1._0.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly TravelProjectAzureContext _context;

        public BookApiController (TravelProjectAzureContext context)
        {
            _context = context;
        }

        public IEnumerable<BookDTO> GetBooks() 
        {
            return _context.Products.Where(b => b.Id == 2).Select(b => new BookDTO
            {
                ProductName = b.ProductName,
                Price = b.Price,
                MainDescribe = b.MainDescribe
            });
        }
    }
}

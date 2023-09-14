using TravelProject1._0.Areas.Admin.Models.DTO;
using TravelProject1._0.Models;

namespace TravelProject1._0.Services
{
    public interface IProductSearchService
    {
        List<SearchProductDTO> SearchUsers(string query);
    }
    public class ProductSearchService : IProductSearchService
    {

        private readonly TravelProjectAzureContext _context;

        public ProductSearchService(TravelProjectAzureContext context)
        {
            _context = context;
        }

        public List<SearchProductDTO> SearchUsers(string query)
        {
            var searchResults = _context.Products
                .Where(u => u.ProductId.ToString().Contains(query) ||
                            u.Id.ToString().Contains(query) ||
                            u.ProductName.Contains(query) ||
                            u.MainDescribe.Contains(query) ||
                            u.Price.ToString().Contains(query)
                            )
                .Select(u => new SearchProductDTO
                {
                    ProductId = u.ProductId,
                    Id = u.Id,
                    ProductName = u.ProductName,
                    MainDescribe = u.MainDescribe,
                    Price = u.Price,
                }).ToList();

            return searchResults;
        }
    }
}
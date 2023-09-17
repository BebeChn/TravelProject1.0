namespace TravelProject1._0.Areas.Admin.Models.ViewModel
{
    public class PutProductVIewModel
    {
        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public string? MainDescribe { get; set; }

        public int Id { get; set; }

        public string? SubDescribe { get; set; }

        public string? ShortDescribe { get; set; }
        public IFormFile? imageFile { get; set; }
    }
}

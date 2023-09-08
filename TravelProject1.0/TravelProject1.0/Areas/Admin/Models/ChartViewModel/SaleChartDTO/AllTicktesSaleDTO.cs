namespace TravelProject1._0.Areas.Admin.Models.ChartViewModel.SaleChartDTO
{
	public class AllTicktesSaleDTO
	{
		public decimal OneyearSale { get; set; }
		public Dictionary<string, decimal> Airplane { get; set; }

		public Dictionary<string, decimal> Hotel { get; set; }

		public Dictionary<string, decimal> Transportation{ get; set;}

		public Dictionary<string , decimal> Attractions { get; set;}
	}
}

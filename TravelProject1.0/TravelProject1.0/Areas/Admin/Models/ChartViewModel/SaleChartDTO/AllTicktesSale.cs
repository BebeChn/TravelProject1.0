namespace TravelProject1._0.Areas.Admin.Models.ChartViewModel.SaleChartDTO
{
	public class AllTicktesSale
	{
		public decimal SaleTotal { get; set; }
		public Dictionary<string, decimal> AirplaneSale { get; set; }

		public Dictionary<string, decimal> HotelSale { get; set; }

		public Dictionary<string, decimal> TransportationSale { get; set; }

		public Dictionary<string, decimal> AttractionsSale { get; set; }

	}
}

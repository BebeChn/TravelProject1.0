namespace TravelProject1._0.Areas.Admin.Models.ChartViewModel.SaleChartDTO
{
	public class AllTicktesTop10Sales
	{
			public Dictionary<string,int> AirplaneSale { get; set; }

			public Dictionary<string,int> HotelSale { get; set; }

			public Dictionary<string,int> TransportationSale { get; set; }

			public Dictionary<string,int> AttractionsSale { get; set; }
	}
}

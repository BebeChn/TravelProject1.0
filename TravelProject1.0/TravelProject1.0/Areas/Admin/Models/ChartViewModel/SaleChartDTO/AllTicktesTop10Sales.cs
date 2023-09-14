namespace TravelProject1._0.Areas.Admin.Models.ChartViewModel.SaleChartDTO
{
	public class AllTicktesTop10Sales
	{
			public List<HighChartBarGraphDTO> AirplaneSale { get; set; }

			public List<HighChartBarGraphDTO> HotelSale { get; set; }

			public List<HighChartBarGraphDTO> TransportationSale { get; set; }

			public List<HighChartBarGraphDTO> AttractionsSale { get; set; }
	}
}

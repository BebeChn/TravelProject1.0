using TravelProject1._0.Areas.Admin.Models.ChartViewModel.HotelChartDTO;

namespace TravelProject1._0.Areas.Admin.Models.ChartViewModel.SaleChartDTO
{
	public class AllTicktesSaleDTO
	{
		public decimal OneyearSale { get; set; }
		public Dictionary<string, decimal> AirplaneOneYearSale { get; set; }

		public Dictionary<string, decimal> HotelOneYearSale { get; set; }

		public Dictionary<string, decimal> TransportationOneYearSale { get; set; }

		public Dictionary<string, decimal> AttractionsOneYearSale { get; set; }

		public List<HighChart3DGraphDTO> Airplane { get; set; }

		public List<HighChart3DGraphDTO> Hotel { get; set; }

		public List<HighChart3DGraphDTO> Transportation { get; set; }

		public List<HighChart3DGraphDTO> Attractions { get; set; }
	}
}

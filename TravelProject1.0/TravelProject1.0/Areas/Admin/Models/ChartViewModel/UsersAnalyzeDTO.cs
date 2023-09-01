namespace TravelProject1._0.Areas.Admin.Models.ChartViewModel
{
	public class UsersAnalyzeDTO
	{
		public int TotalMember { get; set; }

		public Dictionary<string, int> PayingMemberAgeGroup { get; set; }
		public Dictionary<string, int> NonPayingMemberAgeGroup { get; set; }
	}
}

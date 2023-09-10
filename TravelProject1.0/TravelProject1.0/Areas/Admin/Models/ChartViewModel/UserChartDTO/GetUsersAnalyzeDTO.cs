namespace TravelProject1._0.Areas.Admin.Models.ChartViewModel.UserChartDTO
{
    public class GetUsersAnalyzeDTO
    {
        public int TotalMember { get; set; }
        public Dictionary<string, int> Male { get; set; }
        public Dictionary<string, int> Female { get; set; }

        public Dictionary<string, int> PayingMemberAgeGroup { get; set; }
        public Dictionary<string, int> NonPayingMemberAgeGroup { get; set; }
    }
}

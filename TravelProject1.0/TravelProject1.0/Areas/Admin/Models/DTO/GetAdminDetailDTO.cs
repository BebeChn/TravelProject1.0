namespace TravelProject1._0.Areas.Admin.Models.DTO
{
    public class GetAdminDetailDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Account { get; set; }


        public string Describe { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? LoginDate { get; set; }

    }
}

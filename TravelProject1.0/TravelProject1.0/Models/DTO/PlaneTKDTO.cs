﻿namespace TravelProject1._0.Models.DTO
{
    public class PlaneTkdto
    {
        public int ProductId { get; set; }

        public int Id { get; set; }

        public string ProductName { get; set; } = null!;

        public decimal Price { get; set; }

        public string? MainDescribe { get; set; }

        public string? SubDescribe { get; set; }

        public string? ShortDescribe { get; set; }

        public string? Img { get; set; }

    }
}

﻿namespace TravelProject1._0.Models.DTO
{
    public class AttractionDTO
    {
        public int ProductId { get; set; }

        public int Id { get; set; }

        public string ProductName { get; set; } = null!;


        public string? Img { get; set; }
        public string? MainDescribe { get; set; }
    }
}
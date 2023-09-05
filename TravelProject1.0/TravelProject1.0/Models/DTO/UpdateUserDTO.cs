﻿namespace TravelProject1._0.Models.DTO
{
    public class UpdateUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
 
        public string Email { get; set; }

        public DateTime? Birthday { get; set; }
 
        public string? Gender { get; set; }

        public string Phone { get; set; }
    }
}
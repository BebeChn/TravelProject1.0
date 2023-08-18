﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("PlanCalendar")]
    public class PlanCalendar
    {
        public PlanCalendar() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CalenderId { get; set; }
        [ForeignKey(nameof(Plan))]
        public int PlanId { get; set; }

        public DateTime Date { get; set; }    

        public decimal  Price { get; set; }
    }
}
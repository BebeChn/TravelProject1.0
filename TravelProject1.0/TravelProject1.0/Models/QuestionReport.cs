﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("QuestionReport")]
    public class QuestionReport
    {
       
        public QuestionReport() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QuestionReportID{ get; set; }
       
        public string Id { get; set;}
        
        [Column(TypeName = "nvarchar")]
        [MaxLength(200)]
        public string Describe { get; set;}

        public DateTime ReportDate { get; set;} 
    }
}
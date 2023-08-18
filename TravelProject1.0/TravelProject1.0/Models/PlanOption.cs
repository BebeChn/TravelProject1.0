using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelProject1._0.Models
{
    [Table("PlanOption")]
    public class PlanOption
    {
        
        public PlanOption() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OptionID { get; set; }
        [ForeignKey(nameof(PlanCalendar)]
        public int CalendarID  { get; set; }

        public int Quantity { get; set; }
    }
}

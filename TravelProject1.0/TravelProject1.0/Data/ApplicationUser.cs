using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Data
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [Display(Name ="性別")]
        public bool Sex { get; set; }
    }
}

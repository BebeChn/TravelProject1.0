using System.ComponentModel.DataAnnotations;

namespace TravelProject1._0.Models.DTO
{
    public class RegisterDTO
    {
      
        public string Name { get; set; } = null!;
    
        public string Email { get; set; }
       
        public DateTime? Birthday { get; set; }
  
        public string Password { get; set; } = null!;
        public string? Gender { get; set; }
     
        public string Phone { get; set; } = null!;

    }
}


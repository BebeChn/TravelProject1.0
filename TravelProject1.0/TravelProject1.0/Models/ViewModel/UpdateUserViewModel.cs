using TravelProject1._0.Models.DTO;
namespace TravelProject1._0.Models.ViewModel
{
    public class UpdateUserViewModel : PostUserVewModel
    {
        public int UserId { get; set; }

        public string OldPassword { get; set; }

    }
}
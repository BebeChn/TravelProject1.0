﻿using TravelProject1._0.Models.DTO;
using static TravelProject1._0.Models.ViewModel.PutPeopleViewModel;

namespace TravelProject1._0.Models.ViewModel
{
    public class UpdateUserViewModel : User
    {
        public Guid UserId { get; set; }

        public string OldPassword { get; set; }

    }
}
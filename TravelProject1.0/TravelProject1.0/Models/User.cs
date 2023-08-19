using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class User: IdentityUser
{
  public virtual ICollection<Cart> Carts { get; set; }
}

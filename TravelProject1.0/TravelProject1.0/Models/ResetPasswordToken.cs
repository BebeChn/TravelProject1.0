using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class ResetPasswordToken
{
    public int? TokenId { get; set; }

    public int? UserId { get; set; }

    public byte[]? HashedToken { get; set; }

    public DateTime? ExpireTime { get; set; }
}

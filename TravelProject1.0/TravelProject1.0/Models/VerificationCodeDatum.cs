﻿using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class VerificationCodeDatum
{
    public string? Code { get; set; }

    public DateTime? ExpiryTime { get; set; }
}

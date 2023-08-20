using System;
using System.Collections.Generic;

namespace TravelProject1._0.Models;

public partial class QuestionReport
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Describe { get; set; }

    public DateTime? ReportDate { get; set; }

    public virtual User User { get; set; } = null!;
}

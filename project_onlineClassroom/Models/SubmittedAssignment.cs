using System;
using System.Collections.Generic;

namespace project_onlineClassroom.Models;

public partial class SubmittedAssignment
{
    public int Id { get; set; }

    public int AssignmentId { get; set; }

    public int UserId { get; set; }

    public string FilePath { get; set; } = null!;

    public DateTime SubmittedAt { get; set; }

    public byte? Grade { get; set; }

    public string? Feedback { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Assignment Assignment { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

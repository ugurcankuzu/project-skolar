using System;
using System.Collections.Generic;

namespace project_onlineClassroom.Models;

public partial class Assignment
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<SubmittedAssignment> SubmittedAssignments { get; set; } = new List<SubmittedAssignment>();
}

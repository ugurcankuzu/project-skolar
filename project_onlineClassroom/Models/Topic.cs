using System;
using System.Collections.Generic;

namespace project_onlineClassroom.Models;

public partial class Topic
{
    public int Id { get; set; }

    public int ClassId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<TopicNote> TopicNotes { get; set; } = new List<TopicNote>();
}

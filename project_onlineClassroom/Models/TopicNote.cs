using System;
using System.Collections.Generic;

namespace project_onlineClassroom.Models;

public partial class TopicNote
{
    public int Id { get; set; }

    public int TopicId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Topic Topic { get; set; } = null!;

    public virtual ICollection<TopicNoteBlock> TopicNoteBlocks { get; set; } = new List<TopicNoteBlock>();
}

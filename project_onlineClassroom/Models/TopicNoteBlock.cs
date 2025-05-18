namespace project_onlineClassroom.Models;

public partial class TopicNoteBlock
{
    public int Id { get; set; }

    public int NoteId { get; set; }

    public byte BlockOrder { get; set; }

    public string Type { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual TopicNote Note { get; set; } = null!;
}

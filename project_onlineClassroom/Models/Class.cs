namespace project_onlineClassroom.Models;

public partial class Class
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Title { get; set; } = null!;

    public byte UserLimit { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}

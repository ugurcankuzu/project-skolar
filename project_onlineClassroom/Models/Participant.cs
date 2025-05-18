namespace project_onlineClassroom.Models;

public partial class Participant
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ClassId { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace project_onlineClassroom.Models;

[Index(nameof(JoinCode),IsUnique = true)]

public partial class Class
{
    [Key]
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? ImageUrl { get; set; } // Optional image URL for the class
    public string? ImagePublicId { get; set; } // Optional public ID for the image (e.g., from a cloud storage service)

    [Required]
    [StringLength(6)]
    public string JoinCode { get; set; } = null!; // Unique code for joining the class
    public byte UserLimit { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

    public virtual User Owner { get; set; } = null!;

    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();

    public virtual ICollection<Topic> Topics { get; set; } = new List<Topic>();
}

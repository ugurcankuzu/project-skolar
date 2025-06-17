using System;
using System.Collections.Generic;

namespace project_onlineClassroom.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public bool IsEducator { get; set; }
       public string? AuthProvider { get; set; } // "Google", "Facebook" veya "Local" (şifre ile giriş)
    public string? ProviderKey { get; set; }  // Google'dan gelen benzersiz kullanıcı ID'si

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();

    public virtual ICollection<SubmittedAssignment> SubmittedAssignments { get; set; } = new List<SubmittedAssignment>();
}

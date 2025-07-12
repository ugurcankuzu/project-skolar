using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace project_onlineClassroom.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Assignment> Assignments { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<Participant> Participants { get; set; }

    public virtual DbSet<SubmittedAssignment> SubmittedAssignments { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<TopicNote> TopicNotes { get; set; }

    public virtual DbSet<TopicNoteBlock> TopicNoteBlocks { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Assignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Assignme__3214EC2768DBBDE8");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("class_ID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.DueDate)
                .HasColumnType("datetime")
                .HasColumnName("dueDate");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Class).WithMany(p => p.Assignments)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Assignmen__class__4F7CD00D");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Classes__3214EC274616FCFF");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.OwnerId).HasColumnName("owner_ID");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserLimit)
                .HasDefaultValue((byte)50)
                .HasColumnName("user_limit");

            entity.HasOne(d => d.Owner).WithMany(p => p.Classes)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Classes__owner_I__4CA06362");
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Particip__3214EC273F5A0346");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("class_ID");
            entity.Property(e => e.JoinedAt)
                .HasColumnType("datetime")
                .HasColumnName("joinedAt");
            entity.Property(e => e.UserId).HasColumnName("user_ID");

            entity.HasOne(d => d.Class).WithMany(p => p.Participants)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__class__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.Participants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Participa__user___4D94879B");
        });

        modelBuilder.Entity<SubmittedAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Submitte__3214EC27959CBEE5");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssignmentId).HasColumnName("assignment_ID");
            entity.Property(e => e.Feedback)
                .HasMaxLength(255)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("feedback");
            entity.Property(e => e.FilePath)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("filePath");
            entity.Property(e => e.Grade)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("grade");
            entity.Property(e => e.SubmittedAt)
                .HasColumnType("datetime")
                .HasColumnName("submittedAt");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("user_ID");

            entity.HasOne(d => d.Assignment).WithMany(p => p.SubmittedAssignments)
                .HasForeignKey(d => d.AssignmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Submitted__assig__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.SubmittedAssignments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Submitted__user___5165187F");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Topics__3214EC27A961F329");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ClassId).HasColumnName("class_ID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Class).WithMany(p => p.Topics)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Topics__class_ID__52593CB8");
        });

        modelBuilder.Entity<TopicNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TopicNot__3214EC27C943E927");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.TopicId).HasColumnName("topic_ID");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Topic).WithMany(p => p.TopicNotes)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TopicNote__topic__534D60F1");
        });

        modelBuilder.Entity<TopicNoteBlock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TopicNot__3214EC278353CC29");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BlockOrder).HasColumnName("blockOrder");
            entity.Property(e => e.Content)
                .HasMaxLength(255)
                .HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasDefaultValueSql("(NULL)")
                .HasColumnName("imageUrl");
            entity.Property(e => e.NoteId).HasColumnName("note_ID");
            entity.Property(e => e.Type)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("type");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Note).WithMany(p => p.TopicNoteBlocks)
                .HasForeignKey(d => d.NoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TopicNote__note___5441852A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27901035D5");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E616497103F8B").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("first_name");
            entity.Property(e => e.IsEducator).HasColumnName("isEducator");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("last_name");
            entity.Property(e => e.Password)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updatedAt");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

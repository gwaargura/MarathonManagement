using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MM_api.Models;

public partial class MarathonManagementV1Context : DbContext
{
    public MarathonManagementV1Context()
    {
    }

    public MarathonManagementV1Context(DbContextOptions<MarathonManagementV1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Checkpoint> Checkpoints { get; set; }

    public virtual DbSet<Marathon> Marathons { get; set; }

    public virtual DbSet<Organizer> Organizers { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__5E548648836F1F32");

            entity.Property(e => e.Action).HasMaxLength(255);
            entity.Property(e => e.TargetEntity).HasMaxLength(100);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Actor).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.ActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AuditLogs_Users");
        });

        modelBuilder.Entity<Checkpoint>(entity =>
        {
            entity.HasKey(e => e.CheckpointId).HasName("PK__Checkpoi__6C00DFE2BBC202AB");

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Latitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Longitude).HasColumnType("decimal(9, 6)");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Marathon).WithMany(p => p.Checkpoints)
                .HasForeignKey(d => d.MarathonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Checkpoints_Marathons");
        });

        modelBuilder.Entity<Marathon>(entity =>
        {
            entity.HasKey(e => e.MarathonId).HasName("PK__Marathon__6DEAE7190D3D1863");
            entity.Property(e => e.ThumbnailLink).HasDefaultValue("https://static.vecteezy.com/system/resources/previews/025/681/161/non_2x/marathon-running-continuous-one-line-drawing-woman-run-vector.jpg");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Location).HasMaxLength(255);
            entity.Property(e => e.MarathonName).HasMaxLength(255);
            entity.Property(e => e.RegistrationFee).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Upcoming");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Marathons)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Marathons_Organizers");
        });

        modelBuilder.Entity<Organizer>(entity =>
        {
            entity.HasKey(e => e.OrganizerId).HasName("PK__Organize__AD7DAD026251DC1A");

            entity.HasIndex(e => e.UserId, "UQ__Organize__1788CC4D4B1FE720").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.OrganizationName).HasMaxLength(255);
            entity.Property(e => e.Verified).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithOne(p => p.Organizer)
                .HasForeignKey<Organizer>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Organizers_Users");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A3894356B12");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .HasDefaultValue("MoMo");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");
            entity.Property(e => e.ResponseCode).HasMaxLength(20);
            entity.Property(e => e.VnpayTransactionId)
                .HasMaxLength(100)
                .HasColumnName("VNPayTransactionId");

            entity.HasOne(d => d.Marathon).WithMany(p => p.Payments)
                .HasForeignKey(d => d.MarathonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Marathons");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Users");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF58810D5E71447");

            entity.HasIndex(e => new { e.UserId, e.MarathonId }, "UQ_Registration_UserMarathon").IsUnique();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.RegisteredAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("Pending");

            entity.HasOne(d => d.Marathon).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.MarathonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registrations_Marathons");

            entity.HasOne(d => d.Payment).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_Registrations_Payments");

            entity.HasOne(d => d.User).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registrations_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A944A56B9");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160E1EE6509").IsUnique();

            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CF6A739CF");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4ABE690B0").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053492B83411").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.RefreshToken).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.RefreshTokenExpiry)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

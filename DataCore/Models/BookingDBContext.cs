using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DataCore.Models
{
    public partial class BookingDBContext : DbContext
    {
        public BookingDBContext()
        {
        }

        public BookingDBContext(DbContextOptions<BookingDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminRequests> AdminRequests { get; set; }
        public virtual DbSet<Broadcasts> Broadcasts { get; set; }
        public virtual DbSet<PermissionGroups> PermissionGroups { get; set; }
        public virtual DbSet<ProjectMinutesBooked> ProjectMinutesBooked { get; set; }
        public virtual DbSet<Projects> Projects { get; set; }
        public virtual DbSet<ProjectUsers> ProjectUsers { get; set; }
        public virtual DbSet<RequestTypes> RequestTypes { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=tcp:bookingnumbers.database.windows.net,1433;Initial Catalog=BookingDB;Persist Security Info=False;User ID=kaitindall;Password=Bae12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminRequests>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.DateRequested).HasColumnType("datetime");

                entity.Property(e => e.DateResponded).HasColumnType("datetime");

                entity.Property(e => e.RequestDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RequestTypeId).HasColumnName("RequestTypeID");

                entity.Property(e => e.RespondedByUserId).HasColumnName("RespondedByUserID");

                entity.Property(e => e.SentByUserId).HasColumnName("SentByUserID");

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.AdminRequests)
                    .HasForeignKey(d => d.RequestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AdminRequ__Reque__628FA481");

                entity.HasOne(d => d.RespondedByUser)
                    .WithMany(p => p.AdminRequestsRespondedByUser)
                    .HasForeignKey(d => d.RespondedByUserId)
                    .HasConstraintName("FK__AdminRequ__Respo__6477ECF3");

                entity.HasOne(d => d.SentByUser)
                    .WithMany(p => p.AdminRequestsSentByUser)
                    .HasForeignKey(d => d.SentByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AdminRequ__SentB__6383C8BA");
            });

            modelBuilder.Entity<Broadcasts>(entity =>
            {
                entity.HasKey(e => e.BroadcastId);

                entity.Property(e => e.BroadcastId).HasColumnName("BroadcastID");

                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasMaxLength(2048)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Broadcasts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Broadcast__UserI__71D1E811");
            });

            modelBuilder.Entity<PermissionGroups>(entity =>
            {
                entity.HasKey(e => e.PermissionsId);

                entity.Property(e => e.PermissionsId).HasColumnName("PermissionsID");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProjectMinutesBooked>(entity =>
            {
                entity.Property(e => e.ProjectMinutesBookedId).HasColumnName("ProjectMinutesBookedID");

                entity.Property(e => e.DateOfBooking).HasColumnType("datetime");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMinutesBooked)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectHo__Proje__6C190EBB");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectMinutesBooked)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectHo__UserI__6B24EA82");
            });

            modelBuilder.Entity<Projects>(entity =>
            {
                entity.HasKey(e => e.ProjectId);

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.ProjectDescription)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProjectUsers>(entity =>
            {
                entity.HasKey(e => e.ProjectUserId);

                entity.Property(e => e.ProjectUserId).HasColumnName("ProjectUserID");

                entity.Property(e => e.ProjectId).HasColumnName("ProjectID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectUsers)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectUs__Proje__6754599E");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ProjectUs__UserI__68487DD7");
            });

            modelBuilder.Entity<RequestTypes>(entity =>
            {
                entity.HasKey(e => e.RequestTypeId);

                entity.Property(e => e.RequestTypeId).HasColumnName("RequestTypeID");

                entity.Property(e => e.RequestName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.PermissionsId).HasColumnName("PermissionsID");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Permissions)
                    .WithMany(p => p.Roles)
                    .HasForeignKey(d => d.PermissionsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Roles__Permissio__5812160E");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.HashedPassword)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Users__RoleID__5DCAEF64");
            });
        }
    }
}

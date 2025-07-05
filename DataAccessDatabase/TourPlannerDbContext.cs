using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessDatabase
{
    public partial class TourPlannerContext : DbContext
    {
        public TourPlannerContext()
        {
        }

        public TourPlannerContext(DbContextOptions<TourPlannerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Log> Logs { get; set; }

        public virtual DbSet<Tour> Tours { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=127.0.0.1:5433;Database=tour_planner;Username=tour_server;Password=L9vCSjLsY07EAtwC;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.LogId).HasName("logs_pkey");

                entity.ToTable("logs");

                entity.Property(e => e.LogId).HasColumnName("log_id");
                entity.Property(e => e.Comment)
                    .HasMaxLength(500)
                    .HasColumnName("comment");
                entity.Property(e => e.DateCreated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("date_created");
                entity.Property(e => e.Difficulty).HasColumnName("difficulty");
                entity.Property(e => e.Rating).HasColumnName("rating");
                entity.Property(e => e.TotalDistance).HasColumnName("total_distance");
                entity.Property(e => e.TotalTime).HasColumnName("total_time");
                entity.Property(e => e.Tour).HasColumnName("tour");

                entity.HasOne(d => d.TourNavigation).WithMany(p => p.Logs)
                    .HasForeignKey(d => d.Tour)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("logs_tour_fkey");
            });

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.HasKey(e => e.TourId).HasName("tours_pkey");

                entity.ToTable("tours");

                entity.Property(e => e.TourId).HasColumnName("tour_id");
                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .HasColumnName("description");
                entity.Property(e => e.Distance).HasColumnName("distance");
                entity.Property(e => e.Duration).HasColumnName("duration");
                entity.Property(e => e.FromCoord)
                    .HasMaxLength(50)
                    .HasColumnName("from_coord");
                entity.Property(e => e.Information)
                    .HasMaxLength(500)
                    .HasColumnName("information");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
                entity.Property(e => e.ToCoord)
                    .HasMaxLength(50)
                    .HasColumnName("to_coord");
                entity.Property(e => e.TransportType)
                    .HasMaxLength(50)
                    .HasColumnName("transport_type");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    public partial class Tour
    {
        public int TourId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string FromCoord { get; set; } = null!;

        public string ToCoord { get; set; } = null!;

        public string? TransportType { get; set; }

        public float? Distance { get; set; }

        public TimeOnly? Duration { get; set; }

        public string? Information { get; set; }

        public virtual ICollection<Log> Logs { get; set; } = new List<Log>();
    }

    public partial class Log
    {
        public int LogId { get; set; }

        public DateTime DateCreated { get; set; }

        public string? Comment { get; set; }

        public int Difficulty { get; set; }

        public float TotalDistance { get; set; }

        public TimeOnly? TotalTime { get; set; }

        public int? Rating { get; set; }

        public int Tour { get; set; }

        public virtual Tour TourNavigation { get; set; } = null!;
    }
}

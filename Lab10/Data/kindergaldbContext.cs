using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Lab10.Models;

#nullable disable

namespace Lab10.Data
{
    public partial class kindergaldbContext : DbContext
    {
        public kindergaldbContext()
        {
        }

        public kindergaldbContext(DbContextOptions<kindergaldbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Appliance> Appliances { get; set; }
        public virtual DbSet<Fix> Fixes { get; set; }
        public virtual DbSet<Work> Works { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Appliance>(entity =>
            {
                entity.ToTable("appliances");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<Fix>(entity =>
            {
                entity.ToTable("fixes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Work>(entity =>
            {
                entity.ToTable("works");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Appliance).HasColumnName("appliance");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Fix).HasColumnName("fix");

                entity.HasOne(d => d.ApplianceNavigation)
                    .WithMany(p => p.Works)
                    .HasForeignKey(d => d.Appliance)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_works_appliances");

                entity.HasOne(d => d.FixNavigation)
                    .WithMany(p => p.Works)
                    .HasForeignKey(d => d.Fix)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_works_fixes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

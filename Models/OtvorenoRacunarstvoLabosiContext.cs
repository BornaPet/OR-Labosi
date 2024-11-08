using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OtvorenoRacunalstvoLabosi.Models;

public partial class OtvorenoRacunarstvoLabosiContext : DbContext
{
    public OtvorenoRacunarstvoLabosiContext()
    {
    }

    public OtvorenoRacunarstvoLabosiContext(DbContextOptions<OtvorenoRacunarstvoLabosiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Monument> Monuments { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=OtvorenoRacunarstvoLabosi;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("artist_pkey");

            entity.ToTable("artist");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.BirthYear).HasColumnName("birth_year");
            entity.Property(e => e.DeathYear).HasColumnName("death_year");
            entity.Property(e => e.MonumentId).HasColumnName("monument_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(100)
                .HasColumnName("nationality");
                
            });

        modelBuilder.Entity<Monument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("monument_pkey");

            entity.ToTable("monument");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.District)
                .HasMaxLength(100)
                .HasColumnName("district");
            entity.Property(e => e.Height).HasColumnName("height");
            entity.Property(e => e.HistoricalSignificance).HasColumnName("historical_significance");
            entity.Property(e => e.Location)
                .HasMaxLength(255)
                .HasColumnName("location");
            entity.Property(e => e.Material)
                .HasMaxLength(50)
                .HasColumnName("material");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Popularity).HasColumnName("popularity");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.YearInstalled).HasColumnName("year_installed");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

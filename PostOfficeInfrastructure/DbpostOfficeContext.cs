using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PostOfficeDomain.Model;

namespace PostOfficeInfrastructure;

public partial class DbpostOfficeContext : DbContext
{
    public DbpostOfficeContext()
    {
    }

    public DbpostOfficeContext(DbContextOptions<DbpostOfficeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Courier> Couriers { get; set; }

    public virtual DbSet<FacilityType> FacilityTypes { get; set; }

    public virtual DbSet<Parcel> Parcels { get; set; }

    public virtual DbSet<ParcelStatus> ParcelStatuses { get; set; }

    public virtual DbSet<PostalFacility> PostalFacilitys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-LGFLGFJ\\SQLEXPRESS; Database=DBPostOffice; Trusted_Connection=True; TrustServerCertificate=True; ");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ContactNumber).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Courier>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.Parcel).WithMany(p => p.Couriers)
                .HasForeignKey(d => d.ParcelId)
                .HasConstraintName("FK_Couriers_Parcels");
        });

        modelBuilder.Entity<FacilityType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Parcel>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeliveryAddress).HasMaxLength(50);

            entity.HasOne(d => d.CurrentLocation).WithMany(p => p.ParcelCurrentLocations)
                .HasForeignKey(d => d.CurrentLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_PostalFacilitys");

            entity.HasOne(d => d.DeliveryPoints).WithMany(p => p.ParcelDeliveryPoints)
                .HasForeignKey(d => d.DeliveryPointsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_PostalFacilitys1");

            entity.HasOne(d => d.DeparturePoints).WithMany(p => p.ParcelDeparturePoints)
                .HasForeignKey(d => d.DeparturePointsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_PostalFacilitys2");

            entity.HasOne(d => d.Reciver).WithMany(p => p.ParcelRecivers)
                .HasForeignKey(d => d.ReciverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_Clients");

            entity.HasOne(d => d.Sender).WithMany(p => p.ParcelSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_Clients1");

            entity.HasOne(d => d.Status).WithMany(p => p.Parcels)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Parcels_ParcelStatuses");
        });

        modelBuilder.Entity<ParcelStatus>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<PostalFacility>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkSchedule).HasMaxLength(50);

            entity.HasOne(d => d.FacilityType).WithMany(p => p.PostalFacilities)
                .HasForeignKey(d => d.FacilityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PostalFacilitys_FacilityTypes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

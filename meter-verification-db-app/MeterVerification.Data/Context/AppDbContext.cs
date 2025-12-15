using MeterVerification.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MeterVerification.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Organization> Organizations => Set<Organization>();
        public DbSet<DeviceType> DeviceTypes => Set<DeviceType>();
        public DbSet<DeviceModification> DeviceModifications => Set<DeviceModification>();
        public DbSet<Verification> Verifications => Set<Verification>();
        public DbSet<MeasurementUnit> MeasurementUnits => Set<MeasurementUnit>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Organization
            modelBuilder.Entity<Organization>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.HasIndex(e => e.Name).IsUnique();
            });
            // DeviceType
            modelBuilder.Entity<DeviceType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.RegistrationNumber)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.Property(e => e.Designation)
                    .HasMaxLength(100);
                entity.Property(e => e.VerificationInterval);

                entity.HasIndex(e => e.RegistrationNumber).IsUnique();
                // связь с MeasurementUnit
                entity.HasOne(dt => dt.MeasurementUnit)
                    .WithMany(mu => mu.DeviceTypes)
                    .HasForeignKey(dt => dt.MeasurementUnitId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            // MeasurementUnit
            modelBuilder.Entity<MeasurementUnit>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.ShortName).IsRequired().HasMaxLength(10);
                entity.HasIndex(e => e.Name).IsUnique();
            });
            // DeviceModification
            modelBuilder.Entity<DeviceModification>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Designation)
                    .HasMaxLength(100);

                entity.HasOne(dm => dm.DeviceType)
                    .WithMany(dt => dt.Modifications)
                    .HasForeignKey(dm => dm.DeviceTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.Name, e.DeviceTypeId }).IsUnique();
            });
            // Verification
            modelBuilder.Entity<Verification>(entity =>
            {
                entity.HasKey(e => e.Id);

                // основные поля
                entity.Property(e => e.VersionId)
                    .HasMaxLength(100);

                entity.Property(e => e.SerialNumber)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.VerificationDate)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(e => e.ValidUntil)
                    .IsRequired()
                    .HasColumnType("date");

                entity.Property(e => e.CertificateNumber)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.IsSuitable)
                    .IsRequired();

                // связи
                entity.HasOne(v => v.Organization)
                    .WithMany(o => o.Verifications)
                    .HasForeignKey(v => v.OrganizationId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.DeviceType)
                    .WithMany(dt => dt.Verifications)
                    .HasForeignKey(v => v.DeviceTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(v => v.DeviceModification)
                    .WithMany(dm => dm.Verifications)
                    .HasForeignKey(v => v.DeviceModificationId)
                    .OnDelete(DeleteBehavior.Restrict);

                // индексы
                entity.HasIndex(e => e.CertificateNumber);
                entity.HasIndex(e => e.VerificationDate);
                entity.HasIndex(e => e.ValidUntil);
                entity.HasIndex(e => e.IsSuitable);
                entity.HasIndex(e => e.OrganizationId);
                entity.HasIndex(e => e.DeviceTypeId);
            });
        }
    }
}
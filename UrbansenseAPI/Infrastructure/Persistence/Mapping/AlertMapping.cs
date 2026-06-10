using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Infrastructure.Persistence.Mapping;

public class AlertMapping : IEntityTypeConfiguration<Alert>
{
    public void Configure(EntityTypeBuilder<Alert> builder)
    {
        builder.ToTable("ALERTS");

        builder.HasKey(al => al.id);

        builder.Property(al => al.id)
            .HasColumnName("ID_ALERT")
            .UseIdentityColumn();

        builder.Property(al => al.city)
            .HasColumnName("CITY")
            .HasColumnType("VARCHAR2(255)")
            .IsRequired();

        builder.Property(al => al.type)
            .HasColumnName("TYPE")
            .HasColumnType("VARCHAR2(50)")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(al => al.severity)
            .HasColumnName("SEVERITY")
            .HasColumnType("VARCHAR2(50)")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(al => al.message)
            .HasColumnName("MESSAGE")
            .HasColumnType("VARCHAR2(1000)")
            .IsRequired();

        builder.Property(al => al.region)
            .HasColumnName("REGION")
            .HasColumnType("VARCHAR2(255)");

        builder.Property(al => al.latitude)
            .HasColumnName("LATITUDE")
            .HasColumnType("NUMBER(10,6)");

        builder.Property(al => al.longitude)
            .HasColumnName("LONGITUDE")
            .HasColumnType("NUMBER(10,6)");

        builder.Property(al => al.validFrom)
            .HasColumnName("VALID_FROM")
            .HasColumnType("TIMESTAMP")
            .IsRequired();

        builder.Property(al => al.validUntil)
            .HasColumnName("VALID_UNTIL")
            .HasColumnType("TIMESTAMP")
            .IsRequired();

        builder.Property(al => al.active)
            .HasColumnName("ACTIVE")
            .HasColumnType("NUMBER(1)")
            .IsRequired();

        builder.Property(al => al.transitLineId)
            .HasColumnName("ID_TRANSIT_LINE");

        builder.HasOne(al => al.TransitLine)
            .WithMany(tl => tl.Alerts)
            .HasForeignKey(al => al.transitLineId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}

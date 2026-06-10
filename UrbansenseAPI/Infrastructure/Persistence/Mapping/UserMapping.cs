using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Infrastructure.Persistence.Mapping;

public class UserMapping : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("USERS");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("ID_USER")
            .UseIdentityColumn();

        builder.Property(u => u.Username)
            .HasColumnName("USERNAME")
            .HasColumnType("VARCHAR2(100)")
            .IsRequired();

        builder.HasIndex(u => u.Username)
            .IsUnique();

        builder.Property(u => u.Password)
            .HasColumnName("PASSWORD")
            .HasColumnType("VARCHAR2(255)")
            .IsRequired();

        builder.Property(u => u.Role)
            .HasColumnName("ROLE")
            .HasColumnType("VARCHAR2(50)")
            .IsRequired();

        builder.Property(u => u.Active)
            .HasColumnName("ACTIVE")
            .HasColumnType("NUMBER(1)")
            .IsRequired();
    }
}

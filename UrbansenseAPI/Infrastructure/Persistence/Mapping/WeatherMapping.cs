using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Infrastructure.Persistence.Mapping;

public class WeatherMapping : IEntityTypeConfiguration<Weather>
{
    public void Configure(EntityTypeBuilder<Weather> builder)
    {
        builder.ToTable("WEATHER_RECORDS");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasColumnName("ID_WEATHER")
            .UseIdentityColumn();

        builder.Property(w => w.City)
            .HasColumnName("CITY")
            .HasColumnType("VARCHAR2(100)")
            .IsRequired();

        builder.Property(w => w.Latitude)
            .HasColumnName("LATITUDE")
            .HasColumnType("NUMBER(10,6)")
            .IsRequired();

        builder.Property(w => w.Longitude)
            .HasColumnName("LONGITUDE")
            .HasColumnType("NUMBER(10,6)")
            .IsRequired();

        builder.Property(w => w.Temperature)
            .HasColumnName("TEMPERATURE")
            .HasColumnType("NUMBER(5,2)");

        builder.Property(w => w.FeelsLike)
            .HasColumnName("FEELS_LIKE")
            .HasColumnType("NUMBER(5,2)");

        builder.Property(w => w.Humidity)
            .HasColumnName("HUMIDITY")
            .HasColumnType("NUMBER(3)");

        builder.Property(w => w.WindSpeed)
            .HasColumnName("WIND_SPEED")
            .HasColumnType("NUMBER(6,2)");

        builder.Property(w => w.RainMm)
            .HasColumnName("RAIN_MM")
            .HasColumnType("NUMBER(6,2)");

        builder.Property(w => w.UvIndex)
            .HasColumnName("UV_INDEX")
            .HasColumnType("NUMBER(3)");

        builder.Property(w => w.Condition)
            .HasColumnName("CONDITION")
            .HasColumnType("VARCHAR2(20)")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(w => w.Description)
            .HasColumnName("DESCRIPTION")
            .HasColumnType("VARCHAR2(255)");

        builder.Property(w => w.RecordedAt)
            .HasColumnName("RECORDED_AT")
            .HasColumnType("TIMESTAMP")
            .IsRequired();
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrbansenseAPI.Domain.Models;

namespace UrbansenseAPI.Infrastructure.Persistence.Mapping;

public class TransitLineMapping : IEntityTypeConfiguration<TransitLine>
{
    public void Configure(EntityTypeBuilder<TransitLine> builder)
    {
        builder.ToTable("TRANSIT_LINES");

        builder.HasKey(tl => tl.id);

        builder.Property(tl => tl.id)
            .HasColumnName("ID_LINE")
            .UseIdentityColumn();

        builder.Property(tl => tl.code)
            .HasColumnName("CODE")
            .HasColumnType("VARCHAR2(10)")
            .IsRequired();

        builder.Property(tl => tl.name)
            .HasColumnName("NAME")
            .HasColumnType("VARCHAR2(100)")
            .IsRequired();

        builder.Property(tl => tl.technician)
            .HasColumnName("OPERATOR")
            .HasColumnType("VARCHAR2(100)")
            .IsRequired();

        builder.Property(tl => tl.color)
            .HasColumnName("COLOR")
            .HasColumnType("VARCHAR2(10)")
            .IsRequired();

        builder.Property(tl => tl.rainVulnerability)
            .HasColumnName("RAIN_VULNERABILITY")
            .HasColumnType("NUMBER(4,2)")
            .IsRequired();

        builder.Property(tl => tl.rainThresholdMm)
            .HasColumnName("RAIN_THRESHOLD_MM")
            .HasColumnType("NUMBER(5,2)")
            .IsRequired();

        builder.Property(tl => tl.vulnerableSection)
            .HasColumnName("VULNERABLE_SECTION")
            .HasColumnType("VARCHAR2(255)")
            .IsRequired();

        builder.Property(tl => tl.avgDelayPctOnRain)
            .HasColumnName("AVG_DELAY_PCT")
            .HasColumnType("NUMBER(3)")
            .IsRequired();

        // Seed das 12 linhas do Metrô/CPTM de São Paulo
        builder.HasData(
            new TransitLine { id=1,  code="L1",  name="Linha 1 - Azul",      technician="Metrô SP",       color="#0155A8", rainVulnerability=0.70, rainThresholdMm=15, vulnerableSection="Tucuruvi - Jabaquara",            avgDelayPctOnRain=25 },
            new TransitLine { id=2,  code="L2",  name="Linha 2 - Verde",     technician="Metrô SP",       color="#007E5E", rainVulnerability=0.50, rainThresholdMm=20, vulnerableSection="Vila Madalena - Vila Prudente",   avgDelayPctOnRain=15 },
            new TransitLine { id=3,  code="L3",  name="Linha 3 - Vermelha",  technician="Metrô SP",       color="#EE3124", rainVulnerability=0.80, rainThresholdMm=10, vulnerableSection="Itaquera - Barra Funda",          avgDelayPctOnRain=30 },
            new TransitLine { id=4,  code="L4",  name="Linha 4 - Amarela",   technician="ViaQuatro",      color="#FFD400", rainVulnerability=0.30, rainThresholdMm=25, vulnerableSection="Luz - Butantã",                   avgDelayPctOnRain=10 },
            new TransitLine { id=5,  code="L5",  name="Linha 5 - Lilás",     technician="ViaMobilidade",  color="#9B2990", rainVulnerability=0.50, rainThresholdMm=20, vulnerableSection="Capão Redondo - Chácara Klabin",  avgDelayPctOnRain=15 },
            new TransitLine { id=6,  code="L7",  name="Linha 7 - Rubi",      technician="CPTM",           color="#DC241F", rainVulnerability=0.90, rainThresholdMm=8,  vulnerableSection="Luz - Francisco Morato",          avgDelayPctOnRain=40 },
            new TransitLine { id=7,  code="L8",  name="Linha 8 - Diamante",  technician="ViaMobilidade",  color="#97999B", rainVulnerability=0.85, rainThresholdMm=10, vulnerableSection="Júlio Prestes - Amador Bueno",    avgDelayPctOnRain=35 },
            new TransitLine { id=8,  code="L9",  name="Linha 9 - Esmeralda", technician="ViaMobilidade",  color="#01A651", rainVulnerability=0.95, rainThresholdMm=5,  vulnerableSection="Osasco - Grajaú",                 avgDelayPctOnRain=50 },
            new TransitLine { id=9,  code="L10", name="Linha 10 - Turquesa", technician="CPTM",           color="#009FC4", rainVulnerability=0.80, rainThresholdMm=12, vulnerableSection="Mauá - Brás",                     avgDelayPctOnRain=30 },
            new TransitLine { id=10, code="L11", name="Linha 11 - Coral",    technician="CPTM",           color="#F26522", rainVulnerability=0.60, rainThresholdMm=18, vulnerableSection="Luz - Guaianases",                avgDelayPctOnRain=20 },
            new TransitLine { id=11, code="L12", name="Linha 12 - Safira",   technician="CPTM",           color="#133E8E", rainVulnerability=0.55, rainThresholdMm=20, vulnerableSection="Brás - Calmon Viana",             avgDelayPctOnRain=18 },
            new TransitLine { id=12, code="L13", name="Linha 13 - Jade",     technician="CPTM",           color="#00B398", rainVulnerability=0.30, rainThresholdMm=30, vulnerableSection="Engenheiro Goulart - Guarulhos",  avgDelayPctOnRain=10 }
        );
    }
}

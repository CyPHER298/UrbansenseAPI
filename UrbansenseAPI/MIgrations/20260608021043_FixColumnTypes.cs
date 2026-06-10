using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UrbansenseAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixColumnTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 1m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 2m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 3m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 4m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 5m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 6m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 7m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 8m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 9m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 10m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 11m);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 12m);

            migrationBuilder.AlterColumn<long>(
                name: "ID_WEATHER",
                table: "WEATHER_RECORDS",
                type: "NUMBER(19)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "NUMBER")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AlterColumn<long>(
                name: "ID_USER",
                table: "USERS",
                type: "NUMBER(19)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "NUMBER")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AlterColumn<long>(
                name: "ID_LINE",
                table: "TRANSIT_LINES",
                type: "NUMBER(19)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "NUMBER")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AlterColumn<long>(
                name: "ID_ALERT",
                table: "ALERTS",
                type: "NUMBER(19)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "NUMBER")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.InsertData(
                table: "TRANSIT_LINES",
                columns: new[] { "ID_LINE", "AVG_DELAY_PCT", "CODE", "COLOR", "NAME", "RAIN_THRESHOLD_MM", "RAIN_VULNERABILITY", "OPERATOR", "VULNERABLE_SECTION" },
                values: new object[,]
                {
                    { 1L, (byte)25, "L1", "#0155A8", "Linha 1 - Azul", 15m, 0.7m, "Metrô SP", "Tucuruvi - Jabaquara" },
                    { 2L, (byte)15, "L2", "#007E5E", "Linha 2 - Verde", 20m, 0.5m, "Metrô SP", "Vila Madalena - Vila Prudente" },
                    { 3L, (byte)30, "L3", "#EE3124", "Linha 3 - Vermelha", 10m, 0.8m, "Metrô SP", "Itaquera - Barra Funda" },
                    { 4L, (byte)10, "L4", "#FFD400", "Linha 4 - Amarela", 25m, 0.3m, "ViaQuatro", "Luz - Butantã" },
                    { 5L, (byte)15, "L5", "#9B2990", "Linha 5 - Lilás", 20m, 0.5m, "ViaMobilidade", "Capão Redondo - Chácara Klabin" },
                    { 6L, (byte)40, "L7", "#DC241F", "Linha 7 - Rubi", 8m, 0.9m, "CPTM", "Luz - Francisco Morato" },
                    { 7L, (byte)35, "L8", "#97999B", "Linha 8 - Diamante", 10m, 0.85m, "ViaMobilidade", "Júlio Prestes - Amador Bueno" },
                    { 8L, (byte)50, "L9", "#01A651", "Linha 9 - Esmeralda", 5m, 0.95m, "ViaMobilidade", "Osasco - Grajaú" },
                    { 9L, (byte)30, "L10", "#009FC4", "Linha 10 - Turquesa", 12m, 0.8m, "CPTM", "Mauá - Brás" },
                    { 10L, (byte)20, "L11", "#F26522", "Linha 11 - Coral", 18m, 0.6m, "CPTM", "Luz - Guaianases" },
                    { 11L, (byte)18, "L12", "#133E8E", "Linha 12 - Safira", 20m, 0.55m, "CPTM", "Brás - Calmon Viana" },
                    { 12L, (byte)10, "L13", "#00B398", "Linha 13 - Jade", 30m, 0.3m, "CPTM", "Engenheiro Goulart - Guarulhos" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "TRANSIT_LINES",
                keyColumn: "ID_LINE",
                keyValue: 12L);

            migrationBuilder.AlterColumn<decimal>(
                name: "ID_WEATHER",
                table: "WEATHER_RECORDS",
                type: "NUMBER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "NUMBER(19)")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "ID_USER",
                table: "USERS",
                type: "NUMBER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "NUMBER(19)")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "ID_LINE",
                table: "TRANSIT_LINES",
                type: "NUMBER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "NUMBER(19)")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "ID_ALERT",
                table: "ALERTS",
                type: "NUMBER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "NUMBER(19)")
                .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1")
                .OldAnnotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1");

            migrationBuilder.InsertData(
                table: "TRANSIT_LINES",
                columns: new[] { "ID_LINE", "AVG_DELAY_PCT", "CODE", "COLOR", "NAME", "RAIN_THRESHOLD_MM", "RAIN_VULNERABILITY", "OPERATOR", "VULNERABLE_SECTION" },
                values: new object[,]
                {
                    { 1m, (byte)25, "L1", "#0155A8", "Linha 1 - Azul", 15m, 0.7m, "Metrô SP", "Tucuruvi - Jabaquara" },
                    { 2m, (byte)15, "L2", "#007E5E", "Linha 2 - Verde", 20m, 0.5m, "Metrô SP", "Vila Madalena - Vila Prudente" },
                    { 3m, (byte)30, "L3", "#EE3124", "Linha 3 - Vermelha", 10m, 0.8m, "Metrô SP", "Itaquera - Barra Funda" },
                    { 4m, (byte)10, "L4", "#FFD400", "Linha 4 - Amarela", 25m, 0.3m, "ViaQuatro", "Luz - Butantã" },
                    { 5m, (byte)15, "L5", "#9B2990", "Linha 5 - Lilás", 20m, 0.5m, "ViaMobilidade", "Capão Redondo - Chácara Klabin" },
                    { 6m, (byte)40, "L7", "#DC241F", "Linha 7 - Rubi", 8m, 0.9m, "CPTM", "Luz - Francisco Morato" },
                    { 7m, (byte)35, "L8", "#97999B", "Linha 8 - Diamante", 10m, 0.85m, "ViaMobilidade", "Júlio Prestes - Amador Bueno" },
                    { 8m, (byte)50, "L9", "#01A651", "Linha 9 - Esmeralda", 5m, 0.95m, "ViaMobilidade", "Osasco - Grajaú" },
                    { 9m, (byte)30, "L10", "#009FC4", "Linha 10 - Turquesa", 12m, 0.8m, "CPTM", "Mauá - Brás" },
                    { 10m, (byte)20, "L11", "#F26522", "Linha 11 - Coral", 18m, 0.6m, "CPTM", "Luz - Guaianases" },
                    { 11m, (byte)18, "L12", "#133E8E", "Linha 12 - Safira", 20m, 0.55m, "CPTM", "Brás - Calmon Viana" },
                    { 12m, (byte)10, "L13", "#00B398", "Linha 13 - Jade", 30m, 0.3m, "CPTM", "Engenheiro Goulart - Guarulhos" }
                });
        }
    }
}

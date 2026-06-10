using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrbansenseAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedWeatherData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Sao Paulo — 3 registros (Storm, Rain, Clear com UV alto)
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Sao Paulo',-23.5505,-46.6333,19.0,18.0,95,12.0,35.0,2,'Storm','Tempestade forte com raios',SYSTIMESTAMP)");
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Sao Paulo',-23.5505,-46.6333,21.5,20.0,85,8.0,22.0,1,'Rain','Chuva forte com acumulado significativo',SYSTIMESTAMP)");
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Sao Paulo',-23.5505,-46.6333,28.0,30.5,60,5.0,0.0,10,'Clear','Ceu limpo, UV muito alto',SYSTIMESTAMP)");

            // Guarulhos — 2 registros (Storm, Cloudy)
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Guarulhos',-23.4538,-46.5333,18.5,17.0,90,15.0,28.0,1,'Storm','Tempestade com ventos fortes',SYSTIMESTAMP)");
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Guarulhos',-23.4538,-46.5333,25.0,24.0,70,6.0,0.0,7,'Cloudy','Nublado com possibilidade de chuva',SYSTIMESTAMP)");

            // Osasco — 2 registros (Storm com risco alagamento, Rain)
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Osasco',-23.5322,-46.7919,20.0,19.5,88,10.0,42.0,0,'Storm','Chuva intensa com risco de alagamento',SYSTIMESTAMP)");
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Osasco',-23.5322,-46.7919,17.0,15.5,92,18.0,15.0,0,'Rain','Chuva moderada com vento',SYSTIMESTAMP)");

            // Santo Andre — 2 registros (Rain leve, Clear UV extremo)
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Santo Andre',-23.6639,-46.5383,22.0,21.0,75,7.0,8.0,5,'Rain','Garoa e ceu encoberto',SYSTIMESTAMP)");
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Santo Andre',-23.6639,-46.5383,30.0,33.0,55,4.0,0.0,11,'Clear','Sol forte, indice UV extremo',SYSTIMESTAMP)");

            // Campinas — 2 registros (Cloudy, Storm com granizo)
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Campinas',-22.9068,-47.0626,23.5,22.0,68,9.0,0.0,8,'Cloudy','Parcialmente nublado',SYSTIMESTAMP)");
            migrationBuilder.Sql(@"INSERT INTO ""WEATHER_RECORDS"" (""CITY"",""LATITUDE"",""LONGITUDE"",""TEMPERATURE"",""FEELS_LIKE"",""HUMIDITY"",""WIND_SPEED"",""RAIN_MM"",""UV_INDEX"",""CONDITION"",""DESCRIPTION"",""RECORDED_AT"") VALUES ('Campinas',-22.9068,-47.0626,16.0,14.0,97,20.0,55.0,0,'Storm','Tempestade severa com granizo',SYSTIMESTAMP)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM ""WEATHER_RECORDS"" WHERE ""CITY"" IN ('Sao Paulo','Guarulhos','Osasco','Santo Andre','Campinas')");
        }
    }
}

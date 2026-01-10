/*
=============================================================================
AREA52 BIKE RESERVATION REPOSITORY - MySQL DATABASE IMPLEMENTATIE
=============================================================================
Dit bestand implementeert de IBikeReservationRepository interface met MySQL.

Fontys HBO-ICT Bachelor Niveau Documentatie:

REPOSITORY PATTERN:
- Abstractielaag tussen business logic en data access
- Centraliseert alle database operaties voor BikeReservation entiteit
- Loose coupling met de database implementatie

JSON SERIALISATIE:
- Options lijst wordt opgeslagen als JSON string in database
- System.Text.Json voor serialisatie/deserialisatie
- Flexibel: kan verschillende IRentalOption types opslaan

DAPPER USAGE:
- Query<T>() met anonymous type voor type-safe queries
- Execute() voor non-query operaties
- Parameters voorkomen SQL injection aanvallen

TRANSACTIES:
- In productie zou je BeginTransaction() gebruiken voor atomiciteit
- Hier gehouden als simpele implementatie voor leesbaarheid
=============================================================================
*/

using Area52.Domain.Interfaces;
using Area52.Domain.Models;
using Area52.Domain.Options;
using Area52.Data.Data;
using Dapper;
using System.Text.Json;

namespace Area52.Data.Repositories;

/// <summary>
/// Repository class voor BikeReservation entiteiten met MySQL database.
/// Implementeert CRUD operaties voor reserveringen.
/// </summary>
public class BikeReservationRepository : IBikeReservationRepository
{
    // DatabaseContext voor database connecties
    private readonly DatabaseContext _db;

    /// <summary>
    /// Constructor met Dependency Injection van DatabaseContext.
    /// </summary>
    /// <param name="db">DatabaseContext instantie</param>
    public BikeReservationRepository(DatabaseContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Haalt alle reserveringen op uit de database.
    /// </summary>
    /// <returns>IEnumerable van BikeReservation objecten</returns>
    public IEnumerable<BikeReservation> GetAll()
    {
        using var connection = _db.CreateConnection();
        
        // SQL query voor alle reserveringen
        const string sql = @"
            SELECT ReservationId, BikeId, StartDate, Days, BikeCount, OptionsJson, TotalPrice 
            FROM BikeReservations 
            ORDER BY StartDate DESC";
        
        var results = connection.Query<ReservationDto>(sql);
        
        // Map DTO naar domain model met JSON deserialisatie
        return results.Select(MapToReservation).ToList();
    }

    /// <summary>
    /// Haalt een specifieke reservering op basis van ID.
    /// </summary>
    /// <param name="id">ReservationId om te zoeken</param>
    /// <returns>BikeReservation of null als niet gevonden</returns>
    public BikeReservation? GetById(int id)
    {
        using var connection = _db.CreateConnection();
        
        const string sql = @"
            SELECT ReservationId, BikeId, StartDate, Days, BikeCount, OptionsJson, TotalPrice 
            FROM BikeReservations 
            WHERE ReservationId = @Id";
        
        var result = connection.QueryFirstOrDefault<ReservationDto>(sql, new { Id = id });
        
        return result == null ? null : MapToReservation(result);
    }

    /// <summary>
    /// Voegt een nieuwe reservering toe aan de database.
    /// Serialiseert de Options lijst naar JSON voor opslag.
    /// </summary>
    /// <param name="reservation">BikeReservation om toe te voegen</param>
    public void Add(BikeReservation reservation)
    {
        if (reservation == null) throw new ArgumentNullException(nameof(reservation));

        using var connection = _db.CreateConnection();
        
        // Serialiseer Options naar JSON string
        var optionsJson = SerializeOptions(reservation.Options);
        
        // INSERT met LAST_INSERT_ID() voor MySQL
        const string sql = @"
            INSERT INTO BikeReservations (BikeId, StartDate, Days, BikeCount, OptionsJson, TotalPrice) 
            VALUES (@BikeId, @StartDate, @Days, @BikeCount, @OptionsJson, @TotalPrice);
            SELECT LAST_INSERT_ID();";
        
        var newId = connection.ExecuteScalar<int>(sql, new
        {
            reservation.BikeId,
            reservation.StartDate,
            reservation.Days,
            reservation.BikeCount,
            OptionsJson = optionsJson,
            reservation.TotalPrice
        });
        
        reservation.ReservationId = newId;
    }

    /// <summary>
    /// Verwijdert een reservering uit de database.
    /// </summary>
    /// <param name="id">ReservationId van te verwijderen reservering</param>
    public void Delete(int id)
    {
        using var connection = _db.CreateConnection();
        
        const string sql = "DELETE FROM BikeReservations WHERE ReservationId = @Id";
        
        connection.Execute(sql, new { Id = id });
    }

    /// <summary>
    /// Serialiseert de Options lijst naar JSON string.
    /// Slaat type informatie op voor correcte deserialisatie.
    /// </summary>
    private static string SerializeOptions(List<IRentalOption> options)
    {
        if (options == null || options.Count == 0)
            return "[]";

        // Maak een lijst van serialiseerbare objecten met type info
        var serializableOptions = options.Select(opt => new
        {
            Type = opt.GetType().Name,
            opt.Name
        }).ToList();

        return JsonSerializer.Serialize(serializableOptions);
    }

    /// <summary>
    /// Deserialiseert JSON string naar Options lijst.
    /// Herstelt de concrete IRentalOption types.
    /// </summary>
    private static List<IRentalOption> DeserializeOptions(string? json)
    {
        if (string.IsNullOrEmpty(json) || json == "[]")
            return new List<IRentalOption>();

        try
        {
            // Parse JSON naar array van objecten
            using var doc = JsonDocument.Parse(json);
            var options = new List<IRentalOption>();

            foreach (var element in doc.RootElement.EnumerateArray())
            {
                var typeName = element.GetProperty("Type").GetString();
                
                // Factory pattern: maak juiste type aan op basis van Type property
                IRentalOption? option = typeName switch
                {
                    "AssistanceOption" => new AssistanceOption(),
                    "DamageInsuranceOption" => new DamageInsuranceOption(),
                    _ => null
                };

                if (option != null)
                    options.Add(option);
            }

            return options;
        }
        catch
        {
            // Bij parse errors, return lege lijst
            return new List<IRentalOption>();
        }
    }

    /// <summary>
    /// Mapt een DTO naar BikeReservation domain model.
    /// </summary>
    private static BikeReservation MapToReservation(ReservationDto dto)
    {
        return new BikeReservation
        {
            ReservationId = dto.ReservationId,
            BikeId = dto.BikeId,
            StartDate = dto.StartDate,
            Days = dto.Days,
            BikeCount = dto.BikeCount,
            Options = DeserializeOptions(dto.OptionsJson),
            TotalPrice = dto.TotalPrice
        };
    }

    /// <summary>
    /// Data Transfer Object voor database resultaten.
    /// Dapper mapt direct naar deze class.
    /// </summary>
    private class ReservationDto
    {
        public int ReservationId { get; set; }
        public int BikeId { get; set; }
        public DateTime StartDate { get; set; }
        public int Days { get; set; }
        public int BikeCount { get; set; }
        public string? OptionsJson { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

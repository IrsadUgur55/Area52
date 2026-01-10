/*
=============================================================================
AREA52 BIKE REPOSITORY - MySQL DATABASE IMPLEMENTATIE
=============================================================================
Dit bestand implementeert de IBikeRepository interface met MySQL database.

Fontys HBO-ICT Bachelor Niveau Documentatie:

REPOSITORY PATTERN:
- Abstractielaag tussen business logic en data access
- Centraliseert alle database operaties voor Bike entiteit
- Maakt unit testing mogelijk door interface te mocken

DAPPER MICRO-ORM:
- Lightweight Object-Relational Mapper
- Mapt SQL query resultaten direct naar C# objecten
- Betere performance dan Entity Framework voor simpele queries
- Query<T>() voor SELECT statements
- Execute() voor INSERT/UPDATE/DELETE statements

INHERITANCE MAPPING:
- Bike is base class, CityBike en ElectricBike zijn subclasses
- BikeType kolom bepaalt welk type object wordt aangemaakt
- Factory pattern in MapToBike() methode

CRUD OPERATIES:
- Create: INSERT INTO met OUTPUT voor nieuwe ID
- Read: SELECT met WHERE clause
- Update: UPDATE met SET en WHERE
- Delete: DELETE met WHERE clause
=============================================================================
*/

using Area52.Domain.Interfaces;
using Area52.Domain.Models;
using Area52.Data.Data;
using Dapper;

namespace Area52.Data.Repositories;

/// <summary>
/// Repository class voor Bike entiteiten met MySQL database ondersteuning.
/// Implementeert het Repository Pattern voor data access abstractie.
/// </summary>
public class BikeRepository : IBikeRepository
{
    // DatabaseContext voor het aanmaken van database connecties
    private readonly DatabaseContext _db;

    /// <summary>
    /// Constructor met Dependency Injection van DatabaseContext.
    /// </summary>
    /// <param name="db">DatabaseContext instantie voor database connecties</param>
    public BikeRepository(DatabaseContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Haalt alle fietsen op uit de database.
    /// </summary>
    /// <returns>IEnumerable van Bike objecten (CityBike of ElectricBike)</returns>
    public IEnumerable<Bike> GetAll()
    {
        using var connection = _db.CreateConnection();
        
        // SQL query om alle fietsen op te halen
        const string sql = @"
            SELECT BikeId, BikeType, DayPrice, PurchaseDate, BatteryCapacityWh 
            FROM Bikes 
            ORDER BY BikeId";
        
        // Dapper Query retourneert dynamic objecten die we mappen naar Bike types
        var results = connection.Query<dynamic>(sql);
        
        // Map elk database record naar het juiste Bike type (CityBike of ElectricBike)
        return results.Select(MapToBike).ToList();
    }

    /// <summary>
    /// Haalt een specifieke fiets op basis van ID.
    /// </summary>
    /// <param name="id">BikeId om te zoeken</param>
    /// <returns>Bike object of null als niet gevonden</returns>
    public Bike? GetById(int id)
    {
        using var connection = _db.CreateConnection();
        
        // SQL query met parameter om SQL injection te voorkomen
        const string sql = @"
            SELECT BikeId, BikeType, DayPrice, PurchaseDate, BatteryCapacityWh 
            FROM Bikes 
            WHERE BikeId = @Id";
        
        // QueryFirstOrDefault retourneert null als geen resultaat
        var result = connection.QueryFirstOrDefault<dynamic>(sql, new { Id = id });
        
        return result == null ? null : MapToBike(result);
    }

    /// <summary>
    /// Voegt een nieuwe fiets toe aan de database.
    /// </summary>
    /// <param name="bike">Bike object om toe te voegen</param>
    public void Add(Bike bike)
    {
        using var connection = _db.CreateConnection();
        
        // BUGFIX: Gebruik bike.Type property i.p.v. C# type checking
        // Het formulier stuurt Type als string ("CityBike" of "ElectricBike")
        // ASP.NET Model Binding maakt altijd een Bike object, geen subclass
        var bikeType = bike.Type switch
        {
            "ElectricBike" => "ElectricBike",
            "CityBike" => "CityBike",
            _ => "CityBike"  // Default naar CityBike als Type leeg is
        };
        
        // BatteryCapacity: check of het een ElectricBike is (via Type property)
        int? batteryCapacity = bike.Type == "ElectricBike" && bike is ElectricBike eb 
            ? eb.BatteryCapacityWh 
            : null;
        
        // INSERT query met LAST_INSERT_ID() voor MySQL om nieuwe ID te krijgen
        const string sql = @"
            INSERT INTO Bikes (BikeType, DayPrice, PurchaseDate, BatteryCapacityWh) 
            VALUES (@BikeType, @DayPrice, @PurchaseDate, @BatteryCapacity);
            SELECT LAST_INSERT_ID();";
        
        // ExecuteScalar retourneert de nieuwe ID
        var newId = connection.ExecuteScalar<int>(sql, new
        {
            BikeType = bikeType,
            bike.DayPrice,
            bike.PurchaseDate,
            BatteryCapacity = batteryCapacity
        });
        
        // Zet de nieuwe ID op het bike object
        bike.BikeId = newId;
    }

    /// <summary>
    /// Werkt een bestaande fiets bij in de database.
    /// </summary>
    /// <param name="bike">Bike object met bijgewerkte gegevens</param>
    public void Update(Bike bike)
    {
        using var connection = _db.CreateConnection();
        
        // BUGFIX: Gebruik bike.Type property i.p.v. C# type checking
        var bikeType = bike.Type switch
        {
            "ElectricBike" => "ElectricBike",
            "CityBike" => "CityBike",
            _ => "CityBike"
        };
        
        // BatteryCapacity: check via Type property string
        int? batteryCapacity = bike.Type == "ElectricBike" && bike is ElectricBike eb 
            ? eb.BatteryCapacityWh 
            : null;
        
        // UPDATE query met WHERE clause voor specifieke fiets
        const string sql = @"
            UPDATE Bikes 
            SET BikeType = @BikeType, 
                DayPrice = @DayPrice, 
                PurchaseDate = @PurchaseDate, 
                BatteryCapacityWh = @BatteryCapacity 
            WHERE BikeId = @BikeId";
        
        connection.Execute(sql, new
        {
            bike.BikeId,
            BikeType = bikeType,
            bike.DayPrice,
            bike.PurchaseDate,
            BatteryCapacity = batteryCapacity
        });
    }

    /// <summary>
    /// Verwijdert een fiets uit de database.
    /// </summary>
    /// <param name="id">BikeId van de te verwijderen fiets</param>
    public void Delete(int id)
    {
        using var connection = _db.CreateConnection();
        
        // DELETE query met parameter
        const string sql = "DELETE FROM Bikes WHERE BikeId = @Id";
        
        connection.Execute(sql, new { Id = id });
    }

    /// <summary>
    /// Factory methode die database records mapt naar het juiste Bike type.
    /// Gebruikt het BikeType veld om te bepalen of het een CityBike of ElectricBike is.
    /// </summary>
    /// <param name="row">Dynamic object met database velden</param>
    /// <returns>CityBike of ElectricBike instantie</returns>
    private static Bike MapToBike(dynamic row)
    {
        // Cast naar string voor type checking
        string bikeType = row.BikeType?.ToString() ?? "CityBike";
        
        // Factory pattern: maak het juiste type aan op basis van BikeType
        if (bikeType == "ElectricBike")
        {
            return new ElectricBike
            {
                BikeId = (int)row.BikeId,
                DayPrice = (decimal)row.DayPrice,
                PurchaseDate = (DateTime)row.PurchaseDate,
                BatteryCapacityWh = row.BatteryCapacityWh != null ? (int)row.BatteryCapacityWh : 0
            };
        }
        
        // Default: CityBike
        return new CityBike
        {
            BikeId = (int)row.BikeId,
            DayPrice = (decimal)row.DayPrice,
            PurchaseDate = (DateTime)row.PurchaseDate
        };
    }
}

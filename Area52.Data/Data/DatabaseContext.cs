/*
=============================================================================
AREA52 DATABASE CONTEXT - MySQL DATABASE CONNECTIE
=============================================================================
Dit bestand beheert de database connectie voor de Area52 applicatie.

Fontys HBO-ICT Bachelor Niveau Documentatie:

DESIGN PATTERN: Factory Pattern
- CreateConnection() is een factory method die database connecties aanmaakt
- Centraliseert connectie-logica op één plek
- Maakt het eenvoudig om database provider te wijzigen

DEPENDENCY INJECTION:
- IConfiguration wordt geïnjecteerd via constructor
- Connection string komt uit appsettings.json
- Loose coupling met configuratie systeem

MySQL CONNECTIE:
- Gebruikt MySqlConnector library (high-performance .NET MySQL driver)
- Ondersteunt connection pooling voor betere performance
- Async-ready voor schaalbare applicaties

INTERFACE ABSTRACTIE:
- Retourneert IDbConnection interface, niet concrete MySqlConnection
- Hierdoor kunnen repositories werken met elke database provider
- Faciliteert unit testing met mock connections
=============================================================================
*/

using System.Data;
using MySqlConnector;
using Microsoft.Extensions.Configuration;

namespace Area52.Data.Data;

/// <summary>
/// Database context class die verantwoordelijk is voor het aanmaken van
/// MySQL database connecties. Volgt het Factory Pattern voor het creëren
/// van database connectie objecten.
/// </summary>
public class DatabaseContext
{
    // Private field voor de connection string 
    private readonly string _connectionString;

    public DatabaseContext(IConfiguration configuration)
    {
        // Null-coalescing throw operator: gooi exception als connection string null is
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' niet gevonden in appsettings.json.");
    }
    public IDbConnection CreateConnection()
    {
        // MySqlConnection implementeert IDbConnection interface
        return new MySqlConnection(_connectionString);
    }
}

## 📋 Inhoudsopgave

1. [Overzicht Wijzigingen](#overzicht-wijzigingen)
2. [Database Configuratie](#database-configuratie)
3. [Gewijzigde Bestanden](#gewijzigde-bestanden)
4. [Installatie & Setup](#installatie--setup)
5. [Technische Documentatie](#technische-documentatie)

---

## 🔄 Overzicht Wijzigingen

Deze branch (`frontend-anpassungen`) bevat de volgende belangrijke wijzigingen:

| Categorie | Wijziging | Reden |
|-----------|-----------|-------|
| **Database** | SQL Server → MySQL | Hosting op eigen server (81.173.3.59) |
| **ORM** | Geen → Dapper | Lightweight micro-ORM voor database queries |
| **Framework** | .NET 8.0 LTS | Compatibiliteit met Railway cloud hosting |
| **Frontend** | Bootstrap basis → Modern design | Professionele uitstraling passend bij logo |
| **Talen** | Engels → NL/DE/EN | Meertalige ondersteuning met JavaScript |
| **Cloud Hosting** | Geen → Railway | Productie deployment met Docker |
| **Documentatie** | Minimaal → Uitgebreid | Nederlandse comments voor HBO-ICT studenten |

---

## 🎨 Frontend Modernisatie

### CSS Variabelen (wwwroot/css/site.css)

De frontend is volledig gemoderniseerd met CSS custom properties (variabelen). Dit maakt het thema eenvoudig aan te passen:

```css
:root {
    /* Primaire kleuren - Gebaseerd op Area52 logo */
    --primary-color: #3498db;        /* Blauw - hoofdkleur */
    --primary-hover: #2980b9;        /* Donkerder blauw voor hover states */
    --secondary-color: #2c3e50;      /* Donker blauw-grijs - tekst/headers */
    
    /* Achtergronden */
    --background-color: #f8f9fa;     /* Lichtgrijs - pagina achtergrond */
    --card-background: #ffffff;      /* Wit - kaarten en formulieren */
    
    /* Feedback kleuren */
    --success-color: #27ae60;        /* Groen - bevestigingen */
    --warning-color: #f39c12;        /* Oranje - waarschuwingen */
    --danger-color: #e74c3c;         /* Rood - fouten/verwijderen */
}
```

**Waarom CSS variabelen?**
- Eén plek om kleuren aan te passen
- Consistent kleurgebruik door hele applicatie
- Makkelijk te begrijpen voor medestudenten
- Ondersteund door alle moderne browsers

### Moderne UI Componenten

De interface gebruikt nu:
- **Gradient header** met logo en taalwisselaar
- **Card-based layout** voor overzichtelijke content
- **Hover effecten** op knoppen en kaarten
- **Schaduweffecten** (box-shadow) voor diepte
- **Responsive design** voor mobiele apparaten

```css
/* Voorbeeld: Moderne kaart styling */
.card {
    background: var(--card-background);
    border-radius: 12px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
    transition: transform 0.2s ease, box-shadow 0.2s ease;
}

.card:hover {
    transform: translateY(-4px);          /* Kaart "zweeft" omhoog */
    box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
}
```

---

## 🌍 Meertalige Ondersteuning (i18n)

### Architectuur

De applicatie ondersteunt drie talen: Nederlands (NL), Duits (DE), en Engels (EN).

```
┌─────────────────────────────────────────────────────┐
│                    Browser                          │
│  ┌──────────┐   ┌──────────┐   ┌──────────┐       │
│  │    NL    │   │    DE    │   │    EN    │       │
│  └────┬─────┘   └────┬─────┘   └────┬─────┘       │
│       │              │              │              │
│       └──────────────┼──────────────┘              │
│                      ▼                             │
│            localStorage['lang']                    │
│                      │                             │
│                      ▼                             │
│             translations.js                        │
│        (alle vertalingen object)                   │
│                      │                             │
│                      ▼                             │
│     document.querySelectorAll('[data-i18n]')      │
│       (elementen met vertaalsleutel)               │
└─────────────────────────────────────────────────────┘
```

### translations.js Structuur

```javascript
// wwwroot/js/translations.js

const translations = {
    nl: {
        'nav.home': 'Home',
        'nav.bikes': 'Fietsen',
        'nav.reservations': 'Reserveringen',
        'bikes.title': 'Fietsen Overzicht',
        'bikes.create': 'Nieuwe Fiets',
        // ... meer vertalingen
    },
    de: {
        'nav.home': 'Startseite',
        'nav.bikes': 'Fahrräder',
        // ... Duitse vertalingen
    },
    en: {
        'nav.home': 'Home',
        'nav.bikes': 'Bikes',
        // ... Engelse vertalingen
    }
};
```

### HTML Implementatie

In de Razor Views worden `data-i18n` attributen gebruikt:

```html
<!-- De tekst wordt vervangen door JavaScript -->
<a href="/Bike" data-i18n="nav.bikes">Fietsen</a>

<!-- Voor placeholders -->
<input data-i18n-placeholder="search.placeholder" placeholder="Zoeken...">

<!-- Taalwisselaar in header -->
<div class="language-switcher">
    <button onclick="setLanguage('nl')" class="lang-btn active">NL</button>
    <button onclick="setLanguage('de')" class="lang-btn">DE</button>
    <button onclick="setLanguage('en')" class="lang-btn">EN</button>
</div>
```

### JavaScript Functionaliteit

```javascript
// Huidige taal ophalen (default: Nederlands)
function getCurrentLanguage() {
    return localStorage.getItem('lang') || 'nl';
}

// Taal instellen en pagina updaten
function setLanguage(lang) {
    localStorage.setItem('lang', lang);
    updatePageTranslations();
    updateLanguageButtons();
}

// Alle elementen met data-i18n vertalen
function updatePageTranslations() {
    const lang = getCurrentLanguage();
    document.querySelectorAll('[data-i18n]').forEach(element => {
        const key = element.getAttribute('data-i18n');
        if (translations[lang] && translations[lang][key]) {
            element.textContent = translations[lang][key];
        }
    });
}
```

**Voordelen van deze aanpak:**
- Geen server reload nodig bij taalwisseling
- Taal blijft behouden (localStorage)
- Eenvoudig nieuwe vertalingen toevoegen
- Client-side = snelle performance

---

## 🗄️ Database Configuratie

### Connection String (appsettings.json)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=<SERVER_IP>;Port=3306;Database=area52;User=<USERNAME>;Password=<PASSWORD>;"
  }
}
```

⚠️ **BELANGRIJK:** Vraag de connection string credentials aan je teamlead. Zet NOOIT wachtwoorden in git!

### Database Schema

De database bevat twee tabellen:

```sql
-- Bikes tabel
CREATE TABLE Bikes (
    BikeId INT AUTO_INCREMENT PRIMARY KEY,
    BikeType VARCHAR(50) NOT NULL,      -- 'CityBike' of 'ElectricBike'
    DayPrice DECIMAL(10, 2) NOT NULL,
    PurchaseDate DATE NOT NULL,
    BatteryCapacityWh INT NULL          -- Alleen voor ElectricBike
);

-- BikeReservations tabel
CREATE TABLE BikeReservations (
    ReservationId INT AUTO_INCREMENT PRIMARY KEY,
    BikeId INT NOT NULL,
    StartDate DATE NOT NULL,
    Days INT NOT NULL DEFAULT 1,
    BikeCount INT NOT NULL DEFAULT 1,
    OptionsJson TEXT NULL,               -- JSON met geselecteerde opties
    TotalPrice DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId)
);
```

---

## 📁 Gewijzigde Bestanden

### 1. Area52.Data/Area52.Data.csproj

**Wat is gewijzigd:**
- `Microsoft.Data.SqlClient` vervangen door `MySqlConnector`
- `Dapper` package toegevoegd

**Waarom:**
- MySqlConnector is de officiële .NET driver voor MySQL databases
- Dapper is een micro-ORM die SQL queries eenvoudig mapt naar C# objecten

```xml
<ItemGroup>
    <PackageReference Include="MySqlConnector" Version="2.3.7" />
    <PackageReference Include="Dapper" Version="2.1.35" />
</ItemGroup>
```

---

### 2. Area52.Data/Data/DatabaseContext.cs

**Wat is gewijzigd:**
- `SqlConnection` vervangen door `MySqlConnection`
- Using statement aangepast

**Waarom:**
- MySqlConnection is de MySQL-specifieke implementatie van IDbConnection
- Dezelfde interface, andere database provider

```csharp
// OUD (SQL Server)
using Microsoft.Data.SqlClient;
return new SqlConnection(_connectionString);

// NIEUW (MySQL)
using MySqlConnector;
return new MySqlConnection(_connectionString);
```

---

### 3. Area52.Data/Repositories/BikeRepository.cs

**Wat is gewijzigd:**
- In-memory `List<Bike>` vervangen door echte database queries
- Dapper gebruikt voor alle CRUD operaties
- Factory pattern toegevoegd voor Bike type mapping

**Waarom:**
- Data moet persistent worden opgeslagen in MySQL
- Dapper maakt SQL queries type-safe en eenvoudig

**Belangrijke methodes:**

```csharp
// GetAll() - Haalt alle fietsen op met Dapper
public IEnumerable<Bike> GetAll()
{
    using var connection = _db.CreateConnection();
    const string sql = "SELECT BikeId, BikeType, DayPrice, PurchaseDate, BatteryCapacityWh FROM Bikes";
    var results = connection.Query<dynamic>(sql);
    return results.Select(MapToBike).ToList();
}

// MapToBike() - Factory pattern voor CityBike/ElectricBike
private static Bike MapToBike(dynamic row)
{
    string bikeType = row.BikeType?.ToString() ?? "CityBike";
    
    if (bikeType == "ElectricBike")
        return new ElectricBike { /* properties */ };
    
    return new CityBike { /* properties */ };
}

// Add() - INSERT met LAST_INSERT_ID() voor MySQL
public void Add(Bike bike)
{
    const string sql = @"
        INSERT INTO Bikes (BikeType, DayPrice, PurchaseDate, BatteryCapacityWh) 
        VALUES (@BikeType, @DayPrice, @PurchaseDate, @BatteryCapacity);
        SELECT LAST_INSERT_ID();";
    
    bike.BikeId = connection.ExecuteScalar<int>(sql, parameters);
}
```

**Let op:** `LAST_INSERT_ID()` is MySQL-specifiek. Voor SQL Server zou je `SCOPE_IDENTITY()` of `OUTPUT INSERTED.Id` gebruiken.

---

### 4. Area52.Data/Repositories/BikeReservationRepository.cs

**Wat is gewijzigd:**
- In-memory `List<BikeReservation>` vervangen door database queries
- JSON serialisatie toegevoegd voor Options lijst
- DTO class toegevoegd voor database mapping

**Waarom:**
- Reserveringen moeten persistent worden opgeslagen
- Options (IRentalOption) kunnen niet direct in een kolom → JSON serialisatie

**JSON Serialisatie:**

```csharp
// Opslaan: List<IRentalOption> → JSON string
private static string SerializeOptions(List<IRentalOption> options)
{
    var serializableOptions = options.Select(opt => new
    {
        Type = opt.GetType().Name,  // "AssistanceOption" of "DamageInsuranceOption"
        opt.Name
    }).ToList();
    return JsonSerializer.Serialize(serializableOptions);
}

// Laden: JSON string → List<IRentalOption>
private static List<IRentalOption> DeserializeOptions(string? json)
{
    // Parse JSON en maak juiste type aan op basis van "Type" property
    IRentalOption? option = typeName switch
    {
        "AssistanceOption" => new AssistanceOption(),
        "DamageInsuranceOption" => new DamageInsuranceOption(),
        _ => null
    };
}
```

**DTO Pattern:**

```csharp
// Data Transfer Object voor database resultaten
private class ReservationDto
{
    public int ReservationId { get; set; }
    public int BikeId { get; set; }
    public DateTime StartDate { get; set; }
    public int Days { get; set; }
    public int BikeCount { get; set; }
    public string? OptionsJson { get; set; }  // JSON in database
    public decimal TotalPrice { get; set; }
}
```

---

### 5. Area52.Web/appsettings.json

**Wat is gewijzigd:**
- SQL Server connection string → MySQL connection string

**Waarom:**
- Andere database server en formaat

```json
// OUD (SQL Server)
"Server=localhost;Database=Area52Db;User Id=sa;Password=<PASSWORD>;TrustServerCertificate=True;"

// NIEUW (MySQL)
"Server=<SERVER_IP>;Port=3306;Database=area52;User=<USERNAME>;Password=<PASSWORD>;"
```

⚠️ **Credentials staan NIET in git** - gebruik environment variables of vraag aan teamlead.

---

### 6. Area52.Web/Program.cs (NIEUW BESTAND)

**Waarom toegevoegd:**
- Originele repository miste dit bestand
- Nodig voor Dependency Injection configuratie

**Wat doet het:**

```csharp
// Dependency Injection registraties
builder.Services.AddSingleton<DatabaseContext>();           // Database connectie factory
builder.Services.AddScoped<IBikeRepository, BikeRepository>();
builder.Services.AddScoped<IBikeReservationRepository, BikeReservationRepository>();
builder.Services.AddScoped<IBikePricingService, BikePricingService>();
```

**DI Lifetimes uitgelegd:**
- `Singleton`: Één instantie voor hele applicatie (DatabaseContext)
- `Scoped`: Nieuwe instantie per HTTP request (Repositories)

---

## 🚀 Installatie & Setup

### Vereisten
- .NET 8.0 SDK (LTS versie)
- MySQL Server (of toegang tot de gedeelde server)
- Git voor versiebeheer

### Lokale Ontwikkeling

1. **Clone repository:**
   ```bash
   git clone https://github.com/AlferNL/Area52-fork.git
   cd Area52-fork
   git checkout frontend-anpassungen
   ```

2. **appsettings.json aanmaken:**
   ```bash
   # Kopieer het template bestand
   cp Area52.Web/appsettings.template.json Area52.Web/appsettings.json
   
   # Open en vul de juiste credentials in (vraag aan teamlead!)
   ```

3. **Database aanmaken (indien nodig):**
   ```bash
   # Voer Database/create_tables.sql uit op je MySQL server
   # Dit maakt de tabellen aan en voegt testdata toe
   ```

4. **Applicatie starten:**
   ```bash
   cd Area52.Web
   dotnet run
   ```

5. **Open browser:**
   http://localhost:5287

---

## ☁️ Railway Cloud Deployment

### Wat is Railway?

Railway is een Platform-as-a-Service (PaaS) waarmee je eenvoudig web applicaties kunt deployen. Vergelijkbaar met Heroku, maar moderner en met gratis tier.

### Deployment Architectuur

```
┌────────────────────────────────────────────────────────────────┐
│                         Railway Platform                        │
│  ┌─────────────────┐                    ┌───────────────────┐  │
│  │   GitHub Repo   │───── trigger ─────▶│   Build Service   │  │
│  │  (AlferNL/      │                    │   (Dockerfile)    │  │
│  │   Area52-fork)  │                    └─────────┬─────────┘  │
│  └─────────────────┘                              │             │
│                                                   ▼             │
│                                          ┌───────────────────┐  │
│                                          │  Docker Container │  │
│                                          │  (.NET 8.0 app)   │  │
│                                          └─────────┬─────────┘  │
│                                                    │            │
│                                                    ▼            │
│                                          ┌───────────────────┐  │
│                                          │  Public URL       │  │
│                                          │  *.railway.app    │  │
│                                          └───────────────────┘  │
└────────────────────────────────────────────────────────────────┘
                              │
                              │ MySQL connectie
                              ▼
                    ┌───────────────────┐
                    │  Externe MySQL    │
                    │  81.173.3.59      │
                    │  (eigen server)   │
                    └───────────────────┘
```

### Dockerfile Uitleg

```dockerfile
# STAGE 1: Build
# Gebruikt de .NET SDK image om de applicatie te compileren
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Eerst alleen de .csproj bestanden kopiëren
# Dit maakt gebruik van Docker layer caching
COPY ["Area52.Web/Area52.Web.csproj", "Area52.Web/"]
COPY ["Area52.Data/Area52.Data.csproj", "Area52.Data/"]
COPY ["Area52.Domain/Area52.Domain.csproj", "Area52.Domain/"]

# NuGet packages herstellen (gecached als .csproj niet wijzigt)
RUN dotnet restore "Area52.Web/Area52.Web.csproj"

# Nu alle broncode kopiëren en bouwen
COPY . .
RUN dotnet build "Area52.Web/Area52.Web.csproj" -c Release -o /app/build
RUN dotnet publish "Area52.Web/Area52.Web.csproj" -c Release -o /app/publish

# STAGE 2: Runtime
# Kleinere image, alleen runtime, geen SDK nodig
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Gecompileerde applicatie kopiëren van build stage
COPY --from=build /app/publish .

# Railway injecteert PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

# Start de applicatie
ENTRYPOINT ["dotnet", "Area52.Web.dll"]
```

**Multi-stage build voordelen:**
- Kleinere finale image (alleen runtime, ~200MB vs ~700MB)
- Veiligere image (geen SDK tools die misbruikt kunnen worden)
- Snellere deploys door layer caching

### Environment Variables in Railway

In het Railway dashboard moet je de volgende environment variable instellen:

| Variable | Waarde | Beschrijving |
|----------|--------|--------------|
| `ConnectionStrings__DefaultConnection` | `Server=81.173.3.59;Port=3306;Database=area52;User=admin2;Password=***;` | MySQL connectie |

**Let op de dubbele underscore!** Dit is hoe .NET environment variables mapt naar `appsettings.json`:

```
ConnectionStrings__DefaultConnection
        ↓ wordt ↓
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  }
}
```

### Deployment Stappen

1. **Account aanmaken** op [railway.app](https://railway.app)
2. **Nieuw project** → "Deploy from GitHub"
3. **Repository selecteren**: AlferNL/Area52-fork
4. **Branch kiezen**: frontend-anpassungen
5. **Builder instellen**: Dockerfile (niet Nixpacks)
6. **Environment variable toevoegen** (zie tabel hierboven)
7. **Generate Domain** in Settings → Networking
8. **Wachten op build** (~2-3 minuten)

---

## 📚 Technische Documentatie

### Project Structuur

```
Area52/
├── Area52.sln                    # Solution bestand (Visual Studio/Rider)
├── Dockerfile                    # Docker configuratie voor Railway
├── railway.json                  # Railway deployment configuratie
├── README.md                     # Dit bestand
│
├── Area52.Domain/                # 📦 Domain Layer (business entiteiten)
│   ├── Models/
│   │   ├── Bike.cs              # Abstracte basis klasse
│   │   ├── CityBike.cs          # Stadsfiets implementatie
│   │   ├── ElectricBike.cs      # E-bike met batterij
│   │   └── BikeReservation.cs   # Reservering model
│   ├── Interfaces/
│   │   ├── IBikeRepository.cs   # Repository interface
│   │   └── IBikeReservationRepository.cs
│   ├── Options/                  # Huuroptie systeem
│   │   ├── IRentalOptions.cs    # Interface voor opties
│   │   ├── AssistanceOption.cs  # Pechhulp optie
│   │   └── DamageInsuranceOption.cs
│   └── Services/
│       ├── IBikePricingService.cs
│       └── BikePricingService.cs # Prijsberekening logica
│
├── Area52.Data/                  # 💾 Data Layer (database toegang)
│   ├── Data/
│   │   └── DatabaseContext.cs   # Database connectie factory
│   └── Repositories/
│       ├── BikeRepository.cs    # CRUD voor fietsen
│       └── BikeReservationRepository.cs
│
├── Area52.Web/                   # 🌐 Presentation Layer (UI)
│   ├── Program.cs               # Applicatie entry point + DI
│   ├── appsettings.json         # Configuratie (NIET in git!)
│   ├── Controllers/
│   │   ├── HomeController.cs    # Homepage
│   │   ├── BikeController.cs    # Fiets CRUD
│   │   └── ReservationController.cs
│   ├── Views/                   # Razor views (.cshtml)
│   │   ├── Shared/_Layout.cshtml # Hoofdtemplate
│   │   ├── Home/
│   │   ├── Bike/
│   │   └── Reservation/
│   └── wwwroot/                 # Statische bestanden
│       ├── css/site.css         # Aangepaste styling
│       └── js/translations.js   # Meertalige ondersteuning
│
└── Database/
    └── create_tables.sql        # Database setup script
```

### Layered Architecture (N-tier)

```
┌─────────────────────────────────────────────────────────────┐
│                    PRESENTATION LAYER                        │
│                      (Area52.Web)                           │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐ │
│  │ Controllers │  │   Views     │  │   wwwroot (CSS/JS)  │ │
│  └──────┬──────┘  └─────────────┘  └─────────────────────┘ │
│         │                                                   │
│         │ Dependency Injection                              │
│         ▼                                                   │
├─────────────────────────────────────────────────────────────┤
│                     DOMAIN LAYER                            │
│                    (Area52.Domain)                          │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────────────┐ │
│  │   Models    │  │ Interfaces  │  │     Services        │ │
│  │ (Bike, etc.)│  │(IRepository)│  │(BikePricingService) │ │
│  └─────────────┘  └──────┬──────┘  └─────────────────────┘ │
│                          │                                  │
├──────────────────────────┼──────────────────────────────────┤
│                     DATA LAYER                              │
│                    (Area52.Data)                            │
│  ┌─────────────────┐  ┌─────────────────────────────────┐  │
│  │ DatabaseContext │  │      Repositories               │  │
│  │ (MySqlConnection)│ │ (BikeRepository implementatie)  │  │
│  └────────┬────────┘  └─────────────────────────────────┘  │
│           │                                                 │
│           ▼                                                 │
│    ┌─────────────┐                                         │
│    │   MySQL     │                                         │
│    │  Database   │                                         │
│    └─────────────┘                                         │
└─────────────────────────────────────────────────────────────┘
```

**Waarom deze architectuur?**
- **Separation of Concerns**: Elke laag heeft één verantwoordelijkheid
- **Testbaarheid**: Lagen kunnen onafhankelijk getest worden
- **Flexibiliteit**: Database wisselen zonder UI te wijzigen
- **Herbruikbaarheid**: Domain models in meerdere projecten

### Design Patterns Gebruikt

| Pattern | Waar | Doel |
|---------|------|------|
| **Repository Pattern** | BikeRepository, BikeReservationRepository | Abstractie tussen business logic en data access |
| **Factory Pattern** | MapToBike() methode | Aanmaken van juiste Bike subclass (CityBike/ElectricBike) |
| **Dependency Injection** | Program.cs | Loose coupling tussen componenten |
| **DTO Pattern** | ReservationDto | Data transfer tussen database en domain |
| **Strategy Pattern** | IRentalOption implementaties | Uitwisselbare huuroptie berekeningen |
| **Template Method** | Bike abstract class | Gedeelde structuur, specifieke implementatie |

### Inheritance: Bike Hiërarchie

```
        ┌─────────────────────┐
        │     Bike            │  ← Abstract base class
        │  ─────────────────  │
        │  + BikeId           │
        │  + DayPrice         │
        │  + PurchaseDate     │
        │  + BikeType (abstract)│
        └──────────┬──────────┘
                   │
        ┌──────────┴──────────┐
        │                     │
        ▼                     ▼
┌───────────────┐    ┌───────────────────┐
│   CityBike    │    │   ElectricBike    │
│ ───────────── │    │ ───────────────── │
│ BikeType =    │    │ BikeType =        │
│  "CityBike"   │    │  "ElectricBike"   │
│               │    │ + BatteryCapacity │
└───────────────┘    └───────────────────┘
```

**Polymorfisme in actie:**
```csharp
// Repository geeft List<Bike> terug, maar bevat CityBike EN ElectricBike
IEnumerable<Bike> bikes = _bikeRepository.GetAll();

foreach (var bike in bikes)
{
    // BikeType property is abstract → elke subclass geeft eigen waarde
    Console.WriteLine(bike.BikeType);  // "CityBike" of "ElectricBike"
    
    // Type-specifieke logica
    if (bike is ElectricBike eBike)
    {
        Console.WriteLine($"Batterij: {eBike.BatteryCapacity} Wh");
    }
}
```

### Dapper Cheat Sheet

```csharp
// SELECT meerdere records
var bikes = connection.Query<Bike>("SELECT * FROM Bikes");

// SELECT één record
var bike = connection.QueryFirstOrDefault<Bike>(
    "SELECT * FROM Bikes WHERE BikeId = @Id", 
    new { Id = 1 }
);

// INSERT/UPDATE/DELETE
connection.Execute(
    "DELETE FROM Bikes WHERE BikeId = @Id", 
    new { Id = 1 }
);

// INSERT met nieuwe ID terug (MySQL)
var newId = connection.ExecuteScalar<int>(
    "INSERT INTO Bikes (BikeType) VALUES (@Type); SELECT LAST_INSERT_ID();", 
    new { Type = "CityBike" }
);

// Query naar DTO met andere kolomnamen
var results = connection.Query<BikeDto>(sql);
```

### MySQL vs SQL Server Syntax

| Operatie | MySQL | SQL Server |
|----------|-------|------------|
| Auto-increment ID | `LAST_INSERT_ID()` | `SCOPE_IDENTITY()` |
| Limit rows | `LIMIT 10` | `TOP 10` |
| String concat | `CONCAT(a, b)` | `a + b` |
| Boolean | `TINYINT(1)` | `BIT` |
| Current date | `CURDATE()` | `GETDATE()` |
| If null | `IFNULL(x, 0)` | `ISNULL(x, 0)` |

---

## 🔧 Veelgestelde Problemen (FAQ)

### "Connection refused" bij database

**Oorzaak**: MySQL server niet bereikbaar of verkeerde credentials.

**Oplossing**:
1. Check of MySQL server draait op de juiste poort (3306)
2. Controleer firewall instellingen
3. Verifieer credentials in appsettings.json
4. Test connectie met MySQL Workbench of command line

### "Port already in use" bij `dotnet run`

**Oorzaak**: Vorige instantie draait nog.

**Oplossing**:
```powershell
# Stop alle dotnet processen
Get-Process -Name "dotnet" -ErrorAction SilentlyContinue | Stop-Process -Force

# Wacht even en start opnieuw
Start-Sleep -Seconds 2
dotnet run
```

### Railway build faalt

**Mogelijke oorzaken**:
1. **Dockerfile niet gevonden**: Check Settings → Builder = Dockerfile
2. **Verkeerde .NET versie**: Moet .NET 8.0 zijn (niet 10.0)
3. **NuGet restore faalt**: Check internet connectie en package versions

### Vertalingen werken niet

**Check**:
1. Is `translations.js` correct geladen? (F12 → Network tab)
2. Hebben elementen `data-i18n` attributen?
3. Bestaan de vertaalsleutels in het translations object?

---

## 📝 Code Comments Standaard

Voor Fontys HBO-ICT worden Nederlandse comments gebruikt:

```csharp
/// <summary>
/// Haalt alle fietsen op uit de MySQL database.
/// Gebruikt Dapper ORM voor het mappen van database records naar Bike objecten.
/// </summary>
/// <returns>Collectie van Bike objecten (kan CityBike of ElectricBike zijn)</returns>
/// <remarks>
/// Let op: Deze methode gebruikt polymorfisme - de daadwerkelijke types
/// worden bepaald door de BikeType kolom in de database.
/// </remarks>
public IEnumerable<Bike> GetAll()
{
    // Maak een nieuwe database connectie (using zorgt voor automatisch sluiten)
    using var connection = _db.CreateConnection();
    
    // SQL query om alle fiets gegevens op te halen
    const string sql = "SELECT BikeId, BikeType, DayPrice, PurchaseDate, BatteryCapacityWh FROM Bikes";
    
    // Dapper Query<dynamic> haalt de records op als dynamic objecten
    // Dit is nodig omdat we later naar CityBike of ElectricBike moeten casten
    var results = connection.Query<dynamic>(sql);
    
    // Map elk database record naar het juiste Bike subtype met factory method
    return results.Select(MapToBike).ToList();
}
```

---

## 👥 Auteurs & Bijdragen

| Wie | Bijdrage |
|-----|----------|
| **AlferNL** | Frontend modernisatie, MySQL integratie, meertalige ondersteuning, Railway deployment, documentatie |
| **IrsadUgur55** | Originele codebase en architectuur |
| **Fontys HBO-ICT** | Educatieve context |

### Git Workflow

We gebruiken feature branches:

```bash
# Nieuwe feature starten
git checkout frontend-anpassungen
git pull origin frontend-anpassungen

# Wijzigingen maken...

# Committen met beschrijvende message
git add -A
git commit -m "feat: nieuwe functionaliteit toegevoegd"

# Pushen naar remote
git push origin frontend-anpassungen
```

**Commit Message Conventie:**
- `feat:` - Nieuwe functionaliteit
- `fix:` - Bug fix
- `docs:` - Documentatie wijzigingen
- `style:` - Code formatting (geen functionele wijziging)
- `refactor:` - Code refactoring
- `test:` - Tests toevoegen/wijzigen

---

## 📄 Licentie

Dit project is ontwikkeld voor **Fontys HBO-ICT** als onderdeel van het Software Engineering curriculum.

### Repository Links

- **Fork (met aanpassingen)**: https://github.com/AlferNL/Area52-fork
- **Origineel**: https://github.com/IrsadUgur55/Area52

### Branches

| Branch | Beschrijving |
|--------|--------------|
| `main` | Productie-ready code |
| `frontend-anpassungen` | Feature branch met alle wijzigingen |

---

## 📖 Verdere Leermiddelen

### ASP.NET Core
- [Microsoft Docs - ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [ASP.NET Core MVC Tutorial](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/)

### Dapper ORM
- [Dapper Tutorial](https://dapper-tutorial.net/)
- [Dapper GitHub](https://github.com/DapperLib/Dapper)

### MySQL
- [MySQL Documentation](https://dev.mysql.com/doc/)
- [MySqlConnector Docs](https://mysqlconnector.net/)

### Railway Deployment
- [Railway Docs](https://docs.railway.app/)
- [Deploying .NET to Railway](https://docs.railway.app/guides/dotnet)

### CSS & Design
- [CSS Variables (MDN)](https://developer.mozilla.org/en-US/docs/Web/CSS/Using_CSS_custom_properties)
- [Bootstrap 5 Documentation](https://getbootstrap.com/docs/5.0/)

---

*Laatst bijgewerkt: Januari 2026*

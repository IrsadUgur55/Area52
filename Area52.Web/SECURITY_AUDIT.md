# 🔐 SECURITY AUDIT RAPPORT - Area52 Fietsverhuur Applicatie

> **Datum:** Januari 2026  
> **Type:** Code Review & Security Assessment  
> **Applicatie:** Area52 Fietsverhuur (ASP.NET Core MVC)  
> **Status:** ⚠️ Meerdere security issues gevonden

---

## 📊 Samenvatting

| Risico Level | Aantal Issues | Status |
|--------------|---------------|--------|
| 🔴 **KRITIEK** | 2 | ❌ Open |
| 🟠 **HOOG** | 3 | ❌ Open |
| 🟡 **MEDIUM** | 4 | ❌ Open |
| 🟢 **LAAG** | 3 | ✅ Deels opgelost |

---

## 🔴 KRITIEKE BEVEILIGINGSISSUES

### 1. GEEN AUTHENTICATIE/AUTORISATIE

| | |
|---|---|
| **Risico** | 🔴 KRITIEK |
| **Locatie** | Hele applicatie |
| **OWASP** | A01:2021 - Broken Access Control |

#### Probleem
De applicatie heeft **geen login systeem**. Iedereen kan:
- ❌ Fietsen toevoegen, wijzigen, verwijderen
- ❌ Reserveringen van anderen bekijken en verwijderen
- ❌ Alle data manipuleren zonder enige controle

#### Impact
- Complete data integriteit verlies
- Geen audit trail (wie deed wat?)
- Privacy schending (AVG/GDPR)

#### Oplossing

**Stap 1: NuGet packages toevoegen**
```xml
<!-- Area52.Web.csproj -->
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="8.0.0" />
```

**Stap 2: Program.cs aanpassen**
```csharp
// Program.cs - Voeg authenticatie toe
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

// Na app.UseRouting()
app.UseAuthentication();
app.UseAuthorization();
```

**Stap 3: Controllers beveiligen**
```csharp
// Hele controller beveiligen
[Authorize]
public class BikeController : Controller { }

// Specifieke action beveiligen voor admin
[Authorize(Roles = "Admin")]
public IActionResult Delete(int id) { }

// Anonieme toegang toestaan voor specifieke actions
[AllowAnonymous]
public IActionResult Index() { }
```

---

### 2. GEEN RATE LIMITING

| | |
|---|---|
| **Risico** | 🔴 KRITIEK |
| **Locatie** | Alle endpoints |
| **OWASP** | A04:2021 - Insecure Design |

#### Probleem
Geen bescherming tegen:
- ❌ Brute force aanvallen
- ❌ DDoS aanvallen
- ❌ Automated scraping/bots
- ❌ Resource exhaustion

#### Oplossing

**Program.cs aanpassen (.NET 8+)**
```csharp
using System.Threading.RateLimiting;

// Rate limiting configuratie
builder.Services.AddRateLimiter(options =>
{
    // Globale limiet per IP
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,           // Max 100 requests
                Window = TimeSpan.FromMinutes(1), // Per minuut
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 10
            }));
    
    // Custom response bij rate limit
    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync(
            "Te veel verzoeken. Probeer het later opnieuw.", cancellationToken: token);
    };
});

// Na app.UseRouting()
app.UseRateLimiter();
```

---

## 🟠 HOGE RISICO ISSUES

### 3. GEEN SECURITY HEADERS

| | |
|---|---|
| **Risico** | 🟠 HOOG |
| **Locatie** | `Program.cs` |
| **OWASP** | A05:2021 - Security Misconfiguration |

#### Probleem
Ontbrekende HTTP security headers maken de applicatie kwetsbaar voor:
- ❌ Clickjacking (iframe embedding)
- ❌ XSS aanvallen (Cross-Site Scripting)
- ❌ MIME type sniffing
- ❌ Information leakage

#### Huidige Headers Test
```bash
curl -I https://jouw-app.railway.app
# Ontbrekend: X-Frame-Options, X-Content-Type-Options, CSP, etc.
```

#### Oplossing

**Program.cs - Security Headers Middleware**
```csharp
// Na app.UseHsts() toevoegen:
app.Use(async (context, next) =>
{
    var headers = context.Response.Headers;
    
    // Voorkom dat de pagina in een iframe wordt geladen (clickjacking)
    headers.Append("X-Frame-Options", "DENY");
    
    // Activeer XSS filter in browsers
    headers.Append("X-XSS-Protection", "1; mode=block");
    
    // Voorkom MIME type sniffing
    headers.Append("X-Content-Type-Options", "nosniff");
    
    // Content Security Policy - beperkt welke resources geladen mogen worden
    headers.Append("Content-Security-Policy", 
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline'; " +  // Voor inline JS
        "style-src 'self' 'unsafe-inline'; " +   // Voor inline CSS
        "img-src 'self' data:; " +               // Afbeeldingen
        "font-src 'self'; " +                    // Fonts
        "frame-ancestors 'none'");               // Geen iframes
    
    // Referrer Policy - beperkt welke info wordt meegestuurd
    headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    
    // Permissions Policy - beperkt browser features
    headers.Append("Permissions-Policy", 
        "geolocation=(), microphone=(), camera=()");
    
    await next();
});
```

---

### 4. INPUT VALIDATIE ONVOLLEDIG

| | |
|---|---|
| **Risico** | 🟠 HOOG |
| **Locatie** | `ReservationController.cs` |
| **OWASP** | A03:2021 - Injection |

#### Probleem
Huidige validatie in `Create` action:
```csharp
// ONVOLDOENDE - alleen check op <= 0
if (days <= 0)
{
    ModelState.AddModelError(nameof(days), "Days must be at least 1.");
}
```

Ontbrekende validaties:
- ❌ Geen maximum op `days` (kan 999999 dagen zijn → overflow)
- ❌ Geen validatie op `startDate` (kan 1900-01-01 zijn)
- ❌ Geen check op negatieve `DayPrice` bij bikes

#### Oplossing

**ReservationController.cs - Verbeterde validatie**
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public IActionResult Create(int bikeId, DateTime startDate, int days, string[]? optionKeys)
{
    // ===== INPUT VALIDATIE =====
    
    // Days: moet tussen 1 en 365 zijn
    if (days <= 0)
    {
        ModelState.AddModelError(nameof(days), "Aantal dagen moet minimaal 1 zijn.");
    }
    else if (days > 365)
    {
        ModelState.AddModelError(nameof(days), "Maximale huurduur is 365 dagen.");
    }
    
    // StartDate: niet in het verleden
    if (startDate.Date < DateTime.Today)
    {
        ModelState.AddModelError(nameof(startDate), "Startdatum kan niet in het verleden liggen.");
    }
    
    // StartDate: niet te ver in de toekomst (max 1 jaar)
    if (startDate.Date > DateTime.Today.AddYears(1))
    {
        ModelState.AddModelError(nameof(startDate), "Startdatum kan maximaal 1 jaar in de toekomst zijn.");
    }
    
    // BikeId: moet bestaan
    var bike = _bikeRepo.GetById(bikeId);
    if (bike == null)
    {
        return NotFound("Fiets niet gevonden.");
    }
    
    // Check ModelState VOOR verdere verwerking
    if (!ModelState.IsValid)
    {
        var bikes = _bikeRepo.GetAll();
        return View(bikes);
    }
    
    // ... rest van de code
}
```

**Bike Model - Data Annotations**
```csharp
// Area52.Domain/Models/Bike.cs
using System.ComponentModel.DataAnnotations;

public class Bike
{
    public int BikeId { get; set; }
    
    [Required(ErrorMessage = "Type is verplicht")]
    [StringLength(50, ErrorMessage = "Type mag maximaal 50 karakters zijn")]
    public string Type { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Dagprijs is verplicht")]
    [Range(0.01, 1000.00, ErrorMessage = "Dagprijs moet tussen €0.01 en €1000 zijn")]
    public decimal DayPrice { get; set; }
    
    [Required(ErrorMessage = "Aankoopdatum is verplicht")]
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }
}
```

---

### 5. INSECURE DIRECT OBJECT REFERENCE (IDOR)

| | |
|---|---|
| **Risico** | 🟠 HOOG |
| **Locatie** | Alle Controllers met `id` parameter |
| **OWASP** | A01:2021 - Broken Access Control |

#### Probleem
Gebruikers kunnen data van anderen bekijken/wijzigen door ID in URL te manipuleren:

```
# Mijn reservering bekijken
GET /Reservation/Details/1

# Reservering van iemand anders bekijken (ID+1)
GET /Reservation/Details/2  ← ⚠️ WERKT!

# Reservering van iemand anders verwijderen
POST /Reservation/Delete/2  ← ⚠️ WERKT!
```

#### Oplossing (na authenticatie implementatie)

```csharp
public IActionResult Details(int id)
{
    var reservation = _reservationRepo.GetById(id);
    if (reservation == null) 
        return NotFound();
    
    // IDOR BESCHERMING: Check eigenaarschap
    var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
    if (reservation.UserId != currentUserId && !User.IsInRole("Admin"))
    {
        // Log verdachte activiteit
        _logger.LogWarning(
            "IDOR poging: User {UserId} probeerde reservering {ReservationId} te bekijken", 
            currentUserId, id);
        
        return Forbid(); // HTTP 403 Forbidden
    }
    
    return View(reservation);
}
```

---

## 🟡 MEDIUM RISICO ISSUES

### 6. SINGLETON REPOSITORIES (Thread Safety)

| | |
|---|---|
| **Risico** | 🟡 MEDIUM |
| **Locatie** | `Program.cs` regels 14-16 |

#### Probleem
```csharp
// PROBLEEM: Singleton met database connecties
builder.Services.AddSingleton<IBikeRepository, BikeRepository>();
builder.Services.AddSingleton<IBikeReservationRepository, BikeReservationRepository>();
```

Database connecties zijn **niet thread-safe**. Bij gelijktijdige requests kunnen connecties door elkaar lopen.

#### Oplossing
```csharp
// CORRECT: Scoped = nieuwe instantie per HTTP request
builder.Services.AddScoped<IBikeRepository, BikeRepository>();
builder.Services.AddScoped<IBikeReservationRepository, BikeReservationRepository>();
builder.Services.AddScoped<IBikePricingService, BikePricingService>();

// DatabaseContext mag Singleton blijven (bevat alleen connection string)
builder.Services.AddSingleton<DatabaseContext>();
```

**Uitleg DI Lifetimes:**
| Lifetime | Gedrag | Gebruik |
|----------|--------|---------|
| Singleton | 1 instantie voor hele applicatie | Config, logging |
| Scoped | 1 instantie per HTTP request | Repositories, DbContext |
| Transient | Nieuwe instantie elke keer | Lightweight services |

---

### 7. GEEN LOGGING VAN SECURITY EVENTS

| | |
|---|---|
| **Risico** | 🟡 MEDIUM |
| **Locatie** | Hele applicatie |
| **OWASP** | A09:2021 - Security Logging Failures |

#### Probleem
Geen logging van:
- ❌ Data wijzigingen (wie verwijderde wat?)
- ❌ Verdachte activiteit
- ❌ Error details voor debugging

#### Oplossing

**Controller met logging**
```csharp
public class BikeController : Controller
{
    private readonly IBikeRepository _repo;
    private readonly ILogger<BikeController> _logger;

    public BikeController(IBikeRepository repo, ILogger<BikeController> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var bike = _repo.GetById(id);
        if (bike == null)
        {
            _logger.LogWarning("Delete poging voor niet-bestaande bike {BikeId}", id);
            return NotFound();
        }
        
        // Log VOOR de actie (als delete faalt heb je nog de info)
        _logger.LogInformation(
            "Bike verwijderd: BikeId={BikeId}, Type={Type}, Door={User}, IP={IP}, Tijd={Time}",
            id, 
            bike.Type,
            User.Identity?.Name ?? "Anonymous",
            HttpContext.Connection.RemoteIpAddress,
            DateTime.UtcNow);
        
        _repo.Delete(id);
        return RedirectToAction(nameof(Index));
    }
}
```

**appsettings.json - Logging configuratie**
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Area52.Web.Controllers": "Information"
    }
  }
}
```

---

### 8. DEVELOPMENT ERROR PAGINA IN PRODUCTIE

| | |
|---|---|
| **Risico** | 🟡 MEDIUM |
| **Locatie** | Railway deployment |
| **OWASP** | A05:2021 - Security Misconfiguration |

#### Probleem
De foutmelding die bij deployment werd gezien:
```
Development Mode
Swapping to Development environment will display more detailed information...
```

Dit suggereert dat gedetailleerde errors zichtbaar kunnen zijn voor eindgebruikers.

#### Oplossing

**Railway Environment Variable:**
```
ASPNETCORE_ENVIRONMENT=Production
```

**Program.cs - Error handling**
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Gedetailleerde errors
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Generieke error pagina
    app.UseHsts();
}
```

---

### 9. ONGEBRUIKTE SQL SERVER DEPENDENCY

| | |
|---|---|
| **Risico** | 🟡 MEDIUM |
| **Locatie** | `Area52.Web.csproj` |

#### Probleem
```xml
<!-- NOG AANWEZIG maar niet gebruikt -->
<PackageReference Include="Microsoft.Data.SqlClient" Version="6.1.3" />
```

Ongebruikte dependencies:
- Vergroten het aanvalsoppervlak
- Kunnen kwetsbaarheden bevatten
- Vergroten de Docker image

#### Oplossing
Verwijder de regel uit `Area52.Web.csproj`:
```xml
<!-- VERWIJDER DEZE REGEL -->
<PackageReference Include="Microsoft.Data.SqlClient" Version="6.1.3" />
```

---

## 🟢 LAGE RISICO / GOED GEÏMPLEMENTEERD

### 10. ✅ CSRF BESCHERMING AANWEZIG

```csharp
// GOED: Anti-forgery tokens worden gebruikt
[HttpPost]
[ValidateAntiForgeryToken]  ← ✅
public IActionResult Create(Bike bike)
```

### 11. ✅ HTTPS REDIRECTIE

```csharp
// GOED: HTTPS wordt afgedwongen
app.UseHttpsRedirection();  ← ✅
app.UseHsts();              ← ✅
```

### 12. ✅ PARAMETERIZED QUERIES (SQL Injection bescherming)

```csharp
// GOED: Dapper gebruikt parameterized queries
const string sql = "SELECT * FROM Bikes WHERE BikeId = @Id";
connection.QueryFirstOrDefault<dynamic>(sql, new { Id = id });  ← ✅
```

---

## 📋 PRIORITEIT ACTIEPLAN

### Quick Wins (< 1 uur)

| # | Issue | Actie | Tijd |
|---|-------|-------|------|
| 1 | Scoped Services | Wijzig Singleton → Scoped in Program.cs | 5 min |
| 2 | Verwijder SqlClient | Delete regel uit .csproj | 2 min |
| 3 | Security Headers | Voeg middleware toe aan Program.cs | 30 min |

### Korte Termijn (1-2 dagen)

| # | Issue | Actie | Tijd |
|---|-------|-------|------|
| 4 | Input Validatie | Data annotations + controller validatie | 2 uur |
| 5 | Rate Limiting | Voeg rate limiter toe | 1 uur |
| 6 | Security Logging | ILogger injecteren in controllers | 2 uur |

### Lange Termijn (1 week)

| # | Issue | Actie | Tijd |
|---|-------|-------|------|
| 7 | Authenticatie | ASP.NET Identity implementeren | 8 uur |
| 8 | Autorisatie | Roles en policies configureren | 4 uur |
| 9 | IDOR bescherming | Eigenaarschap checks toevoegen | 2 uur |

---

## 🧪 SECURITY TESTING CHECKLIST

### Handmatige Tests
- [ ] Probeer toegang zonder login (alle pagina's)
- [ ] Wijzig ID's in URL's (IDOR test)
- [ ] Voer speciale karakters in (`<script>`, `'; DROP TABLE`)
- [ ] Test met datum in verleden
- [ ] Test met negatieve getallen
- [ ] Test met extreem grote getallen

### Geautomatiseerde Tools
```bash
# OWASP ZAP scan
docker run -t owasp/zap2docker-stable zap-baseline.py -t https://jouw-app.railway.app

# Nikto web scanner
nikto -h https://jouw-app.railway.app

# SSL/TLS check
ssllabs-scan --grade https://jouw-app.railway.app
```

---

## 📚 Referenties

- [OWASP Top 10 (2021)](https://owasp.org/Top10/)
- [ASP.NET Core Security Best Practices](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [Microsoft Secure Coding Guidelines](https://docs.microsoft.com/en-us/dotnet/standard/security/secure-coding-guidelines)
- [OWASP Cheat Sheet Series](https://cheatsheetseries.owasp.org/)

---

## 📝 Audit Log

| Datum | Actie | Door |
|-------|-------|------|
| 2026-01-09 | Initiële security audit | AlferNL |
| | | |

---

*Dit document is gegenereerd als onderdeel van de Area52 security review voor Fontys HBO-ICT.*

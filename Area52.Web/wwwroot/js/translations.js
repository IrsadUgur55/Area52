/**
 * ============================================================================
 * AREA52 - MULTI-LANGUAGE SUPPORT MODULE
 * ============================================================================
 * 
 * Dit JavaScript-bestand implementeert een client-side meertalig systeem
 * voor de Area52 fietsverhuur webapplicatie. Het ondersteunt drie talen:
 * - Nederlands (nl) - standaardtaal
 * - Duits (de)
 * - Engels (en)
 * 
 * ARCHITECTUUR:
 * Het systeem werkt door HTML-elementen te voorzien van een data-translate
 * attribuut met een unieke key. JavaScript zoekt deze elementen en vervangt
 * de tekst met de vertaling uit dit bestand.
 * 
 * VOORBEELD GEBRUIK IN HTML:
 * <h1 data-translate="home.welcome">Welkom bij Area52</h1>
 * 
 * De tekst "Welkom bij Area52" wordt automatisch vervangen door de vertaling
 * die overeenkomt met de huidige taalinstelling van de gebruiker.
 * 
 * PERSISTENTIE:
 * De taalvoorkeur wordt opgeslagen in localStorage, zodat de keuze behouden
 * blijft bij het herladen van de pagina of bij een volgend bezoek.
 * 
 * @author    Frontend Team - Area52
 * @version   1.0.0
 * @date      Januari 2026
 * @course    HBO ICT - Fontys Hogescholen
 * ============================================================================
 */


/**
 * TRANSLATIONS OBJECT
 * ===================
 * 
 * Dit object bevat alle vertalingen voor de applicatie.
 * De structuur is als volgt:
 * 
 * "unique.key": {
 *     nl: "Nederlandse tekst",
 *     de: "Duitse tekst",
 *     en: "Engelse tekst"
 * }
 * 
 * KEY NAMING CONVENTION:
 * We gebruiken een hiërarchische naamgeving met punten (dots) als scheiding:
 * - Eerste deel: de pagina of component (home, bikes, res, nav)
 * - Tweede deel: de sectie of element type (title, btn, table)
 * - Derde deel (optioneel): specifiek element (new, edit, delete)
 * 
 * VOORBEELD: "bikes.table.price" = Fietsen pagina > Tabel > Prijskolom
 */
const translations = {
    
    /* =========================================================================
       NAVIGATIE ELEMENTEN
       =========================================================================
       Teksten voor de hoofdnavigatiebalk die op elke pagina zichtbaar is.
       Deze elementen zijn gedefinieerd in _Layout.cshtml.
    */
    
    /**
     * Home link in de navigatie
     * Verwijst naar de hoofdpagina van de applicatie
     */
    "nav.home": {
        nl: "Home",
        de: "Startseite",
        en: "Home"
    },
    
    /**
     * Fietsen link in de navigatie
     * Verwijst naar het overzicht van alle beschikbare fietsen
     */
    "nav.bikes": {
        nl: "Fietsen",
        de: "Fahrräder",
        en: "Bikes"
    },
    
    /**
     * Reserveringen link in de navigatie
     * Verwijst naar het overzicht van alle reserveringen
     */
    "nav.reservations": {
        nl: "Reserveringen",
        de: "Reservierungen",
        en: "Reservations"
    },

    /* =========================================================================
       FOOTER ELEMENTEN
       =========================================================================
       Teksten voor de footer onderaan elke pagina.
    */
    
    /**
     * Copyright notice in de footer
     * Bevat het jaar, teamnaam en bedrijfsnaam
     */
    "footer.copyright": {
        nl: "© 2026 - Team PLACEHOLDERS - Area52 Fietsverhuur",
        de: "© 2026 - Team PLACEHOLDERS - Area52 Fahrradvermietung",
        en: "© 2026 - Team PLACEHOLDERS - Area52 Bike Rentals"
    },

    /* =========================================================================
       HOME PAGE (Index.cshtml in Home folder)
       =========================================================================
       Teksten voor de welkomstpagina met hero-sectie en informatiekaarten.
    */
    
    /**
     * Welkomstbericht in de hero-sectie
     * Het belangrijkste zichtbare element voor nieuwe bezoekers
     */
    "home.welcome": {
        nl: "🚲 Welkom bij Area52",
        de: "🚲 Willkommen bei Area52",
        en: "🚲 Welcome to Area52"
    },
    
    /**
     * Ondertitel in de hero-sectie
     * Beschrijft kort de diensten van het bedrijf
     */
    "home.subtitle": {
        nl: "Uw premium fietsverhuurservice - Stadsfietsen & Elektrische fietsen beschikbaar",
        de: "Ihr Premium-Fahrradverleih - Stadt- & Elektrofahrräder verfügbar",
        en: "Your premium bike rental service - City bikes & Electric bikes available"
    },
    
    /**
     * Call-to-action knop: Bekijk fietsen
     * Navigeert naar het fietsen-overzicht
     */
    "home.btn.browse": {
        nl: "🚴 Bekijk Fietsen",
        de: "🚴 Fahrräder ansehen",
        en: "🚴 Browse Bikes"
    },
    
    /**
     * Call-to-action knop: Nieuwe reservering
     * Navigeert direct naar het reserveringsformulier
     */
    "home.btn.newres": {
        nl: "📅 Nieuwe Reservering",
        de: "📅 Neue Reservierung",
        en: "📅 New Reservation"
    },
    
    /**
     * Call-to-action knop: Mijn reserveringen
     * Navigeert naar het overzicht van reserveringen
     */
    "home.btn.myres": {
        nl: "📋 Mijn Reserveringen",
        de: "📋 Meine Reservierungen",
        en: "📋 My Reservations"
    },
    
    /**
     * Informatiekaart: Stadsfietsen titel
     */
    "home.card.city.title": {
        nl: "Stadsfietsen",
        de: "Stadtfahrräder",
        en: "City Bikes"
    },
    
    /**
     * Informatiekaart: Stadsfietsen beschrijving
     */
    "home.card.city.desc": {
        nl: "Perfect voor stadsverkenning en dagelijks woon-werkverkeer",
        de: "Perfekt für Stadterkundung und tägliches Pendeln",
        en: "Perfect for urban exploration and daily commutes"
    },
    
    /**
     * Informatiekaart: Elektrische fietsen titel
     */
    "home.card.electric.title": {
        nl: "Elektrische Fietsen",
        de: "Elektrofahrräder",
        en: "Electric Bikes"
    },
    
    /**
     * Informatiekaart: Elektrische fietsen beschrijving
     */
    "home.card.electric.desc": {
        nl: "Moeiteloos rijden met elektrische ondersteuning",
        de: "Müheloses Fahren mit elektrischer Unterstützung",
        en: "Effortless rides with electric assistance"
    },
    
    /**
     * Informatiekaart: Verzekeringen titel
     */
    "home.card.insurance.title": {
        nl: "Verzekeringen",
        de: "Versicherungen",
        en: "Insurance Options"
    },
    
    /**
     * Informatiekaart: Verzekeringen beschrijving
     */
    "home.card.insurance.desc": {
        nl: "Rijd zorgeloos met onze dekkingsplannen",
        de: "Fahren Sie sorglos mit unseren Versicherungsplänen",
        en: "Ride worry-free with our coverage plans"
    },

    /* =========================================================================
       BIKES INDEX PAGE (Index.cshtml in Bike folder)
       =========================================================================
       Teksten voor het overzicht van alle beschikbare fietsen.
    */
    
    /**
     * Paginatitel voor het fietsen-overzicht
     */
    "bikes.title": {
        nl: "🚲 Beschikbare Fietsen",
        de: "🚲 Verfügbare Fahrräder",
        en: "🚲 Available Bikes"
    },
    
    /**
     * Knop: Nieuwe fiets toevoegen
     */
    "bikes.btn.new": {
        nl: "+ Nieuwe Fiets",
        de: "+ Neues Fahrrad",
        en: "+ Add New Bike"
    },
    
    /**
     * Knop: Nieuwe reservering maken
     */
    "bikes.btn.reserve": {
        nl: "📅 Nieuwe Reservering",
        de: "📅 Neue Reservierung",
        en: "📅 New Reservation"
    },
    
    /* Tabel kolomkoppen */
    "bikes.table.id": {
        nl: "ID",
        de: "ID",
        en: "ID"
    },
    "bikes.table.type": {
        nl: "Type",
        de: "Typ",
        en: "Type"
    },
    "bikes.table.price": {
        nl: "Dagprijs",
        de: "Tagespreis",
        en: "Daily Price"
    },
    "bikes.table.date": {
        nl: "Aankoopdatum",
        de: "Kaufdatum",
        en: "Purchase Date"
    },
    "bikes.table.actions": {
        nl: "Acties",
        de: "Aktionen",
        en: "Actions"
    },
    
    /* Fiets type labels */
    /**
     * Label voor elektrische fietsen
     * Gebruikt in tabellen en detail-weergaven
     */
    "bikes.type.electric": {
        nl: "⚡ Elektrische Fiets",
        de: "⚡ Elektrofahrrad",
        en: "⚡ Electric Bike"
    },
    
    /**
     * Label voor stadsfietsen
     */
    "bikes.type.city": {
        nl: "🚲 Stadsfiets",
        de: "🚲 Stadtfahrrad",
        en: "🚲 City Bike"
    },
    
    /**
     * Suffix voor prijsweergave (per dag)
     */
    "bikes.perday": {
        nl: "/dag",
        de: "/Tag",
        en: "/day"
    },
    
    /* Tabel actie links */
    "bikes.action.details": {
        nl: "Details",
        de: "Details",
        en: "Details"
    },
    "bikes.action.edit": {
        nl: "Bewerken",
        de: "Bearbeiten",
        en: "Edit"
    },
    "bikes.action.delete": {
        nl: "Verwijderen",
        de: "Löschen",
        en: "Delete"
    },

    /* =========================================================================
       BIKES CREATE PAGE (Create.cshtml in Bike folder)
       =========================================================================
       Teksten voor het formulier om een nieuwe fiets toe te voegen.
    */
    
    /**
     * Paginatitel voor nieuwe fiets formulier
     */
    "bikes.create.title": {
        nl: "🚲 Nieuwe Fiets Toevoegen",
        de: "🚲 Neues Fahrrad hinzufügen",
        en: "🚲 Add New Bike"
    },
    
    /* Formulier labels */
    "bikes.create.type": {
        nl: "Fietstype",
        de: "Fahrradtyp",
        en: "Bike Type"
    },
    "bikes.create.price": {
        nl: "Dagprijs (€)",
        de: "Tagespreis (€)",
        en: "Daily Price (€)"
    },
    "bikes.create.date": {
        nl: "Aankoopdatum",
        de: "Kaufdatum",
        en: "Purchase Date"
    },
    
    /* Dropdown opties */
    "bikes.create.option.city": {
        nl: "🚲 Stadsfiets",
        de: "🚲 Stadtfahrrad",
        en: "🚲 City Bike"
    },
    "bikes.create.option.electric": {
        nl: "⚡ Elektrische Fiets",
        de: "⚡ Elektrofahrrad",
        en: "⚡ Electric Bike"
    },
    
    /* Knoppen */
    "bikes.create.save": {
        nl: "✓ Fiets Opslaan",
        de: "✓ Fahrrad speichern",
        en: "✓ Save Bike"
    },
    "bikes.create.back": {
        nl: "← Terug naar Overzicht",
        de: "← Zurück zur Übersicht",
        en: "← Back to Overview"
    },

    /* =========================================================================
       RESERVATIONS INDEX PAGE (Index.cshtml in Reservation folder)
       =========================================================================
       Teksten voor het overzicht van alle reserveringen.
    */
    
    /**
     * Paginatitel voor reserveringen-overzicht
     */
    "res.title": {
        nl: "📋 Mijn Reserveringen",
        de: "📋 Meine Reservierungen",
        en: "📋 My Reservations"
    },
    
    /* Actie knoppen */
    "res.btn.new": {
        nl: "+ Nieuwe Reservering",
        de: "+ Neue Reservierung",
        en: "+ New Reservation"
    },
    "res.btn.bikes": {
        nl: "🚲 Bekijk Fietsen",
        de: "🚲 Fahrräder ansehen",
        en: "🚲 View Bikes"
    },
    
    /* Tabel kolomkoppen */
    "res.table.id": {
        nl: "ID",
        de: "ID",
        en: "ID"
    },
    "res.table.bike": {
        nl: "Fiets",
        de: "Fahrrad",
        en: "Bike"
    },
    "res.table.start": {
        nl: "Startdatum",
        de: "Startdatum",
        en: "Start Date"
    },
    "res.table.duration": {
        nl: "Duur",
        de: "Dauer",
        en: "Duration"
    },
    "res.table.options": {
        nl: "Opties",
        de: "Optionen",
        en: "Options"
    },
    "res.table.total": {
        nl: "Totaalprijs",
        de: "Gesamtpreis",
        en: "Total Price"
    },
    "res.table.actions": {
        nl: "Acties",
        de: "Aktionen",
        en: "Actions"
    },
    "res.action.details": {
        nl: "Bekijk Details",
        de: "Details ansehen",
        en: "View Details"
    },
    
    /**
     * Label voor dagen-weergave
     * Gebruikt in badges: "5 dagen"
     */
    "res.days": {
        nl: "dagen",
        de: "Tage",
        en: "days"
    },

    /* =========================================================================
       CREATE RESERVATION PAGE (Create.cshtml in Reservation folder)
       =========================================================================
       Teksten voor het reserveringsformulier.
    */
    
    /**
     * Paginatitel voor het reserveringsformulier
     */
    "res.create.title": {
        nl: "📅 Reservering Maken",
        de: "📅 Reservierung erstellen",
        en: "📅 Create Reservation"
    },
    
    /**
     * Instructietekst voor de gebruiker
     */
    "res.create.subtitle": {
        nl: "🚴 Kies een fiets en selecteer uw reserveringsgegevens hieronder.",
        de: "🚴 Wählen Sie ein Fahrrad und geben Sie Ihre Reservierungsdetails ein.",
        en: "🚴 Choose a bike and select your reservation details below."
    },
    
    /* Tabel kolomkoppen voor fiets-selectie */
    "res.create.table.id": {
        nl: "ID",
        de: "ID",
        en: "ID"
    },
    "res.create.table.type": {
        nl: "Fietstype",
        de: "Fahrradtyp",
        en: "Bike Type"
    },
    "res.create.table.price": {
        nl: "Dagprijs",
        de: "Tagespreis",
        en: "Daily Price"
    },
    "res.create.table.details": {
        nl: "Reserveringsgegevens",
        de: "Reservierungsangaben",
        en: "Reservation Details"
    },
    
    /* Formulier labels */
    "res.create.startdate": {
        nl: "Startdatum",
        de: "Startdatum",
        en: "Start Date"
    },
    "res.create.days": {
        nl: "Dagen",
        de: "Tage",
        en: "Days"
    },
    
    /**
     * Optie: Schadeverzekering
     * Extra service die de klant kan toevoegen (+2% van totaal)
     */
    "res.create.insurance": {
        nl: "🛡️ Verzekering (+2%)",
        de: "🛡️ Versicherung (+2%)",
        en: "🛡️ Insurance (+2%)"
    },
    
    /**
     * Optie: Pechhulp service
     * Extra service die de klant kan toevoegen (vast bedrag €5)
     */
    "res.create.assistance": {
        nl: "📞 Hulpdienst (+€5)",
        de: "📞 Pannenhilfe (+5€)",
        en: "📞 Assistance (+€5)"
    },
    
    /* Knoppen */
    "res.create.submit": {
        nl: "Reserveer Nu",
        de: "Jetzt Reservieren",
        en: "Reserve Now"
    },
    "res.create.back": {
        nl: "← Terug naar Fietsen",
        de: "← Zurück zu Fahrrädern",
        en: "← Back to Bikes"
    },

    /* =========================================================================
       RESERVATION CONFIRMATION PAGE (Confirmation.cshtml)
       =========================================================================
       Teksten voor de bevestigingspagina na succesvolle reservering.
    */
    
    /**
     * Hoofdtitel: Bevestiging van reservering
     */
    "confirm.title": {
        nl: "Reservering Bevestigd!",
        de: "Reservierung Bestätigt!",
        en: "Reservation Confirmed!"
    },
    
    /**
     * Ondertitel met succesmelding
     */
    "confirm.subtitle": {
        nl: "Uw fietsverhuur is succesvol geboekt.",
        de: "Ihre Fahrradmiete wurde erfolgreich gebucht.",
        en: "Your bike rental has been successfully booked."
    },
    
    /**
     * Sectietitel: Details van de reservering
     */
    "confirm.details": {
        nl: "📋 Reserveringsdetails",
        de: "📋 Reservierungsdetails",
        en: "📋 Reservation Details"
    },
    
    /* Detail labels */
    "confirm.resid": {
        nl: "Reserverings-ID",
        de: "Reservierungs-ID",
        en: "Reservation ID"
    },
    "confirm.bike": {
        nl: "Fiets",
        de: "Fahrrad",
        en: "Bike"
    },
    "confirm.start": {
        nl: "Startdatum",
        de: "Startdatum",
        en: "Start Date"
    },
    "confirm.duration": {
        nl: "Duur",
        de: "Dauer",
        en: "Duration"
    },
    
    /**
     * Sectietitel: Geselecteerde extra opties
     */
    "confirm.options": {
        nl: "🛠️ Geselecteerde Opties",
        de: "🛠️ Ausgewählte Optionen",
        en: "🛠️ Selected Options"
    },
    
    /**
     * Melding wanneer geen extra opties zijn geselecteerd
     */
    "confirm.nooptions": {
        nl: "Geen extra opties geselecteerd.",
        de: "Keine zusätzlichen Optionen ausgewählt.",
        en: "No additional options selected."
    },
    
    /**
     * Label voor de totaalprijs
     */
    "confirm.total": {
        nl: "Totaalprijs",
        de: "Gesamtpreis",
        en: "Total Price"
    },
    
    /* Navigatie knoppen na bevestiging */
    "confirm.btn.new": {
        nl: "+ Nieuwe Reservering",
        de: "+ Neue Reservierung",
        en: "+ New Reservation"
    },
    "confirm.btn.bikes": {
        nl: "🚲 Bekijk Fietsen",
        de: "🚲 Fahrräder ansehen",
        en: "🚲 Browse Bikes"
    },
    "confirm.btn.myres": {
        nl: "📋 Mijn Reserveringen",
        de: "📋 Meine Reservierungen",
        en: "📋 My Reservations"
    },
    
    /* =========================================================================
       BIKE DETAILS PAGE (Details.cshtml in Bike folder)
       =========================================================================
       Teksten voor de detail-weergave van een individuele fiets.
    */
    
    /**
     * Paginatitel voor fietsdetails
     */
    "bikes.details.title": {
        nl: "🔍 Fietsdetails",
        de: "🔍 Fahrrad-Details",
        en: "🔍 Bike Details"
    },
    "bikes.details.type": {
        nl: "Type",
        de: "Typ",
        en: "Type"
    },
    "bikes.details.dailyprice": {
        nl: "Dagprijs",
        de: "Tagespreis",
        en: "Daily Price"
    },
    "bikes.details.back": {
        nl: "← Terug naar overzicht",
        de: "← Zurück zur Übersicht",
        en: "← Back to Overview"
    },
    "bikes.details.edit": {
        nl: "✏️ Bewerken",
        de: "✏️ Bearbeiten",
        en: "✏️ Edit"
    },
    "bikes.details.reserve": {
        nl: "📅 Reserveren",
        de: "📅 Reservieren",
        en: "📅 Reserve"
    },
    
    /* =========================================================================
       BIKE EDIT PAGE (Edit.cshtml in Bike folder)
       =========================================================================
       Teksten voor het bewerken van een bestaande fiets.
    */
    
    /**
     * Paginatitel voor fiets bewerken
     */
    "bikes.edit.title": {
        nl: "✏️ Fiets Bewerken",
        de: "✏️ Fahrrad Bearbeiten",
        en: "✏️ Edit Bike"
    },
    "bikes.edit.save": {
        nl: "💾 Wijzigingen Opslaan",
        de: "💾 Änderungen Speichern",
        en: "💾 Save Changes"
    },
    "bikes.edit.cancel": {
        nl: "Annuleren",
        de: "Abbrechen",
        en: "Cancel"
    },
    "bikes.edit.selecttype": {
        nl: "-- Selecteer type --",
        de: "-- Typ auswählen --",
        en: "-- Select type --"
    },
    "bikes.edit.citytype": {
        nl: "Stadsfiets",
        de: "Stadtfahrrad",
        en: "City Bike"
    },
    "bikes.edit.electrictype": {
        nl: "Elektrische Fiets",
        de: "Elektrofahrrad",
        en: "Electric Bike"
    },
    
    /* =========================================================================
       BIKE DELETE PAGE (Delete.cshtml in Bike folder)
       =========================================================================
       Teksten voor de bevestigingspagina bij het verwijderen van een fiets.
       
       BELANGRIJK: Delete-acties moeten duidelijk waarschuwen dat de actie
       niet ongedaan gemaakt kan worden.
    */
    
    /**
     * Paginatitel voor fiets verwijderen
     * Waarschuwings-emoji voor aandacht
     */
    "bikes.delete.title": {
        nl: "⚠️ Fiets Verwijderen",
        de: "⚠️ Fahrrad Löschen",
        en: "⚠️ Delete Bike"
    },
    
    /**
     * Bevestigingsvraag
     * Vraagt de gebruiker om te bevestigen dat zij dit echt willen
     */
    "bikes.delete.confirm": {
        nl: "Weet je zeker dat je deze fiets wilt verwijderen?",
        de: "Bist du sicher, dass du dieses Fahrrad löschen möchtest?",
        en: "Are you sure you want to delete this bike?"
    },
    
    /**
     * Bevestigingsknop voor verwijderen
     */
    "bikes.delete.yes": {
        nl: "🗑️ Ja, Verwijderen",
        de: "🗑️ Ja, Löschen",
        en: "🗑️ Yes, Delete"
    },
    
    /**
     * Annuleerknop - legacy key
     */
    "bikes.delete.cancel": {
        nl: "← Nee, Terug",
        de: "← Nein, Zurück",
        en: "← No, Go Back"
    },
    
    /**
     * Annuleerknop - alternatieve key
     */
    "bikes.delete.no": {
        nl: "← Nee, Terug",
        de: "← Nein, Zurück",
        en: "← No, Go Back"
    },
    
    /* =========================================================================
       RESERVATION DETAILS PAGE (Details.cshtml in Reservation folder)
       =========================================================================
       Teksten voor de detail-weergave van een individuele reservering.
    */
    
    /**
     * Paginatitel voor reserveringsdetails
     */
    "res.details.title": {
        nl: "📋 Reserveringsdetails",
        de: "📋 Reservierungsdetails",
        en: "📋 Reservation Details"
    },
    "res.details.bikeid": {
        nl: "Fiets ID",
        de: "Fahrrad ID",
        en: "Bike ID"
    },
    "res.details.startdate": {
        nl: "Startdatum",
        de: "Startdatum",
        en: "Start Date"
    },
    "res.details.duration": {
        nl: "Duur",
        de: "Dauer",
        en: "Duration"
    },
    "res.details.days": {
        nl: "dagen",
        de: "Tage",
        en: "days"
    },
    "res.details.dailyprice": {
        nl: "Dagprijs",
        de: "Tagespreis",
        en: "Daily Price"
    },
    "res.details.total": {
        nl: "Totaalprijs",
        de: "Gesamtpreis",
        en: "Total Price"
    },
    "res.details.options": {
        nl: "🛠️ Geselecteerde Opties",
        de: "🛠️ Ausgewählte Optionen",
        en: "🛠️ Selected Options"
    },
    "res.details.nooptions": {
        nl: "Geen extra opties geselecteerd.",
        de: "Keine zusätzlichen Optionen ausgewählt.",
        en: "No additional options selected."
    },
    "res.details.back": {
        nl: "← Terug naar overzicht",
        de: "← Zurück zur Übersicht",
        en: "← Back to Overview"
    },
    
    /**
     * Knop voor verwijderen van reservering
     * Dit was eerst "delete" maar is nu "cancel" voor consistentie
     */
    "res.details.delete": {
        nl: "🗑️ Verwijderen",
        de: "🗑️ Löschen",
        en: "🗑️ Delete"
    },
    
    /**
     * Knop voor annuleren van reservering
     * Meer gebruiksvriendelijke term dan "verwijderen"
     */
    "res.details.cancel": {
        nl: "❌ Annuleren",
        de: "❌ Stornieren",
        en: "❌ Cancel"
    },
    
    /* =========================================================================
       RESERVATION DELETE PAGE (Delete.cshtml in Reservation folder)
       =========================================================================
       Teksten voor de bevestigingspagina bij het annuleren van een reservering.
       
       Let op: We gebruiken "Annuleren" / "Stornieren" / "Cancel" in plaats van
       "Verwijderen" / "Löschen" / "Delete" omdat dit gebruiksvriendelijker is
       in de context van reserveringen. Een reservering wordt geannuleerd,
       niet verwijderd.
    */
    
    /**
     * Paginatitel voor reservering annuleren
     */
    "res.delete.title": {
        nl: "⚠️ Reservering Annuleren",
        de: "⚠️ Reservierung Stornieren",
        en: "⚠️ Cancel Reservation"
    },
    
    /**
     * Bevestigingsvraag voor annuleren
     */
    "res.delete.confirm": {
        nl: "Weet je zeker dat je deze reservering wilt annuleren?",
        de: "Bist du sicher, dass du diese Reservierung stornieren möchtest?",
        en: "Are you sure you want to cancel this reservation?"
    },
    
    /**
     * Bevestigingsknop voor annuleren
     */
    "res.delete.yes": {
        nl: "🗑️ Ja, Annuleren",
        de: "🗑️ Ja, Stornieren",
        en: "🗑️ Yes, Cancel"
    },
    
    /**
     * Knop om terug te gaan zonder te annuleren
     */
    "res.delete.no": {
        nl: "← Nee, Terug",
        de: "← Nein, Zurück",
        en: "← No, Go Back"
    },
    
    /**
     * Alternatieve annuleerknop tekst
     */
    "res.delete.cancel": {
        nl: "← Terug",
        de: "← Zurück",
        en: "← Go Back"
    }
};


/* =============================================================================
   LANGUAGE MANAGEMENT FUNCTIONS
   =============================================================================
   
   Deze functies beheren het ophalen, opslaan en toepassen van de taalinstelling.
   
   LOCALSTORAGE:
   We gebruiken de Web Storage API (localStorage) om de taalvoorkeur van de
   gebruiker op te slaan. localStorage blijft behouden, zelfs als de browser
   wordt gesloten, in tegenstelling tot sessionStorage.
   
   KEY: 'area52-language'
   VALUES: 'nl', 'de', of 'en'
*/


/**
 * getCurrentLanguage()
 * ====================
 * Haalt de huidige taalinstelling op uit localStorage.
 * 
 * @returns {string} De taalcode ('nl', 'de', of 'en')
 * 
 * Als er geen taal is opgeslagen (eerste bezoek), wordt 'nl' (Nederlands)
 * als standaard geretourneerd. Dit is logisch omdat de website primair
 * gericht is op de Nederlandse markt.
 */
function getCurrentLanguage() {
    // localStorage.getItem() retourneert null als de key niet bestaat
    // De || operator zorgt dan voor de fallback naar 'nl'
    return localStorage.getItem('area52-language') || 'nl';
}


/**
 * setLanguage(lang)
 * =================
 * Stelt een nieuwe taal in en past deze direct toe op de pagina.
 * 
 * @param {string} lang - De taalcode om in te stellen ('nl', 'de', of 'en')
 * 
 * Deze functie wordt aangeroepen wanneer de gebruiker op een taalknop klikt.
 * Het slaat de nieuwe taal op in localStorage en roept vervolgens de
 * functies aan om de pagina bij te werken.
 */
function setLanguage(lang) {
    // Sla de taalvoorkeur op in localStorage voor persistentie
    localStorage.setItem('area52-language', lang);
    
    // Pas alle vertalingen toe op de huidige pagina
    applyTranslations();
    
    // Update de visuele indicatie van de actieve taalknop
    updateActiveFlag();
}


/**
 * applyTranslations()
 * ===================
 * Past alle vertalingen toe op elementen met data-translate attributen.
 * 
 * Deze functie doorzoekt de hele DOM (Document Object Model) naar elementen
 * met specifieke data-attributen en vervangt hun inhoud met de juiste
 * vertaling.
 * 
 * ONDERSTEUNDE ATTRIBUTEN:
 * - data-translate: Vervangt de textContent van het element
 * - data-translate-placeholder: Vervangt het placeholder attribuut (voor inputs)
 * - data-translate-title: Vervangt het title attribuut (voor tooltips)
 * 
 * VOORBEELD:
 * <button data-translate="home.btn.browse">Bekijk Fietsen</button>
 * 
 * Als de taal op 'de' staat, wordt dit:
 * <button data-translate="home.btn.browse">Fahrräder ansehen</button>
 */
function applyTranslations() {
    // Haal de huidige taal op
    const lang = getCurrentLanguage();
    
    // Zoek alle elementen met data-translate attribuut
    // querySelectorAll retourneert een NodeList van alle matches
    document.querySelectorAll('[data-translate]').forEach(element => {
        // Haal de vertaalsleutel op uit het data-attribuut
        const key = element.getAttribute('data-translate');
        
        // Controleer of de sleutel bestaat in ons translations object
        // en of er een vertaling is voor de huidige taal
        if (translations[key] && translations[key][lang]) {
            // Vervang de tekstinhoud met de vertaling
            element.textContent = translations[key][lang];
        }
    });

    // Verwerk placeholder attributen voor input velden
    // Dit is handig voor formulieren waar de placeholder tekst vertaald moet worden
    document.querySelectorAll('[data-translate-placeholder]').forEach(element => {
        const key = element.getAttribute('data-translate-placeholder');
        if (translations[key] && translations[key][lang]) {
            element.placeholder = translations[key][lang];
        }
    });

    // Verwerk title attributen voor tooltips
    // Tooltips verschijnen wanneer de gebruiker met de muis over een element hovert
    document.querySelectorAll('[data-translate-title]').forEach(element => {
        const key = element.getAttribute('data-translate-title');
        if (translations[key] && translations[key][lang]) {
            element.title = translations[key][lang];
        }
    });
}


/**
 * updateActiveFlag()
 * ==================
 * Werkt de visuele indicatie van de actieve taalknop bij.
 * 
 * Deze functie voegt de CSS class 'active' toe aan de knop die overeenkomt
 * met de huidige taal, en verwijdert deze class van alle andere knoppen.
 * 
 * De 'active' class zorgt voor visuele feedback:
 * - Groene achtergrond met gradient
 * - Subtiele glow-effect (box-shadow)
 * - Lichte border
 * 
 * Dit helpt de gebruiker te zien welke taal momenteel actief is.
 */
function updateActiveFlag() {
    // Haal de huidige taal op
    const lang = getCurrentLanguage();
    
    // Zoek alle taalknop elementen (zowel oude .lang-flag als nieuwe .lang-btn)
    // De comma in de selector betekent "of" - selecteer elementen die aan één
    // van beide selectors voldoen
    document.querySelectorAll('.lang-flag, .lang-btn').forEach(btn => {
        // Verwijder eerst de 'active' class van alle knoppen
        btn.classList.remove('active');
        
        // Controleer of het data-lang attribuut overeenkomt met de huidige taal
        if (btn.getAttribute('data-lang') === lang) {
            // Voeg de 'active' class toe aan deze knop
            btn.classList.add('active');
        }
    });
}


/**
 * DOMContentLoaded Event Listener
 * ================================
 * 
 * Dit is een "event listener" die wacht tot de DOM volledig is geladen
 * voordat de vertalingen worden toegepast.
 * 
 * WAAROM DOMCONTENTLOADED?
 * - De browser laadt HTML van boven naar beneden
 * - JavaScript in de <head> wordt uitgevoerd voordat de <body> is geladen
 * - Als we direct querySelectorAll aanroepen, zijn de elementen nog niet beschikbaar
 * - DOMContentLoaded garandeert dat alle HTML-elementen beschikbaar zijn
 * 
 * VERSCHIL MET 'load' EVENT:
 * - DOMContentLoaded: HTML is geparsed, DOM is beschikbaar
 * - load: Alles is geladen, inclusief afbeeldingen, CSS, etc.
 * 
 * We gebruiken DOMContentLoaded voor snellere uitvoering, aangezien we alleen
 * de DOM-structuur nodig hebben, niet de volledige resources.
 */
document.addEventListener('DOMContentLoaded', function() {
    // Pas alle vertalingen toe op de pagina
    applyTranslations();
    
    // Markeer de juiste taalknop als actief
    updateActiveFlag();
});

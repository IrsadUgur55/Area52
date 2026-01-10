-- =============================================================================
-- AREA52 DATABASE SETUP - MySQL
-- =============================================================================
-- Dit script maakt de benodigde tabellen aan voor de Area52 fiets verhuur app.
-- 
-- INSTRUCTIES:
-- 1. Log in op je MySQL server (phpMyAdmin, MySQL Workbench, of CLI)
-- 2. Maak eerst de database aan: CREATE DATABASE area52;
-- 3. Selecteer de database: USE area52;
-- 4. Voer dit script uit
--
-- Server: 81.173.3.59
-- Database: area52
-- =============================================================================

-- Eerst database aanmaken als deze nog niet bestaat
CREATE DATABASE IF NOT EXISTS area52
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE area52;

-- =============================================================================
-- TABEL: Bikes (Fietsen)
-- =============================================================================
-- Slaat alle fietsen op met hun eigenschappen.
-- BikeType: 'CityBike' of 'ElectricBike'
-- BatteryCapacityWh: Alleen relevant voor ElectricBike (NULL voor CityBike)
-- =============================================================================
CREATE TABLE IF NOT EXISTS Bikes (
    BikeId INT AUTO_INCREMENT PRIMARY KEY,
    BikeType VARCHAR(50) NOT NULL DEFAULT 'CityBike',
    DayPrice DECIMAL(10, 2) NOT NULL,
    PurchaseDate DATE NOT NULL,
    BatteryCapacityWh INT NULL,
    
    -- Metadata velden
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    
    -- Index voor sneller zoeken op type
    INDEX idx_bike_type (BikeType)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =============================================================================
-- TABEL: BikeReservations (Reserveringen)
-- =============================================================================
-- Slaat alle reserveringen op met hun details.
-- OptionsJson: JSON string met geselecteerde opties (AssistanceOption, etc.)
-- =============================================================================
CREATE TABLE IF NOT EXISTS BikeReservations (
    ReservationId INT AUTO_INCREMENT PRIMARY KEY,
    BikeId INT NOT NULL,
    StartDate DATE NOT NULL,
    Days INT NOT NULL DEFAULT 1,
    BikeCount INT NOT NULL DEFAULT 1,
    OptionsJson TEXT NULL,
    TotalPrice DECIMAL(10, 2) NOT NULL,
    
    -- Metadata velden
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    
    -- Foreign key naar Bikes tabel
    FOREIGN KEY (BikeId) REFERENCES Bikes(BikeId) ON DELETE CASCADE,
    
    -- Indexen voor sneller zoeken
    INDEX idx_reservation_bike (BikeId),
    INDEX idx_reservation_date (StartDate)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =============================================================================
-- SEED DATA: Demo fietsen invoegen
-- =============================================================================
-- Dezelfde demo data die nu in de C# code staat
-- =============================================================================
INSERT INTO Bikes (BikeId, BikeType, DayPrice, PurchaseDate, BatteryCapacityWh) VALUES
    (1, 'CityBike', 10.00, '2022-01-01', NULL),
    (2, 'ElectricBike', 25.00, '2023-06-15', 500);

-- =============================================================================
-- VERIFICATIE: Controleer of alles goed is aangemaakt
-- =============================================================================
SELECT 'Tabellen aangemaakt:' AS Status;
SHOW TABLES;

SELECT 'Demo fietsen:' AS Status;
SELECT * FROM Bikes;

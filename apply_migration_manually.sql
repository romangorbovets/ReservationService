-- ============================================
-- SQL скрипт для ручного применения миграции
-- Выполните этот скрипт в pgAdmin, подключившись к базе данных ReservationServiceDb
-- ============================================

-- Убедитесь, что вы подключены к базе данных ReservationServiceDb
-- В pgAdmin: Правый клик на ReservationServiceDb -> Query Tool

-- Этот скрипт создаст все таблицы, индексы и внешние ключи

BEGIN;

-- Создание таблицы Customers
CREATE TABLE IF NOT EXISTS "Customers" (
    "Id" uuid NOT NULL,
    "FirstName" character varying(100) NOT NULL,
    "LastName" character varying(100) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PhoneNumber" character varying(50) NULL,
    "Street" character varying(200) NULL,
    "City" character varying(100) NULL,
    "State" character varying(100) NULL,
    "PostalCode" character varying(20) NULL,
    "Country" character varying(100) NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NULL,
    CONSTRAINT "PK_Customers" PRIMARY KEY ("Id")
);

-- Создание таблицы Restaurants
CREATE TABLE IF NOT EXISTS "Restaurants" (
    "Id" uuid NOT NULL,
    "Name" character varying(200) NOT NULL,
    "Description" character varying(1000) NULL,
    "Street" character varying(200) NOT NULL,
    "City" character varying(100) NOT NULL,
    "State" character varying(100) NULL,
    "PostalCode" character varying(20) NOT NULL,
    "Country" character varying(100) NOT NULL,
    "Email" character varying(255) NOT NULL,
    "PhoneNumber" character varying(50) NULL,
    "TimeZone" character varying(50) NULL,
    "OpeningTime" time NULL,
    "ClosingTime" time NULL,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NULL,
    CONSTRAINT "PK_Restaurants" PRIMARY KEY ("Id")
);

-- Создание таблицы Tables
CREATE TABLE IF NOT EXISTS "Tables" (
    "Id" uuid NOT NULL,
    "RestaurantId" uuid NOT NULL,
    "TableNumber" character varying(50) NOT NULL,
    "Capacity" integer NOT NULL,
    "Location" character varying(200) NULL,
    "IsActive" boolean NOT NULL DEFAULT TRUE,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NULL,
    CONSTRAINT "PK_Tables" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Tables_Restaurants_RestaurantId" FOREIGN KEY ("RestaurantId") REFERENCES "Restaurants" ("Id") ON DELETE RESTRICT
);

-- Создание таблицы Reservations
CREATE TABLE IF NOT EXISTS "Reservations" (
    "Id" uuid NOT NULL,
    "CustomerId" uuid NOT NULL,
    "TableId" uuid NOT NULL,
    "RestaurantId" uuid NOT NULL,
    "StartTime" timestamp with time zone NOT NULL,
    "EndTime" timestamp with time zone NOT NULL,
    "NumberOfGuests" integer NOT NULL,
    "Status" character varying(50) NOT NULL,
    "TotalPriceAmount" numeric(18,2) NOT NULL,
    "TotalPriceCurrency" character varying(10) NOT NULL DEFAULT 'USD',
    "AutoCancellationEnabled" boolean NOT NULL DEFAULT TRUE,
    "AutoCancellationTimeout" interval NULL,
    "SpecialRequests" character varying(1000) NULL,
    "Notes" character varying(2000) NULL,
    "CreatedAt" timestamp with time zone NOT NULL,
    "UpdatedAt" timestamp with time zone NULL,
    "ConfirmedAt" timestamp with time zone NULL,
    "CancelledAt" timestamp with time zone NULL,
    "CancellationReason" character varying(500) NULL,
    "CompletedAt" timestamp with time zone NULL,
    CONSTRAINT "PK_Reservations" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Reservations_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Reservations_Restaurants_RestaurantId" FOREIGN KEY ("RestaurantId") REFERENCES "Restaurants" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Reservations_Tables_TableId" FOREIGN KEY ("TableId") REFERENCES "Tables" ("Id") ON DELETE RESTRICT
);

-- Создание индексов для Customers
CREATE INDEX IF NOT EXISTS "IX_Customers_Reservations_CustomerId" ON "Reservations" ("CustomerId");

-- Создание индексов для Tables
CREATE INDEX IF NOT EXISTS "IX_Tables_RestaurantId" ON "Tables" ("RestaurantId");
CREATE UNIQUE INDEX IF NOT EXISTS "IX_Tables_RestaurantId_TableNumber" ON "Tables" ("RestaurantId", "TableNumber");
CREATE INDEX IF NOT EXISTS "IX_Reservations_TableId" ON "Reservations" ("TableId");

-- Создание индексов для Restaurants
CREATE INDEX IF NOT EXISTS "IX_Reservations_RestaurantId" ON "Reservations" ("RestaurantId");

-- Создание индексов для Reservations
CREATE INDEX IF NOT EXISTS "IX_Reservations_CustomerId" ON "Reservations" ("CustomerId");

-- Создание таблицы истории миграций
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

-- Добавление записи о примененной миграции
INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20251230203106_InitialCreate', '10.0.1')
ON CONFLICT ("MigrationId") DO NOTHING;

COMMIT;

-- Проверка созданных таблиц
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' 
ORDER BY table_name;



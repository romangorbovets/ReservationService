-- ============================================
-- Скрипт для проверки базы данных в pgAdmin
-- Выполните эти запросы в Query Tool pgAdmin
-- ============================================

-- 1. ПРОВЕРКА СУЩЕСТВОВАНИЯ БАЗЫ ДАННЫХ
-- Выполните этот запрос, подключившись к серверу PostgreSQL (не к конкретной БД)
SELECT datname, datcollate, datctype 
FROM pg_database 
WHERE datname = 'ReservationServiceDb';

-- Если база данных существует, подключитесь к ней и выполните следующие запросы:

-- ============================================
-- 2. ПРОВЕРКА ВСЕХ ТАБЛИЦ
-- ============================================
SELECT 
    table_schema,
    table_name,
    table_type
FROM information_schema.tables
WHERE table_schema = 'public'
ORDER BY table_name;

-- Ожидаемый результат: должны быть таблицы:
-- - Customers
-- - Restaurants  
-- - Tables
-- - Reservations
-- - __EFMigrationsHistory

-- ============================================
-- 3. ПРОВЕРКА СТРУКТУРЫ ТАБЛИЦЫ Customers
-- ============================================
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length,
    column_default
FROM information_schema.columns
WHERE table_name = 'Customers'
ORDER BY ordinal_position;

-- ============================================
-- 4. ПРОВЕРКА СТРУКТУРЫ ТАБЛИЦЫ Restaurants
-- ============================================
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length,
    column_default
FROM information_schema.columns
WHERE table_name = 'Restaurants'
ORDER BY ordinal_position;

-- ============================================
-- 5. ПРОВЕРКА СТРУКТУРЫ ТАБЛИЦЫ Tables
-- ============================================
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length,
    column_default
FROM information_schema.columns
WHERE table_name = 'Tables'
ORDER BY ordinal_position;

-- ============================================
-- 6. ПРОВЕРКА СТРУКТУРЫ ТАБЛИЦЫ Reservations
-- ============================================
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length,
    column_default
FROM information_schema.columns
WHERE table_name = 'Reservations'
ORDER BY ordinal_position;

-- ============================================
-- 7. ПРОВЕРКА ПЕРВИЧНЫХ КЛЮЧЕЙ
-- ============================================
SELECT
    tc.table_name, 
    kcu.column_name,
    tc.constraint_name
FROM information_schema.table_constraints AS tc 
JOIN information_schema.key_column_usage AS kcu
  ON tc.constraint_name = kcu.constraint_name
WHERE tc.constraint_type = 'PRIMARY KEY'
  AND tc.table_schema = 'public'
ORDER BY tc.table_name;

-- ============================================
-- 8. ПРОВЕРКА ВНЕШНИХ КЛЮЧЕЙ
-- ============================================
SELECT
    tc.table_name, 
    kcu.column_name, 
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name,
    tc.constraint_name
FROM information_schema.table_constraints AS tc 
JOIN information_schema.key_column_usage AS kcu
  ON tc.constraint_name = kcu.constraint_name
JOIN information_schema.constraint_column_usage AS ccu
  ON ccu.constraint_name = tc.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY'
  AND tc.table_schema = 'public'
ORDER BY tc.table_name;

-- Ожидаемые внешние ключи:
-- - Tables.RestaurantId -> Restaurants.Id
-- - Reservations.CustomerId -> Customers.Id
-- - Reservations.TableId -> Tables.Id
-- - Reservations.RestaurantId -> Restaurants.Id

-- ============================================
-- 9. ПРОВЕРКА ИНДЕКСОВ
-- ============================================
SELECT 
    tablename,
    indexname,
    indexdef
FROM pg_indexes
WHERE schemaname = 'public'
ORDER BY tablename, indexname;

-- ============================================
-- 10. ПРОВЕРКА ПРИМЕНЕННЫХ МИГРАЦИЙ
-- ============================================
SELECT * 
FROM "__EFMigrationsHistory" 
ORDER BY "MigrationId";

-- Должна быть запись: 20251230203106_InitialCreate

-- ============================================
-- 11. ПРОВЕРКА КОЛИЧЕСТВА ЗАПИСЕЙ В ТАБЛИЦАХ
-- ============================================
SELECT 
    'Customers' as table_name, 
    COUNT(*) as row_count 
FROM "Customers"
UNION ALL
SELECT 
    'Restaurants' as table_name, 
    COUNT(*) as row_count 
FROM "Restaurants"
UNION ALL
SELECT 
    'Tables' as table_name, 
    COUNT(*) as row_count 
FROM "Tables"
UNION ALL
SELECT 
    'Reservations' as table_name, 
    COUNT(*) as row_count 
FROM "Reservations";




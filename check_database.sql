-- SQL скрипт для проверки базы данных и таблиц в pgAdmin
-- Выполните эти запросы в Query Tool pgAdmin

-- 1. Проверка существования базы данных
SELECT datname FROM pg_database WHERE datname = 'ReservationServiceDb';

-- 2. Подключитесь к базе данных ReservationServiceDb и выполните следующие запросы:

-- 3. Проверка всех таблиц в базе данных
SELECT 
    table_schema,
    table_name,
    table_type
FROM information_schema.tables
WHERE table_schema = 'public'
ORDER BY table_name;

-- 4. Проверка таблицы Customers
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length
FROM information_schema.columns
WHERE table_name = 'Customers'
ORDER BY ordinal_position;

-- 5. Проверка таблицы Restaurants
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length
FROM information_schema.columns
WHERE table_name = 'Restaurants'
ORDER BY ordinal_position;

-- 6. Проверка таблицы Tables
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length
FROM information_schema.columns
WHERE table_name = 'Tables'
ORDER BY ordinal_position;

-- 7. Проверка таблицы Reservations
SELECT 
    column_name,
    data_type,
    is_nullable,
    character_maximum_length
FROM information_schema.columns
WHERE table_name = 'Reservations'
ORDER BY ordinal_position;

-- 8. Проверка индексов
SELECT 
    tablename,
    indexname,
    indexdef
FROM pg_indexes
WHERE schemaname = 'public'
ORDER BY tablename, indexname;

-- 9. Проверка внешних ключей
SELECT
    tc.table_name, 
    kcu.column_name, 
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name 
FROM information_schema.table_constraints AS tc 
JOIN information_schema.key_column_usage AS kcu
  ON tc.constraint_name = kcu.constraint_name
JOIN information_schema.constraint_column_usage AS ccu
  ON ccu.constraint_name = tc.constraint_name
WHERE tc.constraint_type = 'FOREIGN KEY'
  AND tc.table_schema = 'public'
ORDER BY tc.table_name;

-- 10. Проверка примененных миграций (таблица __EFMigrationsHistory)
SELECT * FROM "__EFMigrationsHistory" ORDER BY "MigrationId";






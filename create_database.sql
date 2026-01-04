-- SQL скрипт для создания базы данных
-- Выполните этот скрипт в pgAdmin, подключившись к серверу PostgreSQL (не к конкретной БД)
-- Или выполните в Query Tool, выбрав базу данных "postgres"

-- Создание базы данных
CREATE DATABASE "ReservationServiceDb"
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'Russian_Russia.1251'
    LC_CTYPE = 'Russian_Russia.1251'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- Комментарий к базе данных
COMMENT ON DATABASE "ReservationServiceDb" IS 'База данных для сервиса бронирования столиков в ресторанах';




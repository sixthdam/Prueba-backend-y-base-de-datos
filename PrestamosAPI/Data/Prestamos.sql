//Base de datos para la gestión de equipos, empleados y préstamos

DROP DATABASE IF EXISTS Prestamos;
CREATE DATABASE Prestamos;
USE Prestamos;

CREATE TABLE IF NOT EXISTS categorias (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS equipos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    serial VARCHAR(255) NOT NULL UNIQUE,
    estado VARCHAR(50) NOT NULL,
    categoria_id INT NOT NULL,
    FOREIGN KEY (categoria_id) REFERENCES categorias(id)
);

CREATE TABLE IF NOT EXISTS empleados (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(255) NOT NULL,
    documento VARCHAR(50) NOT NULL UNIQUE,
    area VARCHAR(100) NOT NULL,
    correo VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS prestamos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    empleado_id INT,
    equipo_id INT,
    fecha_prestamo DATETIME NOT NULL,
    fecha_devolucion DATETIME,
    estado VARCHAR(50) NOT NULL,
    FOREIGN KEY (empleado_id) REFERENCES empleados(id),
    FOREIGN KEY (equipo_id) REFERENCES equipos(id)
);

// Inserción de datos de prueba

INSERT INTO categorias (nombre) VALUES 
('Computo'),
('Audiovisual'),
('Herramientas');

INSERT INTO equipos (nombre, serial, estado, categoria_id) VALUES 
('Laptop Dell', 'SN123456', 'En Mantenimiento', 1),
('Proyector Epson', 'SN654321', 'Prestado', 2),
('Taladro Bosch', 'SN987654', 'Disponible', 3),
('Cámara Canon', 'SN456789', 'Disponible', 2),
('Monitor Samsung', 'SN321654', 'Prestado', 1);

INSERT INTO empleados (nombre, documento, area, correo) VALUES 
('Juan Pérez', '12345678', 'TI', 'jp@ejemplo.com'),
('María Gómez', '87654321', 'Marketing', 'mg@ejemplo.com'),
('Luis Rodríguez', '11223344', 'Ventas', 'lr@ejemplo.com');
create database TestingProject
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'

EXEC sp_MSforeachtable 'DROP TABLE ?'

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Clientes')
	DROP TABLE dbo.Clientes
ELSE
	BEGIN
		CREATE TABLE Clientes (
			ClienteID INT PRIMARY KEY IDENTITY(1,1),
			Identificacion VARCHAR(11) NOT NULL,
			NombreCompleto VARCHAR(70) NOT NULL,
			CorreoElectronico VARCHAR(50) NOT NULL,
			Direccion VARCHAR(100) NOT NULL,
			Telefono VARCHAR(10) NOT NULL,
			Estado VARCHAR(50) NOT NULL,
			Clave VARCHAR(MAX) NOT NULL
		);
	END
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tramites')
	DROP TABLE dbo.Tramites
ELSE
	BEGIN
		CREATE TABLE Tramites (
			TramiteID INT PRIMARY KEY IDENTITY(1,1),
			ClienteID INT NOT NULL,
			Descripcion VARCHAR(MAX) NOT NULL,
			FechaInicio DATE NOT NULL,
			FechaFinalizacion DATE NOT NULL,
			PagoPendiente VARCHAR(5) NOT NULL,
			FormularioPagoCompletado VARCHAR(5) NOT NULL,
			Identificacion VARCHAR(MAX) NOT NULL,
			FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
		);
	END
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Servicios')
	DROP TABLE dbo.Servicios
ELSE
	BEGIN
		CREATE TABLE Servicios (
			ServicioID INT PRIMARY KEY,
			NombreServicio VARCHAR(40) NOT NULL,
			Descripcion VARCHAR(200) NOT NULL,
			Tarifa DECIMAL(10, 2) NOT NULL,
			TipoTarifa VARCHAR(10) NOT NULL
		);
	END
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ServiciosContratados')
	DROP TABLE dbo.ServiciosContratados
ELSE
	BEGIN
		CREATE TABLE ServiciosContratados (
			ServicioContratadoID INT PRIMARY KEY IDENTITY(1,1),
			ServicioID INT NOT NULL,
			ClienteID INT NOT NULL,
			FechaInicio DATE NOT NULL,
			FechaFinalizacion DATE NOT NULL,
			Descripcion VARCHAR(200) NOT NULL,
			Consumo DECIMAL(10, 2) NOT NULL,
			EstadoServicio VARCHAR(10) NOT NULL,
			FOREIGN KEY (ServicioID) REFERENCES Servicios(ServicioID),
			FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
		);
	END
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Pagos')
	DROP TABLE dbo.Pagos
ELSE
	BEGIN
		CREATE TABLE Pagos (
			PagoID INT PRIMARY KEY IDENTITY(1,1),
			ServicioContratadoID INT NOT NULL,
			ClienteID INT NOT NULL,
			Identificacion VARCHAR(MAX) NOT NULL,
			MontoPago DECIMAL(10, 2) NOT NULL,
			FechaPago DATE NOT NULL,
			MetodoPago VARCHAR(50) NOT NULL,
			EstadoPago VARCHAR(20) NOT NULL,
			FOREIGN KEY (ServicioContratadoID) REFERENCES ServiciosContratados(ServicioContratadoID),
			FOREIGN KEY (ClienteID) REFERENCES Clientes(ClienteID)
		);
	END
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Empleados')
	DROP TABLE dbo.Empleados
ELSE
	BEGIN
		CREATE TABLE Empleados (
			EmpleadoID INT PRIMARY KEY IDENTITY(1,1),
			Nombre VARCHAR(255) NOT NULL,
			CorreoElectronico VARCHAR(255) NOT NULL,
			Contraseña VARCHAR(255) NOT NULL,
			Rol VARCHAR(50) NOT NULL,
			Estado VARCHAR(50) NOT NULL,
			UNIQUE (CorreoElectronico)
		);
	END
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Tareas')
	DROP TABLE dbo.Tareas
ELSE
	BEGIN
		CREATE TABLE Tareas (
			TareaID INT PRIMARY KEY IDENTITY(1,1),
			Titulo VARCHAR(255) NOT NULL,
			Descripcion TEXT NOT NULL,
			Prioridad VARCHAR(20) NOT NULL,
			FechaInicio DATE NOT NULL,
			FechaFin DATE NOT NULL,
			EmpleadoID INT NOT NULL,
			Estado VARCHAR(20) NOT NULL,
			FOREIGN KEY (EmpleadoID) REFERENCES Empleados(EmpleadoID)
		);
	END
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Proyectos')
	DROP TABLE dbo.Proyectos
ELSE
	BEGIN
		CREATE TABLE Proyectos (
			NoticiaID INT PRIMARY KEY IDENTITY(1,1),
			TituloNoticia VARCHAR(300) NOT NULL,
			Desarrollo TEXT NOT NULL,
			NivelPrioridad VARCHAR(20) NOT NULL,
			FechaPublicacion DATE NOT NULL,
			Estado VARCHAR(30) NOT NULL
		);
	END

EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'

-- Insert 1
INSERT INTO Clientes (Identificacion, NombreCompleto, CorreoElectronico, Direccion, Telefono, Estado, Clave)
VALUES ('1234567890', 'Juan Perez', 'juan@example.com', '123 Main St', '555-1234', 'Activo', 'password123');

-- Insert 2
INSERT INTO Clientes (Identificacion, NombreCompleto, CorreoElectronico, Direccion, Telefono, Estado, Clave)
VALUES ('9876543210', 'Maria Garcia', 'maria@example.com', '456 Oak St', '555-5678', 'Activo', 'mypass123');

-- Insert 3
INSERT INTO Clientes (Identificacion, NombreCompleto, CorreoElectronico, Direccion, Telefono, Estado, Clave)
VALUES ('1112233444', 'Pedro Ramirez', 'pedro@example.com', '789 Elm St', '555-9876', 'Inactivo', 'securepass');

-- Insert 4
INSERT INTO Clientes (Identificacion, NombreCompleto, CorreoElectronico, Direccion, Telefono, Estado, Clave)
VALUES ('9998887770', 'Ana Lopez', 'ana@example.com', '456 Birch St', '555-5432', 'Activo', 'mysecretpass');

-- Insert 5
INSERT INTO Clientes (Identificacion, NombreCompleto, CorreoElectronico, Direccion, Telefono, Estado, Clave)
VALUES ('5556667770', 'Carlos Rodriguez', 'carlos@example.com', '654 Pine St', '555-8765', 'Activo', 'topsecret');


-- Insert 1
INSERT INTO Tramites (ClienteID, Descripcion, FechaInicio, FechaFinalizacion, PagoPendiente, FormularioPagoCompletado, Identificacion)
VALUES (1, 'Solicitud de permiso de construcción', '2023-01-10', '2023-01-15', 'No', 'Sí', 'ABC123');

-- Insert 2
INSERT INTO Tramites (ClienteID, Descripcion, FechaInicio, FechaFinalizacion, PagoPendiente, FormularioPagoCompletado, Identificacion)
VALUES (2, 'Renovación de licencia de conducir', '2023-02-05', '2023-02-10', 'No', 'Sí', 'XYZ456');

-- Insert 3
INSERT INTO Tramites (ClienteID, Descripcion, FechaInicio, FechaFinalizacion, PagoPendiente, FormularioPagoCompletado, Identificacion)
VALUES (1, 'Solicitud de pasaporte', '2023-03-20', '2023-03-25', 'No', 'No', 'DEF789');

-- Insert 4
INSERT INTO Tramites (ClienteID, Descripcion, FechaInicio, FechaFinalizacion, PagoPendiente, FormularioPagoCompletado, Identificacion)
VALUES (3, 'Inscripción de nacimiento', '2023-04-15', '2023-04-20', 'Sí', 'No', 'GHI101');

-- Insert 5
INSERT INTO Tramites (ClienteID, Descripcion, FechaInicio, FechaFinalizacion, PagoPendiente, FormularioPagoCompletado, Identificacion)
VALUES (4, 'Solicitud de visa', '2023-05-10', '2023-05-15', 'No', 'Sí', 'JKL202');


-- Insert 1
INSERT INTO Servicios (ServicioID, NombreServicio, Descripcion, Tarifa, TipoTarifa)
VALUES (1, 'Servicio de Agua', 'Suministro de agua potable', 25.00, 'Mensual');

-- Insert 2
INSERT INTO Servicios (ServicioID, NombreServicio, Descripcion, Tarifa, TipoTarifa)
VALUES (2, 'Servicio de Electricidad', 'Suministro de energía eléctrica', 50.00, 'Mensual');

-- Insert 3
INSERT INTO Servicios (ServicioID, NombreServicio, Descripcion, Tarifa, TipoTarifa)
VALUES (3, 'Servicio de Gas', 'Suministro de gas natural', 40.00, 'Mensual');

-- Insert 4
INSERT INTO Servicios (ServicioID, NombreServicio, Descripcion, Tarifa, TipoTarifa)
VALUES (4, 'Servicio de Internet', 'Conexión a Internet de alta velocidad', 30.00, 'Mensual');

-- Insert 5
INSERT INTO Servicios (ServicioID, NombreServicio, Descripcion, Tarifa, TipoTarifa)
VALUES (5, 'Servicio de Teléfono', 'Servicio telefónico fijo', 20.00, 'Mensual');


-- Insert 1
INSERT INTO ServiciosContratados (ServicioID, ClienteID, FechaInicio, FechaFinalizacion, Descripcion, Consumo, EstadoServicio)
VALUES (1, 1, '2023-01-01', '2023-01-31', 'Servicio de Agua', 12.5, 'Activo');

-- Insert 2
INSERT INTO ServiciosContratados (ServicioID, ClienteID, FechaInicio, FechaFinalizacion, Descripcion, Consumo, EstadoServicio)
VALUES (2, 2, '2023-02-01', '2023-02-28', 'Servicio de Electricidad', 75.0, 'Activo');

-- Insert 3
INSERT INTO ServiciosContratados (ServicioID, ClienteID, FechaInicio, FechaFinalizacion, Descripcion, Consumo, EstadoServicio)
VALUES (3, 3, '2023-03-01', '2023-03-31', 'Servicio de Gas', 25.0, 'Inactivo');

-- Insert 4
INSERT INTO ServiciosContratados (ServicioID, ClienteID, FechaInicio, FechaFinalizacion, Descripcion, Consumo, EstadoServicio)
VALUES (4, 4, '2023-04-01', '2023-04-30', 'Servicio de Internet', 50.0, 'Activo');

-- Insert 5
INSERT INTO ServiciosContratados (ServicioID, ClienteID, FechaInicio, FechaFinalizacion, Descripcion, Consumo, EstadoServicio)
VALUES (5, 5, '2023-05-01', '2023-05-31', 'Servicio de Teléfono', 30.0, 'Activo');


-- Insert 1
INSERT INTO Pagos (ServicioContratadoID, ClienteID, Identificacion, MontoPago, FechaPago, MetodoPago, EstadoPago)
VALUES (1, 1, 'ABC123', 30.00, '2023-01-15', 'Tarjeta de Crédito', 'Pagado');

-- Insert 2
INSERT INTO Pagos (ServicioContratadoID, ClienteID, Identificacion, MontoPago, FechaPago, MetodoPago, EstadoPago)
VALUES (2, 2, 'XYZ456', 50.00, '2023-02-10', 'Transferencia Bancaria', 'Pagado');

-- Insert 3
INSERT INTO Pagos (ServicioContratadoID, ClienteID, Identificacion, MontoPago, FechaPago, MetodoPago, EstadoPago)
VALUES (3, 3, 'DEF789', 40.00, '2023-03-25', 'Efectivo', 'Pendiente');

-- Insert 4
INSERT INTO Pagos (ServicioContratadoID, ClienteID, Identificacion, MontoPago, FechaPago, MetodoPago, EstadoPago)
VALUES (4, 4, 'GHI101', 60.00, '2023-04-20', 'Tarjeta de Débito', 'Pagado');

-- Insert 5
INSERT INTO Pagos (ServicioContratadoID, ClienteID, Identificacion, MontoPago, FechaPago, MetodoPago, EstadoPago)
VALUES (5, 5, 'JKL202', 40.00, '2023-05-15', 'Cheque', 'Pagado');



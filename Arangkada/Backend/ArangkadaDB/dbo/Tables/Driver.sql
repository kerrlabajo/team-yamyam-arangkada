CREATE TABLE [dbo].[Driver]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[FullName] NVARCHAR(100) NOT NULL,
	[Address] NVARCHAR(100) NOT NULL,
	[ContactNumber] NVARCHAR(20) NOT NULL,
	[LicenseNumber] NVARCHAR(50) NOT NULL,
	[ExpirationDate] NVARCHAR(20) NOT NULL,
	[DLCodes] NVARCHAR(100) NOT NULL,
	[Category] NVARCHAR(20) NOT NULL,
	[OperatorId] INT NOT NULL,
	[VehicleId] INT,
	CONSTRAINT [FK_DriverOperator] FOREIGN KEY ([OperatorId]) REFERENCES [dbo].[Operator]([Id]),
	CONSTRAINT [FK_DriverVehicle] FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicle]([Id])
)
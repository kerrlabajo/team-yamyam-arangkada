CREATE TABLE [dbo].[Vehicle]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[CRNumber] NVARCHAR(50) NOT NULL,
	[PlateNumber] NVARCHAR(50) NOT NULL,
	[BodyType] NVARCHAR(50) NOT NULL,
	[Make] NVARCHAR(50) NOT NULL,
	[DistinctionLabel] NVARCHAR(50) NOT NULL,
	[RentFee] FLOAT NOT NULL,
	[RentStatus] BIT NOT NULL,
	[OperatorId] INT NOT NULL,
	CONSTRAINT [FK_VehicleOperator] FOREIGN KEY ([OperatorId]) REFERENCES [dbo].[Operator]([Id]),
)
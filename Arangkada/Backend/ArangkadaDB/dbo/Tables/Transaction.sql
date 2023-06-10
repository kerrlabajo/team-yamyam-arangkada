CREATE TABLE [dbo].[Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[Amount] FLOAT,
	[Status] NVARCHAR(20),
	[StartDate] NVARCHAR(20) NOT NULL,
	[EndDate] NVARCHAR(20),
	[OperatorId] INT NOT NULL,
	[DriverId] INT NOT NULL,
	CONSTRAINT [FK_TransactionDriver] FOREIGN KEY (DriverId) REFERENCES [dbo].[Driver]([Id]),
	CONSTRAINT [FK_TransactionOperator] FOREIGN KEY ([OperatorId]) REFERENCES [dbo].[Operator]([Id])
)

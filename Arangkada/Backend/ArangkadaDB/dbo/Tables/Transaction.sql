CREATE TABLE [dbo].[Transaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY (1,1),
	[Amount] FLOAT NOT NULL,
	[Date] NVARCHAR(20) NOT NULL,
	[OperatorId] INT NOT NULL,
	[DriverId] INT NOT NULL,
	CONSTRAINT [FK_TransactionDriver] FOREIGN KEY (DriverId) REFERENCES [dbo].[Driver]([Id]),
	CONSTRAINT [FK_TransactionOperator] FOREIGN KEY ([OperatorId]) REFERENCES [dbo].[Operator]([Id])
)

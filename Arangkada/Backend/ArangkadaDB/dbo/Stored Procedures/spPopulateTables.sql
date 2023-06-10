CREATE PROCEDURE spPopulateTables
AS
BEGIN
    -- Drop all foreign key constraints and truncate Transaction table because it is the grandchild table.
    IF EXISTS(SELECT * FROM [dbo].[Transaction])
	    ALTER TABLE [dbo].[Transaction]
	    DROP CONSTRAINT IF EXISTS FK_TransactionOperator

        ALTER TABLE [dbo].[Transaction]
	    DROP CONSTRAINT IF EXISTS FK_TransactionDriver

    IF EXISTS(SELECT * FROM [dbo].[Driver])
	    ALTER TABLE [dbo].[Driver]
	    DROP CONSTRAINT IF EXISTS FK_DriverVehicle

        ALTER TABLE [dbo].[Driver]
        DROP CONSTRAINT IF EXISTS FK_DriverOperator
        TRUNCATE TABLE [dbo].[Transaction]

    -- Truncate Driver, Vehicle, and Operator tables after dropping all foreign key constraints.
    IF EXISTS(SELECT * FROM [dbo].[Driver])
		TRUNCATE TABLE [dbo].[Driver]   

    IF EXISTS(SELECT * FROM [dbo].[Vehicle])
        TRUNCATE TABLE [dbo].[Vehicle];

    IF EXISTS(SELECT * FROM [dbo].[Operator])
        TRUNCATE TABLE [dbo].[Operator];

    -- Insert sample data into Operator table
    SET IDENTITY_INSERT [dbo].[Operator] ON
    INSERT INTO [dbo].[Operator] ([Id], [FullName], [Username], [Password], [Email], [VerificationStatus], [VerificationCode])
    VALUES
        (1, 'Operator 1', 'operator1', 'pass1', 'operator1@example.com', 1, null),
        (2, 'Operator 2', 'operator2', 'pass2', 'operator2@example.com', 1, null),
        (3, 'Operator 3', 'operator3', 'pass3', 'operator3@example.com', 1, null),
        (4, 'Operator 4', 'operator4', 'pass4', 'operator4@example.com', 1, null),
        (5, 'Operator 5', 'operator5', 'pass5', 'operator5@example.com', 1, null);
    SET IDENTITY_INSERT [dbo].[Operator] OFF

    -- Insert sample data into Vehicle table
    SET IDENTITY_INSERT [dbo].[Vehicle] ON
    INSERT INTO [dbo].[Vehicle] ([Id], [CRNumber], [PlateNumber], [BodyType], [Make], [DistinctionLabel], [RentFee], [RentStatus], [OperatorId])
    VALUES
        (1, 'CR0001', 'AAA1234', 'Sedan', 'Toyota', 'Black-White', 1000.00, 0, 1),
        (2, 'CR0002', 'BBB5678', 'SUV', 'Honda', 'Green-White', 1500.00, 1, 2),
        (3, 'CR0003', 'CCC9012', 'Truck', 'Ford', 'Black-Red', 2000.00, 0, 1),
        (4, 'CR0004', 'DDD3456', 'Van', 'Nissan', 'Black-Green', 1800.00, 1, 2),
        (5, 'CR0005', 'EEE7890', 'Sports', 'Mazda', 'Brown-White', 3000.00, 0, 3),
        (6, 'CR0006', 'FFF1234', 'Sedan', 'Hyundai', 'Blue-Red', 1050.00, 0, 4),
        (7, 'CR0007', 'GGG5678', 'SUV', 'Mitsubishi', 'Blue-White', 1550.00, 1, 3),
        (8, 'CR0008', 'HHHH9012', 'Truck', 'Lexus', 'Violet-White', 2540.00, 0, 4),
        (9, 'CR0009', 'III456', 'Van', 'Toyota', 'Orange-Red', 1950.00, 1, 5),
        (10, 'CR0010', 'JJJ7890', 'Sports', 'Nissan', 'Orange-White', 3250.00, 0, 5);
    SET IDENTITY_INSERT [dbo].[Vehicle] OFF

    -- Insert sample data into Driver table
    SET IDENTITY_INSERT [dbo].[Driver] ON
    INSERT INTO [dbo].[Driver] ([Id], [FullName], [Address], [ContactNumber], 
    [LicenseNumber], [ExpirationDate], [DLCodes], [Category], [OperatorId], [VehicleId])
    VALUES
        (1, 'John Smith', '123 Main St', '123-456-7890', 'L123456', '2025-01-01', 'A,B,C', 'Extra', 1, null),
        (2, 'Jane Doe', '456 Oak St', '234-567-8901', 'L234567', '2024-12-31', 'A,B', 'Extra', 1, null),
        (3, 'Michael Johnson', '789 Pine St', '345-678-9012', 'L345678', '2026-05-15', 'A,C,D', 'Primary', 2,2),
        (4, 'Emily Brown', '321 Elm St', '456-789-0123', 'L456789', '2027-07-30', 'B,C', 'Primary', 2,4),
        (5, 'Daniel Williams', '654 Maple St', '567-890-1234', 'L567890', '2023-11-25', 'A,D', 'Extra', 3, null),
        (6, 'Adrian Barcelona', '789 Jade St', '953-751-8436', 'L906592', '2022-012-13', 'A,B,C', 'Primary', 3,7),
        (7, 'Neil Paras', '852 Strawberry St', '548-956-8451', 'L145635', '2027-06-01', 'A,B', 'Extra', 4, null),
        (8, 'Lebrong James', '588 Lakers St', '626-194-5484', 'L997955', '2023-04-04', 'A,C,D', 'Extra', 4, null),
        (9, 'Emily Pink', '321 Pink St', '634-516-4868', 'L456789', '2027-07-30', 'B,C', 'Primary', 5, 9),
        (10,'Steph Curry Concentrate', '768 Golden St.', '215-848-6254', 'L123958', '2025-05-19', 'A,D', 'Extra', 5, null),
        (11,'Steph Curry Powder', '768 Golden St.', '215-848-6254', 'L123958', '2025-05-19', 'A,D', 'Extra', 1, null),
        (12,'Giannis Atatakoumpo', '429 Milwaukee St.', '953-858-8484', 'L612822', '2023-09-05', 'A,D', 'Extra', 3, null);

    SET IDENTITY_INSERT [dbo].[Driver] OFF

    -- Insert sample data into Transaction table
    SET IDENTITY_INSERT [dbo].[Transaction] ON
    INSERT INTO [dbo].[Transaction] ([Id], [Amount], [Status], [StartDate], [EndDate], [OperatorId], [DriverId])
    VALUES
        (1, 1000.00, 'Paid', '2023-05-01', '2023-05-02', 1, 1),
        (2, null, 'Pending', '2023-05-02', null, 1, 2),
        (3, null, 'Pending', '2023-05-03', null, 3, 5),
        (4, 1800.00, 'Paid', '2023-05-04', '2023-05-05', 4, 7),
        (5, null, 'Pending', '2023-05-05', null, 4, 8),
        (6, 1200.00, 'Paid', '2023-05-06', '2023-05-07', 5, 10);    
   SET IDENTITY_INSERT [dbo].[Transaction] OFF
END;

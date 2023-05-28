CREATE PROCEDURE [dbo].[spGetAllDriver]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT d.ID, d.FullName, d.Address, d.ContactNumber, d.LicenseNumber, d.ExpirationDate, d.DLCodes,
        o.FullName AS OperatorName, v.PlateNumber AS VehiclePlateNumber
    FROM Driver d
    LEFT JOIN Operator o ON d.OperatorId = o.ID
    LEFT JOIN Vehicle v ON d.VehicleId = v.ID
    GROUP BY d.ID, o.FullName, v.PlateNumber, d.FullName, d.Address, d.ContactNumber, d.LicenseNumber, d.ExpirationDate, d.DLCodes;
END

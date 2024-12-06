-- PROCEDURE TO STORE A NEW BILL.
CREATE PROCEDURE pr_InsertBill
    @Title NVARCHAR(255),
    @TotalAmount FLOAT,
    @UserID NVARCHAR(MAX), -- Bill's Owner
    @PaymentsJson NVARCHAR(MAX) -- list of payments as json
AS
BEGIN
    BEGIN TRY
BEGIN TRANSACTION;

        -- Insert into Bills table
        INSERT INTO Bills (Title, TotalAmount, OwnerID)
        VALUES (@Title, @TotalAmount, @UserID);

        -- Get the newly generated Bill ID
        DECLARE @BillID INT = SCOPE_IDENTITY();

        -- Parse the JSON into a table variable
        DECLARE @Payments TABLE
        (
            Amount FLOAT,
            IsPaid BIT,
            UserId NVARCHAR(450)
        );

		-- Submit the payments
        INSERT INTO @Payments (Amount, IsPaid, UserId)
        SELECT Amount, IsPaid, UserId
        FROM OPENJSON(@PaymentsJson) -- we read (Deserialize) the json
        WITH (
            Amount FLOAT,
            IsPaid BIT,
            UserId NVARCHAR(450)
        );

		-- Insert payments into the Payments table
        INSERT INTO Payments (BillID, Amount, IsPaid, UserId)
        SELECT @BillID, Amount, IsPaid, UserId
        FROM @Payments;

        COMMIT TRANSACTION;
        -- Return the inserted Bill and Payments
        SELECT 
            b.Id AS BillID,
            b.Title,
            b.TotalAmount,
            b.OwnerID,
            p.Id AS PaymentID,
            p.Amount,
            p.IsPaid,
            p.UserId AS PaymentUserID,
			u.Id AS UserID,
			u.Email AS UserEmail,
			u.Name AS UserName,
			u.UserName AS UserUserName,
			u.PhoneNumber AS UserPhoneNumber
        FROM Bills b
        INNER JOIN Payments p ON b.Id = p.BillID
		INNER JOIN AspNetUsers u On u.Id = b.OwnerID
        WHERE b.Id = @BillID;

    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
go
-- PROCEDURE TO DELETE A BILL AND IT'S PAYMENTS.
CREATE PROCEDURE pr_DeleteBill @BILLID INT
AS
    BEGIN
        DELETE FROM Bills WHERE Id = @BILLID;
    END
GO

-- THE PROCEDURE TO UPDATE A BILL'S INFO.
CREATE PROCEDURE pr_UpdateBill @BILLID INT, @Title nvarchar(255), @TotalAmount FLOAT 
AS
    BEGIN
        UPDATE Bills
        SET
        Title = COALESCE(@Title, Title),
        TotalAmount = COALESCE(@TotalAmount, TotalAmount)
        WHERE Id = @BILLID;
    END
GO

CREATE PROCEDURE pr_GetUserByID @VarID NVARCHAR(50)
AS
	BEGIN
		SELECT * FROM dbo.AspNetUsers WHERE Id = @VarID;
	END
GO

CREATE PROCEDURE pr_GetUserByUsername @VarUsername NVARCHAR(max)
AS
	BEGIN
		SELECT * FROM dbo.AspNetUsers WHERE UserName = @VarUsername;
	END
GO

CREATE PROCEDURE pr_GetUsersByUsername @Usernames NVARCHAR(MAX)
AS
	BEGIN
		SELECT * FROM DBO.AspNetUsers WHERE UserName IN (SELECT value FROM string_split(@Usernames, ','));
	END
GO

CREATE PROCEDURE pr_UpdateUser 
@UserID INT, 
@Username NVARCHAR(32) = NULL,
@Name NVARCHAR(64) = NULL,
@Email NVARCHAR(100) = NULL,
@PhoneNumber NVARCHAR(14) = NULL
AS
	BEGIN
		UPDATE AspNetUsers 
		SET
		UserName = COALESCE(@Username, UserName),
		Name = COALESCE(@NAME, Name),
		Email = COALESCE(@Email, Email),
		PhoneNumber = COALESCE(@PhoneNumber, PhoneNumber) 
		WHERE Id = @UserID;
	END
GO


CREATE PROCEDURE pr_GetUnpaidBillsWithPaymentsForUser
    @UserID NVARCHAR(450) 
AS
BEGIN
    SELECT 
        b.Id AS Id, -- Bill ID
        b.Title AS Title, -- Bill Title
        b.TotalAmount AS TotalAmount, -- Bill Total Amount
        b.UserId AS BillUserID, -- The User who created the bill
        p.Id AS PaymentId, -- Payment ID
        p.Amount AS PaymentAmount, -- Payment Amount
        p.IsPaid AS PaymentIsPaid, -- Is the payment paid?
        p.UserId AS PaymentUserID, -- The User who made this payment
        p.BillId AS PaymentBillID -- Link to the bill
    FROM Payments p
    INNER JOIN Bills b ON p.BillId = b.Id
    WHERE p.UserId = @UserID AND p.IsPaid = 0; -- Filter unpaid payments for the user
END;
GO
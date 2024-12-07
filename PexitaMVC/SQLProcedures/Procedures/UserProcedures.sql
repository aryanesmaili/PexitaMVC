CREATE PROCEDURE pr_UpdateUser 
@UserID NVARCHAR(MAX), 
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
    @UserID NVARCHAR(Max) 
AS
BEGIN
    SELECT 
        b.Id AS Id, -- Bill ID
        b.Title AS Title, -- Bill Title
        b.TotalAmount AS TotalAmount, -- Bill Total Amount
        b.OwnerID AS BillUserID, -- The User who created the bill
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
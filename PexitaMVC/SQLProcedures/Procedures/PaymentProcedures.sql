CREATE PROCEDURE pr_UpdatePayment 
@PAYMENTID INT, 
@AMOUNT FLOAT, 
@ISPAID BIT
AS
	BEGIN
		UPDATE Payments
		SET
		Amount = COALESCE(@AMOUNT, Amount),
		IsPaid = COALESCE(@ISPAID, IsPaid)
		WHERE Id = @PAYMENTID;
	END
GO
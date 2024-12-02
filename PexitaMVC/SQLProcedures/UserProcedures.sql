CREATE PROCEDURE pr_GetUserByID @VarID nVarChar(50)
as
begin
Select * from dbo.AspNetUsers Where Id = @VarID;
end
go

CREATE PROCEDURE pr_GetUserByUsername @VarUsername nVarchar(max)
as
begin
Select * from dbo.AspNetUsers where UserName = @VarUsername;
end
go

CREATE PROCEDURE pr_GetUsersByUsername @Usernames NVARCHAR(MAX)
AS
BEGIN
SELECT * FROM DBO.AspNetUsers WHERE UserName IN (SELECT value FROM string_split(@Usernames, ','));
end
go

CREATE PROCEDURE pr_UpdateUser 
@UserID int, 
@Username nVarchar(32) = NULL,
@Name nVarchar(64) = NULL,
@Email nVarchar(100) = NULL,
@PhoneNumber NVARCHAR(14) = NULL
as
begin
UPDATE AspNetUsers 
SET
UserName = COALESCE(@Username, UserName),
Name = COALESCE(@NAME, Name),
Email = COALESCE(@Email, Email),
PhoneNumber = COALESCE(@PhoneNumber, PhoneNumber) 
WHERE Id = @UserID;
end
go

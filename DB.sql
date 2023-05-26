/*
CREATE DATABASE BankAccount;

GO

USE BankAccount;

GO

CREATE TABLE Account(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Balance DECIMAL(12,0) NOT NULL
);
GO
*/
/*
DROP PROCEDURE AddAccount;
*/

/*
CREATE PROCEDURE AddAccount
@Name VARCHAR(50),
@Balance DECIMAL(12,0)
AS
BEGIN
   INSERT INTO Account (Name, Balance) VALUES (@Name, @Balance);
END;
*/


/*CREATE PROCEDURE DeleteAccount
@Id INT
AS
BEGIN
   DELETE Account WHERE Account.Id = @Id
END;*/


/*INSERT INTO dbo.Account (Name, Balance) VALUES ("Ashor", 150);
GO*/

/*use BankAccount;

EXEC AddAccount @Name='Ayi' ,@Balance=1500;
*/


USE BankAccount;


SELECT * FROM Account;

GO

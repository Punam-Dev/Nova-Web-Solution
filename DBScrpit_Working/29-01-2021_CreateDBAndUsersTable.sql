Create DataBase DotIndiaPvtLtd

--drop table Users
Create Table Users
(
	UserID Char(36) Primary Key Not Null,
	UserName nVarChar(128),
	UserRoles nVarChar(512),
	Password nVarChar(128),
	FirstName nVarChar(256) Not Null,
	LastName nVarChar(256) Not Null,
	Email nVarChar(256) Not Null,
	Phone nVarChar(15) Not Null,
	Address nVarChar(2048),
	DOB DateTime,
	CallerName nVarChar(512),
	ActivationDate DateTime,
	Status Bit,
	WorkStatus Bit,
	OTP int,
	DigitalSignPath nVarChar(2048),
	AgreementPath nVarChar(2048),
	MaxFormsCount Int Not Null Default 698,
	IsActive Bit Not Null Default 1,
	UserCreatedByUserID Char(36),
	UserCreatedDate DateTime Not Null,
	UserUpdatedByUserID Char(36),
	UserUpdatedDate DateTime
)

Go

Insert Into Users Values('07cfcf51-188c-4dca-a7c6-a3a0802124d3','admin','Admin','123','Admin', '', 'admin@gmail.com','9090909090', Null, GETDATE(), 
'Admin', Null, 1, 1, Null, Null, Null, '07cfcf51-188c-4dca-a7c6-a3a0802124d3', GETDATE(), Null,Null)

--Truncate Table Users
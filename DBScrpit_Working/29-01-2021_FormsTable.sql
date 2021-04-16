--Drop Table Forms

Create Table Forms(
	FormsID BigInt Primary Key Identity(1,1) Not Null,
	FirstName nVarChar(128),
	LastName nVarChar(128),
	Email nVarChar(512),
	SSN nVarChar(128),
	Phone nVarChar(15),
	BankName nVarChar(128),
	AccountNo nVarChar(32),
	LoanAmount Decimal(19,2),
	Address nVarChar(2048),
	City nVarChar(64),
	State nVarChar(64),
	Zip nVarChar(16),
	DOB Date,
	LicenceNo nVarChar(64),
	LicenceState nVarChar(64),
	IP nVarChar(64),
	FormIsSubmit Bit,
	FormNo Int,
	FormImagePath nVarChar(500),
	FormsCreatedByUserID Char(36),
	FormsCreatedDate DateTime,
	FormsUpdatedByUserID Char(36),
	FormsUpdatedDate DateTime,
	
	Constraint FK_Forms_Users_UserCreatedByUserID Foreign Key(FormsCreatedByUserID) References Users(UserID),
	Constraint FK_Forms_Users_UserUpdatedByUserID Foreign Key(FormsUpdatedByUserID) References Users(UserID),
)

--Truncate table forms

--Select * from forms

Go

ALTER TABLE novaweb.Forms
ADD CONSTRAINT UQ_Forms_FormNo_FormsCreatedByUserID UNIQUE (FormNo, FormsCreatedByUserID)
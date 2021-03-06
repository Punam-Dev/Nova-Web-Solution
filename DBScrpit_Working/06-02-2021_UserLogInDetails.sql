Create Table UserLogInDetails
(	
	UserLogInDetailsID BigInt Primary Key Identity(1,1) Not Null,
	UserID Char(36),
	UserIP nVarChar(2048),
	CreatedDate DateTime,
	IsLogIn Bit,
	Constraint FK_UserLogInDetails_Users Foreign Key (UserID) References Users(UserID)
)


--Drop table UserLogInDetails
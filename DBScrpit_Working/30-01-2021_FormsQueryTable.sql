Create Table FormQuery
(
	FormQueryID BigInt Primary Key Identity(1,1) Not Null,
	FormQueryText nVarChar(512) Not Null,
	FormQueryStatus nVarChar(256),
	FormQueryCreatedByUserID Char(36),
	FormQueryCreatedDate DateTime,
	FormQueryUpdatedByUserID Char(36),
	FormQueryUpdatedDate DateTime,
	
	Constraint FK_FormQuery_Users_UserCreatedByUserID Foreign Key(FormQueryCreatedByUserID) References Users(UserID),
	Constraint FK_FormQuery_Users_UserUpdatedByUserID Foreign Key(FormQueryUpdatedByUserID) References Users(UserID),
)

--Drop table FormQuery
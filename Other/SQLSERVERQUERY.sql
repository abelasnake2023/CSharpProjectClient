Create database CSharpProject;
use CSharpProject;



Create table Users (
	uId int primary key identity not null,
	aNum varchar(13) not null,
	uPNum varchar(10) not null,
	uName varchar(50) not null,
	uPassword varchar(50) not null,
	uPhoto varbinary(MAX),
	mId int,
	FOREIGN KEY (mId) REFERENCES Manager(mId),
	FOREIGN KEY (aNum) REFERENCES Account(aNum)
);



Create table Manager ( -- You need to add more on manager later
	mId int primary key identity not null,
	mFName varchar(50) not null,
	mLName varchar(50) not null,
    mUName varchar(50) Not null,
    mPassword varchar(50) Not null,
	mPhoto varbinary(MAX) not null
);



Create table Account (
	aNum varchar(13) primary key not null,
	aFName varchar(50) not null,
	aLName varchar(50) not null,
	aBalance Decimal(12, 4),
	aType varchar(8),
	uId int default null
);



Alter Procedure CreateUsers
@accNum varchar, @pNum varchar, @photo varbinary,
@username varchar, @password varchar, @mId int
As 
Begin
	Insert Into Users Values (
		@accNum, @pNum, @username, @password, @photo, @mId
	);
End;



Alter Procedure CreateManager 
@fName varchar, @lName varchar, @photo varbinary
As
Begin
	Insert Into Manager Values (
		@fName, @lName, @photo
	);
End;



Alter Procedure CreateAccount -- not related to Users
@aNum varchar, @fName varchar, @lName varchar, @balance Decimal,
@type varchar
As
Begin
	Insert Into Account Values (
		@aNum, @fName, @lName, @balance, @type, default
	);
End;



Create Function SearchManager 
(
	@username varchar(50),
	@password varchar(50)
)
Returns @search Table
(
	found bit,
	reason varchar(20)
)
As 
Begin 
	Declare @mId int;
	Declare @passSearched varchar(50);

	SET @mId = (
	   SELECT mId
	   FROM Manager
	   WHERE mUName = LTRIM(RTRIM(@username))
	);
		
	If(@mId > 0) 
	Begin
		set @passSearched = (
			Select mPassword From
			Manager where mId = @mId
		);
		
		If(@password = @passSearched) 
		Begin
		    Insert into @search values (1, 'password = username');
			return;
		End;
		Else
		Begin
			Insert into @search values (0, 'password != username');
			return;
		End;
	End;

	Insert into @search values (0, 'Invalid username');
	return;
End;
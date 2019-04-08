USE [master];
GO
BEGIN TRY
CREATE DATABASE DoorToDoor;
END TRY
BEGIN CATCH
END CATCH
GO
USE DoorToDoor;
GO

BEGIN TRANSACTION
	BEGIN TRY
		
		CREATE TABLE Roles (
			id			int				IDENTITY(1,1) NOT NULL,
			[name]		varchar(50)		NOT NULL,
			CONSTRAINT PK_Roles PRIMARY KEY ([id])
		);

		INSERT INTO Roles ([name])
		VALUES ('Admin'), ('Salesperson');

		CREATE TABLE Users (
			id					int				IDENTITY(1,1) NOT NULL,
			firstName			varchar(50)		NOT NULL,
			lastName			varchar(50)		NOT NULL,
			username			varchar(50)		NOT NULL,
			emailAddress		varchar(100)	NOT NULL,
			[hash]				varchar(50)		NOT NULL,
			salt				varchar(50)		NOT NULL,
			roleID				int				NOT NULL,
			CONSTRAINT PK_Users PRIMARY KEY (id),
			CONSTRAINT FK_Roles FOREIGN KEY (roleID) REFERENCES Roles(id)
		);

		CREATE TABLE Admin_Saleperson (
			adminID				int		NOT NULL,
			salespersonID		int		NOT NULL,
			CONSTRAINT PK_Admin_Salesperson PRIMARY KEY (adminID, salespersonID),
			CONSTRAINT FK_Users_Admin FOREIGN KEY (adminID) REFERENCES Users(id),
			CONSTRAINT FK_Users_Salesperson FOREIGN KEY (salespersonID) REFERENCES Users(id)
		);

		CREATE TABLE House_Status (
			id			int				IDENTITY(1,1) NOT NULL,
			[status]	varchar(20)		NOT NULL,
			CONSTRAINT PK_House_Status PRIMARY KEY (id)
		);
		
		CREATE TABLE Houses (
			id					int				IDENTITY(1,1) NOT NULL,
			street				varchar(50)		NOT NULL,
			city				varchar(50)		NOT NULL,
			district			varchar(50)		NOT NULL,
			zipCode				varchar(10)		NOT NULL,
			country				varchar(50)		NOT NULL,
			adminID				int				NOT NULL,
			salespersonID		int				NOT NULL,
			statusID			int				NOT NULL,
			CONSTRAINT PK_Houses PRIMARY KEY (id),
			CONSTRAINT FK_Users_Admin_House FOREIGN KEY (adminID) REFERENCES Users(id),
			CONSTRAINT FK_Users_Salesperson_House FOREIGN KEY (salespersonID) REFERENCES Users(id),
			CONSTRAINT FK_House_Status FOREIGN KEY (statusID) REFERENCES House_Status(id)
		);

		CREATE TABLE Houses_Notes (
			id				int					IDENTITY(1,1) NOT NULL,
			houseID			int					NOT NULL,
			note			varchar(255)		NOT NULL,
			CONSTRAINT PK_Houses_Notes PRIMARY KEY (id),
			CONSTRAINT FK_Houses FOREIGN KEY (houseID) REFERENCES Houses(id)
		);


		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH;
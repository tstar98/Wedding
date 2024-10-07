CREATE TABLE [dbo].[Guests]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[InvitationCode] CHAR(6),
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[Phone] VARCHAR(10) NULL,
	[Email] VARCHAR(50) NULL,
	[DietaryRestrictions] NVARCHAR(250),
	[DietaryRestrictionsUpdated] DATETIME,
	[RsvpStatus] BIT NOT NULL DEFAULT 0,
	[RsvpDate] DATETIME NULL
)

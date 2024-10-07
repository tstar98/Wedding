CREATE FUNCTION [dbo].[fnGetGuestsByInvitationCode](@CODE char(5))
RETURNS @GUESTS TABLE
(
	[Id] INT,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[Phone] VARCHAR(10) NULL,
	[Email] VARCHAR(50) NULL,
	[DietaryRestrictions] NVARCHAR(250) NULL,
	[RsvpStatus] BIT NOT NULL DEFAULT 0,
	[RsvpDate] DATETIME NULL
)
AS
BEGIN
	INSERT @GUESTS
	SELECT Id, FirstName, LastName, Phone, Email, DietaryRestrictions, RsvpStatus, RsvpDate
	FROM Guests
	WHERE InvitationCode = @CODE;
	RETURN;
END

CREATE FUNCTION [dbo].[fnGetGuestsByInvitationCode](@CODE char(5))
RETURNS @GUESTS TABLE
(
	[Id] INT,
	[FirstName] NVARCHAR(50),
	[LastName] NVARCHAR(50),
	[Phone] VARCHAR(10),
	[Email] VARCHAR(50),
	[DietaryRestrictions] NVARCHAR(250),
	[DietaryRestrictionsUpdated] DATETIME,
	[RsvpStatus] BIT,
	[RsvpDate] DATETIME
)
AS
BEGIN
	INSERT @GUESTS
	SELECT Id, FirstName, LastName, Phone, Email, DietaryRestrictions, DietaryRestrictionsUpdated, RsvpStatus, RsvpDate
	FROM Guests
	WHERE InvitationCode = @CODE;
	RETURN;
END

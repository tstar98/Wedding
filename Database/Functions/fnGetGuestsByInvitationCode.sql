CREATE FUNCTION [dbo].[fnGetGuestsByInvitationCode](@CODE char(6))
RETURNS @GUESTS TABLE
(
	[Id] INT,
	[FirstName] NVARCHAR(50),
	[LastName] NVARCHAR(50),
	[Phone] VARCHAR(10),
	[Email] VARCHAR(50),
	[RsvpStatus] BIT,
	[RsvpDate] DATETIME,
	[DietaryRestrictions] NVARCHAR(250),
	[DietaryRestrictionsUpdated] DATETIME
)
AS
BEGIN
	INSERT @GUESTS
	SELECT Id, FirstName, LastName, Phone, Email, RsvpStatus, RsvpDate, DietaryRestrictions, DietaryRestrictionsUpdated
	FROM Guests
	WHERE InvitationCode = @CODE;
	RETURN;
END

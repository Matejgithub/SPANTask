CREATE DATABASE SPANTask;

SELECT DATABASEPROPERTYEX('SPANTask', 'Croatian_100_CS_AI_KS_WS_SC') SQLCollation;
USE SPANTask;
CREATE TABLE [dbo].[Podaci](
	[Id] [int] IDENTITY(1,1) PRIMARY KEY,
	[Name] [nvarchar](50) NOT NULL,
	[Surname] [nvarchar](50) NOT NULL,
	[ZipCode] [int] NOT NULL,
	[City] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](20) NOT NULL
)

CREATE TYPE PodaciTableType 
AS TABLE
    (
		[Name] [nvarchar](50),
		[Surname] [nvarchar](50),
		[ZipCode] [int],
		[City] [nvarchar](50),
		[PhoneNumber] [nvarchar](20)
	);
GO

CREATE PROCEDURE [dbo].[AddPodaciItems]
    @PodaciItems PodaciTableType READONLY
AS
BEGIN
	INSERT INTO dbo.Podaci
	SELECT i.[Name], i.Surname, i.ZipCode, i.City, i.PhoneNumber
	FROM @PodaciItems i
		LEFT JOIN Podaci p on
			p.[Name] = i.[Name] and
			p.Surname = i.Surname and
			p.ZipCode = i.ZipCode and
			p.City = i.City and
			p.PhoneNumber = p.PhoneNumber
	WHERE p.[Name] IS NULL	
	
	IF @@ROWCOUNT > 0 and @@ROWCOUNT < (SELECT COUNT (*) FROM @PodaciItems)
		EXEC xp_logevent 70000, 'Nisu spremljeni svi zapisi';
	IF @@ROWCOUNT = 0
		EXEC xp_logevent 60000, 'Nije spremljen niti jedan zapis';
	ELSE
		EXEC xp_logevent 60000, 'Uspješno spremljeno';
	SELECT @@ROWCOUNT
END
GO
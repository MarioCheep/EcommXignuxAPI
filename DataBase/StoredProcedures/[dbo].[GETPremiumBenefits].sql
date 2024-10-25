USE XignuxDB
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GETPremiumBenefits')
	DROP PROCEDURE [dbo].[GETPremiumBenefits] 
GO

CREATE PROCEDURE [dbo].[GETPremiumBenefits]    
@ClientId integer
AS    
	BEGIN

		SELECT Ben.BenefitId, Ben.BenefitName 
		FROM tblBenefitsByClient BenByCli WITH (NOLOCK)
		INNER JOIN tblBenefits Ben WITH (NOLOCK) ON (BenByCli.BenefitId = Ben.BenefitId)
		INNER JOIN tblClients Cli WITH (NOLOCK) ON (BenByCli.ClientId = Cli.ClientId)
		WHERE Cli.ClientId = @ClientId
		AND Cli.PremiumClient = 1
		AND Ben.BenefitActive = 1

	END 

--EXECUTE  [dbo].[GETPremiumBenefits]  4
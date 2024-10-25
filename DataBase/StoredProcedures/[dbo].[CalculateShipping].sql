USE XignuxDB
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'CalculateShipping')
	DROP PROCEDURE [dbo].[CalculateShipping] 
GO

CREATE PROCEDURE [dbo].[CalculateShipping]
	@ClientId Integer
	,@ProductId integer
AS    
	BEGIN

	DECLARE @Result FLOAT
	, @IncrementByProduct FLOAT
	, @IncrementByDistance FLOAT

	SET @Result = 0
	SET @IncrementByProduct = 0
	SET @IncrementByDistance = 0

	--Premium Client / Benefit Free Shipping Assigned and Active --> No Cost
	IF EXISTS (SELECT * 
				FROM tblBenefitsByClient BenByCli WITH (NOLOCK) 
				INNER JOIN tblClients Cli WITH (NOLOCK) ON (BenByCli.Clientid = Cli.ClientId)
				INNER JOIN tblBenefits Ben WITH (NOLOCK) ON (BenByCli.BenefitId = Ben.BenefitId)
				AND Cli.ClientId = @ClientId
				AND Cli.PremiumClient = 1
				AND Ben.BenefitId = 2  --Benefit Free Shipping
				AND Ben.BenefitActive = 1)
		BEGIN
			SELECT 0 AS ShippingCost, -1 AS IncrementByProduct, -1 AS IncrementByDistance
		END
	ELSE
		BEGIN
			--Calculate Cost Increment By Product Details
			SELECT @Result = (DC.CostIncrement + WC.CostIncrement)
			FROM tblProductDetails PD WITH (NOLOCK)
			INNER JOIN tblDimensionCatalog DC WITH (NOLOCK) ON (PD.DimensionsCatalogId = DC.DimensionsCatalogId)
			INNER JOIN tblWeightCatalog WC WITH (NOLOCK) ON (PD.WeightCatalogId = WC.WeightCatalogId)
			WHERE PD.ProductId = @ProductId

			SET @IncrementByProduct = @Result
			--PRINT 'Cost Increment By Product Details: ' + CONVERT (VARCHAR(5), @Result,10)

			--Calculate Cost Increment By minimal Distance
			SELECT @Result = @Result + X.CostIncrement, @IncrementByDistance = X.CostIncrement
			FROM
				(
					SELECT TOP(1) Distance, SC.CostIncrement
					FROM tblStoreCoverage SC WITH(NOLOCK)
					INNER JOIN tblStores S WITH(NOLOCK) ON (SC.StoreId = S.StoreId)
					INNER JOIN tblClients C WITH(NOLOCK) ON (SC.PostalCodeCoverage = C.PostalCode)
					WHERE C.ClientId = @ClientId
					ORDER BY SC.Distance ASC
				) AS X
			
			--PRINT 'Cost Total: ' + CONVERT (VARCHAR(5), @Result,10)

			SELECT @Result AS ShippingCost, @IncrementByProduct AS IncrementByProduct, @IncrementByDistance AS IncrementByDistance
		END
	END 

--EXECUTE  [dbo].[CalculateShipping] 2, 2


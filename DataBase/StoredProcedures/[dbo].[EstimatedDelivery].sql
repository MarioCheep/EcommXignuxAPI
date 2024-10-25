USE XignuxDB
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'EstimatedDelivery')
	DROP PROCEDURE [dbo].[EstimatedDelivery] 
GO

CREATE PROCEDURE [dbo].[EstimatedDelivery]
	@OrderId Integer
	,@ProductId integer
AS    
	BEGIN

		SELECT 
			FORMAT(O.DateCreated,'dd-MMM-yyyy HH:mm') AS OrderCreated
			,FORMAT(DATEADD(MINUTE, SC.AvgDeliveryTime, O.DateCreated),'dd-MMM-yyyy HH:mm') AS EstimatedDelivery
			,FORMAT(DATEADD(MINUTE, SC.AvgDeliveryTime - O.PreparationTime, O.DateCreated),'dd-MMM-yyyy HH:mm') AS MinDelivery
			,FORMAT(DATEADD(MINUTE, SC.MaxDeliveryTime, O.DateCreated),'dd-MMM-yyyy HH:mm') AS MaxDelivery
			,O.PreparationTime, SC.AvgDeliveryTime, SC.MaxDeliveryTime 
		FROM tblOrders O WITH(NOLOCK)
		INNER JOIN tblStoreCoverage SC WITH(NOLOCK) ON (O.StoreId = SC.StoreId)
		INNER JOIN tblClients C WITH(NOLOCK) ON (O.ClientId = C.ClientId AND SC.PostalCodeCoverage = C.PostalCode)
		WHERE O.OrderId = @OrderId
		AND O.ProductId = @ProductId

	END 

--EXECUTE  [dbo].[EstimatedDelivery] 1, 1


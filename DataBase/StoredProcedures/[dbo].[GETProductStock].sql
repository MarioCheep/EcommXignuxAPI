USE XignuxDB
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GETProductStock')
	DROP PROCEDURE [dbo].[GETProductStock]  
GO

CREATE PROCEDURE [dbo].[GETProductStock]    
	@productId INTEGER
AS    
BEGIN

DECLARE @Result integer

	SELECT @Result = Count(ProductId) 
	FROM tblProductStock WITH (NOLOCK)
	WHERE ProductId = @productId

	RETURN @Result
END 

--EXECUTE [dbo].[GETProductStock] 22
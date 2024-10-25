USE XignuxDB
GO

--
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GETCouponDiscount')
	DROP PROCEDURE [dbo].[GETCouponDiscount] 
GO

CREATE PROCEDURE [dbo].[GETCouponDiscount]    
	@CouponCode NVARCHAR(10)
AS    
BEGIN

	DECLARE @ValidationInformation NVARCHAR(50)
	, @DueDate DateTime
	, @DiscountPercentage INT


	IF EXISTS (SELECT CouponId FROM tblCoupons WITH (NOLOCK)
					WHERE CouponCode = @CouponCode)
		BEGIN

			SELECT @DueDate = DueDate, @DiscountPercentage = DiscountPercentage
			FROM tblCoupons WITH (NOLOCK)
			WHERE CouponCode = @CouponCode

			IF (@DueDate < GetDate())
				BEGIN 
					SET @ValidationInformation = 'Expired Coupon'
					SET @DiscountPercentage = -1

					SELECT @DiscountPercentage AS DiscountPercentage, @ValidationInformation AS ValidationInformation
				END

			ELSE
				BEGIN
					PRINT ('EXISTS')
					SET @ValidationInformation = 'Current Coupon'

					SELECT C.DiscountPercentage, @ValidationInformation AS ValidationInformation, P.ProductName
					FROM tblProductsByCoupon PByC WITH (NOLOCK)
					INNER JOIN tblProducts P WITH (NOLOCK) ON (PByC.ProductId = P.ProductId)
					INNER JOIN tblCoupons C WITH (NOLOCK) ON (PByC.CouponId = C.CouponId)
					WHERE CouponCode = @CouponCode
				END
		END

	ELSE
		BEGIN
			SET @ValidationInformation = 'Coupon does not exist'
			SET @DiscountPercentage = -2

			SELECT @DiscountPercentage AS DiscountPercentage, @ValidationInformation AS ValidationInformation
		END

END 

--EXECUTE  [dbo].[GETCouponDiscount] 'XN20241024'
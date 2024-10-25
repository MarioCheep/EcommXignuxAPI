USE XignuxDB
Go

DELETE FROM tblUsers
DBCC CHECKIDENT ('tblUsers', RESEED, 0);
GO

--SELECT * FROM tblUsers
INSERT INTO tblUsers (UserName, DateCreated) VALUES ('XignuxADMIN', GETDATE())
GO

--Select * From tblStores
INSERT INTO tblStores (StoreName, PostalCode, DateCreated, UserIdCreated) VALUES ('STORE ONE', '66266', GETDATE(), 1)
INSERT INTO tblStores (StoreName, PostalCode, DateCreated, UserIdCreated) VALUES ('STORE TWO', '66000',GETDATE(), 1)
GO


--Select * From tblProducts
INSERT INTO tblProducts (ProductName, DateCreated, UserIdCreated) VALUES ('Product One', GetDate(), 1)
INSERT INTO tblProducts (ProductName, DateCreated, UserIdCreated) VALUES ('Product Two', GetDate(), 1)
INSERT INTO tblProducts (ProductName, DateCreated, UserIdCreated) VALUES ('Product THREE', GetDate(), 1)
INSERT INTO tblProducts (ProductName, DateCreated, UserIdCreated) VALUES ('Product FOUR', GetDate(), 1)

--SELECT * FROM tblProductStock
INSERT INTO tblProductStock (ProductId, StoreId, DateCreated, UserIdCreated) VALUES (1,1,GetDate(), 1)
INSERT INTO tblProductStock (ProductId, StoreId, DateCreated, UserIdCreated) VALUES (1,2,GetDate(), 1)
INSERT INTO tblProductStock (ProductId, StoreId, DateCreated, UserIdCreated) VALUES (2,2,GetDate(), 1)
INSERT INTO tblProductStock (ProductId, StoreId, DateCreated, UserIdCreated) VALUES (3,1,GetDate(), 1)

--SELECT * FROM tblCoupons
INSERT INTO tblCoupons (CouponCode, DiscountPercentage, DueDate, DateCreated, UserIdCreated) VALUES ('XN20241023', 10, '2024-10-23 12:00:00.000', GetDate(), 1)
INSERT INTO tblCoupons (CouponCode, DiscountPercentage, DueDate, DateCreated, UserIdCreated) VALUES ('XN20241024', 15, '2024-10-24 23:00:00.000', GetDate(), 1)
INSERT INTO tblCoupons (CouponCode, DiscountPercentage, DueDate, DateCreated, UserIdCreated) VALUES ('XN20241026', 30, '2024-10-26 23:00:00.000', GetDate(), 1)


--SELECT * FROM tblProductsByCoupon
INSERT INTO tblProductsByCoupon (CouponId, ProductId, DateCreated, UserIdCreated) VALUES (2, 2, GetDate(), 1)
INSERT INTO tblProductsByCoupon (CouponId, ProductId, DateCreated, UserIdCreated) VALUES (2, 4, GetDate(), 1)
INSERT INTO tblProductsByCoupon (CouponId, ProductId, DateCreated, UserIdCreated) VALUES (3, 1, GetDate(), 1)
INSERT INTO tblProductsByCoupon (CouponId, ProductId, DateCreated, UserIdCreated) VALUES (4, 2, GetDate(), 1)
INSERT INTO tblProductsByCoupon (CouponId, ProductId, DateCreated, UserIdCreated) VALUES (4, 3, GetDate(), 1)


--SELECT * FROM tblClients
INSERT INTO tblClients (ClientName, PostalCode, PremiumClient, DateCreated, UserIdCreated) VALUES ('Client ONE', '66266', 1, GetDate(), 1)
INSERT INTO tblClients (ClientName, PostalCode, PremiumClient, DateCreated, UserIdCreated) VALUES ('Client TWO', '66000', 0, GetDate(), 1)
INSERT INTO tblClients (ClientName, PostalCode, PremiumClient, DateCreated, UserIdCreated) VALUES ('Client THREE', '64780', 1, GetDate(), 1)


--SELECT * FROM tblBenefits
INSERT INTO tblBenefits (BenefitName, BenefitActive, DateCreated, UserIdCreated) VALUES ('Benefit 10% Discount', 1, GetDate(), 1)
INSERT INTO tblBenefits (BenefitName, BenefitActive, DateCreated, UserIdCreated) VALUES ('Benefit Free Shipping', 1, GetDate(), 1)
INSERT INTO tblBenefits (BenefitName, BenefitActive, DateCreated, UserIdCreated) VALUES ('Benefit 2x1 in Products', 1, GetDate(), 1)
INSERT INTO tblBenefits (BenefitName, BenefitActive, DateCreated, UserIdCreated) VALUES ('Benefit 50% Discount', 0, GetDate(), 1)

--SELECT * FROM tblBenefitsByClient
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (1, 1, GetDate(), 1)
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (1, 2, GetDate(), 1)
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (1, 3, GetDate(), 1)
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (1, 4, GetDate(), 1)
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (2, 1, GetDate(), 1)
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (2, 2, GetDate(), 1)
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (2, 3, GetDate(), 1)
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (3, 2, GetDate(), 1)
INSERT INTO tblBenefitsByClient (ClientId, BenefitId, DateCreated, UserIdCreated) VALUES (3, 3, GetDate(), 1)

--SELECT * FROM tblDimensionCatalog
INSERT INTO tblDimensionCatalog (DimensionsDescription, CostIncrement, DateCreated, UserIdCreated)
	VALUES ('Small', 10, GetDate(), 1)
INSERT INTO tblDimensionCatalog (DimensionsDescription, CostIncrement, DateCreated, UserIdCreated)
	VALUES ('Medium', 12.5, GetDate(), 1)
INSERT INTO tblDimensionCatalog (DimensionsDescription, CostIncrement, DateCreated, UserIdCreated)
	VALUES ('Large', 15.2, GetDate(), 1)

--SELECT * FROM tblWeightCatalog
INSERT INTO tblWeightCatalog (WeightDescription, CostIncrement, DateCreated, UserIdCreated)
	VALUES ('Light', .8 , GetDate(), 1)
INSERT INTO tblWeightCatalog (WeightDescription, CostIncrement, DateCreated, UserIdCreated)
	VALUES ('Medium', 13.2 , GetDate(), 1)
INSERT INTO tblWeightCatalog (WeightDescription, CostIncrement, DateCreated, UserIdCreated)
	VALUES ('Heavy', 16.4 , GetDate(), 1)

--SELECT * FROM tblProductDetails
INSERT INTO tblProductDetails (ProductId, DimensionsCatalogId, WeightCatalogId) VALUES (1, 1, 1)
INSERT INTO tblProductDetails (ProductId, DimensionsCatalogId, WeightCatalogId) VALUES (2, 2, 3)
INSERT INTO tblProductDetails (ProductId, DimensionsCatalogId, WeightCatalogId) VALUES (3, 3, 3)


SELECT * FROM tblStores
--Select * From tblStoreCoverage
INSERT INTO tblStoreCoverage(StoreId, PostalCodeCoverage, Distance, CostIncrement, DateCreated, UserIdCreated)
	VALUES (1, '66266', 1, 2.3, GetDate(), 1)
INSERT INTO tblStoreCoverage(StoreId, PostalCodeCoverage, Distance, CostIncrement, DateCreated, UserIdCreated)
	VALUES (1, '66000', 5, 4.6, GetDate(), 1)
INSERT INTO tblStoreCoverage(StoreId, PostalCodeCoverage, Distance, CostIncrement, DateCreated, UserIdCreated)
	VALUES (2, '64780', 3, 2, GetDate(), 1)

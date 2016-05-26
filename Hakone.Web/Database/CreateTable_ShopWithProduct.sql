IF OBJECT_ID('ShopWithProduct', 'U') IS NOT NULL
Drop Table ShopWithProduct
Go

SELECT * Into ShopWithProduct From (
SELECT
	IsNull(Shops.IDENTITYCOL + 0, -1) AS 'ID', Shops.ShopName,Shops.Photo, Shops.CatId,
	Shops.IsSelected,Shops.SelectedDate, Shops.IsHot, Shops.HotDate,
	Shops.IsRecommend, Shops.RecommendDate,
	Shops.ShopTags,
	STUFF
	(
		(
			SELECT Top 4 ', ' + Products.Photo
			FROM Products
			WHERE Products.ShopId = Shops.ID
			ORDER BY Products.DefaultSortDate Desc
			FOR XML PATH (''),TYPE
		).value('.','nvarchar(max)')
		,1,2,''
	) AS ProductPhotos
FROM Shops Where Shops.IsDeleted = 0 And (IsHot =1 Or IsSelected =1 Or IsRecommend =1)
) As R Where ProductPhotos Is Not NULL And len(ProductPhotos) - len(replace(ProductPhotos,',','')) >2

Go

IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'ShopWithProduct')
BEGIN
   ALTER TABLE ShopWithProduct ADD CONSTRAINT pk_ShopId PRIMARY KEY (ID)
END

Go

--update Shops set RecommendDate = RecommendDate -100 where DATEDIFF(D,RecommendDate,GETDATE())<2

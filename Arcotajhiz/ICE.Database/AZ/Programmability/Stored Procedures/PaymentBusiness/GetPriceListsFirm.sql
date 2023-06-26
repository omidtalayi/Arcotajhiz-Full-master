CREATE PROCEDURE [AZ].[GetPriceListsFirm](
	@IdentificationTypeVCode AS INT = 1,
	@HasCheque BIT = 0
)
AS
BEGIN
	IF @IdentificationTypeVCode IS NULL
	BEGIN
		SET @IdentificationTypeVCode = 1
	END

	IF @HasCheque = 1
	BEGIN
		SELECT	PL.*,
				('<PriceListTypes>' + (SELECT * FROM AZ.PriceListType (NOLOCK) WHERE VCode = PL.PriceListTypeVCode FOR XML PATH('PriceListType')) + '</PriceListTypes>') PriceListType
		INTO #T FROM AZ.PriceList PL
		WHERE ISNULL(IdentificationTypeVCode,0) = @IdentificationTypeVCode
		UNION 
		SELECT	PL.*,
				('<PriceListTypes>' + (SELECT * FROM AZ.PriceListType (NOLOCK) WHERE VCode = PL.PriceListTypeVCode FOR XML PATH('PriceListType')) + '</PriceListTypes>') PriceListType
		FROM AZ.PriceList PL
		WHERE PL.VCode IN (5,6)

		SELECT * FROM
		(
			SELECT	T.VCode,
					T.[Name],
					T.EnumName,
					T.UserVCode,
					CASE WHEN VCode IN (2,4) THEN T.Price + (SELECT Price FROM #T WHERE VCode = 6) ELSE T.Price END Price,
					T.PriceListTypeVCode,
					T.IdentificationTypeVCode,
					T.PriceListType
			FROM #T T WHERE T.VCode IN (1,2,3,4,5) 
		) T1
		ORDER BY T1.Price DESC

		DROP TABLE #T
	END
	ELSE
	BEGIN
		SELECT	PL.*,
				('<PriceListTypes>' + (SELECT * FROM AZ.PriceListType (NOLOCK) WHERE VCode = PL.PriceListTypeVCode FOR XML PATH('PriceListType')) + '</PriceListTypes>') PriceListType
		FROM AZ.PriceList PL
		WHERE ISNULL(IdentificationTypeVCode,0) = @IdentificationTypeVCode
	END
END

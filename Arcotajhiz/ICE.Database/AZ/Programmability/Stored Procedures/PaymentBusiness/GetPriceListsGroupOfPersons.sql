CREATE PROCEDURE [AZ].[GetPriceListsGroupOfPersons]
AS
BEGIN
	SELECT	0 VCode,
			'قیمت گزارش اطلاعات هویتی' [Name],
			'IDENTITY_INFORMATION_REPORT_PRICE' EnumName,
			NULL UserVCode,
			275250 Price,
			0 PriceListTypeVCode,
			0 IdentificationTypeVCode,
			NULL PriceListType
	UNION
	SELECT	0 VCode,
			'ماليات بر ارزش افزوده' [Name],
			'IDENTITY_INFORMATION_TAX' EnumName,
			NULL UserVCode,
			24750 Price,
			0 PriceListTypeVCode,
			0 IdentificationTypeVCode,
			NULL PriceListType
END
GO
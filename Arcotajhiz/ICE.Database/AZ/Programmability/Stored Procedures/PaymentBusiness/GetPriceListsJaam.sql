CREATE PROCEDURE [AZ].[GetPriceListsJaam]
AS
BEGIN
	SELECT	0 VCode,
			'قیمت گزارش صورت های مالی' [Name],
			'JAAM_REPORT_PRICE' EnumName,
			NULL UserVCode,
			2752294 Price,
			0 PriceListTypeVCode,
			0 IdentificationTypeVCode,
			NULL PriceListType
	UNION
	SELECT	0 VCode,
			'ماليات بر ارزش افزوده' [Name],
			'JAAM_TAX' EnumName,
			NULL UserVCode,
			247706 Price,
			0 PriceListTypeVCode,
			0 IdentificationTypeVCode,
			NULL PriceListType
END

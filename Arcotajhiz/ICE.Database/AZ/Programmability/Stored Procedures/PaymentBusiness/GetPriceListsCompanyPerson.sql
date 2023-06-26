CREATE PROCEDURE [AZ].[GetPriceListsCompanyPerson]
AS
BEGIN
	SELECT	0 VCode,
			'قیمت گزارش اطلاعات شرکتی' [Name],
			'COMPANY_PERSON_REPORT_PRICE' EnumName,
			NULL UserVCode,
			275250 Price,
			0 PriceListTypeVCode,
			0 IdentificationTypeVCode,
			NULL PriceListType
	UNION
	SELECT	0 VCode,
			'ماليات بر ارزش افزوده' [Name],
			'COMPANY_PERSON_TAX' EnumName,
			NULL UserVCode,
			24750 Price,
			0 PriceListTypeVCode,
			0 IdentificationTypeVCode,
			NULL PriceListType
END
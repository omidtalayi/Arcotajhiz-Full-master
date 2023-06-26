CREATE PROCEDURE [AZ].[GetIdentification](
	@VCode BIGINT = NULL,
	@TrackingCode BIGINT = NULL,
	@IdICS24 NVARCHAR(200) = NULL,
	@AppIcs24HashCode NVARCHAR(MAX) = NULL
)
AS
BEGIN
	IF @VCode IS NOT NULL
	BEGIN
		SELECT	1 R,
				I.*,
				UPT.[Name] UserPaymentTypeName,
				CRR.TrackingCode,
				ISNULL(OP_OUTER.TraceNo,ISNULL(IP_OUTER.TraceNo,A_OUTER.TraceNo)) TraceNo,
				ISNULL(OP_OUTER.PaymentAmount,ISNULL(IP_OUTER.PaymentAmount,A_OUTER.PaymentAmount)) PaymentAmount,
				ISNULL(OP_OUTER.PaymentDate,ISNULL(IP_OUTER.PaymentDate,A_OUTER.PaymentDate)) PaymentDate,
				('<IdentificationStates>' + (SELECT * FROM AZ.IdentificationState (NOLOCK) WHERE VCode = I.IdentificationStateVCode FOR XML PATH('IdentificationState')) + '</IdentificationStates>') IdentificationState,
				('<IdentificationTypes>' + (SELECT * FROM AZ.IdentificationType (NOLOCK) WHERE VCode = I.IdentificationTypeVCode FOR XML PATH('IdentificationType')) + '</IdentificationTypes>') IdentificationType,
				('<IdentificationSendToOthers>' + (SELECT * FROM AZ.IdentificationSendToOthers (NOLOCK) WHERE IdentificationVCode = I.VCode FOR XML PATH('IdentificationSendToOther')) + '</IdentificationSendToOthers>') IdentificationSendToOthers,
				BatchJSon.ResponseJson
		FROM AZ.Identification (NOLOCK) I
		LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON I.VCode = CRR.IdentificationVCode
		LEFT JOIN [ICEUserManagement].AZ.UserPaymentType (NOLOCK) UPT ON UPT.VCode = I.UserPaymentTypeVCode
		OUTER APPLY
		(
			SELECT TOP 1 BR.ResponseJson FROM [ICECore].AZ.Batch (NOLOCK) B
			INNER JOIN [ICECore].AZ.BatchResult (NOLOCK) BR ON B.VCode = BR.BatchVCode
			WHERE B.IdentificationVCode = I.VCode
		) BatchJSon
		OUTER APPLY 
		(
			SELECT TOP 1 OPR.TraceNo,OPR.EntryDate PaymentDate,OP.Amount PaymentAmount FROM AZ.OnlinePayment (NOLOCK) OP
			INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode
			WHERE OP.IdentificationVCode = I.VCode
		) OP_OUTER
		OUTER APPLY 
		(
			SELECT TOP 1 IDP.SaleRefID TraceNo,IDP.EntryDate PaymentDate,IDP.SaleAmount PaymentAmount FROM AZ.IdentificationPayment (NOLOCK) IDP
			WHERE IDP.IdentificationVCode = I.VCode
		) IP_OUTER
		OUTER APPLY
		(
			SELECT TOP 1 NULL TraceNo,A.EntryDate PaymentDate,A.Bed PaymentAmount FROM AZ.Accounting (NOLOCK) A
			WHERE A.IdentificationVCode = I.VCode
		) A_OUTER
		WHERE I.VCode = @VCode 
			--ISNULL(I.IdICS24,0) = ISNULL(@IdICS24,ISNULL(I.IdICS24,0)) AND
			--ISNULL(I.AppIcs24HashCode,0) = ISNULL(@AppIcs24HashCode,ISNULL(I.AppIcs24HashCode,0))
	END
	IF @IdICS24 IS NOT NULL
	BEGIN
		SELECT	1 R,
				I.*,
				UPT.[Name] UserPaymentTypeName,
				CRR.TrackingCode,
				ISNULL(OP_OUTER.TraceNo,ISNULL(IP_OUTER.TraceNo,A_OUTER.TraceNo)) TraceNo,
				ISNULL(OP_OUTER.PaymentAmount,ISNULL(IP_OUTER.PaymentAmount,A_OUTER.PaymentAmount)) PaymentAmount,
				ISNULL(OP_OUTER.PaymentDate,ISNULL(IP_OUTER.PaymentDate,A_OUTER.PaymentDate)) PaymentDate,
				('<IdentificationStates>' + (SELECT * FROM AZ.IdentificationState (NOLOCK) WHERE VCode = I.IdentificationStateVCode FOR XML PATH('IdentificationState')) + '</IdentificationStates>') IdentificationState,
				('<IdentificationTypes>' + (SELECT * FROM AZ.IdentificationType (NOLOCK) WHERE VCode = I.IdentificationTypeVCode FOR XML PATH('IdentificationType')) + '</IdentificationTypes>') IdentificationType,
				('<IdentificationSendToOthers>' + (SELECT * FROM AZ.IdentificationSendToOthers (NOLOCK) WHERE IdentificationVCode = I.VCode FOR XML PATH('IdentificationSendToOther')) + '</IdentificationSendToOthers>') IdentificationSendToOthers,
				BatchJSon.ResponseJson
		FROM AZ.Identification (NOLOCK) I
		LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON I.VCode = CRR.IdentificationVCode
		LEFT JOIN [ICEUserManagement].AZ.UserPaymentType (NOLOCK) UPT ON UPT.VCode = I.UserPaymentTypeVCode
		OUTER APPLY
		(
			SELECT TOP 1 BR.ResponseJson FROM [ICECore].AZ.Batch (NOLOCK) B
			INNER JOIN [ICECore].AZ.BatchResult (NOLOCK) BR ON B.VCode = BR.BatchVCode
			WHERE B.IdentificationVCode = I.VCode
		) BatchJSon
		OUTER APPLY 
		(
			SELECT TOP 1 OPR.TraceNo,OPR.EntryDate PaymentDate,OP.Amount PaymentAmount FROM AZ.OnlinePayment (NOLOCK) OP
			INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode
			WHERE OP.IdentificationVCode = I.VCode
		) OP_OUTER
		OUTER APPLY 
		(
			SELECT TOP 1 IDP.SaleRefID TraceNo,IDP.EntryDate PaymentDate,IDP.SaleAmount PaymentAmount FROM AZ.IdentificationPayment (NOLOCK) IDP
			WHERE IDP.IdentificationVCode = I.VCode
		) IP_OUTER
		OUTER APPLY
		(
			SELECT TOP 1 NULL TraceNo,A.EntryDate PaymentDate,A.Bed PaymentAmount FROM AZ.Accounting (NOLOCK) A
			WHERE A.IdentificationVCode = I.VCode
		) A_OUTER
		WHERE ISNULL(I.IdICS24,0) = @IdICS24
	END
	IF @AppIcs24HashCode IS NOT NULL
	BEGIN
		SELECT	1 R,
				I.*,
				UPT.[Name] UserPaymentTypeName,
				CRR.TrackingCode,
				ISNULL(OP_OUTER.TraceNo,ISNULL(IP_OUTER.TraceNo,A_OUTER.TraceNo)) TraceNo,
				ISNULL(OP_OUTER.PaymentAmount,ISNULL(IP_OUTER.PaymentAmount,A_OUTER.PaymentAmount)) PaymentAmount,
				ISNULL(OP_OUTER.PaymentDate,ISNULL(IP_OUTER.PaymentDate,A_OUTER.PaymentDate)) PaymentDate,
				('<IdentificationStates>' + (SELECT * FROM AZ.IdentificationState (NOLOCK) WHERE VCode = I.IdentificationStateVCode FOR XML PATH('IdentificationState')) + '</IdentificationStates>') IdentificationState,
				('<IdentificationTypes>' + (SELECT * FROM AZ.IdentificationType (NOLOCK) WHERE VCode = I.IdentificationTypeVCode FOR XML PATH('IdentificationType')) + '</IdentificationTypes>') IdentificationType,
				('<IdentificationSendToOthers>' + (SELECT * FROM AZ.IdentificationSendToOthers (NOLOCK) WHERE IdentificationVCode = I.VCode FOR XML PATH('IdentificationSendToOther')) + '</IdentificationSendToOthers>') IdentificationSendToOthers,
				BatchJSon.ResponseJson
		FROM AZ.Identification (NOLOCK) I
		LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON I.VCode = CRR.IdentificationVCode
		LEFT JOIN [ICEUserManagement].AZ.UserPaymentType (NOLOCK) UPT ON UPT.VCode = I.UserPaymentTypeVCode
		OUTER APPLY
		(
			SELECT TOP 1 BR.ResponseJson FROM [ICECore].AZ.Batch (NOLOCK) B
			INNER JOIN [ICECore].AZ.BatchResult (NOLOCK) BR ON B.VCode = BR.BatchVCode
			WHERE B.IdentificationVCode = I.VCode
		) BatchJSon
		OUTER APPLY 
		(
			SELECT TOP 1 OPR.TraceNo,OPR.EntryDate PaymentDate,OP.Amount PaymentAmount FROM AZ.OnlinePayment (NOLOCK) OP
			INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode
			WHERE OP.IdentificationVCode = I.VCode
		) OP_OUTER
		OUTER APPLY 
		(
			SELECT TOP 1 IDP.SaleRefID TraceNo,IDP.EntryDate PaymentDate,IDP.SaleAmount PaymentAmount FROM AZ.IdentificationPayment (NOLOCK) IDP
			WHERE IDP.IdentificationVCode = I.VCode
		) IP_OUTER
		OUTER APPLY
		(
			SELECT TOP 1 NULL TraceNo,A.EntryDate PaymentDate,A.Bed PaymentAmount FROM AZ.Accounting (NOLOCK) A
			WHERE A.IdentificationVCode = I.VCode
		) A_OUTER
		WHERE ISNULL(I.AppIcs24HashCode,0) = @AppIcs24HashCode
	END
	IF @TrackingCode IS NOT NULL
	BEGIN
		SELECT	1 R,
				I.*,
				UPT.[Name] UserPaymentTypeName,
				CRR.TrackingCode,
				ISNULL(OP_OUTER.TraceNo,ISNULL(IP_OUTER.TraceNo,A_OUTER.TraceNo)) TraceNo,
				ISNULL(OP_OUTER.PaymentAmount,ISNULL(IP_OUTER.PaymentAmount,A_OUTER.PaymentAmount)) PaymentAmount,
				ISNULL(OP_OUTER.PaymentDate,ISNULL(IP_OUTER.PaymentDate,A_OUTER.PaymentDate)) PaymentDate,
				('<IdentificationStates>' + (SELECT * FROM AZ.IdentificationState (NOLOCK) WHERE VCode = I.IdentificationStateVCode FOR XML PATH('IdentificationState')) + '</IdentificationStates>') IdentificationState,
				('<IdentificationTypes>' + (SELECT * FROM AZ.IdentificationType (NOLOCK) WHERE VCode = I.IdentificationTypeVCode FOR XML PATH('IdentificationType')) + '</IdentificationTypes>') IdentificationType,
				('<IdentificationSendToOthers>' + (SELECT * FROM AZ.IdentificationSendToOthers (NOLOCK) WHERE IdentificationVCode = I.VCode FOR XML PATH('IdentificationSendToOther')) + '</IdentificationSendToOthers>') IdentificationSendToOthers,
				BatchJSon.ResponseJson
		FROM AZ.Identification (NOLOCK) I
		LEFT JOIN AZ.CreditRiskReport (NOLOCK) CRR ON I.VCode = CRR.IdentificationVCode
		LEFT JOIN [ICEUserManagement].AZ.UserPaymentType (NOLOCK) UPT ON UPT.VCode = I.UserPaymentTypeVCode 
		OUTER APPLY
		(
			SELECT TOP 1 BR.ResponseJson FROM [ICECore].AZ.Batch (NOLOCK) B
			INNER JOIN [ICECore].AZ.BatchResult (NOLOCK) BR ON B.VCode = BR.BatchVCode
			WHERE B.IdentificationVCode = I.VCode
		) BatchJSon
		OUTER APPLY 
		(
			SELECT TOP 1 OPR.TraceNo,OPR.EntryDate PaymentDate,OP.Amount PaymentAmount FROM AZ.OnlinePayment (NOLOCK) OP
			INNER JOIN AZ.OnlinePaymentReceived (NOLOCK) OPR ON OP.VCode = OPR.OnlinePaymentVCode
			WHERE OP.IdentificationVCode = I.VCode
		) OP_OUTER
		OUTER APPLY 
		(
			SELECT TOP 1 IDP.SaleRefID TraceNo,IDP.EntryDate PaymentDate,IDP.SaleAmount PaymentAmount FROM AZ.IdentificationPayment (NOLOCK) IDP
			WHERE IDP.IdentificationVCode = I.VCode
		) IP_OUTER
		OUTER APPLY
		(
			SELECT TOP 1 NULL TraceNo,A.EntryDate PaymentDate,A.Bed PaymentAmount FROM AZ.Accounting (NOLOCK) A
			WHERE A.IdentificationVCode = I.VCode
		) A_OUTER
		WHERE CRR.TrackingCode = @TrackingCode
	END
END
GO


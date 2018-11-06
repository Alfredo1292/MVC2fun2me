-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE usp_credito_parientes  @Identificacion VARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT * 
INTO #TEMPORAL
FROM Personas  WITH(NOLOCK)

WHERE Identificacion IN (
SELECT IdentificacionPariente
 FROM 

BURO..Credid_Parientes CP WITH(NOLOCK)
--INNER JOIN Personas P WITH(NOLOCK) ON CP.Identificacion
WHERE Identificacion=@Identificacion
)

SELECT * 
INTO #TEMPCREDITO
FROM Credito WHERE 
IdPersona IN(

SELECT ID FROM #TEMPORAL
)

AND FechaTransferencia IS NOT NULL


	IF EXISTS (SELECT 1 FROM #TEMPCREDITO)
		BEGIN 
			SELECT 
				TMP.Identificacion,CONCAT(	TMP.PrimerNombre,' ',	TMP.SegundoNombre,' ',	TMP.PrimerApellido,' ',	TMP.SegundoApellido,' ') AS nombre,
				TMP.CORREO,
				TMP.TelefonoCel,
				CapitalPendiente
				FROM #TEMPORAL TMP INNER JOIN #TEMPCREDITO CRE WITH(NOLOCK) ON TMP.Id=CRE.IdPersona
		END

DROP TABLE #TEMPORAL
DROP TABLE #TEMPCREDITO

END
GO

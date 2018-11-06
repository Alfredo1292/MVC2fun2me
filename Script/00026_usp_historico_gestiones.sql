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
CREATE PROCEDURE usp_historico_gestiones
	@IdCredito bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT IdCredito,
	FechaGestion,
	Detalle,
	UsrModifica AS Usuario,
	ISNULL(TA.Descripcion,'') AS Accion,
	ISNULL(TRA.Descripcion,'') AS RespuestaGestion 
	FROM dbo.GestionCobros GC
LEFT JOIN dbo.Tabla_Accion TA ON(GC.Accion = TA.IdAccion) LEFT JOIN dbo.Tabla_RespuestaGestion TRA ON(GC.IdRespuestaGestion = TRA.IdRespuestaGestion) 
WHERE IdCredito = @IdCredito AND MontoPromesaPago<=0 ORDER BY FechaGestion DESC
END
GO

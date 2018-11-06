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
CREATE PROCEDURE usp_prestamos_cliente
@Identificacion VARCHAR(25)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT 
CDT.IdSolicitud,
PDTS.nombreProducto,
PDTS.MontoProducto,
--ISNULL(CDT.Comentario,'') AS Comentario,
CONVERT(VARCHAR(10),CDT.FechaCredito,126) AS FechaCredito,
CDT.Interes_Corriente,CDT.InteresMora,CDT.MontoCuota,CDT.CapitalPendiente FROM [dbo].[Credito] CDT WITH(NOLOCK)
INNER JOIN Solicitudes SLC WITH(NOLOCK) ON CDT.IdSolicitud=SLC.Id AND SLC.IdPersona=CDT.IdPersona
INNER JOIN Personas P WITH(NOLOCK) ON P.Id=SLC.IdPersona 
INNER JOIN Productos PDTS WITH(NOLOCK) ON PDTS.Id=SLC.IdProducto
WHERE P.Identificacion=@Identificacion
END
GO

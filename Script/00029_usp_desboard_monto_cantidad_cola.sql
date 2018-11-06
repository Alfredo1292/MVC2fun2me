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
alter PROCEDURE usp_desboard_monto_cantidad_cola
	@cod_agente varchar(25)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


SELECT SUM(Saldo) AS SALDO_PENDIENTE,
COUNT(1) AS CANTIDAD_PENDIENTES
 FROM [BucketCobros] A WITH(NOLOCK)
 INNER JOIN agente B WITH(NOLOCK) ON A.AGENTEASIGNADO=B.cod_agente 
 INNER JOIN Personas D WITH(NOLOCK) ON A.IdPersona=D.Id
 INNER JOIN vReporte_ConsultarCreditos v WITH(NOLOCK) ON V.Identificacion=D.Identificacion
 inner join Productos   Pro WITH(NOLOCK) on v.Idproducto=Pro.id

  WHERE a.AgenteAsignado=@cod_agente and V.FechaTransferencia is not null
  AND a.PROCESADO=0 and Asignado=1


END
GO

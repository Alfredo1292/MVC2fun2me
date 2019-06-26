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
ALTER PROCEDURE usp_cola_cobros
	@cod_agente varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT top 1 a.[IdCredito],
	 D.Identificacion
     ,[PromesaRota]
     ,[CeroPagos]
     ,[CuentaAlDia]
     ,[RangoSaldoBajo]
     ,[RangoSaldoMedio]
     ,[RangoSaldoAlto]
     ,[Bucket]
     ,A.[Estado]
     ,[BucketNuevo]
     ,[AgenteAsignado]
     ,[Asignado]
  FROM [BucketCobros] A WITH(NOLOCK)
 INNER JOIN agente B WITH(NOLOCK) ON A.AGENTEASIGNADO=B.cod_agente 
 INNER JOIN Personas D WITH(NOLOCK) ON A.IdPersona=D.Id
 --INNER JOIN vReporte_ConsultarCreditos v WITH(NOLOCK) ON V.Identificacion=D.Identificacion
-- inner join Productos   Pro WITH(NOLOCK) on v.Idproducto=Pro.id
--inner join Principales Pri WITH(NOLOCK) on Pri.IdProducto=Pro.Id
--	inner join PlanPagos   Pp WITH(NOLOCK) on Pp.Idcredito=Credito and Pp.Cuota=Pri.Cuota
--	inner join Personas P WITH(NOLOCK) ON P.Identificacion=V.Identificacion 
--	inner join Credito C WITH(NOLOCK) ON C.IdPersona=P.Id 
 WHERE B.cod_agente=@cod_agente --and V.FechaTransferencia is not null

 AND a.PROCESADO=0 AND A.ASIGNADO=1
END
GO

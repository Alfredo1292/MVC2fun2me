
/****** Object:  StoredProcedure [dbo].[usp_ParaVosPutito]    Script Date: 2/11/2018 15:38:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
alter PROCEDURE [dbo].[usp_informacion_cuenta]
	@IdCredito as int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	
WITH
cte_credito as (SELECT	
					id,
					idsolicitud,
					idtipo,
					MontoCuota,
					CapitalPendiente
				FROM credito WITH(NOLOCK)
				Where id = @IdCredito),
cte_planpagos	as (SELECT
						pla.idcredito																	idcredito,
						isnull(sum(isnull(montooriginacion,0)),0)										Originacion,
						isnull(SUM(isnull(pla.montointeres,0)),0)										IntCorrienteTotal,
						SUM(iif(datediff(day,fechavencimiento, getdate())>=0, pla.montointeres,0))		IntCorrienteAlDia
					FROM Planpagos pla WITH(NOLOCK)
					--inner join cte_credito cre WITH(NOLOCK) ON pla.idcredito = cre.id
					where IdCredito = @IdCredito
					group by pla.idcredito) ,
cte_cobros		as (SELECT top 1
						idcredito,
						convert(date,max(fechacobro))			UltimaFechaCobro,
						TotalCobro
					FROM cobros cob
					--inner join cte_credito cre WITH(NOLOCK) ON cob.idcredito = cre.id
					where movimientocontable = 75 and IdCredito = @IdCredito
					group by idcredito,fechacobro,TotalCobro
					order by fechacobro desc),
cte_solicitudes as (SELECT
						cre.id,
						cre.idtipo,
						pro.nombreproducto																				Producto,
						--dbo.fechavencimientoactual(cre.id)														FechaVenc,
						iif(cre.idtipo = 14 and (datediff(day,dbo.fechavencimientoactual(cre.id), getdate())>0), 
						datediff(day,dbo.fechavencimientoactual(cre.id), getdate()),0)									DiasVenc,
						cre.MontoCuota																					Cuota,
						pla.Originacion																					Originacion,
						dbo.TotalMora(cre.id, getdate())																SaldoVenc,
						dbo.TotalCancela(cre.id, getdate())																SaldoTotal,
						cre.CapitalPendiente																			CapitalPendiente,
						pla.IntCorrienteAlDia																			IntCorriente,
						iif(dbo.TotalIntMora(cre.id, getdate())>0,
						dbo.TotalIntMora(cre.id, getdate())- pla.IntCorrienteAlDia,0)									IntMoraPendiente,
						dbo.fechavencimientoactual(cre.id)																MoraDesde,
						convert(date,dbo.ultimafechaaldia(cre.id))														UltimaFechaAlDia,
						cob.UltimaFechaCobro																			FechaUltPago,
						cob.TotalCobro																					MontoUltPago

					FROM solicitudes sol WITH(NOLOCK)
					inner join cte_credito cre WITH(NOLOCK) ON cre.idsolicitud = sol.id
					left join cte_cobros cob WITH(NOLOCK) ON cob.idcredito = cre.id
					left join productos pro WITH(NOLOCK) ON pro.id = sol.idproducto
					left join cte_planpagos pla WITH(NOLOCK) ON pla.idcredito = cre.id)				
Select  
--	sol.id,
--	sol.idtipo,
	sol.Producto,
--	sol.FechaVenc,
	sol.DiasVenc,
	sol.Cuota,
	sol.originacion,
	sol.SaldoVenc,
	sol.SaldoTotal,
	sol.CapitalPendiente,
	sol.IntCorriente,
	sol.IntMoraPendiente,
	CONVERT(VARCHAR(10),sol.MoraDesde,126) AS MoraDesde,
	ISNULL(CONVERT(VARCHAR(10),	sol.UltimaFechaAlDia,126),'') AS UltimaFechaAlDia,
	ISNULL(CONVERT(VARCHAR(10),	sol.FechaUltPago,126),'') AS FechaUltPago,
	ISNULL(sol.MontoUltPago,0) AS MontoUltPago
from cte_solicitudes sol WITH(NOLOCK)

/*

 <th>Moneda</th>
                                                <th>Producto</th>
                                                <th>Dias Venc.</th>
                                                <th>Cuota</th>
                                                <th>Comisión</th>
                                                <th>Saldo Venc.</th>
                                                <th>Saldo Total</th>
                                                <th>Saldo Capital</th>
                                                <th>Int.Corriente</th>
                                                <th>Int.Mora</th>
                                                <th>Mora Desde</th>
                                                <th>Fec.Ult.Pago</th>
                                                <th>Mto.Ult.Pago</th>
   */ 
END

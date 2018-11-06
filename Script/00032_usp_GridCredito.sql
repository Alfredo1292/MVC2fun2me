USE [FinTech_DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_GridCredito]    Script Date: 5/11/2018 12:27:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER proc [dbo].[usp_GridCredito]
	@IdCredito int
AS

DECLARE  
	@idbuscado int,
	@CAPITALPEND as decimal(18,2), 
	@ORIGINACIONPEND as decimal(18,2),
	@INTERESPEND as decimal(18,2),
	@FechaActual as date,
	@CapFechaActual as decimal(18,2),
	@OriFechaActual as decimal(18,2),
	@IntFechaActual as decimal(18,2)

--SET @IdCredito = 10

--Catalogo de Movientos
-- 1 = Transferencia
-- 2 = Vencimiento
-- 3 = Cobro
-- 4 = Venc y Cobro
-- 5 = HOY => El día en el cual se está consultando el historial del crédito

--Creo la tabla temporal donde voy a almacenar toda la información del crédito
CREATE TABLE #TempFechas (
	ID INT IDENTITY(1, 1) primary key ,
	tipomovimiento			int,
	idcobro					int,
	idplanpagos				int,
	fecha					date,
	bandera					int,
	capitalpend				decimal(18,2),
	originacionpend			decimal(18,2),
	interespend				decimal(18,2),
	morapend				decimal(18,2),
	saldoaldia				decimal(18,2),
	totalcobrado			decimal(18,2),
	capitalcob				decimal(18,2),
	originacioncob			decimal(18,2),
	interescob				decimal(18,2),
	moracob					decimal(18,2),
	saldototal				decimal(18,2)
)

--Con este CTE traigo toda la informacion que necesito. Abierto en transferencia, Vencimientos y Cobros
;WITH	
	cte_transferencia as(SELECT
							 cre.Id							IdCredito
							,cre.FechaTransferencia			FechaTransferencia
							,SUM(pla.CapitalOriginal)		CapitalOrig
							,SUM(pla.OriginacionOriginal)	OriginacionOrig
							,SUM(pla.InteresOriginal)		InteresOrig
							,SUM(pla.CapitalOriginal) 
							+SUM(pla.OriginacionOriginal)	TotalCapitalOrig
						 FROM Credito cre WITH(NOLOCK)
						 left join PlanPagos pla WITH(NOLOCK) ON pla.IdCredito = cre.Id
						 WHERE cre.Id = @IdCredito
						 GROUP BY cre.id, cre.FechaTransferencia,cre.MontoCuota
						),
	cte_vencimientos as (SELECT
							id,	
							IdCredito,
							FechaVencimiento,
							CapitalOriginal,
							OriginacionOriginal,
							InteresOriginal
						 FROM PlanPagos pla WITH(NOLOCK)
						 WHERE IdCredito = @IdCredito),
	cte_cobros		as  (SELECT
							Id,					
							FechaCobro,
							TotalCobro,
							CapitalCobrado,
							OriginacionCobrado,
							InteresCobrado,
							InteresMoraCobrado
						 FROM Cobros cob WITH(NOLOCK)
						 WHERE IdCredito = @IdCredito and MovimientoContable = 75 --75 son cobros que vienen del banco
						)
	insert into #TempFechas
	SELECT
		 1								--tipomovimiento 
		,0								--idcobro
		,0								--idplanpagos
		,FechaTransferencia				--Fecha
		,1								--bandera
		,CapitalOrig					--capitalpend
		,OriginacionOrig				--originacionpend
		,InteresOrig					--interespend
		,0								--morapend
		,0								--saldoaldia
		,0								--totalcobrado
		,0								--capitalcob
		,0								--originacioncob
		,0								--interescob
		,0								--moracob
		,TotalCapitalOrig				--saldototal
	FROM cte_transferencia
	UNION 
	SELECT
		 2								--tipomovimiento 
		,0								--idcobro
		,id								--idplanpagos
		,FechaVencimiento				--Fecha
		,0								--bandera
		,CapitalOriginal				--capitalpend
		,OriginacionOriginal			--originacionpend
		,InteresOriginal				--interespend
		,0								--morapend
		,0								--saldoaldia
		,0								--totalcobrado
		,0								--capitalcob
		,0								--originacioncob
		,0								--interescob
		,0								--moracob
		,0								--saldototal
	FROM cte_vencimientos 
	UNION
	SELECT
		 3								--tipomovimiento 
		,Id								--idcobro
		,0								--idplanpagos
		,FechaCobro						--Fecha
		,0								--bandera
		,0								--capitalpend
		,0								--originacionpend
		,0								--interespend
		,0								--morapend
		,0								--saldoaldia
		,isnull(TotalCobro,0)			--totalcobrado
		,isnull(CapitalCobrado,0)		--capitalcob
		,isnull(OriginacionCobrado,0)	--originacioncob
		,isnull(InteresCobrado,0)		--interescob
		,isnull(InteresMoraCobrado,0)	--moracob
		,0								--saldototal
	FROM cte_cobros

if exists (SELECT * from #TempFechas WITH(NOLOCK) where fecha = GETDATE())
	BEGIN
		UPDATE #TempFechas	SET tipomovimiento = 5 WHERE fecha = GETDATE()
	END
ELSE
	BEGIN
	INSERT INTO #TempFechas VALUES	--Inserto una línea que lleva el día de hoy
		(5								--tipomovimiento 
		,0								--idcobro
		,0								--idplanpagos
		,GETDATE()						--Fecha
		,0								--bandera
		,0								--capitalpend
		,0								--originacionpend
		,0								--interespend
		,0								--morapend
		,0								--saldoaldia
		,0								--totalcobrado
		,0								--capitalcob
		,0								--originacioncob
		,0								--interescob
		,0								--moracob
		,0)								--saldototal
END

--Primero me fijo si tengo algún cobro el mismo día que el vencimiento para tener sol una línea en vez de 2
WHILE (SELECT COUNT(1) FROM #TempFechas WITH(NOLOCK) where Bandera = 0 and idcobro > 0)>0
	BEGIN
		set @idbuscado = (SELECT TOP 1 ID FROM #TempFechas WITH(NOLOCK) Where Bandera = 0 and idcobro > 0 Order By Id asc) 

		if exists (SELECT * from #TempFechas WITH(NOLOCK) where fecha = (SELECT Fecha from #TempFechas WITH(NOLOCK) where ID = @idbuscado) and idcobro = 0)
			BEGIN
				UPDATE 
					t1
				SET 
					 t1.tipomovimiento	= 4
					,t1.bandera			= 1
					,t1.idcobro			= t2.idcobro
					,t1.totalcobrado	= t2.totalcobrado
					,t1.capitalcob		= t2.capitalcob
					,t1.originacioncob	= t2.originacioncob
					,t1.interescob		= t2.interescob
					,t1.moracob			= t2.moracob
				FROM #TempFechas as t1
				INNER JOIN #TempFechas as t2 ON t1.ID != t2.ID
				WHERE 
					t2.id = @idbuscado and
					t2.fecha = t1.fecha
				
				delete #TempFechas Where ID = @idbuscado
			END  
		ELSE
			UPDATE #TempFechas SET Bandera = 1 Where ID = @idbuscado
	END


UPDATE #TempFechas SET Bandera = 0 WHERE ID > 1




SELECT 
	 @CAPITALPEND			= capitalpend 
	,@ORIGINACIONPEND		= originacionpend
	,@INTERESPEND			= interespend
FROM #TempFechas 
WHERE ID = 1

WHILE (SELECT COUNT(1) FROM #TempFechas WITH(NOLOCK)  where Bandera = 0)>0
	BEGIN
		SELECT TOP 1
			@FechaActual			= fecha
		   ,@CapFechaActual			= capitalcob
		   ,@OriFechaActual			= originacioncob
		   ,@IntFechaActual			= interescob
		FROM #TempFechas WITH(NOLOCK)
		WHERE Bandera = 0 ORDER BY fecha ASC

		UPDATE 
			#TempFechas 
		SET 
			 Bandera = 1 
			,saldoaldia				= capitalpend + originacionpend + interespend + dbo.[TotalIntMora](@IdCredito, @FechaActual)
			,capitalpend			= @CAPITALPEND - @CapFechaActual
			,originacionpend		= @ORIGINACIONPEND - @OriFechaActual
			,interespend			= @INTERESPEND - @IntFechaActual
			,morapend				= dbo.[TotalIntMora](@IdCredito, @FechaActual)
			,saldototal				= dbo.[TotalCancela](@IdCredito, @FechaActual) 
		WHERE fecha = @FechaActual

		SELECT 
			 @CAPITALPEND			= capitalpend 
			,@ORIGINACIONPEND		= originacionpend
			,@INTERESPEND			= interespend
		FROM #TempFechas WITH(NOLOCK)
		WHERE fecha = @FechaActual

		--SET @CAPITALPEND = (SELECT capitalpend FROM #TempFechas where fecha = @FechaActual)


		--UPDATE #TempFechas SET Bandera = 1 Where ID = @idbuscado
	END

SELECT 
	tipomovimiento,
	CONVERT(VARCHAR(12),fecha,126) AS fecha,
	capitalpend			CapitalPendiente,
	originacioncob		OriginacionPendiente,
	interespend			InteresPendiente,
	morapend			MoraPendiente,
	saldoaldia			SaldoAlDia,
	totalcobrado		,
	capitalcob			CapitalCobrado,
	originacioncob		OriginacionCobrado,
	interescob			InteresCobrado,
	moracob				MoraCobrado,
	saldototal			SaldoTotal
FROM #TempFechas WITH(NOLOCK)
ORDER BY fecha ASC

drop table #TempFechas



--exec usp_ConsultaTablasCredito 10
--select  * from planpagos where id = 10
--select top 10 * from cobros
--select top 50 * from credito where fechatransferencia is not null order by fechaingreso desc
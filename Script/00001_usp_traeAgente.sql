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
   USE [FinTech_DEV]
   GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================


ALTER PROCEDURE usp_traeAgente
	@COD_AGENTE VARCHAR(20),
	@CLAVE VARCHAR(250)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT [cod_agente]
      ,[nombre]
      ,[pass]
      ,[vendedor]
      ,[fecha_cambiopassword]
      ,[STR_USUARIO_AD]
      ,[STR_COD_AGENTE_PADRE]
      ,[BIT_DISPONIBLE]
      ,[INT_COD_AREA]
      ,[FEC_CREACION]
      ,[FEC_MODIFICACION]
      ,[estado]
      ,[correo]
	  ,configuracionbucket
  FROM [agente] WITH(NOLOCK) WHERE [pass]=CASE WHEN LEN(@CLAVE)>0 THEN @CLAVE ELSE  [pass]END  AND cod_agente= CASE WHEN LEN( @COD_AGENTE)>0 THEN @COD_AGENTE  ELSE COD_AGENTE END AND estado='Activo'

END
GO

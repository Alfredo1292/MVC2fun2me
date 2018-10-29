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
ALTER PROCEDURE usp_ConsultaAsignacionBuckets(
@id int=null
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT [id]
      ,[BucketInicial]
      ,[BucketFinal]
      ,[PRPRotas]
      ,[CeroPagos]
      ,[SaldoAlto]
      ,[SaldoMedio]
      ,[SaldoBajo]
      ,[Estado]
  FROM [AsignacionBuckets]
  WHERE ID=CASE WHEN @id>0 THEN @id ELSE ID END 

END
GO

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
alter PROCEDURE usp_insertaBuckets
(@BucketInicial int
           ,@BucketFinal int
           ,@PRPRotas bit
           ,@CeroPagos bit
           ,@SaldoAlto bit
           ,@SaldoMedio bit
           ,@SaldoBajo bit
           ,@Estado bit
           ,@CuentaAlDia bit)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here


INSERT INTO [AsignacionBuckets]
           ([BucketInicial]
           ,[BucketFinal]
           ,[PRPRotas]
           ,[CeroPagos]
           ,[SaldoAlto]
           ,[SaldoMedio]
           ,[SaldoBajo]
           ,[Estado]
           ,[CuentaAlDia])
     VALUES
           (@BucketInicial, 
           @BucketFinal ,
           @PRPRotas,
           @CeroPagos,
           @SaldoAlto,
           @SaldoMedio,
           @SaldoBajo,
           @Estado,
           @CuentaAlDia)


SELECT @@IDENTITY AS id, [AsignacionBuckets]=0,
           [BucketInicial]=0
           ,[BucketFinal]=0
           ,[PRPRotas]=0
           ,[CeroPagos]=0
           ,[SaldoAlto]=0
           ,[SaldoMedio]=0
           ,[SaldoBajo]=0
           ,[Estado]=0
           ,[CuentaAlDia] =0  
END
GO

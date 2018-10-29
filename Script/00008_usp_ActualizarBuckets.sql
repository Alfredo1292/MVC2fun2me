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
CREATE PROCEDURE usp_ActualizarBuckets(
@id int 
,@PRPRotas BIT
,@CeroPagos BIT
,@SaldoAlto BIT
,@SaldoMedio BIT
,@SaldoBajo BIT
,@Estado BIT
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


UPDATE A
   SET 
      [PRPRotas] = @PRPRotas
      ,[CeroPagos] = @CeroPagos
      ,[SaldoAlto] = @SaldoAlto
      ,[SaldoMedio] = @SaldoMedio
      ,[SaldoBajo] = @SaldoBajo
      ,[Estado] = @Estado
	 FROM [AsignacionBuckets] A WITH(NOLOCK) 
 WHERE id=@id



END
GO

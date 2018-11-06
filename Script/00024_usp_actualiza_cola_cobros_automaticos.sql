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
CREATE PROCEDURE usp_actualiza_cola_cobros_automaticos
	-- Add the parameters for the stored procedure here
@IdCredito bigint,@AgenteAsignado varchar(25)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE A
	SET A.Procesado=1
	FROM [BucketCobros] A WITH(NOLOCK)

	WHERE IdCredito=@IdCredito AND AgenteAsignado=@AgenteAsignado
END
GO

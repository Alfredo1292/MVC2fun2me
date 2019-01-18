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
ALTER PROCEDURE usp_inserta_agente
	-- Add the parameters for the stored procedure here
					   (
					   	@cod_agente varchar(20)
					   ,@nombre varchar(100)
					   ,@pass varchar(100)
					   ,@vendedor char(1)=NULL
					   ,@STR_USUARIO_AD varchar(50)=NULL
					   ,@STR_COD_AGENTE_PADRE varchar(20)=NULL
					   ,@BIT_DISPONIBLE bit=NULL
					   ,@INT_COD_AREA int=NULL
					   ,@correo varchar(150)
					   ,@estado varchar(10)
					   ,@ROLID int 
					   )
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
BEGIN TRY
    BEGIN TRANSACTION
       
			INSERT INTO [dbo].[agente]
					   ([cod_agente]
					   ,[nombre]
					   ,[pass]
					   ,[vendedor]
					   ,[fecha_cambiopassword]
					   ,[STR_USUARIO_AD]
					   ,[STR_COD_AGENTE_PADRE]
					   ,[BIT_DISPONIBLE]
					   ,[INT_COD_AREA]
					   ,[FEC_CREACION]
					   ,[estado]
					   ,[correo]
					   ,rolid)
				 VALUES
					   (
					   	@cod_agente
					   ,@nombre
					   ,@pass
					   ,@vendedor
					   ,GETDATE()
					   ,@STR_USUARIO_AD
					   ,@STR_COD_AGENTE_PADRE
					   ,@BIT_DISPONIBLE
					   ,@INT_COD_AREA
					   ,GETDATE()
					   ,@estado
					   ,@correo
					   ,@ROLID
					   )
					COMMIT
					SELECT 0
		END TRY
BEGIN CATCH
    ROLLBACK
	SELECT 1
END CATCH
END
GO

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
ALTER PROCEDURE usp_insertar_ScoreCardExperto
           (@NombreModelo varchar(25)
           ,@RangoInicial varchar(25)
           ,@RangoFinal varchar(25)
           ,@Puntaje int)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


INSERT INTO [dbo].[ScoreCardExperto]
           ([NombreModelo]
           ,[RangoInicial]
           ,[RangoFinal]
           ,[Puntaje])
     VALUES
           (@NombreModelo,@RangoInicial,@RangoFinal,@Puntaje)

SELECT @@IDENTITY AS id, 
	[NombreModelo]=@NombreModelo,
           [RangoInicial]=@RangoInicial
           ,[RangoInicial]=@RangoFinal
           ,[Puntaje]=@Puntaje
END
GO

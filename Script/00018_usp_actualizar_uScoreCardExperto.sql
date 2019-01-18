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
alter PROCEDURE usp_actualizar_uScoreCardExperto
	-- Add the parameters for the stored procedure here
@Id INT,
@NombreModelo VARCHAR(25),
@RangoInicial VARCHAR(25),
@RangoFinal VARCHAR(25),
@Puntaje INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   UPDATE A
      SET [NombreModelo]=@NombreModelo
      ,[RangoInicial]=@RangoInicial
      ,[RangoFinal]=@RangoFinal
      ,[Puntaje]=@Puntaje
  FROM [ScoreCardExperto] A WITH(NOLOCK) 
  where id=@Id

END
GO

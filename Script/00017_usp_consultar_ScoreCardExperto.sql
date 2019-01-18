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
ALTER PROCEDURE usp_consultar_ScoreCardExperto @Id INT =NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT @Id=CASE WHEN @Id=0 THEN NULL ELSE @Id END

SELECT [Id]
      ,[NombreModelo]
      ,[RangoInicial]
      ,[RangoFinal]
      ,[Puntaje]
  FROM [ScoreCardExperto] WITH(NOLOCK)
  where id=ISNULL(@Id,Id)



END
GO

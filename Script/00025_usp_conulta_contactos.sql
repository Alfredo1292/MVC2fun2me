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
ALTER PROCEDURE usp_consulta_contactos
 @Identificacion VARCHAR(25) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @TelefonoCel INT,
@TelefonoFijo INT,
@TelefonoLaboral INT,
@FechaIngreso DATETIME,
@IdPersona BIGINT,
@IdCredito BIGINT

SELECT  @IdPersona = Id, @TelefonoCel = TelefonoCel,@TelefonoFijo = TelefonoFijo,@TelefonoLaboral = TelefonoLaboral,@FechaIngreso = ISNULL(FechaIngreso,'') FROM dbo.Personas  WHERE Identificacion = @Identificacion

SELECT TOP 1 @IdCredito = id FROM dbo.Credito WHERE IdTipo = 14 AND IdPersona = @IdPersona ORDER BY Id DESC

SELECT 'Credid' AS FuenteDatos, Tipo, Relacion, Dato AS Telefono,Fecha AS FechaDato FROM Buro.dbo.Credid_Localizacion WHERE Identificacion = @Identificacion AND LEN(Dato) > 5 AND ISNUMERIC(Dato)>0
UNION
SELECT 'Tuca' AS FuenteDatos,Descripcion AS Tipo,'' AS Relacion,Telefono AS Telefono, '' AS FechaDato FROM Buro.[dbo].[Telefono]  WHERE cedula = @Identificacion AND LEN(Telefono) > 5
UNION
(SELECT 'Interno' AS FuenteDatos,'Celular' AS Tipo,'' AS Relacion,@TelefonoCel AS Telefono, ISNULL( @FechaIngreso,'' )AS FechaDato WHERE @TelefonoCel >0)
UNION
(SELECT 'Interno' AS FuenteDatos,'Casa' AS Tipo,'' AS Relacion,@TelefonoFijo AS Telefono,  ISNULL( @FechaIngreso,'' ) AS FechaDato WHERE @TelefonoFijo >0)
UNION
(SELECT 'Interno' AS FuenteDatos,'Trabajo' AS Tipo,'' AS Relacion,@TelefonoLaboral AS Telefono,  ISNULL( @FechaIngreso,'' ) AS FechaDato WHERE @TelefonoLaboral >0)
END
GO

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
alter PROCEDURE usp_consultar_xml_buros @Identificacion VARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   select TOP 1 A.Estructura_XML AS TUCA,B.Estructura_XML AS CREDDIT from Buro..Tuca_ContenidoXML A WITH(NOLOCK) INNER JOIN 

Buro..Credid_ContenidoXML B WITH(NOLOCK) ON A.identificacion=B.identificacion
INNER JOIN Personas C WITH(NOLOCK) ON B.identificacion=C.Identificacion

WHERE C.Identificacion=@Identificacion
order by FechaIngreso desc
END
GO

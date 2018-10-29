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
ALTER PROCEDURE usp_cargaSolicitudBuro
@Id int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT   TOP 1
B.Id AS ID_SOLICITUD
,A.Identificacion 
      ,A.VencimientoIdentificacion
      ,CONCAT( A.PrimerNombre,' ',A.SegundoNombre,' ',A.PrimerApellido,' ',A.SegundoApellido) AS NOMBRE
      ,A.TelefonoCel
      ,A.TelefonoFijo
      ,A.TelefonoLaboral
      ,A.Correo
      ,A.CorreoOpcional 
      ,A.DetalleDireccion
      ,A.FechaIngreso
	  ,E.Id AS ID_ESTADO
      ,E.Descripcion,
	  f.Id as IdProducto,
	  F.NombreProducto,
	  B.MontoMaximo
,C.Estructura_XML AS XML_TUCA
,C.Fecha_Ingreso AS FECHA_INGRESO_TUCA
,C.Activo ESTADO_TUCA
,D.Estructura_XML AS XML_CREDID
,D.Fecha_Ingreso AS FECHA_INGRESO_CREDID
,D.Activo ESTADO_CREDID
 FROM  Personas  A WITH(NOLOCK)
 INNER JOIN Credito G WITH(NOLOCK) ON A.Id=G.IdPersona
INNER JOIN  Solicitudes B WITH(NOLOCK) ON A.ID=B.IdPersona
INNER JOIN Productos F WITH(NOLOCK) ON B.IdProducto=F.Id
INNER JOIN Tipos E WITH(NOLOCK) ON B.Status=E.Id
INNER JOIN Buro..Tuca_ContenidoXML C WITH(NOLOCK) ON A.Identificacion=C.identificacion
INNER JOIN  Buro..Credid_ContenidoXML D WITH(NOLOCK) ON D.identificacion=A.Identificacion
WHERE B.Id=@Id AND C.Activo =1 AND D.Activo=1
ORDER BY FechaIngreso DESC
END
GO

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
ALTER PROCEDURE usp_actualiza_limite_solicitud
@Id int,
@MontoMaximo DECIMAL(20,0),
@IdProducto INT,
@Status INT,
@forzamiento_solicitud bit,
@USUARIO_MODIFICACION  VARCHAR(150)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	UPDATE A
	SET A.MontoMaximo=@MontoMaximo,
	A.Status=@Status,
	A.IdProducto=@IdProducto,
	A.forzamiento_solicitud=@forzamiento_solicitud,
	A.USUARIO_MODIFICACION=@USUARIO_MODIFICACION
FROM Solicitudes A WITH(NOLOCK) WHERE ID=@Id

    -- Insert statements for procedure here
	
END
GO

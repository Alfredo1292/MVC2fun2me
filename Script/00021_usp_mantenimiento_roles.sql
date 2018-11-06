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
ALTER PROCEDURE usp_mantenimiento_roles 
	@ID int=NULL,
	@ROLESNOMBRE varchar(50)='',
	@ACCION varchar(50)=''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT @ID= CASE WHEN @ID=0 THEN NULL ELSE @ID END

    -- Insert statements for procedure here
	IF(@ACCION='CONSULTAR')
		BEGIN
			SELECT ID,ROLES AS ROLESNOMBRE FROM ROLES A WITH(NOLOCK) WHERE ID=ISNULL( @ID,ID)
		END
			IF(@ACCION='ELIMINAR')
		BEGIN
			DELETE FROM ROLES WHERE ID=@ID
		END
			IF(@ACCION='INSERTAR')
		BEGIN
			INSERT INTO ROLES VALUES(@ROLESNOMBRE)
			SELECT @@IDENTITY AS ID,'' AS ROLES
		END
			IF(@ACCION='ACTUALIZAR')
		BEGIN
			UPDATE  A
			SET A.ROLES =@ROLESNOMBRE
			FROM ROLES A WITH(NOLOCK)
			 WHERE ID=@ID
		END
END
GO

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
alter PROCEDURE usp_consultarMenu
@cod_agente varchar(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
SELECT D.MAINMENU AS MainMenuName,D.ID AS MainMenuId,C.SUBMENU AS SubMenuName,C.ID AS SubMenuId,C.CONTROLLER AS ControllerName,C.ACTION AS ActionName,B.ID AS RoleId,B.ROLES AS RoleName
FROM 

agente A WITH(NOLOCK) INNER JOIN ROLES B WITH(NOLOCK) ON A.ROLID=B.ID
INNER JOIN SUBMENU C WITH(NOLOCK) ON B.ID=C.ROLEID 
INNER JOIN MAINMENU D WITH(NOLOCK) ON C.MAINMENUID=D.ID
INNER JOIN AgenteMenu E WITH(NOLOCK) ON E.COD_AGENTE=A.cod_agente AND E.IDSUBMENU=C.ID
	WHERE a.cod_agente=@cod_agente AND A.ESTADO='Activo'
END
GO

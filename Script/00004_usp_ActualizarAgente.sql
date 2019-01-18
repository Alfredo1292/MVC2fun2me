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
ALTER PROCEDURE usp_ActualizarAgente
					   	@cod_agente varchar(20)
					   ,@nombre varchar(100)
					   ,@pass varchar(100)=''
					   ,@correo varchar(150)=''
					   ,@estado varchar(10)=''
					   ,@ConfiguracionBucket  varchar(50)=''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

UPDATE A
   SET [nombre] = CASE WHEN LEN(@nombre )>0 THEN @nombre  ELSE nombre  END 
      ,[pass] = CASE WHEN LEN(@pass )>0 THEN @pass  ELSE pass  END 
      ,[FEC_MODIFICACION] = GETDATE()
      ,[estado] = CASE WHEN LEN(@estado )>0 THEN @estado  ELSE estado  END 
      ,[correo] = CASE WHEN LEN(@correo )>0 THEN @correo  ELSE correo  END 
      ,[ConfiguracionBucket] = CASE WHEN LEN(@ConfiguracionBucket )>0 THEN @ConfiguracionBucket  ELSE ConfiguracionBucket  END 
	  	FROM AGENTE A WITH(NOLOCK)
 WHERE cod_agente=@cod_agente AND estado='Activo'



END
GO

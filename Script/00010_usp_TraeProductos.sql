USE [FinTech_DEV]
GO
/****** Object:  StoredProcedure [dbo].[usp_TraeProductos]    Script Date: 23/10/2018 17:48:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[usp_TraeProductos]--Obligatorio 
AS
BEGIN
   BEGIN TRY
         DECLARE
                --Cambios Alfredo José Vargas Seinfarth
				-- Inicio Variables para el manejo de errores estandar
                @vMsgApp VARCHAR(2000)  = OBJECT_NAME(@@ProcID) ,
                @vMsgFun VARCHAR(2000) ,
                @vMsgTec VARCHAR(4000)  = NULL ,
                @vMsgTraC VARCHAR(10)    = NULL ,
                @vMsgTraT VARCHAR(4000)  = NULL ,
                @vMsgType VARCHAR(5)     = 'SE' ,
                @vMsgErrC INT            = 0 ,
                @vMsgErrL INT            = 0 ,
                @vMsgErrN INT            = 0 ,
                @vReg INT                = 0
               -- Fin
	    SET @vMsgTraC = '1'
		       
                    select distinct Id,Id as IdProducto,NombreProducto from Productos;
		          
				   if @@ROWCOUNT = 0
                         BEGIN
                             SET @vMsgTec = 'Error al consultar la tabla Productos. ERROR#('+ISNULL(convert(varchar,@@ERROR),'') +').';
			                 SET @vMsgTraC = '2'
	                         RETURN 
                         GOTO exception
                    END  
		                  
		         
END TRY 
BEGIN CATCH
        INSERT [dbo].[BitacoraErrores]
        (
            [Proceso],
            [Parametros],
            [Error],
            [FechaRegistro]
        )
        VALUES
        (ERROR_PROCEDURE(),'Error al consultar la tabla de Productos',
         ERROR_MESSAGE(), GETDATE());
		  IF dbo.fu_UT_Msg_ValFormat(ERROR_MESSAGE()) = 0
              BEGIN
               SET @vMsgType = 'OE'
               SET @vMsgTec = ERROR_MESSAGE();
               SET @vMsgErrL = ERROR_LINE();
               SET @vMsgErrN = ERROR_NUMBER();
              GOTO EXCEPTION
          END
             ELSE
                THROW
 END CATCH;
if  @vMsgTraC='2'
  begin
	 EXCEPTION: 
		EXEC dbo.pr_UT_Msg_GenErr 
				@pMsgApp = @vMsgApp, @pMsgFun = @vMsgFun,
				@pMsgTec = @vMsgTec, @pMsgTraC = @vMsgTraC, @pMsgTraT = @vMsgTraT,
				@pMsgType = @vMsgType, @pMsgErrC = @vMsgErrC,
				@pMsgErrL = @vMsgErrL, @pMsgErrN = @vMsgErrN; 
	end
END;





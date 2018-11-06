
/****** Object:  StoredProcedure [dbo].[usp_ConsultarCreditos]    Script Date: 31/10/2018 13:09:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
alter PROCEDURE [dbo].[usp_ConsultarCreditos_cobros] 
 @Identificacion  VARCHAR(25)='',
 @IdCredito       VARCHAR(25)=''

AS
BEGIN


		SET XACT_ABORT, NOCOUNT ON; --ADDED TO PREVENT EXTRA RESULT SETS FROM INTERFERING WITH SELECT STATEMENTS.
        SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED; -- ADD NOLOCK ALL TABLES 

		SELECT @IdCredito= CASE WHEN @IdCredito='0' THEN '' ELSE @IdCredito END

	select TOP 1 Credito, V.Identificacion, 
         isnull(V.PrimerNombre,'')+' '+ isnull(V.SegundoNombre,'')+' '+isnull(V.PrimerApellido,'')+' '+isnull(V.SegundoApellido,'') as Nombre,
         Producto,Cuotas,v.Cuota,v.MontoProducto,CuotasPendientes,TotalPagado,Saldo,Estado,DiasMora,

		 P.DetalleDireccion,
		 P.UsrModifica,
		CONVERT(VARCHAR(12), P.FechaModificacion,101) AS FechaModificacion,
		 P.Id as cod_cliente,
		 p.TelefonoCel,
		 p.TelefonoLaboral AS TelefonoLaboral,
		 p.TelefonoFijo,
		 p.correo,
		  CONVERT(VARCHAR(10), C.FechaCredito, 112) as Apertura,
		 ISNULL(CM.Relacion,T.Descripcion) AS  EstadoCivil,
		 CL.DireccionRelacionada,
		 CL.Nombre AS TrabajoNombre,
		  isnull(CM.Nombre,'')+' '+ isnull(CM.Apellido1,'')+' '+isnull(CM.Apellido2,'') AS Conyugue,
		  c.id AS IdCredito
    from vReporte_ConsultarCreditos v WITH(NOLOCK)
	inner join Productos   Pro WITH(NOLOCK) on v.Idproducto=Pro.id
	inner join Principales Pri WITH(NOLOCK) on Pri.IdProducto=Pro.Id
	inner join PlanPagos   Pp WITH(NOLOCK) on Pp.Idcredito=Credito and Pp.Cuota=Pri.Cuota
	inner join Personas P WITH(NOLOCK) ON P.Identificacion=V.Identificacion 
	inner join Credito C WITH(NOLOCK) ON C.IdPersona=P.Id 
	left join Tipos T WITH(NOLOCK) ON T.Id=P.EstadoCivil
	left join buro..Credid_Localizacion CL WITH(NOLOCK) ON CL.Identificacion=P.Identificacion 	AND CL.Relacion = 'Trabajos anteriores'  
	left join[Buro]..[Credid_Matrimonios] CM WITH(NOLOCK) ON CL.Identificacion=CM.Identificacion AND Actual=1
  Where (V.Identificacion  =   @Identificacion or ISNULL(@Identificacion,'')='')
	and V.FechaTransferencia is not null


order by v.Identificacion


End


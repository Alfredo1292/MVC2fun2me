namespace TwoFunTwoMeFintech.Models.DTO.Colocacion
{
    public class dto_ProcesaSolicitud
    {
        //INICIO FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD
        public int IdSolicitud { set; get; }
        public int IdTipoIdentificacion { set; get; }
        public string Identificacion { set; get; }
        public System.DateTime VencimientoIdentificacion {set; get;}

        public string PrimerNombre { set; get; }
        public string SegundoNombre { set; get; }
        public string PrimerApellido { set; get; }
        public string SegundoApellido { set; get; }
        public int TelefonoCel { set; get; }
        public int TelefonoFijo { set; get; }
        public int TelefonoLaboral { set; get; }
        public string Correo { set; get; }
        public string CorreoOpcional { set; get; }
        public int EstadoCivil { set; get; }
        public int Sexo { set; get; }
        public System.DateTime FechaNacimiento { set; get; }
        public int Provincia { set; get; }
        public int Canton { set; get; }
        public int Distrito { set; get; }
        public string  DetalleDireccion { set; get; }
        public string UsrModifica { set; get; }
        public string NombreProducto { set; get; }
        public int IdProducto { set; get; }
        public string UsoCredito { set; get; }
        public int OrdenPatronal { set; get; }
        //RefereinciaFamiliar
        public int IdTipRefFamiliar { set; get; }
        public string NombreCompleto { set; get; }
        public int TelefonoFamiliar { set; get; }
        //ReferenciaLaboral
        public string Empresa { set; get; }
        public string TelefonoEmpresa { set; get; }
        public string SupervisorDirecto { set; get; }
        //ReferenciaPersonal
        public string NombreCompletoPersonal { set; get; }
        public int TelefonoPersonal { set; get; }
        public int Estado { set; get; }
        public int Soporte { set; get; }

        public string Respuesta { set; get; }
}
}
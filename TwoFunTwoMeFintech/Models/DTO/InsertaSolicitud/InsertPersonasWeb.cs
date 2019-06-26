using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models.DTO.InsertaSolicitud
{
    public class InsertPersonasWeb
    {
        public int? IdSolicitud { set; get; }

        public int? UltidSolicitud { set; get; }
        public int? IdTipoIdentificacion { set; get; }
        [Required]
        public string Identificacion { set; get; }

        public string nombre { set; get; }

        public string PrimerNombre { set; get; }

        public string SegundoNombre { set; get; }

        public string PrimerApellido { set; get; }

        public string SegundoApellido { set; get; }

        public string TelefonoCel { set; get; }

        public string Correo { set; get; }

        public int? IdProducto { set; get; }

        public string IdBanco { set; get; }

        public string UsrModifica { set; get; }

        public string Origen { set; get; }

        public string SubOrigen { get; set; }
        public string Status { set; get; }
        //FCAMACHO 30/01/2019 INICIO LECTURA DE RABBIT HASTA QUE CAMBIE STATUS

        public decimal? MontoMaximo { set; get; }

        public string FechaVencimiento { set; get; }

        public string Respuesta { set; get; }

        public string URL { set; get; }

        public string Descripcion { set; get; }

    }
}
using System;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class comprobantepago
    {

        public int? Id { get; set; }

        public string Identificacion { get; set; }

        public int? IdCredito { get; set; }

        public int? IdBanco { get; set; }

        public int? NumeroComprobante { get; set; }

        public string RutaBlob { get; set; }

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaPago { get; set; }

        public string FechaTransferencia { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public string UsuarioCreacion { get; set; }

        public bool? Estado { get; set; }

        public string Accion { get; set; }

        public string Mensaje { get; set; }

        public string Banco { get; set; }
        public string EstadoNombre { get; set; }
        public string NombreCuenta { get; set; }
        public string ImageComprobante { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class TransferenciasFinanzas
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Producto { get; set; }
        public decimal Monto { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        public string Sinpe { get; set; }
        public string Fecha { get; set; }

        public DateTime FECHA_INICIO { get; set; }
        public DateTime FECHA_FIN { get; set; }
    }
}
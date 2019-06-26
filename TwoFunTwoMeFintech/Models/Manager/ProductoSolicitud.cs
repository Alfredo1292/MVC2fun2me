using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.Manager
{
    public class ProductoSolicitud
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public decimal Cuota { get; set; }
        public int IdMontoCredito { get; set; }
        public int IdPlazoCredito { get; set; }
        public int IdFrecuenciaCredito { get; set; }
        public decimal MontoCredito { get; set; }
    }
}
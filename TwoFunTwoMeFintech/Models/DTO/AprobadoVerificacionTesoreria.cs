using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class AprobadoVerificacionTesoreria
    {
        public int status { get; set; }
        public int status2 { get; set; }
        public string tipo { get; set; }
        public string Filtro { get; set; }
        public int Id { get; set; }
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Producto { get; set; }
        public decimal Monto { get; set; }
        public string Banco { get; set; }
        public string Cuenta { get; set; }
        public string Sinpe { get; set; }
        public string Estado { get; set; }
        public string Correo { get; set; }
    }
}
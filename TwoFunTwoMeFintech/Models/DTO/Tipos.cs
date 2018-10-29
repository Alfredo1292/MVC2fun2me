using System;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Tipos
    {
        public int Id { get; set; }

        //public int Id_Estado { get; set; }

        public int? ID_ESTADO { get; set; }

        public string Descripcion { get; set; }

        public string Otros { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaModificacion { get; set; }
        public string MensajeError { get; set; }
        
    }
}
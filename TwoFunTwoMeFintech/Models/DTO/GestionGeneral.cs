using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class GestionGeneral
    {
        [Key]

        public int? IdGestionGeneral { get; set; }

        public int? IdSolicitud { get; set; }

        public string Detalle { get; set; }

        public DateTime? FechaIngreso { get; set; }

        public string nombre { get; set; }

        public string Cod_Agente { get; set; }

        public string FechaGestion { get; set; }

        public int? TipoGestion { get; set; }

        public string DesTipoGestion { get; set; }

        public bool? Activo { get; set; }

        public string Mensaje { get; set; }

        public string ACCION { get; set; }
        public List<GestionGeneral> ListGestionGeneral { get; set; }
        public string Accion_gestion { set; get; }

    }
}
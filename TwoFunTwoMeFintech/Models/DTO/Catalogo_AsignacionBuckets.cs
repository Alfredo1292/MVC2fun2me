using System;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class Catalogo_AsignacionBuckets_conf
    {
        public int Id { get; set; }

        public string Orden { get; set; }

        public string Nombre { get; set; }
        public string EstadoNombre { get; set; }

        public bool? Estado { get; set; }

        public DateTime? Fecha_inserta { get; set; }

        public DateTime? Fecha_modifica { get; set; }

        public string Usuario_Inserta { get; set; }

        public string Usuario_modifica { get; set; }

        public string Respuesta { get; set; }

        public int FK_Id_AsignacionBuckets { get; set; }
        public int FK_Cat_AsignacionBuckets { get; set; }
        public int OrdenDatos { get; set; }
        public string Accion { get; set; }

    }
    public class Catalogo_AsignacionBuckets
    {
        public int Id { get; set; }

        public string Orden { get; set; }

        public string Nombre { get; set; }
        public string EstadoNombre { get; set; }

        public bool? Estado { get; set; }

        public DateTime? Fecha_inserta { get; set; }

        public DateTime? Fecha_modifica { get; set; }

        public string Usuario_Inserta { get; set; }

        public string Usuario_modifica { get; set; }

        public string Respuesta { get; set; }

        public int FK_Id_AsignacionBuckets { get; set; }
        public int FK_Cat_AsignacionBuckets { get; set; }
        public int OrdenDatos { get; set; }
        public string Accion { get; set; }

    }
    public class AsignacionBucketsConfig
    {

        public string Respuesta { get; set; }

        public int FK_Id_AsignacionBuckets { get; set; }
        public int FK_Cat_AsignacionBuckets { get; set; }
        public int OrdenDatos { get; set; }
        public string Accion { get; set; }
    }

    //public class AsignacionBucketsConfig_conf
    //{

    //    public string Respuesta { get; set; }

    //    public int FK_Id_AsignacionBuckets { get; set; }
    //    public int FK_Cat_AsignacionBuckets { get; set; }
    //    public int OrdenDatos { get; set; }
    //    public string Accion { get; set; }
    //}

}
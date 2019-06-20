using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class GestionNotificacionCobro
    {
        public string Empresa { get; set; }
        public string Empresa_Comercial { get; set; }
        public string Ley { get; set; }
        public string Telefono_Fijo { get; set; }
        public string Telefono_Celular { get; set; }
        public string Porcentaje { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Ruta_Plantilla { set; get; }
        public string Ruta_Carpeta_Temp { set; get; }
        public string Nombre_Arch_Prefijo { set; get; }
        public string RutaTemp { set; get; }
        public string Mensaje { get; set; }
        public string NombrePDF { get; set; }
        public string NombreDOCX { get; set; }
        public string TelefonoDomicilio { get; set; }
        public string DireccionDomicilio { get; set; }
        public string LugarTrabajo { get; set; }
        public string TelefonoTrabajo { get; set; }
        public string DireccionTrabajo { get; set; }
        public string Total { get; set; }
        public string Cnx_Blob_Storage { get; set; }
        public string Nombre_Arch { get; set; }
        public string Formato_Arch { get; set; }
        public string Usuario_Ingreso { get; set; }
        public string Ruta_Almacenar_Docs { get; set; }
        public int IdCredito { get; set; }
        public string Tipo_Documento { get; set; }
        public string Url_Contenedor { get; set; }

        /// <summary>
        /// NOMBRE DEL DOC QUE SE ALMACENO EN EL BLOB STORAGE
        /// </summary>
        public string Nombre_Doc { get; set; }

        public string Arch_Eliminar { get; set; }


    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TwoFunTwoMeFintech.Models.DTO.ListaNegra
{
    public class dto_ListaNegra
    {

        //PROPIEDADES Y ATRIBUTOS DE OBJETOS LISTA NEGRA
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Motivo { get; set; }
        public bool? Activo { get; set; }
        public string Accion { get; set; }
        public string Identificacion { get; set; }
        public string AccionMantemiento { get; set; }
        public string Respuesta { get; set; }
        public string UsrModifica { get; set; }
        public string Telefono { get; set; }

        //*************************************************

        public List<dto_ListaNegra> list_ListaNegra{ get; set; }
      
    }

}
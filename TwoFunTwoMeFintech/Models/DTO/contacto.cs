<<<<<<< HEAD
﻿using System.Collections.Generic;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class contacto
    {
		public int? IdTelefono { set; get; }
		public int? IdDireccion { set; get; }
		public string Identificacion { set; get; }
		public string Categoria { set; get; }
		public string FuenteDatos { set; get; }
        public string Relacion { set; get; }
        public string FechaDato { set; get; }
        public string Tipo { set; get; }
        public int? Telefono { set; get; }
        public string TipoReferencia { set; get; }
        public string Nombre { set; get; }
        public string SupervisorDirectoTrabajo { set; get; }
		public int? Predeterminado { set; get; }
		public string Mensaje { set; get; }
		public List<CatalogoTelefono> ListcatalogoTelefono { get; set; }
		public List<CatalogoDireccion> ListCatalogoDireccion { get; set; }

		public string Direccion { set; get; }
        public int? calificacion { set; get; }
	}
=======
﻿namespace TwoFunTwoMeFintech.Models.DTO
{
    public class contacto
    {

        public string Identificacion { set; get; }
        public string FuenteDatos { set; get; }
        public string Relacion { set; get; }
        public string FechaDato { set; get; }
        public string Tipo { set; get; }
        public string Telefono { set; get; }

    }
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
}
using System.Collections.Generic;
using System.Xml.Serialization;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class CreditXml
    {
        [XmlRoot(ElementName = "IndiceDesarrolloSocial")]
        public class IndiceDesarrolloSocial
        {
            [XmlElement(ElementName = "Indice")]
            public string Indice { get; set; }
            [XmlElement(ElementName = "Nivel")]
            public string Nivel { get; set; }
        }

        [XmlRoot(ElementName = "DomicilioElectoral")]
        public class DomicilioElectoral
        {
            [XmlElement(ElementName = "Distrito")]
            public string Distrito { get; set; }
            [XmlElement(ElementName = "Distrito_Administrativo")]
            public string Distrito_Administrativo { get; set; }
            [XmlElement(ElementName = "Canton")]
            public string Canton { get; set; }
            [XmlElement(ElementName = "Provincia")]
            public string Provincia { get; set; }
            [XmlElement(ElementName = "CodigoAdministrativo")]
            public string CodigoAdministrativo { get; set; }
            [XmlElement(ElementName = "CodigoAdministrativo_Provincia")]
            public string CodigoAdministrativo_Provincia { get; set; }
            [XmlElement(ElementName = "CodigoAdministrativo_Canton")]
            public string CodigoAdministrativo_Canton { get; set; }
            [XmlElement(ElementName = "CodigoAdministrativo_Distrito")]
            public string CodigoAdministrativo_Distrito { get; set; }
        }

        [XmlRoot(ElementName = "FiliacionFisica")]
        public class FiliacionFisica
        {
            [XmlElement(ElementName = "Identificacion")]
            public string Identificacion { get; set; }
            [XmlElement(ElementName = "Nombre")]
            public string Nombre { get; set; }
            [XmlElement(ElementName = "Apellido1")]
            public string Apellido1 { get; set; }
            [XmlElement(ElementName = "Apellido2")]
            public string Apellido2 { get; set; }
            [XmlElement(ElementName = "ConocidoComo")]
            public string ConocidoComo { get; set; }
            [XmlElement(ElementName = "Nacionalidad")]
            public string Nacionalidad { get; set; }
            [XmlElement(ElementName = "Pais")]
            public string Pais { get; set; }
            [XmlElement(ElementName = "TipoIdentificacion")]
            public string TipoIdentificacion { get; set; }
            [XmlElement(ElementName = "VencimientoCedula")]
            public string VencimientoCedula { get; set; }
            [XmlElement(ElementName = "FechaNacimiento")]
            public string FechaNacimiento { get; set; }
            [XmlElement(ElementName = "Edad")]
            public string Edad { get; set; }
            [XmlElement(ElementName = "LugarNacimiento")]
            public string LugarNacimiento { get; set; }
            [XmlElement(ElementName = "Genero")]
            public string Genero { get; set; }
            [XmlElement(ElementName = "GeneroLiteral")]
            public string GeneroLiteral { get; set; }
            [XmlElement(ElementName = "Defuncion")]
            public string Defuncion { get; set; }
            [XmlElement(ElementName = "IndiceDesarrolloSocial")]
            public IndiceDesarrolloSocial IndiceDesarrolloSocial { get; set; }
            [XmlElement(ElementName = "DomicilioElectoral")]
            public DomicilioElectoral DomicilioElectoral { get; set; }
        }

        [XmlRoot(ElementName = "ActividadActual")]
        public class ActividadActual
        {
            [XmlElement(ElementName = "Asalariado")]
            public string Asalariado { get; set; }
            [XmlElement(ElementName = "Independiente")]
            public string Independiente { get; set; }
            [XmlElement(ElementName = "EconomicamenteActivo")]
            public string EconomicamenteActivo { get; set; }
        }

        [XmlRoot(ElementName = "HistoricoLaboral")]
        public class HistoricoLaboral
        {
            [XmlElement(ElementName = "Patrono")]
            public string Patrono { get; set; }
            [XmlElement(ElementName = "RazonSocial")]
            public string RazonSocial { get; set; }
            [XmlElement(ElementName = "FechaInicial")]
            public string FechaInicial { get; set; }
            [XmlElement(ElementName = "FechaFinal")]
            public string FechaFinal { get; set; }
            [XmlElement(ElementName = "CantidadTiempo")]
            public string CantidadTiempo { get; set; }
            [XmlElement(ElementName = "MontoAproximado")]
            public string MontoAproximado { get; set; }
        }

        [XmlRoot(ElementName = "ActividadEconomica")]
        public class ActividadEconomica
        {
            [XmlElement(ElementName = "ActividadActual")]
            public ActividadActual ActividadActual { get; set; }
            [XmlElement(ElementName = "HistoricoLaboral")]
            public List<HistoricoLaboral> HistoricoLaboral { get; set; }
        }

        [XmlRoot(ElementName = "Matrimonios")]
        public class Matrimonios
        {
            [XmlElement(ElementName = "Relacion")]
            public string Relacion { get; set; }
            [XmlElement(ElementName = "Actual")]
            public string Actual { get; set; }
        }

        [XmlRoot(ElementName = "HijosMenores")]
        public class HijosMenores
        {
            [XmlElement(ElementName = "Cantidad")]
            public string Cantidad { get; set; }
            [XmlElement(ElementName = "Rango")]
            public string Rango { get; set; }
        }

        [XmlRoot(ElementName = "Parientes")]
        public class Parientes
        {
            [XmlElement(ElementName = "Identificacion")]
            public string Identificacion { get; set; }
            [XmlElement(ElementName = "Nombre")]
            public string Nombre { get; set; }
            [XmlElement(ElementName = "Apellido1")]
            public string Apellido1 { get; set; }
            [XmlElement(ElementName = "Apellido2")]
            public string Apellido2 { get; set; }
            [XmlElement(ElementName = "Genero")]
            public string Genero { get; set; }
            [XmlElement(ElementName = "NombrePadre")]
            public string NombrePadre { get; set; }
            [XmlElement(ElementName = "NombreMadre")]
            public string NombreMadre { get; set; }
            [XmlElement(ElementName = "IdentificacionMadre")]
            public string IdentificacionMadre { get; set; }
            [XmlElement(ElementName = "IdentificacionPadre")]
            public string IdentificacionPadre { get; set; }
            [XmlElement(ElementName = "FechaNacimiento")]
            public string FechaNacimiento { get; set; }
            [XmlElement(ElementName = "Edad")]
            public string Edad { get; set; }
            [XmlElement(ElementName = "Defuncion")]
            public string Defuncion { get; set; }
            [XmlElement(ElementName = "Relacion")]
            public string Relacion { get; set; }
        }

        [XmlRoot(ElementName = "Items")]
        public class Items
        {
            [XmlElement(ElementName = "PorcentajeProbabilidad")]
            public string PorcentajeProbabilidad { get; set; }
            [XmlElement(ElementName = "Identificacion")]
            public string Identificacion { get; set; }
            [XmlElement(ElementName = "Nombre")]
            public string Nombre { get; set; }
            [XmlElement(ElementName = "Alias")]
            public string Alias { get; set; }
            [XmlElement(ElementName = "Sexo")]
            public string Sexo { get; set; }
            [XmlElement(ElementName = "Nacionalidad")]
            public string Nacionalidad { get; set; }
            [XmlElement(ElementName = "FechaNacimiento")]
            public string FechaNacimiento { get; set; }
            [XmlElement(ElementName = "LugarNacimiento")]
            public string LugarNacimiento { get; set; }
            [XmlElement(ElementName = "BuscadoPor")]
            public string BuscadoPor { get; set; }
            [XmlElement(ElementName = "CicatricesMarcas")]
            public string CicatricesMarcas { get; set; }
            [XmlElement(ElementName = "Comentarios")]
            public string Comentarios { get; set; }
            [XmlElement(ElementName = "Ocupacion")]
            public string Ocupacion { get; set; }
            [XmlElement(ElementName = "Idiomas")]
            public string Idiomas { get; set; }
            [XmlElement(ElementName = "FugitivoNumero")]
            public string FugitivoNumero { get; set; }
            [XmlElement(ElementName = "Jurisdiccion")]
            public string Jurisdiccion { get; set; }
            [XmlElement(ElementName = "UltimoDomicilio")]
            public string UltimoDomicilio { get; set; }
            [XmlElement(ElementName = "Ciudad")]
            public string Ciudad { get; set; }
            [XmlElement(ElementName = "FechaEfectiva")]
            public string FechaEfectiva { get; set; }
            [XmlElement(ElementName = "FechaExpiracion")]
            public string FechaExpiracion { get; set; }
            [XmlElement(ElementName = "Paginaweb")]
            public string Paginaweb { get; set; }
            [XmlElement(ElementName = "Fuente")]
            public string Fuente { get; set; }
        }

        [XmlRoot(ElementName = "ListasInternacionales")]
        public class ListasInternacionales
        {
            [XmlElement(ElementName = "Fuente")]
            public string Fuente { get; set; }
            [XmlElement(ElementName = "Cod_Fuente")]
            public string Cod_Fuente { get; set; }
            [XmlElement(ElementName = "Status")]
            public string Status { get; set; }
            [XmlElement(ElementName = "Total")]
            public string Total { get; set; }
            [XmlElement(ElementName = "Items")]
            public Items Items { get; set; }
        }

        [XmlRoot(ElementName = "Localizacion")]
        public class Localizacion
        {
            [XmlElement(ElementName = "Identificacion")]
            public string Identificacion { get; set; }
            [XmlElement(ElementName = "Nombre")]
            public string Nombre { get; set; }
            [XmlElement(ElementName = "Genero")]
            public string Genero { get; set; }
            [XmlElement(ElementName = "Edad")]
            public string Edad { get; set; }
            [XmlElement(ElementName = "Relacion")]
            public string Relacion { get; set; }
            [XmlElement(ElementName = "Dato")]
            public string Dato { get; set; }
            [XmlElement(ElementName = "DireccionRelacionada")]
            public string DireccionRelacionada { get; set; }
            [XmlElement(ElementName = "Fecha")]
            public string Fecha { get; set; }
            [XmlElement(ElementName = "Tipo")]
            public string Tipo { get; set; }
        }

        [XmlRoot(ElementName = "CCSS")]
        public class CCSS
        {
            [XmlElement(ElementName = "EstadoPatrono")]
            public string EstadoPatrono { get; set; }
            [XmlElement(ElementName = "LugarPago")]
            public string LugarPago { get; set; }
            [XmlElement(ElementName = "MontoAdeudado")]
            public string MontoAdeudado { get; set; }
            [XmlElement(ElementName = "Situacion")]
            public string Situacion { get; set; }
        }

        [XmlRoot(ElementName = "Credid")]
        public class Credid
        {
            [XmlElement(ElementName = "Tipo")]
            public string Tipo { get; set; }
            [XmlElement(ElementName = "FiliacionFisica")]
            public FiliacionFisica FiliacionFisica { get; set; }
            [XmlElement(ElementName = "ActividadEconomica")]
            public ActividadEconomica ActividadEconomica { get; set; }
            [XmlElement(ElementName = "PEP")]
            public string PEP { get; set; }
            [XmlElement(ElementName = "Matrimonios")]
            public Matrimonios Matrimonios { get; set; }
            [XmlElement(ElementName = "HijosMenores")]
            public List<HijosMenores> HijosMenores { get; set; }
            [XmlElement(ElementName = "ParientesTotalHijos")]
            public string ParientesTotalHijos { get; set; }
            [XmlElement(ElementName = "Parientes")]
            public List<Parientes> Parientes { get; set; }
            [XmlElement(ElementName = "APNFD")]
            public string APNFD { get; set; }
            [XmlElement(ElementName = "ListasInternacionalesCoincidenciaExacta")]
            public string ListasInternacionalesCoincidenciaExacta { get; set; }
            [XmlElement(ElementName = "ListasInternacionales")]
            public ListasInternacionales ListasInternacionales { get; set; }
            [XmlElement(ElementName = "Localizacion")]
            public List<Localizacion> Localizacion { get; set; }
            [XmlElement(ElementName = "CCSS")]
            public CCSS CCSS { get; set; }
            [XmlElement(ElementName = "Contribuyente")]
            public string Contribuyente { get; set; }
            [XmlElement(ElementName = "Fotografia")]
            public string Fotografia { get; set; }
            [XmlElement(ElementName = "UltimaConulta")]
            public string UltimaConulta { get; set; }
            [XmlElement(ElementName = "ConsultaId")]
            public string ConsultaId { get; set; }
        }
    }
}
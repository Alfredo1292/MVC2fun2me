using System.Collections.Generic;
using System.Xml.Serialization;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class TucaXml
    {
        [XmlRoot(ElementName = "hd")]
        public class Hd
        {
            [XmlElement(ElementName = "result")]
            public string Result { get; set; }
            [XmlElement(ElementName = "msg")]
            public string Msg { get; set; }
            [XmlElement(ElementName = "idPaisIdentificacion")]
            public string IdPaisIdentificacion { get; set; }
        }

        [XmlRoot(ElementName = "par")]
        public class Par
        {
            [XmlAttribute(AttributeName = "tipo")]
            public string Tipo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "parametrosConsulta")]
        public class ParametrosConsulta
        {
            [XmlElement(ElementName = "par")]
            public List<Par> Par { get; set; }
        }

        [XmlRoot(ElementName = "pais")]
        public class Pais
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "producto")]
        public class Producto
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "entidadConsultante")]
        public class EntidadConsultante
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "usuario")]
        public class Usuario
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "motivo")]
        public class Motivo
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "fechaReporte")]
        public class FechaReporte
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "encabezado")]
        public class Encabezado
        {
            [XmlElement(ElementName = "parametrosConsulta")]
            public ParametrosConsulta ParametrosConsulta { get; set; }
            [XmlElement(ElementName = "pais")]
            public Pais Pais { get; set; }
            [XmlElement(ElementName = "producto")]
            public Producto Producto { get; set; }
            [XmlElement(ElementName = "entidadConsultante")]
            public EntidadConsultante EntidadConsultante { get; set; }
            [XmlElement(ElementName = "usuario")]
            public Usuario Usuario { get; set; }
            [XmlElement(ElementName = "motivo")]
            public Motivo Motivo { get; set; }
            [XmlElement(ElementName = "fechaReporte")]
            public FechaReporte FechaReporte { get; set; }
            [XmlElement(ElementName = "identificadorEstudio")]
            public string IdentificadorEstudio { get; set; }
        }

        [XmlRoot(ElementName = "genero")]
        public class Genero
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "estadoCivil")]
        public class EstadoCivil
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "estado")]
        public class Estado
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "identificacionSujeto")]
        public class IdentificacionSujeto
        {
            [XmlElement(ElementName = "tipo")]
            public string Tipo { get; set; }
            [XmlElement(ElementName = "numero")]
            public string Numero { get; set; }
            [XmlElement(ElementName = "estado")]
            public Estado Estado { get; set; }
        }

        [XmlRoot(ElementName = "identificacionesSujeto")]
        public class IdentificacionesSujeto
        {
            [XmlElement(ElementName = "identificacionSujeto")]
            public IdentificacionSujeto IdentificacionSujeto { get; set; }
        }

        [XmlRoot(ElementName = "datosGenerales")]
        public class DatosGenerales
        {
            [XmlElement(ElementName = "primerApellido")]
            public string PrimerApellido { get; set; }
            [XmlElement(ElementName = "segundoApellido")]
            public string SegundoApellido { get; set; }
            [XmlElement(ElementName = "apellidoCasada")]
            public string ApellidoCasada { get; set; }
            [XmlElement(ElementName = "primerNombre")]
            public string PrimerNombre { get; set; }
            [XmlElement(ElementName = "segundoNombre")]
            public string SegundoNombre { get; set; }
            [XmlElement(ElementName = "nombreCompleto")]
            public string NombreCompleto { get; set; }
            [XmlElement(ElementName = "fechaNacimiento")]
            public string FechaNacimiento { get; set; }
            [XmlElement(ElementName = "edad")]
            public string Edad { get; set; }
            [XmlElement(ElementName = "origen")]
            public string Origen { get; set; }
            [XmlElement(ElementName = "genero")]
            public Genero Genero { get; set; }
            [XmlElement(ElementName = "estadoCivil")]
            public EstadoCivil EstadoCivil { get; set; }
            [XmlElement(ElementName = "dependientes")]
            public string Dependientes { get; set; }
            [XmlElement(ElementName = "estadoVida")]
            public string EstadoVida { get; set; }
            [XmlElement(ElementName = "identificacionesSujeto")]
            public IdentificacionesSujeto IdentificacionesSujeto { get; set; }
        }

        [XmlRoot(ElementName = "modelo")]
        public class Modelo
        {
            [XmlElement(ElementName = "producto")]
            public Producto Producto { get; set; }
            [XmlElement(ElementName = "resultado")]
            public string Resultado { get; set; }
            [XmlElement(ElementName = "razones")]
            public string Razones { get; set; }
        }

        [XmlRoot(ElementName = "modelosAnalisis")]
        public class ModelosAnalisis
        {
            [XmlElement(ElementName = "modelo")]
            public Modelo Modelo { get; set; }
        }

        [XmlRoot(ElementName = "tipo")]
        public class Tipo
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "divisionGeografica1")]
        public class DivisionGeografica1
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "divisionGeografica2")]
        public class DivisionGeografica2
        {
            [XmlAttribute(AttributeName = "codigo")]
            public string Codigo { get; set; }
            [XmlText]
            public string Text { get; set; }
        }

        [XmlRoot(ElementName = "direccion")]
        public class Direccion
        {
            [XmlElement(ElementName = "divisionGeografica1")]
            public DivisionGeografica1 DivisionGeografica1 { get; set; }
            [XmlElement(ElementName = "divisionGeografica2")]
            public DivisionGeografica2 DivisionGeografica2 { get; set; }
            [XmlElement(ElementName = "desc")]
            public string Desc { get; set; }
        }

        [XmlRoot(ElementName = "direccionYTelefono")]
        public class DireccionYTelefono
        {
            [XmlElement(ElementName = "tipo")]
            public Tipo Tipo { get; set; }
            [XmlElement(ElementName = "direccion")]
            public Direccion Direccion { get; set; }
        }

        [XmlRoot(ElementName = "localizacion")]
        public class Localizacion
        {
            [XmlElement(ElementName = "direccionYTelefono")]
            public List<DireccionYTelefono> DireccionYTelefono { get; set; }
        }

        [XmlRoot(ElementName = "resumenCuentas")]
        public class ResumenCuentas
        {
            [XmlElement(ElementName = "obligacionesVigentes")]
            public string ObligacionesVigentes { get; set; }
            [XmlElement(ElementName = "obligacionesCerradas")]
            public string ObligacionesCerradas { get; set; }
        }

        [XmlRoot(ElementName = "consulta")]
        public class Consulta
        {
            [XmlElement(ElementName = "tipoConsulta")]
            public string TipoConsulta { get; set; }
            [XmlElement(ElementName = "entidadConsultante")]
            public EntidadConsultante EntidadConsultante { get; set; }
            [XmlElement(ElementName = "fecha")]
            public string Fecha { get; set; }
            [XmlElement(ElementName = "motivo")]
            public Motivo Motivo { get; set; }
        }

        [XmlRoot(ElementName = "listaConsultas")]
        public class ListaConsultas
        {
            [XmlElement(ElementName = "consulta")]
            public List<Consulta> Consulta { get; set; }
        }

        [XmlRoot(ElementName = "reporteCredito")]
        public class ReporteCredito
        {
            [XmlElement(ElementName = "hd")]
            public Hd Hd { get; set; }
            [XmlElement(ElementName = "parametrosConsulta")]
            public ParametrosConsulta ParametrosConsulta { get; set; }
            [XmlElement(ElementName = "encabezado")]
            public Encabezado Encabezado { get; set; }
            [XmlElement(ElementName = "datosGenerales")]
            public DatosGenerales DatosGenerales { get; set; }
            [XmlElement(ElementName = "modelosAnalisis")]
            public ModelosAnalisis ModelosAnalisis { get; set; }
            [XmlElement(ElementName = "localizacion")]
            public Localizacion Localizacion { get; set; }
            [XmlElement(ElementName = "resumenCuentas")]
            public ResumenCuentas ResumenCuentas { get; set; }
            [XmlElement(ElementName = "cuentasDeposito")]
            public string CuentasDeposito { get; set; }
            [XmlElement(ElementName = "monedaCredito")]
            public string MonedaCredito { get; set; }
            [XmlElement(ElementName = "comportamientoObligaciones")]
            public string ComportamientoObligaciones { get; set; }
            [XmlElement(ElementName = "listaFacturas")]
            public string ListaFacturas { get; set; }
            [XmlElement(ElementName = "garantias")]
            public string Garantias { get; set; }
            [XmlElement(ElementName = "declaracionesCiudadano")]
            public string DeclaracionesCiudadano { get; set; }
            [XmlElement(ElementName = "listaConsultas")]
            public ListaConsultas ListaConsultas { get; set; }
            [XmlElement(ElementName = "controlReporte")]
            public string ControlReporte { get; set; }
        }
    }
}
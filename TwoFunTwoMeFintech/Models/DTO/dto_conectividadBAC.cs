using System.Xml.Serialization;

namespace TwoFunTwoMeFintech.Models.DTO
{
    public class dto_conectividadBAC
    {
        [XmlRoot(ElementName = "Detalle")]
        public class Detalle
        {
            [XmlElement(ElementName = "NumeroFactura")]
            public string NumeroFactura { get; set; }
            [XmlElement(ElementName = "ValorServicio")]
            public string ValorServicio { get; set; }
            [XmlElement(ElementName = "PeriodoRecibo")]
            public string PeriodoRecibo { get; set; }
            [XmlElement(ElementName = "MontoTotal")]
            public string MontoTotal { get; set; }
            [XmlElement(ElementName = "FechaVencimiento")]
            public string FechaVencimiento { get; set; }
            [XmlElement(ElementName = "SelfVerif")]
            public string SelfVerif { get; set; }
        }

        [XmlRoot(ElementName = "ConsultaResult")]
        public class ConsultaResult
        {
            [XmlElement(ElementName = "TipoMensaje")]
            public string TipoMensaje { get; set; }
            [XmlElement(ElementName = "CodBanco")]
            public string CodBanco { get; set; }
            [XmlElement(ElementName = "Agencia")]
            public string Agencia { get; set; }
            [XmlElement(ElementName = "Convenio")]
            public string Convenio { get; set; }
            [XmlElement(ElementName = "CodRespuesta")]
            public string CodRespuesta { get; set; }
            [XmlElement(ElementName = "TipoIdentificacion")]
            public string TipoIdentificacion { get; set; }
            [XmlElement(ElementName = "Identificacion")]
            public string Identificacion { get; set; }
            [XmlElement(ElementName = "NombreCliente")]
            public string NombreCliente { get; set; }
            [XmlElement(ElementName = "CantidadServicios")]
            public string CantidadServicios { get; set; }
            [XmlElement(ElementName = "Detalle")]
            public Detalle Detalle { get; set; }
            [XmlElement(ElementName = "NumeroCuota")]
            public string NumeroCuota { get; set; }
        }

        [XmlRoot(ElementName = "Respuesta_Consulta_Recibos")]
        public class Respuesta_Consulta_Recibos
        {
            [XmlElement(ElementName = "ConsultaResult")]
            public ConsultaResult ConsultaResult { get; set; }
        }
    }
}
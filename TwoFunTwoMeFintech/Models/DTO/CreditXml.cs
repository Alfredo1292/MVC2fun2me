/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TwoFunTwoMeFintech.Models.DTO
{
    [Serializable]
    [XmlRoot(ElementName = "IndiceDesarrolloSocial"), JsonObject]
    public class IndiceDesarrolloSocial
    {
        [XmlElement(ElementName = "Indice"), JsonProperty]
        public string Indice { get; set; }
        [XmlElement(ElementName = "Nivel"), JsonProperty]
        public string Nivel { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Profesion"), JsonObject]
    public class Profesion
    {
        [XmlElement(ElementName = "Descripcion"), JsonProperty]
        public string Descripcion { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "ColegioProfesional"), JsonObject]
    public class ColegioProfesional
    {
        [XmlElement(ElementName = "Colegio"), JsonProperty]
        public string Colegio { get; set; }
        [XmlElement(ElementName = "Carne"), JsonProperty]
        public string Carne { get; set; }
        [XmlElement(ElementName = "Estado"), JsonProperty]
        public string Estado { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "DomicilioElectoral"), JsonObject]
    public class DomicilioElectoral
    {
        [XmlElement(ElementName = "Distrito"), JsonProperty]
        public string Distrito { get; set; }
        [XmlElement(ElementName = "Distrito_Administrativo"), JsonProperty]
        public string Distrito_Administrativo { get; set; }
        [XmlElement(ElementName = "Canton"), JsonProperty]
        public string Canton { get; set; }
        [XmlElement(ElementName = "Provincia"), JsonProperty]
        public string Provincia { get; set; }
        [XmlElement(ElementName = "CodigoAdministrativo"), JsonProperty]
        public string CodigoAdministrativo { get; set; }
        [XmlElement(ElementName = "CodigoAdministrativo_Provincia"), JsonProperty]
        public string CodigoAdministrativo_Provincia { get; set; }
        [XmlElement(ElementName = "CodigoAdministrativo_Canton"), JsonProperty]
        public string CodigoAdministrativo_Canton { get; set; }
        [XmlElement(ElementName = "CodigoAdministrativo_Distrito"), JsonProperty]
        public string CodigoAdministrativo_Distrito { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "FiliacionFisica"), JsonObject]
    public class FiliacionFisica
    {
        [XmlElement(ElementName = "Identificacion"), JsonProperty]
        public string Identificacion { get; set; }
        [XmlElement(ElementName = "Nombre"), JsonProperty]
        public string Nombre { get; set; }
        [XmlElement(ElementName = "Apellido1"), JsonProperty]
        public string Apellido1 { get; set; }
        [XmlElement(ElementName = "Apellido2"), JsonProperty]
        public string Apellido2 { get; set; }
        [XmlElement(ElementName = "ConocidoComo"), JsonProperty]
        public string ConocidoComo { get; set; }
        [XmlElement(ElementName = "Nacionalidad"), JsonProperty]
        public string Nacionalidad { get; set; }
        [XmlElement(ElementName = "Pais"), JsonProperty]
        public string Pais { get; set; }
        [XmlElement(ElementName = "TipoIdentificacion"), JsonProperty]
        public string TipoIdentificacion { get; set; }
        [XmlElement(ElementName = "VencimientoCedula"), JsonProperty]
        public string VencimientoCedula { get; set; }
        [XmlElement(ElementName = "FechaNacimiento"), JsonProperty]
        public string FechaNacimiento { get; set; }
        [XmlElement(ElementName = "Edad"), JsonProperty]
        public string Edad { get; set; }
        [XmlElement(ElementName = "LugarNacimiento"), JsonProperty]
        public string LugarNacimiento { get; set; }
        [XmlElement(ElementName = "Genero"), JsonProperty]
        public string Genero { get; set; }
        [XmlElement(ElementName = "GeneroLiteral"), JsonProperty]
        public string GeneroLiteral { get; set; }
        [XmlElement(ElementName = "Defuncion"), JsonProperty]
        public string Defuncion { get; set; }
        [XmlElement(ElementName = "IndiceDesarrolloSocial"), JsonProperty]
        public IndiceDesarrolloSocial IndiceDesarrolloSocial { get; set; }
        [XmlElement(ElementName = "Profesion"), JsonProperty]
        public List<Profesion> Profesion { get; set; }
        [XmlElement(ElementName = "ColegioProfesional"), JsonProperty]
        public ColegioProfesional ColegioProfesional { get; set; }
        [XmlElement(ElementName = "DomicilioElectoral"), JsonProperty]
        public DomicilioElectoral DomicilioElectoral { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "ActividadActual"), JsonObject]
    public class ActividadActual
    {
        [XmlElement(ElementName = "Asalariado"), JsonProperty]
        public string Asalariado { get; set; }
        [XmlElement(ElementName = "Independiente"), JsonProperty]
        public string Independiente { get; set; }
        [XmlElement(ElementName = "EconomicamenteActivo"), JsonProperty]
        public string EconomicamenteActivo { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "HistoricoLaboral"), JsonObject]
    public class HistoricoLaboral
    {
        [XmlElement(ElementName = "Patrono"), JsonProperty]
        public string Patrono { get; set; }
        [XmlElement(ElementName = "RazonSocial"), JsonProperty]
        public string RazonSocial { get; set; }
        [XmlElement(ElementName = "FechaInicialDiasAHoy"), JsonProperty]
        public string FechaInicialDiasAHoy { get; set; }
        [XmlElement(ElementName = "FechaInicial"), JsonProperty]
        public string FechaInicial { get; set; }
        [XmlElement(ElementName = "FechaFinal"), JsonProperty]
        public string FechaFinal { get; set; }
        [XmlElement(ElementName = "CantidadTiempo"), JsonProperty]
        public string CantidadTiempo { get; set; }
        [XmlElement(ElementName = "MontoAproximado"), JsonProperty]
        public string MontoAproximado { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "ActividadEconomica"), JsonObject]
    public class ActividadEconomica
    {
        [XmlElement(ElementName = "ActividadActual"), JsonProperty]
        public ActividadActual ActividadActual { get; set; }
        [XmlElement(ElementName = "HistoricoLaboral"), JsonProperty]
        public List<HistoricoLaboral> HistoricoLaboral { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Matrimonios"), JsonObject]
    public class Matrimonios
    {
        [XmlElement(ElementName = "Identificacion"), JsonProperty]
        public string Identificacion { get; set; }
        [XmlElement(ElementName = "Nombre"), JsonProperty]
        public string Nombre { get; set; }
        [XmlElement(ElementName = "Apellido1"), JsonProperty]
        public string Apellido1 { get; set; }
        [XmlElement(ElementName = "Apellido2"), JsonProperty]
        public string Apellido2 { get; set; }
        [XmlElement(ElementName = "FechaSuceso"), JsonProperty]
        public string FechaSuceso { get; set; }
        [XmlElement(ElementName = "Relacion"), JsonProperty]
        public string Relacion { get; set; }
        [XmlElement(ElementName = "TieneActividadEconomica"), JsonProperty]
        public string TieneActividadEconomica { get; set; }
        [XmlElement(ElementName = "Ley8204"), JsonProperty]
        public string Ley8204 { get; set; }
        [XmlElement(ElementName = "TotalReferenciasComerciales"), JsonProperty]
        public string TotalReferenciasComerciales { get; set; }
        [XmlElement(ElementName = "TotalJuicios"), JsonProperty]
        public string TotalJuicios { get; set; }
        [XmlElement(ElementName = "Actual"), JsonProperty]
        public string Actual { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "HijosMenores"), JsonObject]
    public class HijosMenores
    {
        [XmlElement(ElementName = "Cantidad"), JsonProperty]
        public string Cantidad { get; set; }
        [XmlElement(ElementName = "Rango"), JsonProperty]
        public string Rango { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "HijosMayores"), JsonObject]
    public class HijosMayores
    {
        [XmlElement(ElementName = "Cedula"), JsonProperty]
        public string Cedula { get; set; }
        [XmlElement(ElementName = "Edad"), JsonProperty]
        public string Edad { get; set; }
        [XmlElement(ElementName = "Bachillerato"), JsonProperty]
        public string Bachillerato { get; set; }
        [XmlElement(ElementName = "Profesion"), JsonProperty]
        public string Profesion { get; set; }
        [XmlElement(ElementName = "Matrimonios"), JsonProperty]
        public string Matrimonios { get; set; }
        [XmlElement(ElementName = "Hijos"), JsonProperty]
        public string Hijos { get; set; }
        [XmlElement(ElementName = "EconomicoActivo"), JsonProperty]
        public string EconomicoActivo { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Parientes"), JsonObject]
    public class Parientes
    {
        [XmlElement(ElementName = "Identificacion"), JsonProperty]
        public string Identificacion { get; set; }
        [XmlElement(ElementName = "Nombre"), JsonProperty]
        public string Nombre { get; set; }
        [XmlElement(ElementName = "Apellido1"), JsonProperty]
        public string Apellido1 { get; set; }
        [XmlElement(ElementName = "Apellido2"), JsonProperty]
        public string Apellido2 { get; set; }
        [XmlElement(ElementName = "Genero"), JsonProperty]
        public string Genero { get; set; }
        [XmlElement(ElementName = "NombrePadre"), JsonProperty]
        public string NombrePadre { get; set; }
        [XmlElement(ElementName = "NombreMadre"), JsonProperty]
        public string NombreMadre { get; set; }
        [XmlElement(ElementName = "IdentificacionMadre"), JsonProperty]
        public string IdentificacionMadre { get; set; }
        [XmlElement(ElementName = "IdentificacionPadre"), JsonProperty]
        public string IdentificacionPadre { get; set; }
        [XmlElement(ElementName = "FechaNacimiento"), JsonProperty]
        public string FechaNacimiento { get; set; }
        [XmlElement(ElementName = "Edad"), JsonProperty]
        public string Edad { get; set; }
        [XmlElement(ElementName = "Defuncion"), JsonProperty]
        public string Defuncion { get; set; }
        [XmlElement(ElementName = "Relacion"), JsonProperty]
        public string Relacion { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Localizacion"), JsonObject]
    public class Localizacion
    {
        [XmlElement(ElementName = "Identificacion"), JsonProperty]
        public string Identificacion { get; set; }
        [XmlElement(ElementName = "Nombre"), JsonProperty]
        public string Nombre { get; set; }
        [XmlElement(ElementName = "Genero"), JsonProperty]
        public string Genero { get; set; }
        [XmlElement(ElementName = "Edad"), JsonProperty]
        public string Edad { get; set; }
        [XmlElement(ElementName = "Relacion"), JsonProperty]
        public string Relacion { get; set; }
        [XmlElement(ElementName = "Dato"), JsonProperty]
        public string Dato { get; set; }
        [XmlElement(ElementName = "DireccionRelacionada"), JsonProperty]
        public string DireccionRelacionada { get; set; }
        [XmlElement(ElementName = "Fecha"), JsonProperty]
        public string Fecha { get; set; }
        [XmlElement(ElementName = "Tipo"), JsonProperty]
        public string Tipo { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Propiedades"), JsonObject]
    public class Propiedades
    {
        [XmlElement(ElementName = "Numero"), JsonProperty]
        public string Numero { get; set; }
        [XmlElement(ElementName = "Medida"), JsonProperty]
        public string Medida { get; set; }
        [XmlElement(ElementName = "Plano"), JsonProperty]
        public string Plano { get; set; }
        [XmlElement(ElementName = "Distrito"), JsonProperty]
        public string Distrito { get; set; }
        [XmlElement(ElementName = "Canton"), JsonProperty]
        public string Canton { get; set; }
        [XmlElement(ElementName = "Provincia"), JsonProperty]
        public string Provincia { get; set; }
        [XmlElement(ElementName = "ValorFiscal"), JsonProperty]
        public string ValorFiscal { get; set; }
        [XmlElement(ElementName = "ValorHipotecas"), JsonProperty]
        public string ValorHipotecas { get; set; }
        [XmlElement(ElementName = "CantidadEmbargos"), JsonProperty]
        public string CantidadEmbargos { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "CCSS"), JsonObject]
    public class CCSS
    {
        [XmlElement(ElementName = "EstadoPatrono"), JsonProperty]
        public string EstadoPatrono { get; set; }
        [XmlElement(ElementName = "LugarPago"), JsonProperty]
        public string LugarPago { get; set; }
        [XmlElement(ElementName = "MontoAdeudado"), JsonProperty]
        public string MontoAdeudado { get; set; }
        [XmlElement(ElementName = "Situacion"), JsonProperty]
        public string Situacion { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "FODESAF"), JsonObject]
    public class FODESAF
    {
        [XmlElement(ElementName = "NumeroSegregado"), JsonProperty]
        public string NumeroSegregado { get; set; }
        [XmlElement(ElementName = "Estado"), JsonProperty]
        public string Estado { get; set; }
        [XmlElement(ElementName = "DeudaTotal"), JsonProperty]
        public string DeudaTotal { get; set; }
        [XmlElement(ElementName = "DeudaArreglos"), JsonProperty]
        public string DeudaArreglos { get; set; }
        [XmlElement(ElementName = "DeudaPeriodos"), JsonProperty]
        public string DeudaPeriodos { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Actividades"), JsonObject]
    public class Actividades
    {
        [XmlElement(ElementName = "Codigo"), JsonProperty]
        public string Codigo { get; set; }
        [XmlElement(ElementName = "Descripcion"), JsonProperty]
        public string Descripcion { get; set; }
        [XmlElement(ElementName = "Fecha"), JsonProperty]
        public string Fecha { get; set; }
        [XmlElement(ElementName = "FechaEntrada"), JsonProperty]
        public string FechaEntrada { get; set; }
        [XmlElement(ElementName = "Clasificacion"), JsonProperty]
        public string Clasificacion { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Obligaciones"), JsonObject]
    public class Obligaciones
    {
        [XmlElement(ElementName = "Descripcion"), JsonProperty]
        public string Descripcion { get; set; }
        [XmlElement(ElementName = "FechaInicio"), JsonProperty]
        public string FechaInicio { get; set; }
        [XmlElement(ElementName = "FechaIngreso"), JsonProperty]
        public string FechaIngreso { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Contribuyente"), JsonObject]
    public class Contribuyente
    {
        [XmlElement(ElementName = "Actividades"), JsonProperty]
        public Actividades Actividades { get; set; }
        [XmlElement(ElementName = "Obligaciones"), JsonProperty]
        public Obligaciones Obligaciones { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "ReferenciasComerciales"), JsonObject]
    public class ReferenciasComerciales
    {
        [XmlElement(ElementName = "Cliente"), JsonProperty]
        public string Cliente { get; set; }
        [XmlElement(ElementName = "FechaInicialDiasAHoy"), JsonProperty]
        public string FechaInicialDiasAHoy { get; set; }
        [XmlElement(ElementName = "fecha_informacion"), JsonProperty]
        public string Fecha_informacion { get; set; }
        [XmlElement(ElementName = "tipo_informacion"), JsonProperty]
        public string Tipo_informacion { get; set; }
        [XmlElement(ElementName = "tipo_deudor"), JsonProperty]
        public string Tipo_deudor { get; set; }
        [XmlElement(ElementName = "tipo_identificacion"), JsonProperty]
        public string Tipo_identificacion { get; set; }
        [XmlElement(ElementName = "tipo_credito"), JsonProperty]
        public string Tipo_credito { get; set; }
        [XmlElement(ElementName = "fecha_otorgamiento_credito"), JsonProperty]
        public string Fecha_otorgamiento_credito { get; set; }
        [XmlElement(ElementName = "codigo_estado_cuenta"), JsonProperty]
        public string Codigo_estado_cuenta { get; set; }
        [XmlElement(ElementName = "fecha_ultimo_pago"), JsonProperty]
        public string Fecha_ultimo_pago { get; set; }
        [XmlElement(ElementName = "fecha_cancelacion_credito"), JsonProperty]
        public string Fecha_cancelacion_credito { get; set; }
        [XmlElement(ElementName = "numero_cuenta_operacion"), JsonProperty]
        public string Numero_cuenta_operacion { get; set; }
        [XmlElement(ElementName = "fecha_vencimiento"), JsonProperty]
        public string Fecha_vencimiento { get; set; }
        [XmlElement(ElementName = "plazo"), JsonProperty]
        public string Plazo { get; set; }
        [XmlElement(ElementName = "cuotas_vencidas"), JsonProperty]
        public string Cuotas_vencidas { get; set; }
        [XmlElement(ElementName = "dias_mora"), JsonProperty]
        public string Dias_mora { get; set; }
        [XmlElement(ElementName = "tipo_moneda"), JsonProperty]
        public string Tipo_moneda { get; set; }
        [XmlElement(ElementName = "saldo_mora"), JsonProperty]
        public string Saldo_mora { get; set; }
    }
    [Serializable]
    [XmlRoot(ElementName = "Credid"), JsonObject]
    public class Credid
    {
        [XmlElement(ElementName = "Tipo"), JsonProperty]
        public string Tipo { get; set; }
        [XmlElement(ElementName = "FiliacionFisica"), JsonProperty]
        public FiliacionFisica FiliacionFisica { get; set; }
        [XmlElement(ElementName = "ActividadEconomica"), JsonProperty]
        public ActividadEconomica ActividadEconomica { get; set; }
        [XmlElement(ElementName = "PEP"), JsonProperty]
        public string PEP { get; set; }
        [XmlElement(ElementName = "Matrimonios"), JsonProperty]
        public Matrimonios Matrimonios { get; set; }
        [XmlElement(ElementName = "HijosMenores"), JsonProperty]
        public List<HijosMenores> HijosMenores { get; set; }
        [XmlElement(ElementName = "HijosMayores"), JsonProperty]
        public List<HijosMayores> HijosMayores { get; set; }
        [XmlElement(ElementName = "ParientesTotalHijos"), JsonProperty]
        public string ParientesTotalHijos { get; set; }
        [XmlElement(ElementName = "Parientes"), JsonProperty]
        public List<Parientes> Parientes { get; set; }
        [XmlElement(ElementName = "APNFD"), JsonProperty]
        public string APNFD { get; set; }
        [XmlElement(ElementName = "ListasInternacionalesCoincidenciaExacta"), JsonProperty]
        public string ListasInternacionalesCoincidenciaExacta { get; set; }
        [XmlElement(ElementName = "Localizacion"), JsonProperty]
        public List<Localizacion> Localizacion { get; set; }
        [XmlElement(ElementName = "Propiedades"), JsonProperty]
        public List<Propiedades> Propiedades { get; set; }
        [XmlElement(ElementName = "CCSS"), JsonProperty]
        public CCSS CCSS { get; set; }
        [XmlElement(ElementName = "FODESAF"), JsonProperty]
        public FODESAF FODESAF { get; set; }
        [XmlElement(ElementName = "Contribuyente"), JsonProperty]
        public Contribuyente Contribuyente { get; set; }
        [XmlElement(ElementName = "ReferenciasComerciales"), JsonProperty]
        public List<ReferenciasComerciales> ReferenciasComerciales { get; set; }
        [XmlElement(ElementName = "Fotografia"), JsonProperty]
        public string Fotografia { get; set; }
        [XmlElement(ElementName = "UltimaConulta"), JsonProperty]
        public string UltimaConulta { get; set; }
        [XmlElement(ElementName = "ConsultaId"), JsonProperty]
        public string ConsultaId { get; set; }
    }

}

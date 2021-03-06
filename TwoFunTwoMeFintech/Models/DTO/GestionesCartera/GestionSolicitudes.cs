﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TwoFunTwoMeFintech.Models.DTO.GestionesCartera
{
    public class GestionSolicitudes
    {
        [Key]
        public int? Id { get; set; }

        [DataMember]
        public List<Solicitudes> ListSolicitudes { get; set; }

        public List<productos> ListProductos { get; set; }

        public List<Tipos> ListTipos { get; set; }

        public int? ID_ESTADO { get; set; }

        public List<Score> ListadoScore { get; set; }

        public string Respuesta { get; set; }

        public int? IdBuro { get; set; }


        [Display(Name = "Producto")]
        public string NombreProducto { get; set; }
        public decimal Cuota { get; set; }

        public string Cliente { get; set; }

        [Display(Name = "Fecha Ingreso")]
        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public string UsrModifica { get; set; }

        public string Mensaje { get; set; }

        public string MensajeError { get; set; }

        public int? IdPersona { get; set; }

        public int? ROLID { get; set; }

        public int? Status { get; set; }

        public DateTime? FechaTransferencia { get; set; }

        public string Comentarios { get; set; }

        public string UsoCredito { get; set; }

        public bool? Consentimiento { get; set; }

        public int IdProducto { get; set; }

        public int? IdAgente { get; set; }

        public int? SoporteTipo { get; set; }

        public int? IdStatusOrden { get; set; }

        public bool? FotoCedula { get; set; }

        public bool? Recibo { get; set; }

        public bool? Contrato { get; set; }

        public bool? Pagare { get; set; }

        public bool? ReciboIngresos { get; set; }

        public bool? CuentaBancaria { get; set; }

        public bool? Selfie { get; set; }

        public string Origen { get; set; }

        public int? IdUsrAsig { get; set; }

        public int? IdEstadoProc { get; set; }

        public int? SubStatus { get; set; }

        public decimal? MontoMaximo { get; set; }

        public int? Score { get; set; }

        public string ScoreGini { get; set; }

        [Display(Name = "Numero de Solicitud:"), Required]
        public int ID_SOLICITUD { get; set; }

        [Display(Name = "Nombre")]
        public string NOMBRE { get; set; }

        [Display(Name = "Estado")]
        public string Descripcion { get; set; }

        [Display(Name = "TUCA")]
        public string XML_TUCA { get; set; }

        public string FECHA_INGRESO_TUCA { get; set; }

        [Display(Name = "Estado TUCA")]
        public string ESTADO_TUCA { get; set; }

        [Display(Name = "CREDID")]
        public string XML_CREDID { get; set; }

        public string FECHA_INGRESO_CREDID { get; set; }

        [Display(Name = "Estado CREDID")]
        public string ESTADO_CREDID { get; set; }

        [Display(Name = "EQUIFAX")]
        public string XML_EQUIFAX { get; set; }

        public string FECHA_INGRESO_EQUIFAX { get; set; }

        public int? IdTipoIdentificacion { get; set; }

        [Display(Name = "Identificación")]
        public string Identificacion { get; set; }

        public DateTime? VencimientoIdentificacion { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        [Display(Name = "Tel. Cel")]
        public int? TelefonoCel { get; set; }

        [Display(Name = "Tel. Fijo")]
        public int? TelefonoFijo { get; set; }

        [Display(Name = "Tel. Trabajo")]
        public int? TelefonoLaboral { get; set; }

        [Display(Name = "Correo")]
        public string Correo { get; set; }
        [Display(Name = "Tel. Trabajo")]
        public string CorreoOpcional { get; set; }

        public int? EstadoCivil { get; set; }

        public int? Sexo { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public int? Provincia { get; set; }

        public int? Canton { get; set; }

        public int? Distrito { get; set; }

        public string DetalleDireccion { get; set; }

        public bool forzamiento_solicitud { get; set; }

        public string USUARIO_MODIFICACION { get; set; }

        public decimal? Monto { get; set; }
        public string UrlFoto { get; set; }

        public string UrlFotoCedula { get; set; }

        public string UrlFotoSelfie { get; set; }

        public string UrlFotoFirma { get; set; }

        public string UrlDirectorioPagare { get; set; }

        public int IdSolicitud { get; set; }

        public string UrlFotoCedulaTrasera { get; set; }

        public float? UnMatchedFace { get; set; }

        public string ImageRotationSource { get; set; }

        public string ImageRotationTarget { get; set; }

        public string MatchedFace { get; set; }

        public float? PorcentMatched { get; set; }

        public float? PorcentUnmatched { get; set; }

        public float? PositionTop { get; set; }

        public float? PositionLeft { get; set; }

        public string ResultTextCed { get; set; }

        public string DetectedText { get; set; }

        public bool Result { get; set; }

        public int? TipoFoto { get; set; }

        public byte[] arrImageCedulaFrontal { get; set; }

        public byte[] arrImageSelfie { get; set; }
        public int? Dia { get; set; }
        public int? Mes { get; set; }
        public int? Anio { get; set; }
        public bool VigenciaCed { get; set; }
    }
}
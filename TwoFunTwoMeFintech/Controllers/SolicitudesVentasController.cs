using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.InsertaSolicitud;
using TwoFunTwoMeFintech.Models.Manager;

namespace TwoFunTwoMeFintech.Controllers
{
    public class SolicitudesVentasController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();
        private DTO_SOLICITUD_VENTAS solicitudes = new DTO_SOLICITUD_VENTAS();
        //
        // GET: /Solicitudes/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CargarResultadoScore(Score score)
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();
            var res = mang.ConsultaResultadoScore(score);

            return Json(res);
        }

        //
        // GET: /Solicitudes/Details/5

        public ActionResult Details(string id = "0")
        {
            solicitudes.Identificacion = id;
            solicitudes.ListSolicitud = new System.Collections.Generic.List<DTO_SOLICITUD_VENTAS>();
            //solicitudes.ListProductos = new List<productos>();
            //solicitudes.ListTipos = new List<Tipos>();
            // solicitudes.ListadoScore = new List<Score>();
            solicitudes.ListSolicitud.Add(solicitudes);

            ManagerSolcitudes manage = new ManagerSolcitudes();
            try
            {
                ViewBag.ListSolicitud = solicitudes;
            }
            catch
            {
                ViewBag.ErrorMessage = "Ocurrio un Error";
            }
            return View(solicitudes);

        }

        //
        // GET: /Solicitudes/Edit/5

        public ActionResult Buscar(string id = "0", string estado = "0", string agente = "0")
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ManagerSolcitudes mang = new ManagerSolcitudes();
                    solicitudes.Identificacion = id;
                    ///solicitudes.Status = estado;
                    solicitudes.FiltroAsesor = agente;
                    solicitudes.ListSolicitud = mang.ConsultaSolicitudPendientes(solicitudes);
                }
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            //return View(asignacionbuckets);
            return View("Details", solicitudes);
        }

        public ActionResult ActualizarSolicitud(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes mang = NewMethod();

            solicitudes.USUARIO_MODIFICACION = Session["agente"].ToString();
            var res = mang.CambiarEstadoRevisionDocumentos(solicitudes);

            return Json(res);
        }

        private static ManagerSolcitudes NewMethod()
        {
            return new ManagerSolcitudes();
        }

        public ActionResult CambioSinpe()
        {
            return View();
        }

        public ActionResult cansultaSimpe(Dto_Sinpe sinpe)
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();
            var dto_ret = mang.ConsultaSolicitudSinpe(sinpe);

            return Json(dto_ret);
        }
        public ActionResult EditaSimpe(Dto_Sinpe sinpe)
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();
            var dto_ret = mang.ConsultaSolicitudSinpe(sinpe);

            return Json(dto_ret);
        }
        public ActionResult ActualizaSimpe(Dto_Sinpe sinpe)
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();
            var dto_ret = mang.ConsultaSolicitudSinpe(sinpe);

            return Json(dto_ret);
        }

        public ActionResult Encabezado()
        {

            ManagerSolcitudes mang = new ManagerSolcitudes();
            //
            solicitudes.ListTipos = mang.CargarTipos("5");
            solicitudes.ListSolicitud = mang.ConsultaSolicitudDocumentos(solicitudes);
            var res = mang.ConsultaSolicitudDocumentos(solicitudes);

            return Json(res);

        }
        public ActionResult CargaTipos(Tipos tipos)
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();
            var dto_ret = mang.CargarTipos(tipos.Id.ToString());

            return Json(dto_ret.Where(x => x.Id == 113 || x.Id == 110));
        }


        public ActionResult BuscarDocumentos(string id = "0", int tipo = 0)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    ManagerSolcitudes mang = new ManagerSolcitudes();
                    //
                    solicitudes.ListTipos = mang.CargarTipos("5");
                    solicitudes.Solicitud = id;
                    solicitudes.tipo = tipo;
                    solicitudes.ListSolicitud = mang.ConsultaSolicitudDocumentos(solicitudes);


                    if (solicitudes.ListSolicitud.Count() == 0)
                    {
                        solicitudes.ListSolicitud = null;
                    }
                    else
                    {
                        Tab_ConfigSys dto_Config = new Tab_ConfigSys();
                        dto_Config.llave_Config1 = "SERVICIO";
                        dto_Config.llave_Config2 = "CONFIGURACION";
                        dto_Config.llave_Config3 = "SERVIDOR";
                        dto_Config.llave_Config4 = "URL";
                        var dto_interval = mang.ConsultaConfiUrlImagen(dto_Config);


                        descargarImagenes(solicitudes.ListSolicitud.FirstOrDefault(), dto_interval, 1);
                        descargarImagenes(solicitudes.ListSolicitud.FirstOrDefault(), dto_interval, 2);
                        descargarImagenes(solicitudes.ListSolicitud.FirstOrDefault(), dto_interval, 3);
                        descargarImagenes(solicitudes.ListSolicitud.FirstOrDefault(), dto_interval, 4);
                        descargarImagenes(solicitudes.ListSolicitud.FirstOrDefault(), dto_interval, 5);
                    }

                    solicitudes._PasosCredito = mang.ConsultaSolicitudPasos(solicitudes);


                }
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            //return View(asignacionbuckets);
            return View("Details", solicitudes);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ventas"></param>
        /// <param name="dto_interval"></param>
        /// <param name="tipoImagen">1=FotoCedula, 2=CedultaTrasera, 3=firma, 4= selfie, 5=pagare PDF</param>
        private void descargarImagenes(DTO_SOLICITUD_VENTAS ventas, List<Tab_ConfigSys> dto_interval, int tipoImagen)
        {
            try
            {
                var dir = dto_interval.Where(x => x.llave_Config5 == "RUTA_REMOVE").Select(x => x.Dato_Char1).FirstOrDefault(); //Server.MapPath("/");
                var _ContainerPrefix = string.Empty;
                var _PatchTempToSave = string.Empty;
                var _ImageExtencion = string.Empty; //Path.GetExtension();

                switch (tipoImagen)
                {
                    case 1:
                        _ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", ventas.Identificacion);
                        _PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), ventas.Identificacion);
                        _ImageExtencion = Path.GetExtension(ventas.UrlFotoCedula);
                        ventas.UrlFotoCedula = string.Concat(_PatchTempToSave, "/", ventas.Identificacion, _ImageExtencion).Replace(dir, "");
                        break;
                    case 2:
                        _ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", ventas.Identificacion);
                        _PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault(), ventas.Identificacion);
                        _ImageExtencion = Path.GetExtension(ventas.UrlFotoCedulaTrasera);
                        ventas.UrlFotoCedulaTrasera = string.Concat(_PatchTempToSave, "/", ventas.Identificacion, _ImageExtencion).Replace(dir, "");
                        break;
                    case 3:
                        _ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", ventas.Identificacion);
                        _PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault(), ventas.Identificacion);
                        _ImageExtencion = Path.GetExtension(ventas.UrlFotoFirma);
                        ventas.UrlFotoFirma = string.Concat(_PatchTempToSave, "/", ventas.Identificacion, _ImageExtencion).Replace(dir, "");
                        break;
                    case 4:
                        _ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_SELFIE").Select(x => x.Dato_Char1).FirstOrDefault(), "/", ventas.Identificacion);
                        _PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOSELFIE").Select(x => x.Dato_Char1).FirstOrDefault(), ventas.Identificacion);
                        _ImageExtencion = Path.GetExtension(ventas.UrlFotoSelfie);
                        ventas.UrlFotoSelfie = string.Concat(_PatchTempToSave, "/", ventas.Identificacion, _ImageExtencion).Replace(dir, "");
                        break;
                    case 5:
                        _ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_PAGARE").Select(x => x.Dato_Char1).FirstOrDefault(), "/", ventas.Identificacion);
                        _PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "CONTRATOPAGARE").Select(x => x.Dato_Char1).FirstOrDefault(), ventas.Identificacion);
                        _ImageExtencion = Path.GetExtension(ventas.UrlDirectorioPagare);
                        ventas.UrlDirectorioPagare = string.Concat(_PatchTempToSave, "/", ventas.Identificacion, _ImageExtencion).Replace(dir, "");
                        break;
                }

                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = _ContainerPrefix,
                    ImageExtencion = _ImageExtencion,
                    ImageToUpload = ventas.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = _PatchTempToSave
                };

                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                var fileDelete = string.Concat(_PatchTempToSave, "/", ventas.Identificacion, _ImageExtencion);
                if (System.IO.File.Exists(fileDelete))
                    System.IO.File.Delete(fileDelete);

                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
            }
            catch (Exception ex) { }
            //return ventas;
        }

        //AVARGAS Inserción de solicitudes por ventas
        public ActionResult InsertarSolictitudes()
        {
            return View();
        }
        public ActionResult InsertarNuevaSolictitudes(DTO_SOLICITUD_VENTAS solicitudes)
        {
            int contadorIntentos = 0;
            InsertPersonasWeb persona = new InsertPersonasWeb();
            try
            {
                if (ModelState.IsValid)
                {
                    persona.Identificacion = solicitudes.Identificacion;
                    persona.TelefonoCel = solicitudes.Telefono;
                    persona.Correo = solicitudes.Mail;
                    persona.SubOrigen = solicitudes.SubOrigen;
                    persona.UsrModifica = Convert.ToString(Session["agente"]);
                    persona.Origen = GlobalClass.Origen_Apps;

                    Models.Manager.clsInsertaSolicitud.InsertaSolicitudesInvoke(ref persona);
                    if (persona.UltidSolicitud == null)
                    {
                        persona.UltidSolicitud = 0;
                    }
                    System.Threading.Thread.Sleep(9000);
                    while ((solicitudes.Status == 12 || solicitudes.Status == null || solicitudes.Status == 0) && (contadorIntentos < 50))
                    {
                        Models.Manager.clsInsertaSolicitud.ApiSolicitudEndConsul(persona, ref solicitudes);
                        contadorIntentos++;
                    }
                    solicitudes.Identificacion = persona.Identificacion;
                    if (persona.SegundoNombre != "")
                    {
                        solicitudes.Nombre = persona.PrimerNombre + " " + persona.SegundoNombre + " " + persona.PrimerApellido + " " + persona.SegundoApellido;
                    }
                    else
                    {
                        solicitudes.Nombre = persona.PrimerNombre + " " + persona.PrimerApellido + " " + persona.SegundoApellido;
                    }

                }
            }
            catch (Exception ex)
            {
                solicitudes.Status = -1;
                solicitudes.Mensaje = ex.Message;
                return Json(solicitudes);
                //persona.Respuesta = "Ocurrio un Error";
            }
            //return View(asignacionbuckets);
            return Json(solicitudes);
        }

        public ActionResult CargaComboProducto(MontoCredito montoCredito)
        {
            try
            {
                PlazoCredito plazo = new PlazoCredito();
                if (ModelState.IsValid)
                {
                    montoCredito.ListMontoCredito = Models.Manager.clsInsertaSolicitud.TraeMontoCalcu(montoCredito);
                    if (montoCredito.Monto > 0)
                    {
                        plazo.IdMontoCredito = montoCredito.ListMontoCredito.Last().IdMontoCredito;
                        montoCredito.ListPlazoCredito = Models.Manager.clsInsertaSolicitud.TraePlazoCalcu(plazo);
                        montoCredito.ListFrecuenciaCredito = Models.Manager.clsInsertaSolicitud.TraeFrecuenciaCalcu();
                        montoCredito.Respuesta = "Datos cargados correctamente";
                    }
                    else
                    {
                        montoCredito.Respuesta = "El monto del credito aprobado es 0";
                    }
                }
            }
            catch (ArgumentException)
            {
                montoCredito.Respuesta = "Ocurrio un Error";
            }
            //return View(asignacionbuckets);
            return Json(montoCredito);
        }
        public ActionResult CargaComboPlazoProducto(PlazoCredito plazoCredito)
        {
            var plazo = new List<PlazoCredito>();
            try
            {

                if (ModelState.IsValid)
                {

                    plazo = Models.Manager.clsInsertaSolicitud.TraePlazoCalcu(plazoCredito);
                }
            }
            catch (ArgumentException)
            {
                var plazoCredierr = new PlazoCredito();
                plazo.Add(plazoCredierr);
                plazo.FirstOrDefault().Respuesta = "Ocurrio un Error";
            }
            //return View(asignacionbuckets);
            return Json(plazo);
        }



        //return View(asignacionbuckets);

        public ActionResult GuardaProductoSolicitud(DTO_SOLICITUD_VENTAS solicitudes)
        {
            var dto_sol = new List<DTO_SOLICITUD_VENTAS>();
            try
            {
                if (ModelState.IsValid)
                {

                    solicitudes.USUARIO_MODIFICACION = Convert.ToString(Session["agente"]);
                    Models.Manager.clsInsertaSolicitud.GuardaProducto(solicitudes);


                }
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            //return View(asignacionbuckets);
            return Json(solicitudes);
        }
        //AVARGAS fin Inserción de solicitudes por ventas
        //jabarca obtener status solicitud
        public ActionResult getEstadoSolicitud(ConsultaEstadoSolicitud idEstado)
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();
            try
            {
                var resul = mang.consultaEstadoSolicitud(idEstado);
                return Json(resul);
            }
            catch { throw; }
        }
        public ActionResult CargaComboFrecuenciaCredito(ProductoSolicitud producto)
        {
            ManagerColocacion mang = new ManagerColocacion();
            var frecu = new List<FrecuenciaCredito>();
            try
            {
                frecu = mang.TraeFrecuenciaCalcu(producto);
            }
            catch (ArgumentException ex)
            {
            }
            return Json(frecu);
        }
        public ActionResult cargaComboSubOrigenes()
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();
            try
            {
                var resul = mang.consultaSubOrigenes();
                return Json(resul);
            }
            catch { throw; }
        }
    }
}
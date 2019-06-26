using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.Colocacion;
using TwoFunTwoMeFintech.Models.Manager;

namespace TwoFunTwoMeFintech.Controllers.Colocacion
{
    public class ColocacionController : Controller
    {
        //
        // GET: /Colocacion/

        public ActionResult Index()
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            return View();
        }

        public ActionResult ListarSolicitudesXAcesor(Dto_SolicitudesXAcesor dto_SolicitudesXAcesor)
        {
            ManagerColocacion manage = new ManagerColocacion();
            try
            {
                if (Session["agente"] == null)
                    return RedirectToAction("LogOff", "Login");

                var result = manage.ConsultaSolicitudesXAsesor(dto_SolicitudesXAcesor);
                if (result == null)
                {
                    Dto_SolicitudesXAcesor objetoNulo = new Dto_SolicitudesXAcesor();
                    objetoNulo.IdSolicitud = 0;
                    result = objetoNulo;
                }
                return Json(result);
            }
            catch
            {
                throw;
            }
        }

        public ActionResult Contactos(Dto_SolicitudesXAcesor dto_SolicitudesXAcesor)
        {
            return View();
        }

        //para buscar el primero de la cola
        public ActionResult BuscarCola(Dto_SolicitudesXAcesor dto_SolicitudesXAcesor)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            return View();
        }

        public ActionResult BuscarSolicitudesLoop(DTO_SOLICITUD_VENTAS dto_Solicitudes)
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();

            try
            {
                if (Session["agente"] == null)
                    return RedirectToAction("LogOff", "Login");
                var resul = mang.ConsultaSolicitudDocumentos(dto_Solicitudes);

                Tab_ConfigSys dto_Config = new Tab_ConfigSys();
                ManagerSolcitudes managerSolicitudes = new ManagerSolcitudes();
                List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();


                dto_Config.llave_Config1 = "SERVICIO";
                dto_Config.llave_Config2 = "CONFIGURACION";
                dto_Config.llave_Config3 = "SERVIDOR";
                dto_Config.llave_Config4 = "URL";
                var dto_interval = managerSolicitudes.ConsultaConfiUrlImagen(dto_Config);

                var dir = dto_interval.Where(x => x.llave_Config5 == "RUTA_REMOVE").Select(x => x.Dato_Char1).FirstOrDefault(); //Server.MapPath("/");
                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_Solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_Solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), dto_Solicitudes.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                //UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                //Thread threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                //Thread.Sleep(1000);
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                resul.FirstOrDefault().UrlFotoCedula = string.Concat(blobDowland.PatchTempToSave, "/", blobDowland.ImageToUpload, blobDowland.ImageExtencion).Replace(dir, "/");

                blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_SELFIE").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_Solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_Solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOSELFIE").Select(x => x.Dato_Char1).FirstOrDefault(), dto_Solicitudes.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                //threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                //Thread.Sleep(1000);
                resul.FirstOrDefault().UrlFotoSelfie = string.Concat(blobDowland.PatchTempToSave, "/", blobDowland.ImageToUpload, blobDowland.ImageExtencion).Replace(dir, "/");

                blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_Solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_Solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault(), dto_Solicitudes.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                //threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                //Thread.Sleep(1000);
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                resul.FirstOrDefault().UrlFotoCedulaTrasera = string.Concat(blobDowland.PatchTempToSave, "/", blobDowland.ImageToUpload, blobDowland.ImageExtencion).Replace(dir, "/");


                blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_Solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_Solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault(), dto_Solicitudes.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                //threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                //Thread.Sleep(1000);
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                resul.FirstOrDefault().UrlFotoFirma = string.Concat(blobDowland.PatchTempToSave, "/", blobDowland.ImageToUpload, blobDowland.ImageExtencion).Replace(dir, "/");

                blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_PAGARE").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_Solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".pdf",
                    ImageToUpload = dto_Solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "CONTRATOPAGARE").Select(x => x.Dato_Char1).FirstOrDefault(), dto_Solicitudes.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                //threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                resul.FirstOrDefault().UrlDirectorioPagare = string.Concat(blobDowland.PatchTempToSave, "/", blobDowland.ImageToUpload, blobDowland.ImageExtencion).Replace(dir, "/");

                //Thread.Sleep(1000);
                //DirectoryInfo directory = new DirectoryInfo(blobDowland.PatchTempToSave);

                return Json(resul);
            }
            catch { throw; }
        }

        public ActionResult CargaCatalogos(dto_catalogos catalogos)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var listCatalog = new System.Collections.Generic.List<object>();

            ManagerColocacion manage = new ManagerColocacion();
            try
            {
                var resul = manage.MostrarCatalogos(catalogos);

                var estadoCivil = resul.Where(x => x.IdTipo.Equals(3)).Select(x => new
                {
                    IdTipo = x.Id,
                    Desc = x.Descripcion,
                    Tipo = 3
                });
                listCatalog.Add(estadoCivil);
                var genero = resul.Where(x => x.IdTipo.Equals(4)).Select(x => new
                {
                    IdTipo = x.Id,
                    Desc = x.Descripcion,
                    Tipo = 4
                });



                listCatalog.Add(genero);

                var asegurado = resul.Where(x => x.IdTipo.Equals(9)).Select(x => new
                {
                    IdTipo = x.Id,
                    Desc = x.Descripcion,
                    Tipo = 9
                });


                listCatalog.Add(asegurado);

                var familia = resul.Where(x => x.IdTipo.Equals(12)).Select(x => new
                {
                    IdTipo = x.Id,
                    Desc = x.Descripcion,
                    Tipo = 12
                });


                listCatalog.Add(familia);

                var soporte = resul.Where(x => x.IdTipo.Equals(13)).Select(x => new
                {
                    IdTipo = x.Id,
                    Desc = x.Descripcion,
                    Tipo = 13
                });


                listCatalog.Add(soporte);

                var Nacionalidad = resul.Where(x => x.IdTipo.Equals(2)).Select(x => new
                {
                    IdTipo = x.Id,
                    Desc = x.Descripcion,
                    Tipo = 2
                });


                listCatalog.Add(Nacionalidad);

                return Json(listCatalog);
            }
            catch { throw; }
        }
        public ActionResult CargaStatusSolicitud()
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            try
            {
                var solStatus = new dto_solicitud_status();
                var resul = mang.CargaEstatusSolicitud(solStatus);
                return Json(resul);
            }
            catch { throw; }
        }

        public ActionResult CargaProductos(Solicitudes solicitudes)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            try
            {
                var resul = mang.getDetalleProducto(solicitudes);
                return Json(resul);
            }
            catch { throw; }
        }
        public ActionResult CargaMontoProducto(Solicitudes solicitudes)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            try
            {
                var resul = mang.CargarProductos(solicitudes);
                return Json(resul);
            }
            catch { throw; }
        }
        public ActionResult CargaComboPlazoProducto(PlazoCredito plazoCredito)
        {
            ManagerColocacion mang = new ManagerColocacion();
            var plazo = new List<PlazoCredito>();
            try
            {
                if (ModelState.IsValid)
                {
                    plazo = mang.TraePlazoCalcu(plazoCredito);
                }
            }
            catch (ArgumentException)
            {
                var plazoCredierr = new PlazoCredito();
                plazo.Add(plazoCredierr);
                plazo.FirstOrDefault().Respuesta = "Ocurrio un Error";
            }
            return Json(plazo);
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
        public ActionResult getProductoSolicitud(ProductoSolicitud producto)
        {
            ManagerColocacion mang = new ManagerColocacion();
            var productoSolicitud = new List<ProductoSolicitud>();
            try
            {
                if (ModelState.IsValid)
                {
                    productoSolicitud = mang.TraeProductoSolicitud(producto);
                }
            }
            catch (ArgumentException ex)
            {
            }
            return Json(productoSolicitud);
        }
        public ActionResult CargaProvincia()
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            try
            {
                var dto_reg = new dto_region();
                var resul = mang.Provincia(dto_reg);
                return Json(resul);
            }
            catch { throw; }
        }
        public ActionResult CargaStatusAsesor()
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            Tab_StatusDispAgente statusDispAgente = new Tab_StatusDispAgente();
            var dto_retorno = (List<TwoFunTwoMeFintech.Models.dto_login>)Session["LoginCredentials"];
            try
            {
                statusDispAgente.cod_agente = dto_retorno.FirstOrDefault().cod_agente;
                var ultstatusasesor = mang.TraeUltimoStatusAsesor(statusDispAgente);
                if (ultstatusasesor.Any())
                {
                    statusDispAgente.IdCatDisponibilidad = ultstatusasesor.FirstOrDefault().IdCatDisponibilidad;
                    var dto_reg = new Tab_CatDisponibilidad();
                    statusDispAgente.ListCatDisponibilidad = mang.TraeStatusAsesor(dto_reg);
                }
                return Json(statusDispAgente);
            }
            catch { throw; }
        }
        public ActionResult CargaCanton(dto_region dto_reg)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            try
            {
                var resul = mang.canton(dto_reg);
                return Json(resul);
            }
            catch { throw; }
        }
        public ActionResult CargaDistrito(dto_region dto_reg)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            try
            {
                var resul = mang.Distrito(dto_reg);
                return Json(resul);
            }
            catch { throw; }
        }
        #region Icortes
        public ActionResult CargaColaVentas(dto_cola_ventas cola_Ventas)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            ManagerUser managerUser = new ManagerUser();
            dto_login dto_login = new dto_login();
            var dto_Config = new Tab_ConfigSys
            {
                llave_Config1 = "CONFIGURACION",
                llave_Config2 = "FENIX",
                llave_Config3 = "VENTAS",
                llave_Config4 = "INICIO_COLA",
                llave_Config5 = "INI_COL"
            };
            try
            {
                var dto_ret_config = managerUser.GetConfigSys(dto_Config);
                if (dto_ret_config.Any())
                {
                    dto_login.IdCatDisponibilidad = dto_ret_config.FirstOrDefault().Dato_Int1;
                    var dto_retorno = (List<dto_login>)Session["LoginCredentials"];
                    dto_login.cod_agente = dto_retorno.FirstOrDefault().cod_agente;
                    var result = managerUser.usp_EjeStatusDisp(dto_login);
                }

                cola_Ventas.cod_agente = Session["agente"].ToString();
                var resul = mang.GetColasVentas(cola_Ventas);
                return Json(resul);
            }
            catch { throw; }
        }

        public ActionResult GuardarDomiciliacion(dto_file dto_File)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            ManagerSolcitudes managerSolicitudes = new ManagerSolcitudes();
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdSolicitud = dto_File.IdSolicitud;
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            try
            {
                var dto_interval = managerSolicitudes.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "DOMICILIACION").Select(x => x.Dato_Char1).FirstOrDefault();
                string ext = Path.GetExtension(dto_File.Name);
                solicitudes.UrlDirectorioPagare = string.Concat(URL, dto_File.Identificacion, "/", dto_File.Identificacion);
                if (Utilitarios.ValidarFichero(solicitudes.UrlDirectorioPagare))
                {
                    byte[] bytes = Convert.FromBase64String(dto_File.Base64);
                    solicitudes.UrlDirectorioPagare = string.Concat(solicitudes.UrlDirectorioPagare, ext);
                    System.IO.File.WriteAllBytes(solicitudes.UrlDirectorioPagare, bytes);
                    mang.GurdarRutas(solicitudes);
                }
                //var resul = mang.GetColasVentas(cola_Ventas);
                return Json("OK");
            }
            catch { throw; }
        }

        public ActionResult GuardarFrontal(dto_file dto_File)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            ManagerSolcitudes managerSolicitudes = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            AWSAccess wSAccess = new AWSAccess();
            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdSolicitud = dto_File.IdSolicitud;
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            int IdTipoIdentificacion = 0;
            int rotate = 0;
            int i = 0;
            byte[] bytes = null;
            try
            {
                //INICIO FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD
                solicitudes.UsrModifica = Session["agente"].ToString();
                //FIN FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD
                solicitudes.TipoFoto = 1;
                if (dto_File.Identificacion.Length <= 9)
                {
                    IdTipoIdentificacion = 1;
                }
                else
                {
                    IdTipoIdentificacion = 2;
                }

                var dto_interval = managerSolicitudes.ConsultaConfiUrlImagen(dto_Config);
                //string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault();
                string ext = ".jpg";//Path.GetExtension(dto_File.Name);
                CONF = mang.ConsultarAccessKeyAWS();
                //solicitudes.UrlFotoCedula = string.Concat(URL, dto_File.Identificacion, "/", dto_File.Identificacion);
                //if (Utilitarios.ValidarFichero(solicitudes.UrlFotoCedula))
                //{
                bytes = Convert.FromBase64String(dto_File.Base64);
                //solicitudes.UrlFotoCedula = string.Concat(solicitudes.UrlFotoCedula, ext);
                //System.IO.File.WriteAllBytes(solicitudes.UrlFotoCedula, bytes);
                //mang.GurdarRutas(solicitudes);

                var soliresp = wSAccess.DetectText(CONF, bytes, dto_File.Identificacion, IdTipoIdentificacion, solicitudes.TipoFoto);

                if (!soliresp.Result)
                {
                    while (i != 4)
                    {
                        i++;
                        using (Image image = Image.FromStream(new MemoryStream(bytes)))
                        {
                            rotate = 8;
                            //int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                            RotateFlipType rotateFlipType = wSAccess.GetOrientationToFlipType(rotate);
                            image.RotateFlip(rotateFlipType);

                            bytes = (byte[])(new ImageConverter()).ConvertTo(image, typeof(byte[]));
                        }
                        soliresp = wSAccess.DetectText(CONF, bytes, dto_File.Identificacion, IdTipoIdentificacion, solicitudes.TipoFoto);
                        if (soliresp.Result)
                            break;
                    }
                }
                var blob = new blobStorage
                {
                    ImageToUploadByte = bytes,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_File.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_File.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                //threadObj.Start();
                UtilBlobStorageAzure.UploadBlobStorage(blob);
                solicitudes.UrlFotoCedula = string.Concat(blob.ContainerPrefix, "/", dto_File.Identificacion, blob.ImageExtencion);

                solicitudes.Result = soliresp.Result;
                var guardaruta = mang.GurdarRutas(solicitudes);
                solicitudes.Mensaje = guardaruta.FirstOrDefault().Mensaje;
                //}
                //var resul = mang.GetColasVentas(cola_Ventas);
                return Json(solicitudes);
            }
            catch { throw; }
        }

        public ActionResult GuardarTrasera(dto_file dto_File)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            ManagerSolcitudes managerSolicitudes = new ManagerSolcitudes();
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            AWSAccess wSAccess = new AWSAccess();
            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdSolicitud = dto_File.IdSolicitud;
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            int IdTipoIdentificacion = 0;
            int rotate = 0;
            int i = 0;
            try
            {
                solicitudes.TipoFoto = 1;
                if (dto_File.Identificacion.Length <= 9)
                {
                    IdTipoIdentificacion = 1;
                }
                else
                {
                    IdTipoIdentificacion = 2;
                }

                CONF = mang.ConsultarAccessKeyAWS();
                var dto_interval = managerSolicitudes.ConsultaConfiUrlImagen(dto_Config);
                //string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault();
                string ext = ".jpg";//Path.GetExtension(dto_File.Name);

                //solicitudes.UrlFotoCedulaTrasera = string.Concat(URL, dto_File.Identificacion, "/", dto_File.Identificacion);
                //if (Utilitarios.ValidarFichero(solicitudes.UrlFotoCedulaTrasera))
                //{
                byte[] bytes = Convert.FromBase64String(dto_File.Base64);
                //solicitudes.UrlFotoCedulaTrasera = string.Concat(solicitudes.UrlFotoCedulaTrasera, ext);
                //System.IO.File.WriteAllBytes(solicitudes.UrlFotoCedulaTrasera, bytes);
                //mang.GurdarRutas(solicitudes);
                var soliresp = wSAccess.DetectText(CONF, bytes, dto_File.Identificacion, IdTipoIdentificacion, solicitudes.TipoFoto);

                if (!soliresp.Result)
                {
                    while (i != 4)
                    {
                        i++;

                        using (Image image = Image.FromStream(new MemoryStream(bytes)))
                        {
                            rotate = 8;
                            //int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                            RotateFlipType rotateFlipType = wSAccess.GetOrientationToFlipType(rotate);
                            image.RotateFlip(rotateFlipType);
                            bytes = (byte[])(new ImageConverter()).ConvertTo(image, typeof(byte[]));
                        }
                        solicitudes.TipoFoto = 2;
                        if (dto_File.Identificacion.Length <= 9)
                        {
                            IdTipoIdentificacion = 1;
                        }
                        else
                        {
                            IdTipoIdentificacion = 2;
                        }
                        soliresp = wSAccess.DetectText(CONF, bytes, dto_File.Identificacion, IdTipoIdentificacion, solicitudes.TipoFoto);
                        if (soliresp.Result == true)
                            break;
                    }
                }
                if (IdTipoIdentificacion == 1)
                {
                    solicitudes.Result = soliresp.Result;
                }
                else
                {
                    solicitudes.Result = true;
                }
                var blob = new blobStorage
                {
                    ImageToUploadByte = bytes,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_File.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_File.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                //threadObj.Start();
                UtilBlobStorageAzure.UploadBlobStorage(blob);
                solicitudes.UrlFotoCedulaTrasera = string.Concat(blob.ContainerPrefix, "/", dto_File.Identificacion, blob.ImageExtencion);

                if (soliresp.Dia == 1 && soliresp.Dia == 1 && soliresp.Dia == 1 && IdTipoIdentificacion == 1)
                {
                    solicitudes.VigenciaCed = true;
                }
                if (soliresp.Dia == 1 && soliresp.Dia == 1 && soliresp.Dia == 1 && IdTipoIdentificacion == 2)
                {
                    solicitudes.VigenciaCed = true;
                }
                if (soliresp.Dia == 0 && soliresp.Dia == 0 && soliresp.Dia == 0 && IdTipoIdentificacion == 1)
                {
                    solicitudes.VigenciaCed = false;
                }
                if (soliresp.Dia == 0 && soliresp.Dia == 0 && soliresp.Dia == 0 && IdTipoIdentificacion == 2)
                {
                    solicitudes.VigenciaCed = true;
                }
                solicitudes.Result = soliresp.Result;
                var guardaruta = mang.GurdarRutas(solicitudes);
                solicitudes.Mensaje = guardaruta.FirstOrDefault().Mensaje;
                //}
                //var resul = mang.GetColasVentas(cola_Ventas);
                return Json(solicitudes);
            }
            catch { throw; }
        }

        public ActionResult GuardarSelfie(dto_file dto_File)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            ManagerSolcitudes managerSolicitudes = new ManagerSolcitudes();
            AWSAccess wSAccess = new AWSAccess();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdSolicitud = dto_File.IdSolicitud;
            solicitudes.Identificacion = dto_File.Identificacion;
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            int IdTipoIdentificacion = 0;

            try
            {
                solicitudes.TipoFoto = 3;
                if (dto_File.Identificacion.Length <= 9)
                {
                    IdTipoIdentificacion = 1;
                }
                else
                {
                    IdTipoIdentificacion = 2;
                }
                CONF = mang.ConsultarAccessKeyAWS();
                var dto_interval = managerSolicitudes.ConsultaConfiUrlImagen(dto_Config);
                //string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOSELFIE").Select(x => x.Dato_Char1).FirstOrDefault();
                string ext = ".jpg";//Path.GetExtension(dto_File.Name);

                //solicitudes.UrlFotoSelfie = string.Concat(URL, dto_File.Identificacion, "/", dto_File.Identificacion);

                //if (Utilitarios.ValidarFichero(solicitudes.UrlFotoSelfie))
                //{
                //solicitudes.UrlFotoSelfie = string.Concat(solicitudes.UrlFotoSelfie, ext);
                byte[] bytes = Convert.FromBase64String(dto_File.Base64);

                //*carga la imagen de la cedula frontal para comparar
                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_File.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_File.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), dto_File.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };
                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                //Thread threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                //Thread.Sleep(2000);
                //DirectoryInfo directory = new DirectoryInfo(blobDowland.PatchTempToSave);

                //FileInfo[] files = directory.GetFiles("*.*");
                solicitudes.arrImageCedulaFrontal = System.IO.File.ReadAllBytes(string.Concat(blobDowland.PatchTempToSave, "/", dto_File.Identificacion, ext));
                //directory.EnumerateFiles().ToList().ForEach(

                //    x =>
                //    {
                //        //solicitudes.arrImageCedulaFrontal = System.IO.File.ReadAllBytes(x.FullName);
                //        x.Delete();
                //    }
                //    );
                //Directory.Delete(blobDowland.PatchTempToSave);
                //fin carga imagen de la cedula temporal

                //}

                //var ImgsolCedula = mang.ConsultarUrlImagenes(solicitudes);
                //solicitudes.UrlFotoCedula = ImgsolCedula.FirstOrDefault().UrlFotoCedula;
                var solires = wSAccess.DetectText(CONF, bytes, dto_File.Identificacion, IdTipoIdentificacion, 1);
                solicitudes.Result = solires.Result;

                solicitudes.arrImageSelfie = bytes;

                //var dto_Recog = wSAccess.GetTestAsync(CONF, solicitudes);
                //solicitudes.PorcentMatched = dto_Recog.PorcentMatched;

                var blob = new blobStorage
                {
                    ImageToUploadByte = bytes,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_SELFIE").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_File.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_File.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOSELFIE").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                //threadObj.Start();
                UtilBlobStorageAzure.UploadBlobStorage(blob);
                solicitudes.UrlFotoSelfie = string.Concat(blob.ContainerPrefix, "/", dto_File.Identificacion, blob.ImageExtencion);
                //solicitudes.UrlFotoCedula = string.Concat(blob.ContainerPrefix, "/", dto_File.Identificacion, blob.ImageExtencion);
                var result = mang.GurdarRutas(solicitudes);
                //var resul = mang.GetColasVentas(cola_Ventas);
                RestClient cliente = new RestClient(dto_interval.Where(x => x.llave_Config5 == "URLAPILOOPFACE").Select(x => x.Dato_Char1).FirstOrDefault()); // Dirección web del reporte
                RestRequest request = new RestRequest(); // Clase propia del RestSharp para asignar parámetros de envio.
                request.Method = Method.POST;

                //request.AddHeader("Content-Type", "application/json");
                //request.AddHeader("Accept", "application/xml");

                request.AddParameter("IdTipoIdentificacion", IdTipoIdentificacion);
                request.AddParameter("Identificacion", solicitudes.Identificacion);
                request.AddParameter("IdSolicitud", solicitudes.IdSolicitud);

                var respuesta = cliente.Execute(request); // Metodo que ejecuta la solicitud.
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK) // Si retorna OK, el reporte fue generado.
                {

                    var REQ = new Solicitudes();
                    REQ = JsonConvert.DeserializeObject<Solicitudes>(respuesta.Content);
                    solicitudes.PorcentMatched = REQ.PorcentMatched;
                }
                else
                {
                    solicitudes.PorcentMatched = 0;
                }
                solicitudes.Mensaje = result.FirstOrDefault().Mensaje;
                return Json(new { solicitudes.PorcentMatched, solicitudes.Mensaje, solicitudes.Result });
            }
            catch (Exception ex) { throw; }
        }



        public ActionResult GuardarFirma(dto_file dto_File)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            ManagerSolcitudes managerSolicitudes = new ManagerSolcitudes();
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdSolicitud = dto_File.IdSolicitud;
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            try
            {
                var dto_interval = managerSolicitudes.ConsultaConfiUrlImagen(dto_Config);
                //string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault();
                string ext = ".jpg";//Path.GetExtension(dto_File.Name);

                //solicitudes.UrlFotoFirma = string.Concat(URL, dto_File.Identificacion, "/", dto_File.Identificacion);
                // if (Utilitarios.ValidarFichero(solicitudes.UrlFotoFirma))
                //{
                byte[] bytes = Convert.FromBase64String(dto_File.Base64);

                //solicitudes.UrlFotoFirma = string.Concat(solicitudes.UrlFotoFirma, ext);
                //System.IO.File.WriteAllBytes(solicitudes.UrlFotoFirma, bytes);
                var blob = new blobStorage
                {
                    ImageToUploadByte = bytes,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", dto_File.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = dto_File.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                //threadObj.Start();
                UtilBlobStorageAzure.UploadBlobStorage(blob);
                solicitudes.UrlFotoFirma = string.Concat(blob.ContainerPrefix, "/", dto_File.Identificacion, blob.ImageExtencion);

                var guardaruta = mang.GurdarRutas(solicitudes);
                //}
                //var resul = mang.GetColasVentas(cola_Ventas);
                return Json("OK");
            }
            catch { throw; }
        }

        public ActionResult ActualizarPersonas(Personas personas)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            ManagerColocacion mang = new ManagerColocacion();
            try
            {
                personas.UsoCredito = Session["agente"].ToString();
                personas.UsrModifica = Session["agente"].ToString();
                mang.ActualizaPersona(personas);
                return Json("OK");
            }
            catch { throw; }
        }

        //Ventanad de pasos y info de solicitudes
        public ActionResult getPAsosAgente(PasosXAgente pasos)
        {
            if (pasos.IdSolicitud != 0)
            {
                ManagerColocacion mang = new ManagerColocacion();
                pasos.CodAgente = Session["agente"].ToString();
                var listaPasosAgente = mang.ConsultaPasosAgente(pasos);
                return Json(listaPasosAgente);
            }
            else
            {
                return Json(new List<PasosXAgente>());
            }
        }
        public void cambioBitCompleto(PasosXAgente pasos)
        {
            ManagerColocacion mang = new ManagerColocacion();
            pasos.CodAgente = Session["agente"].ToString();
            mang.CambioBitCompleto(pasos);
        }
        public ActionResult getDashboardAgente(DashboardVentas dash)
        {
            ManagerColocacion mang = new ManagerColocacion();
            var listaDashAgente = mang.ConsultaDashboardAgente(dash);
            return Json(listaDashAgente);
        }
        public ActionResult GuardaReprogramacion(ReprogramacionVentas reprog)
        {
            ManagerColocacion manager = new ManagerColocacion();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            reprog.CodAgente = Session["agente"].ToString();
            reprog.CodAgenteOriginal = Session["agente"].ToString();
            var ret = manager.CrearReprogramacion(reprog);

            return Json(ret);
        }
        public ActionResult PanelInfo()
        {
            return View();
        }

        public ActionResult GuardarGestion(GestionGeneral gestionGeneral)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            ManagerUser managerUser = new ManagerUser();
            gestionGeneral.ACCION = "INSERTAR";
            gestionGeneral.Cod_Agente = Session["agente"].ToString();
            gestionGeneral.FechaGestion = DateTime.Now.ToString();
            try
            {
                var ret = managerUser.MantenimientioGestionesGeneral(gestionGeneral);
                return Json(ret);
            }
            catch { throw; }
        }


        public ActionResult MonstrarGestion(GestionGeneral gestionGeneral)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            ManagerUser managerUser = new ManagerUser();
            gestionGeneral.ACCION = "CONSULTAR";
            gestionGeneral.Cod_Agente = Session["agente"].ToString();
            try
            {
                var ret = managerUser.MantenimientioGestionesGeneral(gestionGeneral);
                return Json(ret);
            }
            catch { throw; }
        }
        #endregion 
        //INICIO FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA
        public ActionResult CargaContolesXAccionCola(dto_Controles dto_reg)
        {
            ManagerColocacion mang = new ManagerColocacion();
            try
            {
                var resul = mang.CargaContolesXAccionCola(dto_reg);
                return Json(resul);
            }
            catch { throw; }
        }
        //FIN FCAMACHO 14/02/2019 PROCESO DE CONTROLES DINAMICOS SEGUN LA ACCION DE LA COLA


        //INICIO AVARGAS 19/02/2019 PROCESO DE CAMBIO DE STATUS DEL ASESOR
        public ActionResult CambioSatusAsesor(dto_login dto_login)
        {
            ManagerUser mang = new ManagerUser();

            try
            {
                var dto_retorno = (List<dto_login>)Session["LoginCredentials"];
                dto_login.cod_agente = dto_retorno.FirstOrDefault().cod_agente;
                var resul = mang.usp_EjeStatusDisp(dto_login);
                return Json(resul);
            }
            catch { throw; }
        }
        //FIN  AVARGAS 19/02/2019 PROCESO DE CAMBIO DE STATUS DEL ASESOR

        #region PROCESAR SOLICITUD
        //INICIO FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD
        public ActionResult ProcesarColaPersona(dto_ProcesaSolicitud Solicitud)
        {
            ManagerColocacion manager = new ManagerColocacion();
            ManagerUser manageUser = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            Solicitud.UsrModifica = Session["agente"].ToString();
            var ret = manager.ProcesarColaPersona(Solicitud);

            Correo correo = new Correo();
            correo.Mail = Solicitud.Correo;
            var correoValidado = manageUser.validarEmail(correo);
            PasosXAgente pasos = new PasosXAgente();
            pasos.IdSolicitud = Solicitud.IdSolicitud;
            pasos.CodAgente = Solicitud.UsrModifica;
            pasos.Paso = "101";
            if (correoValidado.valid == "true")
            {
                pasos.BitCompleto = true;
                cambioBitCompleto(pasos);
            }
            if (correoValidado.valid == "false")
            {
                pasos.BitCompleto = false;
                cambioBitCompleto(pasos);
            }
            return Json(ret);
        }

        public ActionResult ProcesarColaRefrenciaPersona(dto_ProcesaSolicitud Solicitud)
        {
            ManagerColocacion manager = new ManagerColocacion();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            Solicitud.UsrModifica = Session["agente"].ToString();
            var ret = manager.ProcesarColaReferenciaPersona(Solicitud);

            return Json(ret);
        }

        public ActionResult ProcesarColaProducto(dto_ProcesaSolicitud Solicitud)
        {
            ManagerColocacion manager = new ManagerColocacion();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            Solicitud.UsrModifica = Session["agente"].ToString();
            var ret = manager.ProcesarColaProducto(Solicitud);

            return Json(ret);
        }

        public ActionResult ProcesarColaSolicitud(dto_ProcesaSolicitud Solicitud)
        {
            ManagerColocacion manager = new ManagerColocacion();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            Solicitud.UsrModifica = Session["agente"].ToString();
            var ret = manager.ProcesarColaSolicitud(Solicitud);

            return Json(ret);
        }
        //FIN FCAMACHO 20/02/2019 OBJETO UTILIZADO PARA EL ALMACENAMIENTO AL PROCESAR LA SOLICITUD
        #endregion
        //inicio mostrar guion
        public ActionResult getGuion(Dto_Guion guion)
        {
            ManagerColocacion mang = new ManagerColocacion();
            var listaGuion = mang.consultaGuionSolicitud(guion);
            return Json(listaGuion);
        }
        //fin mostrar guion

        //icortes inicio generacion de link loop
        public ActionResult GenerarLinkLoop(dto_linkLoop linkLoop)
        {
            ManagerColocacion manager = new ManagerColocacion();
            ManagerUser managerUser = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manager.GenerarLinkLoop(linkLoop);
            ret.First().UrlWhatsApp = string.Format(ret.First().UrlWhatsApp, linkLoop.Telefono);
            ret.First().Uri = string.Format(ret.First().Uri, Utility.Encrypt(ret.First().IdSolicitud.ToString()), Utility.Encrypt(ret.First().Identificacion), Utility.Encrypt(ret.First().Tipo));
            //Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            //dto_Config.llave_Config1 = "CONFIGURACION";
            //dto_Config.llave_Config2 = "APIS";
            //dto_Config.llave_Config3 = "LOOP";
            //dto_Config.llave_Config4 = "LINK_SHORT";

            //var result = managerUser.GetConfigSys(dto_Config);

            //dto_Config.Dato_Char1 = result.FirstOrDefault().Dato_Char1;
            //dto_Config.Dato_Char2 = string.Format(result.FirstOrDefault().Dato_Char2, ret.First().Uri);
            //dto_Config.Dato_Char3 = ret.First().Uri;

            //ret.First().Uri = Utility.ShortUrl(dto_Config);

            return Json(ret.First());
        }

        //icortes fin generacion de link loop
        public ActionResult getHistorialSolicitudes(HistoricoSolicitudes historialSol)
        {
            ManagerColocacion mang = new ManagerColocacion();
            var listaHistorial = mang.getHistorialSolicitudes(historialSol);
            return Json(listaHistorial);
        }


        public ActionResult CambioProducto()
        {
            return View();
        }
        public ActionResult ConsultaCambioProducto(string Identificacion)
        {
            ManagerColocacion mang = new ManagerColocacion();
            var listaHistorial = mang.consultaCambioProductosCredito(Identificacion);
            return Json(listaHistorial);
        }
        //apu
        public ActionResult getIdPlazoCredito(PlazoCredito plazoCredito)
        {
            ManagerColocacion mang = new ManagerColocacion();
            var idPLazo = new List<PlazoCredito>();
            try
            {
                if (ModelState.IsValid)
                {
                    idPLazo = mang.getIdPlazoCredito(plazoCredito);
                }
            }
            catch (ArgumentException)
            {
            }
            return Json(idPLazo);
        }
        public ActionResult GuardaProductoSolicitud(ProductoNuevo producto)
        {
            ManagerColocacion mang = new ManagerColocacion();
            producto.P_FUNCION = 2; //le seteamos el valor 2 para el usp
            var resultado = mang.setNuevoProducto(producto);
            if (resultado)
            {
                return Json("El producto se modifico con exito");
            }
            else
            {
                return Json("Error al guardar el producto");
            }
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }
        public ActionResult getSolicitudActual(int idSolicitud)
        {
            ManagerColocacion manage = new ManagerColocacion();
            try
            {
                if (Session["agente"] == null)
                    return RedirectToAction("LogOff", "Login");
                Dto_SolicitudesXAcesor dto_SolicitudesXAcesor = new Dto_SolicitudesXAcesor();
                dto_SolicitudesXAcesor.IdSolicitud = idSolicitud;
                var result = manage.ConsultaSolicitudesXAsesor(dto_SolicitudesXAcesor);
                if (result == null)
                {
                    Dto_SolicitudesXAcesor objetoNulo = new Dto_SolicitudesXAcesor();
                    objetoNulo.IdSolicitud = 0;
                    result = objetoNulo;
                }
                SolicitudActual solicitudActual = new SolicitudActual();
                solicitudActual.IdSolicitud = result.IdSolicitud;
                solicitudActual.Status = result.Status;
                solicitudActual.Identificacion = result.Identificacion;
                solicitudActual.IdProducto = result.IdProducto;
                solicitudActual.DetalleStatus = result.DetalleStatus;
                solicitudActual.MontoMaximo = result.MontoMaximo;
                solicitudActual.MontoProducto = result.MontoProducto;
                return Json(solicitudActual);
            }
            catch
            {
                throw;
            }
        }
        #region Mantenimiento de Contactos

        public ActionResult CargaContactos(contacto contac)

        {
            if (contac.Identificacion == null) return View();

            ManagerUser manage = new ManagerUser();


            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarContactos(contac);

            return Json(ret);
        }
        public ActionResult ConsultaCatalogoTel(string id)
        {
            ManagerUser manage = new ManagerUser();
            CatalogoTelefono catalogoTelefonos = new CatalogoTelefono();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarCatalogoTelefono(catalogoTelefonos);

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtieneTelefono(string id)
        {
            contacto contac = new contacto();
            contac.IdTelefono = Convert.ToInt32(id);
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarTelefono(contac);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActualizarTelefono(contacto contac)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ActualizarTelefono(contac);

            return Json(ret);
        }
        public ActionResult IngresarTelefono(contacto contac)

        {
            if (contac.Identificacion == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.CrearTelefono(contac);

            return Json(ret);
        }
        public ActionResult EliminaTelefono(int id)

        {
            contacto contac = new contacto();
            contac.IdTelefono = id;
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.EliminaTelefono(contac);

            return Json(ret);
        }
        #endregion

        #region MantenimientoDirecciones
        public ActionResult ContactosDireccion(contacto contac)
        {
            if (contac.Identificacion == null) return View();
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ConsultarDirecciones(contac);
            return Json(ret);
        }
        public ActionResult ConsultaCatalogoDIR(string id)
        {
            ManagerUser manage = new ManagerUser();
            CatalogoDireccion catalogoDireccion = new CatalogoDireccion();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ConsultarCatalogoDireccion(catalogoDireccion);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtieneDireccion(string id)
        {
            contacto contac = new contacto();
            contac.IdDireccion = Convert.ToInt32(id);
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ConsultarDireccion(contac);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActualizarDireccion(contacto contac)
        {
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ActualizarDireccion(contac);
            return Json(ret);
        }
        public ActionResult CrearDireccion(contacto contac)
        {
            if (contac.Identificacion == null) return View();
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.CrearDireccion(contac);
            return Json(ret);
        }
        #endregion MantenimientoDirecciones


        public ActionResult CalculaPlanPago(PlanPlagos pago)
        {
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("Login", "Login");
            var ret = manage.ConsultaPlanPagosVentas(pago);
            return Json(ret);
        }
        public ActionResult GuardarTiempoSolicitud(TiempoSolicitud objTiempo)
        {
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("Login", "Login");
            objTiempo.UsrModifica = Session["agente"].ToString();
            var ret = manage.guardarTiempoSolicitud(objTiempo);
            return Json(ret);
        }
        public ActionResult BuscarClienteNombre(Dto_SolicitudesXAcesor solcitud)
        {
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var dto_ret = manage.BusquedaNombre(solcitud);
            return Json(dto_ret);
        }
        public ActionResult ValidarEmail(Correo correo)
        {
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var correoValidado = manage.validarEmail(correo);
            return Json(correoValidado);
        }
        public ActionResult GetImagenSinpe(ImagenSinpe imagen)
        {
            ManagerSolcitudes mang = new ManagerSolcitudes();
            try
            {
                if (Session["agente"] == null)
                    return RedirectToAction("LogOff", "Login");
                Tab_ConfigSys dto_Config = new Tab_ConfigSys();
                ManagerSolcitudes managerSolicitudes = new ManagerSolcitudes();
                List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
                dto_Config.llave_Config1 = "SERVICIO";
                dto_Config.llave_Config2 = "CONFIGURACION";
                dto_Config.llave_Config3 = "SERVIDOR";
                dto_Config.llave_Config4 = "URL";
                var dto_interval = managerSolicitudes.ConsultaConfiUrlImagen(dto_Config);
                var dir = dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_IMAGENSINPE").Select(x => x.Dato_Char1).FirstOrDefault(); //Server.MapPath("/");
                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_IMAGENSINPE").Select(x => x.Dato_Char1).FirstOrDefault(), "/", imagen.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = imagen.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),
                    PatchTempToSave = AppDomain.CurrentDomain.BaseDirectory + string.Concat(dto_interval.Where(x => x.llave_Config5 == "IMAGENSINPE").Select(x => x.Dato_Char1).FirstOrDefault(), imagen.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };
                string LimpiarCarpeta = blobDowland.PatchTempToSave;
                if (Directory.Exists(LimpiarCarpeta))
                {
                    List<string> strDirectories = Directory.GetDirectories(LimpiarCarpeta, "*", SearchOption.AllDirectories).ToList();
                    foreach (string directorio in strDirectories)
                    {
                        Directory.Delete(directorio, true);
                    }
                }
                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                if (UtilBlobStorageAzure.ExistsFileInBlob(blobDowland))
                {
                    UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                    imagen.UrlImagenSinpe = string.Concat(dto_interval.Where(x => x.llave_Config5 == "IMAGENSINPE").Select(x => x.Dato_Char1).FirstOrDefault()) + @"\" + imagen.Identificacion + @"\" + imagen.Identificacion + blobDowland.ImageExtencion;
                    imagen.Mensaje = "Completo";
                }
                else
                {
                    imagen.Mensaje = "Este cliente no tiene imagen sinpe.";
                }
                return Json(imagen);
            }
            catch (Exception ex)
            {
                imagen.Mensaje = "Error al mostrar la imagen";
                return Json(imagen);
            }
        }
        public ActionResult GuardarTelefonoLaboral(TelefonoLaboral telefonoLaboral)
        {
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            telefonoLaboral.UsuarioIngreso = Session["agente"].ToString();
            var telefonoGuardado = manage.GuardarTelefonoLaboral(telefonoLaboral);
            return Json(telefonoGuardado);
        }
        public ActionResult ConsulatTelefonoLaboral(TelefonoLaboral telefonoLaboral)
        {
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var consultaTelefono = manage.GetTelefonoLaboral(telefonoLaboral);
            return Json(consultaTelefono.FirstOrDefault());
        }
    }
}

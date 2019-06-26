using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.Colocacion;

namespace TwoFunTwoMeFintech.Controllers
{
    public class GestionDocumentosController : Controller
    {
        //
        // GET: /GestionDocumentos/

        public ActionResult Index()
        {
            return View();
        }


        //Alfredo José Vargas Seinfarth--INICIO//
        #region MetodoGuardaFotoCedula
        [HttpPost]
        public ActionResult GuardaFotoCedula(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();

            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            dto_Config.llave_Config5 = "FOTOCEDULA";
            try
            {
                var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault();
                solicitudes.UrlFoto = URL + solicitudes.Identificacion + "/" + solicitudes.Identificacion;

                FileStream fileStream = new FileStream(solicitudes.UrlFotoCedula, FileMode.Open);

                Image image = Image.FromStream(fileStream);
                fileStream.Close();

                if (manager.ValidarFichero(solicitudes.UrlFotoCedula))
                {
                    image.Save(solicitudes.UrlFotoCedula);
                    image.Dispose();

                }
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return View();

                //	return Ok();
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return View();
        }
        #endregion MetodoGuardaFotoCedula

        #region MetodoGuardaFotoCedulaDorso
        [HttpPost]
        public ActionResult GuardaFotoCedulaDorso(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };

            Tab_ConfigSys dto_Config = new Tab_ConfigSys();

            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            dto_Config.llave_Config5 = "FOTOCEDULATRASERA";

            try
            {

                var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault();

                solicitudes.UrlFoto = URL + solicitudes.Identificacion + "/" + solicitudes.Identificacion;

                FileStream fileStream = new FileStream(solicitudes.UrlFotoCedulaTrasera, FileMode.Open);

                Image image = Image.FromStream(fileStream);
                fileStream.Close();


                if (manager.ValidarFichero(solicitudes.UrlFotoCedulaTrasera))
                {
                    image.Save(solicitudes.UrlFotoCedulaTrasera);
                    image.Dispose();

                }
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return View();

                //	return Ok();
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return View();
        }
        #endregion MetodoGuardaFotoCedulaDorso

        #region MetodoGuardaFotoSelfie
        [HttpPost]
        public ActionResult GuardaFotoSelfie(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };

            Tab_ConfigSys dto_Config = new Tab_ConfigSys();

            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            dto_Config.llave_Config5 = "FOTOSELFIE";

            try
            {
                var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOSELFIE").Select(x => x.Dato_Char1).FirstOrDefault();

                solicitudes.UrlFoto = URL + solicitudes.Identificacion + "/" + solicitudes.Identificacion;

                FileStream fileStream = new FileStream(solicitudes.UrlFotoSelfie, FileMode.Open);

                Image image = Image.FromStream(fileStream);
                fileStream.Close();

                if (manager.ValidarFichero(solicitudes.UrlFotoSelfie))
                {
                    image.Save(solicitudes.UrlFotoSelfie);
                    image.Dispose();

                }
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return View();

                //	return Ok();
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return View();
        }
        #endregion MetodoGuardaFotoSelfie

        #region MetodoGuardaFotoFirma
        [HttpPost]
        public ActionResult GuardaFotoFirma(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };

            Tab_ConfigSys dto_Config = new Tab_ConfigSys();

            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            dto_Config.llave_Config5 = "FOTOFIRMA";

            try
            {
                var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault();

                solicitudes.UrlFoto = URL + solicitudes.Identificacion + "/" + solicitudes.Identificacion;

                FileStream fileStream = new FileStream(solicitudes.UrlFotoFirma, FileMode.Open);

                Image image = Image.FromStream(fileStream);
                fileStream.Close();

                if (manager.ValidarFichero(solicitudes.UrlFotoFirma))
                {
                    image.Save(solicitudes.UrlFotoFirma);
                    image.Dispose();

                }
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return View();

                //	return Ok();
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return View();
        }
        #endregion MetodoGuardaFotoFirma

        #region CreaContratoPagare
        [HttpPost]
        public ActionResult GuardaFotoFirma(PagareContrato pagareContrato)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pagareContrato),
                FEC_CREACION = DateTime.Now
            };


            DTO_SOLICITUD_VENTAS solicitudes = new DTO_SOLICITUD_VENTAS();
            solicitudes.Identificacion = pagareContrato.Identificacion;
            try
            {

                solicitudes.UrlDirectorioPagare = manager.CrearPdf(pagareContrato);

                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return View();
        }
        #endregion CreaContratoPagare

        #region MetodoGirarFotoCedula
        [HttpPost]
        public ActionResult GirarFotoCedula(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var dto_listLogin = new List<DTO_SOLICITUD_VENTAS>();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };

            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            byte[] bytes = null;
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            //dto_Config.llave_Config5 = "FOTOCEDULA";
            AWSAccess wSAccess = new AWSAccess();
            try
            {
                int rotate = 0;

                var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault();

                var SOL_TEMP = manager.ConsultaDirectorioImagen(solicitudes);

                string ext = Path.GetExtension(SOL_TEMP.FirstOrDefault().UrlFotoCedula);
                ext = ext == null ? ".jpg" : ext;

                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = string.Concat(dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), solicitudes.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                UtilBlobStorageAzure.DownloadBlobStorageBytes(blobDowland);

                if (blobDowland.ImageToUploadByte != null)
                    bytes = blobDowland.ImageToUploadByte;



                // FileStream fileStream = new FileStream(SOL_TEMP.FirstOrDefault().UrlFotoCedula, FileMode.Open);

                //solicitudes.UrlFotoCedula = string.Concat(URL, solicitudes.Identificacion, "/", solicitudes.Identificacion, ext);
                //FileStream fileStream = new FileStream(solicitudes.UrlFotoCedula, FileMode.Open);

                //solicitudes.UrlFotoCedula = string.Concat(solicitudes.UrlFotoCedula, ext);
                Image image = Image.FromStream(new MemoryStream(bytes));
                //fileStream.Close();
                rotate = 8;
                //int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                RotateFlipType rotateFlipType = wSAccess.GetOrientationToFlipType(rotate);
                image.RotateFlip(rotateFlipType);
                bytes = (byte[])(new ImageConverter()).ConvertTo(image, typeof(byte[]));
                //if (Utilitarios.ValidarFichero(solicitudes.UrlFotoCedula))
                //{
                //    image.Save(solicitudes.UrlFotoCedula);
                //    image.Dispose();
                //}
                var blob = new blobStorage
                {
                    ImageToUploadByte = bytes,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ext,
                    ImageToUpload = solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULA").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };
                UtilBlobStorageAzure.UploadBlobStorage(blob);
                //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                //threadObj.Start();

                solicitudes.UrlFotoCedula = string.Concat(blob.ContainerPrefix, "/", solicitudes.Identificacion, blob.ImageExtencion);
                dto_listLogin = manager.MetodoGuardaFoto(solicitudes);
                solicitudes.UrlFotoCedula = string.Format("data:{0};base64,{1}", blobDowland.ContentType, Convert.ToBase64String(blobDowland.ImageToUploadByte));
                return Json(dto_listLogin);

                //	return Ok();
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return Json(dto_listLogin);
        }
        #endregion MetodoGirarFotoCedula

        #region MetodoGirarFotoCedulaDorso
        [HttpPost]
        public ActionResult GirarFotoCedulaDorso(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var dto_listLogin = new List<DTO_SOLICITUD_VENTAS>();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            AWSAccess wSAccess = new AWSAccess();
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            byte[] bytes = null;
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            //dto_Config.llave_Config5 = "FOTOCEDULATRASERA";
            try
            {
                var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault();
                int rotate = 0;

                var SOL_TEMP = manager.ConsultaDirectorioImagen(solicitudes);
                string ext = SOL_TEMP.Any() ? Path.GetExtension(SOL_TEMP.FirstOrDefault().UrlFotoCedulaTrasera) : ".jpg";
                ext = ext == null ? ".jpg" : ext;

                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                UtilBlobStorageAzure.DownloadBlobStorageBytes(blobDowland);

                if (blobDowland.ImageToUploadByte != null)
                    bytes = blobDowland.ImageToUploadByte;
                //solicitudes.UrlFotoCedulaTrasera = string.Concat(URL, solicitudes.Identificacion, "/" + solicitudes.Identificacion, ext);

                // FileStream fileStream = new FileStream(SOL_TEMP.FirstOrDefault().UrlFotoCedula, FileMode.Open);

                //FileStream fileStream = new FileStream(solicitudes.UrlFotoCedulaTrasera, FileMode.Open);

                //solicitudes.UrlFotoCedulaTrasera = string.Concat(solicitudes.UrlFotoCedulaTrasera, ext);
                Image image = Image.FromStream(new MemoryStream(bytes));
                // fileStream.Close();
                rotate = 8;
                //int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                RotateFlipType rotateFlipType = wSAccess.GetOrientationToFlipType(rotate);
                image.RotateFlip(rotateFlipType);
                bytes = (byte[])(new ImageConverter()).ConvertTo(image, typeof(byte[]));
                //if (Utilitarios.ValidarFichero(solicitudes.UrlFotoCedulaTrasera))
                //{
                //    image.Save(solicitudes.UrlFotoCedulaTrasera);
                //    image.Dispose();
                //}
                var blob = new blobStorage
                {
                    ImageToUploadByte = bytes,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ext,
                    ImageToUpload = solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOCEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };
                UtilBlobStorageAzure.UploadBlobStorage(blob);
                //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                //threadObj.Start();

                solicitudes.UrlFotoCedulaTrasera = string.Concat(blob.ContainerPrefix, "/", solicitudes.Identificacion, blob.ImageExtencion);
                dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return Json(dto_listLogin);

                //	return Ok();
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return Json(dto_listLogin);
        }
        #endregion MetodoGirarFotoCedulaDorso

        #region MetodoGirarFotoSelfie
        [HttpPost]
        public ActionResult GirarFotoSelfie(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var dto_listLogin = new List<DTO_SOLICITUD_VENTAS>();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            AWSAccess wSAccess = new AWSAccess();
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();

            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            // dto_Config.llave_Config5 = "FOTOSELFIE";
            byte[] bytes = null;
            int rotate = 0;
            try
            {
                var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOSELFIE").Select(x => x.Dato_Char1).FirstOrDefault();


                var SOL_TEMP = manager.ConsultaDirectorioImagen(solicitudes);


                string ext = Path.GetExtension(SOL_TEMP.FirstOrDefault().UrlFotoSelfie);
                ext = ext == null ? ".jpg" : ext;
                //solicitudes.UrlFotoSelfie = String.Concat(URL, solicitudes.Identificacion, "/" + solicitudes.Identificacion, ext);
                // FileStream fileStream = new FileStream(SOL_TEMP.FirstOrDefault().UrlFotoCedula, FileMode.Open);

                //FileStream fileStream = new FileStream(solicitudes.UrlFotoSelfie, FileMode.Open);
                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_SELFIE").Select(x => x.Dato_Char1).FirstOrDefault(), "/", solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ext,
                    ImageToUpload = solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOSELFIE").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };
                UtilBlobStorageAzure.DownloadBlobStorageBytes(blobDowland);

                if (blobDowland.ImageToUploadByte != null)
                    bytes = blobDowland.ImageToUploadByte;

                Image image = Image.FromStream(new MemoryStream(bytes));
                //fileStream.Close();
                rotate = 8;
                //int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                RotateFlipType rotateFlipType = wSAccess.GetOrientationToFlipType(rotate);
                image.RotateFlip(rotateFlipType);
                bytes = (byte[])(new ImageConverter()).ConvertTo(image, typeof(byte[]));
                //if (Utilitarios.ValidarFichero(solicitudes.UrlFotoSelfie))
                //{
                //    image.Save(solicitudes.UrlFotoSelfie);
                //    image.Dispose();
                //}

                var blob = new blobStorage
                {
                    ImageToUploadByte = bytes,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_SELFIE").Select(x => x.Dato_Char1).FirstOrDefault(), "/", solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ext,
                    ImageToUpload = solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOSELFIE").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };
                UtilBlobStorageAzure.UploadBlobStorage(blob);
                //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                //threadObj.Start();

                solicitudes.UrlFotoSelfie = string.Concat(blob.ContainerPrefix, "/", solicitudes.Identificacion, blob.ImageExtencion);

                dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return Json(dto_listLogin);

                //	return Ok();
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return Json(dto_listLogin);
        }
        #endregion MetodoGirarFotoSelfie

        #region MetodoGiraFotoFirma
        [HttpPost]
        public ActionResult GiraFotoFirma(DTO_SOLICITUD_VENTAS solicitudes)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var dto_listLogin = new List<DTO_SOLICITUD_VENTAS>();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            AWSAccess wSAccess = new AWSAccess();
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };

            Tab_ConfigSys dto_Config = new Tab_ConfigSys();

            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            //dto_Config.llave_Config5 = "FOTOFIRMA";
            byte[] bytes = null;
            try
            {
                int rotate = 0;
                var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
                string URL = dto_interval.Where(x => x.llave_Config5 == "FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault();


                var SOL_TEMP = manager.ConsultaDirectorioImagen(solicitudes);
                string ext = Path.GetExtension(SOL_TEMP.FirstOrDefault().UrlFotoFirma);
                ext = ext == null ? ".jpg" : ext;
                //solicitudes.UrlFotoFirma = String.Concat(URL + solicitudes.Identificacion + "/" + solicitudes.Identificacion, ext);

                // FileStream fileStream = new FileStream(SOL_TEMP.FirstOrDefault().UrlFotoCedula, FileMode.Open);

                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };

                //FileStream fileStream = new FileStream(solicitudes.UrlFotoFirma, FileMode.Open);

                UtilBlobStorageAzure.DownloadBlobStorageBytes(blobDowland);

                if (blobDowland.ImageToUploadByte != null)
                    bytes = blobDowland.ImageToUploadByte;

                Image image = Image.FromStream(new MemoryStream(bytes));
                //fileStream.Close();
                rotate = 8;
                //int orientationValue = image.GetPropertyItem(prop.Id).Value[0];
                RotateFlipType rotateFlipType = wSAccess.GetOrientationToFlipType(rotate);
                image.RotateFlip(rotateFlipType);
                bytes = (byte[])(new ImageConverter()).ConvertTo(image, typeof(byte[]));
                //if (Utilitarios.ValidarFichero(solicitudes.UrlFotoFirma))
                //{
                //    image.Save(solicitudes.UrlFotoFirma);
                //    image.Dispose();
                //}

                var blob = new blobStorage
                {
                    ImageToUploadByte = bytes,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", solicitudes.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ext,
                    ImageToUpload = solicitudes.Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                };
                UtilBlobStorageAzure.UploadBlobStorage(blob);
                //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                //threadObj.Start();

                solicitudes.UrlFotoFirma = string.Concat(blob.ContainerPrefix, "/", solicitudes.Identificacion, blob.ImageExtencion);

                dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return Json(dto_listLogin);

                //	return Ok();
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return Json(dto_listLogin);
        }
        #endregion MetodoGiraFotoFirma

        #region CreaContratoPagare
        [HttpPost]
        public ActionResult CreaContratoPagare(PagareContrato pagareContrato)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var dto_listLogin = new List<DTO_SOLICITUD_VENTAS>();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pagareContrato),
                FEC_CREACION = DateTime.Now
            };

            DTO_SOLICITUD_VENTAS solicitudes = new DTO_SOLICITUD_VENTAS();
            solicitudes.Identificacion = pagareContrato.Identificacion;
            solicitudes.IdSolicitud = pagareContrato.IdSolicitud;
            try
            {

                solicitudes.UrlDirectorioPagare = manager.CrearPdf(pagareContrato);

                dto_listLogin = manager.MetodoGuardaFoto(solicitudes);
                return Json(dto_listLogin);
            }
            catch (ArgumentException)
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            return Json(dto_listLogin);
        }
        #endregion CreaContratoPagare
        //Alfredo José Vargas Seinfarth--FIN//

        #region Crear Documento Domiciliacion
        [HttpPost]
        public ActionResult CrearDocumentoDom(PagareContrato documentoDom)
        {
            ManagerSolcitudes manager = new ManagerSolcitudes();
            dto_file dto_File = new dto_file();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var dto_listLogin = new List<DTO_SOLICITUD_VENTAS>();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(documentoDom),
                FEC_CREACION = DateTime.Now
            };

            DTO_SOLICITUD_VENTAS solicitudes = new DTO_SOLICITUD_VENTAS();
            solicitudes.Identificacion = documentoDom.Identificacion;
            solicitudes.IdSolicitud = documentoDom.IdSolicitud;
            try
            {
                //creamos el documento de domiciliacion y guardamos al url del archivo
                dto_File = manager.CrearDocDom(documentoDom);

            }
            catch (ArgumentException)
            {
                dto_File.Respuesta = "Ocurrio un Error";
            }

            return Json(dto_File);
            ///System.IO.File.Delete(rutaDocDom);
        }
        #endregion

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
    }
}

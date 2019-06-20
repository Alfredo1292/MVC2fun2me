using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class AprobadoVerificacionTesoreriaController : Controller
    {
        //
        // GET: /AprobadoVerificacionTesoreria/

        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Details()
        {
            return View();

        }
        public ActionResult GetAprobadoVerificacionTesoreria(AprobadoVerificacionTesoreria tesoreria)
        {
            ManagerUser mang = new ManagerUser();
            var listaSolicitudesPendienteTesoreria = mang.consultaAprobadoVerificacionTesoreria(tesoreria);
            return Json(listaSolicitudesPendienteTesoreria);
        }

        public string CambiarEstadoSolicitud(int id, int nuevoStatus)
        {
            string mensaje = "";
            string estadoGestion;
            switch (nuevoStatus) {
                case 1:
                    estadoGestion = "Cuenta incorrecta";
                    break;
                case 2:
                    estadoGestion = "Cuenta no pertenece al cliente";
                    break;
                case 3:
                    estadoGestion = "Cuenta inactiva";
                    break;
                case 4:
                    estadoGestion = "Verificado OK";
                    break;
                default:
                    estadoGestion = "";
                    break;
            }

            ManagerUser mang = new ManagerUser();
            if (nuevoStatus > 0 && nuevoStatus < 4)
            {
                nuevoStatus = 110;
            }
            else {
                if (nuevoStatus == 4) {
                    nuevoStatus = 38;
                }               
            }
            if (nuevoStatus != 0)
            {
                string user = Session["username"].ToString();
                mensaje = mang.cabiarStatusSolicitudes(id, 0, nuevoStatus, user, estadoGestion);
            }
            else {
                mensaje = "El Status no a sido Seleccionado o no es correcto";
            }
            return mensaje;

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
                    PatchTempToSave = AppDomain.CurrentDomain.BaseDirectory+string.Concat(dto_interval.Where(x => x.llave_Config5 == "IMAGENSINPE").Select(x => x.Dato_Char1).FirstOrDefault(), imagen.Identificacion)//@"C:\IVAN\images\FotoCedulas\206560175"
                };


                //Limpiamos la carpeta IMAGENSINPE si existe
                string LimpiarCarpeta = blobDowland.PatchTempToSave;
                if (Directory.Exists(LimpiarCarpeta)) {
                    List<string> strDirectories = Directory.GetDirectories(LimpiarCarpeta, "*", SearchOption.AllDirectories).ToList();
                    foreach (string directorio in strDirectories)
                    {
                        Directory.Delete(directorio, true);
                    }
                }

                //Creamos la carpeta IMAGENSINPE y la carpeta #cedula si no existe
                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);

                //Si existe la imagen en Azure procedemos a descargarla en la carpeta Temporal Para mostrarla
                if (UtilBlobStorageAzure.ExistsFileInBlob(blobDowland))
                {
                    UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                    imagen.UrlImagenSinpe = string.Concat(dto_interval.Where(x => x.llave_Config5 == "IMAGENSINPE").Select(x => x.Dato_Char1).FirstOrDefault()) +@"\" + imagen.Identificacion + @"\"+imagen.Identificacion+ blobDowland.ImageExtencion;
                    imagen.Mensaje = "Completo";
                }
                else {
                    imagen.Mensaje = "Este cliente no tiene imagen sinpe.";
                }
                
                return Json(imagen);
            }
            catch(Exception ex) {
                imagen.Mensaje = "Error al mostrar la imagen";
                return Json(imagen);
            }
        }

    }
}

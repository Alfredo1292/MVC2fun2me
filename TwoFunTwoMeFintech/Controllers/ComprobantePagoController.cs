using System;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.Manager;
using System.Linq;
using System.IO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class ComprobantePagoController : Controller
    {
        //
        // GET: /ComprobantePago/

        public ActionResult ComprobantePago()
        {
            return View();
        }

        public ActionResult ConsultarComprobante(comprobantepago compPago)
        {
            ManagerUser manager = new ManagerUser();
            var dto_ret = new System.Collections.Generic.List<comprobantepago>();
            try
            {
                compPago.Accion = "CONSULTA";
                compPago.UsuarioModificacion = Session["agente"].ToString();
                dto_ret = manager.comrobantePago(compPago);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(dto_ret);
        }

        public ActionResult EditaComprobante(comprobantepago compPago)
        {
            ManagerUser manager = new ManagerUser();
            ManagerSolcitudes managerSolicitudes = new ManagerSolcitudes();
            var dto_ret = new System.Collections.Generic.List<comprobantepago>();
            var tempId = compPago.Identificacion;
            compPago.Identificacion = null;
            try
            {
                compPago.Accion = "CONSULTA";
                compPago.UsuarioModificacion = Session["agente"].ToString();
                dto_ret = manager.comrobantePago(compPago);

                Tab_ConfigSys dto_Config = new Tab_ConfigSys();


                dto_Config.llave_Config1 = "SERVICIO";
                dto_Config.llave_Config2 = "CONFIGURACION";
                dto_Config.llave_Config3 = "SERVIDOR";
                dto_Config.llave_Config4 = "URL";

                var dto_interval = managerSolicitudes.ConsultaConfiUrlImagen(dto_Config);

                compPago.Identificacion = tempId;

                var blob = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_COMP_PAGO").Select(x => x.Dato_Char1).FirstOrDefault(), "/", compPago.Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = ".jpg",
                    ImageToUpload = Path.GetFileName(dto_ret.FirstOrDefault().RutaBlob).Replace(".jpg", string.Empty),
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = string.Empty
                };
                UtilBlobStorageAzure.DownloadBlobStorageBytes(blob);
                if (blob.ImageToUploadByte != null)
                {
                    dto_ret.FirstOrDefault().ImageComprobante = Convert.ToBase64String(blob.ImageToUploadByte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(dto_ret);
        }
        public ActionResult MantenimientoComprobante(comprobantepago compPago)
        {
            ManagerUser manager = new ManagerUser();
            var dto_ret = new comprobantepago();
            try
            {
                compPago.Accion = "ACTUALIZA";
                compPago.UsuarioModificacion = Session["agente"].ToString();
                manager.comrobantePago(compPago);


                dto_ret.Mensaje = "OK";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(dto_ret);
        }

        public ActionResult ConsultaBancos()
        {
            ManagerCuentaBancaria managerCuenta = new ManagerCuentaBancaria();
            var dto_ret = new System.Collections.Generic.List<Bancos>();
            try
            {

                dto_ret = managerCuenta.Trae_Bancos();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(dto_ret);
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
    }
}

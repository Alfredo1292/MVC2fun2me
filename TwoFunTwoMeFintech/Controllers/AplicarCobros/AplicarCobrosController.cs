using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.ApicarCobros;
using System.Linq;

namespace TwoFunTwoMeFintech.Controllers
{
    public class AplicarCobrosController : Controller
    {
        public object DialogResult { get; private set; }

        //
        // GET: /AplicarCobros/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCuentasCobros(CuentasCobros cuentas)
        {
            ManagerUser mang = new ManagerUser();
            var listaSTranferencias = mang.consultaCuentasCobros(cuentas);
            return Json(listaSTranferencias);
        }
        public string GuardarCuenta(CuentaGuardar cuenta)
        {
            ManagerUser mang = new ManagerUser();
            cuenta.User = Session["agente"].ToString();
            var mensajeSalida = mang.GuardarCuenta(cuenta);
            return mensajeSalida;
        }
        public ActionResult SubirArchivosCobros(ArchivoCobros[] cobros)
        {
            //ManagerUser mang = new ManagerUser();
            ManagerSolcitudes mang = new ManagerSolcitudes();
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();

            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";

            try
            {
                var dto_interval = mang.ConsultaConfiUrlImagen(dto_Config);

                //creamos el grupo de byte para guardarlos
                byte[] bytesBCR = Convert.FromBase64String(cobros[0].Base64);
                byte[] bytesBAC = Convert.FromBase64String(cobros[1].Base64);
                byte[] bytesBN = Convert.FromBase64String(cobros[2].Base64);

                //guardamos los grupos de bytes con los nombres y rutas por defecto
                //System.IO.File.WriteAllBytes(path + "ECBCR.xlsx", bytesBCR);
                //System.IO.File.WriteAllBytes(path + "ECBAC.xlsx", bytesBCR);
                //System.IO.File.WriteAllBytes(path + "ECBN.csv", bytesBCR);
                var fileBCR = new blobStorage
                {
                    ImageToUploadByte = bytesBCR,
                    ContainerPrefix = dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_DOC_DOMICILIACION").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ImageExtencion = dto_interval.Where(x => x.llave_Config5 == "EXTENCION_DESCARGA_2").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ImageToUpload = dto_interval.Where(x => x.llave_Config5 == "ENTIDA_DESCARGA_1").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "RUTA_DESCARGA").Select(x => x.Dato_Char1).FirstOrDefault()
                    //ContainerPrefix = "documentosdomiciliacion",
                    //ImageExtencion = ".xlsx",
                    //ImageToUpload = "ECBCR",
                    //ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    //PatchTempToSave = @"C:\IVAN\"
                };
                var fileBAC = new blobStorage
                {
                    ImageToUploadByte = bytesBAC,
                    ContainerPrefix = dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_DOC_DOMICILIACION").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ImageExtencion = dto_interval.Where(x => x.llave_Config5 == "EXTENCION_DESCARGA_2").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ImageToUpload = dto_interval.Where(x => x.llave_Config5 == "ENTIDA_DESCARGA_2").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "RUTA_DESCARGA").Select(x => x.Dato_Char1).FirstOrDefault()
                    //ContainerPrefix = "documentosdomiciliacion",
                    //ImageExtencion = ".xlsx",
                    //ImageToUpload = "ECBAC",
                    //ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    //PatchTempToSave = @"C:\IVAN\"
                };
                var fileBN = new blobStorage
                {
                    ImageToUploadByte = bytesBN,
                    ContainerPrefix = dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_DOC_DOMICILIACION").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ImageExtencion = dto_interval.Where(x => x.llave_Config5 == "EXTENCION_DESCARGA_1").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ImageToUpload = dto_interval.Where(x => x.llave_Config5 == "ENTIDA_DESCARGA_3").Select(x => x.Dato_Char1).FirstOrDefault(),
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),
                    PatchTempToSave = dto_interval.Where(x => x.llave_Config5 == "RUTA_DESCARGA").Select(x => x.Dato_Char1).FirstOrDefault()
                    //ContainerPrefix = "documentosdomiciliacion",
                    //ImageExtencion = ".csv",
                    //ImageToUpload = "ECBN",
                    //ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    //PatchTempToSave = @"C:\IVAN\"
                };
                //Thread threadObj = new Thread(new ThreadStart(()=>UtilBlobStorageAzure.UploadBlobStorage(fileBCR)));
                //threadObj.Start();
                UtilBlobStorageAzure.UploadBlobStorage(fileBCR, false); // el segundo parametro nos dira si desea hacer copia de respaldo o no, su valor por default es true.
                UtilBlobStorageAzure.UploadBlobStorage(fileBAC, false);
                UtilBlobStorageAzure.UploadBlobStorage(fileBN, false);
                //mang.bndSubirArchivos(1);
                mang.InsertaCobroAutomatico(new AplicaCobrosAutomaticos { Action = "INSERTA" });
                return Json("Los archivos se subieron correctamente");
            }
            catch (Exception ex)
            {
                return Json("Error al Subir los Archivos");
            }
        }
    }
}

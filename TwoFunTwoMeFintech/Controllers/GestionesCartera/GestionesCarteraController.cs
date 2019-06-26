using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO.Colocacion;
using TwoFunTwoMeFintech.Models.DTO.GestionesCartera;
using TwoFunTwoMeFintech.Models.Manager;

namespace TwoFunTwoMeFintech.Controllers.GestionesCartera
{
    public class GestionesCarteraController : Controller
    {
        //
        // GET: /GestionesCartera/

        public ActionResult Index()
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            return View();
        }

        public ActionResult HistoricoGestiones(GestionCartera gestion)

        {
            if (gestion.IdCredito == null) return View();

            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultaHistoricoGestiones(gestion);

            return Json(ret);
        }
        public ActionResult HistoricoPromesasPagos(GestionCartera gestion)
        {
            if (gestion.IdCredito == null) return View();

            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultaPromesasPagos(gestion);

            return Json(ret);
        }
        public ActionResult HistoricoPagos(GestionCartera gestion)
        {
            if (gestion.IdCredito == null) return View();

            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultaPagos(gestion);

            return Json(ret);
        }
        public ActionResult consultarGridCreditos(GestionCuenta cuenta)
        {
            if (cuenta.IdCredito == null) return View();
            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.consultaCritoCredito(cuenta);

            return Json(ret);
        }
        public ActionResult consultarPrestamosCliente(GestionPrestamo prestamos)
        {
            if (prestamos.Identificacion == null) return View();
            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.consultaPrestamosCliente(prestamos);

            return Json(ret);
        }
        public ActionResult consultarPrestamosParientes(GestionPersona personas)
        {
            if (personas.Identificacion == null) return View();
            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.consultaCreditoParientes(personas);

            return Json(ret);
        }
        public ActionResult calculaCuota(GestionCartera mora)
        {
            if (mora.IdCredito == null || mora.Fecha == null) return View();
            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.calculaCuota(mora);

            return Json(ret);
        }
        public ActionResult ReferenciasPersonales(GestionContacto contacto)
        {
            if (contacto.Identificacion == null) return View();
            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.referenciasPersonales(contacto);

            return Json(ret);
        }
        public ActionResult Contactos(GestionContacto contac)

        {
            if (contac.Identificacion == null) return View();

            ManagerGestionCartera manage = new ManagerGestionCartera();


            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarContactos(contac);

            return Json(ret);
        }
        public ActionResult BuscarCliente(GestionBucket bucketCobros)
        {
            ManagerGestionCartera manage = new ManagerGestionCartera();
           
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            if ((bucketCobros.Identificacion.Length != 9 && bucketCobros.Identificacion.Length != 12) )
            {
                bucketCobros.IdCredito = int.Parse(bucketCobros.Identificacion);
                bucketCobros.Identificacion = null;
                bucketCobros.Nombre = "";
            }
            var dto_ret = manage.ObtenerEncabezado(bucketCobros);

            return Json(dto_ret);
        }
        public ActionResult comboTipo()
        {
            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.monstrarAciones();

            return Json(ret);
        }
        //public ActionResult HistoricoPlanPagos(GestionPlanPlagos plan)
        //{
        //    if (plan.IdCredito == null) return View();

        //    ManagerGestionCartera manage = new ManagerGestionCartera();

        //    if (Session["agente"] == null)
        //        return RedirectToAction("LogOff", "Login");

        //    var ret = manage.ConsultaPlanPagos(plan);

        //    return Json(ret);
        //}
        public ActionResult conultar_Datos_creditos(GestionCuenta cuenta)
        {
            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.consultaInformacionCuenta(cuenta);

            return Json(ret);
        }
        public ActionResult CorreoPersona(GestionPersona personas)

        {
            if (personas.Identificacion == null) return View();

            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.correosPersona(personas);

            return Json(ret);
        }
        public ActionResult ContactosDireccion(GestionContacto contacto)
        {
            if (contacto.Identificacion == null) return View();
            ManagerGestionCartera manage = new ManagerGestionCartera();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ConsultarDirecciones(contacto);
            return Json(ret);
        }
        public ActionResult mostrar_imagenes(GestionSolicitudes solicitudes)
        {
            ManagerGestionCartera manage = new ManagerGestionCartera();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.MostrarImagenesCreddid(solicitudes);

            return Json(ret);
        }
        public ActionResult BuscarClienteNombre(GestionBucket bucketCobros)
        {
            ManagerGestionCartera manage = new ManagerGestionCartera();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            bucketCobros.cod_agente = Session["agente"].ToString();
            var dto_ret = manage.BusquedaNombre(bucketCobros);

            return Json(dto_ret);
        }
        public ActionResult EjecutarAccionCredito(GestionAccion gestion)
        {
            ManagerGestionCartera manage = new ManagerGestionCartera();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            gestion.Usuario = Session["agente"].ToString();
            var dto_ret = manage.EjecutarAccion(gestion);

            return Json(dto_ret.FirstOrDefault());
        }
        #region Administrar Documentos
        public ActionResult AdministrarDocumentos()
        {
            return View();
        }
        public ActionResult ExisteDocumento(string tipoDocumento, string identificacion)
        {
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            blobStorage blob = null;
            blobStorage blob_1 = null;
            if (tipoDocumento == "PAGARE")
            {
                blob = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = "documentos/Pagare/" + identificacion,
                    ImageToUpload = identificacion,
                    ImageExtencion = ".pdf",
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = null
                };
                blob_1 = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = "documentos/Pagare/" + identificacion,
                    ImageToUpload = identificacion + "_1",
                    ImageExtencion = ".pdf",
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = null
                };
            }
            if (tipoDocumento == "LEGAL")
            {
                blob = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = "documentos/ProcesoLegal/" + identificacion,
                    ImageToUpload = identificacion,
                    ImageExtencion = ".pdf",
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = null
                };
                blob_1 = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = "documentos/ProcesoLegal/" + identificacion,
                    ImageToUpload = identificacion + "_1",
                    ImageExtencion = ".pdf",
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = null
                };
            }
            if (blob != null && blob_1 != null)
            {
                if (UtilBlobStorageAzure.ExistsFileInBlob(blob) || UtilBlobStorageAzure.ExistsFileInBlob(blob_1))
                {
                    return Json("SI");
                }
                else
                {
                    return Json("NO");
                }
            }
            else {
                return Json("NULL");
            }          
        }
        public ActionResult SubirDocumentoPagare(GestionArchivoSubir archivo)
        {
            try
            {
                //creamos el grupo de byte para guardarlos
                byte[] bytesPagare = Convert.FromBase64String(archivo.Base64);

                var filePagare = new blobStorage
                {
                    ImageToUploadByte = bytesPagare,
                    ContainerPrefix = "documentos/Pagare/"+archivo.Identificacion+"/",
                    ImageExtencion = ".pdf",
                    ImageToUpload = archivo.Identificacion,
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                };

                if (UtilBlobStorageAzure.ExistsFileInBlob(filePagare))
                {
                    return Json("El Pagare ya existe. No se permite Remplazarlo");
                }
                else {
                    UtilBlobStorageAzure.UploadBlobStorage(filePagare, false); // el segundo parametro nos dira si desea hacer copia de respaldo o no, su valor por default es true.
                    return Json("El Pagare se cargo correctamente");
                }
            }
            catch (Exception ex)
            {
                return Json("Error al Subir los Archivos");
            }
        }
        public ActionResult SubirDocumentoLegal(GestionArchivoSubir archivo)
        {
            try
            {
                //creamos el grupo de byte para guardarlos
                byte[] bytesLegal = Convert.FromBase64String(archivo.Base64);

                var fileLegal = new blobStorage
                {
                    ImageToUploadByte = bytesLegal,
                    ContainerPrefix = "documentos/ProcesoLegal/"+archivo.Identificacion+"/",
                    ImageExtencion = ".pdf",
                    ImageToUpload = archivo.Identificacion,
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                };

                if (UtilBlobStorageAzure.ExistsFileInBlob(fileLegal))
                {
                    return Json("El Documento Legal ya existe. No se permite Remplazarlo");
                }
                else
                {
                    UtilBlobStorageAzure.UploadBlobStorage(fileLegal, false); // el segundo parametro nos dira si desea hacer copia de respaldo o no, su valor por default es true.
                    return Json("El Documento Legal se cargo correctamente");
                }
            }
            catch (Exception ex)
            {
                return Json("Error al Subir los Archivos");
            }
        }
        public ActionResult DescargarDocumentoPagare(string identificacion)
        {
            blobStorage filePagare = null;
            dto_file dto_File = null;
            try
            {
                filePagare = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = "documentos/Pagare/" + identificacion + "/",
                    ImageExtencion = ".pdf",
                    ImageToUpload = identificacion,
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = @"C:\DescargasDocumentosPagareTemp"
                };
              
                if (UtilBlobStorageAzure.ExistsFileInBlob(filePagare))
                {
                    if (!Directory.Exists(filePagare.PatchTempToSave))
                        Directory.CreateDirectory(filePagare.PatchTempToSave);

                    UtilBlobStorageAzure.DownloadBlobStorage(filePagare);
                    var rutaPagare = filePagare.PatchTempToSave + "/" + filePagare.ImageToUpload+".pdf";

                    dto_File = new dto_file();  
                    dto_File.Type = TwoFunTwoMe_DataAccess.Utility.GetMimeType(".pdf");
                    dto_File.Base64 = string.Format("data:{0};base64,{1}", dto_File.Type, Convert.ToBase64String(System.IO.File.ReadAllBytes(rutaPagare)));
                    dto_File.Name = "Pagare-" + filePagare.ImageToUpload;

                    //Borramos la carpeta temporal con los archivos creados para limpiarla
                    if (Directory.Exists(filePagare.PatchTempToSave))
                        Directory.Delete(filePagare.PatchTempToSave, true);

                    return Json(dto_File);
                }
                else {
                    //Si no existe el archivo con el numero de cedula de nombre, verificamos si el archivo existe con el nombre "identificacion_1"
                    filePagare.ImageToUpload = identificacion + "_1";
                    if (UtilBlobStorageAzure.ExistsFileInBlob(filePagare))
                    {
                        if (!Directory.Exists(filePagare.PatchTempToSave))
                            Directory.CreateDirectory(filePagare.PatchTempToSave);

                        UtilBlobStorageAzure.DownloadBlobStorage(filePagare);
                        var rutaPagare = filePagare.PatchTempToSave + "/" + filePagare.ImageToUpload + filePagare.ImageExtencion;

                        dto_File = new dto_file();
                        dto_File.Type = TwoFunTwoMe_DataAccess.Utility.GetMimeType(".pdf");
                        dto_File.Base64 = string.Format("data:{0};base64,{1}", dto_File.Type, Convert.ToBase64String(System.IO.File.ReadAllBytes(rutaPagare)));
                        dto_File.Name = "Pagare-" + filePagare.ImageToUpload;

                        //Borramos la carpeta temporal con los archivos creados para limpiarla
                        if (Directory.Exists(filePagare.PatchTempToSave))
                            Directory.Delete(filePagare.PatchTempToSave, true);

                        return Json(dto_File);
                    }
                    else {
                        return Json("Pagare no existe");
                    }
                    
                }   
            }
            catch (Exception ex)
            {
                return Json("Error al Descargar el archivo");
            }
        }
        public ActionResult DescargarDocumentoLegal(string identificacion)
        {
            blobStorage fileLegal = null;
            dto_file dto_File = null;
            try
            {
                fileLegal = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = "documentos/ProcesoLegal/" + identificacion + "/",
                    ImageExtencion = ".pdf",
                    ImageToUpload = identificacion,
                    ConnectionString = "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = @"C:\DescargasDocumentosLegalTemp"
                };

                if (UtilBlobStorageAzure.ExistsFileInBlob(fileLegal))
                {
                    if (!Directory.Exists(fileLegal.PatchTempToSave))
                        Directory.CreateDirectory(fileLegal.PatchTempToSave);

                    UtilBlobStorageAzure.DownloadBlobStorage(fileLegal);
                    var rutaPagare = fileLegal.PatchTempToSave + "/" + fileLegal.ImageToUpload + ".pdf";

                    dto_File = new dto_file();
                    dto_File.Type = TwoFunTwoMe_DataAccess.Utility.GetMimeType(".pdf");
                    dto_File.Base64 = string.Format("data:{0};base64,{1}", dto_File.Type, Convert.ToBase64String(System.IO.File.ReadAllBytes(rutaPagare)));
                    dto_File.Name = "DocumentoLegal-" + fileLegal.ImageToUpload;

                    //Borramos la carpeta temporal con los archivos creados para limpiarla
                    if (Directory.Exists(fileLegal.PatchTempToSave))
                        Directory.Delete(fileLegal.PatchTempToSave, true);

                    return Json(dto_File);
                }
                else
                {
                    //Si no existe el archivo con el numero de cedula de nombre, verificamos si el archivo existe con el nombre "identificacion_1"
                    fileLegal.ImageToUpload = identificacion + "_1";
                    if (UtilBlobStorageAzure.ExistsFileInBlob(fileLegal))
                    {
                        if (!Directory.Exists(fileLegal.PatchTempToSave))
                            Directory.CreateDirectory(fileLegal.PatchTempToSave);

                        UtilBlobStorageAzure.DownloadBlobStorage(fileLegal);
                        var rutaPagare = fileLegal.PatchTempToSave + "/" + fileLegal.ImageToUpload + fileLegal.ImageExtencion;

                        dto_File = new dto_file();
                        dto_File.Type = TwoFunTwoMe_DataAccess.Utility.GetMimeType(".pdf");
                        dto_File.Base64 = string.Format("data:{0};base64,{1}", dto_File.Type, Convert.ToBase64String(System.IO.File.ReadAllBytes(rutaPagare)));
                        dto_File.Name = "DocumentoLegal-" + fileLegal.ImageToUpload;

                        //Borramos la carpeta temporal con los archivos creados para limpiarla
                        if (Directory.Exists(fileLegal.PatchTempToSave))
                            Directory.Delete(fileLegal.PatchTempToSave, true);

                        return Json(dto_File);
                    }
                    else
                    {
                        return Json("Documento Legal no existe");
                    }

                }
            }
            catch (Exception ex)
            {
                return Json("Error al Descargar el archivo");
            }
        }
        #endregion
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.Manager;

namespace TwoFunTwoMeFintech.Controllers
{
    public class CuentaBancariaController : Controller
    {
        //
        // GET: /CuentaBancaria/

        public ActionResult Index()
        {
            return View();
        }

		#region ConsultaCuentasBancarias
		[HttpPost]
		public ActionResult ConsultaCuentasBancarias(CuentaBancaria cuentaBancaria)
		{
			ManagerCuentaBancaria managerCuenta = new ManagerCuentaBancaria();
			string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
			string xProceso = MethodBase.GetCurrentMethod().Name;
			string strHostName = System.Net.Dns.GetHostName();
			//IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
			IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			var RespCuentaBanc = new List<CuentaBancaria>();

			var dto_excepcion = new UTL_TRA_EXCEPCION
			{
				STR_CLASE = xClase,
				STR_EVENTO = xProceso,
				STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
				STR_SERVIDOR = System.Net.Dns.GetHostName(),
				STR_PARAMETROS = JsonConvert.SerializeObject(cuentaBancaria),
				FEC_CREACION = DateTime.Now
			};			
			try
			{

				RespCuentaBanc = managerCuenta.uspConsultaCuentasBancarias(cuentaBancaria);


				return Json(RespCuentaBanc);

				//	return Ok();
			}
			catch (ArgumentException)
			{
				cuentaBancaria.Respuesta = "Ocurrio un Error";
			}
			return Json(RespCuentaBanc);
		}
		#endregion ConsultaCuentasBancarias

		#region EditaCuentasBancarias
		public ActionResult EditaCuentasBancarias(int Id)
		{
			ManagerCuentaBancaria managerCuenta = new ManagerCuentaBancaria();
			string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
			string xProceso = MethodBase.GetCurrentMethod().Name;
			string strHostName = System.Net.Dns.GetHostName();
			//IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
			IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			var RespCuentaBanc = new List<CuentaBancaria>();

			var cuentabancaria = new CuentaBancaria
			{
				Id = Id
			};

			var dto_excepcion = new UTL_TRA_EXCEPCION
			{
				STR_CLASE = xClase,
				STR_EVENTO = xProceso,
				STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
				STR_SERVIDOR = System.Net.Dns.GetHostName(),
				STR_PARAMETROS = JsonConvert.SerializeObject(Id),
				FEC_CREACION = DateTime.Now
			};
			try
			{

				RespCuentaBanc = managerCuenta.uspConsultaEditCuentaBancaria(cuentabancaria);
				if (RespCuentaBanc.Any())
				{
					RespCuentaBanc.FirstOrDefault().ListBancos = managerCuenta.Trae_Bancos();
					RespCuentaBanc.FirstOrDefault().ListTipoMoneda = managerCuenta.TraeTipoMoneda();
					RespCuentaBanc.FirstOrDefault().ListTipoCuentas = managerCuenta.TraeTipoCuenta();
				}
				else
				{
					RespCuentaBanc = new List<CuentaBancaria>();
					RespCuentaBanc.FirstOrDefault().ListBancos = managerCuenta.Trae_Bancos();
					RespCuentaBanc.FirstOrDefault().ListTipoMoneda = managerCuenta.TraeTipoMoneda();
					RespCuentaBanc.FirstOrDefault().ListTipoCuentas = managerCuenta.TraeTipoCuenta();
				}
				return Json(RespCuentaBanc.FirstOrDefault(), JsonRequestBehavior.AllowGet);

				//	return Ok();
			}
			catch (ArgumentException)
			{
				RespCuentaBanc.FirstOrDefault().Respuesta = "Ocurrio un Error";
			}
			return Json(RespCuentaBanc.FirstOrDefault());
		}
		#endregion EditaCuentasBancarias

		#region CreaCuentasBancarias
		public ActionResult CreaCuentasBancarias(CuentaBancaria cuentaBancaria)
		{
			ManagerCuentaBancaria managerCuenta = new ManagerCuentaBancaria();
			string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
			string xProceso = MethodBase.GetCurrentMethod().Name;
			string strHostName = System.Net.Dns.GetHostName();
			//IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
			IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			var RespCuentaBanc = new List<CuentaBancaria>();
			var calcula = new List<CuentaBancaria>();
			var valida = false;
			var dto_excepcion = new UTL_TRA_EXCEPCION
			{
				STR_CLASE = xClase,
				STR_EVENTO = xProceso,
				STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
				STR_SERVIDOR = System.Net.Dns.GetHostName(),
				STR_PARAMETROS = JsonConvert.SerializeObject(cuentaBancaria),
				FEC_CREACION = DateTime.Now
			};
			try
			{
				cuentaBancaria.UsrModifica = Session["agente"].ToString();
				cuentaBancaria.Operaciones = "I";
				if (cuentaBancaria.IdBanco != 10 && cuentaBancaria.IdBanco != 21 && cuentaBancaria.IdBanco != 17 && cuentaBancaria.IdBanco != 20)
				{
					
					calcula = managerCuenta.CalcularCuentaSinpe(cuentaBancaria);
					cuentaBancaria.CuentaSinpe = calcula.FirstOrDefault().CuentaSinpe;
					valida = managerCuenta.ValidarCuentaSinpe(cuentaBancaria);

				}
				else
				{
					if (cuentaBancaria.CuentaSinpe == "" || cuentaBancaria.CuentaSinpe.Length != 17)
					{
						cuentaBancaria.Respuesta = "Error, el tamaño del número Sinpe es diferente a 17 dígitos";
						cuentaBancaria.result = false;
						return Json(cuentaBancaria);
					}
					valida = managerCuenta.ValidarCuentaSinpe(cuentaBancaria);
				}

				if (valida)
				{
					//Guarda cambios en las cuentas					
					RespCuentaBanc = managerCuenta.InsertaActualizaEliminaCuentaBancaria(cuentaBancaria);
					RespCuentaBanc.FirstOrDefault().result = valida;
				}
				else
				{
					var CuentBanc = new CuentaBancaria();				
					RespCuentaBanc.Add(CuentBanc);
					RespCuentaBanc.FirstOrDefault().Id = 0;
					RespCuentaBanc.FirstOrDefault().Respuesta = "SINPE INCORRECTA!!!";
					RespCuentaBanc.FirstOrDefault().result = valida;
				}
				return Json(RespCuentaBanc.FirstOrDefault());

				//	return Ok();
			}
			catch (ArgumentException)
			{
				RespCuentaBanc[0].Respuesta = "Ocurrio un Error";
			}
			return Json(RespCuentaBanc);
		}
		#endregion CreaCuentasBancarias

		#region ActualizaCuentasBancarias
		public ActionResult ActualizaCuentasBancarias(CuentaBancaria cuentaBancaria)
		{
			ManagerCuentaBancaria managerCuenta = new ManagerCuentaBancaria();
			string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
			string xProceso = MethodBase.GetCurrentMethod().Name;
			string strHostName = System.Net.Dns.GetHostName();
			//IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
			IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			var RespCuentaBanc = new List<CuentaBancaria>();

			var dto_excepcion = new UTL_TRA_EXCEPCION
			{
				STR_CLASE = xClase,
				STR_EVENTO = xProceso,
				STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
				STR_SERVIDOR = System.Net.Dns.GetHostName(),
				STR_PARAMETROS = JsonConvert.SerializeObject(cuentaBancaria),
				FEC_CREACION = DateTime.Now
			};
			var calcula = new List<CuentaBancaria>();
			var valida = false;
			try
			{
				cuentaBancaria.UsrModifica = Session["agente"].ToString();
				cuentaBancaria.Operaciones = "U";
				if (cuentaBancaria.IdBanco != 10 && cuentaBancaria.IdBanco != 21 && cuentaBancaria.IdBanco != 17 && cuentaBancaria.IdBanco != 20)
				{					
					calcula = managerCuenta.CalcularCuentaSinpe(cuentaBancaria);
					cuentaBancaria.CuentaSinpe = calcula.FirstOrDefault().CuentaSinpe;
					valida = managerCuenta.ValidarCuentaSinpe(cuentaBancaria);
									
				}
				else
				{
					if (cuentaBancaria.CuentaSinpe == "" || cuentaBancaria.CuentaSinpe.Length != 17)
					{
						cuentaBancaria.Respuesta = "Error, el tamaño del número Sinpe es diferente a 17 dígitos";
						cuentaBancaria.result = false;
						return Json(cuentaBancaria);
					}
					valida = managerCuenta.ValidarCuentaSinpe(cuentaBancaria);
				}
				
				if (valida)
				{
					//Guarda cambios en las cuentas					
					RespCuentaBanc = managerCuenta.InsertaActualizaEliminaCuentaBancaria(cuentaBancaria);
					RespCuentaBanc.FirstOrDefault().result = valida;
				}
				else
				{
					var CuentBanc = new CuentaBancaria();
					RespCuentaBanc.Add(CuentBanc);
					RespCuentaBanc.FirstOrDefault().Id = 0;
					RespCuentaBanc.FirstOrDefault().Respuesta = "SINPE INCORRECTA!!!";
					RespCuentaBanc.FirstOrDefault().result = valida;
				}
				return Json(RespCuentaBanc);

				//	return Ok();
			}
			catch (ArgumentException)
			{
				RespCuentaBanc[0].Respuesta = "Ocurrio un Error";
			}
			return Json(RespCuentaBanc);
		}
		#endregion ActualizaCuentasBancarias

		#region EiminaCuentasBancarias
		[HttpPost]
		public ActionResult EiminaCuentasBancarias(int ID,string Identificacion)
		{
			ManagerCuentaBancaria managerCuenta = new ManagerCuentaBancaria();
			string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
			string xProceso = MethodBase.GetCurrentMethod().Name;
			string strHostName = System.Net.Dns.GetHostName();
			//IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
			IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			var RespCuentaBanc = new List<CuentaBancaria>();
			var cuentaBancaria = new CuentaBancaria
			{
				Id = ID,
				Identificacion = Identificacion
			};
			var dto_excepcion = new UTL_TRA_EXCEPCION
			{
				STR_CLASE = xClase,
				STR_EVENTO = xProceso,
				STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
				STR_SERVIDOR = System.Net.Dns.GetHostName(),
				STR_PARAMETROS = JsonConvert.SerializeObject(cuentaBancaria),
				FEC_CREACION = DateTime.Now
			};
			try
			{
				cuentaBancaria.UsrModifica = Session["agente"].ToString();
				cuentaBancaria.Operaciones = "D";
				RespCuentaBanc = managerCuenta.InsertaActualizaEliminaCuentaBancaria(cuentaBancaria);

				return Json(RespCuentaBanc.FirstOrDefault());

				//	return Ok();
			}
			catch (ArgumentException)
			{
				cuentaBancaria.Respuesta = "Ocurrio un Error";
			}
			return Json(RespCuentaBanc);
		}
		#endregion EiminaCuentasBancarias

		#region Carga combos para crear nueva Cuenta
		public ActionResult CargaCombobox(int Id)
		{
			ManagerCuentaBancaria managerCuenta = new ManagerCuentaBancaria();
			string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
			string xProceso = MethodBase.GetCurrentMethod().Name;
			string strHostName = System.Net.Dns.GetHostName();
			//IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
			IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			var RespCuentaBanc = new Bancos();

		

			var dto_excepcion = new UTL_TRA_EXCEPCION
			{
				STR_CLASE = xClase,
				STR_EVENTO = xProceso,
				STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
				STR_SERVIDOR = System.Net.Dns.GetHostName(),
				STR_PARAMETROS = JsonConvert.SerializeObject(RespCuentaBanc),
				FEC_CREACION = DateTime.Now
			};
			try
			{
				    RespCuentaBanc.ListBancos = new List<Bancos>();
				    RespCuentaBanc.ListTipoMoneda = new List<TipoMoneda>();
				    RespCuentaBanc.ListTipoCuentas = new List<TipoCuentas>();
				    RespCuentaBanc.ListBancos = managerCuenta.Trae_Bancos();
					RespCuentaBanc.ListTipoMoneda = managerCuenta.TraeTipoMoneda();
					RespCuentaBanc.ListTipoCuentas = managerCuenta.TraeTipoCuenta();
			
				return Json(RespCuentaBanc, JsonRequestBehavior.AllowGet);

				//	return Ok();
			}
			catch (ArgumentException)
			{
				RespCuentaBanc.Respuesta = "Ocurrio un Error";
			}
			return Json(RespCuentaBanc);
		}
		#endregion Carga combos para crear nueva Cuenta
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
    }
}

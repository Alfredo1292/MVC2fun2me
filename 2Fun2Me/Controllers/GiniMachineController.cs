using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Http;
using TwoFunTwoMe.Models.DTO;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_DataAccess;

namespace TwoFunTwoMe.Controllers
{
    public class GiniMachineController : ApiController
    {
		private ManagerGiniMachine manager;

		private InfoClass infDto = new InfoClass
		{
			STR_COD_PAIS = "506"
		};

		#region Procesar

		[HttpPost]
		[Route("api/GiniMachine/Procesar")]
		public IHttpActionResult Procesar(DTO_GiniMachine objGiniMachine)
		{
			manager = new ManagerGiniMachine();
			string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
			string xProceso = MethodBase.GetCurrentMethod().Name;
			string strHostName = System.Net.Dns.GetHostName();
			IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
			IPAddress ipAddress = ipHostInfo.AddressList[0];
			objGiniMachine.UsrCreacion = "WEB";
			var dto_excepcion = new UTL_TRA_EXCEPCION
			{
				STR_CLASE = xClase,
				STR_EVENTO = xProceso,
				STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
				STR_SERVIDOR = ipAddress.ToString(),
				STR_PARAMETROS = JsonConvert.SerializeObject(objGiniMachine),
                FEC_CREACION = DateTime.Now
            };

			var dto_ObjConfig = new DTO_GiniMachineConfig
			{
				Buro = "GINIMACHINE"
			};

			try
			{
				#region CONFIGURACION GINIMACHINE
				dto_ObjConfig = manager.ObtenerConfigGiniMachine(dto_ObjConfig).FirstOrDefault();
				#endregion

				#region OBTENER JSON INPUT
				objGiniMachine.Input = manager.ObtenerJSON( objGiniMachine).FirstOrDefault().Json;
				#endregion

				#region CONSUMIR REST GINIMACHINE
				RestClient cliente = new RestClient(dto_ObjConfig._EndPoint + dto_ObjConfig._Resource); // Dirección web del reporte
				RestRequest request = new RestRequest(); // Clase propia del RestSharp para asignar parámetros de envio.
				request.Method = Method.POST;
				request.AddHeader("Accept", "application/json");
				request.AddParameter(dto_ObjConfig._ParamName1, dto_ObjConfig._ParamVal1,ParameterType.QueryString);
				request.AddParameter("application/json", objGiniMachine.Input, ParameterType.RequestBody);

				//request.AddJsonBody(objGiniMachine.Input);

				var respuesta = cliente.Execute(request); // Metodo que ejecuta la solicitud.
				if (respuesta.StatusCode == System.Net.HttpStatusCode.OK) // Si retorna OK, el reporte fue generado.
				{
					objGiniMachine.Output = respuesta.Content.ToString();
					if (!string.IsNullOrEmpty(objGiniMachine.Output.ToString()))
					{
						if (objGiniMachine.Output.Contains("Approve") || objGiniMachine.Output.Contains("Decl"))
						{
							#region ALMACENAR OUTPUT BD
							objGiniMachine.ModelId = dto_ObjConfig._ParamVal1;
							objGiniMachine.Servidor = dto_ObjConfig._EndPoint;
							manager.GuardarResultadoGinimachine(objGiniMachine);
							objGiniMachine.Input = "";
							#endregion
							objGiniMachine.Mensaje = "SUCCESS";
						}
					}
				}
				else
				{
					String mensajeError = respuesta.Content;
				}
				#endregion

			}
			catch (Exception ex)
			{
				dto_excepcion.STR_MENSAJE = ex.Message;
				DynamicSqlDAO.guardaExcepcion(dto_excepcion, ConfigurationManager.ConnectionStrings["TwoFunTwoMeConnection"].ConnectionString);
				objGiniMachine.Status = "ERR";
				objGiniMachine.Mensaje = ex.Message;
			}

			return Json(objGiniMachine);
		}

		#endregion


	}
}

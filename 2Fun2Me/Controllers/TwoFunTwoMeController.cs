using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using TwoFunTwoMe.Models.DTO;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_DataAccess;

namespace TwoFunTwoMe.Controllers
{
    public class TwoFunTwoMeController : ApiController
    {
        private Manager manager;

        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };

        #region GuardarPersona

        [HttpPost]
        [Route("api/TwoFunTwoMe/GuardarPersonaWeb")]
        public IHttpActionResult GuardarPersonaWeb(InsertPersonasWeb personasWeb)
        {
            manager = new Manager();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            personasWeb.UsrModifica = "WEB";
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(personasWeb),
                FEC_CREACION = DateTime.Now
            };

            var dto_paso_1 = new Pasos_credito
            {
                IdTipoIdentificacion = personasWeb.IdTipoIdentificacion,
                Identificacion = personasWeb.Identificacion,
                Correo = personasWeb.Correo,
                Telefono_celuar = personasWeb.TelefonoCel,
                IdProducto = personasWeb.IdProducto,
            };
            var dto_personasWeb = new InsertPersonasWeb();
            try
            {
                dto_personasWeb = manager.GuardarPersona(personasWeb);
                manager.Inserta_Paso_1(dto_paso_1);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_personasWeb.Status = "ERR";
            }

            return Json(dto_personasWeb);
        }

        #endregion

        #region MetodoGuardaFotoCedula
        [HttpPost]
        [Route("api/TwoFunTwoMe/GuardaFotoCedula")]
        public async Task<IHttpActionResult> GuardaFotoCedula(string IdTipoIdentificacion, string Identificacion)
        {
            Manager manager = new Manager();
            Utilitario Util = new Utilitario();
            List<ConfigSys> CONF = new List<ConfigSys>();
            List<Solicitudes> sol = new List<Solicitudes>();
            AWSAccess wSAccess = new AWSAccess();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(Identificacion),
                FEC_CREACION = DateTime.Now
            };

            var dto_paso_5 = new Pasos_credito
            {
                IdTipoIdentificacion = IdTipoIdentificacion,
                Identificacion = Identificacion
            };

            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdTipoIdentificacion = IdTipoIdentificacion;
            solicitudes.Identificacion = Identificacion;
            solicitudes.TipoFoto = 1;
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


                if (!Request.Content.IsMimeMultipartContent())

                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);

                //foreach (var file in provider.Contents)
                //{
                var file = provider.Contents;
                //.Select(x => x.Headers.ContentDisposition.FileName.Trim('\"'))
                var filename = file.Select(x => x.Headers.ContentDisposition.FileName.Trim('\"')).FirstOrDefault();
                var buffer = await file.FirstOrDefault().ReadAsByteArrayAsync();
                string ext = Path.GetExtension(filename);
                solicitudes.UrlFoto = URL + Identificacion + "/" + Identificacion;
                if (Util.ValidarFichero(solicitudes.UrlFoto) && Util.ValidarExtension(filename))
                {
					//solicitudes.UrlFoto = Util.Renamefile(solicitudes.UrlFoto, Identificacion, ext);
					solicitudes.UrlFoto = solicitudes.UrlFoto + ext;
					CONF = manager.ConsultarAccessKeyAWS();
                    //inicio ivan cortes para tito
                    using (Image image = Image.FromStream(new MemoryStream(buffer)))
                    {
                        //ivan cortes para tito
                        // var nameImage = string.Concat(Identificacion, "_Foto_Cedula_", Guid.NewGuid().ToString(), ".jpg");
                        // var ret = image.ResizeProportional(450, 280);
                        image.Save(solicitudes.UrlFoto);
                    }

                    var soliresp = wSAccess.DetectFace(CONF, solicitudes.UrlFoto, Identificacion, IdTipoIdentificacion, 1);
                    solicitudes.result = soliresp.result;
                    solicitudes.DetectedText = soliresp.DetectedText;
                    dto_paso_5.Foto_identificacion = solicitudes.UrlFoto;
                    //fin ivan cortes;
                    //File.WriteAllBytes(solicitudes.UrlFoto, buffer);
                }
                else
                {
                    return Ok("ERR");
                }
                //}
                manager.Inserta_Paso_5(dto_paso_5);
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return Ok(dto_listLogin);

                //	return Ok();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                return Ok(solicitudes.Mensaje);
            }

        }
        #endregion MetodoGuardaFotoCedula

        #region MetodoGuardaFotoCedulaTrasera
        [HttpPost]
        [Route("api/TwoFunTwoMe/GuardaFotoCedulatrasera")]
        public async Task<IHttpActionResult> GuardaFotoCedulatrasera(string IdTipoIdentificacion, string Identificacion)
        {
            Manager manager = new Manager();
            Utilitario Util = new Utilitario();
            AWSAccess wSAccess = new AWSAccess();
            List<ConfigSys> CONF = new List<ConfigSys>();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(Identificacion),
                FEC_CREACION=DateTime.Now
            };
            var dto_paso_6 = new Pasos_credito
            {
                IdTipoIdentificacion = IdTipoIdentificacion,
                Identificacion = Identificacion
            };

            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdTipoIdentificacion = IdTipoIdentificacion;
            solicitudes.Identificacion = Identificacion;
            solicitudes.TipoFoto = 2;
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

                CONF = manager.ConsultarAccessKeyAWS();

                if (!Request.Content.IsMimeMultipartContent())

                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var file in provider.Contents)
                //{
                var file = provider.Contents;
                //.Select(x => x.Headers.ContentDisposition.FileName.Trim('\"'))
                var filename = file.Select(x => x.Headers.ContentDisposition.FileName.Trim('\"')).FirstOrDefault();

                var buffer = await file.FirstOrDefault().ReadAsByteArrayAsync();
                string ext = Path.GetExtension(filename);
                //Do whatever you want with filename and its binary data.
                solicitudes.UrlFoto = URL + Identificacion + "/" + Identificacion;
                if (Util.ValidarFichero(solicitudes.UrlFoto) && Util.ValidarExtension(filename))
                {
					solicitudes.UrlFoto = solicitudes.UrlFoto + ext;
					//solicitudes.UrlFoto = Util.Renamefile(solicitudes.UrlFoto, Identificacion, ext);
					//File.WriteAllBytes(solicitudes.UrlFoto, buffer);

					//inicio ivan cortes para tito
					using (Image image = Image.FromStream(new MemoryStream(buffer)))
                    {
                        //ivan cortes para tito
                        // var nameImage = string.Concat(Identificacion, "_Foto_Cedula_", Guid.NewGuid().ToString(), ".jpg");
                        //var ret = image.ResizeProportional(450, 280);
                        image.Save(solicitudes.UrlFoto);
                    }

                    var soliresp = wSAccess.DetectFace(CONF, solicitudes.UrlFoto, Identificacion, IdTipoIdentificacion, 2);
                    solicitudes.result = soliresp.result;
                    solicitudes.DetectedText = soliresp.DetectedText;
                    dto_paso_6.Foto_identificacion_Trasera = solicitudes.UrlFoto;
                }
                else
                {
                    return Ok("ERR");
                }
                //}
                manager.Inserta_Paso_6(dto_paso_6);
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                return Ok(dto_listLogin);

                //	return Ok();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR" + dto_excepcion.STR_MENSAJE;
                return Ok(solicitudes.Mensaje);
            }

        }
        #endregion MetodoGuardaFotoCedulaTrasera

        #region MetodoGuardaFotoSelfie
        [HttpPost]
        [Route("api/TwoFunTwoMe/GuardaFotoSelfie")]
        public async Task<IHttpActionResult> GuardaFotoSelfie(string IdTipoIdentificacion, string Identificacion)
        {
            Manager manager = new Manager();
            Utilitario Util = new Utilitario();
			AWSAccess wSAccess = new AWSAccess();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(Identificacion),
                FEC_CREACION = DateTime.Now
            };
            var dto_paso_7 = new Pasos_credito
            {
                IdTipoIdentificacion = IdTipoIdentificacion,
                Identificacion = Identificacion
            };
            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdTipoIdentificacion = IdTipoIdentificacion;
            solicitudes.Identificacion = Identificacion;
            solicitudes.TipoFoto = 3;

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
				var CONF = manager.ConsultarAccessKeyAWS();

				if (!Request.Content.IsMimeMultipartContent())

                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var file in provider.Contents)
                //{
                //    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                //    var buffer = await file.ReadAsByteArrayAsync();
                var file = provider.Contents;
                //.Select(x => x.Headers.ContentDisposition.FileName.Trim('\"'))
                var filename = file.Select(x => x.Headers.ContentDisposition.FileName.Trim('\"')).FirstOrDefault();

                var buffer = await file.FirstOrDefault().ReadAsByteArrayAsync();
                string ext = Path.GetExtension(filename);
                //Do whatever you want with filename and its binary data.
                solicitudes.UrlFoto = URL + Identificacion + "/" + Identificacion;
                if (Util.ValidarFichero(solicitudes.UrlFoto) && Util.ValidarExtension(filename))
                {
                    //solicitudes.UrlFoto = Util.Renamefile(solicitudes.UrlFoto, Identificacion, ext);
					solicitudes.UrlFoto = solicitudes.UrlFoto + ext;
					//File.WriteAllBytes(solicitudes.UrlFoto, buffer);
					//solicitudes.UrlFoto = Util.Renamefile(solicitudes.UrlFoto, Identificacion, ext);
					//inicio ivan cortes para tito
					using (Image image = Image.FromStream(new MemoryStream(buffer)))
                    {
                        //ivan cortes para tito
                        // var nameImage = string.Concat(Identificacion, "_Foto_Cedula_", Guid.NewGuid().ToString(), ".jpg");
                        //var ret = image.ResizeProportional(450, 280);
                        image.Save(solicitudes.UrlFoto);
                    }
                    dto_paso_7.Selfie_identificacion = solicitudes.UrlFoto;
					var soliresp = wSAccess.DetectFace(CONF, solicitudes.UrlFoto, Identificacion, IdTipoIdentificacion, 1);
					solicitudes.result = soliresp.result;
				}
                else
                {
                    return Ok("ERR");
                }
                //}
                solicitudes.result = false;
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);
                manager.Inserta_Paso_7(dto_paso_7);
                return Ok(dto_listLogin.FirstOrDefault().Mensaje);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                return Ok(solicitudes.Mensaje);
            }
        }
        #endregion MetodoGuardaFotoSelfie

        #region MetodoGuardaFotoFirma
        [HttpPost]
        [Route("api/TwoFunTwoMe/GuardaFotoFirma")]
        public async Task<IHttpActionResult> GuardaFotoFirma(string IdTipoIdentificacion, string Identificacion)
        {
            Solicitudes solicitudes = new Solicitudes();
            Utilitario Util = new Utilitario();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(Identificacion),
                FEC_CREACION = DateTime.Now
            };

            var dto_paso_10 = new Pasos_credito
            {
                IdTipoIdentificacion = IdTipoIdentificacion,
                Identificacion = Identificacion
            };
            Manager manager = new Manager();
            solicitudes.IdTipoIdentificacion = IdTipoIdentificacion;
            solicitudes.Identificacion = Identificacion;
            solicitudes.TipoFoto = 4;

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


                if (!Request.Content.IsMimeMultipartContent())

                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                //foreach (var file in provider.Contents)
                //{
                //    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                //    var buffer = await file.ReadAsByteArrayAsync();
                var file = provider.Contents;
                //.Select(x => x.Headers.ContentDisposition.FileName.Trim('\"'))
                var filename = file.Select(x => x.Headers.ContentDisposition.FileName.Trim('\"')).FirstOrDefault();

                var buffer = await file.FirstOrDefault().ReadAsByteArrayAsync();
                string ext = Path.GetExtension(filename);
                //Do whatever you want with filename and its binary data.
                solicitudes.UrlFoto = URL + Identificacion + "/" + Identificacion;
                if (Util.ValidarFichero(solicitudes.UrlFoto) && Util.ValidarExtension(filename))
                {
					solicitudes.UrlFoto = solicitudes.UrlFoto + ext;
					//solicitudes.UrlFoto = Util.Renamefile(solicitudes.UrlFoto, Identificacion, ext);
					//File.WriteAllBytes(solicitudes.UrlFoto, buffer);
					//solicitudes.UrlFoto = Util.Renamefile(solicitudes.UrlFoto, Identificacion, ext);
					//inicio ivan cortes para tito
					using (Image image = Image.FromStream(new MemoryStream(buffer)))
                    {
                        //ivan cortes para tito
                        // var nameImage = string.Concat(Identificacion, "_Foto_Cedula_", Guid.NewGuid().ToString(), ".jpg");
                        var ret = image.ResizeProportional(800, 400);
                        ret.Save(solicitudes.UrlFoto);
                    }
                    dto_paso_10.Firma = solicitudes.UrlFoto;
                }
                else
                {
                    return Ok("ERR");
                }
                //}
                solicitudes.result = false;
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);
                manager.Inserta_Paso_10(dto_paso_10);
                return Ok(dto_listLogin.FirstOrDefault().Mensaje);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                return Ok(solicitudes.Mensaje);
            }
        }
        #endregion MetodoGuardaFotoFirma

        #region MetodoGuardaCuentaBancaria
        [HttpPost]
        [Route("api/TwoFunTwoMe/GuardaCuentaBancaria")]
        public async Task<IHttpActionResult> GuardaCuentaBancaria(CuentaBancaria cuentabancaria)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(cuentabancaria),
                FEC_CREACION = DateTime.Now
            };
            cuentabancaria.Predeterminado = true;
            cuentabancaria.Operaciones = 'I';
            var dto_paso_8 = new Pasos_credito
            {
                IdTipoIdentificacion = cuentabancaria.IdTipoIdentificacion,
                Identificacion = cuentabancaria.Identificacion,
                Cuenta_cliente = cuentabancaria.Cuenta,
                Banco = cuentabancaria.IdBanco
            };
            var dto_listLogin = new List<CuentaBancaria>();
            try
            {

                //var dto_listLogin = manager.usp_InsertaCuentaBancaria(cuentabancaria);
                CuentaBancaria cuentasin = new CuentaBancaria();
                cuentasin.IdBanco = cuentabancaria.IdBanco;
                cuentasin.Cuenta = cuentabancaria.Cuenta;
                cuentasin.TipoCuenta = cuentabancaria.TipoCuenta;
                cuentasin.TipoMoneda = cuentabancaria.TipoMoneda;
                var calcula = new List<CuentaBancaria>();
                var valida = false;
                if (cuentasin.IdBanco != 10 && cuentasin.IdBanco != 21 && cuentasin.IdBanco != 17 && cuentasin.IdBanco != 20)
                {
                    calcula = manager.CalcularCuentaSinpe(cuentasin);
                    valida = manager.ValidarCuentaSinpe(calcula.FirstOrDefault());
                    cuentabancaria.CuentaSinpe = calcula.FirstOrDefault().CuentaSinpe;
                }
                else
                {
                    if (cuentabancaria.CuentaSinpe == "" || cuentabancaria.CuentaSinpe.Length != 17)
                    {
                        cuentabancaria.Mensaje = "Error, el tamaño del número Sinpe es diferente a 17 dígitos";
                        cuentabancaria.result = false;
                        return Json(cuentabancaria);
                    }
                    cuentasin.CuentaSinpe = cuentabancaria.CuentaSinpe;
                    valida = manager.ValidarCuentaSinpe(cuentasin);
                }

                if (valida)
                {
                    //Guarda cambios en las cuentas					
                    dto_listLogin = manager.usp_InsertaCuentaBancaria(cuentabancaria);
                    dto_listLogin.FirstOrDefault().result = valida;
                }
                else
                {
                    dto_listLogin.FirstOrDefault().Mensaje = "SINPE INCORRECTA!!!";
                    dto_listLogin.FirstOrDefault().result = valida;
                }


                manager.Inserta_Paso_8(dto_paso_8);
                return Json(dto_listLogin);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                cuentabancaria.Mensaje = "ERR";
                return Json(cuentabancaria);
            }
        }
        #endregion MetodoGuardaFotoFirma

        #region MetodoValidaPIN

        [HttpPost]
        [Route("api/TwoFunTwoMe/ValidarPIN")]
        public async Task<IHttpActionResult> ValidarPIN(Solicitudes solicitudes)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };

            var dto_paso_2 = new Pasos_credito
            {
                IdTipoIdentificacion = solicitudes.IdTipoIdentificacion,
                Identificacion = solicitudes.Identificacion
            };
            try
            {
                var dto_listLogin = manager.usp_ValidarPIN(solicitudes);
                dto_paso_2.Pin = dto_listLogin.FirstOrDefault().PIN.Value.ToString();
                manager.Inserta_Paso_2(dto_paso_2);
                return Json(dto_listLogin);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                return Json(solicitudes);
            }

        }
        #endregion MetodoValidaPIN

        #region MetodoNuevoPIN

        [HttpPost]
        [Route("api/TwoFunTwoMe/GeneraNuevoPIN")]
        public async Task<IHttpActionResult> GeneraNuevoPIN(Solicitudes solicitudes)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };
            try
            {
                var dto_listLogin = manager.usp_NuevoPIN(solicitudes);
                return Json(dto_listLogin);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                return Json(solicitudes);
            }
        }
        #endregion MetodoNuevoPIN

        #region MetodoCompararImagenes

        [HttpPost]
        [Route("api/TwoFunTwoMe/CompararImagenes")]
        public async Task<IHttpActionResult> CompararImagenes(Solicitudes solicitudes)
        {
            List<Solicitudes> sol = new List<Solicitudes>();
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                FEC_CREACION = DateTime.Now
            };

            AWSAccess wSAccess = new AWSAccess();
            List<ConfigSys> CONF = new List<ConfigSys>();

            try
            {
                CONF = manager.ConsultarAccessKeyAWS();
                sol = manager.ConsultarSolicitudWebApi(solicitudes);

                var dto_listLogin = wSAccess.GetTestAsync(CONF, sol);

                var tempdtologin = manager.usp_GuardaCompareFaceInfo(dto_listLogin);
                dto_listLogin.Mensaje = tempdtologin.FirstOrDefault().Mensaje;
                return Json(dto_listLogin);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR-" + ex.Message;
                return Json(solicitudes);
            }
        }
        #endregion MetodoCompararImagenes

        #region GuardaReferenciaLaboral
        [HttpPost]
        [Route("api/TwoFunTwoMe/GuardaReferenciaLaboral")]
        public async Task<IHttpActionResult> GuardaReferenciaLaboral(ReferenciaLaboral referenciaLaboral)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(referenciaLaboral),
                FEC_CREACION = DateTime.Now
            };

            var dto_paso_12 = new Pasos_credito
            {
                IdTipoIdentificacion = referenciaLaboral.IdTipoIdentificacion,
                Identificacion = referenciaLaboral.Identificacion,
                Empresa = referenciaLaboral.Empresa
            };

            var dto_listLogin = new List<ReferenciaLaboral>();

            try
            {
                dto_listLogin = manager.GuardaReferenciaLaboral(referenciaLaboral);


                manager.Inserta_Paso_12(dto_paso_12);
                return Json(dto_listLogin);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                referenciaLaboral.Mensaje = "ERR";
                return Json(referenciaLaboral);
            }
        }
        #endregion GuardaReferenciaLaboral

        #region Pasos	
        [HttpPost]
        [Route("api/TwoFunTwoMe/PasoIdentificacion")]
        public async Task<IHttpActionResult> PasoIdentificacion(Pasos_credito pasos)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                manager.GuardaIdentificacion(pasos);
                return Json("OK");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return Json("ERR");
            }

        }

        [HttpPost]
        [Route("api/TwoFunTwoMe/Paso0")]
        public async Task<IHttpActionResult> Paso0(Pasos_credito pasos)
        {
            Manager manager = new Manager();

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = strHostName,
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                manager.Inserta_Paso0(pasos);
                return Json("OK");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return Json("ERR");
            }

        }

        [HttpPost]
        [Route("api/TwoFunTwoMe/Paso3")]
        public async Task<IHttpActionResult> Paso3(Pasos_credito pasos)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                manager.Inserta_Paso_3(pasos);
                return Json("OK");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return Json("ERR");
            }

        }


        [HttpPost]
        [Route("api/TwoFunTwoMe/Paso4")]
        public async Task<IHttpActionResult> Paso4(Pasos_credito pasos)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                manager.Inserta_Paso_4(pasos);
                return Json("OK");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return Json("ERR");
            }
        }

        [HttpPost]
        [Route("api/TwoFunTwoMe/Paso5")]
        public async Task<IHttpActionResult> Paso5(Pasos_credito pasos)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                manager.Inserta_Paso_5(pasos);
                return Json("OK");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return Json("ERR");
            }

        }

        [HttpPost]
        [Route("api/TwoFunTwoMe/Paso10")]
        public async Task<IHttpActionResult> Paso10(Pasos_credito pasos)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                manager.Inserta_Paso_10(pasos);
                return Json("OK");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return Json("ERR");
            }

        }

        [HttpPost]
        [Route("api/TwoFunTwoMe/PinVerificado")]
        public async Task<IHttpActionResult> PinVerificado(Pasos_credito pasos)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                pasos.PinVerificado = true;
                manager.Inserta_Paso_14(pasos);
                return Json("OK");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return Json("ERR");
            }

        }

        [HttpPost]
        [Route("api/TwoFunTwoMe/PinFallido")]
        public async Task<IHttpActionResult> PinFallido(Pasos_credito pasos)
        {
            Manager manager = new Manager();

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                manager.Inserta_Paso_15(pasos);
                return Json("OK");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return Json("ERR");
            }

        }

        #endregion

        #region ValidaCuenta
        [HttpPost]
        [Route("api/TwoFunTwoMe/CuentaExistente")]
        public IHttpActionResult ValidaCuentaExistente(InsertPersonasWeb personasWeb)
        {
            manager = new Manager();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            personasWeb.UsrModifica = "WEB";
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(personasWeb),
                FEC_CREACION = DateTime.Now
            };

            var dto_paso_1 = new Pasos_credito
            {
                IdTipoIdentificacion = personasWeb.IdTipoIdentificacion,
                Identificacion = personasWeb.Identificacion,
                Correo = personasWeb.Correo,
                Telefono_celuar = personasWeb.TelefonoCel
            };
            var dto_personasWeb = new InsertPersonasWeb();
            try
            {
                manager.Inserta_Paso_1(dto_paso_1);
                dto_personasWeb = manager.GuardarPersona(personasWeb);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_personasWeb.Status = "ERR";
            }

            return Json(dto_personasWeb);
        }
        #endregion

        #region ConsultaDatosContratoPagare
        [HttpPost]
        [Route("api/TwoFunTwoMe/ConsultaDatosContratoPagare")]
        public IHttpActionResult ConsultaDatosContratoPagare(PagareContrato pagareContrato)
        {
            manager = new Manager();
            string path = "";
            ConvertidorHtmlToPdf convertidorHtmlToPdf = new ConvertidorHtmlToPdf();
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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pagareContrato),
                FEC_CREACION = DateTime.Now
            };

            var dto_paso_9 = new Pasos_credito
            {
                IdTipoIdentificacion = pagareContrato.IdTipoIdentificacion,
                Identificacion = pagareContrato.Identificacion,
            };
            try
            {
                manager.Inserta_Paso_9(dto_paso_9);
                pagareContrato = convertidorHtmlToPdf.TraeDatosContratoPagare(pagareContrato);
                //byte[] bytes = File.ReadAllBytes(path);
                //string file = Convert.ToBase64String(bytes);
                //pagareContrato.file = file;
                //pagareContrato.Pagare = "";
                //pagareContrato.Contrato = "";
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                pagareContrato.Mensaje = "ERR";
            }

            return Json(pagareContrato);
        }
        #endregion ConsultaDatosContratoPagare

        #region CreaContratoPagare
        [HttpPost]
        [Route("api/TwoFunTwoMe/CreaContratoPagare")]
        public async Task<IHttpActionResult> CreaContratoPagare(PagareContrato pagareContrato)
        {
            manager = new Manager();
            string path = "";

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
                STR_SERVIDOR = ipAddress.ToString(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pagareContrato),
                FEC_CREACION = DateTime.Now
            };

            var dto_paso_11 = new Pasos_credito
            {
                IdTipoIdentificacion = pagareContrato.IdTipoIdentificacion,
                Identificacion = pagareContrato.Identificacion,
            };

            Solicitudes solicitudes = new Solicitudes();
            solicitudes.IdTipoIdentificacion = pagareContrato.IdTipoIdentificacion;
            solicitudes.Identificacion = pagareContrato.Identificacion;
            solicitudes.TipoFoto = 5;
            try
            {
                ConvertidorHtmlToPdf convertidorHtmlToPdf = new ConvertidorHtmlToPdf();

                solicitudes.UrlFoto = convertidorHtmlToPdf.CrearPdf(pagareContrato);
                manager.Inserta_Paso_11(dto_paso_11);
                var dto_listLogin = manager.MetodoGuardaFoto(solicitudes);

                //	solicitudes.Mensaje = dto_listLogin.FirstOrDefault().Mensaje;

                //return dto_listLogin.FirstOrDefault().Mensaje;
                return Json(dto_listLogin);

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                pagareContrato.Mensaje = "ERR";

            }

            return Json(pagareContrato);
        }
		#endregion CreaContratoPagare

		#region ConsultaPadron

		[HttpPost]
		[Route("api/TwoFunTwoMe/ConsultaPadron")]
		public IHttpActionResult ConsultaPadron(Padron padron)
		{
			manager = new Manager();
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
				STR_SERVIDOR = ipAddress.ToString(),
				STR_PARAMETROS = JsonConvert.SerializeObject(padron),
				FEC_CREACION = DateTime.Now
			};

		
			var dto_personasWeb = new List<Padron>();
			try
			{
				dto_personasWeb = manager.ConsultaPadron(padron);
				var Nombre = dto_personasWeb.FirstOrDefault().nombre.Split(' ');
				dto_personasWeb.FirstOrDefault().nombre = Nombre[0];
				dto_personasWeb.FirstOrDefault().segundoNombre = "";
				if (Nombre.Count() >= 2)
				{

						dto_personasWeb.FirstOrDefault().segundoNombre = Nombre[1];
				}
				if (Nombre.Count() >= 3)
				{
	
						dto_personasWeb.FirstOrDefault().segundoNombre = dto_personasWeb.FirstOrDefault().segundoNombre + ' ' + Nombre[2];
			
				}
				if (Nombre.Count() >= 4)
				{

					dto_personasWeb.FirstOrDefault().segundoNombre = dto_personasWeb.FirstOrDefault().segundoNombre + ' ' + Nombre[3];

				}
			}
			catch (Exception ex)
			{
				dto_excepcion.STR_MENSAJE = ex.Message;
				DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
				dto_personasWeb.FirstOrDefault().Mensaje = "ERR";
			}

			return Json(dto_personasWeb);
		}

		#endregion

		#region Insertar_TraceEventosLOOP
		[HttpPost]
		[Route("api/TwoFunTwoMe/Insertar_TraceEventosLOOP")]
		public async Task<IHttpActionResult> Insertar_TraceEventosLOOP(TraceLoop Loop)
		{
			Manager manager = new Manager();

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
				STR_SERVIDOR = ipAddress.ToString(),
				STR_PARAMETROS = JsonConvert.SerializeObject(Loop),
				FEC_CREACION = DateTime.Now
			};

			var dto_listRet = new List<TraceLoop>();

			try
			{
				dto_listRet = manager.Insertar_TraceEventosLOOP(Loop);

				
				return Json(dto_listRet);
			}
			catch (Exception ex)
			{
				dto_excepcion.STR_MENSAJE = ex.Message;
				DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
				Loop.Mensaje = "ERR";
				return Json(Loop);
			}
		}
		#endregion GuardaReferenciaLaboral
	}
}

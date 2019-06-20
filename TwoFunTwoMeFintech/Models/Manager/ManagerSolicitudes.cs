using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_Common;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.ApicarCobros;
using TwoFunTwoMeFintech.Models.DTO.Colocacion;
using TwoFunTwoMeFintech.Models.DTO.InsertaSolicitud;
using TwoFunTwoMeFintech.Models.DTO.ProductosMantenimiento;

namespace TwoFunTwoMe.Models.Manager
{
    /// <summary>
    /// CLASE ESPECIALIZADAD EN SOLICITUDES HEREDA LA MANAGER USER
    /// </summary>
    public class ManagerSolcitudes : ManagerUser
    {
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };

        public ManagerSolcitudes()
        {
            String connBD = string.Empty;


            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }

        #region SOLICTUDES

        /// <summary>
        /// OBTIENE LAS SOLICITUDES DE UNA RESPECTIVA SOLICITUD O CLIENTE
        /// </summary>
        /// <param name="Solicitud"></param>
        /// <returns></returns>
        public List<DTO_SOLICITUD_VENTAS> ConsultaSolicitudPendientes(DTO_SOLICITUD_VENTAS Solicitud)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(Solicitud),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            var dto_result = new List<DTO_SOLICITUD_VENTAS>();
            try
            {
                dto.ParameterList.AddRange(from nodo in Solicitud.GetType().GetProperties()
                                           where nodo.GetValue(Solicitud) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(Solicitud).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_ListarSolicXAsesor";


                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_SOLICITUD_VENTAS>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;

        }



        #endregion

        #region PANTALLA REVISION DE DOCUMENTOS
        /// <summary>
        /// OBTIENE LOS DOCMENTOS DE UN RESPECTIVA SOLICITUD
        /// </summary>
        /// <param name="Solicitud"></param>
        /// <returns></returns>
        public List<DTO_SOLICITUD_VENTAS> ConsultaSolicitudDocumentos(DTO_SOLICITUD_VENTAS Solicitud)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(Solicitud),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            var dto_result = new List<DTO_SOLICITUD_VENTAS>();
            dto.ParameterList = new List<SpParameter>();
            try
            {
                dto.ParameterList.AddRange(from nodo in Solicitud.GetType().GetProperties()
                                           where nodo.GetValue(Solicitud) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(Solicitud).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_Obtener_Url_Imagen_Solicictud";


                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_SOLICITUD_VENTAS>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;
        }

        /// <summary>
        /// OBTIENE EL TRACE DE CREDITO
        /// </summary>
        /// <param name="Solicitud"></param>
        /// <returns></returns>
        public List<Pasos_Credito> ConsultaSolicitudPasos(DTO_SOLICITUD_VENTAS Solicitud)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(Solicitud),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in Solicitud.GetType().GetProperties()
                                       where nodo.GetValue(Solicitud) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(Solicitud).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_Obtener_Trace_Solicictud";
            var dto_result = new List<Pasos_Credito>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Pasos_Credito>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }

            return dto_result;
        }

        /// <summary>
        /// CAMBIA DE ESTADO UNA SOLICITU DESDE REVISION DE DOCUMENTOS
        /// </summary>
        /// <param name="solicitudes"></param>
        /// <returns></returns>
        public List<DTO_SOLICITUD_VENTAS> CambiarEstadoRevisionDocumentos(DTO_SOLICITUD_VENTAS solicitudes)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in solicitudes.GetType().GetProperties()
                                       where nodo.GetValue(solicitudes) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(solicitudes).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_Actualiza_Estado_Revision_Documentos";
            var dto_result = new List<DTO_SOLICITUD_VENTAS>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_SOLICITUD_VENTAS>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
                //dto_result.Mensaje = string.Concat("La solicitud N# ", solicitudes.Solicitud, " se actualizo");
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;
        }
        //Alfredo José Vargas Seinfarth-14/02/2019--//INICIO
        #region ConsultaConfiUrlImagen
        public List<Tab_ConfigSys> ConsultaConfiUrlImagen(Tab_ConfigSys configSys)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in configSys.GetType().GetProperties()
                                       where nodo.GetValue(configSys) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(configSys).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_configuracion_general";
            var dto_result = new List<Tab_ConfigSys>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tab_ConfigSys>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                configSys.Mensaje = "ERR";
                throw;

            }
            return dto_result;
        }

        #endregion ConsultaConfiUrlImagen

        #region Funcion rotar imagen
        public RotateFlipType GetOrientationToFlipType(int orientationValue)
        {
            RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;

            switch (orientationValue)
            {
                case 1:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
                case 2:
                    rotateFlipType = RotateFlipType.RotateNoneFlipX;
                    break;
                case 3:
                    rotateFlipType = RotateFlipType.Rotate180FlipNone;
                    break;
                case 4:
                    rotateFlipType = RotateFlipType.Rotate180FlipX;
                    break;
                case 5:
                    rotateFlipType = RotateFlipType.Rotate90FlipX;
                    break;
                case 6:
                    rotateFlipType = RotateFlipType.Rotate90FlipNone;
                    break;
                case 7:
                    rotateFlipType = RotateFlipType.Rotate270FlipX;
                    break;
                case 8:
                    rotateFlipType = RotateFlipType.Rotate270FlipNone;
                    break;
                default:
                    rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                    break;
            }

            return rotateFlipType;
        }
        #endregion Funcion rotar imagen

        #region Cargar datos credito
        public List<DTO_SOLICITUD_VENTAS> CargarDatosCredito(DTO_SOLICITUD_VENTAS solicitudes)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in solicitudes.GetType().GetProperties()
                                       where nodo.GetValue(solicitudes) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(solicitudes).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_carga_Credito";
            var dto_result = new List<DTO_SOLICITUD_VENTAS>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_SOLICITUD_VENTAS>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }
        #endregion Cargar datos credito

        #region Validar Fichero
        public bool ValidarFichero(string Carpeta)
        {
            //obtenemos sólo la carpeta (quitamos el ejecutable) 
            string carpeta = Path.GetDirectoryName(Carpeta);
            try
            {
                //si no existe la carpeta temporal la creamos 
                if (!(Directory.Exists(carpeta)))
                {
                    Directory.CreateDirectory(carpeta);
                    return true;
                }

                if (Directory.Exists(carpeta))
                {
                    DirectoryInfo directory = new DirectoryInfo(carpeta);

                    //FileInfo[] files = directory.GetFiles("*.*");
                    directory.EnumerateFiles().ToList().ForEach(x => x.Delete());

                    //if (File.Exists())
                    //    File.Delete(file);

                    //Directory.Delete(carpeta);
                    return true;
                }
            }
            catch//(Exception ex)
            {
                return false;
            }
            return true;
        }
        #endregion Validar Fichero

        #region Consulta Contrato Pagare
        public List<PagareContrato> PagareContrato(PagareContrato pagareContrato)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pagareContrato.GetType().GetProperties()
                                       where nodo.GetValue(pagareContrato) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pagareContrato).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_datosContrato";
            var dto_result = new List<PagareContrato>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<PagareContrato>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }
        #endregion Consulta Contrato Pagare

        #region Trae Documento Pagare
        public List<PagareContrato> TraeDocumentoPagare()
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.Result = null;
            dto.SPName = "usp_TraeDocumentoPagare";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<PagareContrato>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        #endregion Trae Documento Pagare

        #region  MetodoGuardaFotoCedula

        public List<DTO_SOLICITUD_VENTAS> MetodoGuardaFoto(DTO_SOLICITUD_VENTAS solicitudes)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now,
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in solicitudes.GetType().GetProperties()
                                       where nodo.GetValue(solicitudes) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(solicitudes).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_MetodoGuardaFotoCedulaSelfieFenix";
            var dto_result = new List<DTO_SOLICITUD_VENTAS>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_SOLICITUD_VENTAS>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                throw;

            }
            return dto_result;
        }

        #endregion MetodoGuardaFotoCedula

        #region TraeDatosContratoPagare
        public PagareContrato TraeDatosContratoPagare(PagareContrato pagareContrato)
        {
            Guid x = Guid.NewGuid();
            DTO_SOLICITUD_VENTAS solicitudes = new DTO_SOLICITUD_VENTAS();
            var pagareCon = new List<PagareContrato>();
            var datosContrato = new List<PagareContrato>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pagareContrato),
                FEC_CREACION = DateTime.Now
            };

            try
            {

                solicitudes.Identificacion = pagareContrato.Identificacion;

                var dto_DatosCredito = CargarDatosCredito(solicitudes);

                if (dto_DatosCredito.Any())
                {

                    //pagareCon = manager.TraeDocumentoPagare();
                    pagareContrato.IdSolicitud = dto_DatosCredito.FirstOrDefault().IdSolicitud;
                    pagareContrato.fechagenerapagare = DateTime.Today;
                    datosContrato = PagareContrato(pagareContrato);
                    return pagareContrato = datosContrato[0];

                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.IS_TELEGRAM = true;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return null;
        }
        #endregion TraeDatosContratoPagare

        #region Creación del contrato y pagaré


        public string CrearPdf(PagareContrato pagareContrato)
        {
            // Guid x = Guid.NewGuid();
            var pagareCon = new List<PagareContrato>();
            var datosContrato = new List<PagareContrato>();

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            Tab_ConfigSys dto_Config = new Tab_ConfigSys();

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(pagareContrato),
                FEC_CREACION = DateTime.Now
            };

            try
            {
                if (pagareContrato.IdSolicitud != null)
                {

                    pagareCon = TraeDocumentoPagare();
                    pagareContrato.IdSolicitud = pagareContrato.IdSolicitud;
                    pagareContrato.fechagenerapagare = DateTime.Today;
                    datosContrato = PagareContrato(pagareContrato);
                    pagareContrato.Pagare = pagareCon.FirstOrDefault().Pagare;
                    pagareContrato.Contrato = pagareCon.FirstOrDefault().Contrato;

                    string Contrato = pagareContrato.Contrato;
                    string Pagare = pagareContrato.Pagare;
                    if (datosContrato.Any())
                    {
                        string fileName = @"Pagare y contrato_" + datosContrato[0].Nombre + ".docx";

                        //Remplazo en la cadena los datos para generar el contrato
                        Contrato = Contrato.Replace("<Nombre>", datosContrato[0].Nombre);
                        Contrato = Contrato.Replace("<Identificacion>", datosContrato[0].Identificacion);
                        Contrato = Contrato.Replace("<MontoProductoLetras>", datosContrato[0].MontoProductoLetras);
                        Contrato = Contrato.Replace("<Moneda>", datosContrato[0].Moneda);
                        Contrato = Contrato.Replace("<MontoProducto>", Convert.ToString(datosContrato[0].MontoProducto));
                        Contrato = Contrato.Replace("<CantidadCuotas>", Convert.ToString(datosContrato[0].CantidadCuotas));
                        Contrato = Contrato.Replace("<Frecuencia>", datosContrato[0].Frecuencia);
                        Contrato = Contrato.Replace("<Cuota>", Convert.ToString(datosContrato[0].Cuota));
                        Contrato = Contrato.Replace("<FechaPrimerPagoLetras>", datosContrato[0].FechaPrimerPagoLetras);
                        Contrato = Contrato.Replace("<FechaUltimoPagoLetras>", datosContrato[0].FechaUltimoPagoLetras);
                        Contrato = Contrato.Replace("<Interes>", Convert.ToString(datosContrato[0].Interes));
                        Contrato = Contrato.Replace("<InteresLetras>", datosContrato[0].InteresLetras);
                        Contrato = Contrato.Replace("<CtaCliente>", datosContrato[0].CtaCliente);
                        Contrato = Contrato.Replace("<FechaHoy>", datosContrato[0].FechaHoy);
                        Contrato = Contrato.Replace("<CuotaenLetras>", datosContrato[0].CuotaenLetras);
                        Contrato = Contrato.Replace("<Frecuencia2>", datosContrato[0].Frecuencia2);
                        Contrato = Contrato.Replace("<Frecuencia3>", datosContrato[0].Frecuencia3);
                        Contrato = Contrato.Replace("<CantidadCuotasLetras>", datosContrato[0].CantidadCuotasLetras);

                        //Reemplazo los datos para generar el Pagaré
                        Pagare = Pagare.Replace("<Nombre>", datosContrato[0].Nombre);
                        Pagare = Pagare.Replace("<Identificacion>", datosContrato[0].Identificacion);
                        Pagare = Pagare.Replace("<MontoProductoLetras>", datosContrato[0].MontoProductoLetras);
                        Pagare = Pagare.Replace("<Moneda>", datosContrato[0].Moneda);
                        Pagare = Pagare.Replace("<MontoProductoPagare>", Convert.ToString(datosContrato[0].MontoProductoPagare));
                        Pagare = Pagare.Replace("<MontoProductoLetrasPagare>", Convert.ToString(datosContrato[0].MontoProductoLetrasPagare));
                        Pagare = Pagare.Replace("<CantidadCuotas>", Convert.ToString(datosContrato[0].CantidadCuotas));
                        Pagare = Pagare.Replace("<Frecuencia>", datosContrato[0].Frecuencia);
                        Pagare = Pagare.Replace("<Cuota>", Convert.ToString(datosContrato[0].Cuota));
                        Pagare = Pagare.Replace("<FechaPrimerPagoLetras>", datosContrato[0].FechaPrimerPagoLetras);
                        Pagare = Pagare.Replace("<FechaUltimoPagoLetras>", datosContrato[0].FechaUltimoPagoLetras);
                        Pagare = Pagare.Replace("<Interes>", Convert.ToString(datosContrato[0].Interes));
                        Pagare = Pagare.Replace("<InteresLetras>", datosContrato[0].InteresLetras);
                        Pagare = Pagare.Replace("<CtaCliente>", datosContrato[0].CtaCliente);
                        Pagare = Pagare.Replace("<FechaHoy>", datosContrato[0].FechaHoy);
                        Pagare = Pagare.Replace("<CuotaenLetras>", datosContrato[0].CuotaenLetras);
                        Pagare = Pagare.Replace("<CantidadCuotasLetras>", datosContrato[0].CantidadCuotasLetras);
                        Pagare = Pagare.Replace("<Frecuencia2>", datosContrato[0].Frecuencia2);
                        Pagare = Pagare.Replace("<pagoProximasCuotas>", datosContrato[0].PagoProximasCuotas);

                        //creamos la imagen editada de la firma para agregarla luego al documento
                        if (datosContrato[0].FotoFirma != "" && datosContrato[0].FotoFirma != null)
                        {
                            editarImagen(datosContrato[0].FotoFirma, datosContrato.FirstOrDefault().Identificacion);
                        }

                        //Doy formato al archivo PDF
                        iTextSharp.text.Font fontHeader_1 = FontFactory.GetFont("Calibri", 9, iTextSharp.text.Font.NORMAL, new iTextSharp.text.BaseColor(0, 0, 0));
                        //Genero el documento PDF con la libreria iTextSharp
                        using (Document pdfDoc = new Document(PageSize.LETTER, 25f, 25f, 25f, 25f))
                        {
                            iTextSharp.text.Document oDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER);
                            //Escribo el documento en el archivo PDF generado
                            //PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream);

                            var OutputPathDocumento = AppDomain.CurrentDomain.BaseDirectory + "/Machote/archivos/Documento-" + datosContrato.FirstOrDefault().Nombre.ToString() + ".pdf";
                            using (PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(OutputPathDocumento, FileMode.Create)))
                            {

                                pdfDoc.Open();

                                var espacio = new Paragraph(" ");
                                //Ingreso el texto que corresponde al Contrato
                                var tituloContrato = new Paragraph("Solicitud N°: " + datosContrato.FirstOrDefault().Id.ToString(), FontFactory.GetFont("Calibri", 14, BaseColor.BLACK));
                                tituloContrato.Alignment = Element.ALIGN_CENTER;
                                tituloContrato.Font.Size = 14;
                                pdfDoc.Add(tituloContrato);
                                pdfDoc.Add(espacio);
                                var Contratopdf = new Paragraph(Contrato, fontHeader_1);
                                Contratopdf.Alignment = Element.ALIGN_JUSTIFIED;
                                Contratopdf.Font.Size = 8;
                                pdfDoc.Add(Contratopdf);


                                //si existe foto de la firma lo agregamos al contrato
                                if (datosContrato[0].FotoFirma != "" && datosContrato[0].FotoFirma != null)
                                {
                                    //creamos la imagen de la firma para el PDF
                                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");
                                    imagen.BorderWidth = 0;
                                    imagen.Alignment = Element.ALIGN_LEFT;
                                    float percentage = 0.0f;
                                    percentage = 70 / imagen.Width;
                                    imagen.ScalePercent(percentage * 100);

                                    //creamos una tabla para agregar la imagen
                                    PdfPTable tablaImagen = new PdfPTable(1);
                                    PdfPCell c1 = new PdfPCell();
                                    PdfPCell c2 = new PdfPCell(imagen);
                                    c1.Border = 0;
                                    c2.Border = 0;
                                    c2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    c2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    tablaImagen.AddCell(c1); //celda con espacio a la derecha
                                    tablaImagen.AddCell(c2); //celda con la imagen de la firma

                                    // Insertamos la tabla con la imagen de la firma en el contrato
                                    pdfDoc.Add(tablaImagen);
                                }
                                else
                                {
                                    pdfDoc.Add(espacio);
                                    pdfDoc.Add(espacio);
                                }

                                //Agregamos la info final del contrato
                                string textoFinal = "               ……………………………………………                                                                                          ……………………………………………\n                                   El DEUDOR                                                                                                                                   LA ACREEDORA\n\n\n                                    " + datosContrato.FirstOrDefault().Identificacion.ToString() + "\n               …………………………………………      \n                                     CÉDULA ";
                                var parrafoFinal = new Paragraph(textoFinal, fontHeader_1);
                                pdfDoc.Add(parrafoFinal);

                                //Ingreso el titulo del Pagaré
                                pdfDoc.NewPage();
                                var SubtituloPdf = new Paragraph("Pagaré N°: " + datosContrato.FirstOrDefault().Id.ToString(), FontFactory.GetFont("Calibri", 14, BaseColor.BLACK));
                                SubtituloPdf.Alignment = Element.ALIGN_CENTER;
                                SubtituloPdf.Font.Size = 14;
                                pdfDoc.Add(SubtituloPdf);
                                pdfDoc.Add(espacio);
                                //Ingreso Texto de Pagare
                                var PagarePdf = new Paragraph(Pagare, FontFactory.GetFont("Calibri", 10, BaseColor.BLACK));
                                PagarePdf.Alignment = Element.ALIGN_JUSTIFIED;
                                PagarePdf.Font.Size = 10;
                                pdfDoc.Add(PagarePdf);

                                if (datosContrato[0].FotoFirma != "" && datosContrato[0].FotoFirma != null)
                                {
                                    iTextSharp.text.Image imagen = iTextSharp.text.Image.GetInstance(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");
                                    imagen.BorderWidth = 0;
                                    imagen.Alignment = Element.ALIGN_LEFT;
                                    float percentage = 0.0f;
                                    percentage = 70 / imagen.Width;
                                    imagen.ScalePercent(percentage * 100);
                                    // Insertamos la imagen de la firma en el Pagare
                                    PdfPTable tablaImagenPagare = new PdfPTable(5);
                                    iTextSharp.text.Phrase fraseFirma = new iTextSharp.text.Phrase("Firma Deudor:");
                                    fraseFirma.Font.Size = 10;
                                    PdfPCell espacioCelda = new PdfPCell();
                                    PdfPCell celdaTexto = new PdfPCell(fraseFirma);
                                    PdfPCell celdaFirma = new PdfPCell(imagen);
                                    espacioCelda.Border = 0;
                                    celdaTexto.Border = 0;
                                    celdaTexto.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    celdaTexto.HorizontalAlignment = Element.ALIGN_LEFT;
                                    celdaFirma.Border = 0;
                                    celdaFirma.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    celdaFirma.HorizontalAlignment = Element.ALIGN_LEFT;
                                    tablaImagenPagare.AddCell(celdaTexto); //celda con el texto de la firma
                                    tablaImagenPagare.AddCell(celdaFirma); //celda con la imagen de la firma
                                    tablaImagenPagare.AddCell(espacioCelda);//celda con espacio en blanco
                                    tablaImagenPagare.AddCell(espacioCelda);//celda con espacio en blanco
                                    tablaImagenPagare.AddCell(espacioCelda);//celda con espacio en blanco
                                    pdfDoc.Add(tablaImagenPagare);
                                }
                                else
                                {
                                    var parrafoFirma = new Paragraph("\n\n                     Firma Deudor: \n\n\n", fontHeader_1);
                                    parrafoFirma.Font.Size = 10;
                                    pdfDoc.Add(parrafoFirma);
                                }
                                //Agregamos la info final del contrato
                                string textoFinalPagare = "                     Nombre Completo:   " + datosContrato[0].Nombre + "\n\n\n                     Número de Cédula:   " + datosContrato[0].Identificacion;
                                var parrafoFinalPagare = new Paragraph(textoFinalPagare, fontHeader_1);
                                parrafoFinalPagare.Font.Size = 10;
                                pdfDoc.Add(parrafoFinalPagare);
                                //Cierro el archivo PDF
                                pdfDoc.Close();
                                wri.Close();
                            }
                            oDoc.Close();
                        }

                        //Proceso para crear y agregar el formulario BCCR a el pdf final
                        var path = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\formulario-bac.html";
                        var pathOriginal = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\formulario-bac-sin-remplazo.html";
                        string html = File.ReadAllText(pathOriginal);
                        string EncodedString = "";
                        string ret = "";
                        string DecodedStringFormulario;
                        string htmlOriginal = html; //este sera el html orinal sin hacer los remplazas para luego de todo el proceso volver a guaradr el archivo html con los valores originales
                        using (StringWriter writer = new StringWriter())
                        {
                            HttpUtility.HtmlEncode(html, writer);

                            //Server.HtmlEncode(html, writer);
                            EncodedString = html.ToString();
                            ret = completarFormularioDomiciliacion(EncodedString, datosContrato[0]); // replazamos el html con la info dinamica
                            DecodedStringFormulario = HttpUtility.HtmlDecode(ret); //Server.HtmlDecode(ret);
                            writer.Close();
                        }
                        using (StreamWriter sw = new StreamWriter(path)) //sobrescribimos el html con la nueva info
                        {
                            sw.WriteLine(DecodedStringFormulario);
                            sw.Close();
                        }
                        //Generamos el pdf del formulario caon base en el archivo html

                        NReco.PdfGenerator.HtmlToPdfConverter pdfFormulario = new NReco.PdfGenerator.HtmlToPdfConverter();
                        pdfFormulario.PageHeight = 279;
                        pdfFormulario.PageWidth = 216;
                        pdfFormulario.GeneratePdfFromFile(path, null, @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosContrato.FirstOrDefault().Nombre + ".pdf");

                        //llamamos al metodo interno mergePdfs para unir los pdfs del contrato, el pagare y el formulario
                        string rutaDocumentoFinal = AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Contrato-Pagare-" + pagareContrato.Identificacion + ".pdf";


                        //if (ValidarFichero(rutaDocumentoFinal) == true)
                        //               {
                        //                   rutaDocumentoFinal = Renamefile(rutaDocumentoFinal, pagareContrato.Identificacion, ".pdf");
                        string[] archivos = { AppDomain.CurrentDomain.BaseDirectory + "/Machote/archivos/Documento-" + datosContrato.FirstOrDefault().Nombre + ".pdf", @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosContrato.FirstOrDefault().Nombre + ".pdf" };
                        mergePdfs(rutaDocumentoFinal, archivos);

                        var buffer = File.ReadAllBytes(rutaDocumentoFinal);
                        dto_Config.llave_Config1 = "SERVICIO";
                        dto_Config.llave_Config2 = "CONFIGURACION";
                        dto_Config.llave_Config3 = "SERVIDOR";
                        dto_Config.llave_Config4 = "URL";

                        var dto_interval = ConsultaConfiUrlImagen(dto_Config);

                        string URL = dto_interval.Where(Y => Y.llave_Config5 == "RUTA_BLOB_PAGARE").Select(Y => Y.Dato_Char1).FirstOrDefault();
                        var blob = new blobStorage
                        {
                            ImageToUploadByte = buffer,
                            ContainerPrefix = string.Concat(dto_interval.Where(y => y.llave_Config5 == "RUTA_BLOB_PAGARE").Select(y => y.Dato_Char1).FirstOrDefault(), "/", datosContrato.FirstOrDefault().Identificacion), //"documentos/FotoCedula/206560175",
                            ImageExtencion = ".pdf",
                            ImageToUpload = datosContrato.FirstOrDefault().Identificacion,
                            ConnectionString = dto_interval.Where(y => y.llave_Config5 == "CONECTION").Select(y => y.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                            PatchTempToSave = dto_interval.Where(y => y.llave_Config5 == "CONTRATOPAGARE").Select(y => y.Dato_Char1).FirstOrDefault()//@"C:\IVAN\images\FotoCedulas\206560175"
                        };
                        UtilBlobStorageAzure.UploadBlobStorage(blob);
                        //Thread threadObj = new Thread(new ThreadStart(() => UtilBlobStorageAzure.UploadBlobStorage(blob)));
                        //threadObj.Start();
                        //volvemos a guaradar el archivo html Original(sin los valores remplazados)
                        using (StreamWriter sw = new StreamWriter(path))
                        {
                            sw.WriteLine(htmlOriginal);
                            sw.Close();
                        }
                        File.Delete(AppDomain.CurrentDomain.BaseDirectory + "/Machote/archivos/Documento-" + datosContrato.FirstOrDefault().Nombre + ".pdf");
                        File.Delete(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosContrato.FirstOrDefault().Nombre + ".pdf");
                        File.Delete(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");
                        File.Delete(rutaDocumentoFinal);

                        rutaDocumentoFinal = string.Concat(dto_interval.Where(y => y.llave_Config5 == "RUTA_BLOB_PAGARE").Select(y => y.Dato_Char1).FirstOrDefault(), "/", datosContrato.FirstOrDefault().Identificacion, ".pdf");
                        return rutaDocumentoFinal;

                    }
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                dto_excepcion.IS_TELEGRAM = true;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                pagareContrato.Mensaje = "ERR";
            }

            return null;
        }
        //public Stream ConvertToBase64(Stream stream)
        //{
        //	Byte[] inArray = new Byte[(int)stream.Length];
        //	Char[] outArray = new Char[(int)(stream.Length * 1.34)];
        //	stream.Read(inArray, 0, (int)stream.Length);
        //	Convert.ToBase64CharArray(inArray, 0, inArray.Length, outArray, 0);
        //	return new MemoryStream(Encoding.UTF8.GetBytes(outArray));
        //}

        private string completarFormularioDomiciliacion(string textoHtml, PagareContrato pagare)
        {

            //sustituimos los valores en el texto html
            textoHtml = textoHtml.Replace("{Nombre-titular-cuenta-cliente}", pagare.Nombre);
            textoHtml = textoHtml.Replace("{email-cliente}", pagare.Correo);
            textoHtml = textoHtml.Replace("{telefono-cliente}", pagare.TelefonoCel.ToString());
            textoHtml = textoHtml.Replace("{codigo-cliente}", pagare.Identificacion.ToString());
            var fotoFirmPath = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png";
            if (File.Exists(fotoFirmPath))
            {
                iTextSharp.text.Image imagenFirma = iTextSharp.text.Image.GetInstance(fotoFirmPath);
                var imageWidthFirma = imagenFirma.Width;
                var imageHeightFirma = imagenFirma.Height;
                if (imageWidthFirma > imageHeightFirma)
                {
                    textoHtml = textoHtml.Replace("{Foto-Firma}", "<p class=\"fotoFirma\"><img style=\" margin-left: 0px; width: 420px;  \" src =\"" + @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png" + "\" /></ p >");
                }
                else {
                    textoHtml = textoHtml.Replace("{Foto-Firma}", "<p class=\"fotoFirma\"><img style=\" margin-left: 105px; width: 300px;  \" src =\"" + @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png" + "\" /></ p >");
                }
            }
            else
            {
                textoHtml = textoHtml.Replace("{Foto-Firma}", "<p class=\"fotoFirma\"><img style=\" margin-left: 0px; width: 500px;  \" src =\"\" /></ p >");
                //textoHtml = textoHtml.Replace("{Foto-Firma}", "FotoFirma.jpeg");
            }
            //textoHtml = textoHtml.Replace("{monto-total}", pagare.MontoTotal.ToString());
            textoHtml = remplazarCedulaPDF(textoHtml, pagare);
            textoHtml = remplazarCuentaClientePDF(textoHtml, pagare);
            textoHtml = remplazarMontoTotalPDF(textoHtml, pagare);


            return textoHtml;
        }
        private string remplazarCedulaPDF(string textoHtml, PagareContrato pagare)
        {

            string caracter;
            for (int i = 1; i <= 20; i++)
            {
                if (i <= pagare.Identificacion.Length)
                {
                    caracter = pagare.Identificacion[i - 1] + "";
                    textoHtml = textoHtml.Replace("{a" + i + "}", caracter);
                }
                else
                {
                    textoHtml = textoHtml.Replace("{a" + i + "}", "");
                }
            }


            return textoHtml;
        }
        private string remplazarCuentaClientePDF(string textoHtml, PagareContrato pagare)
        {

            string caracter;
            for (int i = 1; i <= 22; i++)
            {
                if (i <= pagare.CtaCliente.Length)
                {
                    caracter = pagare.CtaCliente[i - 1] + "";
                    textoHtml = textoHtml.Replace("{b" + i + "}", caracter);
                }
                else
                {
                    textoHtml = textoHtml.Replace("{b" + i + "}", "");
                }
            }

            return textoHtml;
        }
        private string remplazarMontoTotalPDF(string textoHtml, PagareContrato pagare)
        {

            string caracter;
            int montoEntero = Convert.ToInt32(pagare.Cuota * pagare.CantidadCuotas);
            string monto = montoEntero.ToString();
            int contador = monto.Length;

            for (int i = 9; i >= 1; i--)
            {
                //vamos a poner ceros en los valores despues de la coma.
                if (i > 7)
                {
                    textoHtml = textoHtml.Replace("{c" + i + "}", "0");
                }
                else
                {
                    if (contador > 0)
                    {
                        caracter = monto[contador - 1].ToString();
                        textoHtml = textoHtml.Replace("{c" + i + "}", caracter);
                        contador--;
                    }
                    else
                    {
                        textoHtml = textoHtml.Replace("{c" + i + "}", "");
                    }

                }

            }

            return textoHtml;
        }

        static void editarImagen(string rutaImagen, string Identificacion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var manager = new Manager.ManagerSolcitudes();
            List<Solicitudes> sol = new List<Solicitudes>();
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(Identificacion),
                FEC_CREACION = DateTime.Now
            };
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            byte[] arrImageCedulaFrontal = null;
            var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
            try
            {
                if (rutaImagen == "")
                {
                    rutaImagen = Identificacion + ".jpg";
                }
                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_FOTOFIRMA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = Path.GetExtension(rutaImagen),
                    ImageToUpload = Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada"//@"C:\IVAN\images\FotoCedulas\206560175"
                };
                if (UtilBlobStorageAzure.ExistsFileInBlob(blobDowland))
                {
                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                //Thread threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                //Thread.Sleep(5000);
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                DirectoryInfo directory = new DirectoryInfo(blobDowland.PatchTempToSave);
                directory.EnumerateFiles().ToList().ForEach(
                        x =>
                        {
                            arrImageCedulaFrontal = System.IO.File.ReadAllBytes(x.FullName);
                            x.Delete();
                        }
                        );
                //Directory.Delete(blobDowland.PatchTempToSave);
                System.Drawing.Bitmap imagen;
                //cargar la imagen de la ruta de la imagen
                imagen = Utilitarios.BytesToBitmap(arrImageCedulaFrontal);
                //crear la secuencia de filtros
                AForge.Imaging.Filters.FiltersSequence filter = new AForge.Imaging.Filters.FiltersSequence();
                //agregar los filtros a la secuencia de filtros
                filter.Add(new AForge.Imaging.Filters.ContrastCorrection(5));
                filter.Add(new AForge.Imaging.Filters.BrightnessCorrection(5));
                //aplicar los filtros 
                imagen = filter.Apply(imagen);
                imagen.Save(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");
                imagen.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }
        private string remplazarCodigoServicioPDF(string textoHtml, PagareContrato pagare)
        {

            string caracter;
            string idSolicitud = pagare.Id.ToString();
            if (idSolicitud != null)
            {
                for (int i = 1; i <= 21; i++)
                {
                    if (i <= idSolicitud.Length)
                    {
                        caracter = idSolicitud[i - 1].ToString();
                        textoHtml = textoHtml.Replace("{d" + i + "}", caracter);
                    }
                    else
                    {
                        textoHtml = textoHtml.Replace("{d" + i + "}", "");
                    }
                }
            }
            else
            {
                for (int i = 1; i <= 21; i++)
                {
                    textoHtml = textoHtml.Replace("{d" + i + "}", "");
                }
            }


            return textoHtml;
        }
        private void mergePdfs(string rutaDocumentoFinal, String[] archivos)
        {
            string[] source_files = archivos;
            string result = rutaDocumentoFinal;
            //create Document object
            using (Document document = new Document())
            {
                //create PdfCopy object
                using (PdfCopy copy = new PdfCopy(document, new FileStream(result, FileMode.Create)))
                {
                    //open the document
                    document.Open();
                    //PdfReader variable
                    //PdfReader reader;
                    for (int i = 0; i < source_files.Length; i++)
                    {
                        if (source_files[i] != "" && source_files[i] != null)
                        {
                            //create PdfReader object
                            using (PdfReader reader = new PdfReader(source_files[i]))
                            {
                                //merge combine pages
                                for (int page = 1; page <= reader.NumberOfPages; page++)
                                    copy.AddPage(copy.GetImportedPage(reader, page));
                                reader.Close();
                            }
                        }
                    }

                    //close the document object
                    document.Close();
                    copy.Close();

                }
            }
        }

        public string Renamefile(string pathFileName, string Nombrefile, string Extension)
        {
            string s_newfilename = "";
            string carpeta = Path.GetDirectoryName(pathFileName);
            int NumFileTemp = 0;
            DirectoryInfo directory = new DirectoryInfo(carpeta);

            FileInfo[] files = directory.GetFiles("*.*");
            if (files.FirstOrDefault() != null)
            {
                int i = 0;
                foreach (var File in files)
                {
                    i++;
                    string NomTemp = Nombrefile + "_" + i + Extension;

                    string nombre = File.Name;
                    int start = nombre.IndexOf("_") + 1;
                    int end = nombre.IndexOf(".", start);
                    NumFileTemp = Convert.ToInt32(nombre.Substring(start, end - start));


                }

                if (NumFileTemp != 0)
                {
                    NumFileTemp = NumFileTemp + 1;
                    s_newfilename = Nombrefile + "_" + NumFileTemp + Extension;
                }
            }
            else
            {
                s_newfilename = Nombrefile + "_1" + Extension;
            }
            return carpeta + "/" + s_newfilename;
        }
        #endregion Creación del contrato y pagaré

        #region ConsultaDirectorioImagen
        public List<DTO_SOLICITUD_VENTAS> ConsultaDirectorioImagen(DTO_SOLICITUD_VENTAS ImageUrls)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in ImageUrls.GetType().GetProperties()
                                       where nodo.GetValue(ImageUrls) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(ImageUrls).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_Obtener_DIR_Imagen_Solicictud";
            var dto_result = new List<DTO_SOLICITUD_VENTAS>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_SOLICITUD_VENTAS>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                ImageUrls.Mensaje = "ERR";
                throw;

            }
            return dto_result;
        }
        #endregion ConsultaDirectorioImagen
        //Alfredo José Vargas Seinfarth-14/02/2019--//FIN

        #endregion

        #region Consulta de simpe

        public List<Dto_Sinpe> ConsultaSolicitudSinpe(Dto_Sinpe sinpe)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(sinpe),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            try
            {
                dto.ParameterList.AddRange(from nodo in sinpe.GetType().GetProperties()
                                           where nodo.GetValue(sinpe) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(sinpe).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_solicitudes_pendientes_cuenta_simpe";


                var objRet = DynamicSqlDAO.ExecuterSp<Dto_Sinpe>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //if (objRet.HasResult)
                //{
                //    dto_result = JsonConvert.DeserializeObject<List<Dto_Sinpe>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                //}
                return objRet.ToList();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<Dto_Sinpe>();
        }

        #endregion

        #region crear Doc Domiciliacion
        public dto_file CrearDocDom(PagareContrato documentoDom)
        {
            dto_file dto_File = new dto_file();
            var datosDomiciliacion = new List<PagareContrato>();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(documentoDom),
                FEC_CREACION = DateTime.Now
            };
            try
            {
                if (documentoDom.IdSolicitud != null)
                {
                    datosDomiciliacion = DatosDomiciliacion(documentoDom);
                    if (datosDomiciliacion.Any())
                    {
                            editarImagen(datosDomiciliacion[0].FotoFirma, datosDomiciliacion.FirstOrDefault().Identificacion);

                        var path = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\formulario-bac.html";
                        var pathOriginal = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\formulario-bac-sin-remplazo.html";
                        string html = File.ReadAllText(pathOriginal);
                        string EncodedString = "";
                        string ret = "";
                        string DecodedStringFormulario;
                        string htmlOriginal = html; //este sera el html orinal sin hacer los remplazas para luego de todo el proceso volver a guaradr el archivo html con los valores originales
                        using (StringWriter writer = new StringWriter())
                        {
                            HttpUtility.HtmlEncode(html, writer);
                            EncodedString = html.ToString();
                            ret = completarFormularioDomiciliacion(EncodedString, datosDomiciliacion[0]); // replazamos el html con la info dinamica
                            DecodedStringFormulario = HttpUtility.HtmlDecode(ret); //Server.HtmlDecode(ret);
                            writer.Close();
                        }
                        using (StreamWriter sw = new StreamWriter(path)) //sobrescribimos el html con la nueva info
                        {
                            sw.WriteLine(DecodedStringFormulario);
                            sw.Close();
                        }
                        NReco.PdfGenerator.HtmlToPdfConverter pdfFormulario = new NReco.PdfGenerator.HtmlToPdfConverter();
                        pdfFormulario.PageHeight = 279;
                        pdfFormulario.PageWidth = 216;
                        pdfFormulario.GeneratePdfFromFile(path, null, @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosDomiciliacion.FirstOrDefault().Nombre + ".pdf");
                        string rutaDocumentoFinal = AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\archivo-dom-" + datosDomiciliacion[0].Identificacion + ".pdf";
                        string pathImagenesPdf = "";
                            try
                            {
                                ObtieneFotoCedula(datosDomiciliacion[0].FotoCedula, datosDomiciliacion[0].Identificacion);
                                ObtieneFotoCedulaDorso(datosDomiciliacion[0].FotoCedulaTrasera, datosDomiciliacion[0].Identificacion);
                            var fotoCedulaPath = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoCedulaFrontal.png";
                            var fotoCedulaDorsoPath = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoCedulaDorso.png";
                            if (File.Exists(fotoCedulaPath) && File.Exists(fotoCedulaDorsoPath))
                            {
                                using (Document pdfDoc = new Document(PageSize.LETTER, 25f, 25f, 25f, 25f))
                                {
                                    iTextSharp.text.Document oDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.LETTER);

                                    pathImagenesPdf = AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\imagenes-temp.pdf";
                                    using (PdfWriter wri = PdfWriter.GetInstance(pdfDoc, new FileStream(pathImagenesPdf, FileMode.Create)))
                                    {
                                        pdfDoc.Open();
                                        var pdfWidth = pdfDoc.PageSize.Width;
                                        var pdfHeight = pdfDoc.PageSize.Height;

                                        iTextSharp.text.Image imagenFrente = iTextSharp.text.Image.GetInstance(fotoCedulaPath);
                                        imagenFrente.Alignment = Element.ALIGN_CENTER;
                                        imagenFrente.Alignment = Element.ALIGN_MIDDLE;
                                        var imageWidthFrente = imagenFrente.Width;
                                        var imageHeightFrente = imagenFrente.Height;
                                        if (imageWidthFrente >= imageHeightFrente)  //la imegen es rectangular con width mayor al heigth
                                        {
                                            if (imageWidthFrente >= pdfWidth)
                                            {
                                                var reduccionWidth = (imageWidthFrente - pdfWidth);
                                                var percentWidth = (100 / imageWidthFrente) * reduccionWidth; //Obtenemos el porcentaje de reduccion para aplicar este mismo porcentaje al heigth
                                                var nuevoHeigth = ((imageHeightFrente / 100) * (100 - percentWidth)); // al Heigth se le reduce el mismo porcentaje con respecto al width
                                                imagenFrente.ScaleAbsoluteWidth(pdfWidth - 10);
                                                imagenFrente.ScaleAbsoluteHeight(nuevoHeigth - 10);
                                            }
                                        }
                                        else  //la imegen es rectangular con heigth mayor al width
                                        {
                                            if (imageHeightFrente >= pdfHeight)
                                            {
                                                var reduccionHeigth = (imageHeightFrente - pdfHeight);
                                                var percentHeigth = (100 / imageHeightFrente) * reduccionHeigth; //Obtenemos el porcentaje de reduccion para aplicar este mismo porcentaje al width
                                                var nuevoWidth = ((imageWidthFrente / 100) * (100 - percentHeigth)); // al width se le reduce el mismo porcentaje con respecto al heigth
                                                imagenFrente.ScaleAbsoluteHeight(pdfHeight);
                                                imagenFrente.ScaleAbsoluteWidth(nuevoWidth);
                                            }
                                        }
                                        iTextSharp.text.Image imagenAtras = iTextSharp.text.Image.GetInstance(fotoCedulaDorsoPath);
                                        imagenAtras.Alignment = Element.ALIGN_CENTER;
                                        imagenAtras.Alignment =  Element.ALIGN_MIDDLE;
                                        var imageWidthAtras = imagenFrente.Width;
                                        var imageHeightAtras = imagenFrente.Height;
                                        if (imageWidthAtras >= imageHeightAtras)  //la imegen es rectangular con width mayor al heigth
                                        {
                                            if (imageWidthAtras >= pdfWidth)
                                            {
                                                var reduccionWidthAtras = (imageWidthAtras - pdfWidth);
                                                var percentWidthAtras = (100 / imageWidthAtras) * reduccionWidthAtras; //Obtenemos el porcentaje de reduccion para aplicar este mismo porcentaje al heigth
                                                var nuevoHeigthAtras = ((imageHeightAtras / 100) * (100 - percentWidthAtras)); // al Heigth se le reduce el mismo porcentaje con respecto al width
                                                imagenAtras.ScaleAbsoluteWidth(pdfWidth - 10);
                                                imagenAtras.ScaleAbsoluteHeight(nuevoHeigthAtras - 10);
                                            }
                                        }
                                        else //la imegen es rectangular con heigth mayor al width
                                        {
                                            if (imageHeightAtras >= pdfHeight)
                                            {
                                                var reduccionHeigth = (imageHeightAtras - pdfHeight);
                                                var percentHeigth = (100 / imageHeightAtras) * reduccionHeigth; //Obtenemos el porcentaje de reduccion para aplicar este mismo porcentaje al width
                                                var nuevoWidth = ((imageWidthAtras / 100) * (100 - percentHeigth)); // al width se le reduce el mismo porcentaje con respecto al heigth
                                                imagenAtras.ScaleAbsoluteHeight(pdfHeight);
                                                imagenAtras.ScaleAbsoluteWidth(nuevoWidth);
                                            }
                                        }
                                        pdfDoc.Open();
                                        //pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(0, 0, imagenFrente.Width, imagenFrente.Height));

                                        //pdfDoc.SetMargins(0, 0, 0, 0);

                                        pdfDoc.NewPage();
                                        pdfDoc.Add(imagenFrente);
                                        //pdfDoc.SetPageSize(new iTextSharp.text.Rectangle(0, 0, imagenAtras.Width, imagenAtras.Height));
                                        //pdfDoc.SetMargins(0, 0, 0, 0);
                                        pdfDoc.NewPage();

                                        pdfDoc.Add(imagenAtras);
                                        wri.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
                                        wri.CompressionLevel = PdfStream.BEST_COMPRESSION;
                                        pdfDoc.Close();
                                        wri.Close();
                                    }
                                    oDoc.Close();
                                }
                            }
                            }
                            catch (Exception ex)
                            {
                                dto_File.Respuesta = "Error al generar pdf";
                        }
                        string[] archivos = { @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosDomiciliacion.FirstOrDefault().Nombre + ".pdf", pathImagenesPdf };
                        mergePdfs(rutaDocumentoFinal, archivos);
                        dto_File.Type = TwoFunTwoMe_DataAccess.Utility.GetMimeType(".pdf");
                        dto_File.Base64 = string.Format("data:{0};base64,{1}", dto_File.Type, Convert.ToBase64String(File.ReadAllBytes(rutaDocumentoFinal)));
                        dto_File.Name = "Formulario - Domiciliación - " + datosDomiciliacion.FirstOrDefault().Nombre;

                        File.Delete(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoCedulaFrontal.png");
                        File.Delete(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoCedulaDorso.png");
                        File.Delete(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoFirma-Copia-Editada.png");
                        File.Delete(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\archivos\Formulario-" + datosDomiciliacion.FirstOrDefault().Nombre + ".pdf");
                        File.Delete(rutaDocumentoFinal);

                        return dto_File;
                    }
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                dto_excepcion.IS_TELEGRAM = true;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return null;
        }
        public List<PagareContrato> DatosDomiciliacion(PagareContrato pagareContrato)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in pagareContrato.GetType().GetProperties()
                                       where nodo.GetValue(pagareContrato) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pagareContrato).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_datosDomiciliacion";
            var dto_result = new List<PagareContrato>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<PagareContrato>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

        static void ObtieneFotoCedula(string rutaImagen, string Identificacion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var manager = new Manager.ManagerSolcitudes();
            List<Solicitudes> sol = new List<Solicitudes>();
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(Identificacion),
                FEC_CREACION = DateTime.Now
            };
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            byte[] arrImageCedulaFrontal = null;
            var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
            try
            {
                if (rutaImagen == "")
                {
                    rutaImagen = Identificacion + ".jpg";
                }
                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = Path.GetExtension(rutaImagen),
                    ImageToUpload = Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoCedulaFrontal"
                };
                if (UtilBlobStorageAzure.ExistsFileInBlob(blobDowland))
                {
                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                //Thread threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                //Thread.Sleep(5000);
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                DirectoryInfo directory = new DirectoryInfo(blobDowland.PatchTempToSave);
                directory.EnumerateFiles().ToList().ForEach(
                        x =>
                        {
                            arrImageCedulaFrontal = System.IO.File.ReadAllBytes(x.FullName);
                            x.Delete();
                        }
                        );
                //Directory.Delete(blobDowland.PatchTempToSave);
                System.Drawing.Bitmap imagen;
                //cargar la imagen de la ruta de la imagen
                imagen = Utilitarios.BytesToBitmap(arrImageCedulaFrontal);
                //crear la secuencia de filtros
                AForge.Imaging.Filters.FiltersSequence filter = new AForge.Imaging.Filters.FiltersSequence();
                //agregar los filtros a la secuencia de filtros
                filter.Add(new AForge.Imaging.Filters.ContrastCorrection(5));
                filter.Add(new AForge.Imaging.Filters.BrightnessCorrection(5));
                //aplicar los filtros 
                imagen = filter.Apply(imagen);
                imagen.Save(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoCedulaFrontal.png");
                imagen.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }

        static void ObtieneFotoCedulaDorso(string rutaImagen, string Identificacion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var manager = new Manager.ManagerSolcitudes();
            List<Solicitudes> sol = new List<Solicitudes>();
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_PARAMETROS = JsonConvert.SerializeObject(Identificacion),
                FEC_CREACION = DateTime.Now
            };
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            dto_Config.llave_Config1 = "SERVICIO";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "SERVIDOR";
            dto_Config.llave_Config4 = "URL";
            byte[] arrImageCedulaFrontal = null;
            var dto_interval = manager.ConsultaConfiUrlImagen(dto_Config);
            try
            {
                if (rutaImagen == "")
                {
                    rutaImagen = Identificacion + ".jpg";
                }
                var blobDowland = new blobStorage
                {
                    ImageToUploadByte = null,
                    ContainerPrefix = string.Concat(dto_interval.Where(x => x.llave_Config5 == "RUTA_BLOB_CEDULATRASERA").Select(x => x.Dato_Char1).FirstOrDefault(), "/", Identificacion), //"documentos/FotoCedula/206560175",
                    ImageExtencion = Path.GetExtension(rutaImagen),
                    ImageToUpload = Identificacion,
                    ConnectionString = dto_interval.Where(x => x.llave_Config5 == "CONECTION").Select(x => x.Dato_Char1).FirstOrDefault(),// "DefaultEndpointsProtocol=https;AccountName=fenix2fun2me;AccountKey=qEZhKqhySrIvLgiplmWIVwTR9kCFtznIFEmMhrfF56jWwlSnUJuh2fCXYmBtKl2dafQb+f/UYBUv1RQP5n9/Mg==;EndpointSuffix=core.windows.net",
                    PatchTempToSave = @AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoCedulaTacera"
                };
                if (UtilBlobStorageAzure.ExistsFileInBlob(blobDowland))
                {
                if (!Directory.Exists(blobDowland.PatchTempToSave))
                    Directory.CreateDirectory(blobDowland.PatchTempToSave);
                //Thread threadObjDowland = new Thread(new ThreadStart(() => UtilBlobStorageAzure.DownloadBlobStorage(blobDowland)));
                //threadObjDowland.Start();
                //Thread.Sleep(5000);
                UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
                DirectoryInfo directory = new DirectoryInfo(blobDowland.PatchTempToSave);
                directory.EnumerateFiles().ToList().ForEach(
                        x =>
                        {
                            arrImageCedulaFrontal = System.IO.File.ReadAllBytes(x.FullName);
                            x.Delete();
                        }
                        );
                //Directory.Delete(blobDowland.PatchTempToSave);
                System.Drawing.Bitmap imagen;
                //cargar la imagen de la ruta de la imagen
                imagen = Utilitarios.BytesToBitmap(arrImageCedulaFrontal);
                //crear la secuencia de filtros
                AForge.Imaging.Filters.FiltersSequence filter = new AForge.Imaging.Filters.FiltersSequence();
                //agregar los filtros a la secuencia de filtros
                filter.Add(new AForge.Imaging.Filters.ContrastCorrection(5));
                filter.Add(new AForge.Imaging.Filters.BrightnessCorrection(5));
                //aplicar los filtros 
                imagen = filter.Apply(imagen);
                imagen.Save(@AppDomain.CurrentDomain.BaseDirectory + @"\Machote\FotoCedulaDorso.png");
                imagen.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion
        #region consulta EstadoSolicitud
        public List<ConsultaEstadoSolicitud> consultaEstadoSolicitud(ConsultaEstadoSolicitud idEstado)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(idEstado),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            var dto_result = new List<ConsultaEstadoSolicitud>();
            try
            {
                dto.ParameterList.AddRange(from nodo in idEstado.GetType().GetProperties()
                                           where nodo.GetValue(idEstado) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(idEstado).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_consultaDescripcionStatus";
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ConsultaEstadoSolicitud>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
            }
            return dto_result;
        }
        #endregion
        #region ConsultaFechaVencimientoCedula
        public Solicitudes ConsultaFechaVencimientoCedula(Solicitudes solicitudes)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["APLICATIVO"].ToString(),
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in solicitudes.GetType().GetProperties()
                                       where nodo.GetValue(solicitudes) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(solicitudes).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_Fecha_Vencimiento_Cedula";
            var dto_result = new List<Solicitudes>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp<Solicitudes>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return objRet.ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        #endregion
        public List<MontoCredito> CargarProductos(MontoCredito montoCredito)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.Result = null;
            dto.SPName = "usp_TraeMontoProductoFenix";
            var dto_result = new List<MontoCredito>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<MontoCredito>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Respuesta = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;
        }
        public List<ProductosMantenimiento> ConsultaProductos(ProductosMantenimiento montoCredito)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.Result = null;
            dto.SPName = "usp_ConsultaProductosFenix";
            var dto_result = new List<ProductosMantenimiento>();
            try
            {
                dto.ParameterList.AddRange(from nodo in montoCredito.GetType().GetProperties()
                                           where nodo.GetValue(montoCredito) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(montoCredito).ToString()
                                           }
                  );
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ProductosMantenimiento>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Respuesta = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;
        }
        public List<DtoSubOrigen> consultaSubOrigenes()
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.Result = null;
            dto.SPName = "usp_TraeOrigenes";
            var dto_result = new List<DtoSubOrigen>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DtoSubOrigen>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;
        }


        public List<AplicaCobrosAutomaticos> InsertaCobroAutomatico(AplicaCobrosAutomaticos config)
        {
            string xClase = string.Format("{0}|{1}",
      MethodBase.GetCurrentMethod().Module.Name,
      MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(config)
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in config.GetType().GetProperties()
                                       where nodo.GetValue(config) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(config).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_aplicion_cobro_automatico";
            var dto_result = new List<AplicaCobrosAutomaticos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<AplicaCobrosAutomaticos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;

        }
    }
}

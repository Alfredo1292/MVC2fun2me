using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_Common;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.InsertaSolicitud;

namespace TwoFunTwoMeFintech.Models.Manager
{
    public class clsInsertaSolicitud
    {
        private static InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };
        public clsInsertaSolicitud()
        {
            String connBD = string.Empty;
            infDto.STR_COD_PAIS = "Ventas";
            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["BuroConnection"].ConnectionString);
            }
        }

        internal static void InsertaSolicitudesInvoke(ref InsertPersonasWeb persona)
        {
            var InsertPersonasWeb = new InsertPersonasWeb();
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(persona),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };


            try
            {
                var next = ConsultaUltimoIngresoSolicitud(persona);

                if (!next)
                {
                    ConsultaConfigConsultaWebApiInsertPerWeb(persona, ref InsertPersonasWeb);
                    if (InsertPersonasWeb != null)
                    {
                        var Nombre = InsertPersonasWeb.nombre.Split(' ');
                        InsertPersonasWeb.PrimerNombre = Nombre[0];
                        InsertPersonasWeb.SegundoNombre = "";
                        if (Nombre.Count() >= 2)
                        {
                            InsertPersonasWeb.SegundoNombre = Nombre[1];
                        }
                        if (Nombre.Count() >= 3)
                        {
                            InsertPersonasWeb.SegundoNombre = InsertPersonasWeb.SegundoNombre + ' ' + Nombre[2];
                        }
                        if (Nombre.Count() >= 4)
                        {
                            InsertPersonasWeb.SegundoNombre = InsertPersonasWeb.SegundoNombre + ' ' + Nombre[3];
                        }
                        InsertPersonasWeb.TelefonoCel = persona.TelefonoCel;
                        InsertPersonasWeb.UsrModifica = persona.Origen;
                        InsertPersonasWeb.IdProducto = 48;
                        InsertPersonasWeb.Correo = persona.Correo;
                        InsertPersonasWeb.SubOrigen = persona.SubOrigen;
                        InserteSolicStartConsult(InsertPersonasWeb, ref persona);
                        persona.UltidSolicitud = persona.IdSolicitud;
                        persona.IdTipoIdentificacion = InsertPersonasWeb.IdTipoIdentificacion;
                        persona.Identificacion = InsertPersonasWeb.Identificacion;
                        //CreddidEndConsul(InsertPersonasWeb, ref persona);
                        persona.PrimerNombre = InsertPersonasWeb.PrimerNombre;
                        persona.SegundoNombre = InsertPersonasWeb.SegundoNombre;
                        persona.PrimerApellido = InsertPersonasWeb.PrimerApellido;
                        persona.SegundoApellido = InsertPersonasWeb.SegundoApellido;
                        persona.MontoMaximo = InsertPersonasWeb.MontoMaximo;
                        persona.Status = InsertPersonasWeb.Status;
                        persona.Respuesta = "ACKCRE00";
                    }
                    else
                    {
                        throw new Exception("No se pudo ingresar la solicitud, el sujeto no existe en el padron");
                    }
                }
                else throw new Exception("No se pudo ingresar la solicitud, el sujeto ya tiene una solicitud ingresado el dia de hoy");
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw ex;
            }
        }

        private static void InserteSolicStartConsult(InsertPersonasWeb insertPersonasWeb, ref InsertPersonasWeb persona)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(insertPersonasWeb),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            insertPersonasWeb.Correo = string.IsNullOrEmpty(insertPersonasWeb.Correo) ? "no tiene" : insertPersonasWeb.Correo;
            insertPersonasWeb.Origen = GlobalClass.Origen_Apps;
            try
            {
                // insertPersonasWeb.URL = "http://localhost:52045//api/TwoFunTwoMe/GuardarPersonaWeb";
                RestClient cliente = new RestClient(insertPersonasWeb.URL); // Dirección web del reporte
                RestRequest request = new RestRequest(); // Clase propia del RestSharp para asignar parámetros de envio.
                                                         //	request.AddHeader("Authorization", buro.Authorization); // Aquí va el token generado para Desyfin
                request.Method = Method.POST;

                request.AddHeader("Accept", "application/json");
                //request.AddHeader("Accept", "application/xml");

                request.AddJsonBody(JsonConvert.SerializeObject(insertPersonasWeb));


                var respuesta = cliente.Execute(request); // Metodo que ejecuta la solicitud.
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK) // Si retorna OK, el reporte fue generado.
                {
                    persona = JsonConvert.DeserializeObject<InsertPersonasWeb>(respuesta.Content);
                }
                else
                {
                    persona.Respuesta = "No se pudo ingresar la solicitud";
                    throw new Exception(persona.Respuesta);
                }

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw ex;
            }
        }

        internal static void ApiSolicitudEndConsul(InsertPersonasWeb insertPersonasWeb, ref DTO_SOLICITUD_VENTAS persona)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in insertPersonasWeb.GetType().GetProperties()
                                       where nodo.GetValue(insertPersonasWeb) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(insertPersonasWeb).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ValidarPIN";

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(insertPersonasWeb),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            try
            {
                var obj = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                var dto_result = new List<DTO_SOLICITUD_VENTAS>();
                if (obj.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_SOLICITUD_VENTAS>>(JsonConvert.SerializeObject(obj.Result.Tables[0]));
                }


                persona = dto_result.ToList().FirstOrDefault();
                if (persona != null)
                    persona.Status = dto_result.Any() ? dto_result.ToList().FirstOrDefault().Status : null;
                else persona = new DTO_SOLICITUD_VENTAS();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
        }
        private static void ConsultaConfigConsultaWebApiInsertPerWeb(InsertPersonasWeb persona, ref InsertPersonasWeb insertPersonasWeb)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(persona),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in persona.GetType().GetProperties()
                                       where nodo.GetValue(persona) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(persona).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultaDatosPersona";


            try
            {
                var obj = DynamicSqlDAO.ExecuterSp<InsertPersonasWeb>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                insertPersonasWeb = obj.ToList().FirstOrDefault();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
        }

        internal static List<MontoCredito> TraeMontoCalcu(MontoCredito montoCredito)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in montoCredito.GetType().GetProperties()
                                       where nodo.GetValue(montoCredito) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(montoCredito).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_TraeMonto";

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(montoCredito),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto_obj = new List<MontoCredito>();
            try
            {
                var obj = DynamicSqlDAO.ExecuterSp<MontoCredito>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_obj = obj.ToList();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return dto_obj;
        }

        internal static List<PlazoCredito> TraePlazoCalcu(PlazoCredito plazoCredito)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in plazoCredito.GetType().GetProperties()
                                       where nodo.GetValue(plazoCredito) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(plazoCredito).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_TraePlazoCredito";

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(plazoCredito),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto_obj = new List<PlazoCredito>();
            try
            {
                var obj = DynamicSqlDAO.ExecuterSp<PlazoCredito>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_obj = obj.ToList();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return dto_obj;
        }


        internal static List<FrecuenciaCredito> TraeFrecuenciaCalcu()
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            FrecuenciaCredito frec = new FrecuenciaCredito();
            dto.Result = null;
            dto.SPName = "usp_TraeFrecuenciaCredito";

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(frec),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto_obj = new List<FrecuenciaCredito>();
            try
            {
                var obj = DynamicSqlDAO.ExecuterSp<FrecuenciaCredito>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_obj = obj.ToList();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return dto_obj;
        }

        internal static List<DTO_SOLICITUD_VENTAS> GuardaProducto(DTO_SOLICITUD_VENTAS solicitudes)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
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
            dto.SPName = "usp_GuardarProducto";

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto_obj = new List<DTO_SOLICITUD_VENTAS>();
            try
            {
                var obj = DynamicSqlDAO.ExecuterSp<DTO_SOLICITUD_VENTAS>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_obj = obj.ToList();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return dto_obj;
        }

        internal static List<InsertPersonasWeb> DescripcionStatusSolicitud(InsertPersonasWeb insertPersonas)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in insertPersonas.GetType().GetProperties()
                                       where nodo.GetValue(insertPersonas) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(insertPersonas).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultaStatus";
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(insertPersonas),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto_obj = new List<InsertPersonasWeb>();
            try
            {
                var obj = DynamicSqlDAO.ExecuterSp<InsertPersonasWeb>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_obj = obj.ToList();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_obj;
        }


        private static bool ConsultaUltimoIngresoSolicitud(InsertPersonasWeb persona)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(persona),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in persona.GetType().GetProperties()
                                       where nodo.GetValue(persona) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(persona).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_ultimo_ingreso_solicitud";


            try
            {
                var obj = DynamicSqlDAO.ExecuterSp<InsertPersonasWeb>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return obj.Any();
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return false;
        }
    }
}
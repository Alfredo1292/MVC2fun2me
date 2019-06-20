using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_Common;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.GestionesCartera;
using System.Linq;
using System.Xml.Linq;

namespace TwoFunTwoMeFintech.Models.Manager
{
    public class ManagerGestionCartera
    {
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };
        public ManagerGestionCartera()
        {
            String connBD = string.Empty;


            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }
        public List<GestionCartera> ConsultaHistoricoGestiones(GestionCartera gestion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(gestion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in gestion.GetType().GetProperties()
                                       where nodo.GetValue(gestion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(gestion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_historico_gestiones_cartera";
            var dto_result = new List<GestionCartera>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCartera>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionCartera> ConsultaPromesasPagos(GestionCartera gestion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(gestion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in gestion.GetType().GetProperties()
                                       where nodo.GetValue(gestion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(gestion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_promesas_pagos";
            var dto_result = new List<GestionCartera>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCartera>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionCartera> ConsultaPagos(GestionCartera gestion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(gestion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in gestion.GetType().GetProperties()
                                       where nodo.GetValue(gestion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(gestion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_historico_pagos";
            var dto_result = new List<GestionCartera>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCartera>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionCuenta> consultaCritoCredito(GestionCuenta GestionCuenta)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(GestionCuenta),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in GestionCuenta.GetType().GetProperties()
                                       where nodo.GetValue(GestionCuenta) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(GestionCuenta).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_GridCredito";
            var dto_result = new List<GestionCuenta>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCuenta>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionPrestamo> consultaGestionPrestamoCliente(GestionPrestamo GestionCuenta)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(GestionCuenta),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in GestionCuenta.GetType().GetProperties()
                                       where nodo.GetValue(GestionCuenta) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(GestionCuenta).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_GestionPrestamo_cliente";
            var dto_result = new List<GestionPrestamo>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionPrestamo>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public GestionCartera calculaCuota(GestionCartera solicitudes)
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
            dto.SPName = "usp_CalculaMontoMoraMontoCancela";
            var dto_result = new List<GestionCartera>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCartera>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result.FirstOrDefault();
        }
        public List<GestionContacto> referenciasPersonales(GestionContacto GestionContacto)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(GestionContacto),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in GestionContacto.GetType().GetProperties()
                                       where nodo.GetValue(GestionContacto) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(GestionContacto).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_referencias_personales";
            var dto_result = new List<GestionContacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionContacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public List<GestionContacto> ConsultarContactos(GestionContacto contacs)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(contacs),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in contacs.GetType().GetProperties()
                                       where nodo.GetValue(contacs) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(contacs).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_contactos";
            var dto_result = new List<GestionContacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionContacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)

            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public List<GestionPersona> consultaCreditoParientes(GestionPersona personas)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(personas),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in personas.GetType().GetProperties()
                                       where nodo.GetValue(personas) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(personas).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_credito_parientes";
            var dto_result = new List<GestionPersona>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionPersona>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public List<GestionPrestamo> consultaPrestamosCliente(GestionPrestamo prestamo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(prestamo),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in prestamo.GetType().GetProperties()
                                       where nodo.GetValue(prestamo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(prestamo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_prestamos_cliente";
            var dto_result = new List<GestionPrestamo>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionPrestamo>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public List<GestionConsulta> ObtenerEncabezado(GestionBucket roles)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(roles),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in roles.GetType().GetProperties()
                                       where nodo.GetValue(roles) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(roles).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultarCreditos_cobros";
            var dto_result = new List<GestionConsulta>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionConsulta>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionCartera> monstrarAciones()
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
            dto.SPName = "usp_TraeAccion";
            var dto_result = new List<GestionCartera>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCartera>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionCuenta> consultaInformacionCuenta(GestionCuenta informacionCuenta)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(informacionCuenta),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in informacionCuenta.GetType().GetProperties()
                                       where nodo.GetValue(informacionCuenta) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(informacionCuenta).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_informacion_cuenta";
            var dto_result = new List<GestionCuenta>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCuenta>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public List<GestionPersona> correosPersona(GestionPersona personas)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(personas),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in personas.GetType().GetProperties()
                                       where nodo.GetValue(personas) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(personas).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_correo_persona";
            var dto_result = new List<GestionPersona>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionPersona>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public List<GestionContacto> ConsultarDirecciones(GestionContacto contacs)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(contacs),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in contacs.GetType().GetProperties()
                                       where nodo.GetValue(contacs) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(contacs).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_contactos_direcciones";
            var dto_result = new List<GestionContacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionContacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public List<GestionPlanPlagos> ConsultaPlanPagos(GestionPlanPlagos plan)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(plan),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in plan.GetType().GetProperties()
                                       where nodo.GetValue(plan) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(plan).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_calcula_plan_pago";
            var dto_result = new List<GestionPlanPlagos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionPlanPlagos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public Credid MostrarImagenesCreddid(GestionSolicitudes solicitudes)
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
            dto.SPName = "usp_consultar_xml_buros";

            var dto_result = new Credid();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    var result = JsonConvert.DeserializeObject<List<BurosXml>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));

                    XDocument dox = XDocument.Parse(result.FirstOrDefault().CREDDIT);

                    dto_result.Fotografia = dox.Descendants().Where(n => n.Name == "Fotografia").Select(x => new Credid { Fotografia = x.Value }).FirstOrDefault().Fotografia;

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
        public List<GestionConsulta> BusquedaNombre(GestionBucket roles)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(roles),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in roles.GetType().GetProperties()
                                       where nodo.GetValue(roles) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(roles).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultarCreditos_Nombre";
            var dto_result = new List<GestionConsulta>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionConsulta>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionAccion> EjecutarAccion(GestionAccion gestion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(gestion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in gestion.GetType().GetProperties()
                                       where nodo.GetValue(gestion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(gestion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_FuncionAdministrarCredito";
            var dto_result = new List<GestionAccion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionAccion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionCartera> ConsultaGestionesPendientes()
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
            dto.SPName = "usp_consulta_gestiones_cartera_pendientes";
            var dto_result = new List<GestionCartera>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCartera>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<GestionAccion> AprobarGestion(GestionCartera gestion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(gestion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in gestion.GetType().GetProperties()
                                       where nodo.GetValue(gestion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(gestion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_AprobarGestionCartera";
            var dto_result = new List<GestionAccion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionAccion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<GestionAccion> RechazarGestion(GestionCartera gestion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(gestion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in gestion.GetType().GetProperties()
                                       where nodo.GetValue(gestion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(gestion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_RechazarGestionCartera";
            var dto_result = new List<GestionAccion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionAccion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
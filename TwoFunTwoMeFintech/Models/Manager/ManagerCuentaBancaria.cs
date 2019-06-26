using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_Common;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.Colocacion;
namespace TwoFunTwoMeFintech.Models.Manager
{
    public class ManagerCuentaBancaria
    {
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };

        public ManagerCuentaBancaria()
        {
            String connBD = string.Empty;


            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }

        public List<CuentaBancaria> uspConsultaCuentasBancarias(CuentaBancaria cuentaBancaria)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(cuentaBancaria),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            try
            {
                dto.ParameterList.AddRange(from nodo in cuentaBancaria.GetType().GetProperties()
                                           where nodo.GetValue(cuentaBancaria) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(cuentaBancaria).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_ConsultarPersonasCuentas_Ventas_Fenix";


                return DynamicSqlDAO.ExecuterSp<CuentaBancaria>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value).ToList();

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<CuentaBancaria>();
        }

        public List<CuentaBancaria> uspConsultaEditCuentaBancaria(CuentaBancaria cuentaBancaria)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(cuentaBancaria),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            try
            {
                dto.ParameterList.AddRange(from nodo in cuentaBancaria.GetType().GetProperties()
                                           where nodo.GetValue(cuentaBancaria) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(cuentaBancaria).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_ConsultarPersonasCuenta_Edit_Ventas_Fenix";


                return DynamicSqlDAO.ExecuterSp<CuentaBancaria>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value).ToList();

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<CuentaBancaria>();
        }
        #region Trae Bancos
        public List<Bancos> Trae_Bancos()
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
            dto.SPName = "usp_Trae_Bancos";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<Bancos>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        #endregion Trae Bancos

        #region Trae Tipo Cuenta
        public List<TipoCuentas> TraeTipoCuenta()
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
            dto.SPName = "usp_TraeTipoCuenta";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<TipoCuentas>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        #endregion Trae Tipo Cuenta

        #region Trae Tipo Moneda
        public List<TipoMoneda> TraeTipoMoneda()
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
            dto.SPName = "usp_TraeTipoMoneda";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<TipoMoneda>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        #endregion Trae Tipo Cuenta

        #region Mantenimiento Cuentas Bancarias
        public List<CuentaBancaria> InsertaActualizaEliminaCuentaBancaria(CuentaBancaria cuentaBancaria)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(cuentaBancaria),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            try
            {
                dto.ParameterList.AddRange(from nodo in cuentaBancaria.GetType().GetProperties()
                                           where nodo.GetValue(cuentaBancaria) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(cuentaBancaria).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_InsertaActualizaEliminaCuentaBancaria";


                return DynamicSqlDAO.ExecuterSp<CuentaBancaria>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value).ToList();

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<CuentaBancaria>();
        }
        #endregion Mantenimiento Cuentas Bancarias

        #region CalcularCuentaSinpe
        public List<CuentaBancaria> CalcularCuentaSinpe(CuentaBancaria cuentabancaria)
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

            dto.ParameterList.AddRange(from nodo in cuentabancaria.GetType().GetProperties()
                                       where nodo.GetValue(cuentabancaria) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(cuentabancaria).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_CalculaCuentaSinpe";
            var dto_result = new List<CuentaBancaria>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<CuentaBancaria>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                cuentabancaria.Respuesta = ex.Message.Split('|')[1].Split(',')[1];
                dto_result.Add(cuentabancaria);
                //throw;
            }
            return dto_result;
        }

        #endregion CalcularCuentaSinpe

        #region ValidarCuentaSinpe
        public bool ValidarCuentaSinpe(CuentaBancaria cuentabancaria)
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

            dto.ParameterList.AddRange(from nodo in cuentabancaria.GetType().GetProperties()
                                       where nodo.GetValue(cuentabancaria) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(cuentabancaria).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ValidaSinpe";
            var dto_result = new List<CuentaBancaria>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<CuentaBancaria>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                cuentabancaria.Respuesta = "ERR";
                dto_result.Add(cuentabancaria);
                //throw;
            }
            return dto_result.Any()? dto_result.FirstOrDefault().result:false;
        }

        #endregion ValidarCuentaSinpe
    }
}
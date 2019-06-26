using System;
using System.Configuration;
using System.Reflection;
using TwoFunTwoMe.Models.DTO;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_DataAccess;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using TwoFunTwoMe_Common;
using System.Collections.Generic;

namespace TwoFunTwoMe.Models.Manager
{
    public class Manager
    {


        private MQRabbit mqRabbit = new MQRabbit();

        string connBD = string.Empty;
        #region [-------------------------------------CONEXION-------------------------------------]

        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };
        private object personasWeb;

        public Manager()
        {
            String connBD = string.Empty;


            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["TwoFunTwoMeConnection"].ConnectionString);
            }
        }

        #endregion [-------------------------------------CONEXION-------------------------------------]

        #region GuardarPersona

        public InsertPersonasWeb GuardarPersona(InsertPersonasWeb personasWeb)
        {
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
                STR_PARAMETROS = JsonConvert.SerializeObject(personasWeb),
                FEC_CREACION = DateTime.Now
            };
            try
            {
                mqRabbit = new MQRabbit();

                mqRabbit.insertaColaRabbit(personasWeb);
                personasWeb.Status = "SOK1";
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                personasWeb.Status = "ERR";
                throw;
            }
            return personasWeb;
        }

        #endregion GuardarPersona

        #region Pasos
        #region GuardarPaso0
        public List<Pasos_credito> Inserta_Paso0(Pasos_credito pasos_Credito)
        {
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
                STR_PARAMETROS = JsonConvert.SerializeObject(pasos_Credito),
                FEC_CREACION=DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos_Credito.GetType().GetProperties()
                                       where nodo.GetValue(pasos_Credito) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos_Credito).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_13";
            var dto_result = new List<Pasos_credito>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Pasos_credito>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

        #endregion GuardarPaso0
        public void Inserta_Paso_1(Pasos_credito pasos)
        {
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
                FEC_CREACION=DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_1";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }

        public void Inserta_Paso_2(Pasos_credito pasos)
        {
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
                FEC_CREACION=DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_2";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        public void Inserta_Paso_3(Pasos_credito pasos)
        {
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
                FEC_CREACION=DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_3";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        public void Inserta_Paso_4(Pasos_credito pasos)
        {
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
                FEC_CREACION=DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_4";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        public void Inserta_Paso_5(Pasos_credito pasos)
        {
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
                FEC_CREACION=DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_5";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }

        public void Inserta_Paso_6(Pasos_credito pasos)
        {
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
                FEC_CREACION=DateTime.Now
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_6";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        public void Inserta_Paso_7(Pasos_credito pasos)
        {
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

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_7";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        public void Inserta_Paso_8(Pasos_credito pasos)
        {
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

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_8";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        public void Inserta_Paso_9(Pasos_credito pasos)
        {
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

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_9";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }

        public void Inserta_Paso_10(Pasos_credito pasos)
        {
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

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_10";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }

        public void Inserta_Paso_11(Pasos_credito pasos)
        {
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

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_11";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }


        public void Inserta_Paso_12(Pasos_credito pasos)
        {
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

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_12";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }

        public void Inserta_Paso_14(Pasos_credito pasos)
        {
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

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_14";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }

        public void Inserta_Paso_15(Pasos_credito pasos)
        {
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

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in pasos.GetType().GetProperties()
                                       where nodo.GetValue(pasos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(pasos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_inserta_paso_15";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        #endregion

        #region  MetodoGuardaFotoCedula

        public List<Solicitudes> MetodoGuardaFoto(Solicitudes solicitudes)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
                FEC_CREACION = DateTime.Now,
                STR_PARAMETROS = JsonConvert.SerializeObject(solicitudes)
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
            dto.SPName = "usp_MetodoGuardaFotoCedulaSelfie";
            var dto_result = new List<Solicitudes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                throw;

            }
            return dto_result;
        }

        #endregion MetodoGuardaFotoCedula

        #region  usp_InsertaCuentaBancaria

        public List<CuentaBancaria> usp_InsertaCuentaBancaria(CuentaBancaria cuentabancaria)
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
            dto.SPName = "usp_InsertaCuentaBancaria";
            var dto_result = new List<CuentaBancaria>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<CuentaBancaria>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                cuentabancaria.Mensaje = "ERR";
                throw;
            }
            return dto_result;
        }

        #endregion usp_InsertaCuentaBancaria

        #region  ValidaPIN

        public List<Solicitudes> usp_ValidarPIN(Solicitudes solicitudes)
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
            dto.SPName = "usp_ValidarPIN";
            var dto_result = new List<Solicitudes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                throw;
            }
            return dto_result;
        }

        #endregion ValidaPIN

        #region  GeneraNuevoPIN

        public List<Solicitudes> usp_NuevoPIN(Solicitudes solicitudes)
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
            dto.SPName = "usp_ReenviaPIN";
            var dto_result = new List<Solicitudes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                throw;
            }
            return dto_result;
        }

        #endregion GeneraNuevoPIN

        #region  ConsultarAccessKeyAWS

        public List<ConfigSys> ConsultarAccessKeyAWS()
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
            dto.SPName = "usp_consultaaccesskey";
            var dto_result = new List<ConfigSys>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ConfigSys>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result[0].Mensaje = "ERR";
                throw;
            }
            return dto_result;
        }

        #endregion ConsultarAccessKeyAWS

        #region ConsultarSolicitudWebApi
        public List<Solicitudes> ConsultarSolicitudWebApi(Solicitudes solicitudes)
        {
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
            dto.SPName = "usp_ConsultarSolicitudAPI";
            var dto_result = new List<Solicitudes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }

        #endregion ConsultarSolicitudWebApi

        #region ConsultaConfiUrlImagen
        public List<ConfigSys> ConsultaConfiUrlImagen(Tab_ConfigSys configSys)
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
            var dto_result = new List<ConfigSys>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ConfigSys>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                configSys.Mensaje = "ERR";
                throw;

            }
            return dto_result;
        }

        #endregion ConsultaConfiUrlImagen

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
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                cuentabancaria.Mensaje = "ERR";
                throw;
            }
            return dto_result.FirstOrDefault().result;
        }

        #endregion ValidarCuentaSinpe

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
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                cuentabancaria.Mensaje = "ERR";
                throw;
            }
            return dto_result;
        }

        #endregion CalcularCuentaSinpe

        #region  usp_GuardaCompareFaceInfo

        public List<Solicitudes> usp_GuardaCompareFaceInfo(Solicitudes solicitudes)
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
            dto.SPName = "usp_GuardaCompareFaceInfo";
            var dto_result = new List<Solicitudes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                solicitudes.Mensaje = "ERR";
                throw;
            }
            return dto_result;
        }

        #endregion usp_GuardaCompareFaceInfo

        #region  GuardaReferenciaLaboral

        public List<ReferenciaLaboral> GuardaReferenciaLaboral(ReferenciaLaboral referenciaLaboral)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();

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

            dto.ParameterList.AddRange(from nodo in referenciaLaboral.GetType().GetProperties()
                                       where nodo.GetValue(referenciaLaboral) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(referenciaLaboral).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_GuardaReferenciaLaboral";
            var dto_result = new List<ReferenciaLaboral>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ReferenciaLaboral>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                referenciaLaboral.Mensaje = "ERR";
                throw;
            }
            return dto_result;
        }

        #endregion GuardaReferenciaLaboral

        #region Configuracion Envio Correos
        public List<Tab_ConfigSys> CargarConfiguracionEnvioCorreos(Tab_ConfigSys login)
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

            dto.ParameterList.AddRange(from nodo in login.GetType().GetProperties()
                                       where nodo.GetValue(login) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(login).ToString()
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
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

        public List<Solicitudes> CargarDatosCredito(Solicitudes solicitudes)
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
            var dto_result = new List<Solicitudes>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

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
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

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

            DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            return JsonConvert.DeserializeObject<List<PagareContrato>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
        }

        #endregion

        #region PASOS CEDULA IDENTIFICACION
        public void GuardaIdentificacion(Pasos_credito solicitudes)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();

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
            dto.SPName = "usp_pasos_creditos_identificacion";

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                throw;
            }
        }
		#endregion

		#region Consulta Padron

		public List<Padron> ConsultaPadron(Padron padron)
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

			dto.ParameterList.AddRange(from nodo in padron.GetType().GetProperties()
									   where nodo.GetValue(padron) != null
									   select new SpParameter
									   {
										   Name = nodo.Name,
										   Value = nodo.GetValue(padron).ToString()
									   }
				);
			dto.Result = null;
			dto.SPName = "usp_ConsultaPadron";
			var dto_result = new List<Padron>();

			try
			{
				var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
				if (objRet.HasResult)
				{
					dto_result = JsonConvert.DeserializeObject<List<Padron>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
					//dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
				}
			}
			catch (Exception ex)
			{
				dto_excepcion.STR_MENSAJE = ex.Message;
				DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
				padron.Mensaje = "ERR";
				throw;
			}
			return dto_result;
		}

		#endregion Consulta Padron


		#region  Insertar_TraceEventosLOOP

		public List<TraceLoop> Insertar_TraceEventosLOOP(TraceLoop Loop)
		{
			string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
			string xProceso = MethodBase.GetCurrentMethod().Name;
			string strHostName = System.Net.Dns.GetHostName();

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

			dto.ParameterList.AddRange(from nodo in Loop.GetType().GetProperties()
									   where nodo.GetValue(Loop) != null
									   select new SpParameter
									   {
										   Name = nodo.Name,
										   Value = nodo.GetValue(Loop).ToString()
									   }
				);
			dto.Result = null;
			dto.SPName = "usp_Insertar_TraceEventosLOOP";
			var dto_result = new List<TraceLoop>();

			try
			{
				var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
				if (objRet.HasResult)
				{
					dto_result = JsonConvert.DeserializeObject<List<TraceLoop>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
					dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
				}
			}
			catch (Exception ex)
			{
				dto_excepcion.STR_MENSAJE = ex.Message;
				DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
				Loop.Mensaje = "ERR";
				throw;
			}
			return dto_result;
		}

		#endregion GuardaReferenciaLaboral

	}
}
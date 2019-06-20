using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_Common;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.Colocacion;
using TwoFunTwoMeFintech.Models.DTO.GestionesCartera;
using TwoFunTwoMeFintech.Models.DTO.ProductosMantenimiento;

namespace TwoFunTwoMe.Models.Manager
{
    public class ManagerUser
    {
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };

        public ManagerUser()
        {
            String connBD = string.Empty;


            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }

        public void actualizarCalificacion(contacto data) {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(data),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.Add(new SpParameter
            {
                Name = "idTelefono",
                Value = data.IdTelefono.ToString()
            });
            dto.ParameterList.Add(new SpParameter
            {
                Name = "calificacion",
                Value = data.calificacion.ToString()
            });
            dto.Result = null;
            dto.SPName = "usp_Actualiza_calificacion_telefono";
            var dto_result = new List<contacto>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<dto_login> Login(dto_login login)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(login),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto
            {
                ParameterList = new List<SpParameter>
                {
                    new SpParameter
                    {
                        Name = "CLAVE",
                        Value = login.pass
                    },
                      new SpParameter
                    {
                        Name = "CORREO",
                        Value = login.correo
                    },
                      new SpParameter
                    {
                        Name = "COD_AGENTE",
                        Value = login.cod_agente
                    }
                },
                Result = null,
                SPName = "usp_traeAgente"
            };
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<dto_login>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<dto_login>();
        }

        public List<dto_login> LoginRol(dto_login login)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(login),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto
            {
                ParameterList = new List<SpParameter>
                {
                    new SpParameter
                    {
                        Name = "CLAVE",
                        Value = login.pass
                    },
                      new SpParameter
                    {
                        Name = "CORREO",
                        Value = login.correo
                    },
                      new SpParameter
                    {
                        Name = "COD_AGENTE",
                        Value = login.cod_agente
                    }
                },
                Result = null,
                SPName = "usp_traeAgenteRol"
            };
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<dto_login>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<dto_login>();
        }

        public List<dto_Configuracion> AsignaConfiguracion()
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
            dto.SPName = "usp_Obtiene_ConfigBucket";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<dto_Configuracion>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<dto_Configuracion>();
        }
        public List<dto_AsignacionBuckets> AsignaBucket()
        {
            //var dto = new DynamicDto
            //{
            //    ParameterList = new List<SpParameter>
            //    {
            //        new SpParameter
            //        {
            //            Name = "cod_agente",
            //            Value = ""
            //        }
            //    },
            //    Result = null,
            //    SPName = "usp_traeBucket"
            //};

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
            dto.SPName = "usp_traeBucket";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<dto_AsignacionBuckets>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<dto_AsignacionBuckets>();
        }


        public List<dto_CantidadBuckets> DetalleBucket()
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

            var dto = new DynamicDto
            {
                ParameterList = new List<SpParameter>
                {
                    new SpParameter
                    {
                        Name = "Bandera",
                        Value = "1"
                    }
                },
                Result = null,
                SPName = "usp_CargaBuckets"
            };


            //var dto = new DynamicDto();
            //dto.ParameterList = new List<SpParameter>();
            //dto.Result = null;
            //dto.SPName = "usp_CargaBuckets";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<dto_CantidadBuckets>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<dto_CantidadBuckets>();
        }

        public List<dto_CantidadBucketsNoAsignado> DetalleBucket_NoAsignado()
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
            var dto = new DynamicDto
            {
                ParameterList = new List<SpParameter>
                {
                    new SpParameter
                    {
                        Name = "Bandera",
                        Value = "2"
                    }
                },
                Result = null,
                SPName = "usp_CargaBuckets"
            };


            //var dto = new DynamicDto();
            //dto.ParameterList = new List<SpParameter>();
            //dto.Result = null;
            //dto.SPName = "usp_CargaBuckets";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<dto_CantidadBucketsNoAsignado>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return new List<dto_CantidadBucketsNoAsignado>();
        }
        public int Registrar(dto_login login)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(login),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto
            {
                ParameterList = new List<SpParameter>
                {
                    new SpParameter
                    {
                        Name = "cod_agente",
                        Value = login.cod_agente
                    },
                      new SpParameter
                    {
                        Name = "nombre",
                        Value = login.nombre
                    },
                      new SpParameter
                    {
                        Name = "pass",
                        Value = login.pass
                    },
                        new SpParameter
                    {
                        Name = "STR_USUARIO_AD",
                        Value = login.STR_USUARIO_AD
                    },
                      new SpParameter
                    {
                        Name = "correo",
                        Value = login.correo
                    },
                      new SpParameter
                    {
                        Name = "estado",
                        Value = login.estado
                    },
                       new SpParameter
                    {
                        Name = "ROLID",
                        Value = login.ROLID.ToString()
                    },
                       new SpParameter
                    {
                        Name = "TipoCola",
                        Value = login.TipoCola.ToString()
                    },
                       new SpParameter
                    {
                        Name = "IdAgencia",
                        Value = login.IdAgencia.ToString()
                    }
                },
                Result = null,
                SPName = "usp_inserta_agente"
            };
            var retorno = 0;
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                if (!ds.HasResult)
                {
                    retorno = 1;

                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return retorno;
        }

        public List<MenuModels> mostrarMenu(string agente)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(agente),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto
            {
                ParameterList = new List<SpParameter>
                {
                    new SpParameter
                    {
                        Name = "cod_agente",
                        Value = agente
                    }
                },
                Result = null,
                SPName = "usp_consultarMenu"
            };
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<MenuModels>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return new List<MenuModels>();
        }

        public int UpdateBuckets(dto_login login)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(login),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            login.correo = string.Empty;
            login.ROLID = null;
            login.nombre = string.Empty;

            dto.ParameterList.AddRange(from nodo in login.GetType().GetProperties()
                                       where nodo.GetValue(login) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(login).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ActualizarAgente";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return 1;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return -1;

            }
        }

        public List<Roles> GetUserRoles()
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
            var dto_result = new List<Roles>();
            dto.Result = null;
            dto.SPName = "usp_consulta_roles";

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Roles>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<AsignacionBuckets> GetAsignacionBuckets(int? id = null)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(id),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            var parameter = new SpParameter();
            parameter.Name = "id";
            parameter.Value = id.GetValueOrDefault().ToString();

            dto.ParameterList.Add(parameter);
            dto.Result = null;
            dto.SPName = "usp_ConsultaAsignacionBuckets";
            var dto_result = new List<AsignacionBuckets>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<AsignacionBuckets>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public bool ActualizarBuckets(AsignacionBuckets buckets)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(buckets),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in buckets.GetType().GetProperties()
                                       where nodo.GetValue(buckets) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(buckets).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ActualizarBuckets";

            var dto_result = new AsignacionBuckets();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.Respuesta = true;
            }
            catch (Exception ex)
            {
                dto_result.MensajeError = "Ocurrio un Error";
                dto_result.Respuesta = false;
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return dto_result.Respuesta;
        }

        public bool EliminaBuckets(AsignacionBuckets buckets)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(buckets),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in buckets.GetType().GetProperties()
                                       where nodo.GetValue(buckets) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(buckets).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_EliminaBuckets";

            var dto_result = new AsignacionBuckets();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.Respuesta = true;
                dto_result.Respuesta = true;
            }
            catch (Exception ex)
            {
                dto_result.MensajeError = "Ocurrio un Error";
                dto_result.Respuesta = false;
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return dto_result.Respuesta;
        }

        public List<Solicitudes> CargarSolicitudBuro(Solicitudes solicitudes)
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
            dto.SPName = "usp_cargaSolicitudBuro";
            var dto_result = new List<Solicitudes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (ArgumentException ex)
            {
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return dto_result;
        }

        public List<productos> CargarProductos(Solicitudes solicitudes)
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
            dto.ParameterList.AddRange(from nodo in solicitudes.GetType().GetProperties()
                                       where nodo.GetValue(solicitudes) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(solicitudes).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_TraeProductos";
            var dto_result = new List<productos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<productos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
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
        public List<Tipos> CargarTipos(string tipo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tipo),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            var parameter = new SpParameter();
            parameter.Name = "Id";
            parameter.Value = tipo;

            dto.ParameterList.Add(parameter);
            dto.Result = null;
            dto.SPName = "usp_TraeStatusTipos";
            var dto_result = new List<Tipos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tipos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
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

        public Solicitudes ActualizaSolicitudCredito(Solicitudes solicitudes)
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
            dto.SPName = "usp_actualiza_limite_solicitud";
            var dto_result = new Solicitudes();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.Mensaje = string.Concat("La solicitud N# ", solicitudes.Id, " se actualizo");
            }
            catch (Exception ex)
            {
                dto_result.MensajeError = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;
        }

        public BurosXml DescargarXmlBuro(Solicitudes solicitudes)
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
            var dto_result = new List<BurosXml>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {

                    dto_result = JsonConvert.DeserializeObject<List<BurosXml>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().MIMETYPE = @"text/xml";

                    var tempTUCA = dto_result.FirstOrDefault().TUCA;
                    var tempCREDID = dto_result.FirstOrDefault().CREDDIT;
                    var tempEQUIFAX = dto_result.FirstOrDefault().EQUIFAX;
                    var tempGINI = dto_result.FirstOrDefault().GINI;
                    var tempCREDISERVER = dto_result.FirstOrDefault().CREDISERVER;

                    dto_result.FirstOrDefault().NOMBRETUCA = String.Concat(solicitudes.Identificacion, "_Tuca_ContenidoXML.xml");
                    dto_result.FirstOrDefault().NOMBRECREDDIT = String.Concat(solicitudes.Identificacion, "_CREDDID_ContenidoXMLL.xml");
                    dto_result.FirstOrDefault().NOMBREEQUIFAX = String.Concat(solicitudes.Identificacion, "_EQUIFAX_ContenidoXML.xml");
                    dto_result.FirstOrDefault().NOMBREGINI = String.Concat(solicitudes.Identificacion, "_GINI_ContenidoXML.Json");
                    dto_result.FirstOrDefault().NOMBRECREDISERVER = String.Concat(solicitudes.Identificacion, "_CREDISERVER_ContenidoXML.xml");
                    if (tempTUCA != null)
                    {
                        dto_result.FirstOrDefault().DATATUCA = string.Format("data:{0};base64,{1}", dto_result.FirstOrDefault().MIMETYPE, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto_result.FirstOrDefault().TUCA)));
                    }
                    if (tempCREDID != null)
                    {
                        dto_result.FirstOrDefault().DATACREDDIT = string.Format("data:{0};base64,{1}", dto_result.FirstOrDefault().MIMETYPE, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto_result.FirstOrDefault().CREDDIT)));
                    }
                    if (tempEQUIFAX != null)
                    {
                        dto_result.FirstOrDefault().DATAEQUIFAX = string.Format("data:{0};base64,{1}", dto_result.FirstOrDefault().MIMETYPE, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto_result.FirstOrDefault().EQUIFAX)));
                    }
                    if (tempGINI != null)
                    {
                        dto_result.FirstOrDefault().DATAGINI = string.Format("data:{0};base64,{1}", dto_result.FirstOrDefault().MIMETYPE, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto_result.FirstOrDefault().GINI)));
                    }
                    if (tempCREDISERVER != null)
                    {
                        dto_result.FirstOrDefault().DATACREDISERVER = string.Format("data:{0};base64,{1}", dto_result.FirstOrDefault().MIMETYPE, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto_result.FirstOrDefault().CREDISERVER)));
                    }
                }
                /*
                  System.Text.Encoding.UTF8.GetBytes(plainText);
  return System.Convert.ToBase64String(plainTextBytes);
                 */
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result.FirstOrDefault();
        }
        public AsignacionBuckets InsertarBuckets(AsignacionBuckets buckets)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(buckets),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in buckets.GetType().GetProperties()
                                       where nodo.GetValue(buckets) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(buckets).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_insertaBuckets";
            var dto_result = new AsignacionBuckets();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<AsignacionBuckets>>(JsonConvert.SerializeObject(objRet.Result.Tables[0])).FirstOrDefault();
                    dto_result.Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.id);
                }
            }
            catch (Exception ex)
            {
                dto_result.MensajeError = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }
        public List<Solicitudes> ConsultaSolicitudes(string Identificacion = "")
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(Identificacion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            var parameter = new SpParameter();
            parameter.Name = "Identificacion";
            parameter.Value = Identificacion;

            dto.ParameterList.Add(parameter);
            dto.Result = null;
            dto.SPName = "usp_consultar_Solicitudes";
            var dto_result = new List<Solicitudes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<dto_login> ConsultaUsuarios(dto_login login)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(login),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            var parameter = new SpParameter();
            parameter.Name = "COD_AGENTE";
            parameter.Value = login.cod_agente;
            dto.ParameterList.Add(parameter);

            parameter = new SpParameter();
            parameter.Name = "CLAVE";
            parameter.Value = login.pass;

            dto.ParameterList.Add(parameter);
            dto.Result = null;
            dto.SPName = "usp_traeAgente";
            var dto_result = new List<dto_login>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<dto_login>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<dto_login> ActualizaUsuarios(dto_login login)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(login),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            var parameter = new SpParameter();
            parameter.Name = "COD_AGENTE";
            parameter.Value = login.cod_agente;
            dto.ParameterList.Add(parameter);

            parameter = new SpParameter();
            parameter.Name = "CLAVE";
            parameter.Value = login.pass;

            dto.ParameterList.Add(parameter);
            dto.Result = null;
            dto.SPName = "usp_traeAgente";
            var dto_result = new List<dto_login>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<dto_login>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        #region ScoreCardExperto
        public List<ScoreCardExperto> ConsultaScoreCardExperto(ScoreCardExperto SCE)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(SCE),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in SCE.GetType().GetProperties()
                                       where nodo.GetValue(SCE) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(SCE).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consultar_ScoreCardExperto";
            var dto_result = new List<ScoreCardExperto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ScoreCardExperto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }

            }
            catch (Exception ex)
            {
                var err = new ScoreCardExperto
                {
                    Mensaje = "Ocurrio un Error"
                };
                dto_result.Add(err);
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return dto_result;
        }
        public int ActualizaScoreCardExperto(ScoreCardExperto SCE)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(SCE),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in SCE.GetType().GetProperties()
                                       where nodo.GetValue(SCE) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(SCE).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_actualizar_uScoreCardExperto";
            var dto_result = 0;

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result = 1;

            }
            catch (Exception ex)
            {
                dto_result = -1;
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;
        }


        public ScoreCardExperto EliminarScoreCardExperto(ScoreCardExperto SCE)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(SCE),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in SCE.GetType().GetProperties()
                                       where nodo.GetValue(SCE) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(SCE).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_eliminar_ScoreCardExperto";


            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                SCE.Mensaje = string.Format("Se elimino el ID N#: {0}", SCE.Id);

            }
            catch (Exception ex)
            {

                SCE.Mensaje = "UPPS, lo ciento, favor contacte con el Administrador";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return SCE;
        }

        public ScoreCardExperto InsertarScoreCardExperto(ScoreCardExperto SCE)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(SCE),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in SCE.GetType().GetProperties()
                                       where nodo.GetValue(SCE) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(SCE).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_insertar_ScoreCardExperto";

            var dto_result = new List<ScoreCardExperto>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ScoreCardExperto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }


                SCE.Mensaje = string.Format("Se creo el la configuración N#: {0}", dto_result.FirstOrDefault().Id);

            }
            catch (Exception ex)
            {

                SCE.Mensaje = "UPPS, lo ciento, favor contacte con el Administrador";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            return SCE;
        }
        #endregion

        #region Rules
        public List<Rules> MantenimientoRules(Rules rules)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(rules),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in rules.GetType().GetProperties()
                                       where nodo.GetValue(rules) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(rules).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_mantenimiento_rules";
            var dto_result = new List<Rules>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Rules>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().IdRule);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                throw;
            }
            return dto_result;
        }

        #endregion
        #region Roles
        public List<Roles> MantenimientoRoles(Roles roles)
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
            dto.SPName = "usp_mantenimiento_roles";
            var dto_result = new List<Roles>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Roles>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                throw;
            }
            return dto_result;
        }

        #endregion

        #region SUBMENU
        public List<clsSUBMENU> MantenimientoSUBMENU(clsSUBMENU submenu)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(submenu),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in submenu.GetType().GetProperties()
                                       where nodo.GetValue(submenu) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(submenu).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_mantenimiento_SUBMENU";
            var dto_result = new List<clsSUBMENU>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<clsSUBMENU>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo el submenú N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

        #endregion

        #region MAINMENU
        public List<clsMAINMENU> MantenimientoMAINMENU(clsMAINMENU mainmenu)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(mainmenu),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in mainmenu.GetType().GetProperties()
                                       where nodo.GetValue(mainmenu) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(mainmenu).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_mantenimiento_MAINMENU";
            var dto_result = new List<clsMAINMENU>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<clsMAINMENU>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo el menú principal N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error:" + ex;
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

        #endregion

        #region Cola Cobros


        public List<BucketCobros> ColaAutomaticaCobros(BucketCobros roles)
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
            dto.SPName = "usp_cola_cobros";
            var dto_result = new List<BucketCobros>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<BucketCobros>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<BucketCobros> ObtenerCobros(BucketCobros roles)
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
            dto.SPName = "usp_ConsultaCreditosCuotas";
            var dto_result = new List<BucketCobros>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<BucketCobros>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<ConsultarCreditos> ObtenerEncabezado(BucketCobros roles)
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
            var dto_result = new List<ConsultarCreditos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ConsultarCreditos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<BucketCobros> ActualizaColaAutomaticaCobros(BucketCobros roles)
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
            dto.SPName = "usp_actualiza_cola_cobros_automaticos";

            var dto_result = new List<BucketCobros>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<BucketCobros>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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


        public List<Reprogramacion> ActualizaReprogramacion(Reprogramacion roles)
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
            dto.SPName = "usp_actualiza_reprogramacion";

            var dto_result = new List<Reprogramacion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Reprogramacion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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


        public List<Tabla_Accion> monstrarAciones()
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
            var dto_result = new List<Tabla_Accion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_Accion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<Tabla_RespuestaGestion> mostrarResultadoLLamada()
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
            dto.SPName = "usp_TraeTabla_RespuestaGestion";
            var dto_result = new List<Tabla_RespuestaGestion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_RespuestaGestion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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


        public void GuardaCobro(GestionCobro cobros)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(cobros),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in cobros.GetType().GetProperties()
                                       where nodo.GetValue(cobros) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(cobros).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_InsertaGestionCobro";

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            catch (Exception ex)
            {
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
        }
        #region Mantenimiento de Contactos
        public List<contacto> ConsultarContactos(contacto contacs)
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
            var dto_result = new List<contacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public contacto ConsultarTelefono(contacto contacs)
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
            dto.SPName = "usp_consulta_TelefonoGeneral";
            var dto_result = new List<contacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result.FirstOrDefault();
        }
        public List<CatalogoTelefono> ConsultarCatalogoTelefono(CatalogoTelefono catalogoTelefono)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogoTelefono),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.Result = null;
            dto.SPName = "usp_consulta_CatalogoTelefono";
            var dto_result = new List<CatalogoTelefono>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<CatalogoTelefono>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public contacto ActualizarTelefono(contacto contacs)
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
            dto.SPName = "usp_Actualiza_Teléfono";
            var dto_result = new List<contacto>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result.FirstOrDefault();
        }
        public contacto CrearDireccion(contacto contacs)
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
            dto.SPName = "usp_Inserta_Dirreccion";
            var dto_result = new List<contacto>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result.FirstOrDefault();
        }
        public contacto EliminaTelefono(contacto contacs)
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
            dto.SPName = "usp_Elimina_Telefono";
            var dto_result = new List<contacto>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result.FirstOrDefault();
        }
        #endregion Mantenimiento de Contactos
        public List<reprogramaciones> ConsultarReprogramaciones(reprogramaciones reprog, string agente)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(reprog),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };



            var dto = new DynamicDto
            {
                ParameterList = new List<SpParameter>
               {
                     new SpParameter
                   {
                       Name = "AgenteAsignado",
                       Value = agente
                   }
               },
                Result = null,
                SPName = "usp_consulta_reprogramaciones"
            };

            dto.ParameterList.AddRange(from nodo in reprog.GetType().GetProperties()
                                       where nodo.GetValue(reprog) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(reprog).ToString()
                                       }
                );

            var dto_result = new List<reprogramaciones>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<reprogramaciones>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<Tabla_Accion> ConsultaHistoricoGestiones(Tabla_Accion tabla_Accion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tabla_Accion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tabla_Accion.GetType().GetProperties()
                                       where nodo.GetValue(tabla_Accion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tabla_Accion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_historico_gestiones";
            var dto_result = new List<Tabla_Accion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_Accion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<Tabla_Accion> ConsultaHistoricoGestionesAutomaticas(Tabla_Accion tabla_Accion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tabla_Accion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tabla_Accion.GetType().GetProperties()
                                       where nodo.GetValue(tabla_Accion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tabla_Accion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_historico_gestiones_SMS";
            var dto_result = new List<Tabla_Accion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_Accion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<Tabla_Accion> ConsultaPromesasPagos(Tabla_Accion tabla_Accion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tabla_Accion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tabla_Accion.GetType().GetProperties()
                                       where nodo.GetValue(tabla_Accion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tabla_Accion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_promesas_pagos";
            var dto_result = new List<Tabla_Accion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_Accion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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


        public List<Tabla_Accion> ConsultaPagos(Tabla_Accion tabla_Accion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tabla_Accion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tabla_Accion.GetType().GetProperties()
                                       where nodo.GetValue(tabla_Accion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tabla_Accion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_historico_pagos";
            var dto_result = new List<Tabla_Accion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_Accion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public Tabla_Accion ConsultaSaldoMontoPendiente(Tabla_Accion tabla_Accion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tabla_Accion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tabla_Accion.GetType().GetProperties()
                                       where nodo.GetValue(tabla_Accion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tabla_Accion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_desboard_monto_cantidad_cola";
            var dto_result = new List<Tabla_Accion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_Accion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public Tabla_Accion ConsultaSaldoMontoProcesado(Tabla_Accion tabla_Accion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tabla_Accion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tabla_Accion.GetType().GetProperties()
                                       where nodo.GetValue(tabla_Accion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tabla_Accion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_desboard_monto_cantidad_cola_procesada";
            var dto_result = new List<Tabla_Accion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_Accion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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


        public List<InformacionCuenta> consultaInformacionCuenta(InformacionCuenta informacionCuenta)
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
            var dto_result = new List<InformacionCuenta>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<InformacionCuenta>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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


        public Credid MostrarImagenesCreddid(Solicitudes solicitudes)
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

        public List<InformacionCuenta> consultaCritoCredito(InformacionCuenta informacionCuenta)
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
            dto.SPName = "usp_GridCredito";
            var dto_result = new List<InformacionCuenta>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<InformacionCuenta>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<Prestamos> consultaPrestamosCliente(Prestamos informacionCuenta)
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
            dto.SPName = "usp_prestamos_cliente";
            var dto_result = new List<Prestamos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Prestamos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<Personas> consultaCreditoParientes(Personas personas)
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
            var dto_result = new List<Personas>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Personas>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public Tabla_Accion calculaCuota(Tabla_Accion solicitudes)
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
            var dto_result = new List<Tabla_Accion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tabla_Accion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<contacto> referenciasPersonales(contacto contacto)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(contacto),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in contacto.GetType().GetProperties()
                                       where nodo.GetValue(contacto) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(contacto).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_referencias_personales";
            var dto_result = new List<contacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<Personas> correosPersona(Personas personas)
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
            var dto_result = new List<Personas>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Personas>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        #endregion


        #region cambio pass
        public void cambioPass(LoginModels login)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(login),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
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
            dto.SPName = "usp_cambio_pass";

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
        }
        #endregion
        #region Agente Menu
        public List<AgenteMenu> ConsultaSubmenusAsignados(AgenteMenu agentemenu)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(agentemenu),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in agentemenu.GetType().GetProperties()
                                       where nodo.GetValue(agentemenu) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(agentemenu).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_AgenteMenuAsignado";
            var dto_result = new List<AgenteMenu>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<AgenteMenu>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error"; dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

        #endregion
        #region Agente Menu
        public List<AgenteMenu> ConsultaSubmenusDesAsignados(AgenteMenu agentemenu)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(agentemenu),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in agentemenu.GetType().GetProperties()
                                       where nodo.GetValue(agentemenu) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(agentemenu).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_AgenteMenuDesAsignado";
            var dto_result = new List<AgenteMenu>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<AgenteMenu>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error"; dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }

        #endregion

        #region Tipo Gestión General
        public List<Tipos> TraeTipoGestionGeneral()
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
            dto.SPName = "usp_TraeTipoGestionGeneral";
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

                return JsonConvert.DeserializeObject<List<Tipos>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<Tipos>();
        }

        public List<Solicitudes> ConsultarSolicitudGestionGeneral(Solicitudes agentemenu)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(agentemenu),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in agentemenu.GetType().GetProperties()
                                       where nodo.GetValue(agentemenu) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(agentemenu).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultarSolicitudGestionGeneral";
            var dto_result = new List<Solicitudes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Solicitudes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().Id);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error"; dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }
        public List<GestionesAprobadas> MantenimientioGestionesAprobadas(GestionesAprobadas gestionesaprobadas)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(gestionesaprobadas),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in gestionesaprobadas.GetType().GetProperties()
                                       where nodo.GetValue(gestionesaprobadas) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(gestionesaprobadas).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_mantenimiento_GestionesAprobadas";
            var dto_result = new List<GestionesAprobadas>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionesAprobadas>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().IdGestionesAprobadas);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error"; dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }
        public List<GestionGeneral> MantenimientioGestionesGeneral(GestionGeneral gestionesgeneral)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(gestionesgeneral),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in gestionesgeneral.GetType().GetProperties()
                                       where nodo.GetValue(gestionesgeneral) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(gestionesgeneral).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_mantenimiento_GestionesGeneral";
            var dto_result = new List<GestionGeneral>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionGeneral>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().IdGestionGeneral);
                }
            }
            catch (Exception ex)
            {
                dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error"; dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }
        #endregion

        #region ConsultaTranferencias
        public List<Transferencias> ConsultarTransferencias(Transferencias transferencias)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(transferencias),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in transferencias.GetType().GetProperties()
                                       where nodo.GetValue(transferencias) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(transferencias).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_transferencias";
            var dto_result = new List<Transferencias>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Transferencias>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<Transferencias> ConsultarTotalTransferencias(Transferencias transferencias)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(transferencias),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in transferencias.GetType().GetProperties()
                                       where nodo.GetValue(transferencias) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(transferencias).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_tota_transferencia_bancos";
            var dto_result = new List<Transferencias>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Transferencias>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<Tab_ConfigSys> GetConfig(Tab_ConfigSys config)
        {

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(config),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
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
            dto.SPName = "usp_consulta_configuracion_general";
            var dto_result = new List<Tab_ConfigSys>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tab_ConfigSys>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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


        public List<Catalogo_AsignacionBuckets> GetAsignados(Catalogo_AsignacionBuckets catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = ipAddress.ToString(),
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_conf_Bucket_asignados";
            var dto_result = new List<Catalogo_AsignacionBuckets>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Catalogo_AsignacionBuckets>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<Catalogo_AsignacionBuckets_conf> GetAsignados_conf(Catalogo_AsignacionBuckets_conf catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = ipAddress.ToString(),
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_conf_Bucket_asignados_conf";
            var dto_result = new List<Catalogo_AsignacionBuckets_conf>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Catalogo_AsignacionBuckets_conf>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<Catalogo_AsignacionBuckets> GetNoAsignados(Catalogo_AsignacionBuckets catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = strHostName,
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_conf_Bucket_noasignados";
            var dto_result = new List<Catalogo_AsignacionBuckets>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Catalogo_AsignacionBuckets>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<Catalogo_AsignacionBuckets_conf> GetNoAsignados_conf(Catalogo_AsignacionBuckets_conf catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = strHostName,
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_conf_Bucket_noasignados_conf";
            var dto_result = new List<Catalogo_AsignacionBuckets_conf>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Catalogo_AsignacionBuckets_conf>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public AsignacionBucketsConfig GuardaConfiguracion(AsignacionBucketsConfig catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = ipAddress.ToString(),
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_guarda_Config_Catalogo_AsignacionBuckets";
            var dto_result = new AsignacionBucketsConfig();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                dto_result.Respuesta = "OK";
            }
            catch (Exception ex)
            {
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.Respuesta = ex.Message;
            }
            return dto_result;
        }
        public AsignacionBucketsConfig GuardaConfiguracion_conf(AsignacionBucketsConfig catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = ipAddress.ToString(),
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_guarda_Config_Catalogo_AsignacionBuckets_conf";
            var dto_result = new AsignacionBucketsConfig();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                dto_result.Respuesta = "OK";
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.Respuesta = ex.Message;
            }
            return dto_result;
        }
        public AsignacionBucketsConfig BorraConfiguracion_conf(AsignacionBucketsConfig catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = ipAddress.ToString(),
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_borrar_Config_Catalogo_AsignacionBuckets_conf";
            var dto_result = new AsignacionBucketsConfig();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                dto_result.Respuesta = "OK";
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.Respuesta = ex.Message;
            }
            return dto_result;

        }

        public AsignacionBucketsConfig BorraConfiguracion(AsignacionBucketsConfig catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = ipAddress.ToString(),
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_borrar_Config_Catalogo_AsignacionBuckets";
            var dto_result = new AsignacionBucketsConfig();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                dto_result.Respuesta = "OK";
            }
            catch (Exception ex)
            {
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";

                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.Respuesta = ex.Message;
            }
            return dto_result;

        }

        public List<Catalogo_AsignacionBuckets> Mantenimiento_Catalogo_AsignacionBucketsn(Catalogo_AsignacionBuckets catalogo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;

            string strHostName = System.Net.Dns.GetHostName();
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName()); <-- Obsolete
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_SERVIDOR = ipAddress.ToString(),
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in catalogo.GetType().GetProperties()
                                       where nodo.GetValue(catalogo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(catalogo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_mantenimiento_Catalogo_AsignacionBuckets";
            var dto_result = new List<Catalogo_AsignacionBuckets>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Catalogo_AsignacionBuckets>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        #endregion

        #region Gestion Cobro Temporal
        //INICIO FCAMACHO 04/12/2018 
        //PROCESO DE GUARDAR EN TABLA LOS DATOS OBTENIDOS PREVIAMENTE ANTES DE DETENER LA COLA
        public void GuardaCobroTemp(GestionCobro cobros)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(cobros),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in cobros.GetType().GetProperties()
                                       where nodo.GetValue(cobros) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(cobros).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_InsertaGestionCobroTemp";

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
        }

        //OBTIENE LA ULTIMA GESTION TEMPORAL PARA UN CLIENTE Y UN RESPECTIVO AGENTE
        public List<GestionCobro> consultaInfoGestionTemp(GestionCobro cobro)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(cobro),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in cobro.GetType().GetProperties()
                                       where nodo.GetValue(cobro) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(cobro).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_informacion_gestion_temporal";
            var dto_result = new List<GestionCobro>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionCobro>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        //FIN FCAMACHO 04/12/2018
        #endregion

        #region PlanPagos
        public List<PlanPlagos> ConsultaPlanPagos(PlanPlagos tabla_Accion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tabla_Accion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tabla_Accion.GetType().GetProperties()
                                       where nodo.GetValue(tabla_Accion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tabla_Accion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_obtener_plan_pagos";
            var dto_result = new List<PlanPlagos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<PlanPlagos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        #endregion

        /*ivan cortes*/
        #region Dashboard
        public List<DashBoard> ConsultarDashBoard(DashBoard dashBoard)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(dashBoard),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in dashBoard.GetType().GetProperties()
                                       where nodo.GetValue(dashBoard) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(dashBoard).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_dashboard_general";
            var dto_result = new List<DashBoard>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DashBoard>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<DashBoardDetallado> ConsultarDashBoardDetallado(DashBoard dashBoard)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(dashBoard),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in dashBoard.GetType().GetProperties()
                                       where nodo.GetValue(dashBoard) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(dashBoard).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_dashboard_detallado";
            var dto_result = new List<DashBoardDetallado>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DashBoardDetallado>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        #endregion


        #region Administrador de cobros
        public List<ConsultarCreditos> ConsultaCobro(ConsultarCreditos dashBoard)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(dashBoard),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in dashBoard.GetType().GetProperties()
                                       where nodo.GetValue(dashBoard) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(dashBoard).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultarCreditos";
            var dto_result = new List<ConsultarCreditos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ConsultarCreditos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<ConsultarCreditos> RealizarAccion(ConsultarCreditos consultarCreditos)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(consultarCreditos),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in consultarCreditos.GetType().GetProperties()
                                       where nodo.GetValue(consultarCreditos) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(consultarCreditos).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultarCreditos";
            var dto_result = new List<ConsultarCreditos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ConsultarCreditos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        #endregion

        //jose abarca
        #region consulta Resultado Score
        public List<Score> ConsultaResultadoScore(Score score)
        {
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in score.GetType().GetProperties()
                                       where (nodo.GetValue(score) != null)
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(score).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultaResultadoScore";
            var dto_result = new List<Score>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Score>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    //dto_result.FirstOrDefault().Mensaje = string.Concat("Se creo Buckets N#: ", dto_result.FirstOrDefault().ID);
                }
            }
            catch
            {
                //dto_result.FirstOrDefault().Mensaje = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }



        #endregion 

        #region consulta Config Reportes
        public List<ConfiguracionReportes> ConsultaConfiguracionReportes(ConfiguracionReportes config)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(config),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in config.GetType().GetProperties()
                                       where (nodo.GetValue(config) != null)
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(config).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultaConfiguracionReportes";
            var dto_result = new List<ConfiguracionReportes>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ConfiguracionReportes>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<ColaReporte> ConsultaColaReportes(ColaReporte config)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(config),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in config.GetType().GetProperties()
                                       where (nodo.GetValue(config) != null)
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(config).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultaColaReportes";
            var dto_result = new List<ColaReporte>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ColaReporte>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public Boolean InsertaColaReportes(ColaReporte nuevoReporte)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(nuevoReporte),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto
            {
                ParameterList = new List<SpParameter>
                {
                    new SpParameter
                    {
                        Name = "idReporte",
                        Value = nuevoReporte.IdReporte.ToString()
                    },
                      new SpParameter
                    {
                        Name = "nombreArchivo",
                        Value = nuevoReporte.NombreArchivo
                    },
                      new SpParameter
                    {
                        Name = "estado",
                        Value = nuevoReporte.Estado
                    },
                        new SpParameter
                    {
                        Name = "usuario",
                        Value = nuevoReporte.UsuarioCreacion
                    },
                      new SpParameter
                    {
                        Name = "FechaCreacion",
                        Value = null
                    },
                      new SpParameter
                    {
                        Name = "FechaModificacion",
                        Value = null
                    },
                },
                Result = null,
                SPName = "usp_InsertarColaReportes"
            };
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (!ds.HasResult)
                {
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return false;
        } // end InsertaColaReportes
        #endregion

        #region Solicitudes Tesoreria

        public List<TesoreriaPendianteTranferencia> consultaTesoreriaPendianteTranferencia(TesoreriaPendianteTranferencia tesoreria)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tesoreria),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in tesoreria.GetType().GetProperties()
                                       where nodo.GetValue(tesoreria) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tesoreria).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_ConsultaSolicitudesStatus";
            var dto_result = new List<TesoreriaPendianteTranferencia>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<TesoreriaPendianteTranferencia>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<AprobadoVerificacionTesoreria> consultaAprobadoVerificacionTesoreria(AprobadoVerificacionTesoreria tesoreria)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tesoreria),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in tesoreria.GetType().GetProperties()
                                       where nodo.GetValue(tesoreria) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tesoreria).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_ConsultaSolicitudesTesoreria";
            var dto_result = new List<AprobadoVerificacionTesoreria>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<AprobadoVerificacionTesoreria>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public string aprobarTesoreriaPendianteTranferencia(int id, string user, string estadoGestion)
        {
            ParametrosAprobarSolicitud parametros = new ParametrosAprobarSolicitud(id, user, 0, 0, "", estadoGestion);
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(parametros),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in parametros.GetType().GetProperties()
                                       where nodo.GetValue(parametros) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(parametros).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_InsertaCredito";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return "!!! La solicitud: " + parametros.Id + " fue aprobada con exito";

            }
            catch (System.Data.SqlClient.SqlException exBd)
            {
                dto_excepcion.STR_MENSAJE = exBd.Message;
                dto_excepcion.STR_DETALLE = exBd.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                string mensaje = "La solicitud no fue cambiada de Status por el siguiente error en la base de datos:\n\n" + exBd.Message.ToString();
                return mensaje;
                throw;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                string mensaje = "La solicitud no fue cambiada de Status por el siguiente error en la aplicacion:\n\n" + ex.Message.ToString();
                return mensaje;
                throw;
            }
        }
        public string verificarCuenta(int id, int status1, int status2, string user)
        {
            ParametrosRechazarSolicitud parametros = new ParametrosRechazarSolicitud(id, status1, status2, user);
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(parametros),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in parametros.GetType().GetProperties()
                                       where nodo.GetValue(parametros) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(parametros).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_RechazaSolicitudesPendienteDocumentos";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return "!!! La solicitud: " + parametros.Id + " fue cambiada a Status: Verificar Cuenta";

            }
            catch (System.Data.SqlClient.SqlException exBd)
            {
                dto_excepcion.STR_MENSAJE = exBd.Message;
                dto_excepcion.STR_DETALLE = exBd.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                string mensaje = "La solicitud no fue cambiada de Status por el siguiente error en la base de datos:\n\n" + exBd.Message.ToString();
                return mensaje;
                throw;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                string mensaje = "La solicitud no fue cambiada de Status por el siguiente error en la aplicacion:\n\n" + ex.Message.ToString();
                return mensaje;
                throw;
            }
        }
        public Boolean pendienteDatosTesoreriaPendianteTranferencia(int id, int status1, int status2, string user)
        {
            ParametrosRechazarSolicitud parametros = new ParametrosRechazarSolicitud(id, status1, status2, user);
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(parametros),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in parametros.GetType().GetProperties()
                                       where nodo.GetValue(parametros) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(parametros).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_RechazaSolicitudesPendienteDocumentos";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return true;

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return false;
                throw;
            }
        }
        public string cabiarStatusSolicitudes(int id, int status1, int status2, string user, string estadoGestion)
        {
            ParametrosAprobarSolicitud parametros = new ParametrosAprobarSolicitud(id, user, status1, status2, "", estadoGestion);
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(parametros),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in parametros.GetType().GetProperties()
                                       where nodo.GetValue(parametros) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(parametros).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_CambiaEstadoSolicitudes";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return "!!!La Solicitud: " + parametros.Id + " fue Cambiada a estado: " + parametros.EstadoGestion;

            }
            catch (System.Data.SqlClient.SqlException exBd)
            {
                dto_excepcion.STR_MENSAJE = exBd.Message;
                dto_excepcion.STR_DETALLE = exBd.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                string mensaje = "La solicitud no fue cambiada de Status por el siguiente error en la base de datos:\n\n" + exBd.Message.ToString();
                return mensaje;
                throw;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                string mensaje = "La solicitud no fue cambiada de Status por el siguiente error en la aplicacion:\n\n" + ex.Message.ToString();
                return mensaje;
                throw;
            }
        }
        public Boolean rechazarAprobadoVerificacionTesoreria(int id, int status1, int status2, string user)
        {
            ParametrosRechazarSolicitud parametros = new ParametrosRechazarSolicitud(id, status1, status2, user);
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(parametros),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in parametros.GetType().GetProperties()
                                       where nodo.GetValue(parametros) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(parametros).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_RechazaSolicitudesPendienteDocumentos";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return true;

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return false;
                throw;
            }
        }
        public Boolean pendienteAprobadoVerificacionTesoreria(int id, int status1, int status2, string user)
        {
            ParametrosRechazarSolicitud parametros = new ParametrosRechazarSolicitud(id, status1, status2, user);
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(parametros),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in parametros.GetType().GetProperties()
                                       where nodo.GetValue(parametros) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(parametros).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_RechazaSolicitudesPendienteDocumentos";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return true;

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return false;
                throw;
            }
        }

        #endregion

        #region Asignacion Ventas
        public List<AgenteVentas> ConsultaAgenteVentas()
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
            dto.SPName = "usp_traeAgenteVentas";
            var dto_result = new List<AgenteVentas>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<AgenteVentas>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<HorarioDisponibleAgente> GetHorariosDisponibles(HorarioDisponibleAgente horarioDisponible)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(horarioDisponible),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in horarioDisponible.GetType().GetProperties()
                                       where nodo.GetValue(horarioDisponible) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(horarioDisponible).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_traeHorarioDisponible";
            var dto_result = new List<HorarioDisponibleAgente>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<HorarioDisponibleAgente>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));

                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }
        public List<HorarioAsignadoAgente> GetHorariosAsignados(HorarioAsignadoAgente horarioDisponible)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(horarioDisponible),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in horarioDisponible.GetType().GetProperties()
                                       where nodo.GetValue(horarioDisponible) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(horarioDisponible).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_traeHorariosAsignados";
            var dto_result = new List<HorarioAsignadoAgente>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<HorarioAsignadoAgente>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));

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
        public Boolean GuardarHorariosAsignados(HorarioAsignadoAgente horarioAsignado)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(horarioAsignado),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in horarioAsignado.GetType().GetProperties()
                                       where nodo.GetValue(horarioAsignado) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(horarioAsignado).ToString()
                                       }
            );
            dto.SPName = "usp_guardarHorarioAsignado";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return true;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return false;
            }

        }
        public Boolean GuardarHorariosDisponibles(HorarioDisponibleAgente horarioDisponible)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(horarioDisponible),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };



            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in horarioDisponible.GetType().GetProperties()
                                       where nodo.GetValue(horarioDisponible) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(horarioDisponible).ToString()
                                       }
            );
            dto.SPName = "usp_guardarHorarioDisponible";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return true;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return false;
            }
        }
        #endregion

        #region Tranferencias Finanzas
        public List<TransferenciasFinanzas> consultatranferenciasFinanzas(TransferenciasFinanzas tranferencias)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tranferencias),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in tranferencias.GetType().GetProperties()
                                       where nodo.GetValue(tranferencias) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tranferencias).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_GeneraTransferencias";
            var dto_result = new List<TransferenciasFinanzas>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<TransferenciasFinanzas>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        #endregion
        //--Alfredo José Vargas Seinfarth -13/02/2019--INICIO//
        #region Ejecuta Status Disponibilidad del agente
        public List<dto_login> usp_EjeStatusDisp(dto_login login)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(login),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
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
            dto.SPName = "usp_EjeStatusDisp";
            var dto_result = new List<dto_login>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<dto_login>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        #endregion

        #region Obtiene configuración del sistema
        public List<Tab_ConfigSys> GetConfigSys(Tab_ConfigSys config)
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
                STR_SERVIDOR = System.Net.Dns.GetHostName(),
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
            dto.SPName = "usp_consulta_configuracion_general";
            var dto_result = new List<Tab_ConfigSys>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<Tab_ConfigSys>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;

        }
        #endregion Obtiene configuración del sistema
        //--Alfredo José Vargas Seinfarth -13/02/2019--FINAL//
        #region Aplicar Cobros
        public List<CuentasCobros> consultaCuentasCobros(CuentasCobros cuentas)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(cuentas),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in cuentas.GetType().GetProperties()
                                       where nodo.GetValue(cuentas) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(cuentas).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_ConsultaTransferenciasBancariasCred";
            var dto_result = new List<CuentasCobros>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<CuentasCobros>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public string GuardarCuenta(CuentaGuardar cuenta)
        {
            string mensajeSalida = "";
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(cuenta),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in cuenta.GetType().GetProperties()
                                       where nodo.GetValue(cuenta) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(cuenta).ToString()
                                       }
            );
            dto.Result = null;
            dto.SPName = "usp_AplicaTransferACredito";
            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                mensajeSalida = "La Cuenta se Actualizo con Exito";
            }
            catch (Exception ex)
            {
                mensajeSalida = "Error al Actualizar la Cuenta";
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return mensajeSalida;
        }
        public Origenes CargarOrigen(Origenes origenes)
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

            dto.ParameterList.AddRange(from nodo in origenes.GetType().GetProperties()
                                       where nodo.GetValue(origenes) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(origenes).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_cargar_configuracion_apps";

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp<Origenes>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
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


        public void aplicaConfigCobros(bool pOPCION)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var obj = new { OPCION = pOPCION };
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

            dto.ParameterList.AddRange(from nodo in obj.GetType().GetProperties()
                                       where nodo.GetValue(obj) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(obj).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_Genera_Conf_Cobros";

            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw ex;
            }
        }
        public void bndSubirArchivos(int bdnSubirArchivos)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();
            var obj = new { bdnSubirArchivos = bdnSubirArchivos };
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
            dto.ParameterList.AddRange(from nodo in obj.GetType().GetProperties()
                                       where nodo.GetValue(obj) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(obj).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_actualiza_bandera_subir_cobros";
            try
            {
                DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw ex;
            }
        }
        #region MANTENIMIENTO DE DIRECCIONES COBROS
        //INICIO FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES
        public List<contacto> ConsultarDirecciones(contacto contacs)
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
            var dto_result = new List<contacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        //FIN FCAMACHO 24/04/2019 MANTENIMIENTO DIRECCIONES

        //INICIO AVARGAS 24/04/2019 MANTENIMIENTO DIRECCIONES
        public contacto ConsultarDireccion(contacto contacs)
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
            dto.SPName = "usp_consulta_DireccionGeneral";
            var dto_result = new List<contacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<CatalogoDireccion> ConsultarCatalogoDireccion(CatalogoDireccion catalogoDireccion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(catalogoDireccion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.Result = null;
            dto.SPName = "usp_consulta_CatalogoDireccion";
            var dto_result = new List<CatalogoDireccion>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<CatalogoDireccion>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public contacto ActualizarDireccion(contacto contacs)
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
            dto.SPName = "usp_Actualiza_Dirreccion";
            var dto_result = new List<contacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public contacto CrearTelefono(contacto contacs)
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
            dto.SPName = "usp_Inserta_Teléfono";
            var dto_result = new List<contacto>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<contacto>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        //FIN AVARGAS 24/04/2019 MANTENIMIENTO DIRECCIONES
        #endregion

        #region mantenimiento comprobante de pagos

        public List<comprobantepago> comrobantePago(comprobantepago compago)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(compago),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in compago.GetType().GetProperties()
                                       where nodo.GetValue(compago) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(compago).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_mantenimiento_comprobante_pago";
            var dto_result = new List<comprobantepago>();


            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                    dto_result = JsonConvert.DeserializeObject<List<comprobantepago>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));

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
        #endregion

        #region ConsultaConfMontoPP
        public List<Tab_ConfigSys> ConsultaConfMontoPP(Tab_ConfigSys configSys)
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

        #endregion ConsultaConfMontoPP

        #region MANTENIMIENTO PRODUCTOS
        //INICIO AVARGAS 16/05/2019 MANTENIMIENTO PRODUCTOS
        public ProductosMantenimiento ConsultaProducto(ProductosMantenimiento productosMantenimiento)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(productosMantenimiento),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in productosMantenimiento.GetType().GetProperties()
                                       where nodo.GetValue(productosMantenimiento) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(productosMantenimiento).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_consulta_Producto_Fenix";
            var dto_result = new List<ProductosMantenimiento>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ProductosMantenimiento>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public ProductosMantenimiento ActualizarProducto(ProductosMantenimiento productosMantenimiento)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(productosMantenimiento),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in productosMantenimiento.GetType().GetProperties()
                                       where nodo.GetValue(productosMantenimiento) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(productosMantenimiento).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_TablaActualizaProducto_Fenix";
            var dto_result = new List<ProductosMantenimiento>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ProductosMantenimiento>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public ProductosMantenimiento CrearProducto(ProductosMantenimiento productosMantenimiento)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(productosMantenimiento),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in productosMantenimiento.GetType().GetProperties()
                                       where nodo.GetValue(productosMantenimiento) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(productosMantenimiento).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_IngresaProductoNuevo_Fenix";
            var dto_result = new List<ProductosMantenimiento>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ProductosMantenimiento>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        //FIN AVARGAS 16/05/2019 MANTENIMIENTO PRODUCTOS
        #endregion MANTENIMIENTO PRODUCTOS

        #region GuardarTiempo
        public List<PlanPlagos> guardarTiempoSolicitud(TiempoSolicitud tiempo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tiempo),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tiempo.GetType().GetProperties()
                                       where nodo.GetValue(tiempo) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tiempo).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_guarda_tiempo_Solicitud";
            var dto_result = new List<PlanPlagos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<PlanPlagos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        #endregion
        public List<PlanPlagos> ConsultaPlanPagosVentas(PlanPlagos tabla_Accion)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(tabla_Accion),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in tabla_Accion.GetType().GetProperties()
                                       where nodo.GetValue(tabla_Accion) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(tabla_Accion).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_calcula_plan_pago";
            var dto_result = new List<PlanPlagos>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<PlanPlagos>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        //INICIO FCAMACHO 22/05/2019 GENERACION DE DOCUMENTO DE NOTIFICACION DE COBRO
        public List<GestionNotificacionCobro> ObtenerConfigPlantillaNotifCobros(GestionNotificacionCobro dtoNotific)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(dtoNotific),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in dtoNotific.GetType().GetProperties()
                                       where nodo.GetValue(dtoNotific) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(dtoNotific).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_COBROS_CONFIG_PLANTILLA_NOTIFICACION_COBRO";

            var dto_result = new List<GestionNotificacionCobro>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionNotificacionCobro>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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

        public List<GestionNotificacionCobro> MantenimientoDocumentosCobros(GestionNotificacionCobro dtoNotific)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(dtoNotific),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in dtoNotific.GetType().GetProperties()
                                       where nodo.GetValue(dtoNotific) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(dtoNotific).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_MANT_COBROS_DOCUMENTOS";

            var dto_result = new List<GestionNotificacionCobro>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<GestionNotificacionCobro>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        //FIN FCAMACHO 22/05/2019 GENERACION DE DOCUMENTO DE NOTIFICACION DE COBRO
        public List<GestionConsulta> BusquedaNombre(Dto_SolicitudesXAcesor nombre)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(nombre),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in nombre.GetType().GetProperties()
                                       where nodo.GetValue(nombre) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(nombre).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_ConsultarSolicitudes_Nombre";
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
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }
            return dto_result;
        }
        public Correo validarEmail(Correo correo)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_PARAMETROS = JsonConvert.SerializeObject(correo),
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            ManagerSolcitudes mang = new ManagerSolcitudes();
            correo.Mail = string.IsNullOrEmpty(correo.Mail) ? "no tiene" : correo.Mail;
            try
            {
                Tab_ConfigSys dto_Config = new Tab_ConfigSys();
                dto_Config.llave_Config1 = "SERVICIO";
                dto_Config.llave_Config2 = "APIS";
                dto_Config.llave_Config3 = "EMAILS";
                dto_Config.llave_Config4 = "URL";
                var dto_interval = mang.ConsultaConfiUrlImagen(dto_Config);

                correo.URL = dto_interval.Where(x => x.llave_Config5 == "VERYFYMAIL").FirstOrDefault().Dato_Char1; // Dirección web del reporte

                //correo.URL = "http://192.168.246.140:9092/api/Mail/VerifyMail"; // Dirección web del reporte
                RestClient cliente = new RestClient(correo.URL);
                RestRequest request = new RestRequest();
                request.Method = Method.POST;
                request.AddHeader("Accept", "application/json");
                request.AddJsonBody(JsonConvert.SerializeObject(correo));
                Correo correoRespuesta = new Correo();
                var dto_result = new List<Correo>();
                var respuesta = cliente.Execute(request); // Metodo que ejecuta la solicitud.
                if (respuesta.StatusCode == System.Net.HttpStatusCode.OK) // Si retorna OK, el reporte fue generado.
                {
                    correoRespuesta = JsonConvert.DeserializeObject<Correo>(respuesta.Content);
                }
                else
                {
                    correoRespuesta.valid = "false";
                }
                return correoRespuesta;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw ex;
            }
        }
        public List<UserAgencias> GetUserAgencias()
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
            var dto_result = new List<UserAgencias>();
            dto.Result = null;
            dto.SPName = "usp_consulta_agencias";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<UserAgencias>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<TelefonoLaboral> GuardarTelefonoLaboral(TelefonoLaboral telefono)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(telefono),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in telefono.GetType().GetProperties()
                                       where nodo.GetValue(telefono) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(telefono).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_Inserta_Telefono_Laboral";
            var dto_result = new List<TelefonoLaboral>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<TelefonoLaboral>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
        public List<TelefonoLaboral> GetTelefonoLaboral(TelefonoLaboral telefono)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(telefono),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.ParameterList.AddRange(from nodo in telefono.GetType().GetProperties()
                                       where nodo.GetValue(telefono) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(telefono).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_Ventas_ConsultarTelefonoLaboral";
            var dto_result = new List<TelefonoLaboral>();
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<TelefonoLaboral>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
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
    }
}

using TwoFunTwoMeFintech.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_Common;
using TwoFunTwoMe_DataAccess;
using System.Reflection;
using TwoFunTwoMeFintech.Models.DTO;

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

        public List<dto_login> Login(dto_login login)
        {
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

            DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            return JsonConvert.DeserializeObject<List<dto_login>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
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


            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.Result = null;
            dto.SPName = "usp_traeBucket";

            DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            return JsonConvert.DeserializeObject<List<dto_AsignacionBuckets>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
        }


        public List<dto_CantidadBuckets> DetalleBucket()
        {
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

            DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            return JsonConvert.DeserializeObject<List<dto_CantidadBuckets>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
        }
        public int Registrar(dto_login login)
        {
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
                        Name = "correo",
                        Value = login.correo
                    },
                      new SpParameter
                    {
                        Name = "ROLID",
                        Value = login.ROLID.ToString()
                    },
                      new SpParameter
                    {
                        Name = "estado",
                        Value = login.estado
                    }
                },
                Result = null,
                SPName = "usp_inserta_agente"
            };

            DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            var retorno = 0;
            if (!ds.HasResult)
            {
                retorno = 1;

            }
            return retorno;
        }

        public List<MenuModels> mostrarMenu(string agente)
        {
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

            DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);

            return JsonConvert.DeserializeObject<List<MenuModels>>(JsonConvert.SerializeObject(ds.Result.Tables[0]));
        }

        public int UpdateBuckets(dto_login login)
        {
            //var dto.ParameterList.
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
            //{
            //    ParameterList = new List<SpParameter>
            //    {
            //        new SpParameter
            //        {
            //            Name = "cod_agente",
            //            Value = login.cod_agente
            //        }, new SpParameter
            //        {
            //            Name = "ConfiguracionBucket",
            //            Value = login.ConfiguracionBucket
            //        }
            //    },
            //    Result = null,
            //    SPName = "usp_ActualizarAgente"
            //};
            try
            {
                DynamicDto ds = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return 1;
            }
            catch { return -1; }
        }

        public List<Roles> GetUserRoles()
        {
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            dto.Result = null;
            dto.SPName = "usp_consulta_roles";
            var dto_result = new List<Roles>();

            var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            if (objRet.HasResult)
            {
                dto_result = JsonConvert.DeserializeObject<List<Roles>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
            }
            return dto_result;
        }

        public List<AsignacionBuckets> GetAsignacionBuckets(int? id = null)
        {
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
            catch (Exception)
            {
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";

            }
            return dto_result;
        }
        public bool ActualizarBuckets(AsignacionBuckets buckets)
        {
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
            catch
            {
                dto_result.MensajeError = "Ocurrio un Error";
                dto_result.Respuesta = false;

            }
            return dto_result.Respuesta;
        }

        public bool EliminaBuckets(AsignacionBuckets buckets)
        {
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
            catch
            {
                dto_result.MensajeError = "Ocurrio un Error";
                dto_result.Respuesta = false;

            }
            return dto_result.Respuesta;
        }

        public List<Solicitudes> CargarSolicitudBuro(Solicitudes solicitudes)
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
            catch
            {
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";

            }
            return dto_result;
        }

        public List<productos> CargarProductos()
        {
            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
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
            catch
            {
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";

            }
            return dto_result;
        }
        public List<Tipos> CargarTipos(string tipo)
        {
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
            catch (Exception)
            {
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";

            }
            return dto_result;
        }

        public Solicitudes ActualizaSolicitudCredito(Solicitudes solicitudes)
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
            dto.SPName = "usp_actualiza_limite_solicitud";
            var dto_result = new Solicitudes();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                dto_result.Mensaje = string.Concat("La solicitud N# ", solicitudes.Id, " se actualizo");
            }
            catch
            {
                dto_result.MensajeError = "Ocurrio un Error";

            }
            return dto_result;
        }

        public BurosXml DescargarXmlBuro(Solicitudes solicitudes)
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
            dto.SPName = "usp_consultar_xml_buros";
            var dto_result = new List<BurosXml>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {

                    dto_result = JsonConvert.DeserializeObject<List<BurosXml>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().MIMETYPE = @"text/xml";

                    dto_result.FirstOrDefault().DATATUCA = string.Format("data:{0};base64,{1}", dto_result.FirstOrDefault().MIMETYPE, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto_result.FirstOrDefault().TUCA)));

                    dto_result.FirstOrDefault().NOMBRETUCA = String.Concat(solicitudes.Identificacion, "_Tuca_ContenidoXML.xml");
                    dto_result.FirstOrDefault().NOMBRECREDDIT = String.Concat(solicitudes.Identificacion, "_CREDDID_ContenidoXMLL.xml");

                    dto_result.FirstOrDefault().DATACREDDIT = string.Format("data:{0};base64,{1}", dto_result.FirstOrDefault().MIMETYPE, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(dto_result.FirstOrDefault().CREDDIT)));

                }
                /*
                  System.Text.Encoding.UTF8.GetBytes(plainText);
  return System.Convert.ToBase64String(plainTextBytes);
                 */
            }
            catch (Exception)
            {
                dto_result.FirstOrDefault().MensajeError = "Ocurrio un Error";
            }
            return dto_result.FirstOrDefault();
        }
        public AsignacionBuckets InsertarBuckets(AsignacionBuckets buckets)
        {
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
            catch
            {
                dto_result.MensajeError = "Ocurrio un Error";
                throw;
            }
            return dto_result;
        }
        public List<Solicitudes> ConsultaSolicitudes(string Identificacion = "")
        {
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


            }
            return dto_result;
        }
        public List<dto_login> ConsultaUsuarios(dto_login login)
        {
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


            }
            return dto_result;
        }
        public List<dto_login> ActualizaUsuarios(dto_login login)
        {
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


            }
            return dto_result;
        }

        #region ScoreCardExperto
        public List<ScoreCardExperto> ConsultaScoreCardExperto(ScoreCardExperto SCE)
        {
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
            catch (Exception)
            {
                var err = new ScoreCardExperto
                {
                    Mensaje = "Ocurrio un Error"
                };
                dto_result.Add(err);

            }
            return dto_result;
        }
        public int ActualizaScoreCardExperto(ScoreCardExperto SCE)
        {
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
            catch (Exception)
            {
                dto_result = -1;

            }
            return dto_result;
        }


        public ScoreCardExperto EliminarScoreCardExperto(ScoreCardExperto SCE)
        {
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
            catch (Exception)
            {

                SCE.Mensaje = "UPPS, lo ciento, favor contacte con el Administrador";
            }
            return SCE;
        }

        public ScoreCardExperto InsertarScoreCardExperto(ScoreCardExperto SCE)
        {
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
            catch (Exception)
            {

                SCE.Mensaje = "UPPS, lo ciento, favor contacte con el Administrador";
            }
            return SCE;
        }
        #endregion

    }
}

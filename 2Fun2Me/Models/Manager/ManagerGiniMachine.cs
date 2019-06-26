using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using TwoFunTwoMe.Models.DTO;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_Common;
using TwoFunTwoMe_DataAccess;

namespace TwoFunTwoMe.Models.Manager
{
    public class ManagerGiniMachine
    {

        public ManagerGiniMachine()
        {
            String connBD = string.Empty;


            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["BuroConnection"].ConnectionString);
            }
        }

        #region Variables Privadas

        private MQRabbit mqRabbit = new MQRabbit();

        private string connBD = string.Empty;

        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };

        #endregion

        #region Conexion BD

        #endregion

        #region Configuracion Buro

        public List<DTO_GiniMachineConfig> ObtenerConfigGiniMachine(DTO_GiniMachineConfig objConfig)
        {

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in objConfig.GetType().GetProperties()
                                       where nodo.GetValue(objConfig) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(objConfig).ToString()
                                       }
                                       );
            dto.Result = null;
            dto.SPName = "usp_ObtenerConfiguracionGiniMachine";
            var dto_result = new List<DTO_GiniMachineConfig>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_GiniMachineConfig>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                //dto_result.Mensaje = "Ocurrio un Error";
                throw;
            }

            return dto_result;

        }

        #endregion

        #region Get JSON Input

        public List<DTO_GiniMachine> ObtenerJSON(DTO_GiniMachine dto_Entrada)
        {

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in dto_Entrada.GetType().GetProperties()
                                       where nodo.GetValue(dto_Entrada) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(dto_Entrada).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_GeneraProspectoModeloGini";
            var dto_result = new List<DTO_GiniMachine>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_GiniMachine>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, ConfigurationManager.ConnectionStrings["TwoFunTwoMeConnection"].ConnectionString);
                dto_Entrada.Mensaje = "ERR";

            }
            return dto_result;
        }

        #endregion

        #region SET Output BD
        public List<DTO_GiniMachine> GuardarResultadoGinimachine(DTO_GiniMachine dto_Entrada)
        {

            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;


            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in dto_Entrada.GetType().GetProperties()
                                       where nodo.GetValue(dto_Entrada) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(dto_Entrada).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_Inserta_XML_GiniMachine";
            var dto_result = new List<DTO_GiniMachine>();

            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<DTO_GiniMachine>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                    dto_result.FirstOrDefault().Mensaje = string.Concat("", dto_result.FirstOrDefault().Mensaje);
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, ConfigurationManager.ConnectionStrings["TwoFunTwoMeConnection"].ConnectionString);
                dto_Entrada.Mensaje = "ERR";

            }
            return dto_result;
        }
        #endregion

    }
}
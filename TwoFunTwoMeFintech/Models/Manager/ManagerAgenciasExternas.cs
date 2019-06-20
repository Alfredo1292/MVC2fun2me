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

namespace TwoFunTwoMeFintech.Models.Manager
{
    public class ManagerAgenciasExternas 
    {
        private InfoClass infDto = new InfoClass();

      
        public ManagerAgenciasExternas() {
            String connBD = string.Empty;
            infDto.STR_COD_PAIS = "506";

          GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }


        public bool guardarAgenciaExterna(DtoAgenciaExterna agenciaExterna)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(agenciaExterna),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in agenciaExterna.GetType().GetProperties()
                                       where nodo.GetValue(agenciaExterna) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(agenciaExterna).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_insertaAgenciaExterna";
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
                throw ex;
            }
        }


        public bool guardarAsignacionAgenciaExterna(DtoAsignacionAgenciaExterna row )
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(row),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();

            dto.ParameterList.AddRange(from nodo in row.GetType().GetProperties()
                                       where nodo.GetValue(row) != null
                                       select new SpParameter
                                       {
                                           Name = nodo.Name,
                                           Value = nodo.GetValue(row).ToString()
                                       }
                );
            dto.Result = null;
            dto.SPName = "usp_asignar_agencia_externa";
            try
            {
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                return true;
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw ex;
            }
        }
    }
}

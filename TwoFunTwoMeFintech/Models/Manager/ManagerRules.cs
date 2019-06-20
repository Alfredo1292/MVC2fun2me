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
using TwoFunTwoMeFintech.Models.DTO.AnalisisRules;

namespace TwoFunTwoMeFintech.Models.Manager
{
    public class ManagerRules
    {

        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };
        public ManagerRules()
        {
            String connBD = string.Empty;


            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }
        public List<ConsultaRules> consultaRules(ConsultaRules rules)
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
            var dto_result = new List<ConsultaRules>();
            try
            {
                dto.ParameterList.AddRange(from nodo in rules.GetType().GetProperties()
                                           where nodo.GetValue(rules) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(rules).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_consultaAnalisisRules";
                var objRet = DynamicSqlDAO.ExecuterSp(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                if (objRet.HasResult)
                {
                    dto_result = JsonConvert.DeserializeObject<List<ConsultaRules>>(JsonConvert.SerializeObject(objRet.Result.Tables[0]));
                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
            }
            return dto_result;
        }
    }
}
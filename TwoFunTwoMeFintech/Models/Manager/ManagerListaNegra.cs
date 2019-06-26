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
using TwoFunTwoMeFintech.Models.DTO.ListaNegra;

namespace TwoFunTwoMe.Models.Manager
{
    /// <summary>
    /// CLASE ESPECIALIZADAD EN SOLICITUDES HEREDA LA MANAGER USER
    /// </summary>
    public class ManagerListaNegra 
    {
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };

        public ManagerListaNegra()
        {
            String connBD = string.Empty;


            GlobalClass.connectionString.TryGetValue(infDto.STR_COD_PAIS, out connBD);
            if (connBD == null || string.IsNullOrEmpty(connBD))
            {
                GlobalClass.connectionString.TryAdd(infDto.STR_COD_PAIS, ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }

        public List<dto_ListaNegra> ConsultaListaNegra(dto_ListaNegra ListaNegra)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(ListaNegra),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            try
            {
                dto.ParameterList.AddRange(from nodo in ListaNegra.GetType().GetProperties()
                                           where nodo.GetValue(ListaNegra) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(ListaNegra).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_ADMINISTRACION_CONSULTAR_LISTA_NEGRA";


                return DynamicSqlDAO.ExecuterSp<dto_ListaNegra>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value).ToList();

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
               // Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<dto_ListaNegra>();
        }

        public List<dto_ListaNegra> MantenimientoListaNegra(dto_ListaNegra ListaNegra)
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_PARAMETROS = JsonConvert.SerializeObject(ListaNegra),
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            var dto = new DynamicDto();
            dto.ParameterList = new List<SpParameter>();
            try
            {
                dto.ParameterList.AddRange(from nodo in ListaNegra.GetType().GetProperties()
                                           where nodo.GetValue(ListaNegra) != null
                                           select new SpParameter
                                           {
                                               Name = nodo.Name,
                                               Value = nodo.GetValue(ListaNegra).ToString()
                                           }
                    );
                dto.Result = null;
                dto.SPName = "usp_ADMINISTRACION_MANTENIMIENTO_LISTANEGRA";


                return DynamicSqlDAO.ExecuterSp<dto_ListaNegra>(dto, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value).ToList();

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                // Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return new List<dto_ListaNegra>();
        }

    }
}

using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using TwoFunTwoMe.Models.DTO;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_Common;
using TwoFunTwoMe_DataAccess;

namespace TwoFunTwoMe.Models.Manager
{
    public class MQRabbit
    {
        private Manager manage;
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };

        public void insertaColaRabbit(InsertPersonasWeb personasWeb)
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
                STR_PARAMETROS = JsonConvert.SerializeObject(personasWeb)
            };

            var dto_Config = new Tab_ConfigSys
            {
                llave_Config1 = "SERVICIO",
                llave_Config2 = "CONFIGURACION",
                llave_Config3 = "SERVIDOR",
                llave_Config4 = "RABBIT"
            };
            try
            {
                var dto_ret_config = GetConfigRabbit(dto_Config);

                var factory = new ConnectionFactory()
                {
                    HostName = dto_ret_config.Where(x => x.llave_Config5 == "HOSTNAME").Select(x => x.Dato_Char1).FirstOrDefault(),
                    //Port = 15672,
                    Password = dto_ret_config.Where(x => x.llave_Config5 == "PASSWORD").Select(x => x.Dato_Char1).FirstOrDefault(),
                    UserName = dto_ret_config.Where(x => x.llave_Config5 == "USERNAME").Select(x => x.Dato_Char1).FirstOrDefault()
                };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    //channel.QueueDeclare(queue: dto_ret_config.Where(x => x.llave_Config5 == "EXCHANGES_SOLICITUDES").Select(x => x.Dato_Char1).FirstOrDefault(),
                    //                     durable: true,
                    //                     exclusive: false,
                    //                     autoDelete: false,
                    //                     arguments: null);




                    string message = string.Concat(personasWeb.Identificacion, "~", personasWeb.IdTipoIdentificacion, "~", personasWeb.PrimerNombre, "~", personasWeb.SegundoNombre,
                        "~", personasWeb.PrimerApellido, "~", personasWeb.SegundoApellido, "~", personasWeb.TelefonoCel, "~", personasWeb.Correo, "~", personasWeb.IdProducto, "~",
                        personasWeb.IdBanco, "~", personasWeb.UsrModifica, "~", personasWeb.Origen);

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: dto_ret_config.Where(x => x.llave_Config5 == "EXCHANGES_SOLICITUDES").Select(x => x.Dato_Char1).FirstOrDefault(),
                                routingKey: "",
                                basicProperties: null,
                                body: body);
                    //channel.BasicPublish(exchange: "",
                    //                     routingKey: dto_ret_config.Where(x => x.llave_Config5 == "QUEUE").Select(x => x.Dato_Char1).FirstOrDefault(),
                    //                     basicProperties: null,
                    //                     body: body);


                }
            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                manage = new Manager();
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
        }

        private List<Tab_ConfigSys> GetConfigRabbit(Tab_ConfigSys config)
        {
            manage = new Manager();

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
                DynamicSqlDAO.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
            }
            return dto_result;

        }
    }
}
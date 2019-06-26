using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using TwoFunTwoMe.Models.DTO;
using TwoFunTwoMe_DataAccess;

namespace TwoFunTwoMe.Models.Utility
{
    public class EnvioCorreos
    {

        private void SendEmail(string Subject, string Body, string sentTo)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            string strHostName = System.Net.Dns.GetHostName();

            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                FEC_CREACION = DateTime.Now,
                STR_SERVIDOR = strHostName
            };

            var dto = new Tab_ConfigSys
            {
                llave_Config1 = "SERVICIO",
                llave_Config2 = "CONFIGURACION",
                llave_Config3 = "SERVIDOR",
                llave_Config4 = "ENVIO_EMAIL"
            };
            TwoFunTwoMe.Models.Manager.Manager manager = new Manager.Manager();
            try
            {
                var dto_config_Service = manager.CargarConfiguracionEnvioCorreos(dto);
                var smtp = dto_config_Service.Where(x => x.llave_Config5 == "SmtpClient").Select(x => x.Dato_Char1).FirstOrDefault();
                var puerto = dto_config_Service.Where(x => x.llave_Config5 == "PUERTO").Select(x => x.Dato_Int1).FirstOrDefault().Value;
                var email = dto_config_Service.Where(x => x.llave_Config5 == "EMAIL").Select(x => x.Dato_Char1).FirstOrDefault();
                var pass = dto_config_Service.Where(x => x.llave_Config5 == "PASSWORD").Select(x => x.Dato_Char1).FirstOrDefault();
                var nombre = dto_config_Service.Where(x => x.llave_Config5 == "NOMBRE").Select(x => x.Dato_Char1).FirstOrDefault();

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(email, nombre, Encoding.UTF8);
                    message.Subject = Subject;
                    message.Body = Body;
                    if (sentTo.Contains(";"))
                    {
                        var multiSent = sentTo.Split(';');
                        for (int index = 0; index < multiSent.Length; ++index)
                        {
                            var Email = multiSent[index].ToString().Trim();
                            if (!string.IsNullOrEmpty(Email))
                            {
                                if (validarEmails(Email))
                                    message.To.Add(multiSent[index]);
                            }
                        }
                    }
                    else
                    {
                        if (validarEmails(sentTo))
                            message.To.Add(sentTo);
                    }
                    //sendDocument
                    message.Attachments.Clear();
                    //message.Attachments.Add(new Attachment(this.generarAdjunto(pImg.Value, idDoc, copia, tipoPlantilla), "FacturaElectronica.pdf"));

                   // message.Attachments.Add(new Attachment());

                    //AlternateView alternateViewFromString1 = AlternateView.CreateAlternateViewFromString(Regex.Replace(Body, "<(.|\\n)*?>", string.Empty), (Encoding)null, "text/plain");
                    //AlternateView alternateViewFromString2 = AlternateView.CreateAlternateViewFromString(Body, (Encoding)null, "text/html");
                    //message.AlternateViews.Clear();
                    //message.AlternateViews.Add(alternateViewFromString1);
                    //message.AlternateViews.Add(alternateViewFromString2);

                    using (SmtpClient smtpClient = new SmtpClient(smtp, puerto))
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        //smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential(email, pass);

                        smtpClient.Send(message);
                    }
                    dto_excepcion.STR_PARAMETROS = String.Format("sentTo={0}; smtp={1}; puerto={2}; email={3}; pass={4}", sentTo, smtp, puerto, email, pass);
                }

            }
            catch (Exception ex)
            {

                dto_excepcion.STR_MENSAJE = ex.Message;
                // manage.guardaExcepcion(dto_excepcion);
            }
        }
        public bool validarEmails(string email)
        {
            try
            {
                string pattern = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
                return Regex.IsMatch(email, pattern) && Regex.Replace(email, pattern, string.Empty).Length == 0;
            }
            catch (Exception ex)
            {
                //S_Exceptions.RegisterException(0, "validadEmails", "admin@facturaelectronica.co.cr", ex.Message);
                return false;
            }
        }
    }
}
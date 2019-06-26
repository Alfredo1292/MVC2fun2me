using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            ManagerUser manage = new ManagerUser();
            var origen = new Origenes
            {
                Aplicativo = GlobalClass.Aplicativo
            };

            GlobalClass.Origen_Apps = manage.CargarOrigen(origen).Origen.ToString();
        }
        protected void Session_Start(object sender, EventArgs e)
        {

            //if (Session.IsNewSession && Session["LoginCredentials"] == null)
            //{
            //    Session["SessionExpire"] = true;
            //    Response.Redirect("Login/Login");
            //    Session.Timeout = 0;
            //}
            Session.Timeout = 5000;
            if (!User.Identity.IsAuthenticated)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
                Response.Redirect(FormsAuthentication.LoginUrl, true);
                //Response.Redirect("Login/Login");
            }
            else Session.Timeout = 5000;
        }
        protected void Session_End(Object sender, EventArgs e)
        {
            try
            {
                ManagerUser mang = new ManagerUser();
                if (Session.IsNewSession && Session["LoginCredentials"] == null)
                {
                    var loginUser = new dto_login
                    {
                        cod_agente = string.Empty,
                        IdCatDisponibilidad = 0
                    };

                    var dto_retorno = (List<TwoFunTwoMeFintech.Models.dto_login>)Session["LoginCredentials"];

                    var dto_Config = new Tab_ConfigSys
                    {
                        llave_Config1 = "CONFIGURACION",
                        llave_Config2 = "FENIX",
                        llave_Config3 = "VENTAS",
                        llave_Config4 = "CIERRE_SESION",
                        llave_Config5 = "CIERR_SES"
                    };

                    var dto_ret_config = mang.GetConfigSys(dto_Config);
                    loginUser.cod_agente = dto_retorno[0].cod_agente;
                    loginUser.IdCatDisponibilidad = dto_ret_config.FirstOrDefault().Dato_Int1;
                    var StsAsesor = mang.usp_EjeStatusDisp(loginUser);
                    // Remove user from HashTable
                    Session.Abandon();
                    Session.Clear();
                    Session.RemoveAll();
                }
            }
            catch { }
        }
        protected void Application_End(object sender, EventArgs e)
        {
            if (Session.IsNewSession && Session["LoginCredentials"] == null)
            {
                Session.Abandon();
                Session.Clear();
                Session.RemoveAll();
            }
        }
    }
}
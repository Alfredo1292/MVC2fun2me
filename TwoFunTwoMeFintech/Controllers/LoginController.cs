using TwoFunTwoMe_DataAccess;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models;
using System.Collections.Generic;

namespace TwoFunTwoMeFintech.Controllers
{
    //[Authorize]
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModels _login)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now);
            Response.Cache.SetNoServerCaching();
            Response.Cache.SetNoStore();

            if (ModelState.IsValid) //validating the user inputs
            {
                var loginUser = new dto_login
                {
                    nombre = string.Empty,
                    pass = TwoFunTwoMe_DataAccess.Utility.Encrypt(_login.Password), //Cryption.Encrypt(_login.Password, ConfigurationManager.AppSettings["claveEncriptacion"]),
                    vendedor = string.Empty,
                    cod_agente = _login.Cedula.ToString(),
                    estado = string.Empty,
                    correo = string.Empty
                };


                //Manager.ClientPostRequest(loginUser, AppSettings.urlApi, AppSettings.controladorLogin);
                //Manager.SendRequestAsync(AppSettings.urlApi , AppSettings.controladorLogin, loginUser);

                ManagerUser mang = new ManagerUser();


                var dto_retorno = mang.Login(loginUser);

                // var result = await _signInManager.PasswordSignInAsync(Input.Cedula, Input.Password, Input.RememberMe, lockoutOnFailure: true);

                if (dto_retorno.Any())
                {

                    if (dto_retorno.FirstOrDefault().esTemporal)
                    {
                        return RedirectToAction("CambioPassword", "Login");
                    }

                    //--Alfredo José Vargas Seinfarth -13/02/2019--INICIO//
                    var dto_Config = new Tab_ConfigSys
                    {
                        llave_Config1 = "CONFIGURACION",
                        llave_Config2 = "FENIX",
                        llave_Config3 = "VENTAS",
                        llave_Config4 = "INICIO_SESION",
                        llave_Config5 = "INI_SEC"
                    };

                    var dto_ret_config = mang.GetConfigSys(dto_Config);
                    if (dto_ret_config.Any())
                    {
                        dto_retorno[0].IdCatDisponibilidad = dto_ret_config.FirstOrDefault().Dato_Int1.GetValueOrDefault();
                        var StsAsesor = mang.usp_EjeStatusDisp(dto_retorno[0]);
                    }
                    //--Alfredo José Vargas Seinfarth -13/02/2019--INICIO//

                    var dto_ret = mang.mostrarMenu(_login.Cedula);

                    Session["LoginCredentials"] = dto_retorno;
                    Session["MenuMaster"] = dto_ret; //Bind the _menus list to MenuMaster session
                    Session["UserName"] = dto_retorno.FirstOrDefault().nombre;
                    Session["agente"] = _login.Cedula;
                    Session["ROLID"] = dto_retorno.Select(x => x.ROLID).FirstOrDefault();
                    FormsAuthentication.SetAuthCookie(dto_retorno.FirstOrDefault().cod_agente, false); // set the formauthentication cookie
                    FormsAuthentication.Initialize();
                    //if (!Membership.ValidateUser(dto_retorno.FirstOrDefault().cod_agente, dto_retorno.FirstOrDefault().pass))
                    //    FormsAuthentication.RedirectFromLoginPage(dto_retorno.FirstOrDefault().cod_agente, false);

                    //Session["LoginCredentials"] = _loginCredentials; // Bind the _logincredentials details to "LoginCredentials" session
                    //Session["MenuMaster"] = _menus; //Bind the _menus list to MenuMaster session
                    //Session["UserName"] = _loginCredentials.UserName;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMsg = "Credenciales inválidas!";
                    return View();
                }
            }
            return View();
        }

        public ActionResult CambioPassword(LoginModels _login)
        {

            if (_login.Password == null) return View();
            ManagerUser mang = new ManagerUser();
            try
            {
                _login.Password = TwoFunTwoMe_DataAccess.Utility.Encrypt(_login.Password); //Cryption.Encrypt(_login.Password, ConfigurationManager.AppSettings["claveEncriptacion"]);
                mang.cambioPass(_login);
                //return Json(new { resultado = "ok" });
            }
            catch
            {
                ViewBag.ErrorMsg = "Ocurrio un error.";
                return View();
            }

            return Json(new { Resultado = "ok" });
        }

        public ActionResult LogOff()
        {
            ManagerUser mang = new ManagerUser();

            var loginUser = new dto_login
            {
                cod_agente = string.Empty,
                IdCatDisponibilidad = 0
            };

            var dto_retorno = (List<TwoFunTwoMeFintech.Models.dto_login>)Session["LoginCredentials"];
            if (dto_retorno != null)
            {
                if (dto_retorno.Any())
                {
                    var dto_Config = new Tab_ConfigSys
                    {
                        llave_Config1 = "CONFIGURACION",
                        llave_Config2 = "FENIX",
                        llave_Config3 = "VENTAS",
                        llave_Config4 = "CIERRE_SESION",
                        llave_Config5 = "CIERR_SES"
                    };

                    var dto_ret_config = mang.GetConfigSys(dto_Config);
                    if (dto_ret_config.Any())
                    {
                        loginUser.cod_agente = dto_retorno[0].cod_agente;
                        loginUser.IdCatDisponibilidad = dto_ret_config.FirstOrDefault().Dato_Int1;
                        var StsAsesor = mang.usp_EjeStatusDisp(loginUser);
                    }
                }
            }
            // Remove user from HashTable
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");

        }
    }
}

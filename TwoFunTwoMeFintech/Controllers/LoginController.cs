using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoFunTwoMeFintech.Models;
using System.Web.Security;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe.Models.Manager;
using ModeAuthentication.Models.Utilitarios;
using System.Configuration;

namespace TwoFunTwoMeFintech.Controllers
{
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
            if (ModelState.IsValid) //validating the user inputs
            {
                var loginUser = new dto_login
                {
                    nombre = string.Empty,
                    pass = Cryption.Encrypt(_login.Password, ConfigurationManager.AppSettings["claveEncriptacion"]),
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

<<<<<<< HEAD
                    if (dto_retorno.FirstOrDefault().esTemporal)
                    {
                        return RedirectToAction("CambioPassword", "Login");
                    }

                    var dto_ret = mang.mostrarMenu(_login.Cedula);
=======
                    //if (dto_retorno.FirstOrDefault().esTemporal)
                    //{
                    //    return RedirectToAction("CambioPassword", "Login");
                    //}

                    var dto_ret = mang.mostrarMenu( _login.Cedula);
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851

                    Session["LoginCredentials"] = dto_retorno;
                    Session["MenuMaster"] = dto_ret; //Bind the _menus list to MenuMaster session
                    Session["UserName"] = dto_retorno.FirstOrDefault().nombre;
                    Session["agente"] = _login.Cedula;
                    FormsAuthentication.SetAuthCookie(dto_retorno.FirstOrDefault().cod_agente, false); // set the formauthentication cookie

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
<<<<<<< HEAD
            if(_login.Password==null) return View();
            ManagerUser mang = new ManagerUser();
            try
            {
                mang.cambioPass(_login);
            }
            catch
            {
                ViewBag.ErrorMsg = "Ocurrio un error.";
                return View();
            }
            return RedirectToAction("Login", "Login"); ;
=======
            return View();
>>>>>>> 02077533187183e7a76adbfd15db5d101424f851
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            return RedirectToAction("Login", "Login");
        }
    }
}

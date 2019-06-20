using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoFunTwoMeFintech.Models.DTO.GestionesCartera;
using TwoFunTwoMeFintech.Models.Manager;

namespace TwoFunTwoMeFintech.Controllers.GestionesCartera
{
    public class AdministrarGestionesCarteraController : Controller
    {
        //
        // GET: /AdministrarGestionesCartera/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConsultaGestionesPendientes()
        {

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            ManagerGestionCartera manage = new ManagerGestionCartera();

            var ret = manage.ConsultaGestionesPendientes();

            return Json(ret);

        }
        public ActionResult AprobarGestion(GestionCartera gestion)
        {

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            ManagerGestionCartera manage = new ManagerGestionCartera();
            gestion.UsuarioAprobacion = Session["agente"].ToString();

            var ret = manage.AprobarGestion(gestion);

            return Json(ret.FirstOrDefault());

        }
        public ActionResult RechazarGestion(GestionCartera gestion)
        {

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            ManagerGestionCartera manage = new ManagerGestionCartera();
            gestion.UsuarioAprobacion = Session["agente"].ToString();

            var ret = manage.RechazarGestion(gestion);

            return Json(ret.FirstOrDefault());

        }

    }
}

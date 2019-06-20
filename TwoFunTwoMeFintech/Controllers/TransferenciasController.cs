using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class TransferenciasController : Controller
    {
        //
        // GET: /Transferencias/


        public ActionResult ConsultarTransferencias(Transferencias transferencias)
        {
                return View();
        }
        public ActionResult ConsultarTransferenciasDocumentos(Transferencias transferencias)
        {
            //if (string.IsNullOrEmpty(transferencias.fecha_inicio) || string.IsNullOrEmpty(transferencias.fecha_fin))
            //    return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var dto_ret = manage.ConsultarTransferencias(transferencias);
            return Json(dto_ret);
        }

        public ActionResult ConsultarTotalTransferencias(Transferencias transferencias)
        {
            //if (string.IsNullOrEmpty(transferencias.fecha_inicio) || string.IsNullOrEmpty(transferencias.fecha_fin))
            //    return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var dto_ret = manage.ConsultarTotalTransferencias(transferencias);
            return Json(dto_ret);
        }
    }
}

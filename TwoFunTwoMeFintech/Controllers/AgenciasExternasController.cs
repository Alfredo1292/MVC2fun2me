using System;
using System.Web.Mvc;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.Manager;

namespace TwoFunTwoMeFintech.Controllers
{
    public class AgenciasExternasController : Controller
    {
        //
        // GET: /AjenciasExternas/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult guardarAgenciaExterna(DtoAgenciaExterna agenciaExterna) {
            ManagerAgenciasExternas managerAgenciaExterna  =new ManagerAgenciasExternas();
            try
            {
                bool result = managerAgenciaExterna.guardarAgenciaExterna(agenciaExterna);
                return Json(new { saved = true });
            }
            catch (Exception e) {
                return Json(new { saved = false });
            }
        }

        public ActionResult guardarAsignacionesAgenciasExternas(DtoAsignacionAgenciaExterna[] registros)
        {
            ManagerAgenciasExternas managerAgenciaExterna = new ManagerAgenciasExternas();
            try
            {
                for (var i = 0; i < registros.Length; i++) {
                    var current=registros[i];
                    managerAgenciaExterna.guardarAsignacionAgenciaExterna(current);
                }
                //bool result = managerAgenciaExterna.guardarAsignacionAgenciaExterna(agenciaExterna);
                return Json(new { saved = true });
            }
            catch (Exception e)
            {
                return Json(new { saved = false });
                throw e;
            }
        }

    }
}

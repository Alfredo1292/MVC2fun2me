using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models.DTO;
using System.Linq;

namespace TwoFunTwoMeFintech.Controllers
{
    public class AdministracionCobrosController : Controller
    {
        //
        // GET: /AdministracionCobros/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdministracionCreditos(DashBoard dashBoard)
        {
            ////if (dashBoard.AgenteAsignado == null) return View();
            ////dashBoard.FechaAsignacion = System.DateTime.Now.ToString("yyyy-MM-dd");
            //ManagerUser manage = new ManagerUser();

            //if (Session["agente"] == null)
            //    return RedirectToAction("LogOff", "Login");

            //var ret = manage.ConsultarDashBoardDetallado(dashBoard);

            //return Json(ret);
            return View();
        }
        public ActionResult RealizarAccion(ConsultarCreditos consultarCreditos)
        {
            //if (dashBoard.AgenteAsignado == null) return View();
            //dashBoard.FechaAsignacion = System.DateTime.Now.ToString("yyyy-MM-dd");
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var resul = manage.ConsultaCobro(consultarCreditos);
            if (resul.Any())
            {
                var ret = manage.RealizarAccion(consultarCreditos);
                return Json(ret);
            }
            else
            {
                return Json(new { Mensaje = "El credito no existe !" });
            }
        }
    }

}


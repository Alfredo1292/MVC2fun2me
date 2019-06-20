using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class TesoreriaPendienteTranferenciaController : Controller
    {
        //
        // GET: /PendienteTesoreria/

        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Details()
        {
            return View();

        }
        public ActionResult GetTesoreriaPendianteTranferencia(TesoreriaPendianteTranferencia tesoreria)
        {
            ManagerUser mang = new ManagerUser();
            var listaSolicitudesPendienteTesoreria = mang.consultaTesoreriaPendianteTranferencia(tesoreria);
            return Json(listaSolicitudesPendienteTesoreria);
        }
        public string AprobarSolicitud(int id)
        {
            ManagerUser mang = new ManagerUser();
            string user = Session["username"].ToString();
            string mensaje = mang.aprobarTesoreriaPendianteTranferencia(id, user, "");
            return mensaje;

        }
        public string VerificarCuenta(int id)
        {
            ManagerUser mang = new ManagerUser();
            string user = Session["username"].ToString();
            string mensaje = mang.verificarCuenta(id, 38, 110, user);
            return mensaje;

        }
    }
}

using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class SolicitudesController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();
        private Solicitudes solicitudes = new Solicitudes();
        //
        // GET: /Solicitudes/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Solicitudes/Details/5

        public ActionResult Details(int id = 0)
        {
            solicitudes.Id = id;
            solicitudes.ListSolicitudes = new System.Collections.Generic.List<Solicitudes>();
            solicitudes.ListProductos = new List<productos>();
            solicitudes.ListTipos = new List<Tipos>();
            //solicitudes.ListSolicitudes.Add(solicitudes);

            //ManagerUser manage = new ManagerUser();
            try
            {
                //solicitudes.ListSolicitudes = manage.CargarSolicitudBuro(solicitudes);
                ViewBag.ListSolicitudes = solicitudes;
            }
            catch
            {
                ViewBag.ErrorMessage = "Ocurrio un Error";
            }
            return View(solicitudes);

        }

        //
        // GET: /Solicitudes/Edit/5

        public ActionResult Buscar(int id = 0)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    ManagerUser mang = new ManagerUser();
                    solicitudes.Id = id;
                    solicitudes.ListSolicitudes = mang.CargarSolicitudBuro(solicitudes);
                    solicitudes.ListProductos = mang.CargarProductos();
                    solicitudes.ListTipos = mang.CargarTipos("5");
                }
            }
            catch
            {
                solicitudes.Respuesta = "Ocurrio un Error";
            }
            //return View(asignacionbuckets);
            return View("Details", solicitudes);
        }

        public ActionResult ActualizaSolicitud(Solicitudes solicitudes)
        {
            ManagerUser mang = new ManagerUser();

            solicitudes.USUARIO_MODIFICACION = Session["agente"].ToString();
            solicitudes.forzamiento_solicitud = true;
            var res = mang.ActualizaSolicitudCredito(solicitudes);

            return Json(res);
        }

        public ActionResult DescargarXmlBuros(Solicitudes solicitudes)
        {
            ManagerUser mang = new ManagerUser();

            var res = mang.DescargarXmlBuro(solicitudes);

            return Json(res);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
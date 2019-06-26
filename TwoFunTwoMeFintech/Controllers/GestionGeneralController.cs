using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMe.Models.Manager;

namespace TwoFunTwoMeFintech.Controllers
{
    public class GestionGeneralController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /GestionGeneral/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /GestionGeneral/Details/5

        public ActionResult Details(int id = 0)
        {
            GestionGeneral gestiongeneral = db.GestionGenerals.Find(id);
            if (gestiongeneral == null)
            {
                return HttpNotFound();
            }
            return View(gestiongeneral);
        }

		public ActionResult Detalle()
		{
			ManagerUser mang = new ManagerUser();
			Tipos _Tipos = new Tipos();
			var ret = mang.TraeTipoGestionGeneral();
			if (ret.Any())
			{
				_Tipos.ListTipos = ret;
			}

			return Json(_Tipos, JsonRequestBehavior.AllowGet);

		}
		//
		// GET: /GestionGeneral/Create

		public ActionResult Buscar(Solicitudes solicitudes)
		{
			ManagerUser mang = new ManagerUser();
			Solicitudes _solicitudes = new Solicitudes();
			_solicitudes.ListSolicitudes = new List<Solicitudes>();

			var ret = mang.ConsultarSolicitudGestionGeneral(solicitudes);
			if (ret.Any())
			{
				_solicitudes.ListSolicitudes = ret;
				_solicitudes.IdAgente = Convert.ToInt32(Session["ROLID"].ToString());
			}


			return Json(_solicitudes, JsonRequestBehavior.AllowGet);

		}
		public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /GestionGeneral/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GestionGeneral gestiongeneral)
        {
            if (ModelState.IsValid)
            {
                db.GestionGenerals.Add(gestiongeneral);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gestiongeneral);
        }

        //
        // GET: /GestionGeneral/Edit/5

        public ActionResult Edit(int id = 0)
        {
            GestionGeneral gestiongeneral = db.GestionGenerals.Find(id);
            if (gestiongeneral == null)
            {
                return HttpNotFound();
            }
            return View(gestiongeneral);
        }

        //
        // POST: /GestionGeneral/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GestionGeneral gestiongeneral)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gestiongeneral).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gestiongeneral);
        }

		//
		// GET: /GestionGeneral/Delete/5
		public ActionResult VerGestiones(GestionGeneral gestionesgeneral)
		{
			ManagerUser mang = new ManagerUser();
			GestionGeneral _solicitudes = new GestionGeneral();
			_solicitudes.ListGestionGeneral = new List<GestionGeneral>();			
			gestionesgeneral.ACCION = "CONSULTAR";
			gestionesgeneral.Cod_Agente = Convert.ToString(Session["agente"]);
			var ret = mang.MantenimientioGestionesGeneral(gestionesgeneral);
			if (ret.Any())
			{
				_solicitudes.ListGestionGeneral = ret;
			}
			return Json(_solicitudes.ListGestionGeneral, JsonRequestBehavior.AllowGet);

		}
		public ActionResult VerGestionesAprobadas(GestionesAprobadas gestionesAprobadas)
		{
			ManagerUser mang = new ManagerUser();
			GestionesAprobadas _solicitudes = new GestionesAprobadas();
			_solicitudes.ListGestionesAprobadas = new List<GestionesAprobadas>();			
			gestionesAprobadas.IdGestionesAprobadas = 0;
			gestionesAprobadas.IdGestionGeneral = 0;
			gestionesAprobadas.ACCION = "CONSULTAR";
			gestionesAprobadas.Cod_Agente = Convert.ToString(Session["agente"]);
			var ret = mang.MantenimientioGestionesAprobadas(gestionesAprobadas);
			if (ret.Any())
			{
				_solicitudes.ListGestionesAprobadas = ret;
			}
			return Json(_solicitudes.ListGestionesAprobadas, JsonRequestBehavior.AllowGet);

		}
		public ActionResult Delete(int id = 0)
        {
            GestionGeneral gestiongeneral = db.GestionGenerals.Find(id);
            if (gestiongeneral == null)
            {
                return HttpNotFound();
            }
            return View(gestiongeneral);
        }

        //
        // POST: /GestionGeneral/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GestionGeneral gestiongeneral = db.GestionGenerals.Find(id);
            db.GestionGenerals.Remove(gestiongeneral);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		public ActionResult Aprobar(GestionesAprobadas gestionesAprobadas)
		{
			ManagerUser mang = new ManagerUser();
			gestionesAprobadas.IdGestionesAprobadas = 0;
			gestionesAprobadas.ACCION = "INSERTAR";
			gestionesAprobadas.Cod_Agente = Convert.ToString(Session["agente"]);
			return Json(mang.MantenimientioGestionesAprobadas(gestionesAprobadas), JsonRequestBehavior.AllowGet);

		}

		public ActionResult guardar (GestionGeneral gestionesgeneral)
		{
			ManagerUser mang = new ManagerUser();
			gestionesgeneral.IdGestionGeneral = 0;
			gestionesgeneral.ACCION = "INSERTAR";
			gestionesgeneral.Cod_Agente = Convert.ToString(Session["agente"]);
			return Json(mang.MantenimientioGestionesGeneral(gestionesgeneral), JsonRequestBehavior.AllowGet);

		}
		public JsonResult EliminarGestion(int? id = null)
		{
			GestionGeneral gestiongeneral = new GestionGeneral();

			gestiongeneral.IdGestionGeneral = id;
			gestiongeneral.IdSolicitud = 0;
			gestiongeneral.Detalle = "";			
			gestiongeneral.Cod_Agente = "";
			gestiongeneral.FechaGestion = null;
			gestiongeneral.TipoGestion = 0;
			gestiongeneral.ACCION = "ELIMINAR";

			ManagerUser mang = new ManagerUser();
			try
			{
				mang.MantenimientioGestionesGeneral(gestiongeneral);
				gestiongeneral.Mensaje = string.Concat("Agente N#: ", id, " Eliminado");
			}
			catch
			{
				gestiongeneral.Mensaje = string.Concat("Ocurrio un error al Eliminar el agente N#: ", id);
			}

			return Json(gestiongeneral);
		}
		public JsonResult EliminarGestionAprobada(int? id = null)
		{
			GestionesAprobadas gestionesAprobadas = new GestionesAprobadas();

			gestionesAprobadas.IdGestionesAprobadas = 0;
			gestionesAprobadas.IdSolicitud = 0;
			gestionesAprobadas.IdGestionGeneral = id;
			gestionesAprobadas.ACCION = "ELIMINAR";

			ManagerUser mang = new ManagerUser();
			try
			{
				mang.MantenimientioGestionesAprobadas(gestionesAprobadas);
				gestionesAprobadas.Mensaje = string.Concat("Agente N#: ", id, " Eliminado");
			}
			catch
			{
				gestionesAprobadas.Mensaje = string.Concat("Ocurrio un error al Eliminar el agente N#: ", id);
			}

			return Json(gestionesAprobadas);
		}
	}
}
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
    public class MAINMENUController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /MAINMENU/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /MAINMENU/Details/5

        public ActionResult Details(int id = 0)
        {        
            return View();
        }

		public ActionResult Detalle(string id = null)
		{
			ManagerUser manage = new ManagerUser();
			clsMAINMENU mainmenu = new clsMAINMENU();
			mainmenu.ACCION = "CONSULTAR";
			var ret = manage.MantenimientoMAINMENU(mainmenu);
			return Json(ret, JsonRequestBehavior.AllowGet);
		}
		//
		// GET: /MAINMENU/Create

		public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MAINMENU/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(clsMAINMENU clsmainmenu)
        {
			ManagerUser manage = new ManagerUser();
			clsmainmenu.ACCION = "INSERTAR";
			var ret = manage.MantenimientoMAINMENU(clsmainmenu);

			return View();
		}
		public ActionResult Crear(clsMAINMENU clsmainmenu)
		{
			ManagerUser manage = new ManagerUser();
			clsmainmenu.ACCION = "INSERTAR";
			return Json(manage.MantenimientoMAINMENU(clsmainmenu));
		}
		//
		// GET: /MAINMENU/Edit/5

		public ActionResult Edit(int id = 0)
        {
			ManagerUser manage = new ManagerUser();
			clsMAINMENU mainmenu = new clsMAINMENU();
			mainmenu.ID = id;
			mainmenu.MAINMENU = "";
			mainmenu.ACCION = "CONSULTAR";
			var ret = manage.MantenimientoMAINMENU(mainmenu);
			// Json(ret);
			return Json(ret, JsonRequestBehavior.AllowGet);
		}

        //
        // POST: /MAINMENU/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(clsMAINMENU clsmainmenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clsmainmenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clsmainmenu);
        }

		public ActionResult Update(clsMAINMENU clsmainmenu)
		{
			ManagerUser mang = new ManagerUser();
			clsmainmenu.ACCION = "ACTUALIZAR";
			return Json(mang.MantenimientoMAINMENU(clsmainmenu));
		}
		//
		// GET: /MAINMENU/Delete/5

		public ActionResult Delete(int id = 0)
        {
            clsMAINMENU clsmainmenu = db.clsMAINMENUs.Find(id);
            if (clsmainmenu == null)
            {
                return HttpNotFound();
            }
            return View(clsmainmenu);
        }

		public ActionResult Eliminar(int id = 0)
		{
			ManagerUser manage = new ManagerUser();
			clsMAINMENU mainmenu = new clsMAINMENU();
			mainmenu.ID = id;
			mainmenu.MAINMENU = "";
			mainmenu.ACCION = "ELIMINAR";


			try
			{
				var ret = manage.MantenimientoMAINMENU(mainmenu);
				mainmenu.Mensaje = string.Concat("Rol N#: ", id, " Eliminado");
			}
			catch
			{
				mainmenu.Mensaje = string.Concat("Ocurrio un error al Eliminar el agente N#: ", id);
			}

			return Json(mainmenu);

			//return View();
		}
		//
		// POST: /MAINMENU/Delete/5

		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            clsMAINMENU clsmainmenu = db.clsMAINMENUs.Find(id);
            db.clsMAINMENUs.Remove(clsmainmenu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
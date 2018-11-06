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
    public class RulesController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /Rules/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Rules/Details/5

        public ActionResult Details(long id = 0)
        {
            return View();
        }

        //
        // GET: /Rules/Create

        public ActionResult Create()
        {
            return View();
        }

		public ActionResult Detalle(string id = null)
		{
			ManagerUser manage = new ManagerUser();
			Rules rul = new Rules();
			rul.ACCION = "CONSULTAR";
			var ret = manage.MantenimientoRules(rul);
			return Json(ret, JsonRequestBehavior.AllowGet);
		}
		//
		// POST: /Rules/Create

		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Rules rules)
        {
			ManagerUser manage = new ManagerUser();
			rules.ACCION = "INSERTAR";
			var ret = manage.MantenimientoRules(rules);

			return View();
		}

        //
        // GET: /Rules/Edit/5

        public ActionResult Edit(long id = 0)
        {
			ManagerUser manage = new ManagerUser();
			Rules rul = new Rules();
			rul.IdRule = id;
			rul.Condicion = "";
			rul.Activo = null;
			rul.ACCION = "CONSULTAR";
			var ret = manage.MantenimientoRules(rul);
			// Json(ret);
			return Json(ret, JsonRequestBehavior.AllowGet);
		}

        //
        // POST: /Rules/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Rules rules)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rules).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rules);
        }

		public ActionResult Update(Rules rules)
		{
			ManagerUser mang = new ManagerUser();
			rules.ACCION = "ACTUALIZAR";
			return Json(mang.MantenimientoRules(rules));
		}
		//
		// GET: /Rules/Delete/5

		public ActionResult Eliminar(int id = 0)
		{
			ManagerUser manage = new ManagerUser();
			Rules rul = new Rules();
			rul.IdRule = id;
			rul.Descripcion = "";
			rul.Condicion = "";
			rul.ACCION = "ELIMINAR";


			try
			{
				var ret = manage.MantenimientoRules(rul);
				rul.Mensaje = string.Concat("Rol N#: ", id, " Eliminado");
			}
			catch
			{
				rul.Mensaje = string.Concat("Ocurrio un error al Eliminar el agente N#: ", id);
			}

			return Json(rul);

			//return View();
		}


		public ActionResult Delete(long id = 0)
        {
            Rules rules = db.Rules.Find(id);
            if (rules == null)
            {
                return HttpNotFound();
            }
            return View(rules);
        }

        //
        // POST: /Rules/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Rules rules = db.Rules.Find(id);
            db.Rules.Remove(rules);
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
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
    public class SUBMENUController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /SUBMENU/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /SUBMENU/Details/5

        public ActionResult Details(int id = 0)
        {
            return View();
        }

		public ActionResult Detalle(int? id = null)
		{
			ManagerUser mang = new ManagerUser();
			clsSUBMENU _SubMenu = new clsSUBMENU();
			_SubMenu.listadoSubMenu = new List<clsSUBMENU>();
			var logSubMenu = new clsSUBMENU
			{
				ID = id,
				SUBMENU = string.Empty,
				CONTROLLER = string.Empty,
				MAINMENUID = 0,
				ROLEID = 0,
				ACTION = string.Empty,
				ACCION = "CONSULTAR"
			};
			var ret = mang.MantenimientoSUBMENU(logSubMenu);
			if (ret.Any())
			{
				_SubMenu.listadoSubMenu = ret;	
			}

			return Json(_SubMenu.listadoSubMenu, JsonRequestBehavior.AllowGet);

		}
		//
		// GET: /SUBMENU/Create

		public ActionResult Create()
        {
			ManagerUser mang = new ManagerUser();
			clsSUBMENU _SubMenu = new clsSUBMENU();
			_SubMenu.listadoMainMenu = new List<clsMAINMENU>();
			_SubMenu.ListRoles = new List<Roles>();
			
			var logMainMenu = new clsMAINMENU
			{
				ID = 0,
				MAINMENU = string.Empty,
				ACCION = "CONSULTAR"
			};
			var ret1 = mang.MantenimientoMAINMENU(logMainMenu);
			var ret2 = mang.GetUserRoles();
			if (ret1.Any() && ret2.Any())
			{
				_SubMenu.listadoMainMenu = ret1;
				_SubMenu.ListRoles = ret2;
			}
			return View(_SubMenu);
        }

        //
        // POST: /SUBMENU/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(clsSUBMENU clssubmenu)
        {
			ManagerUser manage = new ManagerUser();
			clssubmenu.ACCION = "INSERTAR";
			var ret = manage.MantenimientoSUBMENU(clssubmenu);

			return View();
		}

		public ActionResult Crear(clsSUBMENU clssubmenu)
		{
			ManagerUser manage = new ManagerUser();
			clssubmenu.ACCION = "INSERTAR";
	        return Json(manage.MantenimientoSUBMENU(clssubmenu));
		}
		//
		// GET: /SUBMENU/Edit/5

		public ActionResult Edit(int id = 0)
        {
			ManagerUser mang = new ManagerUser();
			clsSUBMENU _SubMenu = new clsSUBMENU();
			_SubMenu.listadoSubMenu = new List<clsSUBMENU>();
			_SubMenu.listadoMainMenu = new List<clsMAINMENU>();
			_SubMenu.ListRoles = new List<Roles>();
			var logSubMenu = new clsSUBMENU
			{
				ID = id,
				SUBMENU = string.Empty,
				CONTROLLER = string.Empty,
				MAINMENUID = 0,
				ROLEID = 0,
				ACTION = string.Empty,
				ACCION = "CONSULTAR"
			};
			var logMainMenu = new clsMAINMENU
			{
				ID = 0,
				MAINMENU = string.Empty,
				ACCION = "CONSULTAR"
			};
			var ret = mang.MantenimientoSUBMENU(logSubMenu);
			var ret1 = mang.MantenimientoMAINMENU(logMainMenu);
			var ret2 = mang.GetUserRoles();
			if (ret.Any() && ret1.Any() && ret2.Any())
			{
				_SubMenu.listadoSubMenu = ret;
				_SubMenu.listadoMainMenu = ret1;
				_SubMenu.ListRoles = ret2;
			}

			return Json(_SubMenu, JsonRequestBehavior.AllowGet);
		}

        //
        // POST: /SUBMENU/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(clsSUBMENU clssubmenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clssubmenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clssubmenu);
        }

		public ActionResult Update(clsSUBMENU clssubmenu)
		{
			ManagerUser mang = new ManagerUser();
			clssubmenu.ACCION = "ACTUALIZAR";
			return Json(mang.MantenimientoSUBMENU(clssubmenu));
		}
		//
		// GET: /SUBMENU/Delete/5

		public ActionResult Eliminar(int id = 0)
		{
			ManagerUser manage = new ManagerUser();
			clsSUBMENU submenu = new clsSUBMENU();
			submenu.ID = id;
			submenu.SUBMENU = "";
			submenu.CONTROLLER = "";
			submenu.MAINMENUID = 0;
			submenu.ROLEID = 0;
			submenu.ACTION = "";
			submenu.ACCION = "ELIMINAR";
			try
			{
				var ret = manage.MantenimientoSUBMENU(submenu);
				submenu.Mensaje = string.Concat("Rol N#: ", id, " Eliminado");
			}
			catch
			{
				submenu.Mensaje = string.Concat("Ocurrio un error al Eliminar el agente N#: ", id);
			}

			return Json(submenu);

			//return View();
		}

		public ActionResult Delete(int id = 0)
        {
            clsSUBMENU clssubmenu = db.clsSUBMENUs.Find(id);
            if (clssubmenu == null)
            {
                return HttpNotFound();
            }
            return View(clssubmenu);
        }

        //
        // POST: /SUBMENU/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            clsSUBMENU clssubmenu = db.clsSUBMENUs.Find(id);
            db.clsSUBMENUs.Remove(clssubmenu);
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
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
    public class AgenteMenuController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /AgenteMenu/

        public ActionResult Index()
        {
		    return View();
				
        }

        //
        // GET: /AgenteMenu/Details/5

        public ActionResult Details(int id = 0)
        {
            AgenteMenu agentemenu = db.AgenteMenus.Find(id);
            if (agentemenu == null)
            {
                return HttpNotFound();
            }
            return View(agentemenu);
        }

		public ActionResult Detalle(string id = null)
		{
			ManagerUser mang = new ManagerUser();
			AgenteMenu agentemenu = new AgenteMenu();
			agentemenu.ListAgenteMenuAsig = new List<AgenteMenu>();
			agentemenu.ListAgenteMenuDesAsig = new List<AgenteMenu>();
			agentemenu.Listdto_Login = new List<dto_login>();
			var age = new AgenteMenu
			{
				COD_AGENTE = id,
				IDSUBMENU = null,
				ACCION = "CONSULTAR"
			};
			var log = new dto_login
			{
				cod_agente = id,
				pass = string.Empty,
				correo = string.Empty
			};
			var agentasig = mang.ConsultaSubmenusAsignados(age);
			var agentedesasig = mang.ConsultaSubmenusDesAsignados(age);
			var Usuario = mang.ConsultaUsuarios(log);
			if (agentasig.Any() )
			{
				agentemenu.ListAgenteMenuAsig = agentasig;
				
			}
			 if(agentedesasig.Any())
			{
				agentemenu.ListAgenteMenuDesAsig = agentedesasig;
			}
			 if (Usuario.Any())
			{
				agentemenu.Listdto_Login = Usuario;
			}

			return Json(agentemenu, JsonRequestBehavior.AllowGet);

		}
		//
		// GET: /AgenteMenu/Create

		public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /AgenteMenu/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AgenteMenu agentemenu)
        {
            if (ModelState.IsValid)
            {
                db.AgenteMenus.Add(agentemenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agentemenu);
        }

		public ActionResult Asignar(List<AgenteMenu> listAsignado)
		{
			ManagerUser mang = new ManagerUser();
			AgenteMenu agentemenu = new AgenteMenu();

			listAsignado.ForEach(x=>
			{
				if(x.IDSUBMENU != null)
				{ mang.ConsultaSubmenusAsignados(x); }
				
			});
			
			return Json(listAsignado, JsonRequestBehavior.AllowGet);

		}
		//
		// GET: /AgenteMenu/Edit/5

		public ActionResult Edit(int id = 0)
        {
            AgenteMenu agentemenu = db.AgenteMenus.Find(id);
            if (agentemenu == null)
            {
                return HttpNotFound();
            }
            return View(agentemenu);
        }

        //
        // POST: /AgenteMenu/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AgenteMenu agentemenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agentemenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agentemenu);
        }

        //
        // GET: /AgenteMenu/Delete/5

        public ActionResult Delete(int id = 0)
        {
            AgenteMenu agentemenu = db.AgenteMenus.Find(id);
            if (agentemenu == null)
            {
                return HttpNotFound();
            }
            return View(agentemenu);
        }

        //
        // POST: /AgenteMenu/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgenteMenu agentemenu = db.AgenteMenus.Find(id);
            db.AgenteMenus.Remove(agentemenu);
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
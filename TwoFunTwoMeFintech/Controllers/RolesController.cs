using System.Data;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class RolesController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /Roles/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Roles/Details/5

        public ActionResult Details()
        {
            return View();
        }

		public ActionResult Detalle(string id = null)
		{
			ManagerUser manage = new ManagerUser();
			Roles rol = new Roles();
			rol.ACCION = "CONSULTAR";
			var ret = manage.MantenimientoRoles(rol);
			return Json(ret,JsonRequestBehavior.AllowGet);
		}

		//
		// GET: /Roles/Create

		public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Roles/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Roles roles)
        {
			ManagerUser manage = new ManagerUser();
			roles.ACCION = "INSERTAR";
			var ret = manage.MantenimientoRoles(roles);

			return View();
		}

        //
        // GET: /Roles/Edit/5

        public ActionResult Edit(int id = 0)
        {
			ManagerUser manage = new ManagerUser();
			Roles rol = new Roles();
			rol.ID = id;
			rol.ACCION = "CONSULTAR";
			var ret = manage.MantenimientoRoles(rol);
			// Json(ret);
			return Json(ret, JsonRequestBehavior.AllowGet);
		}

        //
        // POST: /Roles/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Roles roles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roles);
        }

		public ActionResult Update(Roles roles)
		{
			ManagerUser mang = new ManagerUser();
			roles.ACCION = "ACTUALIZAR";
			return Json(mang.MantenimientoRoles(roles));
		}

        [HttpPost]
        public ActionResult Actualiza(Roles roles)
        {
            ManagerUser mang = new ManagerUser();
            roles.ACCION = "ACTUALIZAR";
            return Json(mang.MantenimientoRoles(roles));
        }
        //
        // GET: /Roles/Delete/5

        public ActionResult Eliminar(int id = 0)
        {
			ManagerUser manage = new ManagerUser();
			Roles rol = new Roles();
			rol.ID = id;
			rol.ROLESNOMBRE = "";
			rol.ACCION = "ELIMINAR";
			

			try
			{
				var ret = manage.MantenimientoRoles(rol);
				rol.Mensaje = string.Concat("Rol N#: ", id, " Eliminado");
			}
			catch
			{
				rol.Mensaje = string.Concat("Ocurrio un error al Eliminar el agente N#: ", id);
			}

			return Json(rol);

			//return View();
        }

		//
		// POST: /Roles/Delete/5

		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Roles roles = db.Roles.Find(id);
            db.Roles.Remove(roles);
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
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
    public class AsignacionBucketsController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /AsignacionBuckets/

        public ActionResult Index()
        {
            return View(db.AsignacionBuckets.ToList());
        }

        //
        // GET: /AsignacionBuckets/Details/5

        public ActionResult Details(int id = 0)
        {
            ManagerUser mang = new ManagerUser();
            AsignacionBuckets asignacionbuckets = new AsignacionBuckets();
            asignacionbuckets.listadoBucket = new List<AsignacionBuckets>();

            var ret = mang.GetAsignacionBuckets();
            if (ret.Any())
            {
                asignacionbuckets.listadoBucket = ret;
            }

            return View(asignacionbuckets);
        }

        //
        // GET: /AsignacionBuckets/Create

        public ActionResult Create()
        {
            ManagerUser mang = new ManagerUser();
            AsignacionBuckets asignacionbuckets = new AsignacionBuckets();
            asignacionbuckets.listadoBucket = new List<AsignacionBuckets>();

            var ret = mang.GetAsignacionBuckets();
            if (ret.Any())
            {
                asignacionbuckets.listadoBucket = ret;
            }

            return View(asignacionbuckets);
        }

        //
        // POST: /AsignacionBuckets/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AsignacionBuckets asignacionbuckets)
        {
            if (ModelState.IsValid)
            {
                var asignacion = new AsignacionBuckets();
                ManagerUser mang = new ManagerUser();
                try
                {
                    asignacion = mang.InsertarBuckets(asignacionbuckets);
                    //db.AsignacionBuckets.Add(asignacionbuckets);
                    //db.SaveChanges();
                    ViewBag.Mensaje = asignacion.Mensaje;
                }
                catch
                {
                    ViewBag.MensajeError = asignacion.MensajeError;
                }
                return View();

            }

            return View(asignacionbuckets);
        }

        //
        // GET: /AsignacionBuckets/Edit/5

        public ActionResult Edit(int? id = null)
        {
            //AsignacionBuckets asignacionbuckets = db.AsignacionBuckets.Find(id);
            //if (asignacionbuckets == null)
            //{
            //    return HttpNotFound();
            //}
            ManagerUser mang = new ManagerUser();
            //AsignacionBuckets asignacionbuckets = new AsignacionBuckets();

            var asignacionbuckets = mang.GetAsignacionBuckets(id);
            return View(asignacionbuckets.FirstOrDefault());
        }

        //
        // POST: /AsignacionBuckets/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AsignacionBuckets asignacionbuckets)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    ManagerUser mang = new ManagerUser();
                    asignacionbuckets.Respuesta = mang.ActualizarBuckets(asignacionbuckets);

                    //db.Entry(asignacionbuckets).State = EntityState.Modified;
                    //db.SaveChanges();
                    //return RedirectToAction("Index");
                }
            }
            catch
            {
                asignacionbuckets.Respuesta = false;
            }
            return View(asignacionbuckets);
        }

        //
        // GET: /AsignacionBuckets/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ManagerUser mang = new ManagerUser();
            //AsignacionBuckets asignacionbuckets = new AsignacionBuckets();

            var asignacionbuckets = mang.GetAsignacionBuckets(id);
            return View(asignacionbuckets.FirstOrDefault());

        }

        //
        // POST: /AsignacionBuckets/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var asignacionbuckets = new AsignacionBuckets();
            asignacionbuckets.id = id;
            try
            {
                ManagerUser mang = new ManagerUser();
                asignacionbuckets.Respuesta = mang.EliminaBuckets(asignacionbuckets);


            }
            catch
            {
                asignacionbuckets.Respuesta = false;
            }
            //return View(asignacionbuckets);

            return RedirectToAction("Details");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    base.OnActionExecuting(filterContext);

        //}
    }
}
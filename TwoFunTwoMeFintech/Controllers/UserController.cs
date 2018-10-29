using ModeAuthentication.Models.Utilitarios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class UserController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(string id = null)
        {
            //ManagerUser mang = new ManagerUser();
            //dto_login dto_login = new dto_login();
            //dto_login.listadoDto_login = new List<dto_login>();
            //var log = new dto_login
            //{
            //	cod_agente = string.Empty,
            //	pass = string.Empty,
            //	correo = string.Empty
            //};
            //var ret = mang.ConsultaUsuarios(log);
            //if (ret.Any())
            //{
            //	dto_login.listadoDto_login = ret;
            //}
            //return Json(dto_login.listadoDto_login, JsonRequestBehavior.AllowGet);
            return View();
        }
        public ActionResult Detalle(string id = null)
        {
            ManagerUser mang = new ManagerUser();
            dto_login dto_login = new dto_login();
            dto_login.listadoDto_login = new List<dto_login>();
            dto_login.ListRoles = new List<Roles>();
            var log = new dto_login
            {
                cod_agente = string.Empty,
                pass = string.Empty,
                correo = string.Empty
            };
            var ret = mang.ConsultaUsuarios(log);
            var ret1 = mang.GetUserRoles();
            if (ret.Any())
            {
                dto_login.listadoDto_login = ret;
                dto_login.ListRoles = ret1;
            }

            return Json(dto_login.listadoDto_login, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult CargaRoles()
        //{
        //	ManagerUser mang = new ManagerUser();
        //	List<Roles> Roles = new List<Roles>();
        //	var ret = mang.GetUserRoles();
        //	if (ret.Any())
        //	{
        //		Roles = ret;

        //	}
        //	return Json(Roles, JsonRequestBehavior.AllowGet);

        //}
        //
        // GET: /User/Create

        public ActionResult Create()
        {
			ManagerUser mang = new ManagerUser();
			dto_login dto_login = new dto_login();			
			dto_login.ListRoles = new List<Roles>();
			Limpiar();
			var ret = mang.GetUserRoles();
			if (ret.Any())
			{				
				dto_login.ListRoles = ret;
			}
			return View(dto_login);
        }

        //
        // POST: /User/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(dto_login dto_login)
        {
            if (ModelState.IsValid)
            {

                ManagerUser mang = new ManagerUser();						
				dto_login.ListRoles = new List<Roles>();				
				dto_login.pass = Cryption.Encrypt(dto_login.pass, ConfigurationManager.AppSettings["claveEncriptacion"]);

                mang.Registrar(dto_login);
				dto_login = Limpiar();

				var ret = mang.GetUserRoles();
				if (ret.Any())
				{
					dto_login.ListRoles = ret;
				}
			}			
			return View(dto_login);
		}

        //
        // GET: /User/Edit/5

        public ActionResult Edit(string id)
        {
            ManagerUser mang = new ManagerUser();
            dto_login dto_login = new dto_login();
            dto_login.listadoDto_login = new List<dto_login>();
            var log = new dto_login
            {
                cod_agente = id,
                pass = string.Empty,
                correo = string.Empty
            };
            var ret = mang.ConsultaUsuarios(log);
            dto_login.ListRoles = mang.GetUserRoles();


            if (ret.Any())
            {
                dto_login.listadoDto_login = ret;
            }

            return Json(dto_login, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(dto_login dto_login)
        {
            ManagerUser mang = new ManagerUser();
            if (ModelState.IsValid)
            {
                mang.ConsultaUsuarios(dto_login);
            }
            return View();
        }

        public JsonResult Update(dto_login dto_login)
        {
            ManagerUser mang = new ManagerUser();
            return Json(mang.Registrar(dto_login), JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /User/Delete/5

        public JsonResult Eliminar(string id = null)
        {
            dto_login dto_login = new dto_login();

            dto_login.cod_agente = id;
            dto_login.estado = "Inactivo";

            ManagerUser mang = new ManagerUser();
            try
            {
                mang.UpdateBuckets(dto_login);
                dto_login.Mensaje = string.Concat("Agente N#: ", id, " Eliminado");
            }
            catch
            {
                dto_login.Mensaje = string.Concat("Ocurrio un error al Eliminar el agente N#: ", id);
            }

            return Json(dto_login);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            dto_login dto_login = db.dto_login.Find(id);
            db.dto_login.Remove(dto_login);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

		private dto_login Limpiar()
		{
			dto_login dto_Login = new dto_login();
			dto_Login.cod_agente = "";
			dto_Login.ConfirmPassword = 0;
			dto_Login.correo = "";
			dto_Login.estado = "";
			dto_Login.nombre = "";
			dto_Login.pass = "";
			dto_Login.ROLID = 0;
			dto_Login.vendedor = "";

			return dto_Login;
		}
	}
}
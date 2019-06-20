using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.ListaNegra;

namespace TwoFunTwoMeFintech.Controllers.ListaNegra
{
    public class ListaNegraController : Controller
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
           
            return View();
        }
        public ActionResult Detalle(string id = null)
        {
            ManagerListaNegra mang = new ManagerListaNegra();
            dto_ListaNegra objListaNegra = new dto_ListaNegra();
      
            var log = new dto_ListaNegra()
            {
                Identificacion = string.Empty
            };
            var ret = mang.ConsultaListaNegra(objListaNegra);
            objListaNegra.list_ListaNegra = new List<dto_ListaNegra>();
            if (ret.Any())
            {
                objListaNegra.list_ListaNegra = ret;
            }

            return Json(objListaNegra.list_ListaNegra, JsonRequestBehavior.AllowGet);

        }

        

        public ActionResult Create()
        {
			ManagerUser mang = new ManagerUser();
			dto_ListaNegra dto_login = new dto_ListaNegra();			
			//dto_login.ListRoles = new List<Roles>();
			//Limpiar();
			//var ret = mang.GetUserRoles();
			//if (ret.Any())
			//{				
			//	dto_login.ListRoles = ret;
			//}
			return View(dto_login);
        }

		public ActionResult Crear(dto_ListaNegra dto_login)
		{
			if (ModelState.IsValid)
			{

				ManagerListaNegra mang = new ManagerListaNegra();
				//dto_login.ListRoles = new List<Roles>();
				//dto_login.pass = TwoFunTwoMe_DataAccess.Utility.Encrypt(dto_login.pass);

				mang.MantenimientoListaNegra(dto_login);
				//dto_login = Limpiar();
			}
			return Json(dto_login);
		}
		//
		// GET: /User/Edit/5

		public ActionResult Edit(string id)
        {
            ManagerListaNegra mang = new ManagerListaNegra();
            dto_ListaNegra dto_login = new dto_ListaNegra();
            dto_login.list_ListaNegra = new List<dto_ListaNegra>();
            var log = new dto_ListaNegra
            {
                Id = id
            };
            var ret = mang.ConsultaListaNegra(log);

            if (ret.Any())
            {
                dto_login.list_ListaNegra = ret;
            }

            return Json(dto_login, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(dto_ListaNegra dto)
        {
            ManagerListaNegra mang = new ManagerListaNegra();
            if (ModelState.IsValid)
            {
                mang.ConsultaListaNegra(dto);
            }
            return View();
        }

        public JsonResult MantenimientoListaNegra(dto_ListaNegra dto)
        {
            //Obtiene el agente en session
            dto.UsrModifica = Session["agente"].ToString();
            ManagerListaNegra mang = new ManagerListaNegra();
            return Json(mang.MantenimientoListaNegra(dto), JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /User/Delete/5

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


	}
}
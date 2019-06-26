using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models.DTO;
using TwoFunTwoMeFintech.Models.DTO.ProductosMantenimiento;

namespace TwoFunTwoMeFintech.Controllers.Producto
{
    public class ProductoController : Controller
    {
        //
        // GET: /Producto/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Producto/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Producto/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Producto/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Producto/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Producto/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Producto/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Producto/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

		public ActionResult CargaMontosProductos(string id = null)
		{
			ManagerSolcitudes manage = new ManagerSolcitudes();
			try
			{
				if (Session["agente"] == null)
					return RedirectToAction("LogOff", "Login");
				MontoCredito montoCredito = new MontoCredito();
				var result = manage.CargarProductos(montoCredito);
				if (result == null)
				{
					List<MontoCredito> objetoNulo = new List<MontoCredito>();
					objetoNulo.FirstOrDefault().Monto = 0;
					result = objetoNulo;
				}
				return Json(result);
			}
			catch
			{
				throw;
			}
		}

		public ActionResult ListarProductos(string Id)
		{
			ManagerSolcitudes manage = new ManagerSolcitudes();
            ProductosMantenimiento productos = new ProductosMantenimiento();
			try
			{
				productos.MontoProducto = Convert.ToDecimal(Id);
				if (Session["agente"] == null)
					return RedirectToAction("LogOff", "Login");

				var result = manage.ConsultaProductos(productos);
				if (result == null)
				{
					List<ProductosMantenimiento> objetoNulo = new List<ProductosMantenimiento>();
					objetoNulo.FirstOrDefault().Id = 0;
					result = objetoNulo;
				}
				return Json(result, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				throw;
			}
		}
		public ActionResult CrearProducto(ProductosMantenimiento productosMantenimiento)
		{
			ManagerUser manage = new ManagerUser();
			if (Session["agente"] == null)
				return RedirectToAction("LogOff", "Login");
			productosMantenimiento.UsrModifica = Session["agente"].ToString();
			var ret = manage.CrearProducto(productosMantenimiento);
			return Json(ret);
		}
		public ActionResult ObtieneProducto(string id)
		{
			ProductosMantenimiento productosMantenimiento = new ProductosMantenimiento();
			productosMantenimiento.Id = Convert.ToInt32(id);
			ManagerUser manage = new ManagerUser();
			if (Session["agente"] == null)
				return RedirectToAction("LogOff", "Login");
			var ret = manage.ConsultaProducto(productosMantenimiento);
			return Json(ret, JsonRequestBehavior.AllowGet);
		}
		public ActionResult ActualizarPruducto(ProductosMantenimiento productosMantenimiento)
		{
			ManagerUser manage = new ManagerUser();
			if (Session["agente"] == null)
				return RedirectToAction("LogOff", "Login");
			productosMantenimiento.UsrModifica = Session["agente"].ToString();
			if (productosMantenimiento.Activo == null) productosMantenimiento.Activo = false;
			var ret = manage.ActualizarProducto(productosMantenimiento);
			return Json(ret);
		}
	}
}

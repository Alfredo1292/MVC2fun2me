using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TwoFunTwoMeFintech.Models.Manager;
using TwoFunTwoMeFintech.Models.DTO;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace TwoFunTwoMeFintech.Controllers.HistorialCobros
{
    public class HistorialCobrosController : Controller
    {
        public object MessageBox { get; private set; }

        //
        // GET: /HistorialCobros/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConsultaHistorialCobros(DTOHistorialCobros cobros)
        {
            ManagerHistorialCobros mang = new ManagerHistorialCobros();
            var listaHistorial = mang.consultaHistorialCobros(cobros);
            return Json(listaHistorial);
        }      
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class TranferenciasFinanzasController : Controller
    {
        //
        // GET: /TranferenciasFinanzas/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetTranferencias(TransferenciasFinanzas transferencias)
        {
            ManagerUser mang = new ManagerUser();
            var listaSTranferencias = mang.consultatranferenciasFinanzas(transferencias);
            return Json(listaSTranferencias);
        }
    }
}

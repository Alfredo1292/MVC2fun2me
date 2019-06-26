using System.Web.Mvc;
using TwoFunTwoMeFintech.Models.DTO.AnalisisRules;
using TwoFunTwoMeFintech.Models.Manager;

namespace TwoFunTwoMeFintech.Controllers.AnalisisRules
{
    public class AnalisisRulesController : Controller
    {
        //
        // GET: /AnalisisRules/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConsultaRules(ConsultaRules rule)
        {
            ManagerRules mang = new ManagerRules();
            var listaHistorial = mang.consultaRules(rule);
            return Json(listaHistorial);
        }

    }
}

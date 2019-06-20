using System.Data;
using System.Linq;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class ScoreCardExpertoController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /ScoreCardExperto/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ScoreCardExperto/Details/5

        public ActionResult Details(int? id = null)
        {
            return View();
        }

        public ActionResult Detalle(int? id = null)
        {
            ManagerUser manage = new ManagerUser();

            ScoreCardExperto scorecardexperto = new ScoreCardExperto();
            var retorno = manage.ConsultaScoreCardExperto(scorecardexperto);

            return Json(retorno);
        }

        public ActionResult Update(ScoreCardExperto scoreCardExperto)
        {
            ManagerUser manage = new ManagerUser();

            var retorno = manage.ActualizaScoreCardExperto(scoreCardExperto);

            return Json(retorno);
        }
        public ActionResult Editar(ScoreCardExperto scoreCardExperto)
        {
            ManagerUser manage = new ManagerUser();
            var retorno = manage.ConsultaScoreCardExperto(scoreCardExperto);

            return Json(retorno.FirstOrDefault());
        }
        public ActionResult Eliminar(ScoreCardExperto scoreCardExperto)
        {
            ManagerUser manage = new ManagerUser();

            var retorno = manage.EliminarScoreCardExperto(scoreCardExperto);

            return Json(retorno);
        }



        //
        // GET: /ScoreCardExperto/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ScoreCardExperto/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ScoreCardExperto scorecardexperto)
        {
            if (ModelState.IsValid)
            {
                ManagerUser manage = new ManagerUser();

                var retorno = manage.InsertarScoreCardExperto(scorecardexperto);

            }
            ViewBag.scorecardexperto = scorecardexperto;
            return View(scorecardexperto);
        }

        //
        // GET: /ScoreCardExperto/Edit/5

        public ActionResult Edit(long id = 0)
        {
            ScoreCardExperto scorecardexperto = db.ScoreCardExpertoes.Find(id);
            if (scorecardexperto == null)
            {
                return HttpNotFound();
            }
            return View(scorecardexperto);
        }

        //
        // POST: /ScoreCardExperto/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ScoreCardExperto scorecardexperto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scorecardexperto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scorecardexperto);
        }

        //
        // GET: /ScoreCardExperto/Delete/5

        public ActionResult Delete(long id = 0)
        {
            ScoreCardExperto scorecardexperto = db.ScoreCardExpertoes.Find(id);
            if (scorecardexperto == null)
            {
                return HttpNotFound();
            }
            return View(scorecardexperto);
        }

        //
        // POST: /ScoreCardExperto/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ScoreCardExperto scorecardexperto = db.ScoreCardExpertoes.Find(id);
            db.ScoreCardExpertoes.Remove(scorecardexperto);
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
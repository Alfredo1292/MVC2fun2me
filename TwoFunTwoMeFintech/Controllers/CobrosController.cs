using System.Data;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;
using System.Linq;

namespace TwoFunTwoMeFintech.Controllers
{
    public class CobrosController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /Cobros/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult BuscarCola()
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var agenteIniciarCola = new BucketCobros
            {
                cod_agente = Session["agente"].ToString()
            };

            var dto_ret = manage.ColaAutomaticaCobros(agenteIniciarCola);
            return Json(dto_ret);

        }

        public ActionResult BuscarCobro(BucketCobros bucketCobros)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var dto_ret = manage.ObtenerCobros(bucketCobros);
            return Json(dto_ret);
        }

        public ActionResult BuscarCliente(BucketCobros bucketCobros)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var dto_ret = manage.ObtenerEncabezado(bucketCobros).FirstOrDefault();
            return Json(dto_ret);
        }


        public ActionResult ActualizaColaAutomatica(BucketCobros bucketCobros)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            bucketCobros.AgenteAsignado = Session["agente"].ToString();
            manage.ActualizaColaAutomaticaCobros(bucketCobros);

            return Json(new { Resultado = true });
        }


        public ActionResult comboTipo()
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.monstrarAciones();

            return Json(ret);
        }

        public ActionResult ResultadoLLamada()
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.mostrarResultadoLLamada();

            return Json(ret);
        }


        public ActionResult GuardarGestionCobro(GestionCobro cobro)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            cobro.UsrModifica = Session["agente"].ToString();
            try
            {
                manage.GuardaCobro(cobro);
                return Json(new { Resultado = true });
            }
            catch
            {
                return Json(new { Resultado = false });
            }
        }



        public ActionResult Contactos(contacto contac)

        {
            if (contac.Identificacion == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarContactos(contac);

            return Json(ret);
        }

        public ActionResult HistoricoGestiones(Tabla_Accion tabla_Accion)

        {
            if (tabla_Accion.IdCredito == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultaHistoricoGestiones(tabla_Accion);

            return Json(ret);
        }

        public ActionResult HistoricoPromesasPagos(Tabla_Accion tabla_Accion)
        {
            if (tabla_Accion.IdCredito == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultaPromesasPagos(tabla_Accion);

            return Json(ret);
        }

        public ActionResult HistoricoPagos(Tabla_Accion tabla_Accion)
        {
            if (tabla_Accion.IdCredito == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultaPagos(tabla_Accion);

            return Json(ret);
        }

        public ActionResult conultar_saldo_monto_pendiente(Tabla_Accion tabla_Accion)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            tabla_Accion.cod_agente = Session["agente"].ToString();
            var ret = manage.ConsultaSaldoMontoPendiente(tabla_Accion);

            return Json(ret);
        }
        public ActionResult conultar_saldo_monto_procesado(Tabla_Accion tabla_Accion)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            tabla_Accion.cod_agente = Session["agente"].ToString();
            var ret = manage.ConsultaSaldoMontoProcesado(tabla_Accion);

            return Json(ret);
        }
        public ActionResult conultar_Datos_creditos(InformacionCuenta tabla_Accion)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.consultaInformacionCuenta(tabla_Accion);

            return Json(ret);
        }

        public ActionResult mostrar_imagenes(Solicitudes solicitudes)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.MostrarImagenesCreddid(solicitudes);

            return Json(ret);
        }

        public ActionResult consultarGridCreditos(InformacionCuenta tabla_Accion)
        {
            if (tabla_Accion.IdCredito == null) return View();
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.consultaCritoCredito(tabla_Accion);

            return Json(ret);
        }

        public ActionResult consultarPrestamosCliente(Prestamos prestamos)
        {
            if (prestamos.Identificacion == null) return View();
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.consultaPrestamosCliente(prestamos);

            return Json(ret);
        }

        public ActionResult consultarPrestamosParientes(Personas personas)
        {
            if (personas.Identificacion == null) return View();
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.consultaCreditoParientes(personas);

            return Json(ret);
        }
        //usp_actualiza_cola_cobros_automaticos

        //
        // GET: /Cobros/Details/5

        public ActionResult Details(int id = 0)
        {
            BucketCobros bucketcobros = new BucketCobros();
            if (bucketcobros == null)
            {
                return HttpNotFound();
            }
            return View(bucketcobros);
        }

        //
        // GET: /Cobros/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Cobros/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BucketCobros bucketcobros)
        {
            if (ModelState.IsValid)
            {
                db.BucketCobros.Add(bucketcobros);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bucketcobros);
        }

        //
        // GET: /Cobros/Edit/5

        public ActionResult Edit(int id = 0)
        {
            BucketCobros bucketcobros = db.BucketCobros.Find(id);
            if (bucketcobros == null)
            {
                return HttpNotFound();
            }
            return View(bucketcobros);
        }

        //
        // POST: /Cobros/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BucketCobros bucketcobros)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bucketcobros).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bucketcobros);
        }

        //
        // GET: /Cobros/Delete/5

        public ActionResult Delete(int id = 0)
        {
            BucketCobros bucketcobros = db.BucketCobros.Find(id);
            if (bucketcobros == null)
            {
                return HttpNotFound();
            }
            return View(bucketcobros);
        }

        //
        // POST: /Cobros/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BucketCobros bucketcobros = db.BucketCobros.Find(id);
            db.BucketCobros.Remove(bucketcobros);
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
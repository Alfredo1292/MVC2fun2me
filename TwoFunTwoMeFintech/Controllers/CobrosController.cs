using _2Fun2MeDAO;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    [Authorize]
    public class CobrosController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        //
        // GET: /Cobros/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConsultaCobros()
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
            bool? resultado = null;
            string temp = "";

            ManagerUser manage = new ManagerUser();
            if (bucketCobros.Identificacion != null || bucketCobros.Identificacion != " ")
            {
                temp = bucketCobros.Identificacion;
                temp = temp.Replace(" ", ""); ;
                resultado = Regex.IsMatch(temp, @"^[a-zA-ZñÑ]+$");
                if (resultado == false)
                {
                    bucketCobros.Nombre = "";
                }
                else
                {
                    bucketCobros.Nombre = bucketCobros.Identificacion;
                    bucketCobros.Identificacion = "";
                }

            }
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            if ((bucketCobros.Identificacion.Length != 9 && bucketCobros.Identificacion.Length != 12) && (bucketCobros.Nombre == null || bucketCobros.Nombre == ""))
            {
                bucketCobros.IdCredito = int.Parse(bucketCobros.Identificacion);
                bucketCobros.Identificacion = null;
                bucketCobros.Nombre = "";
            }
            bucketCobros.cod_agente = Session["agente"].ToString();
            var dto_ret = manage.ObtenerEncabezado(bucketCobros);
            return Json(dto_ret);
        }


        public ActionResult ActualizaColaAutomatica(BucketCobros bucketCobros)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            bucketCobros.AgenteAsignado = Session["agente"].ToString();
            var ret = manage.ActualizaColaAutomaticaCobros(bucketCobros);

            return Json(ret);
        }

        public ActionResult ActualizaReprogramacion(Reprogramacion reprog)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            reprog.AgenteAsignado = Session["agente"].ToString();
            var ret = manage.ActualizaReprogramacion(reprog);

            return Json(ret);
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
            Tab_ConfigSys dto_Config = new Tab_ConfigSys();
            ManagerUser manage = new ManagerUser();
            List<Tab_ConfigSys> CONF = new List<Tab_ConfigSys>();

            dto_Config.llave_Config1 = "FENIX";
            dto_Config.llave_Config2 = "CONFIGURACION";
            dto_Config.llave_Config3 = "COBROS";
            dto_Config.llave_Config4 = "MONTO";
            var dto_interval = manage.ConsultaConfMontoPP(dto_Config);
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            cobro.UsrModifica = Session["agente"].ToString();
            int DifDias = 0;
            try
            {
                decimal MontoMax = 0;
                MontoMax = Convert.ToDecimal(dto_interval.Where(x => x.llave_Config5 == "MONTOMAXIMO").Select(x => x.Dato_Char1).FirstOrDefault());
                DateTime FechaHoy = DateTime.Now.Date;
                TimeSpan timeSpan = Convert.ToDateTime(cobro.FechaPP) - FechaHoy;
                DifDias = timeSpan.Days;
                if (cobro.MontoPP == null)
                {
                    cobro.MontoPP = "0";
                }
                if (cobro.RespuestaGestion == "PRP" && Convert.ToDecimal(cobro.MontoPP) <= 0 || Convert.ToDecimal(cobro.MontoPP) > MontoMax)
                {
                    return Json(new { Resultado = false, Mensaje = "ErrMonto" });
                }

                if (FechaHoy > Convert.ToDateTime(cobro.FechaPP) && cobro.RespuestaGestion == "PRP")
                {
                    return Json(new { Resultado = false, Mensaje = "ErrFec" });
                }

                if (DifDias > 40 || DifDias < 0 && cobro.RespuestaGestion == "PRP")
                {
                    return Json(new { Resultado = false, Mensaje = "ErrFecDias" });
                }

                var respuesta = manage.GuardaCobro(cobro);
                if (respuesta != null)
                {
                    return Json(new { Resultado = true, Mensaje = respuesta.FirstOrDefault().MensajeSalida });
                }
                else {
                    return Json(new { Resultado = false, Mensaje = "ErrBD" });
                }
                
            }
            catch
            {
                return Json(new { Resultado = false, Mensaje = "Err" });
            }
        }

        #region Mantenimiento de Contactos

        public ActionResult Contactos(contacto contac)

        {
            if (contac.Identificacion == null) return View();

            ManagerUser manage = new ManagerUser();


            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarContactos(contac);

            return Json(ret);
        }
        public ActionResult actualizarCalificacion(contacto data) {
            try
            {
                ManagerUser manage = new ManagerUser();
                CatalogoTelefono catalogoTelefonos = new CatalogoTelefono();

                if (Session["agente"] == null)
                    return RedirectToAction("LogOff", "Login");

               manage.actualizarCalificacion(data);

                return Json(new { ok = true });
            }
            catch (Exception ex) {
                return Json(new { ok=false,message=ex.Message});
            }
        }
        public ActionResult ConsultaCatalogoTel(string id)
        {
            ManagerUser manage = new ManagerUser();
            CatalogoTelefono catalogoTelefonos = new CatalogoTelefono();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarCatalogoTelefono(catalogoTelefonos);

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtieneTelefono(string id)
        {
            contacto contac = new contacto();
            contac.IdTelefono = Convert.ToInt32(id);
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarTelefono(contac);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActualizarTelefono(contacto contac)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ActualizarTelefono(contac);

            return Json(ret);
        }
        public ActionResult IngresarTelefono(contacto contac)

        {
            if (contac.Identificacion == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.CrearTelefono(contac);

            return Json(ret);
        }
        public ActionResult EliminaTelefono(int id)

        {
            contacto contac = new contacto();
            contac.IdTelefono = id;
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.EliminaTelefono(contac);

            return Json(ret);
        }
        #endregion
        //AVARGAS - 23/01/2019 - Mantenimiento de Teléfonos del cliente --FINAL--
        public ActionResult Reprogramaciones(reprogramaciones reprogr)

        {

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarReprogramaciones(reprogr, Session["agente"].ToString());

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
        public ActionResult HistoricoPlanPagos(PlanPlagos tabla_Accion)
        {
            if (tabla_Accion.IdCredito == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultaPlanPagos(tabla_Accion);

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

        public ActionResult calculaCuota(Tabla_Accion mora)
        {
            if (mora.IdCredito == null || mora.Fecha == null) return View();
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.calculaCuota(mora);

            return Json(ret);
        }

        public ActionResult ReferenciasPersonales(contacto contacto)
        {
            if (contacto.Identificacion == null) return View();
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.referenciasPersonales(contacto);

            return Json(ret);
        }

        public ActionResult CorreoPersona(Personas personas)

        {
            if (personas.Identificacion == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.correosPersona(personas);

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

        public ActionResult LlamadaAutomaticaIniciar(agente agente)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var dto_login_sess = (List<dto_login>)Session["LoginCredentials"];
            agente.Extencion = dto_login_sess.FirstOrDefault().Extencion;

            var dto_Config = new Tab_ConfigSys
            {
                llave_Config1 = "SERVICIO",
                llave_Config2 = "CONFIGURACION",
                llave_Config3 = "SERVIDOR",
                llave_Config4 = "ASTERISK"
            };


            var dto_config = manage.GetConfig(dto_Config);

            var ftpUri = dto_config.Where(x => x.llave_Config5 == "FTP_URI").Select(x => x.Dato_Char1).FirstOrDefault();
            var ftpUser = dto_config.Where(x => x.llave_Config5 == "FTP_USER").Select(x => x.Dato_Char1).FirstOrDefault();
            var ftpPass = dto_config.Where(x => x.llave_Config5 == "FTP_PASS").Select(x => x.Dato_Char1).FirstOrDefault();


            var Channel = dto_config.Where(x => x.llave_Config5 == "Channel").Select(x => x.Dato_Char1).FirstOrDefault();
            var MaxRetries = dto_config.Where(x => x.llave_Config5 == "MaxRetries").Select(x => x.Dato_Char1).FirstOrDefault();
            var RetryTime = dto_config.Where(x => x.llave_Config5 == "RetryTime").Select(x => x.Dato_Char1).FirstOrDefault();
            var Context = dto_config.Where(x => x.llave_Config5 == "Context-saliente-agente").Select(x => x.Dato_Char1).FirstOrDefault();
            var audio = dto_config.Where(x => x.llave_Config5 == "audio-agente").Select(x => x.Dato_Char1).FirstOrDefault();


            var Priority = dto_config.Where(x => x.llave_Config5 == "Priority").Select(x => x.Dato_Char1).FirstOrDefault();
            var Extension = dto_config.Where(x => x.llave_Config5 == "Extension").Select(x => x.Dato_Char1).FirstOrDefault();
            var CallerID = dto_config.Where(x => x.llave_Config5 == "CallerID").Select(x => x.Dato_Char1).FirstOrDefault();
            var VirtualPatch = dto_config.Where(x => x.llave_Config5 == "REMOTE_PATCH").Select(x => x.Dato_Char1).FirstOrDefault();

            FtpManager ftpClient = new FtpManager(@ftpUri, ftpUser, ftpPass);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format("Channel: {0}{1}", Channel, agente.TelefonoCelCliente));

            sb.AppendLine(string.Format("MaxRetries: {0}", MaxRetries));

            sb.AppendLine(string.Format("RetryTime: {0}", RetryTime));

            sb.AppendLine(string.Format("Context: {0}", Context));

            sb.AppendLine(string.Format("Priority: {0}", Priority));

            sb.AppendLine(string.Format("Extension: {0}", Extension));

            sb.AppendLine(string.Format("CallerID: {0} <{1}>", CallerID, CallerID));

            sb.AppendLine(string.Format("Set: llamada={0}", agente.TelefonoCelCliente));

            sb.AppendLine(string.Format("Set: agente={0}", agente.Extencion));

            //sb.AppendLine(string.Format("Set: audio={0}", audio));

            var tempPatch = string.Concat(VirtualPatch, agente.IdentificacionCliente, ".call");

            ftpClient.upload(tempPatch, sb);

            return Json(new object());
        }
        //INICIO FCAMACHO 04/12/2018 
        //PROCESO DE GUARDAR EN TABLA LOS DATOS OBTENIDOS PREVIAMENTE ANTES DE DETENER LA COLA 
        public ActionResult GuardarGestionCobroTemp(GestionCobro cobro)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            cobro.UsrModifica = Session["agente"].ToString();
            try
            {
                manage.GuardaCobroTemp(cobro);
                return Json(new { Resultado = true });
            }
            catch
            {
                return Json(new { Resultado = false });
            }
        }
        //if (contacto.Identificacion == null) return View();
        //ManagerUser manage = new ManagerUser();

        //          if (Session["agente"] == null)
        //              return RedirectToAction("LogOff", "Login");

        //var ret = manage.referenciasPersonales(contacto);

        //          return Json(ret);
        public ActionResult consultarInfoGestionTemp(GestionCobro cobro)
        {
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            cobro.UsrModifica = Session["agente"].ToString();
            try
            {
                var ret = manage.consultaInfoGestionTemp(cobro);
                return Json(ret);
            }
            catch
            {
                return Json(new { Resultado = false });
            }
        }
        //FIN FCAMACHO 04/12/2018 


        //ICORTES 2018-12-09
        public ActionResult ConsultaDashBoard(DashBoard dashBoard)

        {
            return View();
        }

        public ActionResult ConsultaDashBoardGeneral(DashBoard dashBoard)

        {
            //if (dashBoard.AgenteAsignado == null) return View();
            //dashBoard.FechaAsignacion = System.DateTime.Now.ToString("yyyy-MM-dd");
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarDashBoard(dashBoard);

            return Json(ret);
        }
        public ActionResult ConsultaDashBoardDetallado(DashBoard dashBoard)

        {
            //if (dashBoard.AgenteAsignado == null) return View();
            //dashBoard.FechaAsignacion = System.DateTime.Now.ToString("yyyy-MM-dd");
            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultarDashBoardDetallado(dashBoard);

            return Json(ret);
        }
        #region MantenimientoDirecciones
        public ActionResult ContactosDireccion(contacto contac)
        {
            if (contac.Identificacion == null) return View();
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ConsultarDirecciones(contac);
            return Json(ret);
        }
        public ActionResult ConsultaCatalogoDIR(string id)
        {
            ManagerUser manage = new ManagerUser();
            CatalogoDireccion catalogoDireccion = new CatalogoDireccion();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ConsultarCatalogoDireccion(catalogoDireccion);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ObtieneDireccion(string id)
        {
            contacto contac = new contacto();
            contac.IdDireccion = Convert.ToInt32(id);
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ConsultarDireccion(contac);
            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ActualizarDireccion(contacto contac)
        {
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.ActualizarDireccion(contac);
            return Json(ret);
        }
        public ActionResult CrearDireccion(contacto contac)
        {
            if (contac.Identificacion == null) return View();
            ManagerUser manage = new ManagerUser();
            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");
            var ret = manage.CrearDireccion(contac);
            return Json(ret);
        }
        #endregion MantenimientoDirecciones
        //INICIO FCAMACHO 22/05/2019 GENERACION DE DOCUMENTO DE NOTIFICACION DE COBRO

        #region GENERAR DOCUMENTOS NOTIFICACION COBROS

        //METODOS PRIVADOS
        private void PorcesarMarcadoresDOCX(GestionNotificacionCobro Obj)
        {
            //PROCESAMIENTO DE MARCADORES
            Application app = new Application();
            Document doc = app.Documents.Open(Obj.RutaTemp);


            if (doc.Bookmarks.Exists("NOMBRE"))
            {
                object oBookMark = "NOMBRE";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Nombre;
            }
            if (doc.Bookmarks.Exists("CEDULA"))
            {
                object oBookMark = "CEDULA";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Identificacion;
            }
            if (doc.Bookmarks.Exists("EMPRESA"))
            {
                object oBookMark = "EMPRESA";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Empresa;
            }
            if (doc.Bookmarks.Exists("EMPRESA_COMERCIAL"))
            {
                object oBookMark = "EMPRESA_COMERCIAL";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Empresa_Comercial;
            }
            if (doc.Bookmarks.Exists("LEY"))
            {
                object oBookMark = "LEY";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Ley;
            }
            if (doc.Bookmarks.Exists("PORCENTAJE"))
            {
                object oBookMark = "PORCENTAJE";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Porcentaje;
            }
            if (doc.Bookmarks.Exists("TELEFONO_CELULAR"))
            {
                object oBookMark = "TELEFONO_CELULAR";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Telefono_Celular.TrimEnd();
            }
            if (doc.Bookmarks.Exists("TELEFONO_FIJO"))
            {
                object oBookMark = "TELEFONO_FIJO";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Telefono_Fijo.TrimEnd();
            }
            if (doc.Bookmarks.Exists("NOMBRE_RECIBIDO"))
            {
                object oBookMark = "NOMBRE_RECIBIDO";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Nombre;
            }
            if (doc.Bookmarks.Exists("TOTAL"))
            {
                object oBookMark = "TOTAL";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Total;
            }
            if (doc.Bookmarks.Exists("TELEFONO"))
            {
                object oBookMark = "TELEFONO";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.TelefonoDomicilio;
            }
            if (doc.Bookmarks.Exists("DIRECCION"))
            {
                object oBookMark = "DIRECCION";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.DireccionDomicilio;
            }
            if (doc.Bookmarks.Exists("LUGAR_TRABAJO"))
            {
                object oBookMark = "LUGAR_TRABAJO";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.LugarTrabajo;
            }
            if (doc.Bookmarks.Exists("DIRECCION_TRABAJO"))
            {
                object oBookMark = "DIRECCION_TRABAJO";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.DireccionTrabajo;
            }
            if (doc.Bookmarks.Exists("USUARIO"))
            {
                object oBookMark = "USUARIO";
                doc.Bookmarks.get_Item(ref oBookMark).Range.Text = Obj.Usuario_Ingreso;
            }

            doc.ExportAsFixedFormat(Obj.RutaTemp.Replace(".docx", ".pdf"), WdExportFormat.wdExportFormatPDF);


            ((_Document)doc).Close();
            ((_Application)app).Quit();
        }

        private void SubirBlobStorage(GestionNotificacionCobro Obj)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(Obj.RutaTemp);
            System.IO.File.WriteAllBytes(Obj.RutaTemp, bytes);

            var blob = new blobStorage
            {
                ImageToUploadByte = bytes,
                ContainerPrefix = Obj.Url_Contenedor,
                ImageExtencion = Obj.Formato_Arch,
                ImageToUpload = Obj.NombreDOCX.Replace(".docx", ""),
                ConnectionString = Obj.Cnx_Blob_Storage,
                PatchTempToSave = ""
            };

            UtilBlobStorageAzure.UploadBlobStorage(blob);
        }

        private void BajarBlobStorage(GestionNotificacionCobro Obj)
        {
            var blobDowland = new blobStorage
            {
                ImageToUploadByte = null,
                ContainerPrefix = Obj.Ruta_Plantilla,
                ImageToUpload = Obj.Nombre_Arch,
                ImageExtencion = Obj.Formato_Arch,
                ConnectionString = Obj.Cnx_Blob_Storage,
                PatchTempToSave = Obj.Ruta_Carpeta_Temp
            };

            if (!Directory.Exists(blobDowland.PatchTempToSave))
                Directory.CreateDirectory(blobDowland.PatchTempToSave);
            UtilBlobStorageAzure.DownloadBlobStorage(blobDowland);
        }

        public ActionResult ImprimirNotificacionCobro(GestionNotificacionCobro objEntrada)
        {

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");


            GestionNotificacionCobro objSalida = new GestionNotificacionCobro();
            ManagerUser manage = new ManagerUser();

            //OBTIENE LA CONFIGURACION DE LA PLANTILLA
            GestionNotificacionCobro objConfigNotifCobro = new GestionNotificacionCobro();
            var config = manage.ObtenerConfigPlantillaNotifCobros(objConfigNotifCobro);

            //SETEA LAS PROPIEDADES DEL OBJETO A TRABAJAR
            objSalida = config[0];
            objSalida.Identificacion = objEntrada.Identificacion;
            objSalida.Nombre = objEntrada.Nombre;
            objSalida.Identificacion = objEntrada.Identificacion;
            objSalida.Total = objEntrada.Total;
            objSalida.TelefonoDomicilio = objEntrada.DireccionDomicilio;
            objSalida.LugarTrabajo = objEntrada.LugarTrabajo;
            objSalida.DireccionTrabajo = objEntrada.DireccionTrabajo;
            objSalida.IdCredito = objEntrada.IdCredito;
            objSalida.Usuario_Ingreso = Session["UserName"].ToString();
            objSalida.NombrePDF = objSalida.Nombre_Arch_Prefijo + objSalida.Identificacion + ".pdf";
            objSalida.NombreDOCX = objSalida.Nombre_Arch_Prefijo + objSalida.Identificacion + ".docx";
            objSalida.RutaTemp = @objSalida.Ruta_Carpeta_Temp + objSalida.Nombre_Arch_Prefijo + objSalida.Identificacion + ".docx";
            objSalida.Tipo_Documento = "NOTIFICACION_COBRO";
            string rutaPlantilla = objSalida.Ruta_Carpeta_Temp + objSalida.Nombre_Arch + objSalida.Formato_Arch;


            //ELIMINAR ARCHIVOS DE LA CARPETA

            string[] picList = Directory.GetFiles(objSalida.Ruta_Carpeta_Temp, "*.pdf");
            string[] txtList = Directory.GetFiles(objSalida.Ruta_Carpeta_Temp, "*.docx");

            foreach (string f in txtList)
            {
                //if (f != rutaPlantilla)
                //{
                System.IO.File.Delete(f);
                //}
            }
            foreach (string f in picList)
            {
                //if (f != rutaPlantilla)
                //{
                System.IO.File.Delete(f);
                //}
            }

            //CONSUMIR BLOB STORAGE PARA BAJAR LA PLANTILLA
            BajarBlobStorage(objSalida);

            //DUPLICA LA PLANTILLA CON EL NOMBRE A PROCESAR
            if (System.IO.File.Exists(rutaPlantilla))
            {
                System.IO.File.Copy(rutaPlantilla, objSalida.RutaTemp);
            }

            //ELIMINAR PLANTILLA
            if (System.IO.File.Exists(rutaPlantilla))
            {
                System.IO.File.Delete(rutaPlantilla);
            }

            //PORCESAR MARCADORES ARCHIVO
            PorcesarMarcadoresDOCX(objSalida);

            //CONSUMIR BLOB STORAGE PARA SUBIR DOCS
            objSalida.Url_Contenedor = objSalida.Ruta_Almacenar_Docs + @"\" + objSalida.Identificacion;
            SubirBlobStorage(objSalida);

            //REGISTRAR EN BD LOS DOCS GENERADOS
            objSalida.Nombre_Doc = objSalida.NombrePDF.Replace(".pdf", "");
            manage.MantenimientoDocumentosCobros(objSalida);

            //PROCESO SUCCESS
            objSalida.Mensaje = "OK";


            return Json(objSalida);
        }

        public ActionResult DescargarNotifCobroPDF(string ruta)
        {
            return File(ruta, "application/pdf", ruta.Replace(".docx", ".pdf"));
        }

        public ActionResult DescargarNotifCobroDOCX(string ruta)
        {
            return File(ruta, "application/msword", ruta);
        }

        /// <summary>
        /// eliminar los archivos temporales
        /// </summary>
        /// <param name="rutaPDF"></param>
        /// <param name="rutaDOCX"></param>
        /// <returns></returns>
        public ActionResult EliminarNotifCobro(GestionNotificacionCobro Obj)
        {
            switch (Obj.Arch_Eliminar)
            {
                case "pdf":
                    {
                        //ELIMINAR .PDF
                        if (System.IO.File.Exists(Obj.RutaTemp.Replace(".docx", ".pdf")))
                        {
                            System.IO.File.Delete(Obj.RutaTemp.Replace(".docx", ".pdf"));
                        }
                        break;
                    }
                case "docx":
                    {
                        //ELIMINAR .DOCX
                        if (System.IO.File.Exists(Obj.RutaTemp))
                        {
                            System.IO.File.Delete(Obj.RutaTemp);
                        }
                        break;
                    }
            }

            string jsonData = @"{'Mensaje':'OK'}";

            return Json(jsonData);
        }

        #endregion

        //FIN FCAMACHO 22/05/2019 GENERACION DE DOCUMENTO DE NOTIFICACION DE COBRO

        public ActionResult HistoricoGestionesAutomaticas(Tabla_Accion tabla_Accion)

        {
            if (tabla_Accion.IdCredito == null) return View();

            ManagerUser manage = new ManagerUser();

            if (Session["agente"] == null)
                return RedirectToAction("LogOff", "Login");

            var ret = manage.ConsultaHistoricoGestionesAutomaticas(tabla_Accion);

            return Json(ret);
        }
    }

}
﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMe.Models.Utility;
using TwoFunTwoMe_DataAccess;
using TwoFunTwoMeFintech.Models;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class BucketsController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        public object JSonConvert { get; private set; }
        private InfoClass infDto = new InfoClass
        {
            STR_COD_PAIS = "506"
        };


        //
        // GET: /Buckets/

        public ActionResult Index()
        {
            return View(db.agentes.ToList());
        }


        public ActionResult Agente()
        {
            ManagerUser mnage = new ManagerUser();

            try
            {
                var draw = HttpContext.Request.Form["draw"];
                var start = Request.Form["start"];
                // Paging Length 10,20
                var length = Request.Form["length"];
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"];
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"];

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                if (Session["LoginCredentials"] != null)
                {
                    var agenteSession = Session["LoginCredentials"] as List<dto_login>;
                    var log = new dto_login
                    {
                        cod_agente = string.Empty,
                        pass = string.Empty,
                        correo = string.Empty
                    };
                    var agente = mnage.LoginRol(log);


                    // Getting all Customer data
                    var customerData = (from tempcustomer in agente
                                        select new
                                        {
                                            fechamodificacion = tempcustomer.FEC_MODIFICACION.ToString(),
                                            fechacreacion = tempcustomer.FEC_MODIFICACION.ToString(),
                                            cod_agente = tempcustomer.cod_agente.ToString(),
                                            estado = tempcustomer.estado.ToString(),
                                            nombre = tempcustomer.nombre.ToString(),
                                            configuraciones1 = tempcustomer.ConfiguracionBucket.Substring(0, 1).ToString(),
                                            configuraciones2 = tempcustomer.ConfiguracionBucket.Substring(2, 1).ToString(),
                                            configuraciones3 = tempcustomer.ConfiguracionBucket.ToString().Substring(4, 1),
                                            configuraciones4 = tempcustomer.ConfiguracionBucket.ToString().Substring(6, 1),
                                            configuraciones5 = tempcustomer.ConfiguracionBucket.ToString().Substring(8, 1),
                                            configuraciones6 = tempcustomer.ConfiguracionBucket.ToString().Substring(10, 1),
                                            configuraciones7 = tempcustomer.ConfiguracionBucket.ToString().Substring(12, 1),
                                            configuraciones8 = tempcustomer.ConfiguracionBucket.ToString().Substring(14, 1),
                                        });

                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                    {
                        switch (sortColumn)
                        {

                            case "cod_agente":
                                if (sortColumnDirection == "desc")
                                {

                                    customerData = (from foobars in customerData
                                                    orderby foobars.cod_agente descending
                                                    select foobars
                                                     );
                                }
                                else
                                {

                                    customerData = (from foobars in customerData
                                                    orderby foobars.cod_agente ascending
                                                    select foobars
                                                    );
                                }
                                break;
                            case "nombre":
                                if (sortColumnDirection == "desc")
                                {

                                    customerData = (from foobars in customerData
                                                    orderby foobars.nombre descending
                                                    select foobars
                                                     );
                                }
                                else
                                {

                                    customerData = (from foobars in customerData
                                                    orderby foobars.nombre ascending
                                                    select foobars
                                                    );
                                }
                                break;
                            case "estado":
                                if (sortColumnDirection == "desc")
                                {

                                    customerData = (from foobars in customerData
                                                    orderby foobars.estado descending
                                                    select foobars
                                                     );
                                }
                                else
                                {

                                    customerData = (from foobars in customerData
                                                    orderby foobars.estado ascending
                                                    select foobars
                                                    );
                                }
                                break;
                        }
                        //customerData = customerData.OrderBy(x =>x.cod_agente);


                    }
                    //Search
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        customerData = customerData.Where(m => m.cod_agente.ToLower().Contains(searchValue.ToLower()) || m.nombre.ToLower().Contains(searchValue.ToLower()) || m.estado.ToLower().Contains(searchValue.ToLower()));
                    }

                    //total number of rows count 
                    recordsTotal = customerData.Count();
                    //Paging 
                    var data = customerData.Skip(skip).Take(pageSize).ToList();

                    return Json(new { draw = draw, recordsFiltered = customerData.Count(), recordsTotal = customerData.Count(), data = data });
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }




        public ActionResult ConfiguracionOrden()
        {


            try
            {
                var draw = HttpContext.Request.Form["draw"];
                var start = Request.Form["start"];
                // Paging Length 10,20
                var length = Request.Form["length"];
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"];
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"];

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                ManagerUser mang = new ManagerUser();
                var agenteSession = Session["LoginCredentials"] as List<dto_Configuracion>;
                var bucket = mang.AsignaConfiguracion();

                // Getting all Customer data
                var customerData = (from tempcustomer in bucket
                                    select new
                                    {
                                        id = tempcustomer.Id.ToString(),
                                        Nombre = tempcustomer.Nombre.ToString()
                                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumn)
                    {

                        case "Id":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.id descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.id ascending
                                                select foobars
                                                );
                            }
                            break;
                        case "Nombre":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Nombre descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Nombre ascending
                                                select foobars
                                                );
                            }
                            break;
                    }
                    //customerData = customerData.OrderBy(x =>x.cod_agente);


                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.id.ToLower().Contains(searchValue.ToLower()) || m.Nombre.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count 
                recordsTotal = customerData.Count();
                //Paging 
                var data = customerData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = customerData.Count(), recordsTotal = customerData.Count(), data = data });

            }
            catch (Exception ex)
            {
                throw;
            }

        }





        public ActionResult ConfiguracionBucket()
        {


            try
            {
                var draw = HttpContext.Request.Form["draw"];
                var start = Request.Form["start"];
                // Paging Length 10,20
                var length = Request.Form["length"];
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"];
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"];

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                ManagerUser mang = new ManagerUser();
                var agenteSession = Session["LoginCredentials"] as List<dto_login>;
                var bucket = mang.AsignaBucket();

                // Getting all Customer data
                var customerData = (from tempcustomer in bucket
                                    select new
                                    {
                                        id = tempcustomer.id.ToString(),
                                        BucketInicial = tempcustomer.BucketInicial.ToString(),
                                        BucketFinal = tempcustomer.BucketFinal.ToString(),
                                        PRPRotas = tempcustomer.PRPRotas.ToString(),
                                        CeroPagos = tempcustomer.CeroPagos.ToString(),
                                        SaldoAlto = tempcustomer.SaldoAlto.ToString(),
                                        SaldoMedio = tempcustomer.SaldoMedio.ToString(),
                                        SaldoBajo = tempcustomer.SaldoBajo.ToString(),
                                        CuentaAlDia = tempcustomer.CuentaAlDia.ToString(),
                                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumn)
                    {

                        case "id":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.id descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.id ascending
                                                select foobars
                                                );
                            }
                            break;
                        case "BucketInicial":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.BucketInicial descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.BucketInicial ascending
                                                select foobars
                                                );
                            }
                            break;
                        case "BucketFinal":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.BucketFinal descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.BucketFinal ascending
                                                select foobars
                                                );
                            }
                            break;
                    }
                    //customerData = customerData.OrderBy(x =>x.cod_agente);


                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.id.ToLower().Contains(searchValue.ToLower()) || m.BucketInicial.ToLower().Contains(searchValue.ToLower()) || m.BucketFinal.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count 
                recordsTotal = customerData.Count();
                //Paging 
                var data = customerData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = customerData.Count(), recordsTotal = customerData.Count(), data = data });

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult Buckets_NoAsignado()
        {


            try
            {
                var draw = HttpContext.Request.Form["draw"];
                var start = Request.Form["start"];
                // Paging Length 10,20
                var length = Request.Form["length"];
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"];
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"];

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                ManagerUser mang = new ManagerUser();
                var agenteSession = Session["LoginCredentials"] as List<dto_login>;
                var bucket = mang.DetalleBucket_NoAsignado();

                // Getting all Customer data
                var customerData = (from tempcustomer in bucket
                                    select new
                                    {
                                        Bucket = tempcustomer.Bucket.ToString(),
                                        Cantidad_Cuentas = tempcustomer.Cantidad_Cuentas.ToString(),
                                        PRPRotas = tempcustomer.PromesaRota.ToString(),
                                        CeroPagos = tempcustomer.CeroPagos.ToString(),
                                        SaldoAlto = tempcustomer.RangoSaldoAlto.ToString(),
                                        SaldoMedio = tempcustomer.RangoSaldoMedio.ToString(),
                                        SaldoBajo = tempcustomer.RangoSaldoBajo.ToString(),
                                        CuentaAlDia = tempcustomer.CuentaAlDia.ToString(),
                                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumn)
                    {

                        case "Bucket":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Bucket descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Bucket ascending
                                                select foobars
                                                );
                            }
                            break;
                        case "Cantidad_Cuentas":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Cantidad_Cuentas descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Cantidad_Cuentas ascending
                                                select foobars
                                                );
                            }
                            break;
                    }
                    //customerData = customerData.OrderBy(x =>x.cod_agente);


                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Bucket.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count 
                recordsTotal = customerData.Count();
                //Paging 
                var data = customerData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = customerData.Count(), recordsTotal = customerData.Count(), data = data });

            }
            catch (Exception ex)
            {
                throw;
            }

        }



        public ActionResult Buckets()
        {
            string xClase = string.Format("{0}|{1}", MethodBase.GetCurrentMethod().Module.Name, MethodBase.GetCurrentMethod().DeclaringType.Name);
            string xProceso = MethodBase.GetCurrentMethod().Name;
            var dto_excepcion = new UTL_TRA_EXCEPCION
            {
                STR_CLASE = xClase,
                STR_EVENTO = xProceso,
                STR_APLICATIVO = ConfigurationManager.AppSettings["Aplicativo"],
                STR_SERVIDOR = System.Net.Dns.GetHostName()
            };

            try
            {
                var draw = HttpContext.Request.Form["draw"];
                var start = Request.Form["start"];
                // Paging Length 10,20
                var length = Request.Form["length"];
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"];
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"];
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"];

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                ManagerUser mang = new ManagerUser();
                var agenteSession = Session["LoginCredentials"] as List<dto_login>;
                var bucket = mang.DetalleBucket();

                // Getting all Customer data
                var customerData = (from tempcustomer in bucket
                                    select new
                                    {
                                        AgenteAsignado = tempcustomer.AgenteAsignado.ToString(),
                                        Bucket = tempcustomer.Bucket.ToString(),
                                        Cantidad_Cuentas = tempcustomer.Cantidad_Cuentas.ToString(),
                                        PRPRotas = tempcustomer.PromesaRota.ToString(),
                                        CeroPagos = tempcustomer.CeroPagos.ToString(),
                                        SaldoAlto = tempcustomer.RangoSaldoAlto.ToString(),
                                        SaldoMedio = tempcustomer.RangoSaldoMedio.ToString(),
                                        SaldoBajo = tempcustomer.RangoSaldoBajo.ToString(),
                                        CuentaAlDia = tempcustomer.CuentaAlDia.ToString(),
                                    });

                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    switch (sortColumn)
                    {

                        case "Bucket":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Bucket descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Bucket ascending
                                                select foobars
                                                );
                            }
                            break;
                        case "Cantidad_Cuentas":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Cantidad_Cuentas descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.Cantidad_Cuentas ascending
                                                select foobars
                                                );
                            }
                            break;
                        case "AgenteAsignado":
                            if (sortColumnDirection == "desc")
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.AgenteAsignado descending
                                                select foobars
                                                 );
                            }
                            else
                            {

                                customerData = (from foobars in customerData
                                                orderby foobars.AgenteAsignado ascending
                                                select foobars
                                                );
                            }
                            break;
                    }
                    //customerData = customerData.OrderBy(x =>x.cod_agente);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Bucket.ToLower().Contains(searchValue.ToLower()) || m.AgenteAsignado.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count 
                recordsTotal = customerData.Count();
                //Paging 
                var data = customerData.Skip(skip).Take(pageSize).ToList();

                return Json(new { draw = draw, recordsFiltered = customerData.Count(), recordsTotal = customerData.Count(), data = data });

            }
            catch (Exception ex)
            {
                dto_excepcion.STR_MENSAJE = ex.Message;
                dto_excepcion.STR_DETALLE = ex.StackTrace;
                TwoFunTwoMe_DataAccess.Utility.guardaExcepcion(dto_excepcion, GlobalClass.connectionString.Where(a => a.Key == infDto.STR_COD_PAIS).FirstOrDefault().Value);
                throw;
            }

        }

        //
        // GET: /Buckets/Details/5

        public ActionResult Details(string id = null)
        {
            //agente agente = db.agentes.Find(id);
            //if (agente == null)
            //{
            //    return HttpNotFound();
            //}
            //LoadData();
            return View();
        }


        public ActionResult Edit(string id, string configuracion)
        {
            int result = 0;
            ManagerUser manage = new ManagerUser();
            var login = new dto_login
            {
                cod_agente = id,
                ConfiguracionBucket = configuracion
            };
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Buckets", "Details");
                }
                else
                {

                    try
                    {
                        result = manage.UpdateBuckets(login);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        // Provide for exceptions.
                    }

                    if (result > 0)
                    {
                        return Json(data: true);

                    }
                    else
                    {
                        return Json(data: false);
                    }
                }
                //return View("Edit");
            }
            catch (Exception)
            {
                throw;
            }
        }



        public ActionResult EditBucket(AsignacionBuckets buckets)
        {
            int result = 0;
            ManagerUser manage = new ManagerUser();
            try
            {
                try
                {

                    manage.ActualizarBuckets(buckets);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

                if (result > 0)
                {
                    return Json(data: true);

                }
                else
                {
                    return Json(data: false);
                }

                //return View("Edit");
            }
            catch (Exception)
            {
                throw;
            }
        }



        public ActionResult Delete(string id)
        {
            var login = new dto_login
            {
                cod_agente = id,
                estado = "Inactivo"
            };
            ManagerUser manage = new ManagerUser();
            try
            {
                int result = 0;

                if (string.IsNullOrEmpty(id))
                {
                    return RedirectToAction("Buckets", "Details");
                }

                try
                {

                    result = manage.UpdateBuckets(login);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }

                if (result > 0)
                {
                    return Json(data: true);

                }
                else
                {
                    return Json(data: false);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public ActionResult ConfigAsignados(Catalogo_AsignacionBuckets catalogo)
        {
            ManagerUser manage = new ManagerUser();
            try
            {
                var dto_result = manage.GetAsignados(catalogo);
                return Json(dto_result);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult ConfigAsignados_conf(Catalogo_AsignacionBuckets_conf catalogo)
        {
            ManagerUser manage = new ManagerUser();
            try
            {
                var dto_result = manage.GetAsignados_conf(catalogo);
                return Json(dto_result);

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public ActionResult ConfigNoAsignados(Catalogo_AsignacionBuckets catalogo)
        {
            ManagerUser manage = new ManagerUser();
            try
            {
                var dto_result = manage.GetNoAsignados(catalogo);
                return Json(dto_result);

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public ActionResult ConfigNoAsignados_conf(Catalogo_AsignacionBuckets_conf catalogo)
        {
            ManagerUser manage = new ManagerUser();
            try
            {
                var dto_result = manage.GetNoAsignados_conf(catalogo);
                return Json(dto_result);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public ActionResult GuardarConfiguracion_conf(List<AsignacionBucketsConfig> asignacionBucketsConfigs)
        {
            ManagerUser manage = new ManagerUser();
            var dto_result = new AsignacionBucketsConfig();
            try
            {
                if (!string.IsNullOrEmpty(asignacionBucketsConfigs.FirstOrDefault().Accion) && asignacionBucketsConfigs.FirstOrDefault().Accion.Equals("Eliminar"))
                {
                    manage.BorraConfiguracion_conf(asignacionBucketsConfigs.FirstOrDefault());
                }
                else
                {
                    manage.BorraConfiguracion_conf(asignacionBucketsConfigs.FirstOrDefault());

                    asignacionBucketsConfigs.ForEach(x =>
                    {
                        manage.GuardaConfiguracion_conf(x);
                    }
                    );
                }
                dto_result.Respuesta = "OK";


            }
            catch (Exception ex)
            {
                dto_result.Respuesta = ex.Message;

            }
            return Json(dto_result);
        }
        public ActionResult GuardarConfiguracion(List<AsignacionBucketsConfig> asignacionBucketsConfigs)
        {
            ManagerUser manage = new ManagerUser();
            var dto_result = new AsignacionBucketsConfig();
            try
            {
                if (!string.IsNullOrEmpty(asignacionBucketsConfigs.FirstOrDefault().Accion) && asignacionBucketsConfigs.FirstOrDefault().Accion.Equals("Eliminar"))
                {
                    manage.BorraConfiguracion(asignacionBucketsConfigs.FirstOrDefault());
                }
                else
                {
                    manage.BorraConfiguracion(asignacionBucketsConfigs.FirstOrDefault());

                    asignacionBucketsConfigs.ForEach(x =>
                    {
                        manage.GuardaConfiguracion(x);
                    }
                    );
                }
                dto_result.Respuesta = "OK";


            }
            catch (Exception ex)
            {
                dto_result.Respuesta = ex.Message;

            }
            return Json(dto_result);
        }
        public ActionResult MantenimientoConfiguracion(Catalogo_AsignacionBuckets asignacionBucketsConfigs)
        {
            ManagerUser manage = new ManagerUser();
            var dto_result = new List<Catalogo_AsignacionBuckets>();
            try
            {
                dto_result = manage.Mantenimiento_Catalogo_AsignacionBucketsn(asignacionBucketsConfigs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return Json(dto_result);
        }
        public ActionResult ConfiguracionReportes(ConfiguracionReportes config)
        {
            ManagerUser manage = new ManagerUser();
            var dto_result = new List<ConfiguracionReportes>();
            try
            {
                dto_result = manage.ConsultaConfiguracionReportes(config);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return Json(dto_result);
        }
        public ActionResult ColaReportes(ColaReporte reporte)
        {
            ManagerUser manage = new ManagerUser();
            var dto_result = new List<ColaReporte>();
            try
            {
                dto_result = manage.ConsultaColaReportes(reporte);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return Json(dto_result);
        }
        public ActionResult InsertaColaReportes(ColaReporte reporte)
        {
            ManagerUser manage = new ManagerUser();
            var agenteSession = Session["LoginCredentials"] as List<dto_login>;
            var dto_result = reporte;
            reporte.UsuarioCreacion = agenteSession[0].cod_agente;
            Boolean inserta = false;
            try
            {
                inserta = manage.InsertaColaReportes(reporte);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return Json(dto_result);
        }

        public ActionResult AplicarConfigCobros(bool OPCION)
        {
            ManagerUser manage = new ManagerUser();
            try
            {
                manage.aplicaConfigCobros(OPCION);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            return Json(new { Respuesta = "OK" });
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class BucketsController : Controller
    {
        private TwoFunTwoMeFintechContext db = new TwoFunTwoMeFintechContext();

        public object JSonConvert { get; private set; }

        //
        // GET: /Buckets/

        public ActionResult Index()
        {
            return View(db.agentes.ToList());
        }


        public ActionResult LoadData()
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
                    var agente = mnage.Login(log);


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

        public ActionResult LoadData2()
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
                    return RedirectToAction("ShowGrid", "DemoGrid");
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
                    return RedirectToAction("ShowGrid", "DemoGrid");
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
    }
}
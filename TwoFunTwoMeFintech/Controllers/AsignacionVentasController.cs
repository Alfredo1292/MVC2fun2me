using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TwoFunTwoMe.Models.Manager;
using TwoFunTwoMeFintech.Models.DTO;

namespace TwoFunTwoMeFintech.Controllers
{
    public class AsignacionVentasController : Controller
    {
        //
        // GET: /AsignacionVentas/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            ManagerUser mang = new ManagerUser();
            var listaAgentesVentas = mang.ConsultaAgenteVentas();
            return View(listaAgentesVentas);
        }

        public ActionResult GetHorariosDisponibles(HorarioDisponibleAgente horarioDisponible)
        {
            ManagerUser manage = new ManagerUser();
            var dto_result = new List<HorarioDisponibleAgente>();
            try
            {
                dto_result = manage.GetHorariosDisponibles(horarioDisponible);
                return Json(dto_result);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public ActionResult GetHorariosAsignados(HorarioAsignadoAgente horarioAsignado)
        {
            ManagerUser manage = new ManagerUser();
            var dto_result = new List<HorarioAsignadoAgente>();
            try
            {
                dto_result = manage.GetHorariosAsignados(horarioAsignado);
                return Json(dto_result);

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public Boolean  GuardarHorariosAsignados(HorarioAsignadoAgente horarioAsignado)
        {
            ManagerUser manage = new ManagerUser();

            foreach (PropertyInfo propertyInfo in horarioAsignado.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(horarioAsignado) == null)
                {
                    propertyInfo.SetValue(horarioAsignado, "");
                }
            }

            Boolean resultado;
            try
            {
                resultado = manage.GuardarHorariosAsignados(horarioAsignado);
                return resultado;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }
        public Boolean GuardarHorariosDisponibles(HorarioDisponibleAgente horarioDisponible)
        {
            foreach (PropertyInfo propertyInfo in horarioDisponible.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(horarioDisponible) == null)
                {
                    propertyInfo.SetValue(horarioDisponible, "");
                }
            }
            ManagerUser manage = new ManagerUser();
            Boolean resultado;
            try
            {
                resultado = manage.GuardarHorariosDisponibles(horarioDisponible);
                return resultado;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }
    }
}

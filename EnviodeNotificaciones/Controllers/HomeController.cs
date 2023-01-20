using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EnviodeNotificaciones.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EnvioMail()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarEnvios()
        {
            List<EnvioMail> oLista = new List<EnvioMail>();
            oLista = new CN_EnvioMail().Listar();

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
    }
}
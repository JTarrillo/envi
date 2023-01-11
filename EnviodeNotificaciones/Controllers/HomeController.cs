using CapaEntidad;
using CapaNegocio;
using System;
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

        public JsonResult ListarEnvios()
        {
            List<EnvioMail> olista = new List<EnvioMail>();
            olista = new CN_EnvioMail().Listar();

            return Json(olista, JsonRequestBehavior.AllowGet);
         }
    }
}
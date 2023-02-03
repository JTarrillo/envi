using System.Collections.Generic;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion.Controllers
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

            var jsonResult = Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }


        [HttpGet]
        public JsonResult VerMsj(int Id)
        {
            bool respuesta = false;
            

            respuesta = new CN_EnvioMail().VerMensaje(Id);


            var jsonResult = Json(new { data = respuesta }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

              return jsonResult;
        }


    }
}
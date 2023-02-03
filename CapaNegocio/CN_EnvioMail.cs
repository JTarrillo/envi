using System.Collections.Generic;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class CN_EnvioMail
    {
        private CD_EnvioMail objCapaDato = new CD_EnvioMail();

        public List<EnvioMail> Listar()
        {
            return objCapaDato.Listar();
        }



        public bool VerMensaje(int Id)
        {

            return objCapaDato.VerMensaje(Id);
        }
    }
}
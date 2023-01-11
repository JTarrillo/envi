using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

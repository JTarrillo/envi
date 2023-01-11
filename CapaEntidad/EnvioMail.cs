using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EnvioMail
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
        public string Asunto { get; set; }
        public bool BodyHTML { get; set; }
        public bool PrioridadAlta { get; set; }
        public bool  NotificaFallaEntrega { get; set; }
        public bool EnvioInmediato { get; set; }
        public bool Test { get; set; }
        public int CategoriaMail { get; set; }
        public int IdRemitente { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int ErrorId { get; set; }
    }
}

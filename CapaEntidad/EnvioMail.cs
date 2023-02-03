using System;
using System.Collections.Generic;

namespace CapaEntidad
{
    public class EnvioMail
    {
        public List<EnvioMailDireccion> oEnvioMailDireccion { get; set; }

        //public List<AdjuntosMail> oAdjuntosMailes { get; set; }
        public AdjuntosMail oAdjuntosMail { get; set; }
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Mensaje { get; set; }
        public string Asunto { get; set; }
        public bool BodyHTML { get; set; }
        public bool PrioridadAlta { get; set; }
        public bool NotificaFallaEntrega { get; set; }
        public bool EnvioInmediato { get; set; }
        public bool Test { get; set; }
        public Categoria oCategoria { get; set; }
        public DireccionesMail oDireccionesMail { get; set; }
        public DateTime FechaEnvio { get; set; }
        public ErrorSendMail oErrorSendMail { get; set; }
    }
}

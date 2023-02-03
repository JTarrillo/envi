using System;

namespace CapaEntidad
{
    public class ErrorSendMail
    {

        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public int Intentos { get; set; }
        public DateTime FechaPrimerIntento { get; set; }
        public bool NoReintento { get; set; }
    }
}

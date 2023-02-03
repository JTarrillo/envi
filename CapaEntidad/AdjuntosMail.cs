

namespace CapaEntidad

{
    public class AdjuntosMail
    {
        public int Id { get; set; }
        public int IdMail { get; set; }
        public string NombreArchivo { get; set; }
        public bool Borrado { get; set; }

        public EnvioMail MiEnvioMail { get; set; }


    }
}

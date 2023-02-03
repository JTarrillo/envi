//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Web.UI.WebControls;

//namespace CapaEntidad
//{
//    public class EnvioMail
//    {
//        private int MiIdEnvioMail;
//        private DateTime MiFecha;
//        private string MiMensaje;
//        private string MiAsunto;
//        private bool MiBodyHTML;
//        private bool MiPrioridadAlta;
//        private bool MiNotificaFallaEntrega;
//        private bool MiEnvioInmediato;
//        private bool MiTest;
//        private int MiCategoriaMail;
//        private int MiIdRemitente;
//        private DateTime MiFechaEnvio;
//        private int MiErrorId;
//        private bool MiExiste;

//        private List<EnvioMailDireccion> MiEnvioMailDirecciones;
//        private List<AdjuntosMail> MiAdjuntosMail;
//        private DireccionesMail MiDireccionMailRemitente;
//        private Categoria MiCategoria;
//        private ErrorSendMail MiMotivo;

//        public class Filtro : EnvioMail
//        {
//            private QueFiltrar MiQueFiltro;
//            private QueOrdenar MiQueOrden;
//            private QueBuscar MiQueBusqueda;
//            private string MiQueTextoBuscar;
//            private DateTime MiQueFechaDesde;
//            private DateTime MiQueFechaHasta;
//            private string MiQueDireccionMailRemitente;
//            private string MiQueDireccionMailDestinatario;

//            public Filtro()
//            {
//                this.MiQueFiltro = QueFiltrar.SinFiltro;
//                this.MiQueOrden = QueOrdenar.Id;
//                this.MiQueBusqueda = 0;
//                this.MiQueTextoBuscar = "";
//                this.MiQueFechaDesde = default(DateTime);
//                this.MiQueFechaHasta = default(DateTime);
//                this.MiQueDireccionMailRemitente = "";
//                this.MiQueDireccionMailDestinatario = "";
//            }

//            public string QueDireccionMailDestinatario
//            {
//                get { return MiQueDireccionMailDestinatario; }
//                set { MiQueDireccionMailDestinatario = value; }
//            }



//            public string QueDireccionMailRemitente

//            {
//                get { return MiQueDireccionMailRemitente; }
//                set { MiQueDireccionMailRemitente = value; }
//            }



//            public QueFiltrar QueFiltro
//            {
//                get { return MiQueFiltro; }
//                set { MiQueFiltro = value; }
//            }




//            public enum QueFiltrar : int
//            {
//                SinFiltro = 0,
//                EnviadoSinError = 1,
//                EnviadoConError = 2,
//                SinEnviarConError = 3,
//                SinEnviarSinError = 4,
//                ConError = 5
//            }

//            public static string QueFiltrarDescripcion(QueFiltrar EFiltro, bool eConNumero)
//            {
//                string Descripcion = "";
//                if (eConNumero)
//                    Descripcion = System.Convert.ToInt32(EFiltro).ToString() + " ";

//                if (EFiltro == QueFiltrar.SinFiltro)
//                    Descripcion += "Sin Filtro";
//                else if (EFiltro == QueFiltrar.ConError)
//                    Descripcion += "Con Error";
//                else if (EFiltro == QueFiltrar.EnviadoConError)
//                    Descripcion += "Enviado con Error";
//                else if (EFiltro == QueFiltrar.EnviadoSinError)
//                    Descripcion += "Enviado Sin Error";
//                else if (EFiltro == QueFiltrar.SinEnviarConError)
//                    Descripcion += "Sin enviar con Error";
//                else if (EFiltro == QueFiltrar.SinEnviarSinError)
//                    Descripcion += "Sin enviar sin Error";

//                return Descripcion;
//            }

//            public static void LlenarListaFiltrar(ListBox Lst)
//            {
//                Lst.Items.Clear();

//                Lst.Items.Insert(0, new ListItem(QueFiltrarDescripcion(QueFiltrar.ConError, true), QueFiltrar.ConError.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueFiltrarDescripcion(QueFiltrar.EnviadoConError, true), QueFiltrar.EnviadoConError.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueFiltrarDescripcion(QueFiltrar.EnviadoSinError, true), QueFiltrar.EnviadoSinError.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueFiltrarDescripcion(QueFiltrar.SinEnviarConError, true), QueFiltrar.SinEnviarConError.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueFiltrarDescripcion(QueFiltrar.SinEnviarSinError, true), QueFiltrar.SinEnviarSinError.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueFiltrarDescripcion(QueFiltrar.SinFiltro, true), QueFiltrar.SinFiltro.ToString()));
//            }

//            public QueOrdenar QueOrden

//            {
//                get { return MiQueOrden; }
//                set { MiQueOrden = value; }
//            }
          

//            public enum QueOrdenar : int
//            {
//                Id = 1,
//                FechaMail = 2,
//                Asunto = 3,
//                CategoriaMail = 4,
//                MailRemitente = 5,
//                NombreRemitente = 6,
//                FechaEnvio = 7
//            }

//            public static string QueOrdenarDescripcion(QueOrdenar eOrden)
//            {
//                string Descripcion = "";

//                if (eOrden == QueOrdenar.Id)
//                    Descripcion += "Id (Asc)";
//                else if (eOrden == QueOrdenar.FechaMail)
//                    Descripcion += "Fecha Mail (Asc)";
//                else if (eOrden == QueOrdenar.Asunto)
//                    Descripcion += "Asunto (Asc)";
//                else if (eOrden == QueOrdenar.CategoriaMail)
//                    Descripcion += "Categoria (Asc)";
//                else if (eOrden == QueOrdenar.MailRemitente)
//                    Descripcion += "Mail Remitente (Asc)";
//                else if (eOrden == QueOrdenar.NombreRemitente)
//                    Descripcion += "Nombre de Remitente (Asc)";
//                else if (eOrden == QueOrdenar.FechaEnvio)
//                    Descripcion += "Fecha de Envio (Asc)";

//                return Descripcion;
//            }

//            public static void LlenarListaOrdenar(ListBox Lst)
//            {
//                Lst.Items.Clear();
//                Lst.Items.Insert(0, new ListItem(QueOrdenarDescripcion(QueOrdenar.Id), QueOrdenar.Id.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueOrdenarDescripcion(QueOrdenar.FechaMail), QueOrdenar.FechaMail.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueOrdenarDescripcion(QueOrdenar.Asunto), QueOrdenar.Asunto.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueOrdenarDescripcion(QueOrdenar.CategoriaMail), QueOrdenar.CategoriaMail.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueOrdenarDescripcion(QueOrdenar.MailRemitente), QueOrdenar.MailRemitente.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueOrdenarDescripcion(QueOrdenar.NombreRemitente), QueOrdenar.NombreRemitente.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueOrdenarDescripcion(QueOrdenar.FechaEnvio), QueOrdenar.FechaEnvio.ToString()));
//            }

//            public QueBuscar QueBusqueda

//            {
//                get { return MiQueBusqueda; }
//                set { MiQueBusqueda = value; }
//            }
        
        
//            public string QueTextoBuscar

//            {
//                get { return MiQueTextoBuscar; }
//                set { MiQueTextoBuscar = value; }
//            }
        
//            public enum QueBuscar : int
//            {
//                Id = 1,
//                Cuerpo = 2,
//                Asunto = 3,
//                MailRemitente = 4,
//                NombreRemitente = 5,
//                MailDestinatario = 6,
//                NombreDestinatario = 7,
//                DescripcionDeCategoria = 8,
//                BuscarPorAdjunto = 9
//            }

//            public static string QueBuscarDescripcion(QueBuscar eBuscar)
//            {
//                string Descripcion = "";

//                if (eBuscar == QueBuscar.Id)
//                    Descripcion += "Id";
//                else if (eBuscar == QueBuscar.Cuerpo)
//                    Descripcion += "Mensaje";
//                else if (eBuscar == QueBuscar.Asunto)
//                    Descripcion += "Asunto";
//                else if (eBuscar == QueBuscar.MailRemitente)
//                    Descripcion += "Mail del Remitente";
//                else if (eBuscar == QueBuscar.NombreRemitente)
//                    Descripcion += "Nombre del Remitente";
//                else if (eBuscar == QueBuscar.MailDestinatario)
//                    Descripcion += "Mail del  Destinatario";
//                else if (eBuscar == QueBuscar.NombreDestinatario)
//                    Descripcion += "Nombre del Destinatario";
//                else if (eBuscar == QueBuscar.DescripcionDeCategoria)
//                    Descripcion += "Descripcion de la Categoría";
//                else if (eBuscar == QueBuscar.BuscarPorAdjunto)
//                    Descripcion += "Buscar por Adjunto";

//                return Descripcion;
//            }

//            public static void LlenarListaBuscar(ListBox Lst)
//            {
//                Lst.Items.Clear();
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.Id), QueBuscar.Id.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.Cuerpo), QueBuscar.Cuerpo.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.Asunto), QueBuscar.Asunto.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.MailRemitente), QueBuscar.MailRemitente.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.NombreRemitente), QueBuscar.NombreRemitente.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.MailDestinatario), QueBuscar.MailDestinatario.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.NombreDestinatario), QueBuscar.NombreDestinatario.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.DescripcionDeCategoria), QueBuscar.DescripcionDeCategoria.ToString()));
//                Lst.Items.Insert(0, new ListItem(QueBuscarDescripcion(QueBuscar.BuscarPorAdjunto), QueBuscar.BuscarPorAdjunto.ToString()));
//            }


//            public DateTime QueFechaDesde
//            {
//                get
//                {
//                    return this.MiQueFechaDesde;
//                }
//                set
//                {
//                    this.MiQueFechaDesde = value;
//                }
//            }

//            public DateTime QueFechaHasta
//            {
//                get
//                {
//                    return this.MiQueFechaHasta;
//                }
//                set
//                {
//                    this.MiQueFechaHasta = value;
//                }
//            }
//        }


//        // -------------------------------- AQUI EMPIEZA LOS GETTER AND SETTER----------------------
        
//        public int IdEnvioMail

//        {
//            get { return MiIdEnvioMail; }
//            set { MiIdEnvioMail = value; }
//        }


//        public DateTime Fecha

//        {
//            get { return MiFecha; }
//            set { MiFecha = value; }
//        }


//        public string Mensaje

//        {
//            get { return MiMensaje; }
//            set { MiMensaje = value; }
//        }


//        public string Asunto


//        {
//            get { return MiAsunto; }
//            set { MiAsunto = value; }
//        }


//        public bool PrioridadAlta

//        {
//            get { return MiPrioridadAlta; }
//            set { MiPrioridadAlta = value; }
//        }



//        public bool BodyHTML

//        {
//            get { return MiBodyHTML; }
//            set { MiBodyHTML = value; }
//        }


//        public bool EnvioInmediato

//        {
//            get { return MiEnvioInmediato; }
//            set { MiEnvioInmediato = value; }
//        }

//        public bool NotificaFallaEntrega

//        {
//            get { return MiNotificaFallaEntrega; }
//            set { MiNotificaFallaEntrega = value; }
//        }


//        public bool Test

//        {
//            get { return MiTest; }
//            set { MiTest = value; }
//        }

//        public int CategoriaMail

//        {
//            get { return MiCategoriaMail; }
//            set { MiCategoriaMail = value; }
//        }



//        public int IdRemitente

//        {
//            get { return MiIdRemitente; }
//            set { MiIdRemitente = value; }
//        }



//        public DateTime FechaEnvio

//        {
//            get { return MiFechaEnvio; }
//            set { MiFechaEnvio = value; }
//        }




//        public int ErrorId


//        {
//            get { return MiErrorId; }
//            set { MiErrorId = value; }
//        }





//        public bool Existe
//        {
//            get
//            {
//                return this.MiExiste;
//            }
//        }



//        public List<EnvioMailDireccion> EnvioMailDirecciones
//        {
//            get
//            {
//                if (this.MiEnvioMailDirecciones == null)
//                {
//                    if (this.Existe)
//                        this.MiEnvioMailDirecciones = EnvioMailDireccion.Obtener(this.MiIdEnvioMail);
//                    else
//                        this.MiEnvioMailDirecciones = new List<EnvioMailDireccion>();
//                }
//                return this.MiEnvioMailDirecciones;
//            }
//        }

//        public string ListaDestinatariosTxt_P
//        {
//            get
//            {
//                string Lista = "";
//                foreach (EnvioMailDireccion EnvioDireccion in this.EnvioMailDirecciones)
//                {
//                    if (EnvioDireccion.Tipo == 0)
//                    {
//                        if (Lista != "")
//                            Lista = Lista + ", ";
//                        Lista = Lista + EnvioDireccion.DireccionMail.Direccion;
//                        if (EnvioDireccion.DireccionMail.Nombre != "")
//                            Lista = Lista + " (" + EnvioDireccion.DireccionMail.Nombre + ")";
//                    }
//                }
//                return Lista;
//            }
//        }

//        public string ListaDestinatariosTxt_CC
//        {
//            get
//            {
//                string Lista = "";
//                foreach (EnvioMailDireccion EnvioDireccion in this.EnvioMailDirecciones)
//                {
//                    if (EnvioDireccion.Tipo == 1)
//                    {
//                        if (Lista != "")
//                            Lista = Lista + ", ";
//                        Lista = Lista + EnvioDireccion.DireccionMail.Direccion;
//                        if (EnvioDireccion.DireccionMail.Nombre != "")
//                            Lista = Lista + "(" + EnvioDireccion.DireccionMail.Nombre + ")";
//                    }
//                }
//                return Lista;
//            }
//        }

//        public string ListaDestinatariosTxt_CCO
//        {
//            get
//            {
//                string Lista = "";
//                foreach (EnvioMailDireccion EnvioDireccion in this.EnvioMailDirecciones)
//                {
//                    if (EnvioDireccion.Tipo == 2)
//                    {
//                        if (Lista != "")
//                            Lista = Lista + ", ";
//                        Lista = Lista + EnvioDireccion.DireccionMail.Direccion;
//                        if (EnvioDireccion.DireccionMail.Nombre != "")
//                            Lista = Lista + "(" + EnvioDireccion.DireccionMail.Nombre + ")";
//                    }
//                }
//                return Lista;
//            }
//        }

//        public string ListaDestinatariosTxt_R
//        {
//            get
//            {
//                string Lista = "";
//                foreach (EnvioMailDireccion EnvioDireccion in this.EnvioMailDirecciones)
//                {
//                    if (EnvioDireccion.Tipo == 3)
//                    {
//                        if (Lista != "")
//                            Lista = Lista + ", ";
//                        Lista = Lista + EnvioDireccion.DireccionMail.Direccion;
//                        if (EnvioDireccion.DireccionMail.Nombre != "")
//                            Lista = Lista + "  (" + EnvioDireccion.DireccionMail.Nombre + ")";
//                    }
//                }
//                return Lista;
//            }
//        }

//        public DireccionesMail DireccionMailRemitente
//        {
//            get
//            {
//                if (this.MiDireccionMailRemitente == null)
//                    this.MiDireccionMailRemitente = new DireccionesMail(this.MiIdRemitente);
//                else if (this.MiIdRemitente != this.MiDireccionMailRemitente.IdDireccion)
//                    this.MiDireccionMailRemitente = new DireccionesMail(this.MiIdRemitente);
//                return this.MiDireccionMailRemitente;
//            }
//        }


//        public Categoria Categoria
//        {
//            get
//            {
//                if (this.MiCategoria == null)
//                    this.MiCategoria = new Categoria(this.MiCategoriaMail);
//                else if (this.MiCategoriaMail != this.MiCategoria.IdIdentificador)
//                    this.MiCategoria = new Categoria(this.MiCategoriaMail);
//                return this.MiCategoria;
//            }
//        }

//        public List<AdjuntosMail> AdjuntosMail
//        {
//            get
//            {
//                if (this.MiAdjuntosMail == null)
//                {
//                    if (this.Existe)
//                        this.MiAdjuntosMail = CapaEntidad.AdjuntosMail.Obtener(this.MiIdEnvioMail);
//                    else
//                        this.MiAdjuntosMail = new List<AdjuntosMail>();
//                }
//                return this.MiAdjuntosMail;
//            }
//        }
//        public string ListaNombreDeArchivos
//        {
//            get
//            {
//                string Lista = "";
//                foreach (AdjuntosMail Adjunto in this.AdjuntosMail)
//                {
//                    if (Lista != "")
//                        Lista = Lista + ", ";
//                    Lista = Lista + Adjunto.NombreArchivo;
//                }
//                return Lista;
//            }
//        }

//        public ErrorSendMail ErrorSendMail
//        {
//            get
//            {
//                if (this.MiMotivo == null)
//                    this.MiMotivo = new ErrorSendMail(this.MiErrorId);
//                else if (this.MiErrorId != this.MiMotivo.IdErrorSendMail)
//                    this.MiMotivo = new ErrorSendMail(this.MiErrorId);
//                return this.MiMotivo;
//            }
//        }



//        public EnvioMail()
//        {
//            this.MiIdEnvioMail = 0;
//            this.MiExiste = true;
//            this.LimpiarCampos();
//        }

//        public EnvioMail(int Id)
//        {
//            this.MiIdEnvioMail = Id;
//            this.LlenarCampos();
//        }

//        public EnvioMail(DataRow Row)
//        {
//            this.MiExiste = true;
//            this.LimpiarCampos();
//            this.RowAAtributos(Row);
//        }


//        private void LimpiarCampos()
//        {
//            this.IdEnvioMail = 0;
//            this.MiFecha = default(DateTime);
//            this.MiMensaje = "";
//            this.MiAsunto = "";
//            this.MiBodyHTML = false;
//            this.MiPrioridadAlta = false;
//            this.MiNotificaFallaEntrega = false;
//            this.MiEnvioInmediato = false;
//            this.MiTest = false;
//            this.MiCategoriaMail = 0;
//            this.MiIdRemitente = 0;
//            this.MiFechaEnvio = default(DateTime);
//            this.MiErrorId = 0;


//        }



//        private void RowAAtributos(DataRow Row)
//        {
//            this.MiIdEnvioMail = (int)Row.ItemArray[0];
//            this.MiFecha = (DateTime)Row.ItemArray[1]; 
//            this.MiMensaje = (string)Row.ItemArray[2];
//            this.MiAsunto = (string)Row.ItemArray[3];
//            this.MiBodyHTML = (bool)Row.ItemArray[4];
//            this.MiPrioridadAlta = (bool)Row.ItemArray[5];
//            this.MiNotificaFallaEntrega = (bool)Row.ItemArray[6];
//            this.MiEnvioInmediato = (bool)Row.ItemArray[7];
//            this.MiTest = (bool)Row.ItemArray[8];
//            this.MiCategoriaMail = (int)Row.ItemArray[9];
//            this.MiIdRemitente = (int)Row.ItemArray[10];
//            this.MiFechaEnvio = (DateTime)Row.ItemArray[11];
//            this.MiErrorId = (int)Row.ItemArray[12];
//        }

//        private void LlenarCampos()
//        {
//            if (this.MiIdEnvioMail == 0)
//            {
//                this.MiExiste = false;
//                this.LimpiarCampos();
//            }
//            else
//            {
//                string StrSql = "SELECT  * FROM EnvioMail " + "WHERE EnvioMail.Id =  " + System.Convert.ToString(this.MiIdEnvioMail).Length;
//                DataTable MiDt = new DataTable();

//                using (System.Data.SqlClient.SqlConnection Cn = new System.Data.SqlClient.SqlConnection(EnvioDeNotificaciones.EnvioDeMail.CnStr))
//                {
//                    Cn.Open();
//                    System.Data.SqlClient.SqlTransaction Tr;
//                    Tr = Cn.BeginTransaction(IsolationLevel.ReadUncommitted);
//                    System.Data.SqlClient.SqlDataAdapter Da = new System.Data.SqlClient.SqlDataAdapter(StrSql, Cn);
//                    Da.SelectCommand.Transaction = Tr;
//                    Da.SelectCommand.CommandTimeout = EnvioDeNotificaciones.EnvioDeMail.TiempoDeEsperaSQL;

//                    Da.Fill(MiDt);
//                    Tr.Commit();
//                    Tr.Dispose();
//                    Da.Dispose();
//                    Cn.Close();
//                    Cn.Dispose();
//                }

//                this.LimpiarCampos();
//                if (MiDt.Rows.Count == 0)
//                    this.MiExiste = false;
//                else
//                {
//                    this.MiExiste = true;
//                    this.RowAAtributos(MiDt.Rows[0]);
//                }
//                MiDt.Dispose();
//            }
//        }

//        public static List<EnvioMail> Obtener(EnvioMail.Filtro Filtro, ref int NumeroPagina, int TamañoDePagina, ref int CantidadDePaginas)
//        {
//            List<EnvioMail> Lista = new List<EnvioMail>();
//            string StrWhere = "WHERE EnvioMail.Id IS NOT NULL";
//            string StrJoin = "";
//            string StrOrder = "";
//            bool SumaCategoria_INNER = false;
//            bool SumaRemitente_INNER = false;
//            DataTable MiDt = new DataTable();

//            using (System.Data.SqlClient.SqlConnection Cn = new System.Data.SqlClient.SqlConnection(EnvioDeNotificaciones.EnvioDeMail.CnStr))
//            {
//                System.Data.SqlClient.SqlDataAdapter Da = new System.Data.SqlClient.SqlDataAdapter("", Cn);
//                System.Data.SqlClient.SqlCommand Cm = new System.Data.SqlClient.SqlCommand("", Cn);

//                if (Filtro.QueFiltro == EnvioMail.Filtro.QueFiltrar.EnviadoSinError)
//                    StrWhere = StrWhere + " AND EnvioMail.FechaEnvio is not null and EnvioMail.ErrorId = 0  ";
//                else if (Filtro.QueFiltro == EnvioMail.Filtro.QueFiltrar.EnviadoConError)
//                    StrWhere = StrWhere + " AND EnvioMail.FechaEnvio is not null  AND EnvioMail.ErrorId <> 0";
//                else if (Filtro.QueFiltro == EnvioMail.Filtro.QueFiltrar.SinEnviarConError)
//                    StrWhere = StrWhere + " AND EnvioMail.FechaEnvio is null  AND EnvioMail.ErrorId <> 0";
//                else if (Filtro.QueFiltro == EnvioMail.Filtro.QueFiltrar.SinEnviarSinError)
//                    StrWhere = StrWhere + " AND EnvioMail.FechaEnvio is null And EnvioMail.ErrorId = 0";
//                else if (Filtro.QueFiltro == EnvioMail.Filtro.QueFiltrar.ConError)
//                    StrWhere = StrWhere + " AND EnvioMail.ErrorId <> 0";

//                if (Filtro.IdEnvioMail > 0)
//                    StrWhere = StrWhere + " AND EnvioMail.Id = " + Filtro.IdEnvioMail.ToString();

//                if (Filtro.Asunto != "")
//                {
//                    StrWhere = StrWhere + " AND EnvioMail.Asunto = @Asunto";
//                    Da.SelectCommand.Parameters.AddWithValue("@Asunto", Filtro.Asunto.Length);
//                }

//                if (Filtro.PrioridadAlta == true)
//                    StrWhere = StrWhere + " AND EnvioMail.PrioridadAlta = 1";

//                if (Filtro.BodyHTML == true)
//                    StrWhere = StrWhere + " AND EnvioMail.BodyHTML = 1";

//                if (Filtro.EnvioInmediato == true)
//                    StrWhere = StrWhere + " AND EnvioMail.EnvioInmediato = 1";

//                if (Filtro.NotificaFallaEntrega == true)
//                    StrWhere = StrWhere + " AND EnvioMail.MiNotificaFallaEntrega = 1";

//                if (Filtro.Test == true)
//                    StrWhere = StrWhere + " AND EnvioMail.MiTest = 1";

//                if (Filtro.CategoriaMail > 0)
//                    StrWhere = StrWhere + " AND EnvioMail.CategoriaMail = " + Filtro.CategoriaMail.ToString();

//                if (Filtro.IdRemitente > 0)
//                    StrWhere = StrWhere + " AND EnvioMail.IdRemitente = " + Filtro.IdRemitente.ToString();

//                if (Filtro.ErrorId > 0)
//                    StrWhere = StrWhere + " AND EnvioMail.ErrorId = " + Filtro.ErrorId.ToString();

//                if (!Filtro.QueFechaDesde == null/* TODO Change to default(_) if this is not a reference type */ )
//                {
//                    StrWhere = StrWhere + " AND EnvioMail.FechaEnvio >= @FechaDesde";
//                    Da.SelectCommand.Parameters.AddWithValue("@FechaDesde", Filtro.QueFechaDesde);
//                }
//                if (!Filtro.QueFechaHasta == null/* TODO Change to default(_) if this is not a reference type */ )
//                {
//                    StrWhere = StrWhere + " AND EnvioMail.FechaEnvio <= @FechaHasta";
//                    if (Filtro.QueFechaHasta.Hour == 0 & Filtro.QueFechaHasta.Minute == 0)
//                        Da.SelectCommand.Parameters.AddWithValue("@FechaHasta", Convert.ToDateTime(Filtro.QueFechaHasta.ToShortDateString + " 23:59:59"));
//                    else
//                        Da.SelectCommand.Parameters.AddWithValue("@FechaHasta", Filtro.QueFechaHasta);
//                }

//                if (Filtro.QueDireccionMailRemitente.Length > 0)
//                {
//                    StrWhere = StrWhere + " AND EnvioMail.IdRemitente IN (SELECT DireccionesMail.IdDireccion FROM DireccionesMail  WHERE  DireccionesMail.Direccion LIKE @DireccionMailRemitente)";
//                    Da.SelectCommand.Parameters.AddWithValue("@DireccionMailRemitente", "%" + Filtro.QueDireccionMailRemitente.Length + "%");
//                }

//                if (Filtro.QueDireccionMailDestinatario.Length > 0)
//                {
//                    StrWhere = StrWhere + " AND EnvioMail.Id IN (SELECT EnvioMailDireccion.IdMail FROM EnvioMailDireccion INNER JOIN DireccionesMail ON DireccionesMail.IdDireccion = EnvioMailDireccion.IdDireccion WHERE DireccionesMail.Direccion LIKE @DireccionMailDestinatario)";
//                    Da.SelectCommand.Parameters.AddWithValue("@DireccionMailDestinatario", "%" + Filtro.QueDireccionMailDestinatario.Length + "%");
//                }

//                if (Filtro.QueTextoBuscar.Length > 0)
//                {
//                    if (Filtro.QueBusqueda == EnvioMail.Filtro.QueBuscar.Id)
//                        StrWhere = StrWhere + " AND EnvioMail.Id = @TextoABuscar";
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.Cuerpo)
//                        StrWhere = StrWhere + " AND EnvioMail.Mensaje LIKE @TextoABuscar";
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.Asunto)
//                        StrWhere = StrWhere + " AND EnvioMail.Asunto LIKE @TextoABuscar";
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.MailRemitente)
//                        StrWhere = StrWhere + " AND EnvioMail.IdRemitente IN (SELECT DireccionesMail.IdDireccion FROM DireccionesMail  WHERE DireccionesMail.Direccion LIKE @TextoABuscar)";
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.MailDestinatario)
//                        StrWhere = StrWhere + " AND EnvioMail.Id IN (SELECT EnvioMailDireccion.IdMail FROM EnvioMailDireccion INNER JOIN DireccionesMail ON DireccionesMail.IdDireccion = EnvioMailDireccion.IdDireccion WHERE DireccionesMail.Direccion LIKE @TextoABuscar)";
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.NombreDestinatario)
//                        StrWhere = StrWhere + " AND EnvioMail.Id IN (SELECT EnvioMailDireccion.IdMail FROM EnvioMailDireccion INNER JOIN DireccionesMail ON DireccionesMail.IdDireccion = EnvioMailDireccion.IdDireccion WHERE DireccionesMail.Nombre LIKE @TextoABuscar)";
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.NombreRemitente)
//                        StrWhere = StrWhere + " AND EnvioMail.IdRemitente IN (SELECT DireccionesMail.IdDireccion FROM DireccionesMail  WHERE DireccionesMail.Nombre LIKE @TextoABuscar)";
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.DescripcionDeCategoria)
//                    {
//                        StrWhere = StrWhere + " AND Categoria.Descripcion LIKE @TextoABuscar";
//                        SumaCategoria_INNER = true;
//                    }
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.BuscarPorAdjunto)
//                        StrWhere = StrWhere + " AND EnvioMail.Id IN (SELECT AdjuntosMail.IdMail FROM AdjuntosMail WHERE AdjuntosMail.NombreArchivo LIKE @TextoABuscar)";

//                    if (Filtro.QueBusqueda == Filtro.QueBuscar.Id)
//                    {
//                        int Numero;
//                        if (int.TryParse(Filtro.QueTextoBuscar, out Numero))
//                            Da.SelectCommand.Parameters.AddWithValue("@TextoABuscar", Numero);
//                        else
//                            Da.SelectCommand.Parameters.AddWithValue("@TextoABuscar", 0);
//                    }
//                    else if (Filtro.QueBusqueda == Filtro.QueBuscar.DescripcionDeCategoria | Filtro.QueBusqueda == Filtro.QueBuscar.NombreDestinatario | Filtro.QueBusqueda == Filtro.QueBuscar.NombreRemitente | Filtro.QueBusqueda == Filtro.QueBuscar.MailDestinatario | Filtro.QueBusqueda == Filtro.QueBuscar.MailRemitente | Filtro.QueBusqueda == Filtro.QueBuscar.BuscarPorAdjunto | Filtro.QueBusqueda == Filtro.QueBuscar.Asunto | Filtro.QueBusqueda == Filtro.QueBuscar.Cuerpo)
//                        Da.SelectCommand.Parameters.AddWithValue("@TextoABuscar", "%" + Filtro.QueTextoBuscar.Length + "%");
//                    else
//                        Da.SelectCommand.Parameters.AddWithValue("@TextoABuscar", Filtro.QueTextoBuscar.Length);
//                }

//                if (Filtro.QueOrden != Filtro.QueOrdenar.Id)
//                {
//                    if (Filtro.QueOrden == Filtro.QueOrdenar.Id)
//                        StrOrder = "ORDER BY EnvioMail.Id";
//                    else if (Filtro.QueOrden == Filtro.QueOrdenar.Asunto)
//                        StrOrder = "ORDER BY EnvioMail.Asunto Asc";
//                    else if (Filtro.QueOrden == Filtro.QueOrdenar.CategoriaMail)
//                    {
//                        StrOrder = "ORDER by Categoria.Descripcion Asc";
//                        SumaCategoria_INNER = true;
//                    }
//                    else if (Filtro.QueOrden == Filtro.QueOrdenar.MailRemitente)
//                    {
//                        StrOrder = "ORDER BY DireccionesMailRemitente.Direccion Asc";
//                        SumaRemitente_INNER = true;
//                    }
//                    else if (Filtro.QueOrden == Filtro.QueOrdenar.NombreRemitente)
//                    {
//                        StrOrder = "ORDER BY DireccionesMailRemitente.Nombre Asc";
//                        SumaRemitente_INNER = true;
//                    }
//                    else if (Filtro.QueOrden == Filtro.QueOrdenar.FechaMail)
//                        StrOrder = "ORDER BY EnvioMail.Fecha Asc";
//                    else if (Filtro.QueOrden == Filtro.QueOrdenar.FechaEnvio)
//                        StrOrder = "ORDER BY EnvioMail.FechaEnvio Asc";
//                }
//                else if (NumeroPagina > 0 & TamañoDePagina > 0)
//                    StrOrder = "ORDER BY EnvioMail.Id";



//                if (SumaCategoria_INNER)
//                    StrJoin = StrJoin + "INNER JOIN Categoria ON Categoria.Identificador = EnvioMail.CategoriaMail " + "\n\r";

//                if (SumaRemitente_INNER)
//                    StrJoin = StrJoin + "INNER JOIN DireccionesMail AS DireccionesMailRemitente ON EnvioMail.IdRemitente = DireccionesMailRemitente.IdDireccion" + "\n\r";


//                string StrSqlCantidad = "SELECT COUNT(*) FROM EnvioMail";

//                StrSqlCantidad = StrSqlCantidad + " " + StrJoin + " " + StrWhere;
//                Cm.CommandText = StrSqlCantidad;
//                foreach (System.Data.SqlClient.SqlParameter Parametro in Da.SelectCommand.Parameters)
//                    Cm.Parameters.AddWithValue(Parametro.ParameterName, Parametro.Value);
//                Cm.CommandTimeout = EnvioDeNotificaciones.EnvioDeMail.TiempoDeEsperaSQL;

//                Cn.Open();
//                System.Data.SqlClient.SqlTransaction Tr;
//                Tr = Cn.BeginTransaction(IsolationLevel.ReadUncommitted);
//                Cm.Transaction = Tr;
//                int CantidadRegistros = (int)Cm.ExecuteScalar();
//                Cm.Dispose();
//                if (NumeroPagina > 0 & TamañoDePagina > 0)
//                {
//                    CantidadDePaginas = (int)Math.Ceiling((double)Convert.ToDecimal(CantidadRegistros) / (double)Convert.ToDecimal(TamañoDePagina));
//                    if (NumeroPagina > CantidadDePaginas)
//                        NumeroPagina = 1;
//                }

//                if (CantidadRegistros > 0)
//                {
//                    string StrSql = "";
//                    if (NumeroPagina > 0 & TamañoDePagina > 0)
//                    {
//                        StrSql = StrSql + "SELECT T.*" + "\n\r";
//                        StrSql = StrSql + "FROM (" + "\n\r";
//                        StrSql = StrSql + "   SELECT ROW_NUMBER() OVER(" + StrOrder + ") AS numeroRegistro" + "\n\r";
//                        StrSql = StrSql + "   ,EnvioMail.*" + "\n\r";
//                        StrSql = StrSql + "   FROM EnvioMail" +  "\n\r";
//                        StrSql = StrSql + "   " + StrJoin + "\n\r";
//                        StrSql = StrSql + "   " + StrWhere + "\n\r";
//                        StrSql = StrSql + ") AS T" + "\n\r";
//                        StrSql = StrSql + "WHERE T.numeroRegistro >= " + (((NumeroPagina - 1) * TamañoDePagina) + 1) + "\n\r";
//                        StrSql = StrSql + "AND T.numeroRegistro <= " + (NumeroPagina * TamañoDePagina) + "\n\r";
//                    }
//                    else
//                        StrSql = "select  EnvioMail.* from EnvioMail  " + StrJoin + " " + StrWhere + " " + StrOrder;

//                    Da.SelectCommand.CommandText = StrSql;
//                    Da.SelectCommand.CommandTimeout = EnvioDeNotificaciones.EnvioDeMail.TiempoDeEsperaSQL; ;
//                    Da.SelectCommand.Transaction = Tr;
//                    Da.Fill(MiDt);
//                    Da.Dispose();
//                }

//                Tr.Commit();
//                Tr.Dispose();
//                Cn.Close();
//                Cn.Dispose();
//            }

//            int i = 0;
//            while (i < MiDt.Rows.Count)
//            {
//                EnvioMail Objeto = new EnvioMail(MiDt.Rows[i]);
//                Lista.Add(Objeto);
//                i = i + 1;
//            }
//            MiDt.Dispose();
//            return Lista;
//        }


//    }
//}
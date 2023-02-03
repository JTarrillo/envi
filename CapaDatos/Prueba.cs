//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using CapaEntidad;

//namespace CapaDatos
//{
//    public class FormularioConsulta : System.Web.UI.Page
//    {
//        protected void Page_Load(object sender, System.EventArgs e)
//        {
//            System.Web.UI.Page.Session["IdUsuario"] = 333; // EMILIANO
//            System.Web.UI.Page.Session["Idempresa"] = 1;

//            if (System.Web.UI.Page.Session["IdUsuario"] == null)
//                System.Web.UI.Page.Response.Redirect("PaginaDeError.aspx?Id=" + "Tiempo de sesion caducado. Debe reingresar a la Intranet.");
//            int IdUsuario = Conversion.Val(System.Web.UI.Page.Session["IdUsuario"]);
//            ClSeguridad.Usuario Usu = new ClSeguridad.Usuario(IdUsuario);
//            string Formulario = System.Web.UI.Page.Request.Url.AbsolutePath.Remove(0, System.Web.UI.Control.Page.TemplateSourceDirectory.Length + 1).Trim();
//            if (Usu.PuedeIngresar(IdDelSistema, Formulario, System.Web.UI.Control.Page.IsPostBack) == false)
//            {
//                Usu.Dispose();
//                System.Web.UI.Page.Response.Redirect("PaginaDeError.aspx?Id=" + "Tarea no permitida.");
//            }
//            Usu.Dispose();



//            if (!System.Web.UI.Control.Page.IsPostBack)
//            {
//                EnvioMail.Filtro.LlenarListaFiltrar(this.LstQueFiltro);
//                EnvioMail.Filtro.LlenarListaOrdenar(this.LstQueOrden);
//                EnvioMail.Filtro.LlenarListaBuscar(this.LstQueBuscar);
//                FiltroBusqueda.LlenarListaCantidadElementos(this.PaginadorLstCantidadElementos);
//                this.PaginadorLstCantidadElementos.SelectedIndex = 0;

//                Categoria.Filtro CategoriaFiltro = new Categoria.Filtro();
//                Categoria.LlenarLista(this.LstCategoria, "Todos", CategoriaFiltro);

//                FiltroBusqueda.LlenarListaRangosDeFecha(this.LstQueFecha);
//                this.CambiarFecha();

//                if (!string.IsNullOrEmpty(System.Web.UI.Page.Request.QueryString["Categoria"]))
//                {
//                    long IdCategoria = Val(GSCF.Encriptar.Desencriptar(System.Web.UI.Page.Request.QueryString["Categoria"].ToString().Replace(" ", "+"), EnvioDeNotificaciones.EnvioDeMail.ClaveEncriptacion));
//                    try
//                    {
//                        this.LstCategoria.SelectedValue = IdCategoria;
//                        this.LlenarLista();
//                    }
//                    catch
//                    {
//                    }
//                }
//            }
//            this.MaintainScrollPositionOnPostBack = true;
//            GSCF.BloquearPantalla.GenerarScriptParaBloquearPantallaEnTodosLosPostBack(this.Page, this.form1);
//            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "AbrirPagina", EnvioDeNotificaciones.EnvioDeMail.ScriptDeAbrirPagina(), false);
//        }


//        private System.Collections.Generic.List<EnvioMail> ListaDeEnvioDeNotificaciones(bool ConPaginacion)
//        {
//            EnvioMail.Filtro Filtro = new EnvioMail.Filtro();
//            Filtro.QueFiltro = Val(this.LstQueFiltro.SelectedValue);
//            Filtro.QueBusqueda = Val(this.LstQueBuscar.SelectedValue);
//            Filtro.QueOrden = Val(this.LstQueOrden.SelectedValue);
//            Filtro.CategoriaMail = Val(this.LstCategoria.SelectedValue);
//            Filtro.QueTextoBuscar = this.TxtBuscar.Text.Trim;
//            Filtro.QueFechaDesde = DateTime.Parse(this.TxtFechaDesde.Text);
//            Filtro.QueFechaHasta = DateTime.Parse(this.TxtFechaHasta.Text);
//            Filtro.QueDireccionMailRemitente = this.TxtRemitente.Text.Trim;
//            Filtro.QueDireccionMailDestinatario = this.TxtDestinatario.Text.Trim;

//            int Pagina;
//            int Paginas = 1;
//            if (ConPaginacion)
//                Pagina = this.PaginadorLstPaginaActual.SelectedValue;
//            else
//                Pagina = -1;

//            System.Collections.Generic.List<EnvioMail> Notificacion = EnvioMail.Obtener(Filtro, Pagina, this.PaginadorLstCantidadElementos.SelectedValue, Paginas);

//            if (ConPaginacion)
//            {
//                FiltroBusqueda.LlenarListaDePaginas(this.PaginadorLstPaginaActual, Paginas);
//                try
//                {
//                    this.PaginadorLstPaginaActual.SelectedValue = Pagina;
//                }
//                catch (Exception ex)
//                {
//                    this.PaginadorLstPaginaActual.ClearSelection();
//                }
//                this.PaginadorLblDe.Text = "de " + Paginas;
//            }

//            return Notificacion;
//        }


//        private void LlenarLista()
//        {
//            System.Collections.Generic.List<EnvioMail> Lista = this.ListaDeEnvioDeNotificaciones(true);

//            this.GridClientes.DataSource = Lista;

//            this.GridClientes.DataBind();

//            if (this.GridClientes.Rows.Count == 0)
//                this.PanelPaginador.Visible = true;
//            else
//                this.PanelPaginador.Visible = true;
//        }

//        private bool ValidarDatos()
//        {
//            string Mensaje = "";
//            DateTime FechaDesde;
//            DateTime FechaHasta;
//            if (this.TxtFechaDesde.Text.Trim.Length == 0)
//                Mensaje = Mensaje + "Falta fecha desde" + Constants.vbCrLf;
//            else if (!DateTime.TryParse(this.TxtFechaDesde.Text.Trim, ref FechaDesde))
//                Mensaje = Mensaje + "Fecha desde erronea" + Constants.vbCrLf;
//            if (this.TxtFechaHasta.Text.Trim.Length == 0)
//                Mensaje = Mensaje + "Falta fecha hasta" + Constants.vbCrLf;
//            else if (!DateTime.TryParse(this.TxtFechaHasta.Text.Trim, ref FechaHasta))
//                Mensaje = Mensaje + "Fecha hasta erronea" + Constants.vbCrLf;
//            if (Mensaje == "")
//            {
//                if (FechaDesde > FechaHasta)
//                    Mensaje = Mensaje + "Fecha desde no puede ser posterior a hasta" + Constants.vbCrLf;
//            }
//            if (Mensaje != "")
//            {
//                System.Web.UI.Page.ClientScript.RegisterStartupScript(this.GetType(), "mensajeError", "<script language='javascript'>alert(\"" + GSCF.ArreglarCadenaJavascript(Mensaje) + "\")</script>", false);
//                return false;
//            }
//            return true;
//        }

//        protected void BtnCuentaMostrar_Click(object sender, EventArgs e)
//        {
//            // Dim DtTest As DataTable = ObtenerDataTableMails()

//            if (!this.ValidarDatos())
//                return;

//            FiltroBusqueda.LlenarListaDePaginas(this.PaginadorLstPaginaActual, 1);
//            this.PaginadorLstPaginaActual.SelectedValue = 1;




//            this.LlenarLista();
//        }

//        private void GridClientes_RowDataBound(object sender, GridViewRowEventArgs e)
//        {
//            if (e.Row.RowType == DataControlRowType.DataRow)
//            {
//                WebControls.LinkButton Btn = e.Row.FindControl("BtnMensaje");
//                Btn.OnClientClick = "return AbrirPagina('Mensaje.aspx?Id=" + GSCF.Encriptar.Encriptar(Btn.CommandArgument, EnvioDeNotificaciones.EnvioDeMail.ClaveEncriptacion) + "','1000','600');";
//            }
//        }

//        private void CambiarFecha()
//        {
//            if (this.LstQueFecha.SelectedValue == 999)
//            {
//                this.TxtFechaDesde.Enabled = true;
//                this.TxtFechaHasta.Enabled = true;
//            }
//            else
//            {
//                this.TxtFechaDesde.Enabled = false;
//                this.TxtFechaHasta.Enabled = false;
//                this.TxtFechaHasta.Text = DateTime.Today.ToString("dd/MM/yyyy");
//                this.TxtFechaDesde.Text = DateTime.Today.AddDays(-1 * this.LstQueFecha.SelectedValue).ToString("dd/MM/yyyy");
//            }
//        }

//        private void LstQueFecha_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            this.CambiarFecha();
//        }

//        private void PaginadorLstCantidadElementos_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (!this.ValidarDatos())
//                return;

//            this.LlenarLista();
//        }

//        private void PaginadorBtnInicio_Click(object sender, EventArgs e)
//        {
//            if (!this.ValidarDatos())
//                return;

//            this.PaginadorLstPaginaActual.SelectedValue = 1;

//            this.LlenarLista();
//        }

//        private void PaginadorBtnAnterior_Click(object sender, EventArgs e)
//        {
//            if (!this.ValidarDatos())
//                return;

//            if (this.PaginadorLstPaginaActual.SelectedValue > 1)
//                this.PaginadorLstPaginaActual.SelectedValue = this.PaginadorLstPaginaActual.SelectedValue - 1;

//            this.LlenarLista();
//        }

//        private void PaginadorLstPaginaActual_SelectedIndexChanged(object sender, EventArgs e)
//        {
//            if (!this.ValidarDatos())
//                return;

//            this.LlenarLista();
//        }

//        private void PaginadorBtnSiguiente_Click(object sender, EventArgs e)
//        {
//            if (!this.ValidarDatos())
//                return;

//            if (this.PaginadorLstPaginaActual.SelectedValue < this.PaginadorLstPaginaActual.Items.Count)
//                this.PaginadorLstPaginaActual.SelectedValue = this.PaginadorLstPaginaActual.SelectedValue + 1;

//            this.LlenarLista();
//        }

//        private void PaginadorBtnFin_Click(object sender, EventArgs e)
//        {
//            if (!this.ValidarDatos())
//                return;

//            this.PaginadorLstPaginaActual.SelectedValue = this.PaginadorLstPaginaActual.Items.Count;

//            this.LlenarLista();
//        }
//    }

//}


Imports EnvioDeNotificaciones
Imports EnvioDeNotificaciones.EnvioDeMail
Imports EnvioDeNotificaciones.Funciones
Imports System.Collections.Generic
Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("IdUsuario") = Nothing Then
            Response.Redirect("PaginaDeError.aspx?Id=" & "Tiempo de sesion caducado. Debe reingresar a la Intranet.")
        End If
        Dim IdUsuario As Integer = Val(Session("IdUsuario"))
        Dim Usu As New ClSeguridad.Usuario(IdUsuario)
        Dim Formulario As String = Request.Url.AbsolutePath.Remove(0, Page.TemplateSourceDirectory.Length + 1).Trim
        If Usu.PuedeIngresar(IdDelSistema, Formulario, Page.IsPostBack) = False Then
            Usu.Dispose()
            Response.Redirect("PaginaDeError.aspx?Id=" & "Tarea no permitida.")
        End If
        Usu.Dispose()

        If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("Id")) Then
                Dim IdMail As Long = Val(GSCF.Desencriptar(Request.QueryString("Id").ToString().Replace(" ", "+"), ClaveEncriptacion))

                Try
                    Me.LlenarLista(IdMail)
                Catch
                End Try
            End If
        End If
    End Sub

    Private Sub LlenarLista(ByVal Id As Integer)


        'Dim Lista As System.Collections.Generic.List(Of EnvioMail)



        'Dim Lista As New EnvioDeNotificaciones.Funciones.EnvioMail()
        'Do While I < Lista.Mensaje.Rows.Count
        '    Dim Mensaje = Lista.Rows(I).Item("Mensaje")
        '    Lista.Rows(I).Item("Mensaje") = HttpUtility.HtmlEncode(Mensaje)
        '    I += 1
        'Loop

        'Me.GridMensaje.DataSource = Lista
        'Me.GridMensaje.DataBind()



    End Sub


End Class
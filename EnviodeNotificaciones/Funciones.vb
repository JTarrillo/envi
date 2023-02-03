
Namespace Funciones
    Public Class Funciones
        Public Shared Sub EnviarMailError(Mensaje As String, Programa As String, Metodo As String, Linea As Integer, Asunto As String, Destinatarios As DestinoMailDeError, OtrosDestinatarios As String)
            Dim RemitenteMail As New DireccionDeMail()
            RemitenteMail.Direccion = "control-sistemas@cruzdelsur.com"
            RemitenteMail.Nombre = "ACTUALIZACIONES AUTOMATICAS"
            Dim ListaDestinatarios As New List(Of DireccionDeMail)

            If Destinatarios = DestinoMailDeError.Sistemas Or Destinatarios = DestinoMailDeError.SistemasYWebmaster Then
                Dim Destinatario As New DireccionDeMail()
                Destinatario.Direccion = "control-sistemas@cruzdelsur.com"
                ListaDestinatarios.Add(Destinatario)
            End If
            If Destinatarios = DestinoMailDeError.Webmaster Or Destinatarios = DestinoMailDeError.SistemasYWebmaster Then
                Dim Destinatario As New DireccionDeMail()
                Destinatario.Direccion = "webmaster@cruzdelsur.com"
                ListaDestinatarios.Add(Destinatario)
            End If
            If OtrosDestinatarios.Length > 0 Then
                Dim X As Integer = 0
                Dim ListaMails As Array
                ListaMails = OtrosDestinatarios.Split(";")
                X = 0
                Do While X <= UBound(ListaMails)
                    If CStr(ListaMails(X)).Trim.Length <> 0 Then
                        Try
                            Dim Destinatario As New DireccionDeMail()
                            Destinatario.Direccion = CStr(ListaMails(X)).Trim
                            ListaDestinatarios.Add(Destinatario)
                        Catch
                        End Try
                    End If
                    X = X + 1
                Loop
            End If

            If Asunto.Length = 0 Then
                Asunto = GSCF.SacarRarosParaAsuntoDeMail("Error en Proceso de Actualizacion Automatica")
            Else
                Asunto = GSCF.SacarRarosParaAsuntoDeMail(Asunto)
            End If

            Dim html As String = ""
            html = html & "<b>Mensaje del error:</b><br/>"
            html = html & GSCF.ArreglarHTML(Mensaje)
            html = html & "<br/><b>Programa:</b><br/>"
            html = html & GSCF.ArreglarHTML(Programa)
            html = html & "<br/><b>Metodo:</b><br/>"
            html = html & GSCF.ArreglarHTML(Metodo)
            html = html & "<br/><b>Linea:</b><br/>"
            html = html & GSCF.ArreglarHTML(Linea.ToString())
            html = html

            Dim CC = New List(Of DireccionDeMail)
            Dim CCO = New List(Of DireccionDeMail)
            Dim ResponderA = New List(Of DireccionDeMail)
            Dim Adjunto = New List(Of ArchivoAdjunto)
            Dim Categoria = 96

            Dim MensajeEnvio As String = EnvioDeMail.GuardarEnviodeMails(RemitenteMail, ListaDestinatarios, CC, CCO, ResponderA, html, Asunto, Adjunto, True, False, False, False, Categoria, False)
        End Sub

        Public Enum DestinoMailDeError
            SistemasYWebmaster = 0
            Sistemas = 1
            Webmaster = 2
        End Enum
    End Class
End Namespace

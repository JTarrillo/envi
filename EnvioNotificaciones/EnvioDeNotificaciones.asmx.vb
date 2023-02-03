Imports System.ComponentModel
Imports System.Web.Services
Imports EnvioDeNotificaciones

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://intranet.cruzdelsur.com/Webservices/EnvioNotificaciones")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class EnvioDeNotificaciones
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function EnvioSMS(ByVal sms As SMS, ByVal EnvioInmediato As Boolean, ByVal Categroria As Integer, ByVal Test As Boolean) As String
        Return EnvioDeSMS.GuardarEnviodeSMS(sms, EnvioInmediato, Categroria, Test)
    End Function

    <WebMethod()>
    Public Function ProcesarSMS() As String
        Return EnvioDeSMS.ProcesarEnvioSMS()
    End Function

    <WebMethod()>
    Public Function EnvioMail(ByVal Remitente As DireccionDeMail, ByVal Destinatario As List(Of DireccionDeMail), ByVal CC As List(Of DireccionDeMail), ByVal CCO As List(Of DireccionDeMail), ByVal ResponderA As List(Of DireccionDeMail), ByVal Mensaje As String, ByVal Asunto As String, ByVal ArchivosAdjuntos As List(Of ArchivoAdjunto), ByVal BodyHTML As Boolean, ByVal PrioridadAlta As Boolean, ByVal NotificarFallaEntrega As Boolean, ByVal EnvioInmediato As Boolean, ByVal Categoria As Integer, ByVal Test As Boolean) As String
        Return EnvioDeMail.GuardarEnviodeMails(Remitente, Destinatario, CC, CCO, ResponderA, Mensaje, Asunto, ArchivosAdjuntos, BodyHTML, PrioridadAlta, NotificarFallaEntrega, EnvioInmediato, Categoria, Test)
    End Function

    <WebMethod()>
    Public Function ProcesarMails() As String
        Return EnvioDeMail.ProcesarEnvioCorreos()
    End Function

    <WebMethod()>
    Public Function ProcesarUnMail(ByVal IdMail As Integer) As String
        Return EnvioDeMail.ProcesarEnvioCorreos(IdMail)
    End Function

    <WebMethod()>
    Public Function EnviarWhatsApp() As String
        Return EnvioDeWhatsApp.EnviarWhatsApp(1130360984, "Hola")
    End Function

    <WebMethod()>
    Public Function SolicitarRutaTemporal() As String
        Return EnvioDeMail.ObtenerRutaTemporalAdjuntos()
    End Function
End Class
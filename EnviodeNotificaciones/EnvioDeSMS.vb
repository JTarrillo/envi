Imports System.Data.SqlClient
Imports System.Net
Imports Microsoft.ApplicationBlocks.Data

Public Module EnvioDeSMS
    Public CnStr As String = ClSeguridad.StringDeConexion("EnvioDeNotificaciones", "")

    Private SmsMasivos_Usuario As String = "SMSPLUSDEMO48145"
    Private SmsMasivos_Clave As String = "SMSPLUSDEMO48145593"

    Private Telefonica_Url As String = "http://64.76.120.14:6080/mgtest"
    Private Telefonica_Usuario As String = "movilgate"
    Private Telefonica_Clave As String = "M0v1lg4t3"
    Private Telefonica_NumeroCorto As String = "50150"

    Private infobip_Url As String = "https://9rlpx4.api.infobip.com/sms/1/text/single"
    Private infobip_Usuario As String = "100Cruz!"
    Private infobip_Clave As String = "Infobip2019!"
    Private infobip_From As String = "CDS"

    Public Class infobip_Respuesta
        Public messages As infobip_MensajeRespuesta()
    End Class

    Public Class infobip_MensajeRespuesta
        Public to_ As String
        Public status As infobip_MensajeRespuestaStatus
        Public messageId As String
        Public smsCount As Integer
    End Class

    Public Class infobip_MensajeRespuestaStatus
        Public groupId As Integer
        Public groupName As String
        Public id As Integer
        Public name As String
        Public description As String
    End Class

    Public Class SMS
        Private _Numero As String
        Private _Mensaje As String

        Public Property Numero() As String
            Get
                Return _Numero
            End Get
            Set(ByVal value As String)
                _Numero = value
            End Set
        End Property

        Public Property Mensaje() As String
            Get
                Return _Mensaje
            End Get
            Set(ByVal value As String)
                _Mensaje = value
            End Set
        End Property
    End Class

    Private Function limpiarNumero(ByVal numero As String)
        Dim limpio As String = ""
        For i As Integer = 0 To numero.Length - 1
            If IsNumeric(numero(i)) Then
                limpio = limpio + numero(i)
            End If
        Next
        Return limpio
    End Function

    Private Function limpiarCaracteresPeligrosos(ByVal mensaje As String)
        Dim limpio As String = ""

        For i As Integer = 0 To mensaje.Length - 1
            If mensaje(i) <> ">" And mensaje(i) <> "<" And mensaje(i) <> "'" And mensaje(i) <> """" And mensaje(i) <> "\" Then
                limpio = limpio + mensaje(i)
            End If
        Next
        If limpio.Length > 160 Then
            limpio = limpio.Substring(0, 160)
        End If
        Return limpio
    End Function

    Private Function limpiarCaracteresRaros(ByVal mensaje As String)
        mensaje = mensaje.Replace("ñ", "n")
        mensaje = mensaje.Replace("Ñ", "N")
        Dim limpio As String = ""
        Dim caracteresRaros As String = "|""¡]¨[|'¿}´{¬\`~^"

        For i As Integer = 0 To mensaje.Length - 1
            If Not caracteresRaros.Contains(mensaje(i)) Then
                limpio = limpio + mensaje(i)
            End If
        Next
        If limpio.Length > 160 Then
            limpio = limpio.Substring(0, 160)
        End If
        Return limpio
    End Function

    Private Function LlamarWS(ByVal UrlDelServicio As String, ByVal RequestXML As String) As String
        System.Net.ServicePointManager.Expect100Continue = False

        Dim webRequest As HttpWebRequest = CType(Net.WebRequest.Create(UrlDelServicio), HttpWebRequest)
        webRequest.ContentType = "text/xml;charset=""utf-8"""
        webRequest.Accept = "text/xml"
        webRequest.Method = "POST"
        If RequestXML <> "" Then
            Dim soapEnvelopeXml As System.Xml.XmlDocument = New System.Xml.XmlDocument()
            soapEnvelopeXml.LoadXml(RequestXML)
            Using stream As System.IO.Stream = webRequest.GetRequestStream()
                soapEnvelopeXml.Save(stream)
            End Using
        Else
            webRequest.ContentLength = 0
        End If

        Dim asyncResult As IAsyncResult = webRequest.BeginGetResponse(Nothing, Nothing)
        asyncResult.AsyncWaitHandle.WaitOne()

        Dim soapResult As String
        Using webResponse As Net.WebResponse = webRequest.EndGetResponse(asyncResult)
            Using rd As System.IO.StreamReader = New System.IO.StreamReader(webResponse.GetResponseStream())
                soapResult = rd.ReadToEnd()
            End Using
        End Using

        Return soapResult
    End Function

    Public Function GuardarEnviodeSMS(ByVal sms As SMS,
                                      ByVal EnvioInmediato As Boolean,
                                      ByVal Categoria As Integer,
                                      ByVal Test As Boolean) As String

        Dim IdSMS As Integer
        Try
            If String.IsNullOrEmpty(sms.Numero) Then
                Throw New Exception("Falta el numero teléfonico")
            End If
            If String.IsNullOrEmpty(sms.Mensaje) Then
                Throw New Exception("Falta el mensaje a enviar")
            End If

            If sms.Numero.Length > 20 Then
                Throw New Exception("El número de telefono no puede superar los 20 caracteres de longitud")
            End If
            If sms.Mensaje.Length > 8000 Then
                Throw New Exception("El mensaje no puede superar los 8000 caracteres de longitud")
            End If

            Dim StrSql As String = "INSERT INTO EnvioSMS " &
                                           "(Fecha " &
                                           ",Numero " &
                                           ",Mensaje " &
                                           ",EnvioInmediato " &
                                           ",Test " &
                                           ",CategoriaSMS) " &
                                           " OUTPUT INSERTED.Id " &
                                     "VALUES " &
                                           "(@Fecha " &
                                           ",@Numero " &
                                           ",@Mensaje " &
                                           ",@EnvioInmediato " &
                                           ",@Test " &
                                           ",@CategoriaSMS); " &
                                            "SELECT SCOPE_IDENTITY();"

            Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                     New SqlParameter("@Fecha", SqlDbType.DateTime) With {.Value = DateTime.Now},
                     New SqlParameter("@Numero", SqlDbType.NVarChar, 20) With {.Value = sms.Numero},
                     New SqlParameter("@Mensaje", SqlDbType.NVarChar, 8000) With {.Value = sms.Mensaje},
                     New SqlParameter("@EnvioInmediato", SqlDbType.Bit) With {.Value = EnvioInmediato},
                     New SqlParameter("@Test", SqlDbType.Bit) With {.Value = Test},
                     New SqlParameter("@CategoriaSMS", SqlDbType.Int) With {.Value = Categoria}
                }

            IdSMS = SqlHelper.ExecuteScalar(CnStr, CommandType.Text, StrSql, Parametros)
            If (IdSMS > 0) Then
                If (EnvioInmediato = 0) Then
                    Return ""
                Else
                    Return ProcesarEnvioSMS(IdSMS)
                End If
            Else
                Return "No fue posible el envío del SMS"
            End If

        Catch ex As Exception
            Return "No fue posible el envío del SMS: " & ex.Message
        End Try
    End Function

    Public Function ProcesarEnvioSMS(Optional ByVal IdSMS As Integer = 0) As String

        Try
            Dim DtSMS As New DataTable()
            Dim Procesados As Integer = 0
            Dim MensajeError As String
            Dim EsErrorDeNumero As Boolean
            Dim ErrorId As Integer
            Dim Respuesta As String = ""

            If (IdSMS <> 0) Then
                DtSMS = ObtenerSMS(IdSMS)
            Else
                DtSMS = ObtenerSMS()
            End If

            If (DtSMS.Rows.Count <> 0) Then
                For Each row As DataRow In DtSMS.Rows
                    MensajeError = ""
                    EsErrorDeNumero = False
                    If (Not Convert.ToBoolean(row("Test"))) Then
                        If (EnviarSMS(row("Numero").ToString(), row("Mensaje").ToString(), True, True, "", MensajeError, EsErrorDeNumero)) Then
                            SMSProcesados(0, Convert.ToInt32(row("Id")))
                            Procesados += 1
                        Else
                            If (IsDBNull(row("ErrorId")) Or Convert.ToInt32(row("ErrorId")) = 0) Then
                                ErrorId = ErrorSendInsert(MensajeError, EsErrorDeNumero, 0)
                                SMSProcesados(ErrorId, Convert.ToInt32(row("Id")))
                                Respuesta += "Error: " & (row("Numero").ToString()) & " " & MensajeError & vbCrLf
                            Else
                                ErrorId = ErrorSendInsert(MensajeError, EsErrorDeNumero, Convert.ToInt32(row("ErrorId")))
                                Respuesta += "Error: " & (row("Numero").ToString()) & " " & MensajeError & vbCrLf
                            End If
                        End If

                    End If
                Next
                Return Respuesta
            Else
                Return ""
            End If
        Catch ex As Exception
            Return "No se pudieron procesar los SMS: " + ex.Message
        End Try

    End Function


    Private Function ObtenerSMS(Optional ByVal IdSMS As Integer = 0) As DataTable

        Dim dt As New DataTable()

        Try
            Dim StrSql As String = "SELECT es.*, ess.* " &
                                   "FROM EnvioSMS es " &
                                   "LEFT JOIN ErrorSendSMS ess " &
                                   "ON es.ErrorId = ess.Id " &
                                   "WHERE  "
            If (IdSMS <> 0) Then
                StrSql += " es.Id = @IdInmediato "
            Else
                StrSql += "es.EnvioInmediato = 0 "
                StrSql += "And es.FechaEnvio Is NULL "
                StrSql += "And ISNULL(ess.NoReintento,0) = 0 "
                StrSql += "And ( ISNULL(ess.Intentos,0) < 10 Or ( ISNULL(ess.Intentos,0) < 15 And ess.Fecha <= DATEADD(HOUR, -1,GETDATE()) ) ) "
            End If

            Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                    New SqlParameter("@IdInmediato", SqlDbType.Int) With {.Value = IdSMS}
                }

            dt = SqlHelper.ExecuteDataset(CnStr, CommandType.Text, StrSql, Parametros).Tables(0)

            Return dt
        Catch ex As Exception
            dt.Clear()
            Return dt
        End Try
    End Function

    Private Sub SMSProcesados(ByVal ErrorId As Integer, ByVal Id As Integer)

        Try


            Dim StrSql As String = ""
            Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                    New SqlParameter("@Id", SqlDbType.Int) With {.Value = Id},
                    New SqlParameter("@ErrorId", SqlDbType.Int) With {.Value = ErrorId},
                    New SqlParameter("@FechaEnvio", SqlDbType.DateTime) With {.Value = DateTime.Now}
                }

            If (ErrorId = 0) Then
                StrSql = "UPDATE EnvioSMS 
                      SET FechaEnvio = @FechaEnvio
                      WHERE Id = @Id"
            Else
                StrSql = "UPDATE EnvioSMS 
                      SET ErrorId = @ErrorId
                      WHERE Id = @Id"
            End If

            SqlHelper.ExecuteNonQuery(CnStr, CommandType.Text, StrSql, Parametros)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ErrorSendInsert(ByVal Motivo As String, ByVal NoReintento As Boolean, ByVal ErrorId As Integer) As Integer

        Dim Identity As Integer = 0

        Try

            If (ErrorId = 0) Then
                Dim StrSql As String = "INSERT INTO ErrorSendSMS " &
                                           "(Fecha " &
                                           ",Motivo " &
                                           ",Intentos " &
                                           ",FechaPrimerIntento " &
                                           ",NoReintento) " &
                                           " OUTPUT INSERTED.Id " &
                                     "VALUES " &
                                           "(@Fecha " &
                                           ",@Motivo " &
                                           ",1 " &
                                           ",GETDATE() " &
                                           ",@NoReintento); " &
                                           "SELECT SCOPE_IDENTITY();"

                Dim Parametros() As SqlParameter = New SqlParameter() _
                    {
                         New SqlParameter("@Fecha", SqlDbType.DateTime) With {.Value = DateTime.Now},
                         New SqlParameter("@Motivo", SqlDbType.NVarChar, 8000) With {.Value = Motivo},
                         New SqlParameter("@NoReintento", SqlDbType.Bit, 1) With {.Value = NoReintento}
                    }
                Identity = Convert.ToInt32(SqlHelper.ExecuteScalar(CnStr, CommandType.Text, StrSql, Parametros))
            Else
                Dim StrSql As String = "UPDATE ErrorSendSMS " &
                                           "SET Intentos = Intentos + 1, " &
                                           "Fecha = @Fecha, " &
                                           "Motivo = @Motivo, " &
                                           "NoReintento = @NoReintento " &
                                           "WHERE Id = @ErrorId "

                Dim Parametros() As SqlParameter = New SqlParameter() _
                    {
                         New SqlParameter("@Fecha", SqlDbType.DateTime) With {.Value = DateTime.Now},
                         New SqlParameter("@Motivo", SqlDbType.NVarChar, 8000) With {.Value = Motivo},
                         New SqlParameter("@NoReintento", SqlDbType.Bit, 1) With {.Value = NoReintento},
                         New SqlParameter("@ErrorId", SqlDbType.VarChar, 50) With {.Value = ErrorId}
                    }
                SqlHelper.ExecuteNonQuery(CnStr, CommandType.Text, StrSql, Parametros)
            End If

            Return Identity
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Public Enum TipoDeError
        SinError = 0
        ErrorControlado = 1
        Excepcion = 2
    End Enum


    Private Function EnviarSMS_Telefonica(ByVal telefono As String,
                                          ByVal mensaje As String,
                                          ByVal controlaErroresNumero As Boolean,
                                          ByVal enviaMailError As Boolean,
                                          ByVal emailsAEnviarError As String) As String
        telefono = limpiarNumero(telefono)
        mensaje = limpiarCaracteresPeligrosos(mensaje)
        If telefono.Trim = "" Or mensaje.Trim = "" Then
            Return ""
        End If
        Dim mensajeError As String = ""
        Dim telefonoLong As Long
        If Not Long.TryParse(telefono, telefonoLong) Then
            If controlaErroresNumero Then
                mensajeError = "el número es erróneo (deben ser 10 dígitos)"
            End If
        ElseIf telefonoLong.ToString.Length <> 10 Then
            If controlaErroresNumero Then
                mensajeError = "el número es erróneo (deben ser 10 dígitos)"
            End If
        Else
            Dim tipoError As TipoDeError = TipoDeError.SinError
            Dim mensajeLog As String = ""
            Try
                Dim xml As String = "<?xml version=""1.0"" encoding=""ISO-8859-1""?><MTRequest><Proveedor Id=""" & Telefonica_Usuario & """ Password=""" & Telefonica_Clave & """ /><Servicio Id="""" ContentType=""0"" ShortNumber=""" & Telefonica_NumeroCorto & """  /><Telefono msisdn=""" + telefono + """ IdTran=""""/><Contenido>" + mensaje + "</Contenido></MTRequest>"
                Dim request As String = LlamarWS(Telefonica_Url, xml)
                mensajeLog = request
                If request.Contains("<Texto>OK</Texto>") = False Then
                    tipoError = TipoDeError.ErrorControlado
                    mensajeError = request
                End If
            Catch ex As WebException
                mensajeLog = ex.Message()
                Try
                    Dim webEx As Net.HttpWebResponse = DirectCast(ex.Response, Net.HttpWebResponse)
                    If webEx.StatusCode <> HttpStatusCode.Unauthorized Or controlaErroresNumero Then
                        mensajeError = mensajeLog ' Error de numero fijo
                    End If
                Catch ex2 As Exception
                    mensajeError = mensajeLog
                End Try
                tipoError = TipoDeError.Excepcion
            End Try
            'GuardarLogDeEnviodeSMS(telefono, mensaje, "Telefonica", tipoError, mensajeLog)
        End If
        If enviaMailError And mensajeError <> "" Then
            Dim cuerpoMensaje As String = "Numero: " & telefono & vbCrLf
            cuerpoMensaje += "Mensaje: " & mensaje & vbCrLf
            cuerpoMensaje += "Error: " & mensajeError
            Funciones.Funciones.EnviarMailError(cuerpoMensaje, "", "", 0, "Error en el envio de mensaje de texto (Telefónica)", Funciones.Funciones.DestinoMailDeError.Sistemas, emailsAEnviarError)
        End If
        Return mensajeError
    End Function

    Private Function EnviarSMS_SMSMasivos(ByVal telefono As String,
                                          ByVal mensaje As String,
                                          ByVal controlaErroresNumero As Boolean,
                                          ByVal enviaMailError As Boolean,
                                          ByVal emailsAEnviarError As String,
                                          ByVal esTest As Boolean) As String
        If Not esTest Then
            telefono = limpiarNumero(telefono)
            mensaje = limpiarCaracteresRaros(mensaje)
            If telefono.Trim = "" Or mensaje.Trim = "" Then
                Return ""
            End If
        End If

        Dim mensajeError As String = ""
        Dim telefonoLong As Long
        If Not Long.TryParse(telefono, telefonoLong) Then
            mensajeError = "el número es erróneo (deben ser 10 dígitos)"
        ElseIf telefonoLong.ToString.Length <> 10 Then
            mensajeError = "el número es erróneo (deben ser 10 dígitos)"
        Else
            Dim tipoError As TipoDeError = TipoDeError.SinError
            Dim mensajeLog As String = ""
            Try
                Dim ws As New WSSMSMasivos.SMSMasivosAPI
                mensajeLog = ws.EnviarSMS(SmsMasivos_Usuario, SmsMasivos_Clave, telefonoLong, mensaje, esTest)
                If esTest Then
                    If mensajeLog <> telefonoLong.ToString & ": probando sin enviar. " & mensaje Then
                        mensajeError = mensajeLog
                        tipoError = TipoDeError.ErrorControlado
                    End If
                Else
                    If mensajeLog <> "OK" Then
                        mensajeError = mensajeLog
                        tipoError = TipoDeError.ErrorControlado
                    End If
                End If
            Catch ex As Exception
                mensajeLog = ex.Message
                mensajeError = mensajeLog
                tipoError = TipoDeError.Excepcion
            End Try
            ' GuardarLogDeEnviodeSMS(telefono, mensaje, "SmsMasivos", tipoError, mensajeLog)
        End If
        If Not controlaErroresNumero Then
            If mensajeError.Contains("el número") Then
                mensajeError = ""
            End If
        End If
        If enviaMailError And mensajeError <> "" Then
            Dim cuerpoMensaje As String = "Numero: " & telefono & vbCrLf
            cuerpoMensaje += "Mensaje: " & mensaje & vbCrLf
            cuerpoMensaje += "Error: " & mensajeError
            Funciones.Funciones.EnviarMailError(cuerpoMensaje, "", "", 0, "Error en " & IIf(esTest, "la verificacion de numero de celular", "el envio de mensaje de texto") & " (SmsMasivos)", Funciones.Funciones.DestinoMailDeError.Sistemas, emailsAEnviarError)
        End If
        Return mensajeError
    End Function

    Private Function EnviarSMS_infobip(ByVal telefono As String,
                                       ByVal mensaje As String,
                                       ByVal controlaErroresNumero As Boolean,
                                       ByVal enviaMailError As Boolean,
                                       ByVal emailsAEnviarError As String,
                                       ByRef mensajeRespuesta As String,
                                       ByRef esErrorDeNumero As Boolean) As Boolean
        mensajeRespuesta = ""
        esErrorDeNumero = False

        telefono = limpiarNumero(telefono)
        mensaje = limpiarCaracteresRaros(mensaje)
        If telefono.Trim = "" Or mensaje.Trim = "" Then
            Return True
        End If

        Dim mensajeError As String = ""
        Dim telefonoLong As Long
        If Not Long.TryParse(telefono, telefonoLong) Then
            mensajeError = "el número es erróneo (deben ser 10 dígitos)"
        ElseIf telefonoLong.ToString.Length <> 10 Then
            mensajeError = "el número es erróneo (deben ser 10 dígitos)"
        Else
            Dim tipoError As TipoDeError = TipoDeError.SinError
            Dim mensajeLog As String = ""

            Try
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12

                Dim Credenciales As String = infobip_Usuario & ":" & infobip_Clave
                Dim Data As New Dictionary(Of String, Object)
                Data.Add("from", infobip_From)
                Data.Add("to", "54" & telefono)
                Data.Add("text", mensaje)
                Dim js As New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim DataJson As String = js.Serialize(Data)
                Dim DataByte As Byte() = System.Text.UTF8Encoding.UTF8.GetBytes(DataJson)

                Dim request As System.Net.HttpWebRequest
                request = System.Net.WebRequest.Create(infobip_Url)
                request.Timeout = 10 * 1000
                request.Method = "POST"
                request.ContentLength = DataByte.Length
                request.ContentType = "application/json; charset=utf-8"
                request.Headers.Add("Authorization", "Basic " + System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(Credenciales)))

                Dim postStream As IO.Stream = request.GetRequestStream()
                postStream.Write(DataByte, 0, DataByte.Length)
                Dim response As System.Net.HttpWebResponse = request.GetResponse()
                Dim reader As New IO.StreamReader(response.GetResponseStream())
                Dim RespuestaJson As String = reader.ReadToEnd()
                mensajeLog = RespuestaJson
                RespuestaJson = RespuestaJson.Replace("""to"":", """to_"":")

                Dim Serializador As New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim Respuesta As infobip_Respuesta = Serializador.Deserialize(Of infobip_Respuesta)(RespuestaJson)

                tipoError = TipoDeError.ErrorControlado
                If Not Respuesta Is Nothing Then
                    If Not Respuesta.messages Is Nothing Then
                        If Respuesta.messages.Length > 0 Then
                            If Not Respuesta.messages(0).status Is Nothing Then
                                If Not Respuesta.messages(0).status.groupId = Nothing Then
                                    If Respuesta.messages(0).status.groupId = 1 Or Respuesta.messages(0).status.groupId = 3 Then
                                        tipoError = TipoDeError.SinError
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                If tipoError = TipoDeError.ErrorControlado Then
                    mensajeError = mensajeLog
                End If
            Catch ex As Exception
                mensajeLog = ex.Message
                mensajeError = mensajeLog
                tipoError = TipoDeError.Excepcion
            End Try
        End If
        If mensajeError.Contains("el número") Then
            esErrorDeNumero = True
        End If
        If Not controlaErroresNumero AndAlso esErrorDeNumero Then
            mensajeError = ""
        End If
        If enviaMailError And mensajeError <> "" And Not esErrorDeNumero Then
            Dim cuerpoMensaje As String = "Numero: " & telefono & vbCrLf
            cuerpoMensaje += "Mensaje: " & mensaje & vbCrLf
            cuerpoMensaje += "Error: " & mensajeError
            Funciones.Funciones.EnviarMailError(cuerpoMensaje, "", "", 0, "Error en el envio de mensaje de texto (infobip)", Funciones.Funciones.DestinoMailDeError.Sistemas, emailsAEnviarError)
        End If

        mensajeRespuesta = mensajeError
        Return (mensajeError = "")
    End Function

    Public Function EnviarSMS(ByVal telefono As String,
                              ByVal mensaje As String,
                              ByVal controlaErroresNumero As Boolean,
                              ByVal enviaMailError As Boolean,
                              ByVal emailsAEnviarError As String,
                              ByRef mensajeRespuesta As String,
                              ByRef esErrorDeNumero As Boolean) As Boolean
        'Return EnviarSMS_Telefonica(telefono, mensaje, controlaErroresNumero, enviaMailError, emailsAEnviarError)
        'Return EnviarSMS_SMSMasivos(telefono, mensaje, controlaErroresNumero, enviaMailError, emailsAEnviarError, False)
        Return EnviarSMS_infobip(telefono, mensaje, controlaErroresNumero, enviaMailError, emailsAEnviarError, mensajeRespuesta, esErrorDeNumero)
    End Function

    Public Function VerificarTelefono(ByVal Telefono As String) As String
        Telefono = limpiarNumero(Telefono)
        If Telefono.Trim = "" Then
            Return "Numero en blanco"
        End If

        Dim mensaje As String = DateTime.Now.ToString("yyyyMMddHHmmssffff")
        Dim mensajeError As String = EnviarSMS_SMSMasivos(Telefono, mensaje, True, False, "", True)
        Return mensajeError
    End Function

End Module

Public Module EnvioDeWhatsApp

    Private infobip_Url As String = "https://9rlpx4.api.infobip.com/whatsapp/1/message/text"
    Private infobip_Usuario As String = "100Cruz!"
    Private infobip_Clave As String = "Infobip2019!"
    Private infobip_From As String = "CDS"
    Public Function EnviarWhatsApp(ByVal telefono As Integer, ByVal mensaje As String) As String


        Dim mensajeRespuesta As String = ""
        Dim MensajeError As String = ""

        Dim tipoError As TipoDeError = TipoDeError.SinError
        Dim mensajeLog As String = ""

        Try
            Dim Credenciales As String = infobip_Usuario & ":" & infobip_Clave
            Dim DataText As New Dictionary(Of String, Object)

            Dim Data As New Dictionary(Of String, Object)
            Data.Add("from", infobip_From)
            Data.Add("to", "54" & telefono)
            Data.Add("messageId", "a28dd97c-1ffb-4fcf-99f1-0b557ed381da")
            DataText.Add("text", mensaje)
            Data.Add("content", DataText)
            Data.Add("callbackData", "Callback data")

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
                MensajeError = mensajeLog
            End If
            Return Respuesta.ToString()
        Catch ex As Exception
            Return mensajeLog = ex.Message
            MensajeError = mensajeLog
            tipoError = TipoDeError.Excepcion
        End Try











        '        Dim client = New RestClient("https://{baseUrl}/whatsapp/1/message/text")
        '    client.Timeout = -1;
        'Dim request = New HttpWebRequest(Method.POST)
        '    request.AddHeader("Authorization", "{authorization}")
        '    request.AddHeader("Content-Type", "application/json")
        '    request.AddHeader("Accept", "application/json")
        '    request.AddParameter("application/json", "{\"from\":\"441134960000\",\"To\":\"441134960001\",\"messageId\":\"a28dd97c-1Ffb-4Fcf-99F1-0b557ed381da\",\"content\":{\"text\":\"Some text\"},\"callbackData\":\"Callback data\"}", ParameterType.RequestBody)
        '    IRestResponse response = client.Execute(request)
        'Console.WriteLine(response.Content)
    End Function
End Module

Imports System.Data.SqlClient
Imports System.IO
Imports Microsoft.ApplicationBlocks.Data
Imports Microsoft.VisualBasic.FileIO


Public Module EnvioDeMail


    Public CnStr As String = ClSeguridad.StringDeConexion("EnvioDeNotificaciones", "")
    Public ClaveEncriptacion As String = ")3B$e!/9"
    Public IdDelSistema As Integer = 23
    Public TiempoDeEsperaSQL As Integer = 60

    Public Function ProcesarEnvioCorreos(Optional ByVal IdMail As Integer = 0) As String
        Dim DtMails As New DataTable()
        Dim Procesados As Integer = 0
        Dim Ruta As String = ObtenerRutaFinalAdjuntos()
        Dim Respuesta As String = ""
        Try
            If (IdMail <> 0) Then
                DtMails = ObtenerMails(IdMail)
            Else
                DtMails = ObtenerMails()
            End If

            If (DtMails.Rows.Count <> 0) Then

                For Each row As DataRow In DtMails.Rows
                    Dim Paso As String = ""
                    Try
                        Paso = "Creo objetos"
                        Dim Correo As New System.Net.Mail.MailMessage
                        Dim adjunto As System.Net.Mail.Attachment
                        Dim DtDirecciones As New DataTable()
                        Dim DtAdjuntos As New DataTable()
                        Dim EliminarAdjuntos As Boolean = False
                        Paso = "Obtengo direcciones"
                        DtDirecciones = ObtenerDirecciones(Convert.ToInt32(row("Id")))
                        Paso = "Obtengo adjuntos"
                        DtAdjuntos = ObtenerAdjuntos(Convert.ToInt32(row("Id")))
                        If (DtAdjuntos.Rows.Count > 0) Then
                            Paso = "Proceso adjuntos"
                            For Each AdjuntoRow As DataRow In DtAdjuntos.Rows
                                Paso = "Proceso adjunto " & AdjuntoRow("NombreArchivo").ToString()
                                Try
                                    adjunto = New System.Net.Mail.Attachment(Ruta + row("Id").ToString() + "\" + AdjuntoRow("NombreArchivo").ToString())
                                    Correo.Attachments.Add(adjunto)
                                    EliminarAdjuntos = True
                                Catch ex As Exception
                                    EliminarAdjuntos = False
                                End Try
                            Next
                        End If
                        Paso = "Proceso direcciones"
                        For Each rowDir As DataRow In DtDirecciones.Rows
                            Paso = "Proceso direccion " & rowDir("Direccion").ToString()
                            If (rowDir("Tipo") = "0") Then
                                Correo.To.Add(New System.Net.Mail.MailAddress(rowDir("Direccion"), rowDir("Nombre")))
                            ElseIf (rowDir("Tipo") = "1") Then
                                Correo.CC.Add(New System.Net.Mail.MailAddress(rowDir("Direccion"), rowDir("Nombre")))
                            ElseIf (rowDir("Tipo") = "2") Then
                                Correo.Bcc.Add(New System.Net.Mail.MailAddress(rowDir("Direccion"), rowDir("Nombre")))
                            ElseIf (rowDir("Tipo") = "3") Then
                                Correo.ReplyToList.Add(New System.Net.Mail.MailAddress(rowDir("Direccion"), rowDir("Nombre")))
                            End If
                        Next

                        Paso = "Seteo parametros del mail"
                        Correo.Subject = GSCF.SacarRarosParaAsuntoDeMail(row("Asunto"))
                        Correo.Body = row("Mensaje").ToString()
                        If (Convert.ToBoolean(row("PrioridadAlta"))) Then
                            Correo.Priority = Net.Mail.MailPriority.High
                        Else
                            Correo.Priority = Net.Mail.MailPriority.Normal
                        End If
                        If (Convert.ToBoolean(row("BodyHTML"))) Then
                            Correo.IsBodyHtml = True
                        Else
                            Correo.IsBodyHtml = False
                        End If
                        If (Convert.ToBoolean(row("NotificaFallaEntrega"))) Then
                            Correo.DeliveryNotificationOptions = Net.Mail.DeliveryNotificationOptions.OnFailure
                        End If

                        Paso = "Creo remitente"
                        Correo.From = New System.Net.Mail.MailAddress(row("CorreoRemitente").ToString(), row("NombreRemitente").ToString())

                        If (Not Convert.ToBoolean(row("Test"))) Then
                            Paso = "Hago el envio"
                            Using SmtCli As System.Net.Mail.SmtpClient = GSCF.CrearServidorSMTP()
                                SmtCli.Send(Correo)
                            End Using
                        End If

                        Paso = "Guardo procesamiento"
                        Procesados += 1
                        EnvioDeMail.MailsProcesados(0, Convert.ToInt32(row("Id")))

                        Correo.Dispose()

                        If (EliminarAdjuntos) Then
                            Paso = "Borro adjuntos"
                            EnvioDeMail.EliminarAdjuntos(Convert.ToInt32(row("Id")))
                        End If
                    Catch ex As Exception
                        Respuesta += "Error: " & row("Id").ToString() & " (" & Paso & ") " & ex.Message().Replace(vbCrLf, ". ") & vbCrLf
                        Dim ErrorId As Integer = 0

                        If (IsDBNull(row("ErrorId")) Or Convert.ToInt32(row("ErrorId")) = 0) Then
                            ErrorId = EnvioDeMail.ErrorMailInsert(ex.Message(), False, 0)
                            EnvioDeMail.MailsProcesados(ErrorId, Convert.ToInt32(row("Id")))
                        Else
                            ErrorId = EnvioDeMail.ErrorMailInsert(ex.Message(), False, Convert.ToInt32(row("ErrorId")))
                        End If
                    End Try

                Next

                Return Respuesta
            Else
                Return ""
            End If

        Catch ex As Exception
            Return "No fue posible procesar los correos: " + ex.Message
        End Try
    End Function
    Public Function ObtenerMensaje(Optional ByVal Id As Integer = 0) As DataTable
        Dim dt As New DataTable()
        Try
            Dim StrSql As String = "SELECT Mensaje FROM EnvioMail WHERE Id = @Id "



            Dim Parametros() As SqlParameter = New SqlParameter() _
                    {
                         New SqlParameter("@Id", SqlDbType.Int) With {.Value = Id}
                    }

            dt = SqlHelper.ExecuteDataset(CnStr, CommandType.Text, StrSql, Parametros).Tables(0)

            Return dt
        Catch ex As Exception
            dt.Clear()
            Return dt
        End Try
    End Function
    Private Function ObtenerMails(Optional ByVal IdInmediato As Integer = 0) As DataTable
        Dim dt As New DataTable()
        Try
            Dim StrSql As String = "SELECT em.*, esm.*, dm.Direccion As CorreoRemitente, dm.Nombre As NombreRemitente" &
                                   "FROM EnvioMail em " &
                                   "INNER JOIN DireccionesMail dm " &
                                   "ON em.IdRemitente = dm.IdDireccion " &
                                   "LEFT JOIN ErrorSendMail esm " &
                                   "ON em.ErrorId = esm.Id " &
                                   "WHERE "

            If (IdInmediato <> 0) Then
                StrSql += "em.Id = @IdInmediato "
            Else
                StrSql += "em.EnvioInmediato = 0 "
                StrSql += "AND em.FechaEnvio Is NULL "
                StrSql += "AND ISNULL(esm.NoReintento,0) = 0 "
                StrSql += "AND ( ISNULL(esm.Intentos,0) < 10 OR ( ISNULL(esm.Intentos,0) < 15 AND esm.Fecha <= DATEADD(HOUR, -1,GETDATE()) ) ) "
            End If

            Dim Parametros() As SqlParameter = New SqlParameter() _
                    {
                         New SqlParameter("@IdInmediato", SqlDbType.Int) With {.Value = IdInmediato}
                    }

            dt = SqlHelper.ExecuteDataset(CnStr, CommandType.Text, StrSql, Parametros).Tables(0)

            Return dt
        Catch ex As Exception
            dt.Clear()
            Return dt
        End Try
    End Function

    Private Function ErrorMailInsert(ByVal Motivo As String, ByVal NoReintento As Boolean, ByVal ErrorId As Integer) As Integer
        Dim Identity As Integer
        Try
            Dim StrSql As String = ""

            If (ErrorId = 0) Then
                StrSql = "INSERT INTO ErrorSendMail " &
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
                         New SqlParameter("@Motivo", SqlDbType.NVarChar, 8000) With {.Value = Strings.Left(Motivo, 8000)},
                         New SqlParameter("@NoReintento", SqlDbType.Bit, 1) With {.Value = NoReintento}
                    }
                Identity = Convert.ToInt32(SqlHelper.ExecuteScalar(CnStr, CommandType.Text, StrSql, Parametros))
            Else
                StrSql = "UPDATE ErrorSendMail " &
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
            Return Identity = 0
        End Try
    End Function

    Private Sub MailsProcesados(ByVal ErrorId As Integer, ByVal Id As Integer)

        Try
            Dim StrSql As String = ""
            Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                    New SqlParameter("@Id", SqlDbType.Int) With {.Value = Id},
                    New SqlParameter("@ErrorId", SqlDbType.Int) With {.Value = ErrorId},
                    New SqlParameter("@FechaEnvio", SqlDbType.DateTime) With {.Value = DateTime.Now}
                }

            If (ErrorId = 0) Then
                StrSql = "UPDATE EnvioMail 
                      SET FechaEnvio = @FechaEnvio
                      WHERE Id = @Id"
            Else
                StrSql = "UPDATE EnvioMail 
                      SET ErrorId = @ErrorId
                      WHERE Id = @Id"
            End If

            SqlHelper.ExecuteNonQuery(CnStr, CommandType.Text, StrSql, Parametros)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GuardarEnviodeMails(ByVal Remitente As DireccionDeMail,
                                        ByVal Destinatario As List(Of DireccionDeMail),
                                        ByVal CC As List(Of DireccionDeMail),
                                        ByVal CCO As List(Of DireccionDeMail),
                                        ByVal ResponderA As List(Of DireccionDeMail),
                                        ByVal Mensaje As String,
                                        ByVal Asunto As String,
                                        ByVal Adjuntos As List(Of ArchivoAdjunto),
                                        ByVal BodyHTML As Boolean,
                                        ByVal PrioridadAlta As Boolean,
                                        ByVal NotificaFallaEntrega As Boolean,
                                        ByVal EnvioInmediato As Boolean,
                                        ByVal Categoria As Integer,
                                        ByVal Test As Boolean) As String
        Dim ListaDirecciones As New List(Of DireccionDeMailDB)
        Dim IdMail As Integer
        Try
            If Adjuntos Is Nothing Then
                Adjuntos = New List(Of ArchivoAdjunto)
            End If

            If Remitente Is Nothing OrElse Remitente.Direccion Is Nothing OrElse Not Remitente.Direccion.Contains("@") Then
                Throw New Exception("Falta remitente")
            End If
            If Remitente.Nombre Is Nothing Then
                Remitente.Nombre = ""
            End If

            Dim Cantidad0 As Integer = ValidarDirecciones(ListaDirecciones, Destinatario, 0)
            Dim Cantidad1 As Integer = ValidarDirecciones(ListaDirecciones, CC, 1)
            Dim Cantidad2 As Integer = ValidarDirecciones(ListaDirecciones, CCO, 2)
            Dim Cantidad3 As Integer = ValidarDirecciones(ListaDirecciones, ResponderA, 3)

            If Cantidad0 + Cantidad1 + Cantidad2 = 0 Then
                Return "No hay ningun destinatario"
            End If

            Dim NombresAdjuntosFinales As New List(Of String)

            For Each Adjunto As ArchivoAdjunto In Adjuntos
                If Adjunto.NombreFinal = String.Empty Then
                    Throw New Exception("Falta nombre final de adjunto")
                ElseIf Adjunto.RutaCompletaTemporal = String.Empty AndAlso (Adjunto.ArchivoBytes Is Nothing OrElse Adjunto.ArchivoBytes.Length = 0) Then
                    Throw New Exception("Falta contenido de adjunto")
                ElseIf Not Adjunto.RutaCompletaTemporal = String.Empty AndAlso Not File.Exists(Adjunto.RutaCompletaTemporal) Then
                    Throw New Exception("No existe un adjunto")
                Else
                    If NombresAdjuntosFinales.Contains(Adjunto.NombreFinal) Then
                        Throw New Exception("Nombre final de archivo adjunto repetido")
                    Else
                        NombresAdjuntosFinales.Add(Adjunto.NombreFinal)
                    End If
                End If
            Next

            Dim StrSql As String = "INSERT INTO EnvioMail " &
                                           "(Fecha " &
                                           ",Mensaje " &
                                           ",Asunto " &
                                           ",BodyHTML " &
                                           ",PrioridadAlta " &
                                           ",NotificaFallaEntrega " &
                                           ",EnvioInmediato " &
                                           ",Test " &
                                           ",CategoriaMail) " &
                                           " OUTPUT INSERTED.Id " &
                                     "VALUES " &
                                           "(@Fecha " &
                                           ",@Mensaje " &
                                           ",@Asunto " &
                                           ",@BodyHTML " &
                                           ",@PrioridadAlta " &
                                           ",@NotificaFallaEntrega " &
                                           ",@EnvioInmediato " &
                                           ",@Test " &
                                           ",@CategoriaMail); " &
                                           "SELECT SCOPE_IDENTITY();"

            Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                New SqlParameter("@Fecha", SqlDbType.DateTime) With {.Value = DateTime.Now},
                New SqlParameter("@Mensaje", SqlDbType.NVarChar) With {.Value = Mensaje},
                New SqlParameter("@Asunto", SqlDbType.NVarChar, 500) With {.Value = Asunto},
                New SqlParameter("@BodyHTML", SqlDbType.Bit) With {.Value = BodyHTML},
                New SqlParameter("@PrioridadAlta", SqlDbType.Bit) With {.Value = PrioridadAlta},
                New SqlParameter("@NotificaFallaEntrega", SqlDbType.Bit) With {.Value = NotificaFallaEntrega},
                New SqlParameter("@EnvioInmediato", SqlDbType.Bit) With {.Value = EnvioInmediato},
                New SqlParameter("@Test", SqlDbType.Bit) With {.Value = Test},
                New SqlParameter("@CategoriaMail", SqlDbType.Int) With {.Value = Categoria}
                }

            IdMail = SqlHelper.ExecuteScalar(CnStr, CommandType.Text, StrSql, Parametros)

            If (IdMail <> 0) Then
                GuardarRemitente(IdMail, Remitente)
                GuardarRelacion(IdMail, ListaDirecciones)
                For Each Adjunto As ArchivoAdjunto In Adjuntos
                    If (Adjunto.NombreFinal <> String.Empty) Then
                        GuardarArchivoAdjunto(IdMail, Adjunto)
                    End If
                Next
                If (EnvioInmediato) Then
                    Return ProcesarEnvioCorreos(IdMail)
                Else
                    Return ""
                End If
            Else
                Return "El correo no pudo ser enviado"
            End If

        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function

    Public Function ValidarDirecciones(ByRef ListId As List(Of DireccionDeMailDB), ByVal Lista As List(Of DireccionDeMail), ByVal Tipo As Integer) As Integer
        Dim Id As Integer
        Dim DireccionDB As DireccionDeMailDB
        Dim Cantidad As Integer = 0

        For Each Direccion As DireccionDeMail In Lista
            If Direccion.Nombre Is Nothing Then
                Direccion.Nombre = ""
            End If
            If Direccion.Direccion Is Nothing Then
                Direccion.Direccion = ""
            End If
            If Direccion.Direccion.Contains("@") Then
                Id = GuardarDireccion(Direccion.Nombre, Direccion.Direccion)
                If (Id <> 0) Then
                    DireccionDB = New DireccionDeMailDB
                    DireccionDB.Id = Id
                    DireccionDB.Tipo = Tipo
                    ListId.Add(DireccionDB)
                    Cantidad = Cantidad + 1
                End If
            End If
        Next

        Return Cantidad
    End Function

    Public Sub GuardarRemitente(ByVal IdMail As Integer, ByVal Remitente As DireccionDeMail)

        Dim Id As Integer = 0
        Dim Destino As New DireccionDeMailDB
        Try

            If (Remitente.Direccion.Contains("@")) Then
                Id = GuardarDireccion(Remitente.Nombre, Remitente.Direccion)
                If (Id <> 0) Then
                    Dim StrSql As String = "UPDATE EnvioMail " &
                                               "SET IdRemitente = @IdRemitente " &
                                               "WHERE Id = @IdMail"


                    Dim Parametros() As SqlParameter = New SqlParameter() _
                            {
                                 New SqlParameter("@IdRemitente", SqlDbType.Int) With {.Value = Id},
                                 New SqlParameter("@IdMail", SqlDbType.Int) With {.Value = IdMail}
                            }

                    SqlHelper.ExecuteNonQuery(CnStr, CommandType.Text, StrSql, Parametros)
                End If
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function GuardarDireccion(ByVal Nombre As String, ByVal Direccion As String) As Integer
        Dim StrSql As String = "InsertDireccion"

        Dim Parametros() As SqlParameter = New SqlParameter() _
                    {
                         New SqlParameter("@Nombre", SqlDbType.VarChar, 100) With {.Value = Nombre},
                         New SqlParameter("@Direccion", SqlDbType.VarChar, 100) With {.Value = Direccion.ToLower()}
                    }
        Dim IdMail As Integer = SqlHelper.ExecuteScalar(CnStr, CommandType.StoredProcedure, StrSql, Parametros)
        If IsDBNull(IdMail) OrElse IdMail = Nothing Then
            Throw New Exception("No se pudo insertar direccion")
        End If
        Return IdMail
    End Function

    Public Function ObtenerDirecciones(ByVal IdMail As Integer) As DataTable
        Dim DtDirecciones As New DataTable

        Try
            Dim StrSql As String = "SELECT dm.Nombre, dm.Direccion, md.Tipo " &
                                   "FROM DireccionesMail dm " &
                                   "INNER JOIN EnvioMailDireccion md " &
                                   "ON md.IdDireccion = dm.IdDireccion " &
                                   "WHERE md.IdMail = @IdMail "

            Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                New SqlParameter("@IdMail", SqlDbType.Int) With {.Value = IdMail}
                }

            DtDirecciones = SqlHelper.ExecuteDataset(CnStr, CommandType.Text, StrSql, Parametros).Tables(0)

            Return DtDirecciones
        Catch ex As Exception
            DtDirecciones.Clear()
            Return DtDirecciones
        End Try
    End Function

    Public Sub GuardarRelacion(ByVal IdMail As Integer, ByVal ListDirecciones As List(Of DireccionDeMailDB))
        Try
            For Each direccion As DireccionDeMailDB In ListDirecciones

                Dim StrSql As String = "INSERT INTO EnvioMailDireccion " &
                                          "(IdMail " &
                                          ",IdDireccion " &
                                          ",Tipo) " &
                                    "VALUES " &
                                          "(@IdMail " &
                                          ",@IdDireccion " &
                                          ",@Tipo); "

                Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                New SqlParameter("@IdMail", SqlDbType.Int) With {.Value = IdMail},
                New SqlParameter("@IdDireccion", SqlDbType.Int) With {.Value = direccion.Id},
                New SqlParameter("@Tipo", SqlDbType.Int) With {.Value = direccion.Tipo}
                }

                SqlHelper.ExecuteNonQuery(CnStr, CommandType.Text, StrSql, Parametros)
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GuardarRelacionAdjuntos(ByVal IdMail As Integer, ByVal Adjuntos As String)
        Try
            Dim StrSql As String = "INSERT INTO AdjuntosMail " &
                                          "(IdMail " &
                                          ",NombreArchivo) " &
                                    "VALUES " &
                                          "(@IdMail " &
                                          ",@adjunto); "

            Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                New SqlParameter("@IdMail", SqlDbType.Int) With {.Value = IdMail},
                New SqlParameter("@adjunto", SqlDbType.NVarChar, 100) With {.Value = Adjuntos}
                }

            SqlHelper.ExecuteNonQuery(CnStr, CommandType.Text, StrSql, Parametros)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ObtenerAdjuntos(ByVal IdMail As Integer) As DataTable
        Dim DtAdjuntos As New DataTable

        Try
            Dim StrSql As String = "SELECT Id, NombreArchivo " &
                                   "FROM AdjuntosMail " &
                                   "WHERE IdMail = @IdMail "

            Dim Parametros() As SqlParameter = New SqlParameter() _
                {
                New SqlParameter("@IdMail", SqlDbType.Int) With {.Value = IdMail}
                }

            DtAdjuntos = SqlHelper.ExecuteDataset(CnStr, CommandType.Text, StrSql, Parametros).Tables(0)

            Return DtAdjuntos
        Catch ex As Exception
            DtAdjuntos.Clear()
            Return DtAdjuntos
        End Try
    End Function

    Public Sub EliminarAdjuntos(ByVal IdMail As Integer)

        Try
            Dim RutaFinal As String = ObtenerRutaFinalAdjuntos()

            Directory.Delete(RutaFinal & IdMail.ToString & "\", True)

            Dim StrSql As String = "Update AdjuntosMail " &
                                 "SET Borrado = 1 " &
                                 "WHERE IdMail = @IdMail "

            Dim Parametros() As SqlParameter = New SqlParameter() _
                    {
                    New SqlParameter("@IdMail", SqlDbType.Int) With {.Value = IdMail}
                    }
            SqlHelper.ExecuteNonQuery(CnStr, CommandType.Text, StrSql, Parametros)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub GuardarArchivoAdjunto(ByVal IdMail As Integer, ByVal Adjunto As ArchivoAdjunto)
        Dim Ruta As String = ObtenerRutaFinalAdjuntos()
        FileSystem.CreateDirectory(Ruta + IdMail.ToString())
        Dim RutaDestino As String = $"{Ruta}{IdMail.ToString}\{Adjunto.NombreFinal}"

        Try
            If (Adjunto.RutaCompletaTemporal <> String.Empty) Then
                File.Copy(Adjunto.RutaCompletaTemporal, RutaDestino)
            Else
                File.WriteAllBytes(RutaDestino, Adjunto.ArchivoBytes)
            End If
            GuardarRelacionAdjuntos(IdMail, Adjunto.NombreFinal)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ObtenerRutaFinalAdjuntos() As String

        Dim dtRuta As New DataTable()
        Dim Ruta As String = ""
        Try
            Dim StrSql As String = "SELECT RutaCompartidaFinal " &
                                   "FROM DatosFijos "

            dtRuta = SqlHelper.ExecuteDataset(CnStr, CommandType.Text, StrSql).Tables(0)


            Ruta = dtRuta.Rows(0)(0).ToString()

            Return Ruta
        Catch ex As Exception
            Return Ruta
        End Try

    End Function

    Public Function ObtenerRutaTemporalAdjuntos() As String

        Dim dtRuta As New DataTable()
        Dim Ruta As String = ""
        Try
            Dim StrSql As String = "SELECT RutaCompartidaTemporal " &
                                   "FROM DatosFijos "

            dtRuta = SqlHelper.ExecuteDataset(CnStr, CommandType.Text, StrSql).Tables(0)

            Ruta = dtRuta.Rows(0)(0).ToString()

            Return Ruta
        Catch ex As Exception
            Return Ruta
        End Try

    End Function

    Public Class DireccionDeMail

        Private _Nombre As String

        Public Property Nombre() As String
            Get
                Return _Nombre
            End Get
            Set(ByVal value As String)
                If (value <> "?") Then
                    _Nombre = value
                End If
            End Set
        End Property

        Private _Direccion As String
        Public Property Direccion() As String
            Get
                Return _Direccion
            End Get
            Set(ByVal value As String)
                If (value <> "?") Then
                    _Direccion = value
                End If
            End Set
        End Property

    End Class

    Public Class DireccionDeMailDB

        Private _Id As Integer
        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal value As Integer)
                _Id = value
            End Set
        End Property
        Private _Tipo As Integer
        Public Property Tipo() As Integer
            Get
                Return _Tipo
            End Get
            Set(ByVal value As Integer)
                _Tipo = value
            End Set
        End Property
    End Class

    Public Class ArchivoAdjunto

        Private _NombreFinal As String
        Public Property NombreFinal() As String
            Get
                Return _NombreFinal
            End Get
            Set(ByVal value As String)
                If (value <> "?") Then
                    _NombreFinal = value
                End If

            End Set
        End Property

        Private _RutaCompletaTemporal As String
        Public Property RutaCompletaTemporal() As String
            Get
                Return _RutaCompletaTemporal
            End Get
            Set(ByVal value As String)
                If (value <> "?") Then
                    _RutaCompletaTemporal = value
                End If

            End Set
        End Property

        Private _ArchivoBytes As Byte()
        Public Property ArchivoBytes() As Byte()
            Get
                Return _ArchivoBytes
            End Get
            Set(ByVal value As Byte())
                _ArchivoBytes = value
            End Set
        End Property
    End Class
End Module

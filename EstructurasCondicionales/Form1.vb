Public Class Form1
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Declaracion de Variables
        Dim cant As Integer
        Dim punitario, pparcial, desc, pneto As Single
        Dim marcadesc As Boolean
        'Entrada de Datos'
        cant = Val(TxtCantidad.Text)
        punitario = Val(TxtUnitario.Text)
        marcadesc = chkdesc.Checked



        'Proceso

        pparcial = cant * punitario
        'Evaluar si aplicamos o no un descuento
        'Inicializar la variable descuento
        desc = 0
        If (marcadesc = True) Then
            desc = pparcial * 7 / 100
        End If

        pneto = pparcial - desc

        'salidad de informacion
        txtparcial.Text = pparcial
        txtDescuento.Text = desc
        txtPrecioNeto.Text = pneto

    End Sub

    Private Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        TxtCantidad.Clear()
        txtPrecioNeto.Clear()
        txtparcial.Clear()
        TxtUnitario.Clear()
        txtDescuento.Clear()
    End Sub
End Class

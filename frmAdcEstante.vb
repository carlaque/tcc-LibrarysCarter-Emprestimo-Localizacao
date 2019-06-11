﻿Public Class frmAdcEstante
    Dim vlocal As Boolean = False
    Dim vQuant As Boolean = False

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        frmEstante.aux = True
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        frmEstante.aux = True
        Me.Close()
    End Sub

    Private Sub txtLocal_TextChanged(sender As Object, e As EventArgs) Handles txtLocal.TextChanged
        Dim aux As Integer
        aux = Len(txtLocal.Text)

        If aux >= 2 Then
            vlocal = True
        Else
            vlocal = False
        End If
    End Sub

    Private Sub txtCapacidade_TextChanged(sender As Object, e As EventArgs) Handles txtCapacidade.TextChanged
        Dim aux As Integer
        aux = Len(txtCapacidade.Text)

        If aux >= 1 Then
            vQuant = True
        Else
            vQuant = False
        End If
    End Sub

    Private Sub btnOk_EnabledChanged(sender As Object, e As EventArgs) Handles btnOk.EnabledChanged, txtLocal.TextChanged, txtCapacidade.TextChanged
        If vQuant = True And vlocal = True Then
            btnOk.Enabled = True
        Else
            btnOk.Enabled = False
        End If
    End Sub

    Private Sub txtCapacidade_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCapacidade.KeyPress
        If Not (Char.IsDigit(e.KeyChar) OrElse Char.IsControl(e.KeyChar)) Then
            e.Handled = True
        End If
    End Sub
End Class
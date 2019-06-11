Public Class Escolher
    Private Sub btnEstante_Click(sender As Object, e As EventArgs) Handles btnEstante.Click
        Me.Close()
        frmEstante.ShowDialog()
    End Sub

    Private Sub btnPrateleiras_Click(sender As Object, e As EventArgs) Handles btnPrateleiras.Click
        Me.Close()
        frmPrateleiras.ShowDialog()
    End Sub

    Private Sub btnLivros_Click(sender As Object, e As EventArgs) Handles btnLivros.Click
        Me.Close()
        frmLivrosLocal.rdbNaoAlocados.Checked = True
        frmLivrosLocal.btnAlocar.Enabled = True
        frmLivrosLocal.ShowDialog()
    End Sub

    Private Sub lblSair_Click(sender As Object, e As EventArgs) Handles lblSair.Click
        Me.Close()
        frmMenu.Show()
    End Sub

End Class
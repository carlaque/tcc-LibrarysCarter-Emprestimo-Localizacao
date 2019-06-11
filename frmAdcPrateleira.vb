Imports System.Data.SqlClient
Public Class frmAdcPrateleira
    Dim con As SqlConnection = Biblioteca.Conexao

    Dim dtbEstante As New DataTable
    Dim dtbGenero As New DataTable
    Public genero As String

    Public posicao As Integer

    Public Sub combo()
        con.Open()
        'COMBO BOX Estante
        Dim adpEstante As New SqlDataAdapter("SELECT * FROM Estante", con)
        dtbEstante.Clear()
        adpEstante.Fill(dtbEstante)
        con.Close()
        ''propriedades
        cmbEstante.DataSource = dtbEstante
        cmbEstante.DisplayMember = "localizacao"
        cmbEstante.ValueMember = "codigo"

    End Sub

    Public Sub combosGenero()
        con.Open()
        'COMBO BOX Genero
        Dim adpEstante As New SqlDataAdapter("SELECT * FROM Genero", con)
        dtbGenero.Clear()
        adpEstante.Fill(dtbGenero)
        con.Close()

        ''propriedades
        cbxGenero.DataSource = dtbGenero
        cbxGenero.DisplayMember = "Nome"
        cbxGenero.ValueMember = "codigo"

    End Sub


    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        frmPrateleiras.disponibilidadeEstante = dtbEstante.Rows(cmbEstante.SelectedIndex).Item("disponibilidade")
        frmPrateleiras.localEstante = dtbEstante.Rows(cmbEstante.SelectedIndex).Item("localizacao")
        posicao = dtbEstante.Rows(cmbEstante.SelectedIndex).Item("alocadasP")
        genero = cbxGenero.Text
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        frmPrateleiras.aux = True
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Me.Close()
        frmPrateleiras.aux = True
    End Sub
End Class
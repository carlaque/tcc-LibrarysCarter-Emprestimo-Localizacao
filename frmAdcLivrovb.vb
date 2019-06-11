Imports System.Data.SqlClient
Public Class frmAdcLivro

    Dim con As SqlConnection = Biblioteca.Conexao

    Dim dtbEstante As New DataTable
    Dim dtbPrateleira As New DataTable

    Dim estante As Integer
    Public alocados As Integer
    Public capacidadePrateleira As Integer
    Public local As String

    Public Sub combos()
        con.Open()
        'COMBO BOX Estante
        Dim adpEstante As New SqlDataAdapter("SELECT * FROM Estante WHERE codigo in (select codEstante from prateleira)", con)
        dtbEstante.Clear()
        adpEstante.Fill(dtbEstante)
        con.Close()

        ''propriedades
        cmbEstante.DataSource = dtbEstante
        cmbEstante.DisplayMember = "localizacao"
        cmbEstante.ValueMember = "codigo"
        ' MsgBox(dtbEstante.Rows(cmbEstante.SelectedIndex).Item("codigo"))

    End Sub
    ''LEMBRAR DE ZERA VARIAVEIS QUADO SE FECH PARA ABRIR DNV 

    Private Sub comboPrateleira()
        If dtbEstante.Rows.Count > 0 Then
            If frmPrateleiras.estante > 0 Then
                estante = frmPrateleiras.estante
            ElseIf frmPrateleiras.estante = 0 Then
                estante = dtbEstante.Rows(cmbEstante.SelectedIndex).Item("codigo")
            End If
            con.Open()
            ''COMBO BOX PRATELEIRA
            Dim adpPrateleira As New SqlDataAdapter("SELECT p.codigo AS [codigo], g.nome AS [genero], p.posicao AS [posicao], p.capacidade AS [capacidade] FROM prateleira P INNER JOIN genero G ON g.codigo = p.codGenero WHERE codEstante = @estante AND disponibilidade = 'true' ", con)
            ''LEMBRAR DE SO APARECER PRATELEIRAS QUE NAO ESTAO CHEIAS
            adpPrateleira.SelectCommand.Parameters.Add("@estante", SqlDbType.Int).Value = estante
            dtbPrateleira.Clear()
            adpPrateleira.Fill(dtbPrateleira)

            con.Close()

            'MsgBox(cmbEstante.SelectedIndex + 1)
            cmbPrateleira.DataSource = dtbPrateleira
            cmbPrateleira.DisplayMember = "genero"
            cmbPrateleira.ValueMember = "codigo"
        End If


    End Sub
    Public Sub carregaPrateleira()
        comboPrateleira()
    End Sub


    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        local = dtbEstante.Rows(cmbEstante.SelectedIndex).Item("localizacao") & "." & dtbPrateleira.Rows(cmbPrateleira.SelectedIndex).Item("posicao") & "." & (dtbPrateleira.Rows(cmbPrateleira.SelectedIndex).Item("capacidade") + 1)
        capacidadePrateleira = (dtbPrateleira.Rows(cmbPrateleira.SelectedIndex).Item("capacidade"))

        'MsgBox(local) ESCREVER MENSAGEM INDICANDO DETALHES DA LOCALIZAÇÃO
        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Me.Close()
        frmLivrosLocal.aux = True
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub cmbEstante_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEstante.SelectedIndexChanged
        comboPrateleira()
    End Sub

    Private Sub chkHistorico_CheckedChanged(sender As Object, e As EventArgs) Handles chkHistorico.CheckedChanged

        If chkHistorico.Checked = True Then
            cmbEstante.Enabled = False
            cmbPrateleira.Enabled = False
        ElseIf chkHistorico.Checked = False Then
            cmbEstante.Enabled = True
            cmbPrateleira.Enabled = True
        End If

    End Sub


    Private Sub lblSair_Click(sender As Object, e As EventArgs) Handles lblSair.Click
        Me.Close()
        frmLivrosLocal.aux = True
        frmLivrosLocal.Show()
    End Sub

    Private Sub frmAdcLivro_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        estante = 0
        carregaPrateleira()
    End Sub
End Class
Imports System.Data.SqlClient
Public Class frmPrateleiras
    Dim con As SqlConnection = Biblioteca.Conexao

    Dim dtb As New DataTable
    Dim dtbEstante As New DataTable

    Public capacidade As Integer
    Public disponibilidadeEstante As Boolean

    Public prateleira As Integer
    Public estante As Integer
    Public localEstante As String

    Public aux As Boolean

    Public Sub carregacombo()
        AtualizarGrid()
    End Sub

    Private Sub AtualizarGrid()
        con.Open()
        If frmEstante.linha > 0 Then
            Dim adp As New SqlDataAdapter("SELECT p.codigo AS [Codigo], p.codgenero AS [codGenero], g.Nome [Genero da Prateleira], p.codEstante AS [Codigo da Estante], p.disponibilidade AS [Disponibilidade], e.localizacao AS [Estante] FROM Prateleira p INNER JOIN genero g on g.codigo = p.codGenero INNER JOIN estante e ON e.codigo = p.codEstante WHERE p.codEstante ='" & frmEstante.linha & "' ", con)
            dtb.Clear()
            adp.Fill(dtb)
        Else
            Dim adp As New SqlDataAdapter("SELECT p.codigo AS [Codigo], p.codgenero AS [codGenero], g.Nome [Genero da Prateleira], p.codEstante AS [Codigo da Estante], p.disponibilidade AS [Disponibilidade] ,e.localizacao AS [Estante] FROM Prateleira p INNER JOIN genero g on g.codigo = p.codGenero INNER JOIN estante e ON e.codigo = p.codEstante ", con)
            dtb.Clear()
            adp.Fill(dtb)
        End If
        con.Close()
    End Sub

    Private Sub TabelaEstante()
        con.Open()
        Dim adp As New SqlDataAdapter("SELECT codigo AS [Codigo], Localizacao AS [Localização], Capacidade AS [Quantidade de Prateleiras na Estante] , disponibilidade AS [Disponibilidade]  FROM Estante WHERE codigo = @codigo", con)
        adp.SelectCommand.Parameters.Add("@codigo", SqlDbType.Int).Value = frmEstante.linha
        dtbEstante.Clear()
        adp.Fill(dtbEstante)
        con.Close()

        disponibilidadeEstante = dtbEstante.Rows(0).Item("Disponibilidade")
        capacidade = dtbEstante.Rows(0).Item("Quantidade de Prateleiras na Estante")

        If disponibilidadeEstante = False Or dtgPrateleiras.Rows.Count >= capacidade Then
            btnAdicionar.Enabled = False
        Else
            btnAdicionar.Enabled = True
        End If
    End Sub

    Private Sub Prateleiras_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtgPrateleiras.DataSource = dtb
        AtualizarGrid()
        dtgPrateleiras.Columns("codGenero").Visible = False
        dtgPrateleiras.Columns("Estante").Visible = False
        dtgPrateleiras.Columns("Codigo").Width = 100
        dtgPrateleiras.Columns("Genero da Prateleira").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dtgPrateleiras.Columns("Codigo da Estante").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dtgPrateleiras.Columns("Disponibilidade").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill

        If frmEstante.linha > 0 Then
            TabelaEstante()
            localEstante = frmEstante.local
            dtgPrateleiras.Columns("codigo").Visible = False
            dtgPrateleiras.Columns("codigo da Estante").Visible = False
        End If

    End Sub

    Private Sub btnAdicionar_Click(sender As Object, e As EventArgs) Handles btnAdicionar.Click
        aux = False
        While aux = False

            frmAdcPrateleira.combosGenero()
            frmAdcPrateleira.cbxGenero.SelectedValue = 1
            frmAdcPrateleira.chkDisponibilidade.Checked = 1
            frmAdcPrateleira.combo()

            If frmEstante.linha > 0 Then
                frmAdcPrateleira.cmbEstante.SelectedValue = frmEstante.linha
            Else
                frmAdcPrateleira.cmbEstante.Enabled = True
                frmAdcPrateleira.cmbEstante.SelectedIndex = 1
            End If

            frmAdcPrateleira.ShowDialog()

            If frmAdcPrateleira.DialogResult = Windows.Forms.DialogResult.OK Then

                If disponibilidadeEstante = True Then
                    con.Open()
                    Dim cmd As New SqlCommand("INSERT INTO prateleira VALUES (@genero , @codEstante , @disponibilidade, @capacidade, @posicao)", con)
                    cmd.Parameters.Add("@genero", SqlDbType.VarChar, 100).Value = frmAdcPrateleira.cbxGenero.SelectedValue
                    cmd.Parameters.Add("@codEstante", SqlDbType.Int).Value = frmAdcPrateleira.cmbEstante.SelectedValue
                    cmd.Parameters.Add("@disponibilidade", SqlDbType.Bit).Value = frmAdcPrateleira.chkDisponibilidade.Checked
                    cmd.Parameters.Add("@capacidade", SqlDbType.Int).Value = 1
                    cmd.Parameters.Add("@posicao", SqlDbType.Int).Value = frmAdcPrateleira.posicao
                    cmd.ExecuteNonQuery()
                    con.Close()
                    AtualizarGrid()
                    ''frm msg aqui
                    frmMensagem.lblTexto.Text = "Prateleira do gênero " + frmAdcPrateleira.cbxGenero.Text + vbCrLf + "na estante " + localEstante + " foi cadastrada com sucesso!"
                    frmMensagem.Size = New Size(450, 200)
                    frmMensagem.btnOk.Location = New Point(190, 140)
                    frmMensagem.ShowDialog()
                    aux = True
                ElseIf disponibilidadeEstante = False Then
                    ''msg aqui
                    frmMensagem.lblTexto.Text = "Estante cheia, por favor selecione outra."
                    frmMensagem.Size = New Size(420, 200)
                    frmMensagem.btnOk.Location = New Point(180, 130)
                    frmMensagem.ShowDialog()
                    aux = False
                End If

                If frmEstante.linha <> 0 Then
                    TabelaEstante()
                End If

            End If
        End While

    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click

        aux = False
        While aux = False
            Try
                frmAdcPrateleira.combosGenero()
                frmAdcPrateleira.cbxGenero.SelectedValue = CInt(dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("codGenero"))
                frmAdcPrateleira.chkDisponibilidade.Checked = dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("disponibilidade")
                frmAdcPrateleira.combo()
                frmAdcPrateleira.cmbEstante.SelectedValue = dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("Codigo da Estante")
                frmAdcPrateleira.ShowDialog()

            Catch ex As Exception
                frmMensagem.lblTexto.Text = "Nenhuma prateleira selecionada." + vbCrLf + "Lembre-se de que é necessário" + vbCrLf + " selecionar para fazer a alteração desejada."
                frmMensagem.Size = New Size(450, 200)
                frmMensagem.btnOk.Location = New Point(200, 130)
                frmMensagem.Show()
                aux = True
            End Try

            If frmAdcPrateleira.DialogResult = Windows.Forms.DialogResult.OK Then
                ''trigger para update 
                con.Open()
                Dim cmd As New SqlCommand("UPDATE prateleira SET codGenero = @genero , codEstante = @estante , disponibilidade = @disponibilidade WHERE codigo = @codigo ", con)
                cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = CInt(dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("codigo"))
                cmd.Parameters.Add("@genero", SqlDbType.VarChar, 100).Value = frmAdcPrateleira.cbxGenero.SelectedValue
                cmd.Parameters.Add("@estante", SqlDbType.Int).Value = frmAdcPrateleira.cmbEstante.SelectedValue
                cmd.Parameters.Add("@disponibilidade", SqlDbType.Bit).Value = frmAdcPrateleira.chkDisponibilidade.Checked
                cmd.ExecuteNonQuery()
                con.Close()

                frmMensagem.lblTexto.Text = "PRATELEIRA ALTERADA COM SUCESSO"
                frmMensagem.Size = New Size(400, 200)
                frmMensagem.btnOk.Location = New Point(150, 85)
                frmMensagem.Show()

                AtualizarGrid()
                If frmEstante.linha <> 0 Then
                    TabelaEstante()
                End If
                aux = True
            End If

        End While

    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        ''frm confirmar 
        frmConfirma.lblTexto.Text = "Tem certeza que deseja excluir esta prateleira? " + vbCrLf + "Lembre-se que ao excluir todos os livros que " + vbCrLf + "pertenciam a ela precisarão ser realocados."
        frmConfirma.Size = New Size(600, 250)
        frmConfirma.btnSIM.Location = New Point(115, 170)
        frmConfirma.btnNAO.Location = New Point(415, 170)
        frmConfirma.ShowDialog()

        If frmConfirma.DialogResult = Windows.Forms.DialogResult.Yes Then
            Try
                con.Open()
                If dtgPrateleiras.SelectedRows.Count > 0 Then

                    Dim cmd As New SqlCommand("DELETE FROM Prateleira WHERE codigo = @codigo", con)
                    cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("Codigo")
                    cmd.ExecuteNonQuery()

                    Dim cmdDispo As New SqlCommand("UPDATE ESTANTE SET disponibilidade = 'TRUE' WHERE codigo = @codigo", con)
                    cmdDispo.Parameters.Add("@codigo", SqlDbType.Int).Value = dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("Codigo da Estante")
                    cmdDispo.ExecuteNonQuery()

                    con.Close()
                    AtualizarGrid()
                    If frmEstante.linha <> 0 Then
                        TabelaEstante()
                    End If
                End If
            Catch a As Exception
                frmMensagem.lblTexto.Text = "Selecione alguma prateleira" + vbCrLf + "para excluir.."
                frmMensagem.Size = New Size(400, 200)
                frmMensagem.btnOk.Location = New Point(150, 130)
                frmMensagem.Show()

            End Try
        End If
    End Sub

    Private Sub lblSair_Click(sender As Object, e As EventArgs) Handles lblSair.Click

        If frmEstante.linha <> 0 Then
            frmEstante.pegaGrid()
            frmEstante.Show()
        End If
        prateleira = 0
        estante = 0
        frmLivrosLocal.dtgLocal.Visible = True
        frmLivrosLocal.Label3.Text = "Os Exemplares desse livro estão em:"
        Me.Close()

    End Sub

    Private Sub btnVerMais_Click(sender As Object, e As EventArgs) Handles btnVerMais.Click
        Try
            ' If dtgPrateleiras.SelectedRows.Count > 0 Then
            prateleira = dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("Codigo")
                estante = dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("Codigo da Estante")

                Dim genero As String = dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("Genero da Prateleira")

                frmLivrosLocal.dtgLocal.Visible = False
                frmLivrosLocal.Label3.Text = "Livros na prateleira: " + genero + vbCrLf + "da estante : " + dtb.Rows(dtgPrateleiras.CurrentRow.Index).Item("Estante") + "."
                frmLivrosLocal.btnAlocar.Visible = False
                frmLivrosLocal.btnAlterar.Visible = False
                frmLivrosLocal.btnExcluir.Location = New Point(123, 503)
                frmLivrosLocal.btnExcluir.Size = New Size(248, 60)

                frmLivrosLocal.ShowDialog()
        Catch a As Exception
            frmMensagem.lblTexto.Text = "Nenhuma prateleira selecionada." + vbCrLf + "Lembre-se de que é necessário" + vbCrLf + " selecionar para ver seus livros."
            frmMensagem.Size = New Size(400, 200)
            frmMensagem.btnOk.Location = New Point(150, 130)
            frmMensagem.Show()
            ' aux = True
        Finally
            con.Close()
        End Try

    End Sub

End Class
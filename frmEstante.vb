Imports System.Data.SqlClient
Public Class frmEstante

    Dim con As SqlConnection = Biblioteca.Conexao()

    Dim dtb As New DataTable

    Dim dtbEstante As New DataTable
    Dim jaExiste As Boolean

    Public local As String
    Public linha As Integer

    Dim texto As String

    Public aux As Boolean

    Private Sub existe()
        con.Open()
        Dim adp As New SqlDataAdapter("SELECT * FROM estante WHERE localizacao LIKE '" + texto + "%' ", con)
        dtbEstante.Clear()
        adp.Fill(dtbEstante)

        If dtbEstante.Rows.Count > 0 Then
            jaExiste = True
        Else
            jaExiste = False
        End If
        con.Close()
    End Sub

    Private Sub existe2()
        ''para quando existe um no banco ou seja aquele que foi selecionado
        con.Open()
        Dim adp As New SqlDataAdapter("SELECT * FROM estante WHERE localizacao LIKE '" + texto + "' ", con)
        dtbEstante.Clear()
        adp.Fill(dtbEstante)

        If dtbEstante.Rows.Count > 1 Then
            jaExiste = True
        Else
            jaExiste = False
        End If
        con.Close()
    End Sub

    Public Sub pegaGrid()
        AtualizarGrid()
    End Sub
    Private Sub AtualizarGrid()
        con.Open()
        Dim adp As New SqlDataAdapter("SELECT codigo AS [Codigo], Localizacao AS [Localização], Capacidade AS [Quantidade de Prateleiras] , disponibilidade AS [Disponibilidade]  FROM Estante", con)
        dtb.Clear()
        adp.Fill(dtb)
        con.Close()
    End Sub

    Private Sub txtBusca_TextChanged(sender As Object, e As EventArgs) Handles txtBusca.TextChanged
        con.Open()

        If txtBusca.Text = "" Then
            Dim adp As New SqlDataAdapter("SELECT codigo AS [Codigo], Localizacao AS [Localização], Capacidade AS [Quantidade de Prateleiras] , disponibilidade AS [Disponibilidade]  FROM Estante", con)
            dtb.Clear()
            adp.Fill(dtb)
        Else
            Dim busca As New SqlDataAdapter("SELECT codigo AS [Codigo], Localizacao AS [Localização], Capacidade AS [Quantidade de Prateleiras] , disponibilidade AS [Disponibilidade]  FROM Estante where Localizacao like '%" & txtBusca.Text & "%'", con)
            dtb.Clear()
            busca.Fill(dtb)
        End If
        con.Close()

    End Sub


    Private Sub Estante_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dtgEstantes.DataSource = dtb
        AtualizarGrid()
        dtgEstantes.Columns("codigo").Visible = False
        dtgEstantes.Columns("Localização").Width = 120
        dtgEstantes.Columns("Quantidade de Prateleiras").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dtgEstantes.Columns("Disponibilidade").Width = 180

    End Sub

    Private Sub btnAdicionar_Click(sender As Object, e As EventArgs) Handles btnAdicionar.Click
        aux = False
        While aux = False

            frmAdcEstante.txtLocal.Text = ""
            frmAdcEstante.txtCapacidade.Text = ""
            frmAdcEstante.ShowDialog()

            If frmAdcEstante.DialogResult = Windows.Forms.DialogResult.OK Then
                texto = frmAdcEstante.txtLocal.Text
                existe()
                If jaExiste = False Then
                    con.Open()
                    Dim cmd As New SqlCommand("INSERT INTO estante VALUES(@localizacao , @capacidade , @disponibilidade, @alocadasP)", con)
                    cmd.Parameters.Add("@localizacao", SqlDbType.VarChar, 5).Value = frmAdcEstante.txtLocal.Text
                    cmd.Parameters.Add("@capacidade", SqlDbType.Int).Value = CInt(frmAdcEstante.txtCapacidade.Text)
                    cmd.Parameters.Add("@disponibilidade", SqlDbType.Bit).Value = 1
                    cmd.Parameters.Add("@alocadasP", SqlDbType.Bit).Value = 0
                    cmd.ExecuteNonQuery()
                    con.Close()
                    AtualizarGrid()

                    frmMensagem.lblTexto.Text = "Estante " + frmAdcEstante.txtLocal.Text + " cadastrada com sucesso!"
                    frmMensagem.Size = New Size(380, 200)
                    frmMensagem.btnOk.Location = New Point(160, 130)
                    frmMensagem.ShowDialog()

                    aux = True

                ElseIf jaExiste = True Then
                    frmMensagem.lblTexto.Text = "Já existe uma estante neste lugar, por favor insira outra localização."
                    frmMensagem.Size = New Size(600, 200)
                    frmMensagem.btnOk.Location = New Point(250, 120)
                    frmMensagem.ShowDialog()

                    aux = False

                End If
            End If
        End While
    End Sub

    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        jaExiste = True
        aux = False
        While aux = False
            Try
                If dtgEstantes.SelectedRows.Count > 0 Then
                    frmAlterarEstante.txtLocal.Text = dtb.Rows(dtgEstantes.CurrentRow.Index).Item("Localização")
                    frmAlterarEstante.txtCapacidade.Text = dtb.Rows(dtgEstantes.CurrentRow.Index).Item("Quantidade de Prateleiras")
                    frmAlterarEstante.ShowDialog()
                End If
            Catch ex As Exception
                frmMensagem.lblTexto.Text = "Nenhuma estante selecionada." + vbCrLf + "Lembre-se de que é necessário" + vbCrLf + " selecionar para fazer a alteração desejada."
                frmMensagem.Size = New Size(450, 200)
                frmMensagem.btnOk.Location = New Point(200, 130)
                frmMensagem.Show()
                aux = True
            End Try

            If frmAlterarEstante.DialogResult = Windows.Forms.DialogResult.OK Then

                texto = frmAlterarEstante.txtLocal.Text
                existe2()

                If jaExiste = False Then
                    con.Open()
                    Dim cmd As New SqlCommand("UPDATE estante SET localizacao = @localizacao , capacidade = @capacidade WHERE codigo = @codigo", con)
                    cmd.Parameters.Add("@codigo", SqlDbType.VarChar, 5).Value = dtb.Rows(dtgEstantes.CurrentRow.Index).Item("Codigo")
                    cmd.Parameters.Add("@localizacao", SqlDbType.VarChar, 5).Value = frmAlterarEstante.txtLocal.Text
                    cmd.Parameters.Add("@capacidade", SqlDbType.Int).Value = frmAlterarEstante.txtCapacidade.Text
                    cmd.ExecuteNonQuery()
                    con.Close()

                    frmMensagem.lblTexto.Text = "Estante alterada com sucesso."
                    frmMensagem.Size = New Size(350, 150)
                    frmMensagem.btnOk.Location = New Point(150, 85)
                    frmMensagem.Show()

                    AtualizarGrid()

                    aux = True

                ElseIf jaExiste = True Then
                    frmMensagem.lblTexto.Text = "Já existe uma estante neste lugar, por favor insira outra localização."
                    frmMensagem.Size = New Size(600, 150)
                    frmMensagem.btnOk.Location = New Point(250, 85)
                    frmMensagem.ShowDialog()

                    If frmMensagem.DialogResult = Windows.Forms.DialogResult.OK Then
                        aux = False
                    End If
                End If

            End If

        End While
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        ''frm confirma aqui
        frmConfirma.lblTexto.Text = "Tem certeza que deseja excluir esta estante?" + vbCrLf + " Lembre-se que ao exclui-la, todos os livros que pertenciam" + vbCrLf + " a ela precisarão ser realocados."
        frmConfirma.Size = New Size(700, 250)
        frmConfirma.btnSIM.Location = New Point(200, 170)
        frmConfirma.btnNAO.Location = New Point(400, 170)
        frmConfirma.ShowDialog()

        If frmConfirma.DialogResult = Windows.Forms.DialogResult.Yes Then
            con.Open()

            Dim cmd As New SqlCommand("DELETE FROM Estante WHERE codigo = @codigo", con)
            cmd.Parameters.Add("@codigo", SqlDbType.Int).Value = CInt(dtb.Rows(dtgEstantes.CurrentRow.Index).Item("Codigo"))
            cmd.ExecuteNonQuery()

            con.Close()
            AtualizarGrid()
            ''frm msg aqui
            frmMensagem.lblTexto.Text = "Estante excluida com sucesso!"
            frmMensagem.Size = New Size(320, 150)
            frmMensagem.btnOk.Location = New Point(120, 85)
            frmMensagem.Show()
        End If
    End Sub

    Private Sub btnVerMais_Click(sender As Object, e As EventArgs) Handles btnVerMais.Click
        Try
            linha = dtb.Rows(dtgEstantes.CurrentRow.Index).Item("Codigo")
            local = dtb.Rows(dtgEstantes.CurrentRow.Index).Item("Localização")

            frmPrateleiras.lblSubtitulo.Text = "Pertencentes a Estante: " & local & "."

            frmPrateleiras.Show()
        Catch ex As Exception
            frmMensagem.lblTexto.Text = "Nenhuma estante selecionada." + vbCrLf + "Lembre-se de que é necessário" + vbCrLf + " selecionar para ver suas prateleiras."
            frmMensagem.Size = New Size(450, 200)
            frmMensagem.btnOk.Location = New Point(200, 130)
            frmMensagem.Show()
            aux = True
        End Try
    End Sub

    Private Sub lblSair_Click(sender As Object, e As EventArgs) Handles lblSair.Click
        linha = 0
        local = ""
        Me.Close()

    End Sub

End Class
Imports System.Data.SqlClient
Public Class frmLivrosLocal

    Dim con As SqlConnection = Biblioteca.Conexao
    Dim dtb As New DataTable
    Dim dtbLocal As New DataTable

    Dim auxiliar As Boolean = False
    Dim xnaoAlocado As Boolean = False
    Dim xnaoAlocado2 As Boolean = False

    Public aux As Boolean = False

    Dim dtpLivrosMudarLocal As New DataTable

    Private Sub AtualizarGrid()
        dtb.Clear()

        con.Open()
        If frmPrateleiras.prateleira > 0 Then
            Dim adp As New SqlDataAdapter("SELECT LI.codigo AS [Codigo], LI.isbn AS [ISBN], LI.nome AS [Nome], LI.quant AS [Quantidade], LI.genero AS [Gênero], LI.Autor AS [Autor], lo.localização AS [Local] , lo.codigo AS [codLocal], p.capacidade AS [Capacidade Prateleira] FROM LIVRO li INNER JOIN LOCALIZACAO lo ON LO.CODLIVRO = LI.CODIGO  INNER JOIN prateleira p ON p.codigo = LO.codPrateleira WHERE LO.codPrateleira = '" & frmPrateleiras.prateleira & "' ", con)
            dtb.Clear()
            dtb.Columns.Clear()
            adp.Fill(dtb)
        ElseIf frmPrateleiras.prateleira <= 0 Then

            If rdbAlocados.Checked = True Then
                Dim adp As New SqlDataAdapter("SELECT codigo AS [Codigo], isbn AS [ISBN], nome AS [Nome], quant AS [Quantidade], (quant - (quant - alocados)) AS [Nao Alocados] FROM LIVRO WHERE CODIGO IN (SELECT CODLIVRO FROM LOCALIZACAO) or alocados = 0 ", con)
                dtb.Clear()
                dtb.Columns.Clear()
                adp.Fill(dtb)
            ElseIf rdbNaoAlocados.Checked = True Then
                Dim adp As New SqlDataAdapter("SELECT codigo AS [Codigo], isbn AS [ISBN], nome AS [Nome], quant AS [Quantidade], (quant - (quant - alocados)) AS [Nao Alocados] FROM LIVRO WHERE CODIGO not in (SELECT CODLIVRO FROM LOCALIZACAO) OR alocados > 0", con)
                dtb.Clear()
                dtb.Columns.Clear()
                adp.Fill(dtb)
            End If
            'Else
            '    Dim adp As New SqlDataAdapter("SELECT * FROM LIVRO ", con)
            '    dtb.Clear()
            '    dtb.Columns.Clear()

            '    adp.Fill(dtb)
        End If
        con.Close()

        If auxiliar = True Then
            If frmPrateleiras.prateleira > 0 Then
                dtgLivros.Columns("Autor").Visible = False
                dtgLivros.Columns("codLocal").Visible = False
                dtgLivros.Columns("Quantidade").Visible = False
                dtgLivros.Columns("Capacidade Prateleira").Visible = False
            End If

            dtgLivros.Columns("codigo").Visible = False

            dtgLivros.Columns("ISBN").Width = 150
            dtgLivros.Columns("Nome").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dtgLivros.Columns("Quantidade").Width = 130
        End If
    End Sub

    Private Sub AtualizarLocal()
        con.Open()
        ''n tem livro cadastro 
        If dtgLivros.SelectedRows.Count > 0 Then
            Dim adp As New SqlDataAdapter("select l.codigo as [codigo] , p.capacidade AS [Capacidade Prateleira],  l.codEstante AS [codEstante] , p.posicao AS [Posição da Prateleira] , l.localização AS [Localização do Exemplar], l.codPrateleira AS [codPrateleira] FROM localizacao l INNER JOIN Prateleira p ON l.codPrateleira = p.codigo WHERE codLivro = @codLivro", con)
            adp.SelectCommand.Parameters.Add("@codLivro", SqlDbType.Int).Value = dtb.Rows(dtgLivros.CurrentRow.Index).Item("Codigo")
            dtbLocal.Clear()
            dtbLocal.Columns.Clear()
            adp.Fill(dtbLocal)
        End If
        con.Close()

    End Sub
    Private Sub ajeitagrid()
        If dtb.Rows.Count > 0 Then

            dtgLocal.Columns("codigo").Visible = False
            dtgLocal.Columns("codEstante").Visible = False
            dtgLocal.Columns("codPrateleira").Visible = False

            dtgLocal.Columns("Capacidade Prateleira").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dtgLocal.Columns("Posição da Prateleira").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dtgLocal.Columns("Localização do Exemplar").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If

        If frmPrateleiras.prateleira > 0 Then
            dtgLivros.Columns("Autor").Visible = False
            dtgLivros.Columns("codLocal").Visible = False
            dtgLivros.Columns("Quantidade").Visible = False
            dtgLivros.Columns("Capacidade Prateleira").Visible = False
        End If

        dtgLivros.Columns("codigo").Visible = False

        dtgLivros.Columns("ISBN").Width = 150
        dtgLivros.Columns("Nome").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dtgLivros.Columns("Quantidade").Width = 130

    End Sub

    Private Sub frmLivrosLocal_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtgLivros.DataSource = dtb
        dtgLocal.DataSource = dtbLocal
        AtualizarGrid()

        dtbLocal.Clear()
        dtbLocal.Columns.Clear()
        'dtgLivros.Rows(0).Selected = True
        auxiliar = True

        If dtb.Rows.Count > 0 Then
            'ajeitagrid()
            If dtgLocal.Visible = True Then
                AtualizarLocal()


                dtgLocal.Columns("codigo").Visible = False
                dtgLocal.Columns("codEstante").Visible = False
                dtgLocal.Columns("codPrateleira").Visible = False

                dtgLocal.Columns("Capacidade Prateleira").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                dtgLocal.Columns("Posição da Prateleira").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                dtgLocal.Columns("Localização do Exemplar").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                End If
            End If

            If frmPrateleiras.prateleira > 0 Then
            dtgLivros.Columns("Autor").Visible = False
            dtgLivros.Columns("codLocal").Visible = False
            dtgLivros.Columns("Quantidade").Visible = False
            dtgLivros.Columns("Capacidade Prateleira").Visible = False

        End If

        dtgLivros.Columns("codigo").Visible = False

        dtgLivros.Columns("ISBN").Width = 150
        dtgLivros.Columns("Nome").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        dtgLivros.Columns("Quantidade").Width = 130


        If frmPrateleiras.estante > 0 And frmPrateleiras.prateleira > 0 Then
            rdbAlocados.Visible = False
            rdbNaoAlocados.Visible = False
            GroupBox1.Visible = False
        Else
            rdbAlocados.Visible = True
            rdbNaoAlocados.Visible = True
            GroupBox1.Visible = True
        End If
    End Sub

    Private Sub rdbAlocados_CheckedChanged(sender As Object, e As EventArgs) Handles rdbAlocados.CheckedChanged
        AtualizarGrid()
        If rdbAlocados.Checked = True Then
            btnAlocar.Enabled = False
            btnAlterar.Enabled = True
        Else
            btnAlocar.Enabled = True
            btnAlterar.Enabled = False
        End If
    End Sub

    Private Sub rdbNaoAlocados_CheckedChanged(sender As Object, e As EventArgs) Handles rdbNaoAlocados.CheckedChanged
        AtualizarGrid()
        If rdbNaoAlocados.Checked = True Then
            btnAlocar.Enabled = True
            btnAlterar.Enabled = False
        Else
            btnAlocar.Enabled = False
            btnAlterar.Enabled = True
        End If
    End Sub

    Private Sub btnAlocar_Click(sender As Object, e As EventArgs) Handles btnAlocar.Click
        aux = False
        While aux = False
            Try
                frmAdcLivro.combos()

                If frmPrateleiras.prateleira <> 0 And frmPrateleiras.estante <> 0 Then
                    frmAdcLivro.txtLivro.Text = dtb.Rows(dtgLivros.CurrentRow.Index).Item("Nome")
                    frmAdcLivro.chkHistorico.Checked = True
                    frmAdcLivro.carregaPrateleira()
                    frmAdcLivro.cmbPrateleira.SelectedValue = frmPrateleiras.prateleira
                    frmAdcLivro.cmbEstante.SelectedValue = frmPrateleiras.estante
                ElseIf frmPrateleiras.prateleira = 0 Then
                    frmAdcLivro.txtLivro.Text = dtb.Rows(dtgLivros.CurrentRow.Index).Item("Nome")
                    frmAdcLivro.chkHistorico.Checked = False
                    frmAdcLivro.cmbEstante.SelectedIndex = 0
                End If

                frmAdcLivro.nudQuantidade.Maximum = dtb.Rows(dtgLivros.CurrentRow.Index).Item("Nao Alocados")

                frmAdcLivro.ShowDialog()

                If frmAdcLivro.DialogResult = Windows.Forms.DialogResult.OK Then

                    Dim separar1 As String() = frmAdcLivro.local.Split(New Char() {"."})

                    con.Open()
                    For i As Integer = frmAdcLivro.capacidadePrateleira To ((CInt(frmAdcLivro.capacidadePrateleira) + frmAdcLivro.nudQuantidade.Value) - 1) Step 1
                        Dim cmd As New SqlCommand("INSERT INTO localizacao VALUES(@estante, @prateleira, @codLivro, @local)", con)
                        cmd.Parameters.Add("@estante", SqlDbType.Int).Value = CInt(frmAdcLivro.cmbEstante.SelectedValue)
                        cmd.Parameters.Add("@prateleira", SqlDbType.Int).Value = CInt(frmAdcLivro.cmbPrateleira.SelectedValue)
                        cmd.Parameters.Add("@local", SqlDbType.VarChar, 20).Value = separar1(0) + "." + separar1(1) + "." + i.ToString
                        cmd.Parameters.Add("@codLivro", SqlDbType.Int).Value = dtb.Rows(dtgLivros.CurrentRow.Index).Item("codigo")
                        cmd.ExecuteNonQuery()
                    Next

                    Dim cmdQuantidade As New SqlCommand("UPDATE LIVRO SET alocados = alocados - @quant WHERE codigo = @codigoLivro", con)
                    cmdQuantidade.Parameters.Add("@quant", SqlDbType.Int).Value = CInt(frmAdcLivro.nudQuantidade.Value)
                    cmdQuantidade.Parameters.Add("@codigoLivro", SqlDbType.Int).Value = dtb.Rows(dtgLivros.CurrentRow.Index).Item("codigo")
                    cmdQuantidade.ExecuteNonQuery()

                    Dim cmdCapacidade As New SqlCommand("UPDATE prateleira SET capacidade = capacidade + @quant WHERE codigo = @codPrateleira", con)
                    cmdCapacidade.Parameters.Add("@quant", SqlDbType.Int).Value = CInt(frmAdcLivro.nudQuantidade.Value)
                    cmdCapacidade.Parameters.Add("@codPrateleira", SqlDbType.Int).Value = CInt(frmAdcLivro.cmbPrateleira.SelectedValue)
                    cmdCapacidade.ExecuteNonQuery()

                    con.Close()

                    frmMensagem.lblTexto.Text = "Livro(s) alocado(s) com sucesso!"
                    frmMensagem.Size = New Size(350, 200)
                    frmMensagem.btnOk.Location = New Point(160, 130)
                    frmMensagem.ShowDialog()

                    aux = True

                End If
            Catch a As Exception
                frmMensagem.lblTexto.Text = "Selecione um livro para" + vbCrLf + "adicionar sua localização."
                frmMensagem.Size = New Size(300, 200)
                frmMensagem.btnOk.Location = New Point(120, 130)
                frmMensagem.ShowDialog()
                aux = True
            End Try

            AtualizarGrid()
            'ajeitagrid()
        End While
    End Sub


    Private Sub btnAlterar_Click(sender As Object, e As EventArgs) Handles btnAlterar.Click
        'dtgLivros.Rows(0).Selected = True
        'dtgLocal.Rows(0).Selected = True
        aux = False
        While aux = False
            If dtgLivros.SelectedRows.Count > 0 And dtgLocal.SelectedRows.Count > 0 Then
                ' dtgLocal.CurrentRow.Selected = True
                frmAdcLivro.combos()
                frmAdcLivro.txtLivro.Text = dtb.Rows(dtgLivros.CurrentRow.Index).Item("nome")
                frmAdcLivro.cmbEstante.SelectedValue = dtbLocal.Rows(dtgLocal.SelectedRows(0).Index).Item("codEstante")
                frmAdcLivro.carregaPrateleira()
                frmAdcLivro.cmbPrateleira.SelectedValue = dtbLocal.Rows(dtgLocal.SelectedRows(0).Index).Item("codPrateleira")
                frmAdcLivro.nudQuantidade.Maximum = dtb.Rows(dtgLivros.CurrentRow.Index).Item("Já Alocados")
                frmAdcLivro.ShowDialog()
            Else
                frmMensagem.lblTexto.Text = "Selecione um livro para" + vbCrLf + "alterar sua localização."
                frmMensagem.Size = New Size(300, 200)
                frmMensagem.btnOk.Location = New Point(120, 130)
                frmMensagem.ShowDialog()
                aux = True
            End If

            If frmAdcLivro.DialogResult = Windows.Forms.DialogResult.OK Then

                If frmAdcLivro.cmbPrateleira.SelectedValue <> dtbLocal.Rows(dtgLocal.SelectedRows(0).Index).Item("codPrateleira") Then
                    con.Open()
                    ''ATUALIZANDO PRATELEIRA
                    ''pega a posicao do livro selecionado
                    Dim separarUp As String() = dtbLocal.Rows(dtgLocal.SelectedRows(0).Index).Item("Localização do Exemplar").Split(New Char() {"."})
                    ''u = posicao do livro selecionado ate que u = quantidade de livros ja alocados u ++
                    ''MsgBox(separarUp(2))
                    For u As Integer = CInt(separarUp(2)) To dtbLocal.Rows(dtgLocal.SelectedRows(0).Index).Item("Capacidade Prateleira") Step 1

                        Dim cmdUp As New SqlCommand("UPDATE localizacao SET localização = @local WHERE localização = @localOld ", con)
                        cmdUp.Parameters.Add("@local", SqlDbType.VarChar, 20).Value = separarUp(0) + "." + separarUp(1) + "." + u.ToString
                        cmdUp.Parameters.Add("@localold", SqlDbType.VarChar, 20).Value = separarUp(0) + "." + separarUp(1) + "." + (u + frmAdcLivro.nudQuantidade.Value).ToString
                        cmdUp.ExecuteNonQuery()

                        ' MsgBox("atualizando prateleira      old>>>>>>>" + separarUp(0) + "." + separarUp(1) + "." + (u + frmAdcLivro.nudQuantidade.Value).ToString + "    new>>>>>>>>>>   " + separarUp(0) + "." + separarUp(1) + "." + u.ToString)
                    Next

                    ''INSERIR NOVO ---apagando os antigos
                    For i As Integer = dtbLocal.Rows(dtgLocal.SelectedRows(0).Index).Item("codigo") To ((CInt(dtbLocal.Rows(dtgLocal.SelectedRows(0).Index).Item("codigo") + frmAdcLivro.nudQuantidade.Value)) - 1) Step 1
                        Dim cmdDelete As New SqlCommand("DELETE FROM localizacao WHERE codigo = @cod", con)
                        cmdDelete.Parameters.Add("@cod", SqlDbType.Int).Value = i
                        cmdDelete.ExecuteNonQuery()
                    Next

                    ''inserindo valor novo

                    Dim separar1 As String() = frmAdcLivro.local.Split(New Char() {"."})

                    For i As Integer = frmAdcLivro.capacidadePrateleira To ((CInt(frmAdcLivro.capacidadePrateleira) + frmAdcLivro.nudQuantidade.Value) - 1) Step 1
                        Dim cmd As New SqlCommand("INSERT INTO localizacao VALUES(@estante, @prateleira, @codLivro, @local)", con)
                        cmd.Parameters.Add("@estante", SqlDbType.Int).Value = CInt(frmAdcLivro.cmbEstante.SelectedValue)
                        cmd.Parameters.Add("@prateleira", SqlDbType.Int).Value = CInt(frmAdcLivro.cmbPrateleira.SelectedValue)
                        cmd.Parameters.Add("@local", SqlDbType.VarChar, 20).Value = separar1(0) + "." + separar1(1) + "." + i.ToString
                        cmd.Parameters.Add("@codLivro", SqlDbType.Int).Value = dtb.Rows(dtgLivros.CurrentRow.Index).Item("codigo")
                        cmd.ExecuteNonQuery()
                    Next

                    Dim cmdQuantidade As New SqlCommand("UPDATE LIVRO SET alocados = alocados - @quant WHERE codigo = @codigoLivro", con)
                    cmdQuantidade.Parameters.Add("@quant", SqlDbType.Int).Value = CInt(frmAdcLivro.nudQuantidade.Value)
                    cmdQuantidade.Parameters.Add("@codigoLivro", SqlDbType.Int).Value = dtb.Rows(dtgLivros.CurrentRow.Index).Item("codigo")
                    cmdQuantidade.ExecuteNonQuery()

                    Dim cmdCapacidade As New SqlCommand("UPDATE prateleira SET capacidade = capacidade + @quant WHERE codigo = @codPrateleira", con)
                    cmdCapacidade.Parameters.Add("@quant", SqlDbType.Int).Value = CInt(frmAdcLivro.nudQuantidade.Value)
                    cmdCapacidade.Parameters.Add("@codPrateleira", SqlDbType.Int).Value = CInt(frmAdcLivro.cmbPrateleira.SelectedValue)
                    cmdCapacidade.ExecuteNonQuery()

                    frmMensagem.lblTexto.Text = "Livro(s) realocado(s) com sucesso!"
                    frmMensagem.Size = New Size(500, 300)
                    frmMensagem.btnOk.Location = New Point(250, 200)
                    frmMensagem.ShowDialog()

                    con.Close()

                    AtualizarGrid()
                    aux = True
                End If '' if qyue verifica se a prateleir nova eh diferente da antiga

            End If

        End While
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

        If frmPrateleiras.prateleira <> 0 Then
            frmPrateleiras.carregacombo
            frmPrateleiras.Show()
        End If
        dtb.Clear()
        dtbLocal.Clear()
        auxiliar = False
        btnAlocar.Visible = True
        btnAlterar.Visible = True
        btnExcluir.Location = New Point(1069, 654)
        btnExcluir.Size = New Size(164, 60)
        btnExcluir.Visible = True
        Me.Close()
    End Sub

    Private Sub btnExcluir_Click(sender As Object, e As EventArgs) Handles btnExcluir.Click
        ''confirm aqui em certeza que deseja excluir a localização de todos exemplares deste livro?
        ' Try
        If dtgLivros.SelectedRows.Count > 0 Then
            frmConfirma.lblTexto.Text = "Tem certeza que deseja excluir a localização" + vbCrLf + " deste livro?"
            frmConfirma.Size = New Size(600, 250)
            frmConfirma.btnSIM.Location = New Point(115, 170)
            frmConfirma.btnNAO.Location = New Point(415, 170)
            frmConfirma.ShowDialog()

            If frmConfirma.DialogResult = Windows.Forms.DialogResult.Yes Then

                If frmPrateleiras.prateleira > 0 Then ''verifica se eh chamado da prateeira ou nao
                    con.Open()
                    Dim cmd As New SqlCommand("DELETE FROM localizacao WHERE codigo = @cod", con)
                    cmd.Parameters.Add("@cod", SqlDbType.Int).Value = dtb.Rows(dtgLivros.CurrentRow.Index).Item("codLocal")
                    cmd.ExecuteNonQuery()

                    Dim separarUp As String() = dtb.Rows(dtgLivros.CurrentRow.Index).Item("Local").Split(New Char() {"."})

                    For u As Integer = CInt(separarUp(2)) To dtb.Rows(dtgLivros.CurrentRow.Index).Item("Capacidade Prateleira") Step 1

                        Dim cmdUp As New SqlCommand("UPDATE localizacao SET localização = @local WHERE localização = @localOld ", con)
                        cmdUp.Parameters.Add("@local", SqlDbType.VarChar, 20).Value = separarUp(0) + "." + separarUp(1) + "." + u.ToString
                        cmdUp.Parameters.Add("@localold", SqlDbType.VarChar, 20).Value = separarUp(0) + "." + separarUp(1) + "." + (u + 1).ToString
                        cmdUp.ExecuteNonQuery()
                    Next

                    con.Close()
                    AtualizarGrid()
                    frmMensagem.lblTexto.Text = "Localização excluida com sucesso."
                    frmMensagem.Size = New Size(350, 200)
                    frmMensagem.btnOk.Location = New Point(150, 120)
                    frmMensagem.ShowDialog()

                ElseIf frmPrateleiras.prateleira <= 0 Then

                    'Try
                    If dtgLocal.Rows.Count > 0 Then
                        con.Open()
                        ''codigo maximo
                        Dim adp As New SqlDataAdapter("SELECT MAX(codigo) FROM Localizacao WHERE codLivro = @codLivro", con)
                        adp.SelectCommand.Parameters.Add("@codLivro", SqlDbType.Int).Value = dtb.Rows(dtgLivros.CurrentRow.Index).Item("Codigo")
                        Dim dtbCod As New DataTable
                        adp.Fill(dtbCod)

                        For i As Integer = dtbLocal.Rows(0).Item("codigo") To dtbCod.Rows(0).Item(0) Step 1
                            Dim cmd As New SqlCommand("DELETE FROM localizacao WHERE codigo = @cod AND codLivro = @codlivro", con)
                            cmd.Parameters.Add("@cod", SqlDbType.Int).Value = i
                            cmd.Parameters.Add("@codLivro", SqlDbType.Int).Value = dtb.Rows(dtgLivros.CurrentRow.Index).Item("Codigo")
                            cmd.ExecuteNonQuery()
                            ''trigger que altera a capacidade da prateleira no banco 
                        Next
                        con.Close()

                        AtualizarGrid()

                        frmMensagem.lblTexto.Text = "Localização excluida com sucesso."
                        frmMensagem.Size = New Size(350, 200)
                        frmMensagem.btnOk.Location = New Point(150, 120)
                        frmMensagem.ShowDialog()
                        'msg de excluido aqui
                        ' Catch a As Exception
                    Else
                        frmMensagem.lblTexto.Text = "Este livro não esta alocado ainda."
                        frmMensagem.Size = New Size(350, 200)
                        frmMensagem.btnOk.Location = New Point(150, 120)
                        frmMensagem.ShowDialog()
                    End If
                    'Finally
                    'con.Close()
                    'End Try

                End If ''se pertence a prateleira

                End If ''se clicou yes

        Else
            frmMensagem.lblTexto.Text = "Para excluir, selecine um livro."
            frmMensagem.Size = New Size(300, 200)
            frmMensagem.btnOk.Location = New Point(100, 120)
            frmMensagem.ShowDialog()
        End If

    End Sub

    Private Sub dtgLivros_Click_1(sender As Object, e As EventArgs) Handles dtgLivros.Click

        If dtb.Rows.Count > 0 Then
            AtualizarLocal()
            dtgLocal.Columns("codigo").Visible = False
            dtgLocal.Columns("codEstante").Visible = False
            dtgLocal.Columns("codPrateleira").Visible = False

            dtgLocal.Columns("Capacidade Prateleira").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dtgLocal.Columns("Posição da Prateleira").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            dtgLocal.Columns("Localização do Exemplar").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        End If

    End Sub
End Class
Imports System.IO
Imports System.Text

Public Class Form1
    Private MLabel As LabelProject.Label
    Private ReadOnly Trn As String = """"
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        With OpenFileDialog1

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                Label1.Text = .FileName
                MLabel = New LabelProject.Label
                MLabel.ReadXML(.FileName)

                ListBox1.Items.Clear()

                For Each Itm As LabelProject.LabelItem In MLabel
                    ListBox1.Items.Add(Itm.Name)
                Next

            End If
        End With
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim SFileName As String
        With SaveFileDialog1
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                SFileName = .FileName
                Label2.Text = SFileName
            Else
                Exit Sub
            End If
        End With

        Dim outfile As New StreamWriter(SFileName)
        Dim sb As New StringBuilder()

        With sb
            .AppendLine("Imports System.Drawing")
            .AppendLine("Public Class XLabel")
            .AppendLine("Private M_Label As New LabelProject.Label")

            .AppendLine("#Region ""Properties""")
            For Each Itm As LabelProject.LabelItem In MLabel
                If Itm.Type = LabelProject.LabelItem.ItemType.Text Then
                    .AppendLine("Public WriteOnly Property Txt" & Itm.Name.Replace(" ", "_") & " As String")
                    .AppendLine("Set(value As String)")
                    .AppendLine("M_Label.Item(" & """" & Itm.ItemNumber.ToString & """" & ").Text = value")
                    .AppendLine("End Set")
                    .AppendLine("End Property")
                End If
            Next
            .AppendLine("#End Region")

            .AppendLine("Public Sub New()")
            .AppendLine("Dim Item As New LabelProject.LabelItem")
            .AppendLine("Dim PgSettings As New Printing.PageSettings")

            .AppendLine("With PgSettings")
            .AppendLine("For Each P As Printing.PaperSize In M_Label.PrintDoc.PrinterSettings.PaperSizes")
            .AppendLine("If P.PaperName = " & """" & MLabel.PrintDoc.DefaultPageSettings.PaperSize.PaperName & """" & " Then")
            .AppendLine(".PaperSize = P")
            .AppendLine("Exit For")
            .AppendLine("End If")
            .AppendLine("Next")

            .AppendLine("With .Margins")
            .AppendLine(".Top =" & MLabel.PrintDoc.DefaultPageSettings.Margins.Top)
            .AppendLine(".Bottom = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Bottom)
            .AppendLine(".Left = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Left)
            .AppendLine(".Right = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Right)
            .AppendLine("End With") 'Margins

            .AppendLine(".Landscape = " & MLabel.PrintDoc.DefaultPageSettings.Landscape.ToString)

            .AppendLine("End With") 'PgSettings

            .AppendLine("M_Label.PrintDoc.DefaultPageSettings = PgSettings")


            .AppendLine("M_Label.CokluEtiket = " & MLabel.CokluEtiket.ToString)
            .AppendLine("M_Label.EtiketSatırSayısı = " & MLabel.EtiketSatırSayısı)
            .AppendLine("M_Label.EtiketSutunSayısı = " & MLabel.EtiketSutunSayısı)


            For Each Itm As LabelProject.LabelItem In MLabel
                .AppendLine("Item = New LabelProject.LabelItem")
                .AppendLine("With Item")
                .AppendLine(".Type = LabelProject.LabelItem.ItemType." & Itm.Type.ToString) 'Box
                .AppendLine(".Name = " & """" & Itm.Name & """")
                .AppendLine(".x1 = " & Itm.x1)
                .AppendLine(".y1 = " & Itm.y1)
                .AppendLine(".Width = " & Itm.Width)
                .AppendLine(".Height = " & Itm.Height)
                .AppendLine(".LineWidth = " & Itm.LineWidth)
                .AppendLine(".ItemNumber = " & Itm.ItemNumber)
                .AppendLine("End With")
                .AppendLine("M_Label.Add(Item)")
            Next

            .AppendLine("End Sub")

            .AppendLine("Public Sub OnIzleme()")
            .AppendLine("M_Label.PrintPreview()")
            .AppendLine("End Sub")

            .AppendLine("Public Sub Yazdır()")
            .AppendLine("M_Label.PrintDoc.Print()")
            .AppendLine("End Sub")

            .AppendLine("End Class")
        End With

        Try
            outfile.Write(sb.ToString())
            outfile.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub


End Class

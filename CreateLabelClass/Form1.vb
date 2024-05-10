Imports System.IO
Imports System.Text
Imports LabelLibrary
Imports LabelLibrary.LabelProject


Public Class Form1
    Private MLabel As Label
    Private ReadOnly Trn As String = ControlChars.Quote
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        With OpenFileDialog1

            If .ShowDialog = Windows.Forms.DialogResult.OK Then

                Label1.Text = .FileName
                MLabel = New Label
                MLabel.ReadXML(.FileName)

                ListBox1.Items.Clear()

                For Each Itm As LabelItem In MLabel
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
            .AppendLine("Imports LabelLibrary")
            .AppendLine("Public Class XLabel")
            .AppendLine("Private M_Label As New LabelProject.Label")
            .AppendLine("")

            If MLabel IsNot Nothing Then

                '  .AppendLine("#Region ""Properties""")

                '  For Each Itm As LabelItem In MLabel
                '      If Itm.Type = LabelItem.ItemType.Text Then
                '          .AppendLine("Public WriteOnly Property Txt" & Itm.Name.Replace(" ", "_") & " As String")
                '          .AppendLine("Set(value As String)")
                '          .AppendLine("M_Label.Item(" & Trn & Itm.ItemNumber.ToString & Trn & ").Text = value")
                '          .AppendLine("End Set")
                '          .AppendLine("End Property")
                '      End If
                '  Next

                '  .AppendLine("#End Region")
                .AppendLine("")

                .AppendLine("Public Sub New()")
                .AppendLine("Dim Item As New LabelProject.LabelItem")
                .AppendLine("Dim PgSettings As New Printing.PageSettings")

                .AppendLine("With PgSettings")
                .AppendLine("For Each P As Printing.PaperSize In M_Label.PrintDoc.PrinterSettings.PaperSizes")
                .AppendLine("If P.PaperName = " & Trn & MLabel.PrintDoc.DefaultPageSettings.PaperSize.PaperName & Trn & " Then")
                .AppendLine(".PaperSize = P")
                .AppendLine("Exit For")
                .AppendLine("End If")
                .AppendLine("")
                .AppendLine("Next")

                .AppendLine("")
                .AppendLine("With .Margins")
                .AppendLine(".Top =" & MLabel.PrintDoc.DefaultPageSettings.Margins.Top)
                .AppendLine(".Bottom = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Bottom)
                .AppendLine(".Left = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Left)
                .AppendLine(".Right = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Right)
                .AppendLine("End With") 'Margins

                .AppendLine("")
                .AppendLine(".Landscape = " & MLabel.PrintDoc.DefaultPageSettings.Landscape.ToString)

                .AppendLine("End With") 'PgSettings
                .AppendLine("")

                .AppendLine("M_Label.PrintDoc.DefaultPageSettings = PgSettings")


                .AppendLine("M_Label.CokluEtiket = " & MLabel.CokluEtiket.ToString)
                .AppendLine("M_Label.EtiketSatırSayısı = " & MLabel.EtiketSatırSayısı)
                .AppendLine("M_Label.EtiketSutunSayısı = " & MLabel.EtiketSutunSayısı)
                .AppendLine("")


                For Each Itm As LabelItem In MLabel


                    .AppendLine("Item = New LabelProject.LabelItem")
                    .AppendLine("With Item")
                    .AppendLine(".Type = LabelProject.LabelItem.ItemType." & Itm.Type.ToString) 'Box
                    .AppendLine(".Name = " & Trn & Itm.Name & Trn)
                    .AppendLine(".x1 = " & Itm.x1)
                    .AppendLine(".y1 = " & Itm.y1)
                    .AppendLine(".Width = " & Itm.Width)
                    .AppendLine(".Height = " & Itm.Height)
                    .AppendLine(".LineWidth = " & Itm.LineWidth)
                    .AppendLine(".ItemNumber = " & Itm.ItemNumber)
                    .AppendLine(".Text = """ & Itm.Text & Trn)
                    .AppendLine(".DataField = """ & Itm.DataField & Trn)
                    .AppendLine(".BarcodePrefixChar = """ & Itm.BarcodePrefixChar & Trn)
                    .AppendLine(".RefAlanListesi = """ & Itm.RefAlanListesi & Trn)
                    .AppendLine("End With")
                    .AppendLine("M_Label.Add(Item)")
                    .AppendLine("")
                Next

                .AppendLine("End Sub")
                .AppendLine("")
            End If

            .AppendLine("Public Sub OnIzleme()")
            .AppendLine("M_Label.PrintPreview()")
            .AppendLine("End Sub")
            .AppendLine("")

            .AppendLine("Public Sub Yazdır()")
            .AppendLine("M_Label.PrintDoc.Print()")
            .AppendLine("End Sub")
            .AppendLine("")

            .AppendLine("End Class")
        End With

        Try
            outfile.Write(sb.ToString())
            outfile.Close()
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    ''Class oluşturma v2
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
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
            .AppendLine("Imports LabelLibrary")
            .AppendLine("Public Class XLabel")
            .AppendLine("Private M_Label As New LabelProject.Label")
            .AppendLine("")

            If MLabel IsNot Nothing Then

                .AppendLine("Public Sub New()")
                .AppendLine("Dim Item As New LabelProject.LabelItem")
                .AppendLine("Dim PgSettings As New Printing.PageSettings")

                .AppendLine("With PgSettings")
                .AppendLine("For Each P As Printing.PaperSize In M_Label.PrintDoc.PrinterSettings.PaperSizes")
                .AppendLine("If P.PaperName = " & Trn & MLabel.PrintDoc.DefaultPageSettings.PaperSize.PaperName & Trn & " Then")
                .AppendLine(".PaperSize = P")
                .AppendLine("Exit For")
                .AppendLine("End If")
                .AppendLine("")
                .AppendLine("Next")

                .AppendLine("")
                .AppendLine("With .Margins")
                .AppendLine(".Top =" & MLabel.PrintDoc.DefaultPageSettings.Margins.Top)
                .AppendLine(".Bottom = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Bottom)
                .AppendLine(".Left = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Left)
                .AppendLine(".Right = " & MLabel.PrintDoc.DefaultPageSettings.Margins.Right)
                .AppendLine("End With") 'Margins

                .AppendLine("")
                .AppendLine(".Landscape = " & MLabel.PrintDoc.DefaultPageSettings.Landscape.ToString)

                .AppendLine("End With") 'PgSettings
                .AppendLine("")

                .AppendLine("M_Label.PrintDoc.DefaultPageSettings = PgSettings")

                .AppendLine("M_Label.CokluEtiket = " & MLabel.CokluEtiket.ToString)
                .AppendLine("M_Label.EtiketSatırSayısı = " & MLabel.EtiketSatırSayısı)
                .AppendLine("M_Label.EtiketSutunSayısı = " & MLabel.EtiketSutunSayısı)
                .AppendLine("")


                ListBox2.Items.Clear()

                For Each Itm As LabelItem In MLabel

                    ListBox2.Items.Add("Name : " & Itm.Name)
                    'CreateItem("Cizgi_1", LabelProject.LabelItem.ItemType.Line, 0, 25, 146, 0, "")

                    Dim K As String = "CreateItem(" & Trn & Itm.Name & Trn & ","
                    K &= "LabelProject.LabelItem.ItemType." & [Enum].GetName(GetType(LabelItem.ItemType), Itm.Type) & ","
                    K &= Itm.x1 & "," & Itm.y1 & ","
                    K &= Itm.Width & "," & Itm.Height & ","
                    K &= Trn & Itm.Text & Trn & ","
                    K &= Trn & Itm.DataField & Trn & ","
                    K &= Trn & Itm.RefAlanListesi.Replace(Trn, Trn & " & ControlChars.Quote & " & Trn) & Trn & ","
                    K &= "LabelProject.LabelItem.BarcodeTypes." & [Enum].GetName(GetType(LabelItem.BarcodeTypes), Itm.BarcodeType) & ","
                    K &= Trn & Itm.BarcodePrefixChar & Trn & ","
                    K &= Trn & Itm.BarkodVeriAlanı & Trn & ","
                    K &= IIf(Itm.BarcodeText, "True", "False") & ","

                    'New Font("Arial", 10, FontStyle.Bold Or FontStyle.Italic)
                    K &= "New Font(" & Trn & Itm.MyFont.FontFamily.Name.ToString & Trn & "," & Itm.MyFont.Size & ","

                    Dim Fs As String = vbNullString
                    If Itm.MyFont.Bold Then Fs &= "FontStyle.Bold Or"
                    If Itm.MyFont.Italic Then Fs &= "FontStyle.Italic Or"
                    If Itm.MyFont.Underline Then Fs &= "FontStyle.UnderLine Or"
                    If Itm.MyFont.Strikeout Then Fs &= "FontStyle.Strikeout"

                    If Fs = vbNullString Then
                        K &= "FontStyle.Regular"
                    Else
                        K &= Fs.TrimEnd("O", "r")
                    End If
                    K &= "),"

                    K &= "ContentAlignment." & [Enum].GetName(GetType(ContentAlignment), Itm.TextAlign) & ","
                    K &= "Color." & Itm.LineColor.Name & ","
                    K &= Itm.LineWidth

                    K &= ")"
                    .AppendLine(K)

                Next
                'Regular    0   Normal metin.
                'Bold       1   Kalın metin.
                'Italic     2   italik metin.
                'Underline  4   Altı çizili metin.
                'Strikeout  8   Ortada bir çizgi olan metin.

                'Name As String, ItmType As LabelProject.LabelItem.ItemType,
                'x1 As Single, y1 As Single,
                'Width As Single, Height As Single,
                'Text As String,
                'Optional DataField As String = vbNullString,
                'Optional RefAlanlist As String = vbNullString,
                'Optional BarkodType As LabelProject.LabelItem.BarcodeTypes = LabelProject.LabelItem.BarcodeTypes.Code128,
                'Optional BarkodPrfxChr As String = vbNullString,
                'Optional BarkodVeriAlan As String = vbNullString,
                'Optional BarkodText As String = true,
                'Optional YazıTipi As Font = Nothing,
                'Optional YazıHiza As ContentAlignment = ContentAlignment.MiddleLeft,
                'Optional CizgiRengi As Color = Nothing,
                'Optional CizgiK As Single = 1,
                'Optional Resim As Image = Nothing

                'CreateItem("Cizgi_dikey_3", LabelProject.LabelItem.ItemType.Line, 55, 25, 0, 25, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Bold Or FontStyle.Italic))


                .AppendLine("End Sub")

                .AppendLine("Public Sub OnIzleme()")
                .AppendLine("M_Label.PrintPreview()")
                .AppendLine("End Sub")
                .AppendLine("")

                .AppendLine("Public Sub Yazdır()")
                .AppendLine("M_Label.PrintDoc.Print()")
                .AppendLine("End Sub")
                .AppendLine("")



                .AppendLine("Private Sub CreateItem(Name As String, ItmType As LabelProject.LabelItem.ItemType,")
                .AppendLine("                   x1 As Single, y1 As Single,")
                .AppendLine("                   Width As Single, Height As Single,")
                .AppendLine("                   Text As String,")
                .AppendLine("                   Optional DataField As String = vbNullString,")
                .AppendLine("                   Optional RefAlanlist As String = vbNullString,")
                .AppendLine("                   Optional BarkodType As LabelProject.LabelItem.BarcodeTypes = LabelProject.LabelItem.BarcodeTypes.Code128,")
                .AppendLine("                   Optional BarkodPrfxChr As String = vbNullString,")
                .AppendLine("                   Optional BarkodVeriAlan As String = vbNullString,")
                .AppendLine("                   Optional BarkodText As Boolean = True,")
                .AppendLine("                   Optional YazıTipi As Font = Nothing,")
                .AppendLine("                   Optional YazıHiza As ContentAlignment = ContentAlignment.MiddleLeft,")
                .AppendLine("                   Optional CizgiRengi As Color = Nothing,")
                .AppendLine("                   Optional CizgiK As Single = 1,")
                .AppendLine("                   Optional Resim As Image = Nothing)")
                .AppendLine("")
                .AppendLine("Dim Item = New LabelProject.LabelItem")
                .AppendLine("With Item")
                .AppendLine("    .Type = ItmType")
                .AppendLine("    .Name = Name")
                .AppendLine("    .Text = Text")
                .AppendLine("    .x1 = x1")
                .AppendLine("    .y1 = y1")
                .AppendLine("    .Width = Width")
                .AppendLine("    .Height = Height")
                .AppendLine("    If IsNothing(CizgiRengi) Then")
                .AppendLine("        .LineColor = Color.Black")
                .AppendLine("    Else")
                .AppendLine("        .LineColor = CizgiRengi")
                .AppendLine("    End If")
                .AppendLine("    .LineWidth = CizgiK")
                .AppendLine("    .DataField = DataField")
                .AppendLine("    .BarcodePrefixChar = BarkodPrfxChr")
                .AppendLine("    .BarcodeType = BarkodType")
                .AppendLine("    .BarkodVeriAlanı = BarkodVeriAlan")
                .AppendLine("    .RefAlanListesi = RefAlanlist")
                .AppendLine("    .BarcodeText = BarkodText")
                .AppendLine("    If IsNothing(YazıTipi) Then")
                .AppendLine("        .MyFont = New Font(" & ControlChars.Quote & "Arial" & ControlChars.Quote & ", 10)")
                .AppendLine("    Else")
                .AppendLine("        .MyFont = YazıTipi")
                .AppendLine("    End If")
                .AppendLine("    .TextAlign = YazıHiza")
                .AppendLine("    .Image = Resim")
                .AppendLine("End With")
                .AppendLine("")
                .AppendLine("M_Label.Add(Item)")
                .AppendLine("End Sub")

            End If

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

Imports System.Xml
Imports System.Drawing.Printing
Imports System.Data
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Net.NetworkInformation
Imports System.IO.Compression

Public Class LabelProject
    Shared ItemCount As Integer = 0
    Const mm As Single = 3.937

    Public Class Label
        Implements IEnumerable, IEnumerator

        Private List As New System.Collections.Specialized.OrderedDictionary
        Private Position As Integer = -1
        Private WithEvents mPrintDoc As New Printing.PrintDocument
        Private WithEvents mPageSetupDialog As New PageSetupDialog With {.Document = mPrintDoc, .EnableMetric = True}

        Private PrintAllPage As Boolean = False
        Private PrintPageNum As Integer = 0
        Private PrintLabelNum As Integer = 0
        Private mSelectedItm As LabelItem

#Region "Properties"

        Public Property Açıklama As String
        Public Property DSNName As String
        Public Property SQLString As String

        'Public Property PageSetupDlg As New PageSetupDialog With {.Document = mPrintDoc, .EnableMetric = True}
        Public Property PrintDlg As New PrintDialog With {.Document = mPrintDoc}
        Public Property PrintPreviewDlg As New PrintPreviewDialog With {.Document = mPrintDoc}
        Public Property MyDataTable As New DataTable

        Public Property CokluEtiket As Boolean = False
        Public Property EtiketSatırSayısı As Integer = 1
        Public Property EtiketSutunSayısı As Integer = 1

        Public Property SelectedItem As LabelItem
            Set(value As LabelItem)
                mSelectedItm = value
            End Set
            Get
                Return mSelectedItm
            End Get
        End Property

        Public Property PrintDoc As Printing.PrintDocument
            Get
                Return mPrintDoc
            End Get
            Set(value As Printing.PrintDocument)
                mPrintDoc = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal Index As Integer) As LabelItem
            Get
                Return CType(List(Index), LabelItem)
            End Get
        End Property

        Default Public ReadOnly Property Item(ByVal Key As String) As LabelItem
            Get
                Return CType(List(Key), LabelItem)
            End Get
        End Property

        Public ReadOnly Property Count() As Integer
            Get
                Return List.Count
            End Get
        End Property

        Public ReadOnly Property Current() As Object Implements System.Collections.IEnumerator.Current
            Get
                Return CType(List(Position), LabelItem)
            End Get
        End Property

        Public Property ParameterTable As New ParameterTableClass

        Public Event ErrorEvent(Hata As String)
        Public Property DrawMargins As Boolean = False
#End Region

        Public Sub New()
            'List = New System.Collections.Specialized.OrderedDictionary
            'PageSetupDlg.Document = PrintDoc
            'PageSetupDlg.EnableMetric = True
            'PrintDlg.Document = PrintDoc
        End Sub

        Public Sub Add(ByVal Value As LabelItem)
            Try
                List.Add(Value.Name, Value)
                'List.Add(Value.ItemNumber, Value)
            Catch ex As Exception
                RaiseEvent ErrorEvent(ex.Message)
            End Try
        End Sub

        Public Sub AddText(Txt As String,
                           x As Single, y As Single,
                           W As Single, H As Single,
                           Optional FSize As Integer = 10,
                           Optional FBold As Boolean = True,
                           Optional FItalik As Boolean = False,
                           Optional Box As Boolean = False,
                           Optional Hiza As ContentAlignment = ContentAlignment.MiddleLeft,
                           Optional ItemName As String = vbNullString)

            Dim NewItm As New LabelItem(LabelItem.ItemType.Text, ItemName)

            With NewItm
                .Text = Txt
                .x1 = x
                .y1 = y
                .Width = W
                .Height = H

                Dim FStyle As FontStyle = 0
                If FBold Then FStyle = FStyle Or FontStyle.Bold
                If FItalik Then FStyle = FStyle Or FontStyle.Italic

                .MyFont = New Font(FontFamily.GenericSansSerif, FSize, FStyle)
                .TextAlign = Hiza
            End With

            Try
                List.Add(NewItm.Name, NewItm)
            Catch ex As Exception
                RaiseEvent ErrorEvent(ex.Message)
            End Try


            If Box Then AddBox(x, y, W, H, Color.Black, 1)

        End Sub

        Public Sub AddBox(x As Single, y As Single,
                           W As Single, H As Single,
                           Renk As Color,
                           Optional LineW As Single = 1,
                          Optional ItemName As String = vbNullString)

            Dim NewItm As New LabelItem(LabelItem.ItemType.Box, ItemName)
            With NewItm
                .LineWidth = LineW
                .LineColor = Renk 'Color.Black
                .x1 = x
                .y1 = y
                .Width = W
                .Height = H
            End With
            Try
                List.Add(NewItm.Name, NewItm)
            Catch ex As Exception
                RaiseEvent ErrorEvent(ex.Message)
            End Try

        End Sub

        Public Sub AddLine(x As Single, y As Single,
                           W As Single, H As Single,
                           Renk As Color,
                           Optional LineW As Single = 1,
                           Optional ItemName As String = vbNullString)

            Dim NewItm As New LabelItem(LabelItem.ItemType.Line, ItemName)
            With NewItm
                .LineWidth = LineW
                .LineColor = Renk 'Color.Black
                .x1 = x
                .y1 = y
                .Width = W
                .Height = H
            End With
            Try
                List.Add(NewItm.Name, NewItm)
            Catch ex As Exception
                RaiseEvent ErrorEvent(ex.Message)
            End Try

        End Sub

        Public Sub AddImage(Img As Image,
                           x As Single, y As Single,
                           W As Single, H As Single, Optional Box As Boolean = False,
                            Optional ItemName As String = vbNullString)

            Dim NewItm As New LabelItem(LabelItem.ItemType.Image, ItemName)
            With NewItm
                .Image = Img
                .x1 = x
                .y1 = y
                .Width = W
                .Height = H
            End With
            Try
                List.Add(NewItm.Name, NewItm)
            Catch ex As Exception
                RaiseEvent ErrorEvent(ex.Message)
            End Try


            If Box Then AddBox(x, y, W, H, Color.Black, 1)
        End Sub

        Public Sub AddBarcode(Txt As String,
                              x As Single, y As Single,
                              W As Single, H As Single,
                              Barcodetype As LabelItem.BarcodeTypes,
                              Optional PrfxChar As String = vbNullString,
                              Optional ItemName As String = vbNullString)

            Dim NewItm As New LabelItem(LabelItem.ItemType.Barcode, ItemName)
            With NewItm
                .Text = Txt
                .BarcodePrefixChar = PrfxChar
                .x1 = x
                .y1 = y
                .BarcodeType = Barcodetype
                .Height = H
            End With
            Try
                List.Add(NewItm.Name, NewItm)
            Catch ex As Exception
                RaiseEvent ErrorEvent(ex.Message)
            End Try

        End Sub

        Public Sub Remove(ByVal Key As String)
            For Each Itm As LabelItem In List.Values
                If Itm.Name = Key Then
                    Try
                        List.Remove(Itm.ItemNumber)
                    Catch ex As Exception
                        MsgBox(ex.Message, MsgBoxStyle.Critical, "Silme Hatası")
                    End Try
                    Exit For
                End If
            Next

            'List.Remove(Key)
        End Sub

        Public Sub Remove(ByVal Index As Integer)
            List.RemoveAt(Index)
        End Sub

        Public Sub Clear()
            List.Clear()
            ItemCount = 0
        End Sub

        Public Function MoveNext() As Boolean Implements System.Collections.IEnumerator.MoveNext
            Position += 1
            Return Position < List.Count
        End Function

        Public Function MoveFirst() As Boolean
            Position = 0
            Return Position < List.Count
        End Function

        Public Sub Reset() Implements System.Collections.IEnumerator.Reset
            Position = -1
        End Sub

        Public Function GetEnumerator() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
            Position = -1
            Return CType(Me, IEnumerator)
        End Function

        Public Sub PrintAll()

            If PrintDlg.ShowDialog = DialogResult.OK Then
                PrintAllPage = True
                PrintPageNum = 0
                PrintLabelNum = 0

                PrintDoc.Print()

                PrintAllPage = False
                PrintPageNum = 0
            End If

        End Sub

        Public Sub PrintPreview()
            PrintPreviewDlg.ShowDialog()
        End Sub

        Public Sub SetBarkodText()
            For Each BarkodItem As LabelItem In Me
                For i = 0 To Me.List.Count - 1
                    Dim Itmx As LabelItem = CType(List(i), LabelItem)
                    If Itmx.Type = LabelItem.ItemType.Text AndAlso BarkodItem.BarkodVeriAlanı = Itmx.Name Then
                        BarkodItem.Text = Itmx.Text
                        Exit For
                    End If
                Next
            Next
        End Sub

        Public Sub SetBirlesikAlanlar()
            For Each Itm As LabelItem In Me
                If Itm.Type = LabelItem.ItemType.Text AndAlso Itm.RefAlanListesi.Length > 0 Then
                    Dim Refs() As String = Split(Itm.RefAlanListesi, ";")
                    Itm.Text = ""

                    For Each Rf As String In Refs

                        ' çift tırnak kontrolü
                        If Rf.StartsWith(ControlChars.Quote) And Rf.EndsWith(ControlChars.Quote) Then
                            Itm.Text &= Rf.Replace(ControlChars.Quote, vbNullString)
                        Else
                            For i = 0 To Me.List.Count - 1
                                Dim Itmx As LabelItem = CType(List(i), LabelItem)
                                If Itmx.Name = Rf Then
                                    Itm.Text &= Itmx.Text
                                    Exit For
                                End If
                            Next
                        End If

                    Next
                End If
            Next
        End Sub

        Private Sub mPrintDoc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles mPrintDoc.PrintPage
            Dim SatırArası, SutunArası As Single
            Dim Stn, Str As Integer

            'Margins 
            Dim MarginPen As New Pen(Brushes.Blue) With {.DashStyle = Drawing2D.DashStyle.Dash}
            With e.Graphics
                If DrawMargins Then .DrawRectangle(MarginPen, e.MarginBounds)
            End With

            If CokluEtiket Then

                Dim MyPen As New Pen(Brushes.Black) With {.DashStyle = Drawing2D.DashStyle.DashDot}

                If EtiketSatırSayısı > 1 Then
                    SatırArası = e.MarginBounds.Height / EtiketSatırSayısı
                    For i As Single = 1 To EtiketSatırSayısı - 1
                        e.Graphics.DrawLine(MyPen, e.MarginBounds.Left, SatırArası * i + e.MarginBounds.Top, e.MarginBounds.Width + e.MarginBounds.Left, SatırArası * i + e.MarginBounds.Top)
                    Next
                End If
                If EtiketSutunSayısı > 1 Then
                    SutunArası = e.MarginBounds.Width / EtiketSutunSayısı
                    For i As Single = 1 To EtiketSutunSayısı - 1
                        e.Graphics.DrawLine(MyPen, SutunArası * i + e.MarginBounds.Left, e.MarginBounds.Top, SutunArası * i + e.MarginBounds.Left, e.MarginBounds.Height + e.MarginBounds.Top)
                    Next
                End If
            End If

            If PrintAllPage Then
                Dim Row As DataRow

                For Str = 1 To EtiketSatırSayısı
                    For Stn = 1 To EtiketSutunSayısı
                        PrintLabelNum += 1

                        If MyDataTable.Rows.Count > 0 Then
                            Row = MyDataTable.Rows(PrintLabelNum - 1)

                            For Each Itm As LabelProject.LabelItem In Me

                                If Itm.Type = LabelProject.LabelItem.ItemType.Text Or
                                    Itm.Type = LabelProject.LabelItem.ItemType.Barcode Then

                                    If Itm.DataField.Length > 0 Then
                                        If IsDBNull(Row(Itm.DataField)) Then
                                            Itm.Text = ""
                                        Else
                                            Itm.Text = Row(Itm.DataField)
                                        End If
                                    End If

                                End If
                            Next
                        End If

                        SetBirlesikAlanlar()
                        SetBarkodText()

                        For Each Itm As LabelProject.LabelItem In Me
                            Itm.Yazdır(e.Graphics, (Stn - 1) * SutunArası + e.MarginBounds.Left, (Str - 1) * SatırArası + e.MarginBounds.Top)
                        Next

                        If PrintLabelNum >= MyDataTable.Rows.Count Then
                            e.HasMorePages = False
                            Exit Sub
                        End If
                    Next
                Next

                If PrintLabelNum < MyDataTable.Rows.Count Then
                    e.HasMorePages = True
                Else
                    e.HasMorePages = False
                End If

            Else

                SetBirlesikAlanlar()
                SetBarkodText()

                For Each Itm As LabelProject.LabelItem In Me
                    Itm.Yazdır(e.Graphics, e.MarginBounds.Left, e.MarginBounds.Top)

                    ' seçili öğeyi kırmızı çerçeveye alma 
                    If (Not mSelectedItm Is Nothing) AndAlso Itm.Name = mSelectedItm.Name Then
                        e.Graphics.DrawRectangle(New Pen(Color.Red, 1), Itm.x1 * mm - 1 + e.MarginBounds.Left, Itm.y1 * mm - 1 + e.MarginBounds.Top, Itm.Width * mm + 2, Itm.Height * mm + 2)
                    End If
                Next
            End If
        End Sub

        Public Function PageSetup() As PageSettings
            Dim PS As PageSettings = mPrintDoc.DefaultPageSettings

            With mPageSetupDialog
                .PageSettings = PS

                If .ShowDialog() = DialogResult.OK Then
                    PS = .PageSettings
                    mPrintDoc.DefaultPageSettings = PS
                End If

            End With

            Return PS

        End Function

        Public Function SetupDialog() As Boolean

            Using SD As New SetupDialog
                SD.PrintDoc = mPrintDoc
                SD.ShowDialog()
            End Using

            Return True
        End Function

#Region " XML Oku - Yaz"

        Public Sub WriteXML(File As String)

            Dim ColorConv As New ColorConverter

            Dim settings As New XmlWriterSettings()
            settings.Indent = True

            Try

                If IO.File.Exists(File) Then IO.File.Delete(File)

                Using XmlWrt As XmlWriter = XmlWriter.Create(File, settings)

                    With XmlWrt

                        .WriteStartDocument()
                        .WriteComment(Açıklama)

                        .WriteStartElement("Etiket")

                        .WriteStartElement("PageSettings")
                        With PrintDoc.DefaultPageSettings
                            XmlWrt.WriteAttributeString("PaperName", .PaperSize.PaperName)
                            XmlWrt.WriteAttributeString("PaperKind", .PaperSize.Kind.ToString)
                            XmlWrt.WriteAttributeString("PaperHeight", .PaperSize.Height.ToString)
                            XmlWrt.WriteAttributeString("PaperWidth", .PaperSize.Width.ToString)
                            XmlWrt.WriteAttributeString("PaperRawKind", .PaperSize.RawKind.ToString)
                            XmlWrt.WriteAttributeString("TopMargin", .Margins.Top.ToString)
                            XmlWrt.WriteAttributeString("BottomMargin", .Margins.Bottom.ToString)
                            XmlWrt.WriteAttributeString("LeftMargin", .Margins.Left.ToString)
                            XmlWrt.WriteAttributeString("RightMargin", .Margins.Right.ToString)
                            XmlWrt.WriteAttributeString("Landscape", .Landscape.ToString)
                        End With
                        .WriteEndElement() 'PageSettings

                        .WriteStartElement("Data")
                        .WriteAttributeString("DSN", DSNName)
                        .WriteAttributeString("SQL", SQLString)
                        .WriteEndElement()  'Data

                        .WriteStartElement("Parametre")
                        For Each Rw As DataRow In ParameterTable.Rows
                            .WriteStartElement("ParametreItem")
                            .WriteAttributeString("P_Name", Rw("PName"))
                            .WriteAttributeString("P_Value", Rw("PValue"))
                            .WriteEndElement() 'ParametreItem
                        Next
                        .WriteEndElement()  'Parametre

                        .WriteStartElement("EtiketYerleşim")
                        .WriteAttributeString("ÇokluEtiket", CokluEtiket.ToString)
                        .WriteAttributeString("SatırSayısı", EtiketSatırSayısı)
                        .WriteAttributeString("SutunSayısı", EtiketSutunSayısı)
                        .WriteEndElement() 'EtiketYerleşim

                        .WriteStartElement("LabelElements")
                        For Each Itm As LabelItem In List.Values
                            .WriteStartElement("Item")

                            .WriteAttributeString("Name", Itm.Name)
                            .WriteAttributeString("Text", Itm.Text)
                            .WriteAttributeString("Type", Itm.Type.ToString)
                            .WriteAttributeString("x1", Itm.x1.ToString)
                            .WriteAttributeString("y1", Itm.y1.ToString)
                            .WriteAttributeString("Width", Itm.Width.ToString)
                            .WriteAttributeString("Height", Itm.Height.ToString)
                            .WriteAttributeString("BarcodePrefix", Itm.BarcodePrefixChar)
                            .WriteAttributeString("BarcodeType", Itm.BarcodeType.ToString)
                            .WriteAttributeString("BarcodeText", Itm.BarcodeText.ToString)
                            .WriteAttributeString("TextAlign", Itm.TextAlign.ToString)
                            .WriteAttributeString("LineColor", ColorConv.ConvertToString(Itm.LineColor))
                            .WriteAttributeString("LineWidth", Itm.LineWidth.ToString)
                            .WriteAttributeString("DataField", Itm.DataField)
                            .WriteAttributeString("BarkodVeriAlanı", Itm.BarkodVeriAlanı)
                            .WriteAttributeString("RefAlanListesi", Itm.RefAlanListesi)

                            .WriteStartElement("Font")
                            .WriteAttributeString("FontName", Itm.MyFont.Name)
                            .WriteAttributeString("Size", Itm.MyFont.Size)
                            .WriteAttributeString("Bold", Itm.MyFont.Bold)
                            .WriteAttributeString("Italic", Itm.MyFont.Italic)
                            .WriteEndElement() 'Font

                            '********** resim kaydet
                            If Not IsNothing(Itm.Image) Then

                                Dim bmp As Bitmap = Itm.Image
                                Dim MS As New MemoryStream

                                bmp.Save(MS, Drawing.Imaging.ImageFormat.Bmp)

                                Dim Bytes() As Byte = MS.ToArray

                                .WriteStartElement("Resim")
                                .WriteBase64(Bytes, 0, Bytes.Length)
                                .WriteEndElement()  'Resim
                            End If

                            .WriteEndElement() 'Item
                        Next

                        .WriteEndElement() 'LabelElements

                        .WriteEndElement() 'Etiket

                        .WriteEndDocument()
                        .Flush()
                        .Close()

                    End With

                End Using

                MessageBox.Show(File & " XML Dosyası Kaydedildi", "Dosya Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "XML Write Error")
            End Try
        End Sub

        Public Sub ReadXML(File As String)

            Dim LblItm As LabelItem = Nothing
            Dim XR As XmlTextReader = Nothing

            Try
                If (IO.File.Exists(File)) Then

                    XR = New XmlTextReader(File)
                    Me.Clear()

                    While XR.Read()
                        Select Case XR.NodeType
                            Case XmlNodeType.XmlDeclaration
                            Case XmlNodeType.Whitespace

                            Case XmlNodeType.Comment
                                Açıklama = XR.Value

                            Case XmlNodeType.Element

                                Select Case XR.Name
                                    Case "Etiket"

                                    Case "PageSettings"
                                        ReadPaperInfo(XR)

                                    Case "Data"
                                        If XR.HasAttributes Then
                                            While XR.MoveToNextAttribute
                                                Select Case XR.Name
                                                    Case "DSN"
                                                        DSNName = XR.Value
                                                    Case "SQL"
                                                        SQLString = XR.Value
                                                End Select
                                            End While
                                        End If

                                    Case "ParametreItem"
                                        If XR.HasAttributes Then
                                            Dim PN As String = ""
                                            Dim PV As String = ""
                                            While XR.MoveToNextAttribute
                                                Select Case XR.Name
                                                    Case "P_Name"
                                                        PN = XR.Value
                                                    Case "P_Value"
                                                        PV = XR.Value
                                                End Select
                                            End While
                                            ParameterTable.Rows.Add(PN, PV)
                                        End If

                                    Case "EtiketYerleşim"
                                        If XR.HasAttributes Then
                                            While XR.MoveToNextAttribute
                                                Select Case XR.Name
                                                    Case "ÇokluEtiket"
                                                        CokluEtiket = CBool(XR.Value)
                                                    Case "SatırSayısı"
                                                        EtiketSatırSayısı = CInt(XR.Value)
                                                    Case "SutunSayısı"
                                                        EtiketSutunSayısı = CInt(XR.Value)
                                                End Select
                                            End While
                                        End If

                                        '.WriteStartElement("")
                                        '.WriteAttributeString(, CokluEtiket.ToString)
                                        '.WriteAttributeString(, EtiketSatırSayısı)
                                        '.WriteAttributeString(, EtiketSutunSayısı)

                                    Case "LabelElements"

                                    Case "Item"
                                        LblItm = ReadLabelItmFromXml(XR)
                                        List.Add(LblItm.ItemNumber, LblItm)

                                    Case "Font"
                                        If Not IsNothing(LblItm) Then
                                            LblItm.MyFont = ReadFontXml(XR)
                                        End If

                                    Case "Resim"
                                        If Not IsNothing(LblItm) Then
                                            LblItm.Image = ReadImageFromXml(XR)
                                        End If
                                End Select
                        End Select
                    End While
                Else
                    MessageBox.Show("The filename you selected was not found.")
                End If

            Catch ex As Exception
                MsgBox(ex.Message & " - " & XR.Name, MsgBoxStyle.Critical, "XML Read Error ")
                Me.Clear()
            End Try
        End Sub

        Private Function ReadFontXml(FontXml As XmlReader) As Font
            Dim FontName As String = "Arial"
            Dim FontSize As Integer = 10
            Dim Bold As Boolean = False
            Dim Italik As Boolean = False

            If FontXml.HasAttributes Then
                While FontXml.MoveToNextAttribute
                    Select Case FontXml.Name
                        Case "FontName"
                            FontName = FontXml.Value
                        Case "Size"
                            FontSize = CInt(FontXml.Value)
                        Case "Bold"
                            Bold = CBool(FontXml.Value)
                        Case "Italic"
                            Italik = CBool(FontXml.Value)
                    End Select
                End While
            End If

            Dim FStyle As FontStyle = 0

            If Bold Then FStyle = FStyle Or FontStyle.Bold
            If Italik Then FStyle = FStyle Or FontStyle.Italic

            Return New Font(FontName, FontSize, FStyle)
        End Function

        Private Function ReadLabelItmFromXml(LblXml As XmlReader) As LabelItem
            Dim Itm As New LabelItem

            If LblXml.HasAttributes Then
                While LblXml.MoveToNextAttribute
                    Select Case LblXml.Name
                        Case "Name"
                            Itm.Name = LblXml.Value
                        Case "Text"
                            Itm.Text = LblXml.Value
                        Case "Type"
                            Select Case LblXml.Value
                                Case "Label"
                                    Itm.Type = LabelItem.ItemType.Label
                                Case "Barcode"
                                    Itm.Type = LabelItem.ItemType.Barcode
                                Case "Box"
                                    Itm.Type = LabelItem.ItemType.Box
                                Case "Text"
                                    Itm.Type = LabelItem.ItemType.Text
                                Case "Line"
                                    Itm.Type = LabelItem.ItemType.Line
                                Case "Image"
                                    Itm.Type = LabelItem.ItemType.Image
                            End Select
                        Case "x1"
                            Itm.x1 = CSng(LblXml.Value)
                        Case "y1"
                            Itm.y1 = CSng(LblXml.Value)
                        Case "Width"
                            Itm.Width = CSng(LblXml.Value)
                        Case "Height"
                            Itm.Height = CSng(LblXml.Value)
                        Case "BarcodePrefix"
                            Itm.BarcodePrefixChar = LblXml.Value
                        Case "BarcodeType"

                            Select Case LblXml.Value
                                Case "Code39"
                                    Itm.BarcodeType = LabelItem.BarcodeTypes.Code39
                                Case "Code128"
                                    Itm.BarcodeType = LabelItem.BarcodeTypes.Code128
                                Case "EAN_13"
                                    Itm.BarcodeType = LabelItem.BarcodeTypes.EAN_13
                                Case "QR"
                                    Itm.BarcodeType = LabelItem.BarcodeTypes.QR
                                Case "DATA_MATRIX"
                                    Itm.BarcodeType = LabelItem.BarcodeTypes.DATA_MATRIX
                            End Select

                        Case "BarcodeText"
                            Select Case LblXml.Value
                                Case "True"
                                    Itm.BarcodeText = True
                                Case "False"
                                    Itm.BarcodeText = False
                            End Select

                        Case "TextAlign"
                            Select Case LblXml.Value
                                Case "BottomCenter"
                                    Itm.TextAlign = ContentAlignment.BottomCenter
                                Case "BottomLeft"
                                    Itm.TextAlign = ContentAlignment.BottomLeft
                                Case "BottomRight"
                                    Itm.TextAlign = ContentAlignment.BottomRight
                                Case "MiddleCenter"
                                    Itm.TextAlign = ContentAlignment.MiddleCenter
                                Case "MiddleLeft"
                                    Itm.TextAlign = ContentAlignment.MiddleLeft
                                Case "MiddleRight"
                                    Itm.TextAlign = ContentAlignment.MiddleRight
                                Case "TopCenter"
                                    Itm.TextAlign = ContentAlignment.TopCenter
                                Case "TopLeft"
                                    Itm.TextAlign = ContentAlignment.TopLeft
                                Case "TopRight"
                                    Itm.TextAlign = ContentAlignment.TopRight
                                Case Else
                                    Itm.TextAlign = ContentAlignment.TopLeft
                            End Select
                        Case "LineColor"
                            Dim ColorConv As New ColorConverter
                            Itm.LineColor = ColorConv.ConvertFromString(LblXml.Value)
                        Case "LineWidth"
                            Itm.LineWidth = CSng(LblXml.Value)
                        Case "DataField"
                            Itm.DataField = LblXml.Value
                        Case "BarkodVeriAlanı"
                            Itm.BarkodVeriAlanı = LblXml.Value
                        Case "RefAlanListesi"
                            Itm.RefAlanListesi = LblXml.Value
                    End Select
                End While
            End If

            Return Itm
        End Function

        Private Function ReadPaperInfo(Xr As XmlReader)
            Dim PgSettings As New Printing.PageSettings

            Dim name As String = ""
            Dim kind As String = ""
            Dim height As Integer = 0
            Dim width As Integer = 0
            Dim rawkind As Integer = 0
            Dim top As Integer = 0
            Dim bottom As Integer = 0
            Dim left As Integer = 0
            Dim right As Integer = 0
            Dim landscape As Boolean = False

            If Xr.HasAttributes Then
                While Xr.MoveToNextAttribute

                    Select Case Xr.Name
                        Case "PaperName"
                            name = Xr.Value
                        Case "PaperKind"
                            kind = Xr.Value
                        Case "PaperHeight"
                            If IsNumeric(Xr.Value) Then height = CInt(Xr.Value)
                        Case "PaperWidth"
                            If IsNumeric(Xr.Value) Then width = CInt(Xr.Value)
                        Case "PaperRawKind"
                            If IsNumeric(Xr.Value) Then rawkind = CInt(Xr.Value)
                        Case "TopMargin"
                            If IsNumeric(Xr.Value) Then top = CInt(Xr.Value)
                        Case "BottomMargin"
                            If IsNumeric(Xr.Value) Then bottom = CInt(Xr.Value)
                        Case "LeftMargin"
                            If IsNumeric(Xr.Value) Then left = CInt(Xr.Value)
                        Case "RightMargin"
                            If IsNumeric(Xr.Value) Then right = CInt(Xr.Value)
                        Case "Landscape"
                            landscape = IIf(Xr.Value = "True", True, False)
                    End Select
                End While
            End If

            Try
                With PgSettings

                    If kind = "Custom" Then 'Printing.PaperKind.Custom 
                        Dim PS As New PaperSize
                        With PS
                            .PaperName = name
                            .Height = height
                            .Width = width
                            .RawKind = rawkind
                        End With

                        .PaperSize = PS

                    Else
                        For Each Paper As PaperSize In PrintDoc.PrinterSettings.PaperSizes 'PaperKindList
                            If Paper.PaperName = name Then
                                .PaperSize = Paper
                                Exit For
                            End If
                        Next
                    End If

                    With .Margins
                        .Top = top
                        .Bottom = bottom
                        .Left = left
                        .Right = right
                    End With

                    .Landscape = landscape
                End With

                'For Each Pp As PaperSize In PrintDocument1.PrinterSettings.PaperSizes
                '	If Pp.PaperName = Paper Then
                '		PrintDocument1.DefaultPageSettings.PaperSize = Pp
                '		Exit For
                '	End If
                'Next

                PrintDoc.DefaultPageSettings = PgSettings

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            Return ""
        End Function

        Private Function ReadImageFromXml(ImgXml As XmlReader) As Image
            Dim img As Image = Nothing
            Dim Bytes() As Byte = Nothing

            If ImgXml.Read Then
                Bytes = Convert.FromBase64String(ImgXml.Value)
                img = Image.FromStream(New MemoryStream(Bytes))
            End If

            Return img
        End Function

        'Örnek Kod
        '    Public Shared Sub Base64EncodeImageFile()
        '
        '        Dim bufferSize As Integer = 1000
        '        Dim buffer(bufferSize) As Byte
        '        Dim readBytes As Integer = 0
        '
        '        Using writer As XmlWriter = XmlWriter.Create("output.xml")
        '
        '            Dim inputFile As New FileStream("C:\artFiles\sunset.jpg", FileMode.OpenOrCreate, FileAccess.Read, FileShare.Read)
        '            writer.WriteStartDocument()
        '            writer.WriteStartElement("image")
        '
        '            Dim br As New BinaryReader(inputFile)
        '            'Console.WriteLine(vbCr + vbLf + "Writing Base64 data...")
        '
        '            Do
        '                readBytes = br.Read(buffer, 0, bufferSize)
        '                writer.WriteBase64(buffer, 0, readBytes)
        '            Loop While bufferSize <= readBytes
        '            br.Close()
        '
        '            writer.WriteEndElement() ' </image>
        '            writer.WriteEndDocument()
        '
        '        End Using

        ' End Sub 'Base64EncodeImageFile

#End Region

#Region "Read-Write PaperSettings to Registry"
        Public Sub WritePaperSettingToReg(AppName As String, Optional Section As String = "PaperSettings")
            With PrintDoc.DefaultPageSettings
                SaveSetting(AppName, Section, "PaperName", .PaperSize.PaperName)
                SaveSetting(AppName, Section, "PaperKind", .PaperSize.Kind.ToString)
                SaveSetting(AppName, Section, "PaperHeight", .PaperSize.Height.ToString)
                SaveSetting(AppName, Section, "PaperWidth", .PaperSize.Width.ToString)
                SaveSetting(AppName, Section, "PaperRawKind", .PaperSize.RawKind.ToString)
                SaveSetting(AppName, Section, "TopMargin", .Margins.Top.ToString)
                SaveSetting(AppName, Section, "BottomMargin", .Margins.Bottom.ToString)
                SaveSetting(AppName, Section, "LeftMargin", .Margins.Left.ToString)
                SaveSetting(AppName, Section, "RightMargin", .Margins.Right.ToString)
                SaveSetting(AppName, Section, "Landscape", .Landscape.ToString)
            End With
        End Sub

        Public Sub ReadPaperSettingFromReg(AppName As String,
                                           Optional Section As String = "PaperSettings",
                                           Optional DefPS As PageSettings = Nothing)

            If DefPS Is Nothing Then
                'Throw New ArgumentNullException(NameOf(DefaultSettings))
                DefPS = New PageSettings With
                    {.PaperSize = New PaperSize With {.PaperName = "A4", .RawKind = 9},
                    .Landscape = False,
                    .Margins = New Margins With {.Top = 39, .Bottom = 39, .Left = 39, .Right = 39}
                }
            End If

            Dim PgSettings As New Printing.PageSettings

            Dim name As String = GetSetting(AppName, Section, "PaperName", DefPS.PaperSize.PaperName)
            Dim kind As String = GetSetting(AppName, Section, "PaperKind", DefPS.PaperSize.Kind)
            Dim height As Integer = GetSetting(AppName, Section, "PaperHeight", DefPS.PaperSize.Height)
            Dim width As Integer = GetSetting(AppName, Section, "PaperWidth", DefPS.PaperSize.Width)
            Dim rawkind As Integer = GetSetting(AppName, Section, "PaperRawKind", DefPS.PaperSize.RawKind)
            Dim top As Integer = GetSetting(AppName, Section, "TopMargin", DefPS.Margins.Top)
            Dim bottom As Integer = GetSetting(AppName, Section, "BottomMargin", DefPS.Margins.Bottom)
            Dim left As Integer = GetSetting(AppName, Section, "LeftMargin", DefPS.Margins.Left)
            Dim right As Integer = GetSetting(AppName, Section, "RightMargin", DefPS.Margins.Right)
            Dim landscape As Boolean = GetSetting(AppName, Section, "Landscape", DefPS.Landscape)

            Try
                With PgSettings

                    If kind = "Custom" Then 'Printing.PaperKind.Custom 
                        Dim PS As New PaperSize
                        With PS
                            .PaperName = name
                            .Height = height
                            .Width = width
                            .RawKind = rawkind
                        End With

                        .PaperSize = PS

                    Else
                        For Each Paper As PaperSize In PrintDoc.PrinterSettings.PaperSizes 'PaperKindList
                            If Paper.PaperName = name Then
                                .PaperSize = Paper
                                Exit For
                            End If
                        Next
                    End If

                    With .Margins
                        .Top = top
                        .Bottom = bottom
                        .Left = left
                        .Right = right
                    End With

                    .Landscape = landscape
                End With

                PrintDoc.DefaultPageSettings = PgSettings

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End Sub


#End Region

        Public Class ParameterTableClass
            Inherits DataTable

            Public Sub New()
                With Me.Columns
                    .Add("PName")
                    .Add("PValue")
                End With
            End Sub
        End Class
    End Class

    Public Class LabelItem

        Public Enum ItemType As Byte
            Label
            Text
            Barcode
            Box
            Line
            Image
        End Enum

        Public Enum BarcodeTypes As Byte
            Code39
            Code128
            EAN_13
            QR
            DATA_MATRIX
        End Enum

#Region " Properties"
        Public ItemNumber As Integer

        <CategoryAttribute("Genel"), Description("Nesne Adı")>
        Public Property Name As String

        <CategoryAttribute("Genel"), Description("Nesne Tipi")>
        Public Property Type As ItemType = ItemType.Label

        <CategoryAttribute("Text"), Description("Nesne verisi")>
        Public Property Text As String = "New Item"

        '<TypeConverter(GetType(DataFieldList)), CategoryAttribute("Data"), DescriptionAttribute("Nesne Data alanı")> _
        <CategoryAttribute("Data"), DescriptionAttribute("Nesne Data alanı")>
        Public Property DataField As String = ""

        <Category("Data"), Description("Birleştirilecek alanların listesi")>
        Public Property RefAlanListesi As String = ""

        <CategoryAttribute("Yer"), Description("Nesne Sol üst köşe X koordinatı")>
        Public Property x1 As Single = 10

        <CategoryAttribute("Yer"), Description("Nesne Sol üst köşe Y koordinatı")>
        Public Property y1 As Single = 10

        <CategoryAttribute("Boyut"), Description("Nesne genişliği")>
        Public Property Width As Single = 100

        <CategoryAttribute("Boyut"), Description("Nesne yüksekliği")>
        Public Property Height As Single = 10

        <CategoryAttribute("Barkod"), Description("Barkod için Prefix karakteri")>
        Public Property BarcodePrefixChar As String = ""

        <CategoryAttribute("Text"), Description("Nesne yazı fontu")>
        Public Property MyFont As Font = New Font("Arial", 10)

        <CategoryAttribute("Text"), Description("Yazı hizalama şekli")>
        Public Property TextAlign As ContentAlignment = ContentAlignment.TopLeft

        <CategoryAttribute("Çizgi"), Description("Nesne çizgi rengi")>
        Public Property LineColor As Color = Color.Black
        <CategoryAttribute("Çizgi"), Description("Nesne çizgi kalınlığı")> Public Property LineWidth As Single = 1
        <CategoryAttribute("Barkod"), Description("Barkod Tipi")> Public Property BarcodeType As BarcodeTypes

        '<TypeConverter(GetType(VeriFieldList)),
        'CategoryAttribute("Barkod"),
        'DescriptionAttribute("Barkod veri alanı")> _

        <CategoryAttribute("Barkod"), DescriptionAttribute("Barkod veri alanı")> Public Property BarkodVeriAlanı As String = ""

        <CategoryAttribute("Barkod"), Description("Barkod alt yazı")>
        Public Property BarcodeText As Boolean = True
        <CategoryAttribute("Resim"), Description("Resim")> Public Property Image As Image

        Public Grf As Drawing.Graphics
#End Region

        Public Sub New(Optional ItemType As ItemType = ItemType.Label, Optional Ad As String = vbNullString)
            ItemCount += 1
            ItemNumber = ItemCount
            Type = ItemType

            If Ad = vbNullString Then
                Select Case ItemType
                    Case ItemType.Label
                        Name = "Etiket Alanı " & ItemCount.ToString
                    Case LabelItem.ItemType.Text
                        Name = "Yazı Alanı" & ItemCount.ToString
                    Case LabelItem.ItemType.Barcode
                        Name = "Barkod Alanı " & ItemCount.ToString
                    Case LabelItem.ItemType.Box
                        Name = "Kutu " & ItemCount.ToString
                    Case LabelItem.ItemType.Line
                        Name = "Cizgi " & ItemCount.ToString
                    Case LabelItem.ItemType.Image
                        Name = "Resim " & ItemCount.ToString
                End Select
            Else
                Name = Ad
            End If

        End Sub

        Public Sub Yazdır(Grafics As Drawing.Graphics, X0 As Single, Y0 As Single)

            Grf = Grafics

            Select Case Type
                Case ItemType.Label
                    DrawText(X0, Y0)
                Case ItemType.Barcode
                    DrawBarcode(X0, Y0)
                Case ItemType.Text
                    DrawText(X0, Y0)
                Case ItemType.Box
                    DrawBox(X0, Y0)
                Case ItemType.Line
                    DrawLine(X0, Y0)
                Case ItemType.Image
                    DrawImage(X0, Y0)
            End Select
        End Sub

#Region "Drawing Items"
        Private Sub DrawText(X0 As Single, Y0 As Single)
            Dim prFont As Font = MyFont
            Dim Sf As New StringFormat

            'X hesapla
            Select Case TextAlign
                Case ContentAlignment.BottomLeft, ContentAlignment.MiddleLeft, ContentAlignment.TopLeft
                    Sf.Alignment = StringAlignment.Near
                Case ContentAlignment.BottomRight, ContentAlignment.MiddleRight, ContentAlignment.TopRight
                    Sf.Alignment = StringAlignment.Far
                Case ContentAlignment.BottomCenter, ContentAlignment.MiddleCenter, ContentAlignment.TopCenter
                    Sf.Alignment = StringAlignment.Center
            End Select

            'Y Hesapla
            Select Case TextAlign
                Case ContentAlignment.BottomCenter, ContentAlignment.BottomLeft, ContentAlignment.BottomRight
                    Sf.LineAlignment = StringAlignment.Far
                Case ContentAlignment.MiddleCenter, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight
                    Sf.LineAlignment = StringAlignment.Center
                Case ContentAlignment.TopCenter, ContentAlignment.TopLeft, ContentAlignment.TopRight
                    Sf.LineAlignment = StringAlignment.Near
            End Select

            Sf.FormatFlags = Sf.FormatFlags And StringFormatFlags.NoWrap

            Dim Layout As New RectangleF(x1 * mm + X0, y1 * mm + Y0, Width * mm, Height * mm)

            Grf.DrawString(Text, prFont, Brushes.Black, Layout, New StringFormat(Sf))

            'StringFormatFlags.DirectionRightToLeft = 1
            'StringFormatFlags.DirectionVertical = 2
            'StringFormatFlags.FitBlackBox = 4
            'StringFormatFlags.DisplayFormatControl = 32
            'StringFormatFlags.NoFontFallback = 1024
            'StringFormatFlags.MeasureTrailingSpaces = 2048
            'StringFormatFlags.NoWrap = 4096
            'StringFormatFlags.LineLimit = 8192
            'StringFormatFlags.NoClip = 16384
        End Sub

        Private Sub DrawBarcode(X0 As Single, Y0 As Single)
            Dim Img As Image = Nothing

            Dim BCode As New ZXing.BarcodeWriter

            Dim TmpText As String = BarcodePrefixChar & Text.TrimEnd

            Select Case BarcodeType
                Case BarcodeTypes.Code128

                    Try
                        BCode.Format = ZXing.BarcodeFormat.CODE_128
                        BCode.Options.Height = Height * mm
                        BCode.Options.PureBarcode = BarcodeText
                        Img = BCode.Write(TmpText)
                    Catch ex As Exception
                        Img = Nothing
                    End Try

                Case BarcodeTypes.Code39

                    Try
                        BCode.Format = ZXing.BarcodeFormat.CODE_39
                        BCode.Options.Height = Height * mm
                        BCode.Options.PureBarcode = BarcodeText
                        Img = BCode.Write(TmpText)
                    Catch ex As Exception
                        Img = Nothing
                    End Try
                Case BarcodeTypes.EAN_13

                    Try
                        BCode.Format = ZXing.BarcodeFormat.EAN_13

                        BCode.Options.Height = Height * mm
                        BCode.Options.Width = Width * mm
                        BCode.Options.PureBarcode = BarcodeText
                        If (TmpText).Length = 12 Then
                            Img = BCode.Write(TmpText)
                        ElseIf (TmpText).Length > 12 Then
                            Img = BCode.Write((TmpText).Substring(0, 12))
                        Else
                            Img = Nothing
                        End If

                    Catch ex As Exception
                        Img = Nothing
                    End Try

                Case BarcodeTypes.QR
                    Try
                        BCode.Format = ZXing.BarcodeFormat.QR_CODE
                        BCode.Options.Height = Height * mm
                        BCode.Options.Width = Width * mm
                        Img = BCode.Write(TmpText)
                    Catch ex As Exception
                        Img = Nothing
                    End Try

                Case BarcodeTypes.DATA_MATRIX
                    Try
                        BCode.Format = ZXing.BarcodeFormat.DATA_MATRIX
                        BCode.Options.Height = Height * mm
                        BCode.Options.Width = Width * mm
                        Img = BCode.Write(TmpText)
                    Catch ex As Exception
                        Img = Nothing
                    End Try

            End Select
            Try
                If Not IsNothing(Img) Then Grf.DrawImage(Img, x1 * mm + X0, y1 * mm + Y0)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "Barkod Yazdırma Hatası")
            End Try

        End Sub

        Private Sub DrawBox(X0 As Single, Y0 As Single)
            Dim LineBrush As Brush = New SolidBrush(LineColor)
            Dim LinePen As Pen = New Pen(LineBrush)
            LinePen.Width = LineWidth

            Grf.DrawRectangle(LinePen, x1 * mm + X0, y1 * mm + Y0, Width * mm, Height * mm)
        End Sub

        Private Sub DrawLine(X0 As Single, Y0 As Single)
            Dim LineBrush As Brush = New SolidBrush(LineColor)
            Dim LinePen As Pen = New Pen(LineBrush)
            LinePen.Width = LineWidth

            Grf.DrawLine(LinePen, x1 * mm + X0, y1 * mm + Y0, (x1 + Width) * mm + X0, (y1 + Height) * mm + Y0)
        End Sub

        Private Sub DrawImage(X0 As Single, Y0 As Single)
            Dim Img As Image = Me.Image
            If Not IsNothing(Img) Then Grf.DrawImage(Img, x1 * mm + X0, y1 * mm + Y0, Width * mm, Height * mm)
        End Sub
#End Region

    End Class

    Public Class DataFieldList
        Inherits StringConverter

        Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As TypeConverter.StandardValuesCollection
            Dim MyList As New List(Of String)

            'For Each T As String In Frm_Main.FieldList
            '    MyList.Add(T)
            'Next

            Return New StandardValuesCollection(MyList)
        End Function

        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function
    End Class

    Public Class VeriFieldList
        Inherits StringConverter

        Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As TypeConverter.StandardValuesCollection
            Dim MyList As New List(Of String)
            Return New StandardValuesCollection(MyList)
        End Function

        Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

        Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
            Return True
        End Function

    End Class


    'https://learn.microsoft.com/en-us/dotnet/api/system.drawing.printing.papersize.rawkind?view=net-8.0

End Class


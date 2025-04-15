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
    Const DefaultFontSize As Integer = 10
    Const DefaultMargin As Integer = 39
    Const DashLineWidth As Single = 1
    Const DefaultLineWidth As Single = 1
    Const DefaultBarcodeHeight As Integer = 100
    Const DefaultBarcodeWidth As Integer = 100

    Public Class Label
        Implements IEnumerable, IEnumerator

        ' Label sınıfı, etiket yazdırma işlemleri için temel işlevselliği sağlar.
        ' Bu sınıf, etiket öğelerini yönetir ve yazdırma işlemlerini gerçekleştirir.

        ' Etiket öğelerini tutan bir sözlük.
        Private List As New System.Collections.Specialized.OrderedDictionary 'önceki versiyon
        'Private List As New Dictionary(Of String, LabelItem)

        ' Yazdırma işlemi sırasında kullanılan pozisyon.
        Private Position As Integer = -1


        ' Yazdırma belgesi ve sayfa ayarları için kullanılan nesneler.
        Private WithEvents mPrintDoc As New Printing.PrintDocument
        Private WithEvents mPageSetupDialog As New PageSetupDialog With {.Document = mPrintDoc, .EnableMetric = True}

        Private _printDocument As IPrintDocument = New PrintDocumentWrapper(mPrintDoc)

        ' Çoklu etiket yazdırma işlemi için kullanılan bayraklar ve sayaçlar.
        Private PrintAllPage As Boolean = False
        Private PrintPageNum As Integer = 0
        Private PrintLabelNum As Integer = 0


        ' Seçili etiket öğesi.
        Private mSelectedItm As LabelItem

        ' Log dosyasının konumu. Varsayılan olarak uygulama ile aynı klasörde.
        Private _logFilePath As String = IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog.txt")

        ' Kullanıcıya JSON veya XML formatında okuma/yazma seçeneği sunmak için bir özellik.
        Public Enum FileFormat
            XML
            JSON
        End Enum
        Public Property PreferredFileFormat As FileFormat = FileFormat.XML

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
                If List.Count > 0 Then
                    If Position >= 0 AndAlso Position < List.Count Then
                        'Dim keyAtPosition As String = List.Keys.ElementAt(Position)
                        'Return List(keyAtPosition)
                        Return CType(List(Position), LabelItem)
                    Else
                        Return Nothing
                    End If
                Else
                    Return Nothing
                End If
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

        Private Sub AddItemToList(item As LabelItem)
            Try
                List.Add(item.Name, item)
            Catch ex As Exception
                HandleError("Error adding item to list.", ex)
            End Try
        End Sub

        Public Sub AddText(Txt As String,
                           x As Single, y As Single,
                           W As Single, H As Single,
                           Optional FSize As Integer = DefaultFontSize,
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

            AddItemToList(NewItm)

            If Box Then AddBox(x, y, W, H, Color.Black, DefaultLineWidth)
        End Sub

        Public Sub AddBox(x As Single, y As Single,
                           W As Single, H As Single,
                           Renk As Color,
                           Optional LineW As Single = DefaultLineWidth,
                          Optional ItemName As String = vbNullString)

            Dim NewItm As New LabelItem(LabelItem.ItemType.Box, ItemName)
            With NewItm
                .LineWidth = LineW
                .LineColor = Renk
                .x1 = x
                .y1 = y
                .Width = W
                .Height = H
            End With

            AddItemToList(NewItm)
        End Sub

        Public Sub AddLine(x As Single, y As Single,
                           W As Single, H As Single,
                           Renk As Color,
                           Optional LineW As Single = DefaultLineWidth,
                           Optional ItemName As String = vbNullString)

            Dim NewItm As New LabelItem(LabelItem.ItemType.Line, ItemName)
            With NewItm
                .LineWidth = LineW
                .LineColor = Renk
                .x1 = x
                .y1 = y
                .Width = W
                .Height = H
            End With

            AddItemToList(NewItm)
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

            AddItemToList(NewItm)

            If Box Then AddBox(x, y, W, H, Color.Black, DefaultLineWidth)
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

            AddItemToList(NewItm)
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
            ' Dictionary'de RemoveAt metodu olmadığından, Index'e karşılık gelen anahtarı bulup Remove metodunu kullanıyoruz.
            If Index >= 0 AndAlso Index < List.Count Then
                'Dim keyToRemove = List.Keys.ElementAt(Index)
                'List.Remove(keyToRemove)
                List.RemoveAt(Index)
            Else
                Throw New ArgumentOutOfRangeException("Index", "Index is out of range.")
            End If
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

        Public Interface IPrintDocument
            Sub Print()
            Function GetDefaultPageSettings() As PageSettings
        End Interface

        Public Class PrintDocumentWrapper
            Implements IPrintDocument

            Private ReadOnly _printDocument As PrintDocument

            Public Sub New(printDocument As PrintDocument)
                _printDocument = printDocument
            End Sub

            Public Sub Print() Implements IPrintDocument.Print
                _printDocument.Print()
            End Sub

            Public Function GetDefaultPageSettings() As PageSettings Implements IPrintDocument.GetDefaultPageSettings
                Return _printDocument.DefaultPageSettings
            End Function
        End Class

        Public Sub SetPrintDocument(printDocument As IPrintDocument)
            _printDocument = printDocument
        End Sub

        Public Sub PrintAll()
            If PrintDlg.ShowDialog = DialogResult.OK Then
                PrintAllPage = True
                PrintPageNum = 0
                PrintLabelNum = 0

                _printDocument.Print()

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
                If Itm.Type = LabelItem.ItemType.Text AndAlso Itm.RefAlanListesi.Count > 0 Then
                    Itm.Text = ""

                    For Each Rf As String In Itm.RefAlanListesi

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

        ' Yazdırma işlemi sırasında kenar boşluklarını çizen metot.
        Private Sub DrawMarginsx(e As Printing.PrintPageEventArgs)
            Dim MarginPen As New Pen(Brushes.Blue) With {.DashStyle = Drawing2D.DashStyle.Dash}
            If DrawMargins Then
                e.Graphics.DrawRectangle(MarginPen, e.MarginBounds)
            End If
        End Sub

        ' Çoklu etiket yazdırma işlemi için ızgara çizgilerini çizen metot.
        Private Sub DrawGrid(e As Printing.PrintPageEventArgs)
            If CokluEtiket Then
                Dim MyPen As New Pen(Brushes.Black) With {.DashStyle = Drawing2D.DashStyle.DashDot}

                ' Satır çizgilerini çiz.
                If EtiketSatırSayısı > 1 Then
                    Dim SatırArası = e.MarginBounds.Height / EtiketSatırSayısı
                    For i As Single = 1 To EtiketSatırSayısı - 1
                        e.Graphics.DrawLine(MyPen,
                                            CSng(e.MarginBounds.Left),
                                            CSng(SatırArası * i + e.MarginBounds.Top),
                                            CSng(e.MarginBounds.Width + e.MarginBounds.Left),
                                            CSng(SatırArası * i + e.MarginBounds.Top))
                    Next
                End If

                ' Sütun çizgilerini çiz.
                If EtiketSutunSayısı > 1 Then
                    Dim SutunArası = e.MarginBounds.Width / EtiketSutunSayısı
                    For i As Single = 1 To EtiketSutunSayısı - 1
                        e.Graphics.DrawLine(MyPen,
                                            CSng(SutunArası * i + e.MarginBounds.Left),
                                            CSng(e.MarginBounds.Top),
                                            CSng(SutunArası * i + e.MarginBounds.Left),
                                            CSng(e.MarginBounds.Height + e.MarginBounds.Top))
                    Next
                End If
            End If
        End Sub

        ' Etiketleri yazdıran metot.
        Private Sub PrintLabels(e As Printing.PrintPageEventArgs)
            Dim SatırArası = e.MarginBounds.Height / EtiketSatırSayısı
            Dim SutunArası = e.MarginBounds.Width / EtiketSutunSayısı

            Dim Row As DataRow

            ' Satır ve sütun döngüsü ile etiketleri yazdır.
            For Strx = 1 To EtiketSatırSayısı
                For Stn = 1 To EtiketSutunSayısı
                    PrintLabelNum += 1

                    ' Veri tablosundan veri al ve etiketlere uygula.
                    If MyDataTable.Rows.Count > 0 Then
                        Row = MyDataTable.Rows(PrintLabelNum - 1)

                        For Each Itm As LabelProject.LabelItem In Me
                            If Itm.Type = LabelProject.LabelItem.ItemType.Text Or
                               Itm.Type = LabelProject.LabelItem.ItemType.Barcode Then

                                If Itm.DataField.ToString.Length > 0 Then
                                    If IsDBNull(Row(Itm.DataField)) Then
                                        Itm.Text = ""
                                    Else
                                        Itm.Text = Row(Itm.DataField)
                                    End If
                                End If
                            End If
                        Next
                    End If

                    ' Birleşik alanları ve barkod metinlerini ayarla.
                    SetBirlesikAlanlar()
                    SetBarkodText()

                    ' Etiket öğelerini yazdır.
                    For Each Itm As LabelProject.LabelItem In Me
                        Itm.Yazdır(e.Graphics, (Stn - 1) * SutunArası + e.MarginBounds.Left, (Strx - 1) * SatırArası + e.MarginBounds.Top)
                    Next

                    ' Tüm etiketler yazdırıldıysa çık.
                    If PrintLabelNum >= MyDataTable.Rows.Count Then
                        e.HasMorePages = False
                        Exit Sub
                    End If
                Next
            Next

            ' Daha fazla sayfa olup olmadığını kontrol et.
            If PrintLabelNum < MyDataTable.Rows.Count Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        End Sub

        ' Yazdırma işlemini yöneten ana metot.
        Private Sub mPrintDoc_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles mPrintDoc.PrintPage
            ' Kenar boşluklarını çiz.
            DrawMarginsx(e)

            ' Izgara çizgilerini çiz.
            DrawGrid(e)

            ' Tüm sayfaları yazdır veya yalnızca seçili öğeleri yazdır.
            If PrintAllPage Then
                PrintLabels(e)
            Else
                SetBirlesikAlanlar()
                SetBarkodText()

                For Each Itm As LabelProject.LabelItem In Me
                    Itm.Yazdır(e.Graphics, e.MarginBounds.Left, e.MarginBounds.Top)

                    ' Seçili öğeyi vurgula.
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

#Region " XML / JSON Oku - Yaz"

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
                            .WriteAttributeString("RefAlanListesi", String.Join(";", Itm.RefAlanListesi))

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

        Public Sub WriteXMLModern(File As String)
            Try
                Dim serializer As New Xml.Serialization.XmlSerializer(GetType(Label))
                Using writer As New StreamWriter(File)
                    serializer.Serialize(writer, Me)
                End Using
                MessageBox.Show(File & " XML Dosyası Kaydedildi", "Dosya Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                HandleError("XML Write Error", ex)
            End Try
        End Sub

        Public Sub ReadXMLModern(File As String)
            Try
                If IO.File.Exists(File) Then
                    Dim serializer As New Xml.Serialization.XmlSerializer(GetType(Label))
                    Using reader As New StreamReader(File)
                        Dim deserializedLabel As Label = CType(serializer.Deserialize(reader), Label)
                        Me.List = deserializedLabel.List
                        Me.MyDataTable = deserializedLabel.MyDataTable
                        ' Diğer özellikler de burada kopyalanabilir.
                    End Using
                Else
                    MessageBox.Show("The filename you selected was not found.")
                End If
            Catch ex As Exception
                HandleError("XML Read Error", ex)
            End Try
        End Sub

        ' JSON yazma metodu.
        Public Sub WriteJSON(File As String)
            Try
                Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(Me, Newtonsoft.Json.Formatting.Indented)
                IO.File.WriteAllText(File, json)
                MessageBox.Show(File & " JSON Dosyası Kaydedildi", "Dosya Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                HandleError("JSON Write Error", ex)
            End Try
        End Sub

        ' JSON okuma metodu.
        Public Sub ReadJSON(File As String)
            Try
                If IO.File.Exists(File) Then
                    Dim json As String = IO.File.ReadAllText(File)
                    Dim deserializedLabel As Label = Newtonsoft.Json.JsonConvert.DeserializeObject(Of Label)(json)
                    Me.List = deserializedLabel.List
                    Me.MyDataTable = deserializedLabel.MyDataTable
                    ' Diğer özellikler de burada kopyalanabilir.
                Else
                    MessageBox.Show("The filename you selected was not found.")
                End If
            Catch ex As Exception
                HandleError("JSON Read Error", ex)
            End Try
        End Sub

        ' Format seçimine göre yazma işlemi.
        Public Sub WriteToFile(File As String)
            Select Case PreferredFileFormat
                Case FileFormat.XML
                    WriteXML(File)
                Case FileFormat.JSON
                    WriteJSON(File)
            End Select
        End Sub

        ' Format seçimine göre okuma işlemi.
        Public Sub ReadFromFile(File As String)
            Select Case PreferredFileFormat
                Case FileFormat.XML
                    ReadXML(File)
                Case FileFormat.JSON
                    ReadJSON(File)
            End Select
        End Sub

        Private Function ReadFontXml(FontXml As XmlReader) As Font
            Dim FontName As String = "Arial"
            Dim FontSize As Integer = DefaultFontSize
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
                            Itm.RefAlanListesi = LblXml.Value  'LblXml.Value.Split(";"c).ToList()
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
                    .Margins = New Margins With {.Top = DefaultMargin, .Bottom = DefaultMargin, .Left = DefaultMargin, .Right = DefaultMargin}
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

        Public Interface IUserNotifier
            Sub Notify(message As String, title As String, icon As MessageBoxIcon)
        End Interface

        Public Class UserNotifier
            Implements IUserNotifier

            Public Sub Notify(message As String, title As String, icon As MessageBoxIcon) Implements IUserNotifier.Notify
                MessageBox.Show(message, title, MessageBoxButtons.OK, icon)
            End Sub
        End Class

        Private _notifier As IUserNotifier = New UserNotifier()

        Public Sub SetNotifier(notifier As IUserNotifier)
            _notifier = notifier
        End Sub

        ' Log dosyasının konumunu ayarlamak için bir özellik.
        Public Property LogFilePath As String
            Get
                Return _logFilePath
            End Get
            Set(value As String)
                If Not String.IsNullOrWhiteSpace(value) Then
                    _logFilePath = value
                End If
            End Set
        End Property

        Private Sub LogError(message As String, Optional ex As Exception = Nothing)
            Dim logMessage As String = $"[{DateTime.Now}] {message}"
            If ex IsNot Nothing Then
                logMessage &= $" - Exception: {ex.Message}"
            End If

            Try
                IO.File.AppendAllText(_logFilePath, logMessage & Environment.NewLine)
            Catch ioEx As Exception
                ' Eğer loglama başarısız olursa, kullanıcıya bir hata mesajı göster.
                MessageBox.Show("Error logging failed: " & ioEx.Message, "Logging Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub HandleError(message As String, Optional ex As Exception = Nothing)
            LogError(message, ex)
            _notifier.Notify(message, "Error", MessageBoxIcon.Error)
        End Sub

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

        Public Enum DataFieldType
            Text
            Number
            Date_
            Boolean_
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
        Public Property DataField As String = ""  'DataFieldType = DataFieldType.Text

        <Category("Data"), Description("Birleştirilecek alanların listesi")>
        Public Property RefAlanListesi As String = ""  'List(Of String) = New List(Of String)()

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
        Public Property MyFont As Font = New Font("Arial", DefaultFontSize)

        <CategoryAttribute("Text"), Description("Yazı hizalama şekli")>
        Public Property TextAlign As ContentAlignment = ContentAlignment.TopLeft

        <CategoryAttribute("Çizgi"), Description("Nesne çizgi rengi")>
        Public Property LineColor As Color = Color.Black
        <CategoryAttribute("Çizgi"), Description("Nesne çizgi kalınlığı")> Public Property LineWidth As Single = DefaultLineWidth
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
                        'BCode.Options.Height = DefaultBarcodeHeight * mm
                        BCode.Options.Height = Height * mm
                        BCode.Options.PureBarcode = BarcodeText
                        Img = BCode.Write(TmpText)
                    Catch ex As Exception
                        Img = Nothing
                    End Try

                Case BarcodeTypes.Code39

                    Try
                        BCode.Format = ZXing.BarcodeFormat.CODE_39
                        'BCode.Options.Height = DefaultBarcodeHeight * mm
                        BCode.Options.Height = Height * mm
                        BCode.Options.PureBarcode = BarcodeText
                        Img = BCode.Write(TmpText)
                    Catch ex As Exception
                        Img = Nothing
                    End Try
                Case BarcodeTypes.EAN_13

                    Try
                        BCode.Format = ZXing.BarcodeFormat.EAN_13

                        'BCode.Options.Height = DefaultBarcodeHeight * mm
                        'BCode.Options.Width = DefaultBarcodeWidth * mm
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
                        'BCode.Options.Height = DefaultBarcodeHeight * mm
                        'BCode.Options.Width = DefaultBarcodeWidth * mm
                        BCode.Options.Height = Height * mm
                        BCode.Options.Width = Width * mm

                        Img = BCode.Write(TmpText)
                    Catch ex As Exception
                        Img = Nothing
                    End Try

                Case BarcodeTypes.DATA_MATRIX
                    Try
                        BCode.Format = ZXing.BarcodeFormat.DATA_MATRIX
                        'BCode.Options.Height = DefaultBarcodeHeight * mm
                        'BCode.Options.Width = DefaultBarcodeWidth * mm
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

        Private Sub DisposeGraphicsResources()
            If Grf IsNot Nothing Then
                Grf.Dispose()
                Grf = Nothing
            End If
        End Sub

        Private Sub DisposeDrawingResources()
            ' Dispose pens, brushes, and other GDI+ objects if necessary
            ' Example:
            ' If MyPen IsNot Nothing Then
            '     MyPen.Dispose()
            '     MyPen = Nothing
            ' End If
        End Sub

        Public Sub CleanUp()
            DisposeGraphicsResources()
            DisposeDrawingResources()
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


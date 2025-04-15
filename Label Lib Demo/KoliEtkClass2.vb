Imports LabelLibrary
Public Class XLabel

    Public M_Label As New LabelProject.Label

    Public Sub New()
        Dim Item As New LabelProject.LabelItem
        Dim PgSettings As New Printing.PageSettings

        With PgSettings
            For Each P As Printing.PaperSize In M_Label.PrintDoc.PrinterSettings.PaperSizes
                If P.PaperName = "custom" Then
                    .PaperSize = P
                    Exit For
                End If

            Next

            With .Margins
                .Top = 8
                .Bottom = 8
                .Left = 8
                .Right = 8
            End With

            .Landscape = True
        End With

        M_Label.PrintDoc.DefaultPageSettings = PgSettings
        M_Label.CokluEtiket = False
        M_Label.EtiketSatýrSayýsý = 1
        M_Label.EtiketSutunSayýsý = 1

        M_Label.PreferredFileFormat = LabelProject.Label.FileFormat.XML

        CreateItem("Cizgi_yatay_1", LabelProject.LabelItem.ItemType.Line, 0, 25, 146, 0, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_yatay_2", LabelProject.LabelItem.ItemType.Line, 0, 50, 146, 0, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_yatay_3", LabelProject.LabelItem.ItemType.Line, 0, 75, 146, 0, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_yatay_4", LabelProject.LabelItem.ItemType.Line, 120, 62, 26, 0, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)

        CreateItem("Cizgi_dikey_1", LabelProject.LabelItem.ItemType.Line, 60, 0, 0, 25, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_dikey_2", LabelProject.LabelItem.ItemType.Line, 105, 0, 0, 25, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_dikey_3", LabelProject.LabelItem.ItemType.Line, 55, 25, 0, 25, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_dikey_4", LabelProject.LabelItem.ItemType.Line, 40, 50, 0, 25, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_dikey_5", LabelProject.LabelItem.ItemType.Line, 70, 50, 0, 25, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_dikey_6", LabelProject.LabelItem.ItemType.Line, 120, 50, 0, 25, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_dikey_7", LabelProject.LabelItem.ItemType.Line, 40, 75, 0, 21, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Cizgi_dikey_8", LabelProject.LabelItem.ItemType.Line, 78, 75, 0, 21, "New Item", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)

        CreateItem("Etiket RECEIVER", LabelProject.LabelItem.ItemType.Label, 0, 0, 50, 5, "RECEIVER / CUSTOMER", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket AGENT", LabelProject.LabelItem.ItemType.Label, 60, 0, 40, 5, "FORWARDING AGENT", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket SUPPLIER ADDRES", LabelProject.LabelItem.ItemType.Label, 105, 0, 40, 5, "SUPPLIER ADDRESS", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket CUSTOMER PART NO", LabelProject.LabelItem.ItemType.Label, 0, 25, 50, 5, "CUSTOMER PART NO (P)", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket SUPPLIER PART NO", LabelProject.LabelItem.ItemType.Label, 55, 25, 50, 5, "SUPPLIER PART NO (C)", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket QUANTITY", LabelProject.LabelItem.ItemType.Label, 0, 50, 50, 5, "QUANTITY (Q)", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket PARCEL NO", LabelProject.LabelItem.ItemType.Label, 40, 50, 30, 5, "PARCEL NO ", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket BATCHNO", LabelProject.LabelItem.ItemType.Label, 70, 50, 30, 5, "BATCH NO (H)", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket GROSSW", LabelProject.LabelItem.ItemType.Label, 120, 50, 30, 5, "GROSS WT (KG)", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket DELIVERY DATE", LabelProject.LabelItem.ItemType.Label, 120, 62, 30, 5, "DELIVERY DATE", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket SUPPLIER CODE", LabelProject.LabelItem.ItemType.Label, 0, 75, 50, 5, "SUPPLIER CODE (V)", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket SERIAL NO", LabelProject.LabelItem.ItemType.Label, 40, 75, 50, 5, "SERIAL NO (S)", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Etiket ADVICE NOTE", LabelProject.LabelItem.ItemType.Label, 80, 75, 50, 5, "ADVICE NOTE (N)", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Bold), ContentAlignment.TopLeft, Color.Black, 1)

        CreateItem("RECEIVER_1", LabelProject.LabelItem.ItemType.Text, 0, 5, 59, 5, "", "!Adres1", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("RECEIVER_2", LabelProject.LabelItem.ItemType.Text, 0, 10, 59, 5, "", "!Adres2", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("RECEIVER_3", LabelProject.LabelItem.ItemType.Text, 0, 15, 59, 5, "", "!Adres3", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("RECEIVER_4", LabelProject.LabelItem.ItemType.Text, 0, 20, 59, 5, "", "!Adres4", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)

        CreateItem("SUPPLIER", LabelProject.LabelItem.ItemType.Text, 105, 5, 40, 20, "ALBERT ZIPS TURKEY   MOSB Ekrem Elginkan Cd. No:7 / Manisa", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("SUPPLIER PARTNO", LabelProject.LabelItem.ItemType.Text, 60, 30, 70, 20, "", "#SÄ°PARÄ°Åž ÃœRÃœN", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("FORWARDING AGENT", LabelProject.LabelItem.ItemType.Text, 60, 5, 45, 20, "--", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Barkod CUSTOMER PARTNO", LabelProject.LabelItem.ItemType.Barcode, 2, 35, 50, 10, "", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "P", "_CUSTOMER PART NO", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("CUSTOMER PART NO", LabelProject.LabelItem.ItemType.Text, 5, 30, 55, 20, "", "#MUS_URUN_KODU", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Barkod SUPPLIER PARTNO", LabelProject.LabelItem.ItemType.Barcode, 58, 35, 87, 10, "", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "C", "_SUPPLIER PARTNO", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("R LOGO", LabelProject.LabelItem.ItemType.Image, 130, 76, 15, 15, "", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("QUANTITY", LabelProject.LabelItem.ItemType.Text, 3, 55, 35, 5, "", "#URK_MIKTAR", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopCenter, Color.Black, 1)
        CreateItem("Barkod QUANTITY", LabelProject.LabelItem.ItemType.Barcode, 2, 60, 45, 10, "", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "Q", "_QUANTITY", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("PARCEL NO", LabelProject.LabelItem.ItemType.Text, 42, 55, 25, 15, "", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("BATCH NO", LabelProject.LabelItem.ItemType.Text, 72, 55, 35, 5, "", "#SIP_NO", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Barkod BATCHNO", LabelProject.LabelItem.ItemType.Barcode, 72, 60, 45, 10, "", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "H", "_BATCH NO", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("SUPPLIER CODE", LabelProject.LabelItem.ItemType.Text, 5, 80, 45, 15, "353482", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("SERIAL NO", LabelProject.LabelItem.ItemType.Text, 45, 80, 40, 5, "1234567", "#BARKOD", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("ADVICE NOTE", LabelProject.LabelItem.ItemType.Text, 85, 80, 40, 5, "-", "", "BATCH NO;" & ControlChars.Quote & "-" & ControlChars.Quote & ";PARCEL NO", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Barkod SUPPLIER CODE", LabelProject.LabelItem.ItemType.Barcode, 2, 85, 35, 8, "353482", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "V", "_SUPPLIER CODE", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Barkod SERIALNO", LabelProject.LabelItem.ItemType.Barcode, 45, 85, 45, 8, "1234567", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "S", "_SERIAL NO", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("Barkod ADVICENOTE", LabelProject.LabelItem.ItemType.Barcode, 83, 85, 45, 8, "-", "", "", LabelProject.LabelItem.BarcodeTypes.Code128, "N", "_ADVICE NOTE", True, New Font("Arial", 10, FontStyle.Regular), ContentAlignment.TopLeft, Color.Black, 1)
        CreateItem("GROSSWT", LabelProject.LabelItem.ItemType.Text, 122, 55, 22, 5, "", "", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.MiddleCenter, Color.Black, 1)
        CreateItem("DELIVERY DATE", LabelProject.LabelItem.ItemType.Text, 122, 67, 22, 5, "", "@BugÃ¼n Tarihi", "", LabelProject.LabelItem.BarcodeTypes.Code39, "", "", True, New Font("Arial", 9, FontStyle.Regular), ContentAlignment.MiddleCenter, Color.Black, 1)
    End Sub
    Public Sub OnIzleme()
        M_Label.PrintPreview()
    End Sub

    Public Sub Yazdýr()
        M_Label.PrintDoc.Print()
    End Sub

    Private Sub CreateItem(Name As String, ItmType As LabelProject.LabelItem.ItemType,
                   x1 As Single, y1 As Single,
                   Width As Single, Height As Single,
                   Text As String,
                   Optional DataField As String = vbNullString,
                   Optional RefAlanlist As String = "",
                   Optional BarkodType As LabelProject.LabelItem.BarcodeTypes = LabelProject.LabelItem.BarcodeTypes.Code128,
                   Optional BarkodPrfxChr As String = vbNullString,
                   Optional BarkodVeriAlan As String = vbNullString,
                   Optional BarkodText As Boolean = True,
                   Optional Yazýtipi As Font = Nothing,
                   Optional YaziHiza As ContentAlignment = ContentAlignment.MiddleLeft,
                   Optional CizgiRengi As Color = Nothing,
                   Optional CizgiK As Single = 1,
                   Optional Resim As Image = Nothing)

        Dim Item = New LabelProject.LabelItem
        With Item
            .Type = ItmType
            .Name = Name
            .Text = Text
            .x1 = x1
            .y1 = y1
            .Width = Width
            .Height = Height
            If IsNothing(CizgiRengi) Then
                .LineColor = Color.Black
            Else
                .LineColor = CizgiRengi
            End If
            .LineWidth = CizgiK
            .DataField = DataField
            .BarcodePrefixChar = BarkodPrfxChr
            .BarcodeType = BarkodType
            .BarkodVeriAlaný = BarkodVeriAlan
            .RefAlanListesi = RefAlanlist ' .Split(","c).ToList()
            .BarcodeText = BarkodText
            If IsNothing(Yazýtipi) Then
                .MyFont = New Font("Arial", 10)
            Else
                .MyFont = Yazýtipi
            End If
            .TextAlign = YaziHiza
            .Image = Resim
        End With

        M_Label.Add(Item)

    End Sub

End Class

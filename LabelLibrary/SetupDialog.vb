Imports System.Drawing
Imports System.Windows.Forms

Public Class SetupDialog
	Const mm As Single = 3.9380952380952379
	Friend WithEvents PrintDoc As New Printing.PrintDocument
	Public Property YazıcıAyar As Printing.PrinterSettings
		Set(value As Printing.PrinterSettings)
			Me.PrintDoc.PrinterSettings = value
		End Set
		Get
			Return Me.PrintDoc.PrinterSettings
		End Get
	End Property
	Public Property SayfaAyar As Printing.PageSettings
		Set(value As Printing.PageSettings)
			Me.PrintDoc.DefaultPageSettings = value
		End Set
		Get
			Return Me.PrintDoc.DefaultPageSettings
		End Get
	End Property

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		Me.PrintPreviewDialog1.Document = PrintDoc
		Try
			For Each pkInstalledPrinters As String In Printing.PrinterSettings.InstalledPrinters
				ComboBoxPrinter.Items.Add(pkInstalledPrinters)
				If pkInstalledPrinters = PrintDoc.PrinterSettings.PrinterName Then ComboBoxPrinter.SelectedItem = pkInstalledPrinters
			Next
			For Each Pp As String In ComboBoxPaper.Items
				If Pp = PrintDoc.DefaultPageSettings.PaperSize.PaperName Then
					ComboBoxPaper.SelectedItem = Pp
					Exit For
				End If
			Next
			If PrintDoc.DefaultPageSettings.Landscape Then
				RadioButtonYatay.Checked = True
			Else
				RadioButtonDikey.Checked = True
			End If
			TextBoxSol.Text = PrintDoc.DefaultPageSettings.Margins.Left / mm
			TextBoxSag.Text = PrintDoc.DefaultPageSettings.Margins.Right / mm
			TextBoxUst.Text = PrintDoc.DefaultPageSettings.Margins.Top / mm
			TextBoxAlt.Text = PrintDoc.DefaultPageSettings.Margins.Bottom / mm
			TextBoxEn.Text = PrintDoc.DefaultPageSettings.PaperSize.Width / mm
			TextBoxBoy.Text = PrintDoc.DefaultPageSettings.PaperSize.Height / mm
			If PrintDoc.DefaultPageSettings.PaperSize.RawKind = 0 Then
				CustomPaper.Checked = True
			End If
		Catch ex As Exception
			MessageBox.Show(ex.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
	End Sub
	Private Sub ComboBoxPrinter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxPrinter.SelectedIndexChanged
		PrintDoc.PrinterSettings.PrinterName = ComboBoxPrinter.SelectedItem
		ComboBoxPaper.Items.Clear()
		For Each Pr As Printing.PaperSize In PrintDoc.PrinterSettings.PaperSizes
			ComboBoxPaper.Items.Add(Pr.PaperName)
		Next
	End Sub
	Private Sub ComboBoxPaper_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBoxPaper.SelectedIndexChanged
		For Each Paper As Printing.PaperSize In PrintDoc.PrinterSettings.PaperSizes
			If Paper.PaperName = ComboBoxPaper.SelectedItem Then
				PrintDoc.DefaultPageSettings.PaperSize = Paper
				TextBoxEn.Text = Paper.Width / mm
				TextBoxBoy.Text = Paper.Height / mm
				Exit For
			End If
		Next
	End Sub

	Private Sub Landscape_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonDikey.CheckedChanged, RadioButtonYatay.CheckedChanged
		If RadioButtonDikey.Checked Then
			PrintDoc.DefaultPageSettings.Landscape = False
		Else
			PrintDoc.DefaultPageSettings.Landscape = True
		End If
	End Sub
	Private Sub Margins_TextChanged(sender As Object, e As EventArgs) Handles TextBoxSol.TextChanged, TextBoxSag.TextChanged, TextBoxUst.TextChanged, TextBoxAlt.TextChanged
		Try
			With PrintDoc.DefaultPageSettings.Margins
				.Left = CInt(TextBoxSol.Text) * mm
				.Right = CInt(TextBoxSag.Text) * mm
				.Top = CInt(TextBoxUst.Text) * mm
				.Bottom = CInt(TextBoxAlt.Text) * mm
			End With
		Catch ex As Exception
		End Try
	End Sub
	Private Sub CustomPaper_CheckedChanged(sender As Object, e As EventArgs) Handles CustomPaper.CheckedChanged
		If CustomPaper.Checked Then
			TextBoxEn.Enabled = True
			TextBoxBoy.Enabled = True
			ComboBoxPaper.Enabled = False
			With PrintDoc.DefaultPageSettings.PaperSize
				.RawKind = 0
				.PaperName = "Custom"
			End With
		Else
			TextBoxEn.Enabled = False
			TextBoxBoy.Enabled = False
			ComboBoxPaper.Enabled = True
		End If
	End Sub

	Private Sub TextBoxEnBoy_TextChanged(sender As Object, e As EventArgs) Handles TextBoxEn.TextChanged, TextBoxBoy.TextChanged
		If CustomPaper.Checked Then
			Try
				With PrintDoc.DefaultPageSettings.PaperSize
					.PaperName = "Custom"
					.RawKind = 0
					.Width = CInt(TextBoxEn.Text) * mm
					.Height = CInt(TextBoxBoy.Text) * mm
				End With
			Catch ex As Exception
			End Try
		End If
	End Sub
	Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
		Me.DialogResult = DialogResult.OK
		Me.Close()
	End Sub
	Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
		Me.DialogResult = DialogResult.Cancel
		Me.Close()
	End Sub
	Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
		PrintPreviewDialog1.ShowDialog()
	End Sub

	Private Sub PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDoc.PrintPage
		Dim MarginPen As New Pen(Brushes.Blue) With {.DashStyle = Drawing2D.DashStyle.Dash}
		With e.Graphics
			.DrawRectangle(MarginPen, e.MarginBounds)
		End With
	End Sub


End Class

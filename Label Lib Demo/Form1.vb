Imports System.Drawing.Printing
Imports LabelLibrary
Public Class Form1
    Private MLblb As New XLabel

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        For Each Itm As LabelProject.LabelItem In MLblb.M_Label
            ListBox1.Items.Add(Itm.Name)
        Next

        MLblb.M_Label.ReadPaperSettingFromReg("testapp")

        MLblb.M_Label.DrawMargins = True

        PrintPreviewControl1.Document = MLblb.M_Label.PrintDoc

    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        PropertyGrid1.SelectedObject = MLblb.M_Label.Item(ListBox1.SelectedItem)

        MLblb.M_Label.SelectedItem = MLblb.M_Label.Item(ListBox1.SelectedItem)
        PrintPreviewControl1.Document = MLblb.M_Label.PrintDoc
    End Sub

    Private Sub ButtonLabelProperty_Click(sender As Object, e As EventArgs) Handles Button2.Click
        MLblb.M_Label.MoveFirst()
        PropertyGrid1.SelectedObject = MLblb.M_Label
    End Sub

    Private Sub ButtonSetup_Click(sender As Object, e As EventArgs) Handles Button3.Click

        MLblb.M_Label.SetupDialog()
        PrintPreviewControl1.Document = MLblb.M_Label.PrintDoc

        'MLblb.M_Label.PageSetup()
        MLblb.M_Label.WritePaperSettingToReg("testapp")

    End Sub

    Private Sub PropertyGrid1_PropertyValueChanged(s As Object, e As PropertyValueChangedEventArgs) Handles PropertyGrid1.PropertyValueChanged
        PrintPreviewControl1.InvalidatePreview()
    End Sub

    Private Sub ButtonPrint_Click(sender As Object, e As EventArgs) Handles Button1.Click
        MLblb.M_Label.PrintAll()
    End Sub

    Private Sub TümÖğeleriSilToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TümÖğeleriSilToolStripMenuItem.Click
        MLblb.M_Label.Clear()
        ListBox1.Items.Clear()
    End Sub

    Private Sub ÖğeEkleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÖğeEkleToolStripMenuItem.Click
        MLblb.M_Label.AddText("Yeni Text", 0, 0, 20, 5)

        ListBox1.Items.Clear()
        For Each Itm As LabelLibrary.LabelProject.LabelItem In MLblb.M_Label
            ListBox1.Items.Add(Itm.Name)
        Next
    End Sub
End Class

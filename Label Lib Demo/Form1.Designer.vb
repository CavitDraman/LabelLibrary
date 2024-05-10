<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.PropertyGrid1 = New System.Windows.Forms.PropertyGrid()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.PrintPreviewControl1 = New System.Windows.Forms.PrintPreviewControl()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.DosyaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TümÖğeleriSilToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ÖğeEkleToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 16
        Me.ListBox1.Location = New System.Drawing.Point(12, 60)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(275, 260)
        Me.ListBox1.TabIndex = 1
        '
        'PropertyGrid1
        '
        Me.PropertyGrid1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PropertyGrid1.Location = New System.Drawing.Point(12, 326)
        Me.PropertyGrid1.Name = "PropertyGrid1"
        Me.PropertyGrid1.Size = New System.Drawing.Size(275, 174)
        Me.PropertyGrid1.TabIndex = 2
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(12, 33)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(144, 23)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "Etiket Özellikleri"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(163, 33)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(124, 23)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "Sayfa Ayarları"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'PrintPreviewControl1
        '
        Me.PrintPreviewControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PrintPreviewControl1.Location = New System.Drawing.Point(293, 60)
        Me.PrintPreviewControl1.Name = "PrintPreviewControl1"
        Me.PrintPreviewControl1.Size = New System.Drawing.Size(776, 440)
        Me.PrintPreviewControl1.TabIndex = 5
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(293, 34)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 6
        Me.Button1.Text = "Yazdır"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(19, 19)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DosyaToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1081, 28)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'DosyaToolStripMenuItem
        '
        Me.DosyaToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TümÖğeleriSilToolStripMenuItem, Me.ÖğeEkleToolStripMenuItem})
        Me.DosyaToolStripMenuItem.Name = "DosyaToolStripMenuItem"
        Me.DosyaToolStripMenuItem.Size = New System.Drawing.Size(64, 24)
        Me.DosyaToolStripMenuItem.Text = "Dosya"
        '
        'TümÖğeleriSilToolStripMenuItem
        '
        Me.TümÖğeleriSilToolStripMenuItem.Name = "TümÖğeleriSilToolStripMenuItem"
        Me.TümÖğeleriSilToolStripMenuItem.Size = New System.Drawing.Size(217, 26)
        Me.TümÖğeleriSilToolStripMenuItem.Text = "Tüm Öğeleri Sil"
        '
        'ÖğeEkleToolStripMenuItem
        '
        Me.ÖğeEkleToolStripMenuItem.Name = "ÖğeEkleToolStripMenuItem"
        Me.ÖğeEkleToolStripMenuItem.Size = New System.Drawing.Size(217, 26)
        Me.ÖğeEkleToolStripMenuItem.Text = "Öğe Ekle"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1081, 512)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.PrintPreviewControl1)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.PropertyGrid1)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form1"
        Me.Text = "Label Tester"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents PropertyGrid1 As PropertyGrid
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents PrintPreviewControl1 As PrintPreviewControl
    Friend WithEvents Button1 As Button
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents DosyaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TümÖğeleriSilToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ÖğeEkleToolStripMenuItem As ToolStripMenuItem
End Class

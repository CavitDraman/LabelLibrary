<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SetupDialog
    Inherits System.Windows.Forms.Form

    'Form, bileşen listesini temizlemeyi bırakmayı geçersiz kılar.
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

    'Windows Form Tasarımcısı tarafından gerektirilir
    Private components As System.ComponentModel.IContainer

    'NOT: Aşağıdaki yordam Windows Form Tasarımcısı için gereklidir
    'Windows Form Tasarımcısı kullanılarak değiştirilebilir.  
    'Kod düzenleyicisini kullanarak değiştirmeyin.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SetupDialog))
        Me.PrintPreviewDialog1 = New System.Windows.Forms.PrintPreviewDialog()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TextBoxBoy = New System.Windows.Forms.TextBox()
        Me.TextBoxEn = New System.Windows.Forms.TextBox()
        Me.TextBoxAlt = New System.Windows.Forms.TextBox()
        Me.TextBoxSag = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBoxUst = New System.Windows.Forms.TextBox()
        Me.CustomPaper = New System.Windows.Forms.CheckBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TextBoxSol = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RadioButtonDikey = New System.Windows.Forms.RadioButton()
        Me.RadioButtonYatay = New System.Windows.Forms.RadioButton()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ComboBoxPaper = New System.Windows.Forms.ComboBox()
        Me.ComboBoxPrinter = New System.Windows.Forms.ComboBox()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PrintPreviewDialog1
        '
        Me.PrintPreviewDialog1.AutoScrollMargin = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.AutoScrollMinSize = New System.Drawing.Size(0, 0)
        Me.PrintPreviewDialog1.ClientSize = New System.Drawing.Size(400, 300)
        Me.PrintPreviewDialog1.Enabled = True
        Me.PrintPreviewDialog1.Icon = CType(resources.GetObject("PrintPreviewDialog1.Icon"), System.Drawing.Icon)
        Me.PrintPreviewDialog1.Name = "PrintPreviewDialog1"
        Me.PrintPreviewDialog1.Text = "Baskı önizleme"
        Me.PrintPreviewDialog1.Visible = False
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(4, 4)
        Me.OK_Button.Margin = New System.Windows.Forms.Padding(4)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(89, 28)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "TAMAM"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(101, 4)
        Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(89, 28)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "İptal"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(261, 354)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(4)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(195, 36)
        Me.TableLayoutPanel2.TabIndex = 18
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(23, 59)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 16)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "Boy"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(23, 27)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(26, 16)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "En "
        '
        'TextBoxBoy
        '
        Me.TextBoxBoy.Enabled = False
        Me.TextBoxBoy.Location = New System.Drawing.Point(61, 55)
        Me.TextBoxBoy.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBoxBoy.Name = "TextBoxBoy"
        Me.TextBoxBoy.Size = New System.Drawing.Size(137, 22)
        Me.TextBoxBoy.TabIndex = 1
        '
        'TextBoxEn
        '
        Me.TextBoxEn.Enabled = False
        Me.TextBoxEn.Location = New System.Drawing.Point(61, 23)
        Me.TextBoxEn.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBoxEn.Name = "TextBoxEn"
        Me.TextBoxEn.Size = New System.Drawing.Size(137, 22)
        Me.TextBoxEn.TabIndex = 0
        '
        'TextBoxAlt
        '
        Me.TextBoxAlt.Location = New System.Drawing.Point(192, 55)
        Me.TextBoxAlt.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBoxAlt.Name = "TextBoxAlt"
        Me.TextBoxAlt.Size = New System.Drawing.Size(65, 22)
        Me.TextBoxAlt.TabIndex = 3
        '
        'TextBoxSag
        '
        Me.TextBoxSag.Location = New System.Drawing.Point(192, 23)
        Me.TextBoxSag.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBoxSag.Name = "TextBoxSag"
        Me.TextBoxSag.Size = New System.Drawing.Size(65, 22)
        Me.TextBoxSag.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(27, 60)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(27, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Üst"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(27, 28)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(27, 16)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Sol"
        '
        'TextBoxUst
        '
        Me.TextBoxUst.Location = New System.Drawing.Point(68, 55)
        Me.TextBoxUst.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBoxUst.Name = "TextBoxUst"
        Me.TextBoxUst.Size = New System.Drawing.Size(65, 22)
        Me.TextBoxUst.TabIndex = 2
        '
        'CustomPaper
        '
        Me.CustomPaper.AutoSize = True
        Me.CustomPaper.Location = New System.Drawing.Point(23, 110)
        Me.CustomPaper.Margin = New System.Windows.Forms.Padding(4)
        Me.CustomPaper.Name = "CustomPaper"
        Me.CustomPaper.Size = New System.Drawing.Size(120, 20)
        Me.CustomPaper.TabIndex = 11
        Me.CustomPaper.Text = "Özel Kağıt Boyu"
        Me.CustomPaper.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.Label7)
        Me.GroupBox3.Controls.Add(Me.TextBoxBoy)
        Me.GroupBox3.Controls.Add(Me.TextBoxEn)
        Me.GroupBox3.Location = New System.Drawing.Point(23, 123)
        Me.GroupBox3.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox3.Size = New System.Drawing.Size(271, 90)
        Me.GroupBox3.TabIndex = 17
        Me.GroupBox3.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(148, 59)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(22, 16)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Alt"
        '
        'TextBoxSol
        '
        Me.TextBoxSol.Location = New System.Drawing.Point(68, 23)
        Me.TextBoxSol.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBoxSol.Name = "TextBoxSol"
        Me.TextBoxSol.Size = New System.Drawing.Size(65, 22)
        Me.TextBoxSol.TabIndex = 0
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(23, 345)
        Me.Button3.Margin = New System.Windows.Forms.Padding(4)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(108, 28)
        Me.Button3.TabIndex = 15
        Me.Button3.Text = "Ön İzleme"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.TextBoxAlt)
        Me.GroupBox2.Controls.Add(Me.TextBoxSag)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.TextBoxUst)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.TextBoxSol)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Location = New System.Drawing.Point(171, 232)
        Me.GroupBox2.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox2.Size = New System.Drawing.Size(283, 96)
        Me.GroupBox2.TabIndex = 16
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Kenar Boşlukları"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(148, 27)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Sağ"
        '
        'RadioButtonDikey
        '
        Me.RadioButtonDikey.AutoSize = True
        Me.RadioButtonDikey.Checked = True
        Me.RadioButtonDikey.Location = New System.Drawing.Point(24, 23)
        Me.RadioButtonDikey.Margin = New System.Windows.Forms.Padding(4)
        Me.RadioButtonDikey.Name = "RadioButtonDikey"
        Me.RadioButtonDikey.Size = New System.Drawing.Size(60, 20)
        Me.RadioButtonDikey.TabIndex = 0
        Me.RadioButtonDikey.TabStop = True
        Me.RadioButtonDikey.Text = "Dikey"
        Me.RadioButtonDikey.UseVisualStyleBackColor = True
        '
        'RadioButtonYatay
        '
        Me.RadioButtonYatay.AutoSize = True
        Me.RadioButtonYatay.Location = New System.Drawing.Point(24, 52)
        Me.RadioButtonYatay.Margin = New System.Windows.Forms.Padding(4)
        Me.RadioButtonYatay.Name = "RadioButtonYatay"
        Me.RadioButtonYatay.Size = New System.Drawing.Size(60, 20)
        Me.RadioButtonYatay.TabIndex = 1
        Me.RadioButtonYatay.Text = "Yatay"
        Me.RadioButtonYatay.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.RadioButtonDikey)
        Me.GroupBox1.Controls.Add(Me.RadioButtonYatay)
        Me.GroupBox1.Location = New System.Drawing.Point(23, 232)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(4)
        Me.GroupBox1.Size = New System.Drawing.Size(137, 96)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Yön"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(19, 52)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 16)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "Kağıt"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(19, 19)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 16)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Yazıcı"
        '
        'ComboBoxPaper
        '
        Me.ComboBoxPaper.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxPaper.FormattingEnabled = True
        Me.ComboBoxPaper.Location = New System.Drawing.Point(74, 48)
        Me.ComboBoxPaper.Margin = New System.Windows.Forms.Padding(4)
        Me.ComboBoxPaper.Name = "ComboBoxPaper"
        Me.ComboBoxPaper.Size = New System.Drawing.Size(303, 24)
        Me.ComboBoxPaper.TabIndex = 10
        '
        'ComboBoxPrinter
        '
        Me.ComboBoxPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBoxPrinter.FormattingEnabled = True
        Me.ComboBoxPrinter.Location = New System.Drawing.Point(74, 15)
        Me.ComboBoxPrinter.Margin = New System.Windows.Forms.Padding(4)
        Me.ComboBoxPrinter.Name = "ComboBoxPrinter"
        Me.ComboBoxPrinter.Size = New System.Drawing.Size(303, 24)
        Me.ComboBoxPrinter.TabIndex = 9
        '
        'SetupDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(469, 403)
        Me.Controls.Add(Me.TableLayoutPanel2)
        Me.Controls.Add(Me.CustomPaper)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBoxPaper)
        Me.Controls.Add(Me.ComboBoxPrinter)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SetupDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "SetupDialog"
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents PrintPreviewDialog1 As Windows.Forms.PrintPreviewDialog
    Friend WithEvents OK_Button As Windows.Forms.Button
    Friend WithEvents Cancel_Button As Windows.Forms.Button
    Friend WithEvents TableLayoutPanel2 As Windows.Forms.TableLayoutPanel
    Friend WithEvents Label8 As Windows.Forms.Label
    Friend WithEvents Label7 As Windows.Forms.Label
    Friend WithEvents TextBoxBoy As Windows.Forms.TextBox
    Friend WithEvents TextBoxEn As Windows.Forms.TextBox
    Friend WithEvents TextBoxAlt As Windows.Forms.TextBox
    Friend WithEvents TextBoxSag As Windows.Forms.TextBox
    Friend WithEvents Label6 As Windows.Forms.Label
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents TextBoxUst As Windows.Forms.TextBox
    Friend WithEvents CustomPaper As Windows.Forms.CheckBox
    Friend WithEvents GroupBox3 As Windows.Forms.GroupBox
    Friend WithEvents Label5 As Windows.Forms.Label
    Friend WithEvents TextBoxSol As Windows.Forms.TextBox
    Friend WithEvents Button3 As Windows.Forms.Button
    Friend WithEvents GroupBox2 As Windows.Forms.GroupBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents RadioButtonDikey As Windows.Forms.RadioButton
    Friend WithEvents RadioButtonYatay As Windows.Forms.RadioButton
    Friend WithEvents GroupBox1 As Windows.Forms.GroupBox
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents ComboBoxPaper As Windows.Forms.ComboBox
    Friend WithEvents ComboBoxPrinter As Windows.Forms.ComboBox
End Class

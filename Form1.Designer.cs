namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            label1 = new Label();
            groupBox1 = new GroupBox();
            numDiem = new NumericUpDown();
            comboBox1 = new ComboBox();
            dtpNgaySinh = new DateTimePicker();
            txtDienThoai = new TextBox();
            txtEmail = new TextBox();
            txtHoTen = new TextBox();
            label10 = new Label();
            txtMaSV = new TextBox();
            label9 = new Label();
            label7 = new Label();
            label8 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            textBox8 = new TextBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDiem).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 11F);
            label1.ForeColor = SystemColors.HotTrack;
            label1.Location = new Point(25, 24);
            label1.Name = "label1";
            label1.Size = new Size(143, 20);
            label1.TabIndex = 1;
            label1.Text = "QUẢN LÝ SINH VIÊN";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(numDiem);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(dtpNgaySinh);
            groupBox1.Controls.Add(txtDienThoai);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(txtHoTen);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(txtMaSV);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label6);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Font = new Font("Segoe UI", 11F);
            groupBox1.Location = new Point(25, 55);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1176, 130);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin sinh viên";
            groupBox1.Enter += groupBox1_Enter;
            // 
            // numDiem
            // 
            numDiem.ImeMode = ImeMode.NoControl;
            numDiem.Location = new Point(913, 55);
            numDiem.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            numDiem.Name = "numDiem";
            numDiem.Size = new Size(231, 27);
            numDiem.TabIndex = 9;
            numDiem.TextAlign = HorizontalAlignment.Center;
            numDiem.ValueChanged += numDiem_ValueChanged;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(913, 22);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(231, 28);
            comboBox1.TabIndex = 3;
            // 
            // dtpNgaySinh
            // 
            dtpNgaySinh.CustomFormat = "dd-MM-yyyy";
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.Location = new Point(86, 55);
            dtpNgaySinh.Name = "dtpNgaySinh";
            dtpNgaySinh.Size = new Size(231, 27);
            dtpNgaySinh.TabIndex = 8;
            // 
            // txtDienThoai
            // 
            txtDienThoai.Location = new Point(494, 90);
            txtDienThoai.Name = "txtDienThoai";
            txtDienThoai.Size = new Size(231, 27);
            txtDienThoai.TabIndex = 6;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(84, 90);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(231, 27);
            txtEmail.TabIndex = 6;
            // 
            // txtHoTen
            // 
            txtHoTen.Location = new Point(494, 22);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new Size(231, 27);
            txtHoTen.TabIndex = 6;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(819, 93);
            label10.Name = "label10";
            label10.Size = new Size(75, 20);
            label10.TabIndex = 5;
            label10.Text = "Trạng thái";
            // 
            // txtMaSV
            // 
            txtMaSV.Location = new Point(86, 18);
            txtMaSV.Name = "txtMaSV";
            txtMaSV.Size = new Size(231, 27);
            txtMaSV.TabIndex = 6;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(849, 60);
            label9.Name = "label9";
            label9.Size = new Size(45, 20);
            label9.TabIndex = 4;
            label9.Text = "Điểm";
            label9.Click += label6_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(400, 93);
            label7.Name = "label7";
            label7.Size = new Size(88, 20);
            label7.TabIndex = 5;
            label7.Text = "Điện thoại *";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(822, 25);
            label8.Name = "label8";
            label8.Size = new Size(72, 20);
            label8.TabIndex = 3;
            label8.Text = "Lớp học *";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(423, 60);
            label6.Name = "label6";
            label6.Size = new Size(65, 20);
            label6.TabIndex = 4;
            label6.Text = "Giới tính";
            label6.Click += label6_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(405, 25);
            label5.Name = "label5";
            label5.Size = new Size(83, 20);
            label5.TabIndex = 3;
            label5.Text = "Họ và tên *";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(22, 93);
            label4.Name = "label4";
            label4.Size = new Size(56, 20);
            label4.TabIndex = 2;
            label4.Text = "Email *";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 60);
            label3.Name = "label3";
            label3.Size = new Size(74, 20);
            label3.TabIndex = 1;
            label3.Text = "Ngày sinh";
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 25);
            label2.Name = "label2";
            label2.Size = new Size(61, 20);
            label2.TabIndex = 0;
            label2.Text = "Mã SV *";
            // 
            // textBox8
            // 
            textBox8.Location = new Point(766, 370);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(231, 23);
            textBox8.TabIndex = 6;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1251, 606);
            Controls.Add(groupBox1);
            Controls.Add(textBox8);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDiem).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private GroupBox groupBox1;
        private Label label3;
        private Label label2;
        private Label label4;
        private Label label7;
        private Label label6;
        private Label label5;
        private TextBox txtMaSV;
        private ContextMenuStrip contextMenuStrip1;
        private TextBox txtDienThoai;
        private TextBox txtEmail;
        private TextBox txtHoTen;
        private TextBox textBox8;
        private Label label10;
        private Label label9;
        private Label label8;
        private DateTimePicker dtpNgaySinh;
        private ComboBox comboBox1;
        private NumericUpDown numDiem;
    }
}

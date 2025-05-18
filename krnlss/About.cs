using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace krnlss;

public class About : Form
{
	public const int WM_NCLBUTTONDOWN = 161;

	public const int HT_CAPTION = 2;

	private IContainer components;

	private Button button1;

	private Panel panel1;

	private PictureBox pictureBox1;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label10;

	private Label label11;

	private Label label9;

	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	public About()
	{
		InitializeComponent();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void panel1_Move(object sender, EventArgs e)
	{
	}

	private void panel1_Paint(object sender, PaintEventArgs e)
	{
	}

	private void panel1_MouseMove(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			ReleaseCapture();
			SendMessage(base.Handle, 161, 2, 0);
		}
	}

	private void About_Load(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(krnlss.About));
		this.button1 = new System.Windows.Forms.Button();
		this.panel1 = new System.Windows.Forms.Panel();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.button1.BackColor = System.Drawing.Color.FromArgb(29, 29, 29);
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Corbel", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
		this.button1.Location = new System.Drawing.Point(285, 0);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(35, 33);
		this.button1.TabIndex = 3;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.panel1.BackColor = System.Drawing.Color.FromArgb(29, 29, 29);
		this.panel1.Controls.Add(this.button1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(320, 33);
		this.panel1.TabIndex = 1;
		this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(panel1_Paint);
		this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(panel1_MouseMove);
		this.panel1.Move += new System.EventHandler(panel1_Move);
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(-64, -21);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(270, 237);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 2;
		this.pictureBox1.TabStop = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(140, 59);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(168, 17);
		this.label1.TabIndex = 3;
		this.label1.Text = "UI Design and Components";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.DarkGray;
		this.label2.Location = new System.Drawing.Point(140, 78);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(25, 17);
		this.label2.TabIndex = 4;
		this.label2.Text = "Iris";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.DarkGray;
		this.label3.Location = new System.Drawing.Point(164, 78);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(51, 17);
		this.label3.TabIndex = 5;
		this.label3.Text = "Littensy";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.DarkGray;
		this.label4.Location = new System.Drawing.Point(140, 116);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(54, 17);
		this.label4.TabIndex = 7;
		this.label4.Text = "Ice Bear";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(140, 97);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(111, 17);
		this.label5.TabIndex = 6;
		this.label5.Text = "Exploit Developer";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.DarkGray;
		this.label6.Location = new System.Drawing.Point(230, 154);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(72, 17);
		this.label6.TabIndex = 10;
		this.label6.Text = "KowalskiFX";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.DarkGray;
		this.label7.Location = new System.Drawing.Point(140, 154);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(75, 17);
		this.label7.TabIndex = 9;
		this.label7.Text = "Customality";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(140, 135);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(49, 17);
		this.label8.TabIndex = 8;
		this.label8.Text = "Credits";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.DarkGray;
		this.label9.Location = new System.Drawing.Point(212, 78);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(24, 17);
		this.label9.TabIndex = 11;
		this.label9.Text = "XV";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.DarkGray;
		this.label10.Location = new System.Drawing.Point(234, 78);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(35, 17);
		this.label10.TabIndex = 11;
		this.label10.Text = "0x00";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.DarkGray;
		this.label11.Location = new System.Drawing.Point(268, 78);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(34, 17);
		this.label11.TabIndex = 11;
		this.label11.Text = "King";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		base.ClientSize = new System.Drawing.Size(320, 191);
		base.Controls.Add(this.label11);
		base.Controls.Add(this.label10);
		base.Controls.Add(this.label9);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.label8);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.pictureBox1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "About";
		this.Text = "About";
		base.TopMost = true;
		base.Load += new System.EventHandler(About_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

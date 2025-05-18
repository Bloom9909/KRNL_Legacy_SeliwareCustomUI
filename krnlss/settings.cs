using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using krnlss.Properties;
using ToggleSlider;

namespace krnlss;

public class settings : Form
{
	private Form parent;

	private Thread autoinjectiont;

	private IContainer components;

	private Panel panel1;

	private BunifuCustomLabel bunifuCustomLabel1;

	private BunifuCustomLabel bunifuCustomLabel2;

	private ToggleSliderComponent toggleSliderComponent1;

	private ToggleSliderComponent toggleSliderComponent2;

	private Button button3;

	private Button button4;

	private ToggleSliderComponent toggleSliderComponent3;

	private BunifuCustomLabel bunifuCustomLabel3;

	private Label label1;

	private PictureBox pictureBox1;

	private BunifuCustomLabel bunifuCustomLabel4;

	private Button button1;

	private ToggleSliderComponent toggleSliderComponent4;

	private BunifuCustomLabel bunifuCustomLabel5;

	private ToggleSliderComponent toggleSliderComponent5;

	private BunifuCustomLabel bunifuCustomLabel6;

	private BunifuElipse bunifuElipse1;

	public static bool monaco_changed;

	[DllImport("user32.dll")]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	public settings(Form parentt)
	{
		InitializeComponent();
		parent = parentt;
	}

	private void krnl_Load(object sender, EventArgs e)
	{
		if (Settings.Default.autoinject)
		{
			toggleSliderComponent1.Checked = true;
		}
		if (Settings.Default.topmostchecked)
		{
			toggleSliderComponent2.Checked = true;
		}
		if (Settings.Default.fadein_out_opacity)
		{
			toggleSliderComponent3.Checked = true;
		}
		if (Settings.Default.remove_crash_logs)
		{
			toggleSliderComponent4.Checked = true;
		}
		if (Settings.Default.monaco)
		{
			toggleSliderComponent5.Checked = false;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Application.Exit();
	}

	private void button2_Click(object sender, EventArgs e)
	{
	}

	private void button3_Click(object sender, EventArgs e)
	{
	}

	private void OPACITYASSS_ValueChanged(object sender, EventArgs e)
	{
	}

	private void button1_Click_1(object sender, EventArgs e)
	{
		Close();
	}

	private void label1_Click(object sender, EventArgs e)
	{
	}

	private void panel1_MouseDown_1(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			krnl.ReleaseCapture();
			krnl.SendMessage(base.Handle, 161, 2, 0);
		}
	}

	private void toggleSliderComponent2_Load(object sender, EventArgs e)
	{
	}

	private void button4_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void button3_Click_1(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
	}

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool WaitNamedPipe(string name, int timeout);

	private static bool findpipe(string pipeName)
	{
		try
		{
			if (WaitNamedPipe(Path.GetFullPath("\\\\.\\pipe\\" + pipeName), 0) && (Marshal.GetLastWin32Error() == 0 || Marshal.GetLastWin32Error() == 2))
			{
				return false;
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private void autoinjectbruh()
	{
	}

	private void toggleSliderComponent1_Load(object sender, EventArgs e)
	{
	}

	private void toggleSliderComponent1_CheckChanged(object sender, EventArgs e)
	{
		if (toggleSliderComponent1.Checked)
		{
			autoinjectiont = new Thread(autoinjectbruh);
			autoinjectiont.IsBackground = true;
			autoinjectiont.Start();
			Settings.Default.autoinject = true;
			Settings.Default.Save();
			return;
		}
		if (autoinjectiont != null)
		{
			autoinjectiont.Abort();
			autoinjectiont = null;
		}
		Settings.Default.autoinject = false;
		Settings.Default.Save();
	}

	private void toggleSliderComponent2_CheckChanged(object sender, EventArgs e)
	{
		if (toggleSliderComponent2.Checked)
		{
			base.TopMost = true;
			parent.TopMost = true;
			Settings.Default.topmostchecked = true;
			Settings.Default.Save();
		}
		else
		{
			base.TopMost = false;
			parent.TopMost = false;
			Settings.Default.topmostchecked = false;
			Settings.Default.Save();
		}
	}

	private void settings_FormClosing(object sender, FormClosingEventArgs e)
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.bunifuCustomLabel1 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.bunifuCustomLabel2 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.bunifuCustomLabel3 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.bunifuCustomLabel4 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.bunifuCustomLabel5 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.bunifuCustomLabel6 = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Location = new System.Drawing.Point(-9, -3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(281, 37);
            this.panel1.TabIndex = 13;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown_1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(7, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 36);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(110, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 17);
            this.label1.TabIndex = 23;
            this.label1.Text = "Settings";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(200, -1);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(25, 37);
            this.button3.TabIndex = 22;
            this.button3.Text = "—";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.Color.White;
            this.button4.Location = new System.Drawing.Point(228, -1);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(25, 37);
            this.button4.TabIndex = 21;
            this.button4.Text = "✕";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // bunifuCustomLabel1
            // 
            this.bunifuCustomLabel1.AutoSize = true;
            this.bunifuCustomLabel1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel1.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel1.Location = new System.Drawing.Point(10, 81);
            this.bunifuCustomLabel1.Name = "bunifuCustomLabel1";
            this.bunifuCustomLabel1.Size = new System.Drawing.Size(64, 17);
            this.bunifuCustomLabel1.TabIndex = 15;
            this.bunifuCustomLabel1.Text = "Top Most";
            // 
            // bunifuCustomLabel2
            // 
            this.bunifuCustomLabel2.AutoSize = true;
            this.bunifuCustomLabel2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel2.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel2.Location = new System.Drawing.Point(10, 55);
            this.bunifuCustomLabel2.Name = "bunifuCustomLabel2";
            this.bunifuCustomLabel2.Size = new System.Drawing.Size(75, 17);
            this.bunifuCustomLabel2.TabIndex = 17;
            this.bunifuCustomLabel2.Text = "Auto Attach";
            this.bunifuCustomLabel2.Click += new System.EventHandler(this.bunifuCustomLabel2_Click);
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // bunifuCustomLabel3
            // 
            this.bunifuCustomLabel3.AutoSize = true;
            this.bunifuCustomLabel3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel3.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel3.Location = new System.Drawing.Point(12, 108);
            this.bunifuCustomLabel3.Name = "bunifuCustomLabel3";
            this.bunifuCustomLabel3.Size = new System.Drawing.Size(123, 17);
            this.bunifuCustomLabel3.TabIndex = 15;
            this.bunifuCustomLabel3.Text = "Opacity Fade-in/out";
            // 
            // bunifuCustomLabel4
            // 
            this.bunifuCustomLabel4.AutoSize = true;
            this.bunifuCustomLabel4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel4.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel4.Location = new System.Drawing.Point(12, 195);
            this.bunifuCustomLabel4.Name = "bunifuCustomLabel4";
            this.bunifuCustomLabel4.Size = new System.Drawing.Size(116, 17);
            this.bunifuCustomLabel4.TabIndex = 21;
            this.bunifuCustomLabel4.Text = "Install missing files";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(155, 195);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(62, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "INSTALL";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // bunifuCustomLabel5
            // 
            this.bunifuCustomLabel5.AutoSize = true;
            this.bunifuCustomLabel5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel5.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel5.Location = new System.Drawing.Point(12, 134);
            this.bunifuCustomLabel5.Name = "bunifuCustomLabel5";
            this.bunifuCustomLabel5.Size = new System.Drawing.Size(124, 17);
            this.bunifuCustomLabel5.TabIndex = 15;
            this.bunifuCustomLabel5.Text = "Remove Crash Logs";
            // 
            // bunifuCustomLabel6
            // 
            this.bunifuCustomLabel6.AutoSize = true;
            this.bunifuCustomLabel6.Enabled = false;
            this.bunifuCustomLabel6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuCustomLabel6.ForeColor = System.Drawing.Color.White;
            this.bunifuCustomLabel6.Location = new System.Drawing.Point(12, 163);
            this.bunifuCustomLabel6.Name = "bunifuCustomLabel6";
            this.bunifuCustomLabel6.Size = new System.Drawing.Size(100, 17);
            this.bunifuCustomLabel6.TabIndex = 15;
            this.bunifuCustomLabel6.Text = "Toggle Monaco";
            this.bunifuCustomLabel6.Click += new System.EventHandler(this.bunifuCustomLabel6_Click);
            // 
            // settings
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(248, 230);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.bunifuCustomLabel4);
            this.Controls.Add(this.bunifuCustomLabel2);
            this.Controls.Add(this.bunifuCustomLabel6);
            this.Controls.Add(this.bunifuCustomLabel5);
            this.Controls.Add(this.bunifuCustomLabel3);
            this.Controls.Add(this.bunifuCustomLabel1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.settings_FormClosing);
            this.Load += new System.EventHandler(this.krnl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}

	private void toggleSliderComponent3_Load(object sender, EventArgs e)
	{
	}

	private void toggleSliderComponent3_CheckChanged(object sender, EventArgs e)
	{
		Settings.Default.fadein_out_opacity = toggleSliderComponent3.Checked;
		if (Settings.Default.fadein_out_opacity)
		{
			while (parent.Opacity > 0.5)
			{
				Task.Delay(10).GetAwaiter().GetResult();
				parent.Opacity -= 0.05;
			}
		}
		else
		{
			while (parent.Opacity < 1.0)
			{
				Task.Delay(10).GetAwaiter().GetResult();
				parent.Opacity += 0.05;
			}
		}
		Settings.Default.Save();
	}

	private void button1_Click_2(object sender, EventArgs e)
	{
		MessageBox.Show("Coming Soon", "KRNL", 0);
	}

	private void toggleSliderComponent4_CheckChanged(object sender, EventArgs e)
	{
		Settings.Default.remove_crash_logs = toggleSliderComponent4.Checked;
		Settings.Default.Save();
	}

	private void toggleSliderComponent4_Load(object sender, EventArgs e)
	{
	}

	private void toggleSliderComponent5_Load(object sender, EventArgs e)
	{
	}

	private void toggleSliderComponent5_CheckChanged(object sender, EventArgs e)
	{
		if (toggleSliderComponent5.Checked)
		{
			toggleSliderComponent5.Checked = false;
			MessageBox.Show(new Form
			{
				TopMost = true
			}, "Monaco is disabled! Use the beta UI for Monaco instead!");
			Settings.Default.monaco = false;
		}
		Settings.Default.Save();
	}

    private void bunifuCustomLabel2_Click(object sender, EventArgs e)
    {

    }

    private void bunifuCustomLabel6_Click(object sender, EventArgs e)
    {

    }
}

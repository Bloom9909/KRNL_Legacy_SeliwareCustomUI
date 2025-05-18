using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using CefSharp;
using CefSharp.WinForms;
using Controls;
using krnlss.Properties;
using Microsoft.Win32;
using SeliwareAPI;
using SirhurtUI.Controls;

namespace krnlss;

public class krnl_monaco : Form
{
	public class BrowserMenuRenderer : ToolStripProfessionalRenderer
	{
		public BrowserMenuRenderer()
			: base(new BrowserColors())
		{
		}
	}

	public class BrowserColors : ProfessionalColorTable
	{
		public override Color ToolStripDropDownBackground => Color.FromArgb(40, 40, 40);

		public override Color ImageMarginGradientBegin => Color.FromArgb(40, 40, 40);

		public override Color ImageMarginGradientMiddle => Color.FromArgb(40, 40, 40);

		public override Color ImageMarginGradientEnd => Color.FromArgb(40, 40, 40);

		public override Color MenuBorder => Color.FromArgb(45, 45, 45);

		public override Color MenuItemBorder => Color.FromArgb(45, 45, 45);

		public override Color MenuItemSelected => Color.FromArgb(45, 45, 45);

		public override Color MenuStripGradientBegin => Color.FromArgb(40, 40, 40);

		public override Color MenuStripGradientEnd => Color.FromArgb(45, 45, 45);

		public override Color MenuItemSelectedGradientBegin => Color.FromArgb(40, 40, 40);

		public override Color MenuItemSelectedGradientEnd => Color.FromArgb(40, 40, 40);

		public override Color MenuItemPressedGradientBegin => Color.FromArgb(40, 40, 40);

		public override Color MenuItemPressedGradientEnd => Color.FromArgb(40, 40, 40);
	}

	private enum EdgeEnum
	{
		None,
		Right,
		Left,
		Top,
		Bottom,
		TopLeft,
		BottomRight
	}

	public const int WM_NCLBUTTONDOWN = 161;

	public const int HT_CAPTION = 2;

	private dynamic ScriptPath = Settings.Default.ScriptPath;

	public TabPanelControl tpc = new TabPanelControl();

	public bool changed;

	private IContainer components;

	public Panel panel1;

	private Label label1;

	private ToolStripMenuItem clearToolStripMenuItem;

	private ToolStripMenuItem openIntoToolStripMenuItem;

	private ToolStripMenuItem saveToolStripMenuItem;

	private ToolStripMenuItem renameToolStripMenuItem;

	public ContextMenuStrip TabContextMenu;

	public MonacoCustomTabControl customTabControl1;

	private TabPage tabPage1;

	private TreeView ScriptView;

	private BunifuFlatButton bunifuFlatButton1;

	private BunifuFlatButton bunifuFlatButton2;

	private BunifuFlatButton bunifuFlatButton3;

	private BunifuFlatButton bunifuFlatButton4;

	public BunifuFlatButton bunifuFlatButton5;

	private ContextMenuStrip contextMenuStrip1;

	private ToolStripMenuItem executeToolStripMenuItem;

	private ToolStripMenuItem loadIntoEditorToolStripMenuItem;

	private ToolStripMenuItem deleteFileToolStripMenuItem;

	private ToolStripMenuItem changePathToolStripMenuItem;

	private ToolStripMenuItem reloadToolStripMenuItem;

	private BunifuFlatButton bunifuFlatButton6;

	private MenuStrip menuStrip1;

	private ToolStripMenuItem fileToolStripMenuItem;

	private ToolStripMenuItem injectToolStripMenuItem;

	private ToolStripMenuItem aboutToolStripMenuItem;

	private ToolStripMenuItem gamesToolStripMenuItem;

	private ToolStripMenuItem hotScriptsToolStripMenuItem;

	private ToolStripMenuItem openGuiToolStripMenuItem;

	private ToolStripMenuItem toolStripMenuItem1;

	private ToolStripMenuItem killRobloxToolStripMenuItem;

	private ToolStripMenuItem remoteSpyToolStripMenuItem;

	private ToolStripMenuItem toolStripMenuItem2;

	private ToolStripMenuItem toolStripMenuItem3;

    private System.Windows.Forms.Timer timer1;

    private ToolStripMenuItem toolStripMenuItem4;

	private ToolTip toolTip1;

	private ToolStripMenuItem toolStripMenuItem5;

	private ToolStripMenuItem toolStripMenuItem6;

	private ToolStripMenuItem toolStripMenuItem7;

	private Panel panel2;

	public Panel panel3;

	private ErrorProvider errorProvider1;

	private Button button1;

	private Button button2;

	private PictureBox pictureBox2;

	private ToolStripMenuItem toolStripMenuItem8;

	private ToolStripMenuItem toolStripMenuItem10;

	private ToolStripMenuItem unnamedESPToolStripMenuItem;

	public static int injectedPID = 0;

	public static RegistryKey SOFTWARE = Registry.CurrentUser.OpenSubKey("SOFTWARE", writable: true);

	public static bool activated = false;

	public static bool launcherDetected = false;

	public static double timeout = 6.0;

	private EdgeEnum mEdge;

	private bool isonEdge;

	private int mWidth = 20;

	private bool mMouseDown;

	private bool heightUnchanged = true;

	private bool widthUnchanged = true;

	private ToolStripMenuItem cMDXToolStripMenuItem;

	private bool Anim_ATF_break;

	[DllImport("user32.dll", SetLastError = true)]
	internal static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

	[DllImport("user32.dll")]
	public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	public void PopulateTree(dynamic dir, TreeNode node)
	{
		try
		{
			dynamic val = new DirectoryInfo(dir);
			foreach (dynamic directory in val.GetDirectories())
			{
				dynamic val2 = new TreeNode(directory.Name);
				if (node != null && 0 == 0)
				{
					node.Nodes.Add(val2);
				}
				else
				{
					ScriptView.Nodes.Add(val2);
				}
				this.PopulateTree(directory.FullName, val2);
			}
			foreach (dynamic file in val.GetFiles())
			{
				dynamic val3 = new TreeNode(file.Name);
				if (node != null)
				{
					node.Nodes.Add(val3);
				}
				else
				{
					ScriptView.Nodes.Add(val3);
				}
			}
		}
		catch
		{
		}
	}

	private void ScriptLoading()
	{
		try
		{
			dynamic val = Directory.Exists(Settings.Default.ScriptPath);
			if ((!val))
			{
				Directory.CreateDirectory(Settings.Default.ScriptPath);
			}
		}
		catch
		{
		}
		PopulateTree(Settings.Default.ScriptPath, null);
	}

	public krnl_monaco()
	{
		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		InitializeComponent();
		panel3.Width = base.Width;
		MonacoCustomTabControl.Form1 = this;
		customTabControl1.ShowClosingButton = true;
	}

	private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		Settings.Default.monaco = false;
		Settings.Default.Save();
		Exception ex = (Exception)e.ExceptionObject;
		File.WriteAllText("error.txt", string.Join("\n", "Message: " + ex.Message, "StackTrace: " + ex.StackTrace, "Source: " + ex.Source, "TargetSite: " + ex.TargetSite, "HResult: " + ex.HResult, "HelpLink: " + ex.HelpLink, "Values: [ " + string.Join("\n", ex.Data.Values) + " ]"));
		MessageBox.Show(new Form
		{
			TopMost = true
		}, "Send `error.txt` to krnl server", "Caught an oopsies!");
		if (MessageBox.Show(new Form
		{
			TopMost = true
		}, "Click `Yes` if you want to get an invite to krnl discord server.", "Krnl Prompt", MessageBoxButtons.YesNo) == DialogResult.Yes)
		{
			Process.Start("https://" + Program.domain + "/invite");
		}
		Process.Start(Process.GetCurrentProcess().MainModule.FileName);
	}

	private void Form1_Load(object sender, EventArgs e)
	{
		for (int i = 0; i < hotScriptsToolStripMenuItem.DropDownItems.Count; i++)
		{
			ToolStripItem toolStripItem = hotScriptsToolStripMenuItem.DropDownItems[i];
			if (toolStripItem.Text == "Owl Hub" || toolStripItem.Text == "Galaxy Hub")
			{
				toolStripItem.Visible = false;
				toolStripItem.Enabled = false;
			}
		}
		if (!Directory.Exists("bin/tabs"))
		{
			Directory.CreateDirectory("bin/tabs");
		}
		if (Directory.GetFiles("bin/tabs").Length != 0)
		{
			for (int j = 0; j < Directory.GetFiles("bin/tabs").Length / 2; j++)
			{
				_ = customTabControl1.TabPages.Count;
				int num = j;
				using FileStream stream = new FileStream($"bin/tabs/{num}_source.lua", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
				using StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
				string content = streamReader.ReadToEnd();
				customTabControl1.addScript(Program.db64(content));
				streamReader.Close();
			}
			for (int k = 0; k < Directory.GetFiles("bin/tabs").Length / 2; k++)
			{
				customTabControl1.addnewtab();
				int count = customTabControl1.TabPages.Count - 2;
				int curr = k;
				if (k + 1 != Directory.GetFiles("bin/tabs").Length / 2)
				{
					continue;
				}
				ChromiumWebBrowser editor = customTabControl1.GetWorkingTextEditor();
				editor.LoadingStateChanged += delegate(object obj2, LoadingStateChangedEventArgs args)
				{
					if (!args.IsLoading && args.CanReload && args.Browser.HasDocument && editor.CanExecuteJavascriptInMainFrame)
					{
						try
						{
							using (FileStream stream2 = new FileStream($"bin/tabs/{curr}_name.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
							{
								using StreamReader streamReader2 = new StreamReader(stream2, Encoding.UTF8);
								customTabControl1.TabPages[count].Text = Program.db64(streamReader2.ReadToEnd());
								streamReader2.Close();
							}
							using FileStream stream3 = new FileStream($"bin/tabs/{curr}_source.lua", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
							using StreamReader streamReader3 = new StreamReader(stream3, Encoding.UTF8);
                            CefSharp.WebBrowserExtensions.ExecuteScriptAsync((IWebBrowser)(object)editor, "SetText", new object[1] { Program.db64(streamReader3.ReadToEnd()) });
							streamReader3.Close();
						}
						catch
						{
						}
					}
				};
			}
		}
		else
		{
			customTabControl1.addnewtab();
		}
		if (!Directory.Exists(Settings.Default.ScriptPath))
		{
			Settings.Default.ScriptPath = Environment.CurrentDirectory + "\\scripts";
		}
		menuStrip1.Renderer = new ToolStripProfessionalRenderer(new BrowserColors());
		ScriptLoading();
		Anim_ATF_break = true;
		anim_AwaitingTaskFinish();
	}

	private async void button1_Click(object sender, EventArgs e)
	{
		while (base.Opacity > 0.0)
		{
			await Task.Delay(10);
			base.Opacity -= 0.1;
		}
		string[] files = Directory.GetFiles("bin/tabs");
		for (int i = 0; i < ((files.Length != 0) ? (files.Length / 2) : 0); i++)
		{
			if (customTabControl1.TabPages.Count <= i + 1)
			{
				try
				{
					File.Delete($"bin/tabs/{i}_name.txt");
					File.Delete($"bin/tabs/{i}_source.lua");
				}
				catch
				{
				}
			}
		}
		string text = CefSharp.WebBrowserExtensions.EvaluateScriptAsync((IWebBrowser)(object)customTabControl1.GetWorkingTextEditor(), "GetText()", (TimeSpan?)null).GetAwaiter().GetResult()
			.Result.ToString();
		Program.tabScripts[customTabControl1.realIndex] = ((text != "-- Krnl Monaco") ? text : Program.tabScripts[customTabControl1.realIndex]);
		for (int j = 0; j < customTabControl1.TabCount - 1; j++)
		{
			File.WriteAllText($"bin/tabs/{j}_name.txt", customTabControl1.TabPages[j].Text);
			File.WriteAllText($"bin/tabs/{j}_source.lua", Program.tabScripts[j]);
		}
		Environment.Exit(Environment.ExitCode);
	}

	private void button2_Click(object sender, EventArgs e)
	{
		base.WindowState = FormWindowState.Minimized;
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

	private void tabPage1_Click(object sender, EventArgs e)
	{
	}

	private void closeToolStripMenuItem_Click(object sender, EventArgs e)
	{
		TabPage contextTab = customTabControl1.contextTab;
		customTabControl1.CloseTab(contextTab);
	}

	private void clearToolStripMenuItem_Click(object sender, EventArgs e)
	{
        CefSharp.WebBrowserExtensions.ExecuteScriptAsync((IWebBrowser)(object)customTabControl1.GetWorkingTextEditor(), "SetText", new object[1] { "" });
	}

	private void openIntoToolStripMenuItem_Click(object sender, EventArgs e)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		TabPage contextTab = customTabControl1.contextTab;
		if (contextTab == null)
		{
			throw new Exception("SELECTED TAB NOT FOUND");
		}
		using OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.CheckFileExists = true;
		openFileDialog.Filter = "Script Files (*.txt, *.lua)|*.txt;*.lua|All Files (*.*)|*.*";
		openFileDialog.RestoreDirectory = true;
		if (openFileDialog.ShowDialog() == DialogResult.OK)
		{
			contextTab.Text = Path.GetFileNameWithoutExtension(openFileDialog.SafeFileName);
			object obj = File.ReadAllText(openFileDialog.FileName);
			try
			{
				Control control = customTabControl1.contextTab.Controls[0];
                CefSharp.WebBrowserExtensions.ExecuteScriptAsync((IWebBrowser)(ChromiumWebBrowser)((control is ChromiumWebBrowser) ? control : null), "SetText", new object[1] { obj });
				return;
			}
			catch
			{
				return;
			}
		}
	}

	private void saveToolStripMenuItem_Click(object sender, EventArgs e)
	{
		TabPage contextTab = customTabControl1.contextTab;
		if (contextTab == null)
		{
			throw new Exception("TAB NOT FOUND");
		}
		contextTab.Text = customTabControl1.OpenSaveDialog(contextTab, CefSharp.WebBrowserExtensions.EvaluateScriptAsync((IWebBrowser)(object)customTabControl1.GetWorkingTextEditor(), "GetText", new object[0]).GetAwaiter().GetResult()
			.Result.ToString());
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool WaitNamedPipe(string name, int timeout);

	public static bool findpipe(string pipeName)
	{
		if (!WaitNamedPipe(Path.GetFullPath("\\\\.\\pipe\\" + pipeName), 0) && (Marshal.GetLastWin32Error() == 0 || Marshal.GetLastWin32Error() == 2))
		{
			return false;
		}
		return true;
	}

	public static void pipeshit(string script)
	{
		try
		{
			if (findpipe("krnlpipe"))
			{
				using (NamedPipeClientStream namedPipeClientStream = new NamedPipeClientStream(".", "krnlpipe", PipeDirection.Out))
				{
					namedPipeClientStream.Connect();
					if (!namedPipeClientStream.IsConnected)
					{
						throw new IOException("Failed To Connect To Pipe....");
					}
					StreamWriter streamWriter = new StreamWriter(namedPipeClientStream, Encoding.Default, 999999);
					streamWriter.Write(script);
					streamWriter.Dispose();
					return;
				}
			}
			MessageBox.Show(new Form
			{
				TopMost = true
			}, "Please Inject To Execute Scripts", "krnl");
		}
		catch (Exception)
		{
		}
	}

	public static void Pipe(string script)
	{
		Program.execute_script(script);
	}

	private void bunifuFlatButton1_Click(object sender, EventArgs e)
	{
		try
		{
			Pipe(CefSharp.WebBrowserExtensions.EvaluateScriptAsync((IWebBrowser)(object)customTabControl1.GetWorkingTextEditor(), "GetText", new object[0]).GetAwaiter().GetResult()
				.Result.ToString());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void bunifuFlatButton2_Click(object sender, EventArgs e)
	{
        CefSharp.WebBrowserExtensions.ExecuteScriptAsync((IWebBrowser)(object)customTabControl1.GetWorkingTextEditor(), "SetText", new object[1] { "" });
	}

	private void bunifuFlatButton3_Click(object sender, EventArgs e)
	{
		customTabControl1.OpenFileDialog(customTabControl1.SelectedTab);
	}

	private void bunifuFlatButton4_Click(object sender, EventArgs e)
	{
		ScriptView.Nodes.Clear();
		ScriptLoading();
		customTabControl1.OpenSaveDialog(customTabControl1.SelectedTab, "");
	}

	private void injectToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void bunifuFlatButton5_Click(object sender, EventArgs e)
	{
		Program.Inject();
	}

	private void executeToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			dynamic fullPath = ScriptView.SelectedNode.FullPath;
			dynamic val = File.ReadAllText(Settings.Default.ScriptPath + "//" + fullPath);
			Pipe(val);
		}
		catch
		{
		}
	}

	private void loadIntoEditorToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			dynamic fullPath = ScriptView.SelectedNode.FullPath;
			object obj = File.ReadAllText(Settings.Default.ScriptPath + "//" + fullPath);
            CefSharp.WebBrowserExtensions.ExecuteScriptAsync((IWebBrowser)(object)customTabControl1.GetWorkingTextEditor(), "SetText", new object[1] { obj });
		}
		catch (Exception)
		{
		}
	}

	private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			dynamic fullPath = ScriptView.SelectedNode.FullPath;
			File.Delete(Settings.Default.ScriptPath + "//" + fullPath);
		}
		catch (Exception)
		{
		}
	}

	private void changePathToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			dynamic val = new FolderBrowserDialog();
			using ((IDisposable)val)
			{
				dynamic val2 = val.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(val.SelectedPath);
				if (val2)
				{
					ScriptPath = val.SelectedPath;
					Settings.Default.ScriptPath = val.SelectedPath;
					Settings.Default.Save();
				}
			}
			ScriptView.Nodes.Clear();
			ScriptLoading();
		}
		catch
		{
		}
	}

	private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
	{
		ScriptView.Nodes.Clear();
		ScriptLoading();
	}

	private void ScriptView_AfterSelect(object sender, TreeViewEventArgs e)
	{
	}

	private void customTabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void renameToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void openGuiToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://pastebin.com/raw/UXmbai5q', true))()");
	}

	private void toolStripMenuItem1_Click(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://raw.githubusercontent.com/CriShoux/OwlHub/master/OwlHub.txt'))();");
	}

	private void toolStripMenuItem2_Click(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://raw.githubusercontent.com/Babyhamsta/RBLX_Scripts/main/Universal/BypassedDarkDexV3.lua', true))()");
	}

	private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void bunifuFlatButton6_Click(object sender, EventArgs e)
	{
		if (Application.OpenForms.OfType<settings>().Count() != 1)
		{
			new settings(this).Show();
			Application.OpenForms.OfType<settings>().First().SetDesktopLocation(base.Location.X + base.Size.Width + 5, base.Location.Y);
		}
	}

	private void gamesToolStripMenuItem_Click_1(object sender, EventArgs e)
	{
		MessageBox.Show(new Form
		{
			TopMost = true
		}, "Disabled as most scripts are patched.");
	}

	private void aboutToolStripMenuItem_Click_1(object sender, EventArgs e)
	{
		if (Application.OpenForms.OfType<About>().Count() != 1)
		{
			new About().Show();
			Application.OpenForms.OfType<About>().First().SetDesktopLocation(base.Location.X + base.Size.Width + 5, base.Location.Y);
		}
	}

	private void injectToolStripMenuItem_Click_1(object sender, EventArgs e)
	{
		Program.Inject();
	}

	private void openGuiToolStripMenuItem_Click_1(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://pastebin.com/raw/UXmbai5q', true))()");
	}

	private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://raw.githubusercontent.com/CriShoux/OwlHub/master/OwlHub.txt'))();");
	}

	private void killRobloxToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Process[] processesByName = Process.GetProcessesByName("Windows10Universal");
		int num = 0;
		for (int i = 0; i < processesByName.Length; i++)
		{
			processesByName[i].Kill();
			num++;
		}
		MessageBox.Show($"Terminated {num} Process", "krnl");
	}

	private void krnl_FormClosing(object sender, FormClosingEventArgs e)
	{
		string[] files = Directory.GetFiles("bin/tabs");
		for (int i = 0; i < ((files.Length != 0) ? (files.Length / 2) : 0); i++)
		{
			if (customTabControl1.TabPages.Count <= i + 1)
			{
				try
				{
					File.Delete($"bin/tabs/{i}_name.txt");
					File.Delete($"bin/tabs/{i}_source.lua");
				}
				catch
				{
				}
			}
		}
		string text = CefSharp.WebBrowserExtensions.EvaluateScriptAsync((IWebBrowser)(object)customTabControl1.GetWorkingTextEditor(), "GetText", new object[0]).GetAwaiter().GetResult()
			.Result.ToString();
		Program.tabScripts[customTabControl1.realIndex] = ((text != "-- Krnl Monaco") ? text : Program.tabScripts[customTabControl1.realIndex]);
		for (int j = 0; j < customTabControl1.TabCount - 1; j++)
		{
			File.WriteAllText($"bin/tabs/{j}_name.txt", customTabControl1.TabPages[j].Text);
			File.WriteAllText($"bin/tabs/{j}_source.lua", Program.tabScripts[j]);
		}
		Environment.Exit(Environment.ExitCode);
	}

	private async void krnl_Deactivate(object sender, EventArgs e)
	{
	}

	protected override async void OnActivated(EventArgs e)
	{
		activated = true;
		if (Settings.Default.fadein_out_opacity)
		{
			while (base.Opacity < 1.0 && activated)
			{
				await Task.Delay(10);
				base.Opacity += 0.05;
			}
		}
		else
		{
			base.Opacity = 1.0;
		}
	}

	protected override async void OnDeactivate(EventArgs e)
	{
		activated = false;
		if (Settings.Default.fadein_out_opacity)
		{
			while (base.Opacity > 0.5 && !activated)
			{
				await Task.Delay(10);
				base.Opacity -= 0.05;
			}
		}
		else
		{
			base.Opacity = 1.0;
		}
	}

	private async void krnl_Activated(object sender, EventArgs e)
	{
	}

	private void gameSenseToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://pastebin.com/raw/rPnPiYZV'))();");
	}

	private void remoteSpyToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://pastebin.com/raw/JZaJe9Sg'))();");
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(krnlss.krnl_monaco));
		this.panel1 = new System.Windows.Forms.Panel();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.button2 = new System.Windows.Forms.Button();
		this.button1 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.TabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.openIntoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
		this.ScriptView = new System.Windows.Forms.TreeView();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.loadIntoEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.deleteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.changePathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.reloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.bunifuFlatButton1 = new Bunifu.Framework.UI.BunifuFlatButton();
		this.bunifuFlatButton2 = new Bunifu.Framework.UI.BunifuFlatButton();
		this.bunifuFlatButton3 = new Bunifu.Framework.UI.BunifuFlatButton();
		this.bunifuFlatButton4 = new Bunifu.Framework.UI.BunifuFlatButton();
		this.bunifuFlatButton5 = new Bunifu.Framework.UI.BunifuFlatButton();
		this.bunifuFlatButton6 = new Bunifu.Framework.UI.BunifuFlatButton();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.injectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.killRobloxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.gamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.hotScriptsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
		this.openGuiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
		this.remoteSpyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
		this.unnamedESPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
		this.cMDXToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.panel2 = new System.Windows.Forms.Panel();
		this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
		this.customTabControl1 = new Controls.MonacoCustomTabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		this.TabContextMenu.SuspendLayout();
		this.contextMenuStrip1.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider1).BeginInit();
		this.customTabControl1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.BackColor = System.Drawing.Color.FromArgb(29, 29, 29);
		this.panel1.Controls.Add(this.pictureBox2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.button2);
		this.panel1.Controls.Add(this.button1);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(690, 33);
		this.panel1.TabIndex = 0;
		this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(panel1_Paint);
		this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(panel1_MouseMove);
		this.pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
		this.pictureBox2.Location = new System.Drawing.Point(4, 4);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(25, 25);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox2.TabIndex = 8;
		this.pictureBox2.TabStop = false;
		this.panel3.AutoSize = true;
		this.panel3.BackColor = System.Drawing.Color.DodgerBlue;
		this.panel3.Location = new System.Drawing.Point(0, 1);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(682, 3);
		this.panel3.TabIndex = 5;
		this.panel3.MouseMove += new System.Windows.Forms.MouseEventHandler(krnl_MouseMove);
		this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.button2.BackColor = System.Drawing.Color.FromArgb(29, 29, 29);
		this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.button2.FlatAppearance.BorderSize = 0;
		this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button2.Font = new System.Drawing.Font("Corbel", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button2.ForeColor = System.Drawing.Color.White;
		this.button2.Image = (System.Drawing.Image)resources.GetObject("button2.Image");
		this.button2.Location = new System.Drawing.Point(620, 0);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(35, 33);
		this.button2.TabIndex = 7;
		this.button2.UseVisualStyleBackColor = false;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
		this.button1.BackColor = System.Drawing.Color.FromArgb(29, 29, 29);
		this.button1.FlatAppearance.BorderSize = 0;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Corbel", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Image = (System.Drawing.Image)resources.GetObject("button1.Image");
		this.button1.Location = new System.Drawing.Point(655, 0);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(35, 33);
		this.button1.TabIndex = 6;
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.button1.MouseMove += new System.Windows.Forms.MouseEventHandler(krnl_MouseMove);
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(328, 7);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(45, 20);
		this.label1.TabIndex = 0;
		this.label1.Text = "KRNL";
		this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(panel1_MouseMove);
		this.TabContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.TabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.clearToolStripMenuItem, this.openIntoToolStripMenuItem, this.saveToolStripMenuItem, this.toolStripMenuItem10 });
		this.TabContextMenu.Name = "TabContextMenu";
		this.TabContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
		this.TabContextMenu.Size = new System.Drawing.Size(128, 92);
		this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
		this.clearToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
		this.clearToolStripMenuItem.Text = "Clear";
		this.clearToolStripMenuItem.Click += new System.EventHandler(clearToolStripMenuItem_Click);
		this.openIntoToolStripMenuItem.Name = "openIntoToolStripMenuItem";
		this.openIntoToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
		this.openIntoToolStripMenuItem.Text = "Open Into";
		this.openIntoToolStripMenuItem.Click += new System.EventHandler(openIntoToolStripMenuItem_Click);
		this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
		this.saveToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
		this.saveToolStripMenuItem.Text = "Save";
		this.saveToolStripMenuItem.Click += new System.EventHandler(saveToolStripMenuItem_Click);
		this.toolStripMenuItem10.Name = "toolStripMenuItem10";
		this.toolStripMenuItem10.Size = new System.Drawing.Size(127, 22);
		this.toolStripMenuItem10.Text = "Rename";
		this.toolStripMenuItem10.Click += new System.EventHandler(toolStripMenuItem10_Click);
		this.ScriptView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.ScriptView.BackColor = System.Drawing.Color.FromArgb(29, 29, 29);
		this.ScriptView.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.ScriptView.ContextMenuStrip = this.contextMenuStrip1;
		this.ScriptView.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ScriptView.ForeColor = System.Drawing.Color.White;
		this.ScriptView.HideSelection = false;
		this.ScriptView.LineColor = System.Drawing.Color.White;
		this.ScriptView.Location = new System.Drawing.Point(565, 59);
		this.ScriptView.Name = "ScriptView";
		this.ScriptView.Size = new System.Drawing.Size(121, 259);
		this.ScriptView.TabIndex = 4;
		this.ScriptView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(ScriptView_AfterSelect);
		this.ScriptView.MouseMove += new System.Windows.Forms.MouseEventHandler(krnl_MouseMove);
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.executeToolStripMenuItem, this.loadIntoEditorToolStripMenuItem, this.deleteFileToolStripMenuItem, this.changePathToolStripMenuItem, this.reloadToolStripMenuItem });
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(159, 114);
		this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
		this.executeToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.executeToolStripMenuItem.Text = "Execute";
		this.executeToolStripMenuItem.Click += new System.EventHandler(executeToolStripMenuItem_Click);
		this.loadIntoEditorToolStripMenuItem.Name = "loadIntoEditorToolStripMenuItem";
		this.loadIntoEditorToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.loadIntoEditorToolStripMenuItem.Text = "Load Into Editor";
		this.loadIntoEditorToolStripMenuItem.Click += new System.EventHandler(loadIntoEditorToolStripMenuItem_Click);
		this.deleteFileToolStripMenuItem.Name = "deleteFileToolStripMenuItem";
		this.deleteFileToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.deleteFileToolStripMenuItem.Text = "Delete File";
		this.deleteFileToolStripMenuItem.Click += new System.EventHandler(deleteFileToolStripMenuItem_Click);
		this.changePathToolStripMenuItem.Name = "changePathToolStripMenuItem";
		this.changePathToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.changePathToolStripMenuItem.Text = "Change Path";
		this.changePathToolStripMenuItem.Click += new System.EventHandler(changePathToolStripMenuItem_Click);
		this.reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
		this.reloadToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
		this.reloadToolStripMenuItem.Text = "Reload";
		this.reloadToolStripMenuItem.Click += new System.EventHandler(reloadToolStripMenuItem_Click);
		this.bunifuFlatButton1.Activecolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.bunifuFlatButton1.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.bunifuFlatButton1.BorderRadius = 0;
		this.bunifuFlatButton1.ButtonText = "EXECUTE";
		this.bunifuFlatButton1.Cursor = System.Windows.Forms.Cursors.Hand;
		this.bunifuFlatButton1.DisabledColor = System.Drawing.Color.Gray;
		this.bunifuFlatButton1.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton1.Iconcolor = System.Drawing.Color.Transparent;
		this.bunifuFlatButton1.Iconimage = null;
		this.bunifuFlatButton1.Iconimage_right = null;
		this.bunifuFlatButton1.Iconimage_right_Selected = null;
		this.bunifuFlatButton1.Iconimage_Selected = null;
		this.bunifuFlatButton1.IconMarginLeft = 0;
		this.bunifuFlatButton1.IconMarginRight = 0;
		this.bunifuFlatButton1.IconRightVisible = true;
		this.bunifuFlatButton1.IconRightZoom = 0.0;
		this.bunifuFlatButton1.IconVisible = true;
		this.bunifuFlatButton1.IconZoom = 20.0;
		this.bunifuFlatButton1.IsTab = false;
		this.bunifuFlatButton1.Location = new System.Drawing.Point(4, 321);
		this.bunifuFlatButton1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
		this.bunifuFlatButton1.MinimumSize = new System.Drawing.Size(84, 25);
		this.bunifuFlatButton1.Name = "bunifuFlatButton1";
		this.bunifuFlatButton1.Normalcolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton1.OnHovercolor = System.Drawing.Color.FromArgb(39, 39, 39);
		this.bunifuFlatButton1.OnHoverTextColor = System.Drawing.Color.White;
		this.bunifuFlatButton1.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.bunifuFlatButton1.selected = false;
		this.bunifuFlatButton1.Size = new System.Drawing.Size(100, 25);
		this.bunifuFlatButton1.TabIndex = 7;
		this.bunifuFlatButton1.Text = "EXECUTE";
		this.bunifuFlatButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.bunifuFlatButton1.Textcolor = System.Drawing.Color.White;
		this.bunifuFlatButton1.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton1.Click += new System.EventHandler(bunifuFlatButton1_Click);
		this.bunifuFlatButton2.Activecolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.bunifuFlatButton2.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.bunifuFlatButton2.BorderRadius = 0;
		this.bunifuFlatButton2.ButtonText = "CLEAR";
		this.bunifuFlatButton2.Cursor = System.Windows.Forms.Cursors.Hand;
		this.bunifuFlatButton2.DisabledColor = System.Drawing.Color.Gray;
		this.bunifuFlatButton2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton2.Iconcolor = System.Drawing.Color.Transparent;
		this.bunifuFlatButton2.Iconimage = null;
		this.bunifuFlatButton2.Iconimage_right = null;
		this.bunifuFlatButton2.Iconimage_right_Selected = null;
		this.bunifuFlatButton2.Iconimage_Selected = null;
		this.bunifuFlatButton2.IconMarginLeft = 0;
		this.bunifuFlatButton2.IconMarginRight = 0;
		this.bunifuFlatButton2.IconRightVisible = true;
		this.bunifuFlatButton2.IconRightZoom = 0.0;
		this.bunifuFlatButton2.IconVisible = true;
		this.bunifuFlatButton2.IconZoom = 20.0;
		this.bunifuFlatButton2.IsTab = false;
		this.bunifuFlatButton2.Location = new System.Drawing.Point(107, 321);
		this.bunifuFlatButton2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
		this.bunifuFlatButton2.MinimumSize = new System.Drawing.Size(84, 25);
		this.bunifuFlatButton2.Name = "bunifuFlatButton2";
		this.bunifuFlatButton2.Normalcolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton2.OnHovercolor = System.Drawing.Color.FromArgb(39, 39, 39);
		this.bunifuFlatButton2.OnHoverTextColor = System.Drawing.Color.White;
		this.bunifuFlatButton2.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.bunifuFlatButton2.selected = false;
		this.bunifuFlatButton2.Size = new System.Drawing.Size(100, 25);
		this.bunifuFlatButton2.TabIndex = 8;
		this.bunifuFlatButton2.Text = "CLEAR";
		this.bunifuFlatButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.bunifuFlatButton2.Textcolor = System.Drawing.Color.White;
		this.bunifuFlatButton2.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton2.Click += new System.EventHandler(bunifuFlatButton2_Click);
		this.bunifuFlatButton3.Activecolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.bunifuFlatButton3.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.bunifuFlatButton3.BorderRadius = 0;
		this.bunifuFlatButton3.ButtonText = "OPEN FILE";
		this.bunifuFlatButton3.Cursor = System.Windows.Forms.Cursors.Hand;
		this.bunifuFlatButton3.DisabledColor = System.Drawing.Color.Gray;
		this.bunifuFlatButton3.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton3.Iconcolor = System.Drawing.Color.Transparent;
		this.bunifuFlatButton3.Iconimage = null;
		this.bunifuFlatButton3.Iconimage_right = null;
		this.bunifuFlatButton3.Iconimage_right_Selected = null;
		this.bunifuFlatButton3.Iconimage_Selected = null;
		this.bunifuFlatButton3.IconMarginLeft = 0;
		this.bunifuFlatButton3.IconMarginRight = 0;
		this.bunifuFlatButton3.IconRightVisible = true;
		this.bunifuFlatButton3.IconRightZoom = 0.0;
		this.bunifuFlatButton3.IconVisible = true;
		this.bunifuFlatButton3.IconZoom = 20.0;
		this.bunifuFlatButton3.IsTab = false;
		this.bunifuFlatButton3.Location = new System.Drawing.Point(210, 321);
		this.bunifuFlatButton3.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
		this.bunifuFlatButton3.MinimumSize = new System.Drawing.Size(84, 25);
		this.bunifuFlatButton3.Name = "bunifuFlatButton3";
		this.bunifuFlatButton3.Normalcolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton3.OnHovercolor = System.Drawing.Color.FromArgb(39, 39, 39);
		this.bunifuFlatButton3.OnHoverTextColor = System.Drawing.Color.White;
		this.bunifuFlatButton3.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.bunifuFlatButton3.selected = false;
		this.bunifuFlatButton3.Size = new System.Drawing.Size(100, 25);
		this.bunifuFlatButton3.TabIndex = 9;
		this.bunifuFlatButton3.Text = "OPEN FILE";
		this.bunifuFlatButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.bunifuFlatButton3.Textcolor = System.Drawing.Color.White;
		this.bunifuFlatButton3.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton3.Click += new System.EventHandler(bunifuFlatButton3_Click);
		this.bunifuFlatButton4.Activecolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.bunifuFlatButton4.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.bunifuFlatButton4.BorderRadius = 0;
		this.bunifuFlatButton4.ButtonText = "SAVE FILE";
		this.bunifuFlatButton4.Cursor = System.Windows.Forms.Cursors.Hand;
		this.bunifuFlatButton4.DisabledColor = System.Drawing.Color.Gray;
		this.bunifuFlatButton4.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton4.Iconcolor = System.Drawing.Color.Transparent;
		this.bunifuFlatButton4.Iconimage = null;
		this.bunifuFlatButton4.Iconimage_right = null;
		this.bunifuFlatButton4.Iconimage_right_Selected = null;
		this.bunifuFlatButton4.Iconimage_Selected = null;
		this.bunifuFlatButton4.IconMarginLeft = 0;
		this.bunifuFlatButton4.IconMarginRight = 0;
		this.bunifuFlatButton4.IconRightVisible = true;
		this.bunifuFlatButton4.IconRightZoom = 0.0;
		this.bunifuFlatButton4.IconVisible = true;
		this.bunifuFlatButton4.IconZoom = 20.0;
		this.bunifuFlatButton4.IsTab = false;
		this.bunifuFlatButton4.Location = new System.Drawing.Point(313, 321);
		this.bunifuFlatButton4.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
		this.bunifuFlatButton4.MinimumSize = new System.Drawing.Size(84, 25);
		this.bunifuFlatButton4.Name = "bunifuFlatButton4";
		this.bunifuFlatButton4.Normalcolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton4.OnHovercolor = System.Drawing.Color.FromArgb(39, 39, 39);
		this.bunifuFlatButton4.OnHoverTextColor = System.Drawing.Color.White;
		this.bunifuFlatButton4.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.bunifuFlatButton4.selected = false;
		this.bunifuFlatButton4.Size = new System.Drawing.Size(100, 25);
		this.bunifuFlatButton4.TabIndex = 10;
		this.bunifuFlatButton4.Text = "SAVE FILE";
		this.bunifuFlatButton4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.bunifuFlatButton4.Textcolor = System.Drawing.Color.White;
		this.bunifuFlatButton4.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton4.Click += new System.EventHandler(bunifuFlatButton4_Click);
		this.bunifuFlatButton5.Activecolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.bunifuFlatButton5.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.bunifuFlatButton5.BorderRadius = 0;
		this.bunifuFlatButton5.ButtonText = "INJECT";
		this.bunifuFlatButton5.Cursor = System.Windows.Forms.Cursors.Hand;
		this.bunifuFlatButton5.DisabledColor = System.Drawing.Color.Gray;
		this.bunifuFlatButton5.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton5.Iconcolor = System.Drawing.Color.Transparent;
		this.bunifuFlatButton5.Iconimage = null;
		this.bunifuFlatButton5.Iconimage_right = null;
		this.bunifuFlatButton5.Iconimage_right_Selected = null;
		this.bunifuFlatButton5.Iconimage_Selected = null;
		this.bunifuFlatButton5.IconMarginLeft = 0;
		this.bunifuFlatButton5.IconMarginRight = 0;
		this.bunifuFlatButton5.IconRightVisible = true;
		this.bunifuFlatButton5.IconRightZoom = 0.0;
		this.bunifuFlatButton5.IconVisible = true;
		this.bunifuFlatButton5.IconZoom = 20.0;
		this.bunifuFlatButton5.IsTab = false;
		this.bunifuFlatButton5.Location = new System.Drawing.Point(416, 321);
		this.bunifuFlatButton5.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
		this.bunifuFlatButton5.MinimumSize = new System.Drawing.Size(84, 25);
		this.bunifuFlatButton5.Name = "bunifuFlatButton5";
		this.bunifuFlatButton5.Normalcolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton5.OnHovercolor = System.Drawing.Color.FromArgb(39, 39, 39);
		this.bunifuFlatButton5.OnHoverTextColor = System.Drawing.Color.White;
		this.bunifuFlatButton5.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.bunifuFlatButton5.selected = false;
		this.bunifuFlatButton5.Size = new System.Drawing.Size(100, 25);
		this.bunifuFlatButton5.TabIndex = 11;
		this.bunifuFlatButton5.Text = "INJECT";
		this.bunifuFlatButton5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.bunifuFlatButton5.Textcolor = System.Drawing.Color.White;
		this.bunifuFlatButton5.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton5.Click += new System.EventHandler(bunifuFlatButton5_Click);
		this.bunifuFlatButton6.Activecolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton6.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.bunifuFlatButton6.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.bunifuFlatButton6.BorderRadius = 0;
		this.bunifuFlatButton6.ButtonText = "OPTIONS";
		this.bunifuFlatButton6.Cursor = System.Windows.Forms.Cursors.Hand;
		this.bunifuFlatButton6.DisabledColor = System.Drawing.Color.Gray;
		this.bunifuFlatButton6.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton6.Iconcolor = System.Drawing.Color.Transparent;
		this.bunifuFlatButton6.Iconimage = null;
		this.bunifuFlatButton6.Iconimage_right = null;
		this.bunifuFlatButton6.Iconimage_right_Selected = null;
		this.bunifuFlatButton6.Iconimage_Selected = null;
		this.bunifuFlatButton6.IconMarginLeft = 0;
		this.bunifuFlatButton6.IconMarginRight = 0;
		this.bunifuFlatButton6.IconRightVisible = true;
		this.bunifuFlatButton6.IconRightZoom = 0.0;
		this.bunifuFlatButton6.IconVisible = true;
		this.bunifuFlatButton6.IconZoom = 20.0;
		this.bunifuFlatButton6.IsTab = false;
		this.bunifuFlatButton6.Location = new System.Drawing.Point(586, 321);
		this.bunifuFlatButton6.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
		this.bunifuFlatButton6.MinimumSize = new System.Drawing.Size(84, 25);
		this.bunifuFlatButton6.Name = "bunifuFlatButton6";
		this.bunifuFlatButton6.Normalcolor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.bunifuFlatButton6.OnHovercolor = System.Drawing.Color.FromArgb(39, 39, 39);
		this.bunifuFlatButton6.OnHoverTextColor = System.Drawing.Color.White;
		this.bunifuFlatButton6.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
		this.bunifuFlatButton6.selected = false;
		this.bunifuFlatButton6.Size = new System.Drawing.Size(100, 25);
		this.bunifuFlatButton6.TabIndex = 12;
		this.bunifuFlatButton6.Text = "OPTIONS";
		this.bunifuFlatButton6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.bunifuFlatButton6.Textcolor = System.Drawing.Color.White;
		this.bunifuFlatButton6.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.bunifuFlatButton6.Click += new System.EventHandler(bunifuFlatButton6_Click);
		this.menuStrip1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.menuStrip1.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
		this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.fileToolStripMenuItem, this.aboutToolStripMenuItem, this.gamesToolStripMenuItem, this.hotScriptsToolStripMenuItem, this.toolStripMenuItem5 });
		this.menuStrip1.Location = new System.Drawing.Point(0, 33);
		this.menuStrip1.Name = "menuStrip1";
		this.menuStrip1.Size = new System.Drawing.Size(289, 24);
		this.menuStrip1.TabIndex = 13;
		this.menuStrip1.Text = "menuStrip1";
		this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.injectToolStripMenuItem, this.killRobloxToolStripMenuItem });
		this.fileToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fileToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
		this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
		this.fileToolStripMenuItem.Text = "File";
		this.injectToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.injectToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.injectToolStripMenuItem.Name = "injectToolStripMenuItem";
		this.injectToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
		this.injectToolStripMenuItem.Text = "Inject";
		this.injectToolStripMenuItem.Click += new System.EventHandler(injectToolStripMenuItem_Click_1);
		this.killRobloxToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.killRobloxToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.killRobloxToolStripMenuItem.Name = "killRobloxToolStripMenuItem";
		this.killRobloxToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
		this.killRobloxToolStripMenuItem.Text = "Kill Roblox";
		this.killRobloxToolStripMenuItem.Click += new System.EventHandler(killRobloxToolStripMenuItem_Click);
		this.aboutToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.aboutToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
		this.aboutToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
		this.aboutToolStripMenuItem.Text = "Credits";
		this.aboutToolStripMenuItem.Click += new System.EventHandler(aboutToolStripMenuItem_Click_1);
		this.gamesToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gamesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.gamesToolStripMenuItem.Name = "gamesToolStripMenuItem";
		this.gamesToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
		this.gamesToolStripMenuItem.Text = "Games";
		this.gamesToolStripMenuItem.Click += new System.EventHandler(gamesToolStripMenuItem_Click_1);
		this.hotScriptsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripMenuItem2, this.openGuiToolStripMenuItem, this.toolStripMenuItem4, this.toolStripMenuItem1, this.remoteSpyToolStripMenuItem, this.toolStripMenuItem3, this.unnamedESPToolStripMenuItem, this.toolStripMenuItem8, this.cMDXToolStripMenuItem });
		this.hotScriptsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.hotScriptsToolStripMenuItem.Name = "hotScriptsToolStripMenuItem";
		this.hotScriptsToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
		this.hotScriptsToolStripMenuItem.Text = "Hot-Scripts";
		this.toolStripMenuItem2.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.toolStripMenuItem2.ForeColor = System.Drawing.Color.White;
		this.toolStripMenuItem2.Name = "toolStripMenuItem2";
		this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem2.Text = "DarkDex";
		this.toolStripMenuItem2.Click += new System.EventHandler(toolStripMenuItem2_Click);
		this.openGuiToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.openGuiToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.openGuiToolStripMenuItem.Name = "openGuiToolStripMenuItem";
		this.openGuiToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
		this.openGuiToolStripMenuItem.Text = "OpenGui";
		this.openGuiToolStripMenuItem.Click += new System.EventHandler(openGuiToolStripMenuItem_Click_1);
		this.toolStripMenuItem4.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.toolStripMenuItem4.ForeColor = System.Drawing.Color.White;
		this.toolStripMenuItem4.Name = "toolStripMenuItem4";
		this.toolStripMenuItem4.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem4.Text = "Owl Hub";
		this.toolStripMenuItem4.Click += new System.EventHandler(toolStripMenuItem4_Click);
		this.toolStripMenuItem1.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.toolStripMenuItem1.ForeColor = System.Drawing.Color.White;
		this.toolStripMenuItem1.Name = "toolStripMenuItem1";
		this.toolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem1.Text = "Galaxy Hub";
		this.toolStripMenuItem1.Click += new System.EventHandler(toolStripMenuItem1_Click_2);
		this.remoteSpyToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.remoteSpyToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.remoteSpyToolStripMenuItem.Name = "remoteSpyToolStripMenuItem";
		this.remoteSpyToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
		this.remoteSpyToolStripMenuItem.Text = "Remote Spy";
		this.remoteSpyToolStripMenuItem.Click += new System.EventHandler(remoteSpyToolStripMenuItem_Click);
		this.toolStripMenuItem3.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.toolStripMenuItem3.ForeColor = System.Drawing.Color.White;
		this.toolStripMenuItem3.Name = "toolStripMenuItem3";
		this.toolStripMenuItem3.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem3.Text = "Game Sense";
		this.toolStripMenuItem3.Click += new System.EventHandler(gameSenseToolStripMenuItem_Click);
		this.unnamedESPToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.unnamedESPToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.unnamedESPToolStripMenuItem.Name = "unnamedESPToolStripMenuItem";
		this.unnamedESPToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
		this.unnamedESPToolStripMenuItem.Text = "Unnamed ESP";
		this.unnamedESPToolStripMenuItem.Click += new System.EventHandler(unnamedESPToolStripMenuItem_Click);
		this.toolStripMenuItem8.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.toolStripMenuItem8.ForeColor = System.Drawing.Color.White;
		this.toolStripMenuItem8.Name = "toolStripMenuItem8";
		this.toolStripMenuItem8.Size = new System.Drawing.Size(148, 22);
		this.toolStripMenuItem8.Text = "Infinite Yield";
		this.toolStripMenuItem8.Click += new System.EventHandler(toolStripMenuItem8_Click);
		this.cMDXToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.cMDXToolStripMenuItem.ForeColor = System.Drawing.Color.White;
		this.cMDXToolStripMenuItem.Name = "cMDXToolStripMenuItem";
		this.cMDXToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
		this.cMDXToolStripMenuItem.Text = "CMD-X";
		this.cMDXToolStripMenuItem.Click += new System.EventHandler(cMDXToolStripMenuItem_Click);
		this.toolStripMenuItem5.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripMenuItem6, this.toolStripMenuItem7 });
		this.toolStripMenuItem5.ForeColor = System.Drawing.Color.White;
		this.toolStripMenuItem5.Name = "toolStripMenuItem5";
		this.toolStripMenuItem5.Size = new System.Drawing.Size(54, 20);
		this.toolStripMenuItem5.Text = "Others";
		this.toolStripMenuItem6.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.toolStripMenuItem6.ForeColor = System.Drawing.Color.White;
		this.toolStripMenuItem6.Name = "toolStripMenuItem6";
		this.toolStripMenuItem6.Size = new System.Drawing.Size(173, 22);
		this.toolStripMenuItem6.Text = "Get Key";
		this.toolStripMenuItem6.Click += new System.EventHandler(bunifuFlatButton8_Click);
		this.toolStripMenuItem7.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.toolStripMenuItem7.ForeColor = System.Drawing.Color.White;
		this.toolStripMenuItem7.Name = "toolStripMenuItem7";
		this.toolStripMenuItem7.Size = new System.Drawing.Size(173, 22);
		this.toolStripMenuItem7.Text = "Join Discord Server";
		this.toolStripMenuItem7.Click += new System.EventHandler(toolStripMenuItem7_Click);
		this.timer1.Enabled = false;
		this.timer1.Interval = 300;
		this.timer1.Tick += new System.EventHandler(timer1_Tick);
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel2.BackColor = System.Drawing.Color.FromArgb(33, 33, 33);
		this.panel2.Location = new System.Drawing.Point(288, 32);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(402, 25);
		this.panel2.TabIndex = 0;
		this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(krnl_MouseMove);
		this.errorProvider1.ContainerControl = this;
		this.customTabControl1.ActiveColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.customTabControl1.AllowDrop = true;
		this.customTabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.customTabControl1.BackTabColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.customTabControl1.BorderColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.customTabControl1.ClosingButtonColor = System.Drawing.Color.WhiteSmoke;
		this.customTabControl1.ClosingMessage = null;
		this.customTabControl1.Controls.Add(this.tabPage1);
		this.customTabControl1.HeaderColor = System.Drawing.Color.FromArgb(45, 45, 48);
		this.customTabControl1.HorizontalLineColor = System.Drawing.Color.FromArgb(30, 30, 30);
		this.customTabControl1.ItemSize = new System.Drawing.Size(240, 16);
		this.customTabControl1.Location = new System.Drawing.Point(4, 59);
		this.customTabControl1.Name = "customTabControl1";
		this.customTabControl1.SelectedIndex = 0;
		this.customTabControl1.SelectedTextColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.customTabControl1.ShowClosingButton = false;
		this.customTabControl1.ShowClosingMessage = false;
		this.customTabControl1.Size = new System.Drawing.Size(556, 259);
		this.customTabControl1.TabIndex = 3;
		this.customTabControl1.TextColor = System.Drawing.Color.FromArgb(255, 255, 255);
		this.customTabControl1.SelectedIndexChanged += new System.EventHandler(customTabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.FromArgb(36, 36, 36);
		this.tabPage1.Location = new System.Drawing.Point(4, 20);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Size = new System.Drawing.Size(548, 235);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Click += new System.EventHandler(tabPage1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(18, 18, 18);
		base.ClientSize = new System.Drawing.Size(690, 350);
		base.Controls.Add(this.menuStrip1);
		base.Controls.Add(this.ScriptView);
		base.Controls.Add(this.customTabControl1);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.bunifuFlatButton5);
		base.Controls.Add(this.bunifuFlatButton4);
		base.Controls.Add(this.bunifuFlatButton3);
		base.Controls.Add(this.bunifuFlatButton2);
		base.Controls.Add(this.bunifuFlatButton1);
		base.Controls.Add(this.bunifuFlatButton6);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "krnl_monaco";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "krnl";
		base.TopMost = true;
		base.Activated += new System.EventHandler(krnl_Activated);
		base.Deactivate += new System.EventHandler(krnl_Deactivate);
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(krnl_FormClosing);
		base.Load += new System.EventHandler(Form1_Load);
		base.MouseDown += new System.Windows.Forms.MouseEventHandler(krnl_MouseDown);
		base.MouseMove += new System.Windows.Forms.MouseEventHandler(krnl_MouseMove);
		base.MouseUp += new System.Windows.Forms.MouseEventHandler(krnl_MouseUp);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		this.TabContextMenu.ResumeLayout(false);
		this.contextMenuStrip1.ResumeLayout(false);
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.errorProvider1).EndInit();
		this.customTabControl1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}

	private void unnamedESPToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://ic3w0lf.xyz/rblx/protoesp.lua', true))()");
	}

	private void timer1_Tick(object sender, EventArgs e)
	{
		if (Settings.Default.remove_crash_logs)
		{
			try
			{
				if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Roblox\\logs\\archive"))
				{
					Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Roblox\\logs\\archive", recursive: true);
				}
				else if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\Roblox\\logs\\archive"))
				{
					Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\Roblox\\logs\\archive", recursive: true);
				}
			}
			catch
			{
			}
		}
		if ((Settings.Default.autoinject ? true : false) && !Program.attaching && Process.GetProcessesByName("RobloxPlayerBeta").Length == 1 && File.Exists(Program.dll_path) && File.Exists(Program.injector_path))
		{
			Task.Run(delegate
			{
				Program.attaching = true;
				Injector.SeliwareInjection();
				Task.Delay(3000).GetAwaiter().GetResult();
				Program.attaching = false;
			});
		}
	}

	private void toolStripMenuItem1_Click_2(object sender, EventArgs e)
	{
		Seliware.Execute("loadstring(game:HttpGet('https://raw.githubusercontent.com/LaziestBoy/Krnl-Hub/master/Krnl-Hub.lua', true))()");
	}

	private void toolStripMenuItem4_Click(object sender, EventArgs e)
	{
        Seliware.Execute("loadstring(game:HttpGet('https://raw.githubusercontent.com/CriShoux/OwlHub/master/OwlHub.txt'))();");
	}

	private void pictureBox2_MouseEnter(object sender, EventArgs e)
	{
		toolTip1.SetToolTip((PictureBox)sender, "Click to join the server");
	}

	private void pictureBox2_MouseLeave(object sender, EventArgs e)
	{
		toolTip1.Hide((PictureBox)sender);
	}

	private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
	{
		Process.Start("https://discord.gg/8v6cETPera");
	}

	private void bunifuFlatButton8_Click(object sender, EventArgs e)
	{
		MessageBox.Show("Seliware is paid", "KRNL", 0);
		Task.Delay(1000);
		Process.Start("https://discord.gg/7qGec3z9jS");
	}

	private void toolStripMenuItem7_Click(object sender, EventArgs e)
	{
		Process.Start("https://" + Program.domain + "/invite");
	}

	private void krnl_MouseMove(object sender, MouseEventArgs e)
	{
		if (mMouseDown)
		{
			Refresh();
			SuspendLayout();
			if (isonEdge)
			{
				if (Cursor == Cursors.PanSouth)
				{
					if (e.Y > 350)
					{
						heightUnchanged = false;
						SetBounds(base.Left, base.Top, base.Width, base.Height - (base.Height - e.Y));
					}
					else
					{
						heightUnchanged = true;
						SetBounds(base.Left, base.Top, base.Width, 350);
					}
				}
				if (Cursor == Cursors.PanEast)
				{
					if (e.X > 690)
					{
						widthUnchanged = false;
						SetBounds(base.Left, base.Top, base.Width - (base.Width - e.X), base.Height);
					}
					else
					{
						widthUnchanged = true;
						SetBounds(base.Left, base.Top, 690, base.Height);
					}
				}
				else if (Cursor == Cursors.PanSE)
				{
					SetBounds(base.Left, base.Top, (base.Width - (base.Width - e.X) < 690) ? 690 : (base.Width - (base.Width - e.X)), (base.Height - (base.Height - e.Y) < 350) ? 350 : (base.Height - (base.Height - e.Y)));
				}
				panel3.Width = base.Width;
			}
			ResumeLayout();
		}
		else if (e.Y > base.Height - 10 && e.X < base.Width - 5)
		{
			Cursor = Cursors.PanSouth;
			isonEdge = true;
		}
		else if (e.X > base.Width - (mWidth + 2) && e.Y > base.Height - (mWidth + 2))
		{
			Cursor = Cursors.PanSE;
			isonEdge = true;
		}
		else if (e.X > base.Width - 5 && e.Y > button1.Size.Height)
		{
			Cursor = Cursors.PanEast;
			isonEdge = true;
		}
		else
		{
			Cursor = Cursors.Default;
			isonEdge = false;
		}
	}

	private void krnl_MouseDown(object sender, MouseEventArgs e)
	{
		if (e.Button == MouseButtons.Left)
		{
			mMouseDown = true;
		}
	}

	private void krnl_MouseUp(object sender, MouseEventArgs e)
	{
		mMouseDown = false;
	}

	private void bunifuFlatButton7_Click(object sender, EventArgs e)
	{
	}

	public async void anim_CompletedTask()
	{
		for (int j = 0; j < 70; j += 5)
		{
			await Task.Delay(1);
			panel3.BackColor = Color.FromArgb(30, 144 - j, 255 - j);
		}
		for (int j = 0; j < 69; j += 5)
		{
			await Task.Delay(1);
			panel3.BackColor = Color.FromArgb(30, 74 + j, 185 + j);
		}
	}

	public async void anim_AwaitingTaskFinish()
	{
		while (Anim_ATF_break)
		{
			for (int j = 0; j < 70; j++)
			{
				if (!Anim_ATF_break)
				{
					panel3.BackColor = Color.FromArgb(30, 144, 255);
					break;
				}
				await Task.Delay(3);
				panel3.BackColor = Color.FromArgb(30, 144 - j, 255 - j);
			}
			for (int j = 0; j < 69; j++)
			{
				if (!Anim_ATF_break)
				{
					panel3.BackColor = Color.FromArgb(30, 144, 255);
					break;
				}
				await Task.Delay(3);
				panel3.BackColor = Color.FromArgb(30, 74 + j, 185 + j);
			}
		}
	}

	private void bunifuFlatButton8_Click_1(object sender, EventArgs e)
	{
		Anim_ATF_break = true;
		anim_AwaitingTaskFinish();
		Anim_ATF_break = false;
	}

	private void bunifuFlatButton10_Click(object sender, EventArgs e)
	{
	}

	private void toolStripMenuItem8_Click(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://raw.githubusercontent.com/EdgeIY/infiniteyield/master/source'))()");
	}

	private void toolStripMenuItem10_Click(object sender, EventArgs e)
	{
		TabPage contextTab = customTabControl1.contextTab;
		if (contextTab != null)
		{
			Form prompt = new Form
			{
				Width = 200,
				Height = 50,
				MinimumSize = new Size(200, 50),
				MaximumSize = new Size(200, 50),
				FormBorderStyle = FormBorderStyle.None,
				Text = "What do you want to rename this tab to?",
				StartPosition = FormStartPosition.CenterParent
			};
			Label value = new Label
			{
				Width = 200,
				Height = 50,
				Text = "What do you want to rename this tab to?",
				Top = 0,
				Left = 0
			};
			TextBox textBox = new TextBox
			{
				Left = 0,
				Top = 30,
				Width = 150,
				Text = contextTab.Text
			};
			Button button = new Button
			{
				Text = "Ok",
				Left = 150,
				Width = 50,
				Top = 30,
				DialogResult = DialogResult.OK
			};
			prompt.TopMost = true;
			button.Click += delegate
			{
				prompt.Close();
			};
			prompt.Controls.Add(textBox);
			prompt.Controls.Add(button);
			prompt.Controls.Add(value);
			prompt.AcceptButton = button;
			string text = ((prompt.ShowDialog() == DialogResult.OK) ? textBox.Text : "");
			if (text.Length > 0)
			{
				contextTab.Text = text;
			}
		}
	}

	private void cMDXToolStripMenuItem_Click(object sender, EventArgs e)
	{
		Pipe("loadstring(game:HttpGet('https://raw.githubusercontent.com/CMD-X/CMD-X/master/Source', true))()");
	}
}

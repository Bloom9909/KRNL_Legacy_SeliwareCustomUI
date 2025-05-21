using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp.WinForms;
using IWshRuntimeLibrary;
using krnlss.Properties;
using Microsoft.Win32;
using SeliwareAPI;

namespace krnlss;

internal static class Program
{
	public static bool is_wrd = false;

	public static string domain = "";

	public static string dll_path = "";

	public static string injector_path = "";

	public static bool attached = false;

	public static bool attaching = false;

	public static Form form;

	public static List<string> tabScripts = new List<string>();

	public static bool injecting = false;

	public static bool failed_inject = false;

	public static int __i;

	public static bool debugme;

	public static int idx;

	public static bool is_attached = false;

	public static bool is_closed = false;

	private static int injectedPID { get; set; }

	[STAThread]
	public static void writerblx()
	{
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool WaitNamedPipe(string name, int timeout);

	[DllImport("user32.dll", SetLastError = true)]
	internal static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

	public static string eb64(string content)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
	}

	public static string db64(string content)
	{
		try
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(content));
		}
		catch
		{
			return content;
		}
	}

	public static void pc(bool start = false, bool urlPassed = false, int key = -1)
	{
		if (debugme)
		{
			if (start)
			{
				File.WriteAllText("pass check.txt", "");
			}
			string text = Convert.ToString(__i++);
			File.AppendAllText("pass check.txt", text + key switch
			{
				1 => " Key", 
				0 => " No Key", 
				_ => "", 
			} + (urlPassed ? " Url Passed" : "") + "\n");
		}
	}

	public static bool isCompatible()
	{
		string text = (string)Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion").GetValue("ProductName");
		if (text.IndexOf("Windows 8.1") == -1 && (text.IndexOf("Windows 8") == -1 || text.IndexOf("1") == -1))
		{
			return text.IndexOf("Windows 10") != -1;
		}
		return true;
	}

	public static bool hasFolder(string name, string path)
	{
		DirectoryInfo[] directories = new DirectoryInfo(path).GetDirectories();
		for (int i = 0; i < directories.Length; i++)
		{
			if (directories[i].Name == name)
			{
				return true;
			}
		}
		return false;
	}

	public static bool hasFile(string name, string path)
	{
		FileInfo[] files = new DirectoryInfo(path).GetFiles();
		for (int i = 0; i < files.Length; i++)
		{
			if (files[i].Name == name)
			{
				return true;
			}
		}
		return false;
	}

	private static void LoadReferencedAssembly(Assembly assembly)
	{
		AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
		foreach (AssemblyName name in referencedAssemblies)
		{
			if (!AppDomain.CurrentDomain.GetAssemblies().Any((Assembly a) => a.FullName == name.FullName))
			{
				LoadReferencedAssembly(Assembly.Load(name));
			}
		}
	}

	public static void test()
	{
	}

	public static void create_shortcut(string name, string target)
	{
		string pathLink = Path.Combine(Directory.GetCurrentDirectory(), name + ".lnk");
		WshShell wshShell = (WshShell)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")));
		IWshShortcut obj = (IWshShortcut)(dynamic)wshShell.CreateShortcut(pathLink);
		obj.TargetPath = target;
		obj.Save();
	}

	[STAThread]// ўа покажу прогресс))
	private static void Main()
	{
        Process.GetCurrentProcess().Disposed += Program_Disposed;
		Process.GetCurrentProcess().Exited += Program_Exited;
		AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
		//Console.Title = "Krnl UWP Console";
		//ConsoleFramework.Init();
		string text = "KRNL Workspace";
		string[] directories = Directory.GetDirectories(Environment.GetEnvironmentVariable("LocalAppData") + "\\Packages");
		foreach (string text2 in directories)
		{
		}
		if (text == "")
		{
			MessageBox.Show(new Form
			{
				TopMost = true
			}, "Please install ROBLOX from Microsoft Store", "Krnl", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Environment.Exit(0);
			return;
		}
		while (true)
		{
			{

				{
					if (!File.Exists("krnlss.exe.config"))
					{
						File.WriteAllText("krnlss.exe.config", "<?xml version=\"1.0\" encoding=\"utf-8\" ?><configuration><runtime><assemblyBinding xmlns=\"urn:schemas-microsoft-com:asm.v1\"><probing privatePath=\"bin;bin/src\" /></assemblyBinding></runtime></configuration>");
						Task.Delay(500).GetAwaiter().GetResult();
						Process.Start("krnlss.exe");
						Environment.Exit(0);
					}
					test();
					try
					{
						string[] array = new string[4] { "CefSharp.dll", "CefSharp.Core.dll", "CefSharp.WinForms.dll", "CefSharp.OffScreen.dll" };
						for (int num2 = 0; num2 < array.Length; num2++)
						{
							try
							{
								LoadReferencedAssembly(Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin/src", array[num2])));
							}
							catch
							{
							}
						}
					}
					catch (Exception)
					{
					}
					try
					{
						string[] array2 = new string[2] { "ScintillaNET.dll", "Bunifu_UI_v1.5.3.dll" };
						for (int num3 = 0; num3 < array2.Length; num3++)
						{
							try
							{
								LoadReferencedAssembly(Assembly.LoadFrom(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", array2[num3])));
							}
							catch
							{
							}
						}
					}
					catch (Exception)
					{
					}
					test();
					ServicePointManager.Expect100Continue = true;
					test();
					Process[] processes = Process.GetProcesses();
					for (int num4 = 0; num4 < processes.Length; num4++)
					{
						if (processes[num4] != Process.GetCurrentProcess() && new string[2] { "krnl", "krnlss" }.ToList().IndexOf(processes[num4].ProcessName.Split('.')[0].ToLower()) != -1)
						{
							try
							{
								processes[num4].CloseMainWindow();
							}
							catch
							{
							}
						}
					}
					test();
					try
					{
						if (Directory.Exists("workspace"))
						{
							Directory.Move("workspace", "old_workspace");
						}
						if (Directory.Exists("autoexec"))
						{
							Directory.Move("autoexec", "old_autoexec");
						}
					}
					catch
					{
					}
					string text3 = Path.Combine(text, "workspace");
					string text4 = Path.Combine(text, "autoexec");
					string text5 = Path.Combine(text, "ipc");
					if (!Directory.Exists(text3))
					{
						Directory.CreateDirectory(text3);
					}
					if (!Directory.Exists(text4))
					{
						Directory.CreateDirectory(text4);
					}
					if (!Directory.Exists(text5))
					{
						Directory.CreateDirectory(text5);
					}
					if (!File.Exists("autoexec.lnk"))
					{
						create_shortcut("autoexec", text4);
					}
					if (!File.Exists("workspace.lnk"))
					{
						create_shortcut("workspace.lnk", text3);
					}
					test();
					writerblx();
					test();
					Stack<string> stack = new Stack<string>(Environment.CurrentDirectory.Split('\\'));
					bool flag = false;
					stack.Reverse();
					while (stack.Count != 0)
					{
						if (string.Join("\\", stack.ToArray().Reverse()) + "\\" == Path.GetTempPath())
						{
							flag = true;
							break;
						}
						stack.Pop();
					}
					if (!flag)
					{
						if (Directory.GetCurrentDirectory().Split('\\').ToList()
							.Last()
							.StartsWith("Rar$EX"))
						{
							flag = true;
						}
						if (Directory.GetCurrentDirectory().ToLower().IndexOf("c:\\windows\\system32") != -1)
						{
							flag = false;
							MessageBox.Show(new Form
							{
								TopMost = true
							}, "You cannot run this here!\nYou must extract the zip file!", "Zip file detected.");
						}
					}
					if (flag)
					{
						string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
						if (!Directory.Exists(folderPath + "\\krnl"))
						{
							Directory.CreateDirectory(folderPath + "\\krnl");
						}
						string text6 = folderPath + "\\krnl";
						new DirectoryInfo(text6);
						DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory);
						directoryInfo.GetDirectories();
						FileInfo[] files = directoryInfo.GetFiles();
						MessageBox.Show(new Form
						{
							TopMost = true
						}, "You cannot run this here!\nExtracting the zip file to your Desktop.", "Zip file detected.");
						MessageBox.Show(Process.GetCurrentProcess().MainModule.FileName);
						for (int num5 = 0; num5 < files.Length; num5++)
						{
							if (!hasFile(files[num5].Name, text6))
							{
								files[num5].CopyTo(files[num5].FullName.Replace(Environment.CurrentDirectory, text6), overwrite: true);
							}
						}
						Process.Start(text6);
						Process.Start(new ProcessStartInfo
						{
							WorkingDirectory = text6,
							FileName = "krnlss.exe",
							CreateNoWindow = true
						});
						Environment.Exit(1);
					}
					else
					{
						test();
						bool flag2 = !is_wrd && Settings.Default.monaco;
						Settings.Default.monaco = flag2;
						form = (flag2 ? ((Form)new krnl()) : ((Form)new krnl()));
						create_wrd_button();
						//auto_attach();
						ConsoleFramework.TailFrom(Path.Combine(text5, "k_ipc.txt"));
						test();
						form.Width = 690;
						form.Opacity = 0.0;
						Application.Run(form);
						form.Load += Form_Activated;
						form.Disposed += Form_Disposed;
						form.FormClosing += Form_FormClosing;
						test();
					}
					return;
				}
			}
		}
    }

	private static void Program_Disposed(object sender, EventArgs e)
	{
	}

	private static void Program_Exited(object sender, EventArgs e)
	{
	}

	private static void Form_FormClosing(object sender, FormClosingEventArgs e)
	{
	}

	private static void Form_Disposed(object sender, EventArgs e)
	{
	}

	private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
	{
	}

	public static void writeToDir(string directory)
	{
	}

	private static void Form_Activated(object sender, EventArgs e)
	{
		while (form.Opacity < 1.0)
		{
			form.Opacity += Math.Min(1.0, 0.05);
			Task.Delay(1).GetAwaiter().GetResult();
		}
	}

	public static async void auto_attach()
	{
		while (true)
		{
			{
				if (Process.GetProcessesByName("RobloxPlayerBeta").Length != 0)
				{
					Injector.SeliwareInjection();
				}
				else
				{
					is_closed = true;
					is_attached = false;
				}
			}
			await Task.Delay(3000);
		}
	}

	public static void execute_script(string script)
	{
		if (Injector.inject_status() == false)
		{
			MessageBox.Show(new Form
			{
				TopMost = true
			}, "Please inject before executing!");
		}
		else
		{
			Injector.run_script();
		}
	}

	public static void create_wrd_button()
	{
		Button btn = new Button();
		btn.Anchor = AnchorStyles.Right;
		btn.Width = 100;
		btn.Height = 20;
		btn.FlatStyle = FlatStyle.Flat;
		btn.FlatAppearance.BorderColor = Color.FromArgb(22, 22, 22);
		btn.FlatAppearance.BorderSize = 0;
		btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
		btn.Location = new Point(480, 8);
		btn.Margin = new Padding(0, 3, 3, 3);
		btn.BackColor = Color.FromArgb(29, 29, 29);
		btn.ForeColor = Color.FromArgb(200, 200, 200);
		btn.Font = new Font("Segoe UI", 9f, FontStyle.Regular, GraphicsUnit.Point, 0);
		btn.Text = "seliware.com";
		btn.TextAlign = ContentAlignment.MiddleCenter;
		btn.TabStop = false;
		btn.Click += delegate
		{
			Process.Start("https://seliware.com/");
			((dynamic)form).Focus();
		};
		Task.Run(async delegate
		{
			while (form == null)
			{
				await Task.Delay(1);
			}
			form.Load += delegate
			{
				((dynamic)form).panel1.Controls.Add(btn);
			};
		});
	}

	public static void Inject()
	{
		bool status;
		status = Injector.inject_status();
		if (status == false)
		{
			Injector.SeliwareInjection();
		}
		if(status == true)
		{
			MessageBox.Show("Seliware already injected", "KRNL", 0);
		}

	}
}

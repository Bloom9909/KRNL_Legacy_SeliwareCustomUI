using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using krnlss;

namespace Controls;

public class MonacoCustomTabControl : TabControl
{
    public static krnl_monaco Form1;

    private ChromiumWebBrowser browser;

    public TabPage contextTab;

    public Color selectedTextColor = Color.FromArgb(255, 255, 255);

    private readonly StringFormat CenterSringFormat = new StringFormat
    {
        Alignment = StringAlignment.Near,
        LineAlignment = StringAlignment.Center
    };

    private Color activeColor = Color.FromArgb(36, 36, 36);

    private Color backTabColor = Color.FromArgb(0, 0, 0);

    private Color borderColor = Color.FromArgb(30, 30, 30);

    private Color closingButtonColor = Color.WhiteSmoke;

    private string closingMessage;

    private int count = 1;

    public int realIndex = -1;

    private Color headerColor = Color.FromArgb(45, 45, 48);

    private Color horizLineColor = Color.FromArgb(36, 36, 36);

    private TabPage predraggedTab;

    private Color textColor = Color.FromArgb(255, 255, 255);

    public static bool removed = false;

    public static int removeIdx = -1;

    public static int current_index = -1;

    public static int last_index = -1;

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the selected page")]
    public Color ActiveColor
    {
        get
        {
            return activeColor;
        }
        set
        {
            activeColor = value;
        }
    }

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the background of the tab")]
    public Color BackTabColor
    {
        get
        {
            return backTabColor;
        }
        set
        {
            backTabColor = value;
        }
    }

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the border of the control")]
    public Color BorderColor
    {
        get
        {
            return borderColor;
        }
        set
        {
            borderColor = value;
        }
    }

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the closing button")]
    public Color ClosingButtonColor
    {
        get
        {
            return closingButtonColor;
        }
        set
        {
            closingButtonColor = value;
        }
    }

    [Category("Options")]
    [Browsable(true)]
    [Description("The message that will be shown before closing.")]
    public string ClosingMessage
    {
        get
        {
            return closingMessage;
        }
        set
        {
            closingMessage = value;
        }
    }

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the header.")]
    public Color HeaderColor
    {
        get
        {
            return headerColor;
        }
        set
        {
            headerColor = value;
        }
    }

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the horizontal line which is located under the headers of the pages.")]
    public Color HorizontalLineColor
    {
        get
        {
            return horizLineColor;
        }
        set
        {
            horizLineColor = value;
        }
    }

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the title of the page")]
    public Color SelectedTextColor
    {
        get
        {
            return selectedTextColor;
        }
        set
        {
            selectedTextColor = value;
        }
    }

    public bool ShowClosingButton { get; set; }

    [Category("Options")]
    [Browsable(true)]
    [Description("Show a Yes/No message before closing?")]
    public bool ShowClosingMessage { get; set; }

    [Category("Colors")]
    [Browsable(true)]
    [Description("The color of the title of the page")]
    public Color TextColor
    {
        get
        {
            return textColor;
        }
        set
        {
            textColor = value;
        }
    }

    public MonacoCustomTabControl()
    {
        InitializeChromium();
        SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, value: true);
        DoubleBuffered = true;
        base.SizeMode = TabSizeMode.Normal;
        base.ItemSize = new Size(240, 16);
        AllowDrop = true;
        base.Selecting += TabChanging;
    }

    public void AddEvent(string name = "Script.lua", string content = "")
    {
        addnewtab();
    }

    public void AddIntellisense(ChromiumWebBrowser chrome, string p1, string p2, string p3, string p4)
    {
        Invoke((MethodInvoker)delegate
        {
            try
            {
                CefSharp.WebBrowserExtensions.ExecuteScriptAsyncWhenPageLoaded((IWebBrowser)(object)chrome, "AddIntellisense('" + p1 + "', '" + p2 + "', '" + p3 + "', '" + p4 + "')", true);
            }
            catch
            {
            }
        });
    }

    public void addnewtab()
    {
        int index = base.TabCount - 1;
        base.TabPages.Insert(index, $"Script{base.TabCount}.lua");
        SelectTab(base.TabPages[index]);
        ((Control)(object)browser).Parent = base.TabPages[index];
        base.SelectedIndex = index;
    }

    public void CloseTab(TabPage tab)
    {
        _ = tab.Controls[0];
        int num = base.TabPages.IndexOf(tab);
        if (num != 0 || base.TabCount > 2)
        {
            base.TabPages.RemoveAt(num);
            count--;
        }
    }

    public ChromiumWebBrowser GetWorkingTextEditor()
    {
        _ = base.SelectedTab;
        return browser;
    }

    public void InitializeChromium()
    {
        bool? nullableBool = (!Cef.IsInitialized);
        {
            Cef.Initialize((CefSettingsBase)new CefSettings
            {
                BrowserSubprocessPath = Path.Combine(Environment.CurrentDirectory, "bin", "src", "CefSharp.BrowserSubprocess.exe"),
                LocalesDirPath = Path.Combine(Environment.CurrentDirectory, "bin", "src", "locales"),
                ResourcesDirPath = Path.Combine(Environment.CurrentDirectory, "bin", "src"),
                LogSeverity = (LogSeverity)99
            });
        }
        browser = new ChromiumWebBrowser(Environment.CurrentDirectory.Replace("\\", "/") + "/bin/Monaco/Monaco.html", (IRequestContext)null);
        ((Control)(object)browser).Dock = DockStyle.Fill;
        ((Control)(object)browser).BringToFront();
        browser.LoadingStateChanged += delegate (object o, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading && browser.CanExecuteJavascriptInMainFrame)
            {
                Invoke((MethodInvoker)delegate
                {
                    ((Control)(object)browser).Visible = true;
                    Intellisense.addIntellisense(browser);
                });
            }
        };
        browser.BrowserSettings.WindowlessFrameRate = 120;
    }

    public void addScript(string content)
    {
        Program.tabScripts.Add(content);
    }

    public bool OpenFileDialog(TabPage tab)
    {
        using OpenFileDialog openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "Lua Files (*.lua)|*.lua|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        openFileDialog.RestoreDirectory = true;
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            CefSharp.WebBrowserExtensions.ExecuteScriptAsync((IWebBrowser)(object)GetWorkingTextEditor(), "SetText", new object[1] { File.ReadAllText(openFileDialog.FileName) });
            tab.Text = Path.GetFileName(openFileDialog.FileName);
            return true;
        }
        return false;
    }

    public string OpenSaveDialog(TabPage tab, string text)
    {
        using SaveFileDialog saveFileDialog = new SaveFileDialog();
        saveFileDialog.Filter = "Lua Files (*.lua)|*.lua|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
        saveFileDialog.RestoreDirectory = true;
        saveFileDialog.FileName = tab.Text;
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(saveFileDialog.FileName, CefSharp.WebBrowserExtensions.EvaluateScriptAsync((IWebBrowser)(object)GetWorkingTextEditor(), "GetText", new object[0]).GetAwaiter().GetResult()
                .Result.ToString());
            return new FileInfo(saveFileDialog.FileName).Name;
        }
        return tab.Text;
    }

    public void TabChanging(object sender, TabControlCancelEventArgs e)
    {
        if (browser != null && !browser.IsLoading && browser.IsBrowserInitialized && browser.CanExecuteJavascriptInMainFrame && realIndex >= 0)
        {
            if (removed)
            {
                Program.tabScripts.RemoveAt(removeIdx);
            }
            if (removeIdx != e.TabPageIndex)
            {
                string text = CefSharp.WebBrowserExtensions.EvaluateScriptAsync((IWebBrowser)(object)browser, "GetText()", (TimeSpan?)null).GetAwaiter().GetResult()
                    .Result.ToString();
                Program.tabScripts[realIndex] = ((text != "-- Krnl Monaco") ? text : Program.tabScripts[realIndex]);
            }
            removed = false;
            removeIdx = -1;
        }
        if (Program.tabScripts.Count < e.TabPageIndex + 1)
        {
            Program.tabScripts.Add("-- Krnl Monaco");
        }
        if (e.TabPageIndex == base.TabCount - 1)
        {
            e.Cancel = true;
        }
        else if (e.Action == TabControlAction.Selecting)
        {
            realIndex = e.TabPageIndex;
            if (!browser.IsLoading && browser.IsBrowserInitialized)
            {
                CefSharp.WebBrowserExtensions.ExecuteScriptAsync((IWebBrowser)(object)browser, "SetText", new object[1] { Program.tabScripts[realIndex] });
                ((Control)(object)browser).Parent = GetPointedTab();
            }
            else
            {
                CefSharp.WebBrowserExtensions.ExecuteScriptAsyncWhenPageLoaded((IWebBrowser)(object)browser, "SetText(`" + Program.tabScripts[current_index].Replace("`", "\\`").Replace("\\", "\\\\") + "`)", true);
            }
        }
    }

    protected override void CreateHandle()
    {
        base.CreateHandle();
        base.Alignment = TabAlignment.Top;
        SendMessage(base.Handle, 4913, IntPtr.Zero, (IntPtr)16);
    }

    public static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T value = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = value;
    }

    protected override void OnDragOver(DragEventArgs drgevent)
    {
        base.OnDragOver(drgevent);
    }

    protected override void OnMouseDown(MouseEventArgs e)
    {
        predraggedTab = GetPointedTab();
        Point location = e.Location;
        for (int i = 0; i < base.TabCount; i++)
        {
            dynamic val = GetTabRect(i);
            val.Offset(val.Width - 15, 2);
            val.Width = 10;
            val.Height = 10;
            dynamic val2 = !val.Contains(location);
            if (val2 || e.Button != MouseButtons.Left)
            {
                continue;
            }
            if (i != base.TabCount - 1)
            {
                predraggedTab = null;
                TabPage tabPage = base.TabPages[i];
                if (!ShowClosingMessage)
                {
                    if (base.TabCount == 2)
                    {
                        if (MessageBox.Show(new Form
                        {
                            TopMost = true
                        }, "Are you sure you want to clear this tab?\nThe reason why you see this prompt is because there is only one tab currently opened.", "SINGLE TAB DETECTED", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            Program.tabScripts[0] = "";
                            tabPage.Text = "Untitled.lua";
                            ChromiumWebBrowser workingTextEditor = GetWorkingTextEditor();
                            object[] array = new string[1] { "" };
                            object[] array2 = array;
                            CefSharp.WebBrowserExtensions.ExecuteScriptAsync((IWebBrowser)(object)workingTextEditor, "SetText", array2);
                        }
                        return;
                    }
                    if (tabPage.Controls.Count > 0)
                    {
                        removeIdx = i;
                        removed = true;
                        SelectTab(base.TabPages[i - 1]);
                        ((Control)(object)browser).Parent = base.TabPages[i - 1];
                        base.TabPages[i].Dispose();
                    }
                    break;
                }
                MessageBox.Show(new Form
                {
                    TopMost = true
                }, "Changing tab?");
            }
            else if (GetTabRect(base.TabCount - 1).Contains(e.Location))
            {
                AddEvent();
                predraggedTab = null;
                break;
            }
        }
        base.OnMouseDown(e);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
    }

    protected override void OnMouseUp(MouseEventArgs e)
    {
        predraggedTab = null;
        contextTab = null;
        if (e.Button == MouseButtons.Right)
        {
            Form1.TabContextMenu.Show(Cursor.Position);
            contextTab = GetPointedTab();
        }
        base.OnMouseUp(e);
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        Graphics graphics = e.Graphics;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
        graphics.Clear(headerColor);
        try
        {
            base.SelectedTab.BackColor = backTabColor;
        }
        catch
        {
        }
        try
        {
            base.SelectedTab.BorderStyle = BorderStyle.None;
        }
        catch
        {
        }
        for (int i = 0; i <= base.TabCount - 1; i++)
        {
            TabPage tabPage = base.TabPages[i];
            int num = (tabPage.Width = (int)e.Graphics.MeasureString(tabPage.Text, Font).Width + 16);
            Rectangle rectangle = new Rectangle(new Point(GetTabRect(i).Location.X + 2, GetTabRect(i).Location.Y), new Size(GetTabRect(i).Width, GetTabRect(i).Height));
            Rectangle rectangle2 = new Rectangle(rectangle.Location, new Size(rectangle.Width, rectangle.Height));
            Brush brush = new SolidBrush(closingButtonColor);
            if (i != base.SelectedIndex)
            {
                graphics.DrawString(tabPage.Text, Font, new SolidBrush(textColor), rectangle2, CenterSringFormat);
            }
            else
            {
                graphics.FillRectangle(new SolidBrush(headerColor), rectangle2);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(36, 36, 36)), new Rectangle(rectangle.X - 5, rectangle.Y - 3, rectangle.Width, rectangle.Height + 5));
                graphics.DrawString(tabPage.Text, Font, new SolidBrush(selectedTextColor), rectangle2, CenterSringFormat);
            }
            if (i != base.TabCount - 1)
            {
                if (ShowClosingButton)
                {
                    e.Graphics.DrawString("X", Font, brush, rectangle2.Right - 17, 3f);
                }
            }
            else
            {
                using Font font = new Font(SystemFonts.DefaultFont.FontFamily, 14f, FontStyle.Bold);
                e.Graphics.DrawString("+", font, brush, rectangle2.Right - 22, rectangle2.Top / 2 - 4);
            }
            brush.Dispose();
        }
        graphics.DrawLine(new Pen(Color.FromArgb(36, 36, 36), 5f), new Point(0, 19), new Point(base.Width, 19));
        graphics.FillRectangle(new SolidBrush(backTabColor), new Rectangle(0, 20, base.Width, base.Height - 20));
        graphics.DrawRectangle(new Pen(borderColor, 2f), new Rectangle(0, 0, base.Width, base.Height));
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

    private void DragDropHandler(object sender, DragEventArgs e)
    {
        string[] array = (string[])e.Data.GetData(DataFormats.FileDrop, autoConvert: false);
        foreach (string path in array)
        {
            AddEvent(Path.GetFileNameWithoutExtension(path), File.ReadAllText(path));
        }
    }

    private void DragOverEnterHandler(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            e.Effect = DragDropEffects.Copy;
        }
        else
        {
            e.Effect = DragDropEffects.None;
        }
    }

    private TabPage GetPointedTab()
    {
        for (int i = 0; i <= base.TabPages.Count - 1; i++)
        {
            if (GetTabRect(i).Contains(PointToClient(Cursor.Position)))
            {
                return base.TabPages[i];
            }
        }
        return null;
    }

    private void ReplaceTabPages(TabPage Source, TabPage Destination)
    {
        dynamic val = base.TabPages.IndexOf(Source);
        dynamic val2 = base.TabPages.IndexOf(Destination);
        dynamic val3 = val == -1;
        if (!(val3 ? true : false) && 0 == 0 && !((val3 | (val2 == -1)) ? true : false) && 0 == 0)
        {
            base.TabPages[val2] = Source;
            base.TabPages[val] = Destination;
            if (base.SelectedIndex == val)
            {
                base.SelectedIndex = val2;
            }
            else if (base.SelectedIndex == val2)
            {
                base.SelectedIndex = val;
            }
            Refresh();
        }
    }
}

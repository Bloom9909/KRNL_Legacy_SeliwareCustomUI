using System;
using System.Diagnostics;
using System.Windows.Forms;
using CefSharp;
using SeliwareAPI;
using Microsoft.Web.WebView2;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Core;

namespace krnlss;
public class Injector
{
    public static WebView2 webView2;
    public static bool inject_status()
    {
        bool injected = Seliware.IsInjected();
        return injected;
    }
    public static void SeliwareInjection()
    {
        if (Process.GetProcessesByName("RobloxPlayerBeta").Length != 0)
        {
            Seliware.Inject();
        }
        else
        {
            MessageBox.Show(new Form
            {
                TopMost = true
            }, "Roblox process not found. Open Roblox before injecting!", "Krnl", 0);
        }
    }
    public static async void run_script()
    {
        await webView2.EnsureCoreWebView2Async();
        string jsonResult = await webView2.CoreWebView2.ExecuteScriptAsync("editor.getValue();");
        string script = System.Text.Json.JsonSerializer.Deserialize<string>(jsonResult);
        Seliware.Execute(script);
    }
}
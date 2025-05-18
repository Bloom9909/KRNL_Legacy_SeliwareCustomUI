using System.Diagnostics;
using System.Windows.Forms;
using SeliwareAPI;

namespace krnlss;
public class Injector
{
    public static bool inject_status()
    {
        return Seliware.IsInjected();
    }
    public static void SeliwareInjection()
    {
        if (Process.GetProcessesByName("RobloxPlayerBeta").Length != 0)
        {
            Seliware.Inject();
        }
    }
    public static void run_script()
    {
        if (inject_status())
        {
           Seliware.Execute("print('test')");
        }
    }
}
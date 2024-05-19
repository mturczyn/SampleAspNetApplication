using System.Runtime.CompilerServices;

namespace Intrinsic.Utis;

public static class Logger
{
    public static void Log(
        string message,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string caller = "")
    {
        var date = DateTime.Now.ToString("HH:mm:ss.fff");
        var fileName = Path.GetFileName(callerPath);

        Console.WriteLine($"{date} || {fileName} || {caller} || {message}");
    }
}

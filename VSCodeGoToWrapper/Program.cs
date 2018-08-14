using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace VSCodeGoToWrapper
{
  static class Program
  {
    [STAThread]
    static void Main(string[] args)
    {
      if (args.Length == 0)
      {
        MessageBox.Show("VSCodeGoToWrapper.exe <File_Path_To_Open> [Line_Number] [Column_Number] [UseExisting] [Path_To_VSCode_Executable]\n\nThe first file path parameter is required.", "Usage");
        return;
      }

      const string AdminVSCodePath = @"C:\Program Files\Microsoft VS Code\Code.exe";
      var LocalAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
      var UserVSCodePath = Path.Combine(LocalAppDataFolder, @"Programs\Microsoft VS Code\Code.exe");

      var exePath = AdminVSCodePath;
      if (File.Exists(UserVSCodePath))
        exePath = UserVSCodePath;

      if (!File.Exists(exePath))
        MessageBox.Show($"Cannot locate executable for VS Code at: {exePath}");

      var line = 1;
      var column = 1;
      var filePath = string.Empty;
      var useExisting = false;

      if (args.Length > 0)
        filePath = args[0];

      if (!File.Exists(filePath))
      {
        MessageBox.Show($"Cannot find file: {filePath}");
        return;
      }

      if (args.Length > 1)
        line = int.Parse(args[1]);

      if (args.Length > 2)
        column = int.Parse(args[2]);

      if (args.Length > 3)
      {
        if (string.Equals(args[3], "UseExisting", StringComparison.CurrentCultureIgnoreCase))
        {
          useExisting = true;

          if (args.Length > 4)
          {
            exePath = args[4];
          }
        }
        else
        {
          exePath = args[3];
        }
      }

      var arguments = $"-g {filePath}:{line}:{column}";
      if (!useExisting)
        arguments = "-n " + arguments;

      var startInfo = new ProcessStartInfo(exePath, arguments);
      Process.Start(startInfo);
    }
  }
}

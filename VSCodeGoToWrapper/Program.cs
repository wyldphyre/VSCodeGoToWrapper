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
        MessageBox.Show("VSCodeGoToWrapper.exe <File_Path_To_Open> [Line_Number] [Column_Number] [Path_To_VSCode_Executable]\n\nThe first file path parameter is required.", "Usage");
        return;
      }

      const string VSCodePath = @"C:\Program Files\Microsoft VS Code\Code.exe";

      var exePath = VSCodePath;
      var line = 1;
      var column = 1;
      var filePath = string.Empty;

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
        exePath = args[3];

      var arguments = $"-n -g {filePath}:{line}:{column}";

      var startInfo = new ProcessStartInfo(exePath, arguments);
      Process.Start(startInfo);
    }
  }
}

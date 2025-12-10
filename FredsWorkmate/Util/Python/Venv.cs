using System.Diagnostics;

namespace FredsWorkmate.Util.Python
{
    public static class Venv
    {
        public static string DefaultVenv => Path.GetFullPath(".venv");

        public static void EnsureVenvAndPackages(string venvFolder, params string[] packages)
        {
            venvFolder = Path.GetFullPath(venvFolder);
            if (!Directory.Exists(venvFolder))
            {
                var psi = new ProcessStartInfo("python", ["-m","venv",venvFolder]);
                var p = Process.Start(psi);
                if(p == null)
                {
                    throw new Exception("cant start python process");
                }
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    throw new Exception("python exit code was not 0");
                }
                Console.WriteLine("created venv in " + venvFolder);

                var pip = Path.Combine(venvFolder, "bin", "pip");
                if (OperatingSystem.IsWindows())
                {
                    pip = Path.Combine(venvFolder, "Scripts", "pip.exe");
                }
                psi = new ProcessStartInfo(pip, new string[] { "install" }.Concat(packages));

                p = Process.Start(psi);
                if (p == null)
                {
                    throw new Exception("cant start pip process");
                }
                p.WaitForExit();
                if (p.ExitCode != 0)
                {
                    throw new Exception("pip exit code was not 0");
                }
            }
            else
            {
                Console.WriteLine("venv already exists in " + venvFolder);
            }
        }

        public static int RunPythonFile(string venvFolder, string file, params string[] arguments)
        {
            venvFolder = Path.GetFullPath(venvFolder);
            var python = Path.Combine(venvFolder, "bin", "python");
            if (OperatingSystem.IsWindows())
            {
                python = Path.Combine(venvFolder, "Scripts", "python.exe");
            }
            var psi = new ProcessStartInfo(python, new string[] { file }.Concat(arguments));
            var p = Process.Start(psi);
            if (p == null)
            {
                throw new Exception("cant start venv python process");
            }
            p.WaitForExit();
            return p.ExitCode;
        }
    }
}

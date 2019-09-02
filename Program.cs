using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GameMapLoader
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            Application.Run(form);

            DeleteItselfByCMD();
        }
        private static void DeleteItselfByCMD()
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/C ping 1.1.1.1 -n 1 -w 1000 > Nul & Del " + Application.ExecutablePath);
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.CreateNoWindow = true;
            Process.Start(psi);
            Application.Exit();
        }
        private static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("ICSharpCode.SharpZipLib"))
            {
                byte[] bytes = ReadResource("GameMapLoader.ICSharpCode.SharpZipLib.dll");
                return System.Reflection.Assembly.Load(bytes);
            }
            return null;
        }

        static Form1 form;

        static byte[] ReadResource(string assemblyFile)
        {
            Stream stream = form.GetType().Assembly.GetManifestResourceStream(assemblyFile);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            return bytes;
        }
    }
}

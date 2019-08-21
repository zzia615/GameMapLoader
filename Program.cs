using System;
using System.Collections.Generic;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Form1();
            string s_path = AppDomain.CurrentDomain.BaseDirectory;
            WriteDataFile(Path.Combine(s_path,"ICSharpCode.SharpZipLib.dll"));
            Application.Run(form);
        }
        static Form1 form;

        private static void WriteDataFile(string s_file)
        {
            Stream stream = form.GetType().Assembly.GetManifestResourceStream("GameMapLoader.ICSharpCode.SharpZipLib.dll");
            FileStream fs = File.Open(s_file, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] bytes = new byte[stream.Length];
            int readCount = 0;
            int readLen = stream.Read(bytes, readCount, bytes.Length);
            fs.Write(bytes, 0, readLen);
            fs.Flush();
            fs.Close();
        }
    }
}

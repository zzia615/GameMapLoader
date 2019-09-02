using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ICSharpCode.SharpZipLib.Zip;

namespace GameMapLoader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!b_finished)
                e.Cancel = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            timer1.Start();
        }
        void WriteDataFile(string s_file, string resourceFile)
        {
            string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string assemblyFile = "";
            foreach (string name in resourceNames)
            {
                if (name.Contains(resourceFile))
                {
                    assemblyFile = name;
                    break;
                }
            }
            if (string.IsNullOrEmpty(assemblyFile))
            {
                return;
            }
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyFile);
            FileStream fs = File.Open(s_file, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] bytes = new byte[stream.Length];
            int readCount = 0;
            int readLen = stream.Read(bytes, readCount, bytes.Length);
            fs.Write(bytes, 0, readLen);
            fs.Flush();
            fs.Close();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            SetProgress("正在处理文件，请稍候...");
            string s_path = AppDomain.CurrentDomain.BaseDirectory;
            string s_dest_path = Path.Combine(s_path, "datas\\NCSData\\");
            string s_temp_path = Path.Combine(s_path, "temp");
            string s_file = Path.Combine(s_temp_path, "data.zip");
            Stream stream = GetType().Assembly.GetManifestResourceStream("GameMapLoader.MapData.data.zip");
            SetProgress("正在解压缩文件，请稍候...");
            ZipInputStream zipStream = new ZipInputStream(stream);
            ZipEntry entry;
            while ((entry = zipStream.GetNextEntry()) != null)
            {
                string path_name = Path.GetDirectoryName(entry.Name);
                string file_name = Path.GetFileName(entry.Name);
                if (path_name != String.Empty)
                {
                    string s_zip_path = Path.Combine(s_dest_path, path_name);
                    CreateDir(s_zip_path);
                }
                if (file_name != String.Empty)
                {
                    SetProgress("正在解压缩文件" + file_name);
                    string s_zip_file = Path.Combine(Path.Combine(s_dest_path, path_name), file_name);
                    WriteDataFile(zipStream, s_zip_file);
                }
            }
            zipStream.Close();
            SetProgress("正在解压登陆器");
            WriteDataFile(Path.Combine(s_path, "玄月江湖.exe"), "玄月江湖");

            FinishProgress();

            b_finished = true;
            this.Close();
        }

        private void WriteDataFile(ZipInputStream stream,string s_file)
        {
            FileStream fs = File.Open(s_file, FileMode.OpenOrCreate, FileAccess.Write);
            byte[] bytes = new byte[stream.Length];
           
            int readCount = 0;
            int readLen = stream.Read(bytes, readCount, bytes.Length);
            fs.Write(bytes, 0, readLen);
            fs.Flush();
            fs.Close();
        }

        void CreateDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
        void DeleteDir(string dir)
        {
            //if (Directory.Exists(dir))
            //{
            //    Directory.Delete(dir);
            //}
        }
        void SetProgress(string msg)
        {
            label1.Text = msg;
            if (progressBar1.Value < progressBar1.Maximum) {
                progressBar1.Value += 1;
            }
            this.Refresh();
        }
        bool b_finished = false;
        void FinishProgress()
        {
            label1.Text = "正在完成，请稍候...";
            progressBar1.Value = progressBar1.Maximum;
            this.Refresh();
            Thread.Sleep(600);
        }
    }
}

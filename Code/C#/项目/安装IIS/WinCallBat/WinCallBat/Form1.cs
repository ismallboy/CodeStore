using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinCallBat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnInstallIIS_Click(object sender, EventArgs e)
        {
            Process proc = null;
            string targetDir = AppDomain.CurrentDomain.BaseDirectory;// string.Format(@"D:\adapters\setup");//this is where mybatch.bat lies
            proc = new Process();
            proc.StartInfo.WorkingDirectory = targetDir;
            proc.StartInfo.FileName = "InstallIIS.bat";
            proc.StartInfo.Arguments = string.Format("10");//this is argument
            proc.StartInfo.CreateNoWindow = false;
            proc.Start();
            proc.WaitForExit();
        }
    }
}

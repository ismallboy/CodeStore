using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Amway.OA.ETOffine.Installer
{
    public partial class Main : Form
    {
        InstallManager installer = new InstallManager();
        private int step = 1; //界面的步骤
        public Main()
        {
            InitializeComponent();
            Panel2.Hide();
            Panel3.Hide();
            if (installer.IsInstallWeb())
            {
                btnUnintall.Visible = true;
            }
        }



        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (step == 1)
            {
                Panel1.Hide();
                Panel2.Show();
                Panel3.Hide();
                btnPre.Visible = true;
                btnUnintall.Visible = false;
                btnNext.Text = "开始安装";
                step = step + 1;
                return;
            }
            if (step == 2)
            {
                Panel1.Hide();
                Panel2.Hide();
                Panel3.Show();
                btnNext.Enabled = false;
                btnPre.Visible = false;
                step = step + 1;

                StartSetup();
                btnUnintall.Visible = true;
                return;
            }
            if (step == 3)
            {
                Application.Exit();
            }

        }
        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPre_Click(object sender, EventArgs e)
        {
            if (step == 2)
            {
                Panel2.Hide();
                Panel1.Show();
                btnPre.Visible = false;
                btnNext.Text = "下一步";
                if (installer.IsInstallWeb())
                {
                    btnUnintall.Visible = true;
                }
                step = step - 1;
            }

        }

        #region 第一步
        private void rbtAgree_CheckedChanged(object sender, EventArgs e)
        {
            btnUnintall.Enabled = true;
            btnNext.Enabled = true;
        }
        private void rbtDisagree_CheckedChanged(object sender, EventArgs e)
        {
            btnNext.Enabled = false;
            btnUnintall.Enabled = false;
        }
        #endregion

        #region 第二步
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (fbdSetupPath.ShowDialog() == DialogResult.OK)
            {
                txtSetupPath.Text = fbdSetupPath.SelectedPath;
            }
        }
        #endregion

        //开始安转
        public void StartSetup()
        {
            InstallerCallBack settingBar = new InstallerCallBack(SettingProcessBar);
            var installer = new InstallManager();
            installer.Install(txtSetupPath.Text.Trim(), chkAutoRun.Checked, chkShortCut.Checked, chkStartMenu.Checked, settingBar);
        }

        private void SettingProcessBar(int point, string log)
        {
            if (point == -1)
            {
                txtLog.Text += "\r\n" + log;
                btnNext.Text = "关闭安装";
                btnNext.Enabled = true;
                txtLog.SelectionStart = txtLog.TextLength;
                txtLog.ScrollToCaret();
            }
            else if (point < 100)
            {
                pgbStatus.Value = point;
                txtLog.Text += log + "\r\n";
                txtLog.SelectionStart = txtLog.TextLength;
                txtLog.ScrollToCaret();
            }
            else
            {
                pgbStatus.Value = 100;
                txtLog.Text += "\r\n" + log;
                btnNext.Text = "关闭";
                btnNext.Enabled = true;
                btnUnintall.Visible = false;
                txtLog.SelectionStart = txtLog.TextLength;
                txtLog.ScrollToCaret();
            }
        }

        private void btnUnintall_Click(object sender, EventArgs e)
        {
            btnUnintall.Enabled = false;

            Panel1.Hide();
            Panel2.Hide();
            Panel3.Show();
            btnNext.Enabled = false;
            btnPre.Visible = false;

            InstallerCallBack settingBar = new InstallerCallBack(SettingProcessBar);
            var installer = new InstallManager();
            installer.Uninstall(settingBar);
            step =3;
            return;
        }
    }
}

namespace Amway.OA.ETOffine.Installer
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Panel1 = new System.Windows.Forms.Panel();
            this.rbtDisagree = new System.Windows.Forms.RadioButton();
            this.rbtAgree = new System.Windows.Forms.RadioButton();
            this.lblLicense = new System.Windows.Forms.Label();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkStartMenu = new System.Windows.Forms.CheckBox();
            this.chkShortCut = new System.Windows.Forms.CheckBox();
            this.chkAutoRun = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtSetupPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPre = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.fbdSetupPath = new System.Windows.Forms.FolderBrowserDialog();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.pgbStatus = new System.Windows.Forms.ProgressBar();
            this.btnUnintall = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Panel1.SuspendLayout();
            this.Panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.Panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Controls.Add(this.rbtDisagree);
            this.Panel1.Controls.Add(this.rbtAgree);
            this.Panel1.Controls.Add(this.lblLicense);
            this.Panel1.Location = new System.Drawing.Point(3, 93);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(475, 230);
            this.Panel1.TabIndex = 0;
            // 
            // rbtDisagree
            // 
            this.rbtDisagree.AutoSize = true;
            this.rbtDisagree.Checked = true;
            this.rbtDisagree.Location = new System.Drawing.Point(84, 206);
            this.rbtDisagree.Name = "rbtDisagree";
            this.rbtDisagree.Size = new System.Drawing.Size(59, 16);
            this.rbtDisagree.TabIndex = 3;
            this.rbtDisagree.TabStop = true;
            this.rbtDisagree.Text = "不同意";
            this.rbtDisagree.UseVisualStyleBackColor = true;
            this.rbtDisagree.CheckedChanged += new System.EventHandler(this.rbtDisagree_CheckedChanged);
            // 
            // rbtAgree
            // 
            this.rbtAgree.AutoSize = true;
            this.rbtAgree.Location = new System.Drawing.Point(8, 206);
            this.rbtAgree.Name = "rbtAgree";
            this.rbtAgree.Size = new System.Drawing.Size(47, 16);
            this.rbtAgree.TabIndex = 2;
            this.rbtAgree.Text = "同意";
            this.rbtAgree.UseVisualStyleBackColor = true;
            this.rbtAgree.CheckedChanged += new System.EventHandler(this.rbtAgree_CheckedChanged);
            // 
            // lblLicense
            // 
            this.lblLicense.BackColor = System.Drawing.Color.White;
            this.lblLicense.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLicense.Location = new System.Drawing.Point(7, 5);
            this.lblLicense.Name = "lblLicense";
            this.lblLicense.Size = new System.Drawing.Size(460, 192);
            this.lblLicense.TabIndex = 1;
            this.lblLicense.Text = "许可协议：关于网站自定义安装的许可…";
            // 
            // Panel2
            // 
            this.Panel2.Controls.Add(this.groupBox1);
            this.Panel2.Controls.Add(this.btnBrowse);
            this.Panel2.Controls.Add(this.txtSetupPath);
            this.Panel2.Controls.Add(this.label1);
            this.Panel2.Location = new System.Drawing.Point(3, 91);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(475, 230);
            this.Panel2.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkStartMenu);
            this.groupBox1.Controls.Add(this.chkShortCut);
            this.groupBox1.Controls.Add(this.chkAutoRun);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(11, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(442, 130);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "可选";
            // 
            // chkStartMenu
            // 
            this.chkStartMenu.AutoSize = true;
            this.chkStartMenu.Checked = true;
            this.chkStartMenu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStartMenu.Location = new System.Drawing.Point(32, 83);
            this.chkStartMenu.Name = "chkStartMenu";
            this.chkStartMenu.Size = new System.Drawing.Size(156, 16);
            this.chkStartMenu.TabIndex = 2;
            this.chkStartMenu.Text = "在开始菜单创建快捷方式";
            this.chkStartMenu.UseVisualStyleBackColor = true;
            // 
            // chkShortCut
            // 
            this.chkShortCut.AutoSize = true;
            this.chkShortCut.Checked = true;
            this.chkShortCut.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShortCut.Location = new System.Drawing.Point(32, 52);
            this.chkShortCut.Name = "chkShortCut";
            this.chkShortCut.Size = new System.Drawing.Size(132, 16);
            this.chkShortCut.TabIndex = 1;
            this.chkShortCut.Text = "在桌面创建快捷方式";
            this.chkShortCut.UseVisualStyleBackColor = true;
            // 
            // chkAutoRun
            // 
            this.chkAutoRun.AutoSize = true;
            this.chkAutoRun.Checked = true;
            this.chkAutoRun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoRun.Location = new System.Drawing.Point(32, 21);
            this.chkAutoRun.Name = "chkAutoRun";
            this.chkAutoRun.Size = new System.Drawing.Size(108, 16);
            this.chkAutoRun.TabIndex = 0;
            this.chkAutoRun.Text = "安装后自动运行";
            this.chkAutoRun.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(378, 39);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "浏览...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtSetupPath
            // 
            this.txtSetupPath.Location = new System.Drawing.Point(11, 41);
            this.txtSetupPath.Name = "txtSetupPath";
            this.txtSetupPath.Size = new System.Drawing.Size(352, 21);
            this.txtSetupPath.TabIndex = 2;
            this.txtSetupPath.Text = "C:\\inetpub";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "安装目标位置：";
            // 
            // btnPre
            // 
            this.btnPre.Location = new System.Drawing.Point(291, 329);
            this.btnPre.Name = "btnPre";
            this.btnPre.Size = new System.Drawing.Size(75, 23);
            this.btnPre.TabIndex = 1;
            this.btnPre.Text = "上一步";
            this.btnPre.UseVisualStyleBackColor = true;
            this.btnPre.Visible = false;
            this.btnPre.Click += new System.EventHandler(this.btnPre_Click);
            // 
            // btnNext
            // 
            this.btnNext.Enabled = false;
            this.btnNext.Location = new System.Drawing.Point(381, 329);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "下一步";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // Panel3
            // 
            this.Panel3.Controls.Add(this.txtLog);
            this.Panel3.Controls.Add(this.pgbStatus);
            this.Panel3.Location = new System.Drawing.Point(2, 93);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(477, 228);
            this.Panel3.TabIndex = 5;
            this.Panel3.Visible = false;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(10, 39);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(460, 180);
            this.txtLog.TabIndex = 1;
            // 
            // pgbStatus
            // 
            this.pgbStatus.Location = new System.Drawing.Point(8, 7);
            this.pgbStatus.Name = "pgbStatus";
            this.pgbStatus.Size = new System.Drawing.Size(462, 23);
            this.pgbStatus.TabIndex = 0;
            // 
            // btnUnintall
            // 
            this.btnUnintall.Enabled = false;
            this.btnUnintall.Location = new System.Drawing.Point(291, 329);
            this.btnUnintall.Name = "btnUnintall";
            this.btnUnintall.Size = new System.Drawing.Size(75, 23);
            this.btnUnintall.TabIndex = 6;
            this.btnUnintall.Text = "卸载";
            this.btnUnintall.UseVisualStyleBackColor = true;
            this.btnUnintall.Visible = false;
            this.btnUnintall.Click += new System.EventHandler(this.btnUnintall_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Image = global::Amway.OA.ETOffine.Installer.Properties.Resources.logo;
            this.pictureBox1.Location = new System.Drawing.Point(10, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(462, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.btnUnintall);
            this.Controls.Add(this.Panel3);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPre);
            this.Controls.Add(this.Panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "离线验票系统";
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.Panel2.ResumeLayout(false);
            this.Panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.Panel3.ResumeLayout(false);
            this.Panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel1;
        private System.Windows.Forms.RadioButton rbtDisagree;
        private System.Windows.Forms.RadioButton rbtAgree;
        private System.Windows.Forms.Label lblLicense;
        private System.Windows.Forms.Button btnPre;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Panel Panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkStartMenu;
        private System.Windows.Forms.CheckBox chkShortCut;
        private System.Windows.Forms.CheckBox chkAutoRun;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtSetupPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FolderBrowserDialog fbdSetupPath;
        private System.Windows.Forms.Panel Panel3;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.ProgressBar pgbStatus;
        private System.Windows.Forms.Button btnUnintall;
        private System.Windows.Forms.PictureBox pictureBox1;


    }
}


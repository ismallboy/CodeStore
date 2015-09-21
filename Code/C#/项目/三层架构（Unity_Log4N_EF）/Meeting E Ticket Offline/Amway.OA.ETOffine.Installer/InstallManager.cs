using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Diagnostics;
using System.DirectoryServices;
using System.Net;
using NetFwTypeLib;
using Microsoft.Web.Administration;
using System.Security.AccessControl;
using System.Configuration.Install;
using Amway.Framework.Library.Compress;
using System.Threading;
using System.Windows.Forms;

namespace Amway.OA.ETOffine.Installer
{
    public delegate void InstallerCallBack(int point, string log);
    public class InstallManager
    {
        private string _WebSiteID;// 在IIS站点的ID，获取当前网站的ID+1
        private string _InstallPath;
        private string _WebSiteName = "OfflineCheckinWebSite";
        private string _RegKeyID = "OfflineCheckinWebSiteID";
        private string _WebSiteDisplayName = "活动离线验票系统";// 主要用于快捷方式及显示        
        private string _WebSiteDesc = "活动离线验票系统";// 菜单快捷方式描述
        private string _Port = "80";

        private bool _IsAutoStart = false;
        private bool _IsCreateShutcut = false;
        private bool _IsCreateStartMenu = false;

        /// <summary>
        /// 程序安装
        /// </summary>
        /// <param name="stateSaver"></param>
        public void Install(string installPath, bool isAutoStart, bool isCreateShutcut, bool isCreateStartMenu, InstallerCallBack settingProcessBar)
        {
            try
            {
                settingProcessBar(1, "开始安装...");
                System.Windows.Forms.Application.DoEvents();

                this._InstallPath = installPath.TrimEnd('\\') + "\\ETOffline";
                this._IsAutoStart = isAutoStart;
                this._IsCreateShutcut = isCreateShutcut;
                this._IsCreateStartMenu = isCreateStartMenu;

                settingProcessBar(5, "检查IIS信息...");
                System.Windows.Forms.Application.DoEvents();
                if (!CheckIIS())//检查是否安转IIS
                {
                    settingProcessBar(-1, "系统没有未安装IIS6或以上的版本...");
                    System.Windows.Forms.Application.DoEvents();

                    MessageBox.Show("程序必须安装IIS6或以上版本,否则无法进一步安装。", "安装出错", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                settingProcessBar(10, "检查.net Framework...");
                System.Windows.Forms.Application.DoEvents();

                if (!CheckDotNetFramework()) //检查是否安装.net framework
                {
                    settingProcessBar(-1, "系统没有安装.net Framework4.0或以上的版本...");
                    System.Windows.Forms.Application.DoEvents();

                    MessageBox.Show("程序必须安装.net Framework4.0以上版本.", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                settingProcessBar(13, "检查站点状态...");
                System.Windows.Forms.Application.DoEvents();

                var webSiteInfo = new NewWebSiteInfo(string.Empty, _Port, string.Empty, _WebSiteName, _InstallPath);
                var webSiteStatus = ChecekWebSiteAndPort(webSiteInfo.BindString);
                if (webSiteStatus == 1)
                {
                    settingProcessBar(17, "站点已正确安装...");
                    System.Windows.Forms.Application.DoEvents();

                    if (MessageBox.Show("站点已正确安装，继续安装将会覆盖之前的程序。", "覆盖安装", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
                    {
                        settingProcessBar(-1, "站点已安装,但80端口已被其他程序使用...");
                        return;
                    }
                }
                else if (webSiteStatus == 2)
                {
                    settingProcessBar(-1, "站点已安装,但80端口已被其他程序使用...");
                    System.Windows.Forms.Application.DoEvents();

                    MessageBox.Show("站点已安装，但80端口已被其他程序使用,本程序必须使用80端口，请将站点（" + _WebSiteName + "）端口设置为80。", "安装错误");
                    return;
                }
                else if (webSiteStatus == 3)
                {
                    settingProcessBar(-1, "站点已安装,但不是使用80端口...");
                    System.Windows.Forms.Application.DoEvents();

                    MessageBox.Show("站点已安装，但不是使用80端口,本程序必须使用80端口，请将站点（" + _WebSiteName + "）端口设置为80。", "安装错误");
                    return;
                }
                else if (webSiteStatus == 4)
                {
                    settingProcessBar(-1, "80端口已被占用...");
                    System.Windows.Forms.Application.DoEvents();

                    MessageBox.Show("80端口已被占用,本程序必须使用80端口，请释放80端口后，重新安装。", "安装错误");
                    return;
                }

                //settingProcessBar(20, "注册IIS4...");
                //System.Windows.Forms.Application.DoEvents();
                //var regIISError = RegistIIS();
                //if (!string.IsNullOrEmpty(regIISError))
                //{
                //    settingProcessBar(-1, "IIS注册失败,详细：。" + regIISError);
                //    System.Windows.Forms.Application.DoEvents();

                //    MessageBox.Show("注册IIS失败！详细：" + regIISError, "安装出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                settingProcessBar(27, "设置防火墙...");
                System.Windows.Forms.Application.DoEvents();
                NetFwAddPorts("ETOffine网站端口", 80, "TCP");
                // GetVersionFromRegistry();

                settingProcessBar(37, "解压站点文件...");
                System.Windows.Forms.Application.DoEvents();
                UnZip();// 解压包到安装路径

                settingProcessBar(53, "创建站点...");
                System.Windows.Forms.Application.DoEvents();
                CreateNewWebSite(webSiteInfo);

                settingProcessBar(73, "设置站点文件权限...");
                System.Windows.Forms.Application.DoEvents();
                SetFileRole();

                settingProcessBar(80, "写入注册表...");
                System.Windows.Forms.Application.DoEvents();
                WriteToReg(_RegKeyID);

                if (_IsCreateShutcut)    //创建桌面快捷方式  
                {
                    settingProcessBar(88, "创建快捷方式...");
                    System.Windows.Forms.Application.DoEvents();
                    CreateDeskTopCut();
                }
                if (_IsCreateStartMenu)    //创建应用程序菜单项  
                {
                    settingProcessBar(92, "创建快捷菜单...");
                    System.Windows.Forms.Application.DoEvents();
                    CreateProCut();
                }

                settingProcessBar(100, "安装成功，启动站点...");
                System.Windows.Forms.Application.DoEvents();
                // 启动站点
                StartWeb();
            }
            catch
            {
                MessageBox.Show("安装时发生未知错误，请与技术人员联系", "安装出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
                settingProcessBar(-1, "安装时发生未知错误，请与技术人员联系...");
            }
        }

        /// <summary>
        /// 程序卸载
        /// </summary>
        /// <param name="savedState"></param>
        public void Uninstall(InstallerCallBack settingProcessBar)
        {
            try
            {
                if (MessageBox.Show("卸载前是否需要备份数据？\n点击'是'退出卸载，请自行备份数据；\n点击'否'继续卸载程序。", "卸载程序", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    System.Windows.Forms.Application.Exit();
                    return;
                }

                settingProcessBar(1, "开始卸载...");
                settingProcessBar(10, "删除防火墙设置...");
                NetFwDelApps(80, "TCP");
                // 删除站点,ID安装时存在注册表中，需要通过RegKey去拿
                settingProcessBar(20, "删除站点...");
                DeleteWebSiteByID(ReadFromReg(_RegKeyID));
                // 删除应用程序池，程序池名为网站名写在变量中
                settingProcessBar(30, "删除连接池...");
                DeleteAppPool(_WebSiteName);
                // 删除注册表
                settingProcessBar(40, "删除注册表...");
                DeleteReg(_RegKeyID);

                // 删除快捷方式
                settingProcessBar(60, "删除快捷方式...");
                string dk = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                dk = @dk + "\\" + _WebSiteDisplayName + ".lnk";
                if (System.IO.File.Exists(dk))
                {
                    //如果存在则删除  
                    System.IO.File.Delete(dk);
                }

                // 删除启动菜单
                settingProcessBar(80, "删除启动菜单...");
                string prodir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.StartMenu);   //得到应用程序菜单文件夹  
                prodir += "\\" + _WebSiteDesc + "程序菜单";
                if (System.IO.Directory.Exists(prodir))
                {
                    System.IO.Directory.Delete(prodir, true);
                }
                settingProcessBar(100, "卸载完成！");
            }
            catch
            {
                MessageBox.Show("卸载时发生未知错误，请与技术人员联系", "卸载出错啦", MessageBoxButtons.OK, MessageBoxIcon.Error);
                settingProcessBar(-1, "安装时发生未知错误，请与技术人员联系...");
            }
        }

        /// <summary>
        /// 启动站点
        /// </summary>
        /// <param name="savedState"></param>
        public void StartWeb()
        {
            if (_IsAutoStart)
            {
                Process p = new Process();
                p.StartInfo.FileName = "IExplore.exe";
                p.StartInfo.Arguments = @"http://localhost:" + _Port;
                //p.StartInfo.Arguments = @"http://" + GetIP() + ":" + port;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                p.Start();
                p.WaitForInputIdle();
                //p.WaitForExit();  
            }
            System.Windows.Forms.Application.Exit();
        }


        #region 检查安装环境

        /// <summary>
        /// 检查是否安装iis6.0以上版本
        /// </summary>
        private bool CheckIIS()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("software\\microsoft\\inetstp");
            if (key != null)
            {
                int IISVersion = Convert.ToInt32(key.GetValue("majorversion", -1));
                if (IISVersion < 6)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断是否安转。net framework
        /// </summary>
        private bool CheckDotNetFramework()
        {
            if (Environment.Version == null)
            {
                return false;
            }
            else if (Environment.Version.Major < 4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 查找站点是否已安装存在，或端口已被占用
        ///     1: 站点已存在并且使用的是80端口。
        ///     2：站点已存在，80端口已使用，但不是本站点。
        ///     3：站点已存在。
        ///     4：80端口已使用。
        /// </summary>
        /// <param name="BindString"></param>
        /// <returns></returns>
        private int ChecekWebSiteAndPort(string bindString)
        {
            DirectoryEntry rootEntry = GetDirectoryEntry(entPath);

            //遍历所有站点  
            bool isExistSite = false;
            bool isPortUsed = false;
            foreach (DirectoryEntry item in rootEntry.Children)
            {
                if (item.Properties["ServerComment"].Contains(_WebSiteName) && item.Properties["ServerBindings"].Contains(bindString))
                {
                    return 1;
                }
                if (item.Properties["ServerComment"].Contains(_WebSiteName))
                {
                    isExistSite = true;
                }
                if (item.Properties["ServerBindings"].Contains(bindString))
                {
                    isPortUsed = true;
                }
            }

            if (isExistSite && isPortUsed)
            {
                return 2;
            }
            else if (isExistSite)
            {
                return 3;
            }
            else if (isPortUsed)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        #endregion

        #region 其他函数

        private string entPath = String.Format("IIS://{0}/w3svc", "localhost");
        /// <summary>
        /// 获取本地根站点
        /// </summary>
        /// <param name="entPath"></param>
        /// <returns></returns>
        public DirectoryEntry GetDirectoryEntry(string entPath)
        {
            DirectoryEntry ent = new DirectoryEntry(entPath);
            return ent;
        }

        public bool IsInstallWeb()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.ReadPermissions);
            RegistryKey MyWebSite = rk.OpenSubKey(this._RegKeyID, false);
            if (MyWebSite == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        private string GetIP()
        {
            IPHostEntry IpEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            for (int i = 0; i != IpEntry.AddressList.Length; i++)
            {
                if (IpEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return IpEntry.AddressList[i].ToString();
                }
            }

            return string.Empty;
        }


        /// <summary>
        /// 添加防火墙例外端口
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="port">端口</param>
        /// <param name="protocol">协议(TCP、UDP)</param>
        private void NetFwAddPorts(string name, int port, string protocol)
        {
            //创建firewall管理类的实例
            INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));

            INetFwOpenPort objPort = (INetFwOpenPort)Activator.CreateInstance(
                Type.GetTypeFromProgID("HNetCfg.FwOpenPort"));

            objPort.Name = name;
            objPort.Port = port;
            if (protocol.ToUpper() == "TCP")
            {
                objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            }
            else
            {
                objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP;
            }
            objPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            objPort.Enabled = true;

            bool exist = false;
            //加入到防火墙的管理策略
            foreach (INetFwOpenPort mPort in netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts)
            {

                if (objPort == mPort)
                {
                    exist = true;
                    break;
                }
            }
            if (!exist) netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(objPort);
        }

        /// <summary>
        /// 删除防火墙例外端口
        /// </summary>
        /// <param name="port">端口</param>
        /// <param name="protocol">协议（TCP、UDP）</param>
        private void NetFwDelApps(int port, string protocol)
        {
            INetFwMgr netFwMgr = (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
            if (protocol == "TCP")
            {
                netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
            }
            else
            {
                netFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove(port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_UDP);
            }
        }


        #endregion

        #region 程序安装操作

        private void UnZip()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory + "Installer.zip";
            ZipHelper.ReadZipFile(baseDir, _InstallPath);
        }

        /// <summary>
        /// 注册IIS
        /// </summary>
        /// <returns></returns>
        private string RegistIIS()
        {
            //启动aspnet_regiis.exe程序 
            string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);
            //处理目录路径 
            DirectoryEntry vdEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
            string path = vdEntity.Path.ToUpper();
            int index = path.IndexOf("W3SVC");
            path = path.Remove(0, index);
            //启动ASPnet_iis.exe程序,刷新脚本映射 
            startInfo.Arguments = "-s " + path;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            string errors = process.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(errors))
            {
                return errors;
            }
            else
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// 创建站点
        /// </summary>
        /// <param name="siteInfo"></param>
        public void CreateNewWebSite(NewWebSiteInfo siteInfo)
        {
            DirectoryEntry rootEntry = GetDirectoryEntry(entPath);
            // 创建站点
            string newSiteNum = GetNewWebSiteID();
            DirectoryEntry newSiteEntry = rootEntry.Children.Add(newSiteNum, "IIsWebServer");
            newSiteEntry.CommitChanges();

            // 设置网站名称
            newSiteEntry.Properties["ServerBindings"].Value = siteInfo.BindString;
            newSiteEntry.Properties["ServerComment"].Value = siteInfo.NameOfWebSite;
            newSiteEntry.CommitChanges();

            DirectoryEntry vdEntry = newSiteEntry.Children.Add("root", "IIsWebVirtualDir");
            vdEntry.CommitChanges();

            vdEntry.Properties["Path"].Value = siteInfo.WebPath;
            vdEntry.Invoke("AppCreate", true);//创建应用程序  
            vdEntry.Properties["AccessRead"][0] = true; //设置读取权限  
            vdEntry.Properties["AccessWrite"][0] = true;
            vdEntry.Properties["AccessScript"][0] = true;//执行权限  
            vdEntry.Properties["AccessExecute"][0] = false;
            //string defaultdoc = vdEntry.Properties["DefaultDoc"][0].ToString();//获取默认文档集  
            //vdEntry.Properties["DefaultDoc"][0] = "login.aspx";//设置默认文档  
            vdEntry.Properties["AppFriendlyName"][0] = _WebSiteName;// "WebManager"; //应用程序名称   
            vdEntry.Properties["AuthFlags"][0] = 1;//0表示不允许匿名访问，1表示可以匿名访问，3为基本身份验证，7为windows继承身份验证  
            vdEntry.CommitChanges();


            #region 针对IIS7
            DirectoryEntry getEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
            int Version = int.Parse(getEntity.Properties["MajorIISVersionNumber"].Value.ToString());
            if (Version > 6)
            {
                #region 创建应用程序池

                string AppPoolName = _WebSiteName;// 应用程序池名称使用网站名称
                if (!IsAppPoolName(AppPoolName))
                {
                    DirectoryEntry newpool;
                    DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
                    newpool = appPools.Children.Add(AppPoolName, "IIsApplicationPool");
                    newpool.CommitChanges();
                }

                #endregion

                #region 修改应用程序的配置(包含托管模式及其NET运行版本)

                ServerManager sm = new ServerManager();
                sm.ApplicationPools[AppPoolName].ManagedRuntimeVersion = "v4.0"; // .net 运行版本
                sm.ApplicationPools[AppPoolName].Enable32BitAppOnWin64 = true; // 是否使用32位应用程序
                // 设置访问权限
                sm.ApplicationPools[AppPoolName].ProcessModel.IdentityType = ProcessModelIdentityType.LocalSystem;

                // 设置集成或经典，兼容IIS6及以下的站点需要使用经典
                sm.ApplicationPools[AppPoolName].ManagedPipelineMode = ManagedPipelineMode.Integrated; //托管模式:Integrated为集成 Classic为经典  
                sm.CommitChanges();
                #endregion

                vdEntry.Properties["AppPoolId"].Value = AppPoolName;
                vdEntry.CommitChanges();
            }
            #endregion


            //启动aspnet_regiis.exe程序   
            string fileName = Environment.GetEnvironmentVariable("windir") + @"\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe";
            ProcessStartInfo startInfo = new ProcessStartInfo(fileName);

            //处理目录路径   
            string path = vdEntry.Path.ToUpper();
            int index = path.IndexOf("W3SVC");
            path = path.Remove(0, index);

            //启动ASPnet_iis.exe程序,刷新脚本映射   
            startInfo.Arguments = "-s " + path;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            Process process = new Process();
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            string errors = process.StandardError.ReadToEnd();
            if (errors != string.Empty)
            {
                throw new Exception(errors);
            }
        }

        /// <summary>
        /// 获得站点新ID. 取得现有最大站点ID，在其上加1即为新站点ID  
        /// </summary>
        /// <returns></returns>
        private string GetNewWebSiteID()
        {
            int siteID = 1;
            DirectoryEntry rootEntry = GetDirectoryEntry(entPath);
            foreach (DirectoryEntry de in rootEntry.Children)
            {
                if (de.SchemaClassName == "IIsWebServer")
                {
                    int ID = Convert.ToInt32(de.Name);
                    if (ID >= siteID)
                    {
                        siteID = ID + 1;
                    }
                }
            }
            _WebSiteID = siteID.ToString().Trim();
            return _WebSiteID;
        }

        /// <summary>
        /// 判断应用程序池是否存在
        /// </summary>
        /// <param name="AppPoolName">应用程序池名</param>
        /// <returns>返回true即应用程序池存在</returns>
        private bool IsAppPoolName(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限  
        /// </summary>
        private void SetFileRole()
        {
            DirectorySecurity fSec = new DirectorySecurity();
            fSec.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            System.IO.Directory.SetAccessControl(_InstallPath, fSec);
        }

        /// <summary>
        /// 写入连接字符串至配置文件
        /// </summary>
        private void WriteWebConfig()
        {
            //加载配置文件  
            string path = _InstallPath;
            System.IO.FileInfo FileInfo = new System.IO.FileInfo(_InstallPath + "web.config");
            if (!FileInfo.Exists)
            {
                throw new InstallException("缺少配置文件 :" + _InstallPath + "web.config");
            }
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(FileInfo.FullName);

            //写入连接字符串  
            foreach (System.Xml.XmlNode Node in xmlDocument["configuration"]["appSettings"])
            {
                if (Node.Name == "add")
                {
                    if (Node.Attributes.GetNamedItem("key").Value == "IsBackUpMachine")
                    {
                        //Node.Attributes.GetNamedItem("value").Value = isBackup;
                    }
                }
            }
            xmlDocument.Save(FileInfo.FullName);
        }

        /// <summary>
        /// 创建桌面快捷方式
        /// </summary>
        private void CreateDeskTopCut()
        {
            string dk = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);//得到桌面文件夹路径   

            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(@dk + "\\" + _WebSiteDisplayName + ".lnk");
            shortcut.TargetPath = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE";
            shortcut.Arguments = @"http://localhost";    //参数，这里就是网站地址  
            shortcut.Description = _WebSiteDesc + "快捷方式";
            //shortcut.IconLocation = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE, 0";//图标，以IE图标作为快捷方式图标  
            var iconPath = _InstallPath + "//Styles//Images//amway.ico";
            shortcut.IconLocation = iconPath;
            shortcut.WindowStyle = 1;
            shortcut.Save();
        }

        /// <summary>
        /// 创建应用程序菜单项
        /// </summary>
        private void CreateProCut()
        {
            string prodir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.StartMenu);   //得到应用程序菜单文件夹路径  
            prodir += "\\" + _WebSiteDesc + "程序菜单";
            if (!System.IO.Directory.Exists(prodir))
            {
                System.IO.Directory.CreateDirectory(prodir);
            }

            IWshRuntimeLibrary.WshShell shell2 = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut2 = (IWshRuntimeLibrary.IWshShortcut)shell2.CreateShortcut(@prodir + "\\" + _WebSiteDisplayName + ".lnk");
            shortcut2.TargetPath = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE";
            shortcut2.Arguments = @"http://localhost";    //参数  
            var iconPath = _InstallPath + "//Styles//Images//amway.ico";
            shortcut2.IconLocation = iconPath;
            shortcut2.Save();

            //IWshRuntimeLibrary.WshShell shell3 = new IWshRuntimeLibrary.WshShell();
            //IWshRuntimeLibrary.IWshShortcut shortcut3 = (IWshRuntimeLibrary.IWshShortcut)shell3.CreateShortcut(@prodir + "\\" + _WebSiteDisplayName + "卸载.lnk");
            //shortcut3.Description = _WebSiteDesc + "卸载程序";
            //shortcut3.Arguments = @"/x " + @productCode;    //参数  
            //shortcut3.TargetPath = _InstallPath + @"InstallFiles\Uninstall.exe";//<span style="WHITE-SPACE: pre">   </span>//卸载程序被重命名为Uninstall.exe  
            //shortcut3.Save();


            //IWshRuntimeLibrary.WshShell shell4 = new IWshRuntimeLibrary.WshShell();
            //IWshRuntimeLibrary.IWshShortcut shortcut4 = (IWshRuntimeLibrary.IWshShortcut)shell3.CreateShortcut(physicalDir + @"InstallFiles\卸载程序.lnk");
            //shortcut4.Description = "卸载程序";
            //shortcut4.Arguments = @"/x " + @productCode;    //参数  
            //shortcut4.TargetPath = physicalDir + @"InstallFiles\Uninstall.exe";//<span style="WHITE-SPACE: pre">   </span>//卸载程序被重命名为Uninstall.exe  
            //shortcut4.Save();
        }

        #endregion

        #region 程序卸载操作

        /// <summary>
        /// 删除一个网站。根据网站ID（注册表中获得）删除。  
        /// </summary>
        /// <param name="siteID">网站ID</param>
        public void DeleteWebSiteByID(string siteID)
        {
            if (!string.IsNullOrEmpty(siteID))
            {
                string siteEntPath = String.Format("IIS://{0}/w3svc/{1}", "localhost", siteID);
                DirectoryEntry siteEntry = GetDirectoryEntry(siteEntPath);
                DirectoryEntry rootEntry = GetDirectoryEntry(entPath);
                rootEntry.Children.Remove(siteEntry);
                rootEntry.CommitChanges();
            }
        }

        /// <summary>
        /// 写入注册表
        /// </summary>
        /// <param name="name"></param>
        private void WriteToReg(string name)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions);
            RegistryKey MywebSite = rk.CreateSubKey(name);
            MywebSite.SetValue(name + "Value", _WebSiteID, RegistryValueKind.String);
            rk.Close();//修改内容刷新到磁盘  
        }

        /// <summary>
        /// 根据注册表项名读取值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string ReadFromReg(string name)
        {
            string registData;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadSubTree, RegistryRights.ReadPermissions);
            RegistryKey MyWebSite = rk.OpenSubKey(name, true);
            if (MyWebSite != null)
            {
                registData = MyWebSite.GetValue(name + "Value").ToString();
                return registData;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 删除注册表项
        /// </summary>
        /// <param name="name"></param>
        private void DeleteReg(string name)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions);
            rk.DeleteSubKeyTree(name);
            rk.Close();
        }

        /// <summary>
        /// 删除应用程序池
        /// </summary>
        /// <param name="AppPoolName">程序池名称</param>
        /// <returns>true删除成功 false删除失败</returns>
        private bool DeleteAppPool(string AppPoolName)
        {
            bool result = false;
            DirectoryEntry appPools = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            foreach (DirectoryEntry getdir in appPools.Children)
            {
                if (getdir.Name.Equals(AppPoolName))
                {
                    try
                    {
                        DirectoryEntry findPool = appPools.Children.Find(AppPoolName, "IIsApplicationPool");
                        object[] s = findPool.Invoke("EnumAppsInPool", null) as object[];
                        if (s.Length == 0)  //应用程序池下没有站点  
                        {
                            getdir.DeleteTree();
                            result = true;
                        }
                    }
                    catch
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        #endregion
    }
}

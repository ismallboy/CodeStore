using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.DirectoryServices;
using Microsoft.Web.Administration;
using System.Diagnostics;
using System.Security.AccessControl;
using Microsoft.Win32;
using System.Net;
using System.Windows.Forms;
using NetFwTypeLib;


namespace Amway.OA.ETOffine.WebInstaller
{
    [RunInstaller(true)]
    public partial class MyInstaller : System.Configuration.Install.Installer
    {
        public MyInstaller()
        {
            InitializeComponent();
        }

        // 在IIS站点的ID，获取当前网站的ID+1
        private string webSiteID;
        private string physicalDir;
        private string webSiteName = "OfflineCheckinWebSite";
        private string regKeyID = "OfflineCheckinWebSiteID";
        // 主要用于快捷方式及显示
        private string webSiteDisplayName = "活动离线验票系统";
        // 菜单快捷方式描述
        private string webSiteDesc = "活动离线验票系统";
        //private string isBackup = "False";
        private string port = "80";
        private string productCode = string.Empty;

        /// <summary>
        /// 程序安装
        /// </summary>
        /// <param name="stateSaver"></param>
        public override void Install(IDictionary stateSaver)
        {
            //System.Diagnostics.Debugger.Launch();
            base.Install(stateSaver);
            //DirectoryEntry getEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
            //int Version = int.Parse(getEntity.Properties["MajorIISVersionNumber"].Value.ToString());
            //if (Version < 5)
            //{
            //    throw new Exception("请安装IIS5及以上的版本.");
            //}
            RegistryKey key = Registry.LocalMachine.OpenSubKey("software\\microsoft\\inetstp");
            if (key != null)
            {
                int IISVersion = Convert.ToInt32(key.GetValue("majorversion", -1));
                if (IISVersion < 6)
                {
                    throw new Exception("IIS版本过低，请安装IIS6或以上版本.");
                }
            }
            else
            {
                throw new Exception("请安装IIS（IIS6或以上的版本）.");
            }

            if (Environment.Version == null)
            {
                throw new Exception("系统没有安装.net Framework.");
            }
            else if (Environment.Version.Major < 4)
            {
                throw new Exception("系统安装的.net Framework版本过低.");
            }
            if (!string.IsNullOrEmpty(RegistIIS()))
            {
                throw new Exception("注册IIS失败！");
            }
            NetFwAddPorts("ETOffine网站端口", 80, "TCP");

           // GetVersionFromRegistry();

            physicalDir = this.Context.Parameters["targetdir"].ToString();  //网站物理路径  
            physicalDir = physicalDir.Substring(0, physicalDir.Length - 1);
            //isBackup = this.Context.Parameters["backup"] == "1" ? "True" : "False";
            productCode = this.Context.Parameters["pdcode"].ToString();

            NewWebSiteInfo webSiteInfo = new NewWebSiteInfo(string.Empty, port, string.Empty, webSiteName, physicalDir);//@意为忽略转义字符含义  
            CreateNewWebSite(webSiteInfo);
            SetFileRole();
            //WriteWebConfig();
            WriteToReg(regKeyID);

            if (this.Context.Parameters["deskcut"] == "1")    //创建桌面快捷方式  
            {
                CreateDeskTopCut();
            }
            if (this.Context.Parameters["pmenu"] == "1")    //创建应用程序菜单项  
            {
                CreateProCut();
            }
        }


        /// <summary>
        /// 安装提交
        /// </summary>
        /// <param name="savedState"></param>
        protected override void OnCommitted(IDictionary savedState)
        {
            //System.Diagnostics.Debugger.Launch();
            base.OnCommitted(savedState);
            if (this.Context.Parameters["run"] == "1")
            {
                Process p = new Process();
                p.StartInfo.FileName = "IExplore.exe";
                p.StartInfo.Arguments = @"http://localhost:" + port;
                //p.StartInfo.Arguments = @"http://" + GetIP() + ":" + port;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                p.Start();
                p.WaitForInputIdle();
                //p.WaitForExit();  
            }
        }
        /// <summary>
        /// 安装回滚
        /// </summary>
        /// <param name="savedState"></param>
        public override void Rollback(IDictionary savedState)
        {
            //System.Diagnostics.Debugger.Launch();
            base.Rollback(savedState);
            //删除防火墙
            NetFwDelApps(80, "TCP");
            // 删除站点,ID安装时存在注册表中，需要通过RegKey去拿
            DeleteWebSiteByID(ReadFromReg(regKeyID));
            // 删除应用程序池，程序池名为网站名写在变量中
            DeleteAppPool(webSiteName);
            // 删除注册表
            DeleteReg(regKeyID);

            // 删除快捷方式
            string dk = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            dk = @dk + "\\" + webSiteDisplayName + ".lnk";
            if (System.IO.File.Exists(dk))
            {
                //如果存在则删除  
                System.IO.File.Delete(dk);
            }

            // 删除启动菜单
            string prodir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.StartMenu);   //得到应用程序菜单文件夹  
            prodir += "\\" + webSiteDesc + "程序菜单";
            if (System.IO.Directory.Exists(prodir))
            {
                System.IO.Directory.Delete(prodir, true);
            }

        }


        /// <summary>
        /// 程序卸载
        /// </summary>
        /// <param name="savedState"></param>
        public override void Uninstall(IDictionary savedState)
        {
            //System.Diagnostics.Debugger.Launch();
            NetFwDelApps(80, "TCP");
            // 删除站点,ID安装时存在注册表中，需要通过RegKey去拿
            DeleteWebSiteByID(ReadFromReg(regKeyID));
            // 删除应用程序池，程序池名为网站名写在变量中
            DeleteAppPool(webSiteName);
            // 删除注册表
            DeleteReg(regKeyID);

            // 删除快捷方式
            string dk = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            dk = @dk + "\\" + webSiteDisplayName + ".lnk";
            if (System.IO.File.Exists(dk))
            {
                //如果存在则删除  
                System.IO.File.Delete(dk);
            }

            // 删除启动菜单
            string prodir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.StartMenu);   //得到应用程序菜单文件夹  
            prodir += "\\" + webSiteDesc + "程序菜单";
            if (System.IO.Directory.Exists(prodir))
            {
                System.IO.Directory.Delete(prodir, true);
            }
            base.Uninstall(savedState);
        }

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

        //private void GetVersionFromRegistry()
        //{
        //    using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
        //        RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
        //    {
        //        foreach (string versionKeyName in ndpKey.GetSubKeyNames())
        //        {
        //            if (versionKeyName.StartsWith("v"))
        //            {
        //                RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
        //                string name = (string)versionKey.GetValue("Version", "");
        //                string sp = versionKey.GetValue("SP", "").ToString();
        //                string install = versionKey.GetValue("Install", "").ToString();
        //                if (install == "") //no install info, ust be later
        //                    Console.WriteLine(versionKeyName + "  " + name);
        //                else
        //                {
        //                    if (sp != "" && install == "1")
        //                    {
        //                        Console.WriteLine(versionKeyName + "  " + name + "  SP" + sp);
        //                    }
        //                }
        //                if (name != "")
        //                {
        //                    continue;
        //                }
        //                foreach (string subKeyName in versionKey.GetSubKeyNames())
        //                {
        //                    RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
        //                    name = (string)subKey.GetValue("Version", "");
        //                    if (name != "")
        //                        sp = subKey.GetValue("SP", "").ToString();
        //                    install = subKey.GetValue("Install", "").ToString();
        //                    if (install == "") //no install info, ust be later
        //                        Console.WriteLine(versionKeyName + "  " + name);
        //                    else
        //                    {
        //                        if (sp != "" && install == "1")
        //                        {
        //                            Console.WriteLine("  " + subKeyName + "  " + name + "  SP" + sp);
        //                        }
        //                        else if (install == "1")
        //                        {
        //                            Console.WriteLine("  " + subKeyName + "  " + name);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}



        /// <summary>
        /// 注册IIS
        /// </summary>
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
            if (!EnsureNewSiteEnavaible(siteInfo.BindString))//存在就移除
            {
                //throw new Exception("网站{0}端口已被占用,请修改后再次启动安装。" + siteInfo.BindString);
                throw new Exception("80端口已被已有站点使用,请修改为其他端口。本程序必须使用80端口。");
                //foreach (DirectoryEntry item in rootEntry.Children)
                //{
                //    PropertyValueCollection serverBindings = item.Properties["ServerBindings"];
                //    if (serverBindings.Contains(siteInfo.BindString))
                //    {
                //        rootEntry.Children.Remove(item);
                //    }
                //}
            }


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

            string ChangWebPath = siteInfo.WebPath.Trim().Remove(siteInfo.WebPath.Trim().LastIndexOf('\\'), 1);
            vdEntry.Properties["Path"].Value = ChangWebPath;

            vdEntry.Invoke("AppCreate", true);//创建应用程序  
            vdEntry.Properties["AccessRead"][0] = true; //设置读取权限  
            vdEntry.Properties["AccessWrite"][0] = true;
            vdEntry.Properties["AccessScript"][0] = true;//执行权限  
            vdEntry.Properties["AccessExecute"][0] = false;
            //string defaultdoc = vdEntry.Properties["DefaultDoc"][0].ToString();//获取默认文档集  
            //vdEntry.Properties["DefaultDoc"][0] = "login.aspx";//设置默认文档  
            vdEntry.Properties["AppFriendlyName"][0] = webSiteName;// "WebManager"; //应用程序名称   
            vdEntry.Properties["AuthFlags"][0] = 1;//0表示不允许匿名访问，1表示可以匿名访问，3为基本身份验证，7为windows继承身份验证  
            vdEntry.CommitChanges();


            #region 针对IIS7
            DirectoryEntry getEntity = new DirectoryEntry("IIS://localhost/W3SVC/INFO");
            int Version = int.Parse(getEntity.Properties["MajorIISVersionNumber"].Value.ToString());
            if (Version > 6)
            {
                #region 创建应用程序池

                string AppPoolName = webSiteName;// 应用程序池名称使用网站名称
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
        /// 判断站点是否存在
        /// </summary>
        /// <param name="BindString">站点信息</param>
        /// <returns>返回true即站点不存在 </returns>
        private bool EnsureNewSiteEnavaible(string BindString)
        {
            bool Isavaible = false;
            DirectoryEntry rootEntry = GetDirectoryEntry(entPath);

            //遍历所有站点  
            foreach (DirectoryEntry item in rootEntry.Children)
            {
                PropertyValueCollection serverBindings = item.Properties["ServerBindings"];
                if (!serverBindings.Contains(BindString))
                {
                    Isavaible = true;
                }
                else
                {
                    return false;   //存在  
                }
            }
            return Isavaible;
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
            webSiteID = siteID.ToString().Trim();
            return webSiteID;
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
            System.IO.Directory.SetAccessControl(physicalDir, fSec);
        }

        /// <summary>
        /// 写入连接字符串至配置文件
        /// </summary>
        private void WriteWebConfig()
        {
            //加载配置文件  
            string path = physicalDir;
            System.IO.FileInfo FileInfo = new System.IO.FileInfo(physicalDir + "web.config");
            if (!FileInfo.Exists)
            {
                throw new InstallException("缺少配置文件 :" + physicalDir + "web.config");
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
            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(@dk + "\\" + webSiteDisplayName + ".lnk");
            shortcut.TargetPath = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE";
            shortcut.Arguments = @"http://" + GetIP() + ":" + port;    //参数，这里就是网站地址  
            shortcut.Description = webSiteDesc + "快捷方式";
            shortcut.IconLocation = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE, 0";//图标，以IE图标作为快捷方式图标  
            shortcut.WindowStyle = 1;
            shortcut.Save();
        }

        /// <summary>
        /// 创建应用程序菜单项
        /// </summary>
        private void CreateProCut()
        {
            string prodir = System.Environment.GetFolderPath(System.Environment.SpecialFolder.StartMenu);   //得到应用程序菜单文件夹路径  
            prodir += "\\" + webSiteDesc + "程序菜单";
            if (!System.IO.Directory.Exists(prodir))
            {
                System.IO.Directory.CreateDirectory(prodir);
            }

            IWshRuntimeLibrary.WshShell shell2 = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut2 = (IWshRuntimeLibrary.IWshShortcut)shell2.CreateShortcut(@prodir + "\\" + webSiteDisplayName + ".lnk");
            shortcut2.TargetPath = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE";
            shortcut2.Arguments = @"http://" + GetIP() + ":" + port;    //参数  
            shortcut2.Save();

            IWshRuntimeLibrary.WshShell shell3 = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut3 = (IWshRuntimeLibrary.IWshShortcut)shell3.CreateShortcut(@prodir + "\\" + webSiteDisplayName + "卸载.lnk");
            shortcut3.Description = webSiteDesc + "卸载程序";
            shortcut3.Arguments = @"/x " + @productCode;    //参数  
            shortcut3.TargetPath = physicalDir + @"InstallFiles\Uninstall.exe";//<span style="WHITE-SPACE: pre">   </span>//卸载程序被重命名为Uninstall.exe  
            shortcut3.Save();


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
            string siteEntPath = String.Format("IIS://{0}/w3svc/{1}", "localhost", siteID);
            DirectoryEntry siteEntry = GetDirectoryEntry(siteEntPath);
            DirectoryEntry rootEntry = GetDirectoryEntry(entPath);
            rootEntry.Children.Remove(siteEntry);
            rootEntry.CommitChanges();
        }

        /// <summary>
        /// 写入注册表
        /// </summary>
        /// <param name="name"></param>
        private void WriteToReg(string name)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.ChangePermissions);
            RegistryKey MywebSite = rk.CreateSubKey(name);
            MywebSite.SetValue(name + "Value", webSiteID, RegistryValueKind.String);
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
            registData = MyWebSite.GetValue(name + "Value").ToString();
            return registData;
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

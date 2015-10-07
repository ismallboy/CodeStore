using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.ETOffine.Installer
{
    public class NewWebSiteInfo
    {
        private string hostIP; // 主机IP  
        private string portNum; // 网站端口号  
        private string descOfWebSite; // 网站表示。一般为网站的网站名。如"www.myweb.com.cn"  
        private string nameOfWebSite;// 网站名称。如"我的网站"，此处即为在IIS管理器中的网站名称  
        private string webPath; // 网站的主目录。例如@"e:\\ mp"  

        public NewWebSiteInfo(string hostIP, string portNum, string descOfWebSite, string nameOfWebSite, string webPath)
        {
            this.hostIP = "";// hostIP;
            this.portNum = portNum;
            this.descOfWebSite = descOfWebSite;
            this.nameOfWebSite = nameOfWebSite;
            this.webPath = webPath;
        }

        public string BindString
        {
            get
            {
                return String.Format("{0}:{1}:{2}", hostIP, portNum, descOfWebSite); //网站标识（IP,端口，主机头值）  
            }
        }

        public string PortNum
        {
            get
            {
                return portNum;
            }
        }

        public string NameOfWebSite
        {
            get
            {
                return nameOfWebSite;
            }
        }

        public string WebPath
        {
            get
            {
                return webPath;
            }
        }
    }
}

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.ETOffine.Utilities
{
    public  class ConfigManage
    {
        public static string GetActivityIDUrl {
            get { return string.Format("{0}OA/MSETCK/Pages/AddActivity.aspx", ConfigurationManager.AppSettings["ServerURL"].ToString()); }
        }
        public static string DownloadUrl
        {
            get { return string.Format("{0}OA/MSETCK/WebService/OffineWebService.asmx", ConfigurationManager.AppSettings["ServerURL"].ToString()); }
        }
    }
}

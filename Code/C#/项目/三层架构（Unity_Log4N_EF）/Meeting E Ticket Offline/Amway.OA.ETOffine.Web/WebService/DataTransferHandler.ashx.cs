using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Utilities;

namespace Amway.OA.ETOffine.Web.WebService
{
    /// <summary>
    /// DataTransferHandler 的摘要说明
    /// </summary>
    public class DataTransferHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var method = context.Request["method"].ToNullString();

            if (method == "CheckHost")
            {
                var recordID = context.Request["recordid"].ToNullString();
                context.Response.Write(CheckHost(recordID));
            }
            else if (method == "FindAllHost")
            {
                context.Response.Write(FindAllHost());
            }
        }
        

        /// <summary>
        /// 判断并返回会议主机IP
        /// </summary>
        /// <param name="recorID"></param>
        /// <returns></returns>
        public string CheckHost(string recorID)
        {
            string checkIP = Utility.GetIP();
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var activityList = activityBO.GetFilteredList(o => o.RecordID == recorID && o.Status == 1 && o.IsHost == 1 && o.HostIp == checkIP)
                .Select(o => new { RecordID = o.RecordID, ActivityName = o.ActivityName, ActivitySn = o.ActivitySn, HostIp = o.HostIp, UpdateHostTime = o.UpdateHostTime }).ToList();
            
            // 找到当前会议，并且当前会议被设置为主机，而且主机IP与当前机器IP相同（以防机器换主机或者换IP了）
            if (activityList.Count > 0)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(activityList); 
            }
            else
            {
                return string.Empty;
            }
        }

        public string FindAllHost()
        {
            string checkIP = Utility.GetIP();
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var activityList = activityBO.GetFilteredList(o => o.HostIp == checkIP && o.IsHost == 1 && o.Status == 1)
                                         .Select(o => new { RecordID = o.RecordID, ActivityName = o.ActivityName, ActivitySn = o.ActivitySn, HostIp = o.HostIp, UpdateHostTime = o.UpdateHostTime }).ToList();
            
            if (activityList != null && activityList.Count > 0)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(activityList); 
            }
            else
            {
                return string.Empty;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
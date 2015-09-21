using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Amway.OA.ETOffine.Entities;
using Amway.OA.ETOffine.BLL;

namespace Amway.OA.ETOffine.Web.WebService
{
    /// <summary>
    /// ForRemotingReWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    // [System.Web.Script.Services.ScriptService]
    [System.Web.Script.Services.ScriptService]
    public class ForRemotingWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// 获取会议
        /// </summary>
        /// <param name="recordID">会议ID</param>
        /// <returns></returns>
        [WebMethod]
        public MsetOfflineActivity GetActivity(string recordID)
        {
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            return activityBO.GetSingle(o => o.RecordID == recordID && o.Status == 1);            
        }

        /// <summary>
        /// 获取会议门票信息
        /// </summary>
        /// <param name="recordID">会议ID</param>
        /// <returns></returns>
        [WebMethod]
        public List<MsetOfflineTicket> GetTicketList(string recordID)
        {
            var ticketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();
            return ticketBO.GetFilteredList(o => o.RecordID == recordID && o.Status == 1);
        }
        
        /// <summary>
        /// 获取会议验票Log
        /// </summary>
        /// <param name="recordID">会议ID</param>
        /// <returns></returns>
        [WebMethod]
        public List<MsetOfflineCheckingLog> GetTicketLogList(string recordID)
        {
            var logBO = ServiceLocator.GetService<IMsetOfflineCheckingLogBO>();
            return logBO.GetFilteredList(o => o.RecordID == recordID && o.Status == 1);
        }
    }
}

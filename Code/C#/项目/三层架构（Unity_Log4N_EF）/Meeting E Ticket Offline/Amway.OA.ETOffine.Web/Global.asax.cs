using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;
using System.Web.Security;
using System.Web.SessionState;
using System.Configuration;
using Amway.OA.ETOffine.Entities;
using Amway.OA.ETOffine.BLL;
using System.Net;
using System.Text;
using Amway.OA.ETOffine.Utilities;
using AutoMapper;

namespace Amway.OA.ETOffine.Web.App_Code
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Timer timer = new Timer(SyncDataInterval * 1000);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += new ElapsedEventHandler(SyncData);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        
        private int SyncDataInterval
        {
            get
            {
                return ConfigurationManager.AppSettings["SyncDataInterval"].ToInt(300);
            }
        }

        /// <summary>
        /// 定时执行的同步数据方法
        /// </summary>
        protected void SyncData(object source, System.Timers.ElapsedEventArgs e)
        {
            //SyncActivity();
            //SyncTicket();
            //SyncCheckinLog();

            // 数据采用备机去同步主机的数据，同步动作由备机发起,全部更新
            var localHost = Utility.GetIP();
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var dateNow = DateTime.Now.AddMinutes(20);
            var dateNowStart = DateTime.Now.AddHours(-2);
            var activityList = activityBO.GetPagedFilteredList(o => o.IsHost == 0 && o.Status == 1 && (o.StartTime < dateNowStart && o.EndTime < dateNow), s => s.SyncDate, false, 0, 5);
            if (activityList.Count > 0)
            {
                foreach (var at in activityList)
                {
                    if (string.IsNullOrWhiteSpace(at.HostIp) || localHost == at.HostIp)
                    {
                        continue;
                    }

                    try
                    {
                        Logger.Debug("开始同步数据。RecordID:{0},ActivitySN:{1},ActivityName:{2},HostIP:{3}.", at.RecordID, at.ActivitySn, at.ActivityName, at.HostIp);
                        var recordID = at.RecordID;
                        using (var service = new OfflineService.ForRemotingWebService())
                        {
                            service.Url = string.Format("http://{0}/WebService/ForRemotingWebService.asmx", at.HostIp);

                            // 获取会议信息
                            var activity = new MsetOfflineActivity();
                            var remoteActivity = service.GetActivity(recordID);
                            if (remoteActivity != null)
                            {
                                Logger.Debug("同步数据,获取活动成功。RecordID:{0},ActivitySN:{1},ActivityName:{2},HostIP:{3}.", at.RecordID, at.ActivitySn, at.ActivityName, at.HostIp);
                       
                                Mapper.DynamicMap(remoteActivity, activity);
                                activity.ID = at.ID;
                                activity.IsHost = 0;
                                activity.SyncDate = DateTime.Now;
                                activity.SyncStatus = 1;
                                activity.UpdateHostTime = DateTime.Now;
                                //activityBO.ExcuteSQL(string.Format("DELETE FROM TSTB_MSET_OFFLINE_ACTIVITY WHERE RECORD_ID='{0}'", recordID));
                                activityBO.Update(activity);
                            }

                            // 获取会议门票信息
                            var ticketList = service.GetTicketList(recordID);
                            var ticketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();
                            ticketBO.ExcuteSQL(string.Format("DELETE FROM TSTB_MSET_OFFLINE_TICKET WHERE RECORD_ID='{0}'", recordID));
                            if (ticketList != null && ticketList.Length > 0)
                            {
                                Logger.Debug("同步数据,获取门票成功。RecordID:{0},ActivitySN:{1},ActivityName:{2},HostIP:{3},RecordCount:{4}.", at.RecordID, at.ActivitySn, at.ActivityName, at.HostIp, ticketList.Length);
                       
                                foreach (var item in ticketList)
                                {
                                    var ticket = new MsetOfflineTicket();
                                    Mapper.DynamicMap(item, ticket);
                                    ticket.ID = null;
                                    ticket.SyncDate = DateTime.Now;
                                    ticket.SyncStatus = 1;
                                    ticketBO.Add(ticket);
                                }
                            }

                            // 获取验票Log信息
                            var ticketLogList = service.GetTicketLogList(recordID);
                            var ticketLogBO = ServiceLocator.GetService<IMsetOfflineCheckingLogBO>();
                            ticketLogBO.ExcuteSQL(string.Format("DELETE FROM TSTB_MSET_OFFLINE_CHECKING_LOG WHERE RECORD_ID='{0}'", recordID));
                            if (ticketLogList != null && ticketLogList.Length > 0)
                            {
                                Logger.Debug("同步数据,获取门票Log成功。RecordID:{0},ActivitySN:{1},ActivityName:{2},HostIP:{3},RecordCount:{4}.", at.RecordID, at.ActivitySn, at.ActivityName, at.HostIp, ticketLogList.Length);
                       
                                foreach (var item in ticketLogList)
                                {
                                    var ticketLog = new MsetOfflineCheckingLog();
                                    Mapper.DynamicMap(item, ticketLog);
                                    ticketLog.ID = null;
                                    ticketLog.SyncDate = DateTime.Now;
                                    ticketLog.SyncStatus = 1;
                                    ticketLogBO.Add(ticketLog);
                                }
                            }
                        }
                        Logger.Debug("结束同步数据。RecordID:{0},ActivitySN:{1},ActivityName:{2},HostIP:{3}.", at.RecordID, at.ActivitySn, at.ActivityName, at.HostIp);
                       
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("数据同步错误.ActivitySN:{0},Error:{1};{2}", at.ActivitySn, ex.Message, ex.StackTrace);
                    }
                }
            }
        }
    }
}
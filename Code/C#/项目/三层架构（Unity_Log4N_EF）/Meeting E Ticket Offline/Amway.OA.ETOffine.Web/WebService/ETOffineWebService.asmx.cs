using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Transactions;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Entities;
using System.Configuration;
using Amway.OA.ETOffine.Utilities;

namespace Amway.OA.ETOffine.Web.WebService
{
    /// <summary>
    /// ETOffineWebService提供包括自动完成textbox的搜索接口和AgileWork回调服务
    /// 使用时可以通过 系统管理-》应用设置-》修改ETOffine应用
    /// 勾选是否需要回调WebService-》然后下方填入
    /// http://localhost/OA/ETOffine/WebService/ETOffineWebService.asmx
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class ETOffineWebService : System.Web.Services.WebService
    {


        [WebMethod]
        public ReturnValue CheckTicket(string activityID, string ticketNumber)
        {
            ReturnValue rv = new ReturnValue();
            try
            {
                Logger.Debug("验票开始");
                string checkIP = HttpContext.Current.Request.UserHostAddress;

                if (!string.IsNullOrWhiteSpace(checkIP) && checkIP.Length > 9 && checkIP.Substring(0, 8) != "127.0.0." && !Utility.IsConnectionNet())
                {
                    rv.Result = 2;
                    rv.Message = "本机网络已断开，请检查。";
                    return rv;
                }

                System.Net.IPAddress ip = System.Net.IPAddress.Parse(checkIP);
                //System.Net.IPHostEntry ine = System.Net.Dns.GetHostEntry(ip);
                //string machineName = ine.HostName;
                string machineName = HttpContext.Current.Request.UserHostName;
                rv = CheckTicket(activityID, ticketNumber, checkIP, machineName);
                rv.Message = ticketNumber + " " + rv.Message;
            }
            catch (Exception ex)
            {
                Logger.Error("验票失败；详细：" + ex.Message + ex.StackTrace);
                rv.Result = 0;
                rv.Message = "异常，验票失败";
            }
            return rv;
        }

        #region 数据同步函数
        /// <summary>
        /// 每次同步的记录数，由webconfig配置
        /// </summary>
        private int SyncQuantity
        {
            get { return ConfigurationManager.AppSettings["SyncQuantityForOneTime"].ToInt(30); }
        }

        /// <summary>
        /// 获取更新的活动表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<MsetOfflineActivity> SyncETOfflineActivity()
        {
            List<MsetOfflineActivity> activityList = new List<MsetOfflineActivity>();
            var etoffineActivityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            try
            {
                activityList = etoffineActivityBO.GetPagedFilteredList(o => o.SyncStatus == (int)SyncStatus.UnSync, 0, SyncQuantity);
                
                if (activityList.Count > 0)
                {
                    activityList.ForEach(o =>
                    {
                        o.SyncStatus = (int)SyncStatus.Synced;
                        o.SyncDate = DateTime.Now;
                        etoffineActivityBO.Update(o);
                    });
                }
                return activityList;
            }
            catch (Exception e)
            {
                Logger.Error("同步活动表数据失败" + e.Message);
                throw new Exception("同步活动表数据失败" + e.Message + ";详情：" + e.StackTrace);
            }
        }

        /// <summary>
        /// 获取更新的电子验票日志表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<MsetOfflineCheckingLog> SyncETOfflineCheckingLog()
        {
            List<MsetOfflineCheckingLog> checkLogList = new List<MsetOfflineCheckingLog>();
            var etoffineCheckingLogBO = ServiceLocator.GetService<IMsetOfflineCheckingLogBO>();
            try
            {
                checkLogList = etoffineCheckingLogBO.GetPagedFilteredList(o => o.SyncStatus == (int)SyncStatus.UnSync, 0, SyncQuantity);
               
                if (checkLogList.Count > 0)
                {
                    checkLogList.ForEach(o =>
                    {
                        o.SyncStatus = (int)SyncStatus.Synced;
                        etoffineCheckingLogBO.Update(o);
                    });
                }
                return checkLogList;
            }
            catch (Exception e)
            {
                Logger.Error("同步验票日志表数据失败" + e.Message);
                throw new Exception("同步验票日志表数据失败" + e.Message + ";详情：" + e.StackTrace);
            }
        }


        /// <summary>
        /// 获取更新的门票表
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public List<MsetOfflineTicket> SyncETOfflineTicket()
        {
            List<MsetOfflineTicket> checkLogList = new List<MsetOfflineTicket>();
            var etofflineTicketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();
            try
            {
                checkLogList = etofflineTicketBO.GetPagedFilteredList(o => o.SyncStatus == (int)SyncStatus.UnSync, 0, SyncQuantity);

                if (checkLogList.Count > 0)
                {
                    checkLogList.ForEach(o =>
                    {
                        o.SyncStatus = (int)SyncStatus.Synced;
                        etofflineTicketBO.Update(o);
                    });
                }
                return checkLogList;
            }
            catch (Exception e)
            {
                Logger.Error("同步门票表数据失败" + e.Message);
                throw new Exception("同步门票表数据失败" + e.Message + ";详情：" + e.StackTrace);
            }
        }
        #endregion


        /// <summary>
        /// 添加到验票日志
        /// </summary>
        /// <param name="activityID"></param>
        /// <param name="ticketNumber"></param>
        /// <param name="checkIP"></param>
        /// <param name="checkMachine"></param>
        /// <param name="status"></param>
        /// <param name="adaCard"></param>
        private void AddCheckingLog(string activityID, string ticketNumber, string checkIP, string checkMachine, CheckingStatus status, string adaCard)
        {
            IMsetOfflineCheckingLogBO ckLogDAO = ServiceLocator.GetService<IMsetOfflineCheckingLogBO>();
            MsetOfflineCheckingLog ckLog = new MsetOfflineCheckingLog
            {
                RecordID = activityID,
                TicketSn = ticketNumber,
                CheckinIp = checkIP,
                MachineName = checkMachine,
                CheckinTime = DateTime.Now,
                AdaCard = adaCard,
                CheckingStatus = (int)status,
                Status = 1,
                SyncDate=DateTime.Now,
                SyncStatus=(int)SyncStatus.UnSync
       
            };
            ckLogDAO.Add(ckLog);
        }
        /// <summary>
        /// 验证电子票
        /// </summary>
        /// <param name="activityID">活动ID</param>
        /// <param name="ticketNumber">电子票号或者安利卡号</param>
        /// <param name="checkIp">验证机器IP</param>
        /// <param name="checkMachine">验证机器名字</param>
        /// <returns></returns>
        private ReturnValue CheckTicket(string activityID, string ticketNumber, string checkIp, string checkMachine)
        {
            if (ticketNumber == string.Empty)
            {
                return new ReturnValue(0, "门票号不能为空！");
            }

            ReturnValue rValue = new ReturnValue();
            var ticketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();

            //using (TransactionScope ts = new TransactionScope())
            //{
            var activity = activityBO.GetSingle(o => o.RecordID == activityID.Trim() && o.Status == 1);
            if (activity != null)
            {
                if (activity.IsRealName == (int)IsRealName.N)//普通验票
                {
                    var ticket = ticketBO.GetFilteredList(o => o.TicketSn == ticketNumber.Trim() && (o.TicketStatus == (int)TicketStatus.UnChecked || o.TicketStatus == (int)TicketStatus.Checked) && o.RecordID == activityID.Trim() && o.Status == 1).FirstOrDefault();
                    if (ticket != null)
                    {
                        if (ticket.TicketStatus == (int)TicketStatus.UnChecked)
                        {
                            ticket.TicketStatus = (int)TicketStatus.Checked;
                            ticket.CheckinTime = DateTime.Now;
                            ticket.CheckinIp = checkIp;
                            ticket.MachineName = checkMachine;
                            ticket.SyncDate = DateTime.Now;
                            ticket.SyncStatus = (int)SyncStatus.UnSync;
                            ticketBO.Update(ticket);
                            AddCheckingLog(activityID, ticketNumber, checkIp, checkMachine, CheckingStatus.Normal, "");
                            rValue.Result = 1;
                            rValue.Message = "正常";

                            //更新活动为未上传
                            activity.CheckingStatus =(int) ActivityCheckingStatus.UnUpload;
                            activityBO.Update(activity);
                        }
                        else
                        {
                            AddCheckingLog(activityID, ticketNumber, checkIp, checkMachine, CheckingStatus.Used, "");
                            rValue.Message = "异常，此票已使用<br/><span>验票时间：{0}<br/>持票人识别码：{1}</span>".FormatWith(ticket.CheckinTime, ticket.AdaCard);
                        }
                    }
                    else
                    {
                        AddCheckingLog(activityID, ticketNumber, checkIp, checkMachine, CheckingStatus.None, "");
                        rValue.Message = "异常-无此票";
                    }

                }
                if (activity.IsRealName == (int)IsRealName.Y)// 实名验票
                {
                    if (ticketNumber.Trim().Length > 50)
                    {
                        ticketNumber = GetAdaCardFromECard(ticketNumber.Trim());
                    }

                    var tickets = ticketBO.GetFilteredList(o => o.AdaCard == ticketNumber.Trim() && o.RecordID == activityID.Trim() && (o.TicketStatus == (int)TicketStatus.UnChecked || o.TicketStatus == (int)TicketStatus.Checked) && o.Status == 1);
                    if (tickets.Count > 0)
                    {
                        if (tickets.Select(o=>o.TicketStatus).Contains((int)TicketStatus.Checked))
                        {
                            AddCheckingLog(activityID, "", checkIp, checkMachine, CheckingStatus.Used, ticketNumber);
                            rValue.Message = "异常，此票已使用<br/><span>验票时间：{0}<br/>持票人识别码：{1}</span>".FormatWith(tickets[0].CheckinTime, tickets[0].AdaCard);
                        }
                        else
                        {
                            tickets.ForEach(o =>
                            {
                                o.TicketStatus = (int)TicketStatus.Checked;
                                o.CheckinTime = DateTime.Now;
                                o.CheckinIp = checkIp;
                                o.MachineName = checkMachine;
                                o.SyncDate = DateTime.Now;
                                o.SyncStatus = (int)SyncStatus.UnSync;
                                ticketBO.Update(o);
                                AddCheckingLog(activityID, o.TicketSn, checkIp, checkMachine, CheckingStatus.Normal, o.AdaCard);
                                rValue.Message += o.OwnerName + ",";
                            });
                            rValue.Result = 1;
                            rValue.Message =rValue.Message.TrimEnd(',')+ "<br>正常";
                        }
                        

                        //更新活动为未上传
                        activity.CheckingStatus = (int)ActivityCheckingStatus.UnUpload;
                        activityBO.Update(activity);
                    }
                    else
                    {
                        rValue.Message = "异常-无对应的票";
                        AddCheckingLog(activityID, "", checkIp, checkMachine, CheckingStatus.None, ticketNumber);
                    }
                }
                if (activity.IsRealName == null)
                {
                    throw new Exception("活动状态异常！");
                }
                //}
                //ts.Complete();
            }
            return rValue;
        }

        public string GetAdaCardFromECard(string eCard)
        {
            /*             
                授权卡确定下来的二维码规则如下：
                授权标识位+悦享分标识位+授权人区号+授权人安利卡号+被授权人区号+被授权人安利卡号+ID+到期时间（YYYYMMDDHHMMSS）
                Ø  到期时间：按二维码生成时间加60分钟，格式为YYYYMMDDHHMMSS，其中生成时间按服务器时间。
                Ø  授权标识位：1位，0表示本人安利卡，1表示授权卡
                Ø  悦享分标识位：1位，本人安利卡以0表示，1表示授权不允许兑换悦享分，2表示授权允许兑换悦享分；
                Ø  授权人区号：3位，不满3位前面补零；
                Ø  授权人安利卡号：；11位，不满11位前面补零；
                Ø  被授权人区号（本人二维码以全“0”表示）：3位，不满3位前面补零；
                Ø  被授权人安利卡号（本人二维码以全“0”表示）：11位，不满11位前面补零；
                Ø  ID（本人二维码以 全“0”表示）：18位，授权记录在数据库所对应的ID。
 
                例子数据：授权人卡号12345，被授权人卡号67890，
                例子：
                卡号为12345的本人卡：OO360000000123450000000000000000000000000000000020150516121212
                他人授权卡（允许兑换悦享分）：12360000000123453600000006789000000000000000000220150516121212
             */
            var reVal = string.Empty;
            if (eCard.Length >= 62)
            {
                if (eCard.Substring(0, 1) == "0")
                {
                    //if (eCard.Substring(2, 3) == "360")
                    //{

                    //}
                    //else
                    //{
                    //    // 非大陆卡
                    // Logger.Warning(eCard + "非大陆卡");
                    //}
                    reVal = eCard.Substring(5, 11).TrimStart('0');
                }
                else
                {
                    // 非本人安利卡
                    Logger.Warning(eCard + "非本人安利卡");
                }
            }
            else
            {
                // 卡长度不正确
                Logger.Warning(eCard + "卡长度不正确");
            }

            return reVal;
        }
    }
}

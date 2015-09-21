using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Entities;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class MonitorCheck : ETOffineBasePage
    {
        public string RecordID { get { return Request.QueryString["RID"].ToNullString(); } }
        /// <summary>
        /// 设置显示LOG的数量
        /// </summary>
        public int LogQuantity
        {
            get
            {
                return ConfigurationManager.AppSettings["LogQuantity"].ToInt(20);
            }
        }

        /// <summary>
        /// 每多长时间刷新页面
        /// </summary>
        public int CheckPageRefreshTime
        {
            get
            {
                return ConfigurationManager.AppSettings["CheckPageRefreshTime"].ToInt(30000);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
                reLoadTime.Value = CheckPageRefreshTime.ToString();

            }
        }

        /// <summary>
        /// 获取星期名称
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        public string GetWeekName(DayOfWeek week)
        {
            switch (week)
            {
                case DayOfWeek.Sunday:
                    return "星期日";
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Saturday:
                    return "星期六";
            }
            return string.Empty;
        }

        private void InitData()
        {
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var activity = activityBO.GetFilteredList(o => o.RecordID == RecordID && o.Status == 1).FirstOrDefault();
            if (activity != null)
            {
                var checkLogBO = ServiceLocator.GetService<IMsetOfflineCheckingLogBO>();
                var ticketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();

                var ticketList = ticketBO.GetFilteredList(o => o.RecordID == RecordID && o.Status == 1);
                var ticketSum = ticketList.Count;
                var ticketLogList = checkLogBO.GetFilteredList(o => o.RecordID == RecordID && o.Status == 1);
                var okTicket = ticketLogList.Count(o => o.CheckingStatus == (int)CheckingStatus.Normal);

                double entranceRate = Math.Round((double)okTicket / (double)ticketSum * 100, 2);

                ltrActivitySN.Text = activity.ActivitySn;
                ltrTicketCount.Text = ticketSum.ToNullString();
                ltrEntranceCount.Text = okTicket.ToNullString();
                ltrEntrancePercent.Text = string.Format("{0}%", entranceRate.ToNullString());
                ltrError.Text = (ticketLogList.Count() - okTicket).ToNullString();

                var msetckLogGroupByMachine = (from a in ticketLogList
                                               group a by a.MachineName into newGroup
                                               select new
                                               {
                                                   CheckIP = newGroup.Max(o => o.CheckinIp),
                                                   MachineName = newGroup.Key,
                                                   TotalOK = newGroup.Sum(o => o.CheckingStatus == (int)CheckingStatus.Normal ? 1 : 0),
                                                   TotalErr = newGroup.Sum(o => o.CheckingStatus != (int)CheckingStatus.Normal ? 1 : 0),
                                                   NewCheckTime = Convert.ToDateTime(newGroup.Max(o => o.CheckinTime)).ToString("HH:mm:ss")
                                               }).ToList();
                rptIpChecked.DataSource = msetckLogGroupByMachine;
                rptIpChecked.DataBind();

                // 绑定最新的验票记录
                StringBuilder htmlStr = new StringBuilder();
                var listLogView = ticketLogList.OrderByDescending(o => o.CheckinTime).Take(LogQuantity);
                var ticketSNList = listLogView.Select(o => o.TicketSn).ToList();
                foreach (var item in listLogView)
                {
                    var ticket = ticketList.Where(o => (o.TicketSn == item.TicketSn || o.AdaCard == item.AdaCard) && o.Status == 1).FirstOrDefault();
                    htmlStr.Append("<p>");
                    htmlStr.Append(item.CheckinIp);
                    htmlStr.Append("&nbsp;时间：");
                    htmlStr.Append(Convert.ToDateTime(item.CheckinTime).ToString("HH:mm:ss"));
                    if (item.AdaCard != string.Empty)
                    {
                        htmlStr.Append("&nbsp;安利卡号：");
                        htmlStr.Append(item.AdaCard);
                    }
                    if (item.TicketSn != string.Empty)
                    {
                        htmlStr.Append("&nbsp;电子门票号：");
                        htmlStr.Append(item.TicketSn);
                    }
                    switch (item.CheckingStatus)
                    {
                        case (int)CheckingStatus.Normal:
                            htmlStr.Append("&nbsp;&nbsp;正常&nbsp;");
                            break;
                        case (int)CheckingStatus.Used:
                            htmlStr.Append("&nbsp;&nbsp;异常-此票已使用 &nbsp;");

                            if (ticket != null)
                            {
                                htmlStr.AppendFormat("验票时间：{0}&nbsp;持票人标识码：{1}", ticket.CheckinTime.ToString(), ticket.AdaCard.ToString());
                            }
                            break;
                        default: htmlStr.Append("&nbsp;&nbsp;异常-无此票 &nbsp;");
                            break;
                    }

                    htmlStr.Append("</p>");
                }
                ltrLog.Text = htmlStr.ToString();
            }
        }
    }
}
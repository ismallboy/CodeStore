using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Entities;
using Amway.OA.ETOffine.Web.MSETOfflineService;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class UpLoadData : ETOffineBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();

            Expression<Func<MsetOfflineActivity, bool>> filter = null;

            if (!string.IsNullOrWhiteSpace(txtKey.Text))
            {
                var strKey = txtKey.Text.Trim();
                filter = o => (o.ActivitySn.Contains(strKey) || o.ActivityName.Contains(strKey)) && o.Status == 1;
            }
            else
            {
                filter = o => o.Status == 1;
            }

            var activityList = activityBO.GetPagedFilteredList(filter, o => o.StartTime, false, 0, 20);
            var space = Server.HtmlDecode("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            var activiyDTOList = activityList.Select(o => new KeyValuePair<string, string>(o.RecordID, Server.HtmlDecode(o.ActivitySn.CPadLeft(12) + o.ActivityName.CPadLeft(40) + GetActivityStatus(o.CheckingStatus.Value))));

            lstAcivityList.DataSource = activiyDTOList;
            lstAcivityList.DataTextField = "Value";
            lstAcivityList.DataValueField = "Key";
            lstAcivityList.DataBind();
        }

        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void ltnDel_Click(object sender, EventArgs e)
        {
            try
            {
                var recordID = lstAcivityList.SelectedValue;
                var etoActivityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
                var etoTicketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();
                var activity = etoActivityBO.GetSingle(o => o.RecordID == recordID && o.Status == 1);
                if (activity != null)
                {
                    activity.Status = 9;
                    activity.SyncDate = DateTime.Now;
                    activity.SyncStatus = (int)SyncStatus.UnSync;
                    etoActivityBO.Update(activity);
                }
                var ticketList = etoTicketBO.GetFilteredList(o => o.RecordID == recordID && o.Status == 1);
                ticketList.ForEach(o => { o.Status = 9; etoTicketBO.Update(o); });
                BindData();
                ShowMessage("移除成功");
            }
            catch (Exception ex)
            {
                Logger.Error("移除活动失败！\n" + ex.Message + "\n" + ex.StackTrace);
                ShowMessage("移除活动失败！");
            }
        }

        protected void ltnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                var recordID = lstAcivityList.SelectedValue;
                var ticketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();
                var ticketList = ticketBO.GetFilteredList(o => o.RecordID == recordID && o.Status == 1 && o.TicketStatus == (int)TicketStatus.Checked);
                if (ticketList.Count==0)
                {
                    ShowMessage("无门票信息，无须再次上传！");
                    return;
                }
                using (var serviceBO = new MSETOfflineService.OffineWebService())
                {
                    var serverURL =Utilities.ConfigManage.DownloadUrl;
                    serviceBO.Url = serverURL;

                    MSETOfflineService.WsSecurityHeader header = new MSETOfflineService.WsSecurityHeader();
                    header.AppId = WebConfigurationManager.AppSettings["AppId"].ToNullString();
                    header.AppKey = WebConfigurationManager.AppSettings["AppKey"].ToNullString();
                    serviceBO.WsSecurityHeaderValue = header;

                    var updateList = ticketList.Select(o => new MSETOfflineService.ETOffineTicketDTO
                    {
                        AdaCard = o.AdaCard,
                        Category = o.Category,
                        CheckingTime = o.CheckinTime,
                        CheckinIP = o.CheckinIp,
                        MachineName = o.MachineName,
                        Owner_Name = o.OwnerName,
                        RecordID = o.RecordID,
                        TicketID = o.TicketID,
                        TicketSN = o.TicketID,
                        TicketStatus = o.TicketStatus.Value
                    });
                    if (serviceBO.UploadActivity(recordID, updateList.ToArray()))
                    {
                        var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
                        var activity = activityBO.GetSingle(o=>o.RecordID == recordID && o.Status == 1);
                        if (activity != null)
                        {
                            activity.CheckingStatus = (int)ActivityCheckingStatus.HadUpload;
                            activity.SyncStatus = (int)SyncStatus.UnSync;
                            activity.SyncDate = DateTime.Now;
                            activityBO.Update(activity);
                        }
                    }
                }
                ShowMessage("上传验票数据成功！");
                BindData();
            }
            catch (Exception ex)
            {
                Logger.Error("上传验票数据失败！\n" + ex.Message + "\n" + ex.StackTrace);
                ShowMessage("上传验票数据失败！");
            }
        }
    }
}
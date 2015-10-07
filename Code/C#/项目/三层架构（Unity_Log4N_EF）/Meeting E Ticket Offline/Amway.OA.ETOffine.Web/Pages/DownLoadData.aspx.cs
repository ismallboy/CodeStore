using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.SqlClient;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Transactions;
using Amway.OA.ETOffine.Web;
using Amway.OA.ETOffine.Entities;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Utilities;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class DownLoadData : ETOffineBasePage
    {
        private string RecordID
        {
            get { return Request.QueryString["rid"].ToNullString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (RecordID != string.Empty)
                {
                    DownloadActivityInfo(RecordID);//第一次下载活动和门票信息
                }
                LoadData();
            }
        }

        private void LoadData()
        {
            var etoActivityDAO = ServiceLocator.GetService<IMsetOfflineActivityBO>();

            Expression<Func<MsetOfflineActivity, bool>> filter = o => o.Status == 1;
            var key =txtKey.Text.Trim();
            if (!string.IsNullOrWhiteSpace(key))
            {
                filter = o => o.Status == 1 && (o.ActivitySn.Contains(key) ||o.ActivityName.Contains(key));
            }
            var listActivity = etoActivityDAO.GetPagedFilteredList(filter, o => o.StartTime, false, 0, 20);
            var listBind = listActivity.Select(o => new KeyValuePair<string, string>(o.RecordID, Server.HtmlDecode(o.ActivitySn.CPadLeft(12) + o.ActivityName.CPadLeft(40) + GetActivityStatus(o.CheckingStatus.Value))));
            lstAcivityList.DataSource = listBind;
            lstAcivityList.DataValueField = "Key";
            lstAcivityList.DataTextField = "Value";
            lstAcivityList.DataBind();
        }

        private void DownloadActivityInfo(string rid)
        {
            try
            {
                using (var serviceBO = new MSETOfflineService.OffineWebService())
                {
                    var serverURL = ConfigManage.DownloadUrl;
                    serviceBO.Url = serverURL;

                    MSETOfflineService.WsSecurityHeader header = new MSETOfflineService.WsSecurityHeader();
                    header.AppId = WebConfigurationManager.AppSettings["AppId"].ToNullString();
                    header.AppKey = WebConfigurationManager.AppSettings["AppKey"].ToNullString();
                    serviceBO.WsSecurityHeaderValue = header;
                    var InfoDTO = serviceBO.DownLoadActivity(rid);

                    if (InfoDTO == null)
                    {
                        ShowMessage("添加失败");
                    }
                    else
                    {
                        var etoActivityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
                        var etoTicketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();

                        var activity = etoActivityBO.GetSingle(o => o.RecordID == rid && o.Status == 1);
                        if (activity != null)
                        {
                            etoActivityBO.Delete(activity);
                        }
                        var ticketList = etoTicketBO.GetFilteredList(o => o.RecordID == rid && o.Status == 1);
                        ticketList.ForEach(o => etoTicketBO.Delete(o));

                        etoActivityBO.Add(new MsetOfflineActivity
                        {
                            RecordID = InfoDTO.RecordID,
                            ActivitySn = InfoDTO.ActivitySN,
                            ActivityName = InfoDTO.ActivityName,
                            IsRealName = InfoDTO.IsRealName,
                            CheckingStatus = (int)ActivityCheckingStatus.Download,
                            TicketCount = InfoDTO.TicketCount,
                            ActivityPeopleCount = InfoDTO.ActivityCount,
                            ActivityAddr = InfoDTO.ActivityAddress,
                            ActivityCity = InfoDTO.ActivityCityName,
                            StartTime = InfoDTO.StartTime,
                            EndTime = InfoDTO.FinishTime,
                            ActivityCategory = InfoDTO.ActivityCategory,
                            ActivityDetail = InfoDTO.ActivityDetail,
                            ActivitySubDetail = InfoDTO.ActivityChildDetail,
                            Creator = InfoDTO.Creator,
                            CreateDate = InfoDTO.CreateDate,
                            UpdateDate = InfoDTO.UpdateDate,
                            Updator = InfoDTO.Updator,
                            Status = 1,
                            SyncDate = DateTime.Now,
                            SyncStatus = (int)SyncStatus.UnSync
                        });
                        InfoDTO.Items.Select(o => new MsetOfflineTicket
                        {
                            RecordID = o.RecordID,
                            TicketID = o.TicketID,
                            TicketSn = o.TicketSN,
                            Category = o.Category,
                            TicketStatus = o.TicketStatus,
                            CheckinTime = o.CheckingTime,
                            AdaCard = o.AdaCard,
                            OwnerName = o.Owner_Name,
                            ImportTime = DateTime.Now,
                            Status = 1,
                            SyncDate = DateTime.Now,
                            SyncStatus = (int)SyncStatus.UnSync
                        }).ToList().ForEach(o => etoTicketBO.Add(o));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("下载活动失败！\n" + ex.Message + "\n" + ex.StackTrace);
                ShowMessage("下载活动失败！");
            }
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
                LoadData();
                ShowMessage("移除成功");
            }
            catch (Exception ex)
            {
                Logger.Error("移除活动失败！\n" + ex.Message + "\n" + ex.StackTrace);
                ShowMessage("移除活动失败！");
            }

        }

        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// 更新活动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtUpdate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(lstAcivityList.SelectedValue))
            {
                DownloadActivityInfo(lstAcivityList.SelectedValue);
                LoadData();
                ShowMessage("更新活动成功！");
            }
        }

    }
}
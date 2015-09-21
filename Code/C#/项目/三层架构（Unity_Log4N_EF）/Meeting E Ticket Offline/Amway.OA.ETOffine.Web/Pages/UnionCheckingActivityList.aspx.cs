using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Entities;
using System.Linq.Expressions;
using Amway.OA.ETOffine.Utilities;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class UnionCheckingActivityList : ETOffineBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData(bool isSearch = false)
        {
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();

            Expression<Func<MsetOfflineActivity, bool>> filter = null;

            if (isSearch == true && !string.IsNullOrWhiteSpace(txtKey.Text))
            {
                var strKey = txtKey.Text.Trim();
                filter = o => (o.ActivitySn.Contains(strKey) || o.ActivityName.Contains(strKey)) && o.Status == 1;
            }
            else
            {
                filter = o => o.Status == 1;
            }

            var activityList = activityBO.GetPagedFilteredList(filter, o => o.StartTime, false, 0, 20);
            var activiyDTOList = activityList.Select(o => new KeyValuePair<string, string>(o.RecordID, Server.HtmlDecode(o.ActivitySn.CPadLeft(12) + o.ActivityName.CPadLeft(40) + GetActivityStatus(o.CheckingStatus.Value))));

            lstAcivityList.DataSource = activiyDTOList;
            lstAcivityList.DataTextField = "Value";
            lstAcivityList.DataValueField = "Key";
            lstAcivityList.DataBind();
        }

        protected void lbtDel_Click(object sender, EventArgs e)
        {
            var recordID = lstAcivityList.SelectedValue;
            if (string.IsNullOrWhiteSpace(recordID))
            {
                ShowMessage("请选择一个活动");
                return;
            }

            var ticketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();
            var ticketList = ticketBO.GetFilteredList(o => o.RecordID == recordID);
            if (ticketList != null && ticketList.Count > 0)
            {
                foreach (var item in ticketList)
                {
                    ticketBO.Delete(item);
                }
            }

            var ticketLogBO = ServiceLocator.GetService<IMsetOfflineCheckingLogBO>();
            var ticketLogList = ticketLogBO.GetFilteredList(o => o.RecordID == recordID);
            if (ticketLogList != null && ticketLogList.Count > 0)
            {
                foreach (var item in ticketLogList)
                {
                    ticketLogBO.Delete(item);
                }
            }

            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var activityList = activityBO.GetFilteredList(o => o.RecordID == recordID);
            if (activityList != null && activityList.Count > 0)
            {
                foreach (var item in activityList)
                {
                    activityBO.Delete(item);
                }
            }
            ShowMessage("移除成功");
            BindData();

        }

        protected void lbtSearch_Click(object sender, EventArgs e)
        {
            BindData(true);
        }
    }
}
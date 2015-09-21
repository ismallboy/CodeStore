using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Entities;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class CheckTicketData : ETOffineBasePage
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
            var activiyDTOList = activityList.Select(o => new KeyValuePair<string, string>(o.RecordID, Server.HtmlDecode(o.ActivitySn.CPadLeft(12) + o.ActivityName.CPadLeft(20) + GetActivityStatus(o.CheckingStatus.Value))));

            lstAcivityList.DataSource = activiyDTOList;
            lstAcivityList.DataTextField = "Value";
            lstAcivityList.DataValueField = "Key";
            lstAcivityList.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData(true);
        }
    }
}
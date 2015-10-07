using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.BLL;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class ActivityInfo : ETOffineBasePage
    {
        public string RID { get { return Request["RID"] == null ? string.Empty : Request["RID"].ToString(); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            BindData();
        }

        private void BindData()
        {
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var activity = activityBO.GetSingle(o => o.RecordID == RID && o.Status == 1);

            if (activity != null)
            {
                lblActivitySN.Text = activity.ActivitySn;
                lblActivityName.Text = activity.ActivityName;
                lblActivityCity.Text = activity.ActivityCity;
                lblActivityAddress.Text = activity.ActivityAddr;
                lblStartTime.Text = activity.StartTime.Value.ToString("yyyy-MM-dd HH:mm");
                lblActivityFinishTime.Text = activity.EndTime.Value.ToString("yyyy-MM-dd HH:mm");
                lblActivityCategory.Text = activity.ActivityCategory;
                lblActivityDetail.Text = activity.ActivityDetail;
                lblActivityChildDetail.Text = activity.ActivitySubDetail;
                lblActivityCount.Text = activity.ActivityPeopleCount.ToNullString();
            }
            else
            {
                ShowMessage("活动不存在");
            }
        }
    }
}
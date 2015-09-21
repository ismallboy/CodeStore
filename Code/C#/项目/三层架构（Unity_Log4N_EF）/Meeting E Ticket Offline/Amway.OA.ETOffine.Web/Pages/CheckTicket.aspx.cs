using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.Web;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Entities;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class CheckTicket : ETOffineBasePage
    {
        public string RecordID { get { return Request.QueryString["RID"].ToNullString(); } }
        public int Single { get { return Request.QueryString["SINGLE"].ToInt(0); } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadInit();
            }
        }

        /// <summary>
        /// 初始化加载数据
        /// </summary>
        private void LoadInit()
        {
            var ActivityInfoBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var msEntity = ActivityInfoBO.GetSingle(o => o.RecordID == this.RecordID && o.Status == 1);
           
            if (msEntity != null)
            {
                lbActivitySN.Text = msEntity.ActivitySn;
                ltlActivityName.Text = msEntity.ActivityName.ToNullString();
                ltlTitle.Text = msEntity.IsRealName == (int)IsRealName.Y ? "安利卡号：" : "电子门票号：";
                ltlCheckTitle.Text = Single == 1 ? "电子门票单机验票" : "电子门票多机验票";
            }
            else
            {
                ShowMessage("不存在该活动");
                return;
            }
        }
    }
}
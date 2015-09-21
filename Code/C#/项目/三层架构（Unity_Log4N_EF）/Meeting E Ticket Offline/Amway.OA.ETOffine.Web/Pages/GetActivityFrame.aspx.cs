using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.Utilities;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class GetActivityFrame : ETOffineBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            hidQueryUrl.Value = ConfigManage.GetActivityIDUrl;
        }
    }
}
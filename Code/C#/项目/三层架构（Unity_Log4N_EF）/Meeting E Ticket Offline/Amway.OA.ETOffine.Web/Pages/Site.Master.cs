using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> javascriptSet = new List<string>();
            javascriptSet.Add("<script src=\"{0}\" type=\"text/javascript\"></script>".FormatWith(ResolveUrl("~/Scripts/jquery-1.7.1.min.js")));
            javascriptSet.Add("<script src=\"{0}\" type=\"text/javascript\"></script>".FormatWith(ResolveUrl("~/Scripts/json2.js")));
            javascriptSet.Add("<script src=\"{0}\" type=\"text/javascript\"></script>".FormatWith(ResolveUrl("~/Scripts/DatePicker/WdatePicker.js")));

            JavaScriptBlock.Text = string.Join("\n", javascriptSet);
            ServiceURL.Text = "<script type=\"text/javascript\">var ETOffineServiceUrl = \"{0}\";</script>".FormatWith(this.ResolveUrl("~/WebService/ETOffineWebService.asmx"));
        }
    }
}
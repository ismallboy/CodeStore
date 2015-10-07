using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Amway.OA.ETOffine.Entities;
using System.Text;
using System.IO;

namespace Amway.OA.ETOffine.Web.Pages
{
    public class ETOffineBasePage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            this.ClientIDMode = System.Web.UI.ClientIDMode.Static;
        }

        /// <summary>
        /// 注册JS内容到前端页面
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="jsContent">JS内容</param>
        public void RegisterScriptBlock(string key, string jsContent)
        {
            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), key, jsContent, true);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMessage(string msg, bool closeWindow)
        {
            if (closeWindow)
            {
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "MsetInformation", string.Format(" alert('{0}');window.close();", msg.Replace("'", "\'")), true);
            }
            else
            {
                ShowMessage(msg);
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMessage(string msg, bool closeWindow, string func)
        {
            if (closeWindow)
            {
                //string js = string.Format("<script> alert('{0}');window.close();if (undefined != opener && undefined != opener.{1}) {{ opener.{1}(); }} </script>", msg.Replace("'", "\'"), func);
                //Response.Write(js);
                //Response.Flush();
                string js = string.Format(" alert('{0}');window.close();if (undefined != opener && undefined != opener.{1}) {{ opener.{1}(); }} ", msg.Replace("'", "\'"), func);
                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "MsetInformation", js, true);

            }
            else
            {
                ShowMessage(msg);
            }
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMessage(string msg)
        {
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "MsetInformation", "alert('" + msg.Replace("'", "\'") + "');", true);
        }


        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="msg"></param>
        public void ShowMessage(string msg, string redirectURL)
        {
            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "MsetInformation", "alert('" + msg.Replace("'", "\'") + "');window.location.href = '" + redirectURL +"';", true);
        }

        /// <summary>
        /// 当前页面的客户端可访问URL
        /// </summary>
        public string BaseUrl
        {
            get
            {
                return this.ResolveUrl("~/");
            }
        }

        /// <summary>
        /// 获取状态名称
        /// </summary>
        /// <param name="activityStatus"></param>
        /// <returns></returns>
        public string GetActivityStatus(int activityStatus)
        {
            if (activityStatus == (int)ActivityCheckingStatus.Download)
            {
                return "已下载";
            }
            else if (activityStatus == (int)ActivityCheckingStatus.UnUpload)
            {
                return "未上传";
            }
            else if (activityStatus == (int)ActivityCheckingStatus.HadUpload)
            {
                return "已上传";
            }
            return string.Empty;
        }

        public void ProcessStart()
        {
            try
            {
                //根据 ProgressBar.htm 显示进度条界面
                string templateFileName = Path.Combine(Server.MapPath("."), "ProgressBar.htm");
                using (StreamReader reader = new StreamReader(@templateFileName, System.Text.Encoding.GetEncoding("gb2312")))
                {
                    string html = reader.ReadToEnd();
                    Response.Write(html);
                    Response.Flush();
                }
            }
            catch { }
        }

        public void SetProcessBar(string msg, int percent)
        {
            try
            {
                var jsBlock = string.Format("<script>SetPorgressBar('{0}','{1}');</script>", msg, percent);
                Response.Write(jsBlock);
                Response.Flush();
            }
            catch { }
        }

        public void ProcessEnd(string msg)
        {
            try
            {
                // 处理完成
                var jsBlock = string.Format("<script>EndTrans('{0}');</script>", msg);
                Response.Write(jsBlock);
                Response.Flush();

                jsBlock = "<script>disPlay();</script>";
                Response.Write(jsBlock);
                Response.Flush();
            }
            catch { }
        }
    }
}
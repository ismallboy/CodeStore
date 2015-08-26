using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.Framework.Core;
using Amway.Framework.Security;
using Amway.OA.MS.BLL;
using Amway.OA.MS.Common;
using Amway.OA.MS.Entities.Constant;
using System.Web.Script.Serialization;
using Amway.OA.MS.Entities.DTO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Amway.OA.MS.Entities.DTO.Assistant;
using System.IO;

namespace Amway.OA.MS.Web.Pages.Assistant
{
    public partial class SendNotice : MSBasePage
    {
        public string RecordID
        {
            get { return Request["rid"].ToNullString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var msatActivityInfoBO = GetService<IMsatActivityInfoBO>();
                var actInfo = msatActivityInfoBO.GetSingle(o => o.ID == RecordID && o.Status == 1);
                if (actInfo == null)
                {
                    this.RegisterScriptBlock("fail", "alert('此活动不存在');");
                    return;
                }
                // 只有修改未发布或已发布状态允许推送信息
                if (!(actInfo.PublishStatus == (int)PublishStatus.Modify || actInfo.PublishStatus == (int)PublishStatus.Published))
                {
                    this.RegisterScriptBlock("fail", "alert('只允许状态为修改未发布或已发布的活动推送信息');");
                    return;
                }

                // 获取ADA号数
                IMsAssistantSqlBO msAssistantSqlBO = ServiceLocator.GetService<IMsAssistantSqlBO>();
                var attendeesSet = msAssistantSqlBO.GetEnrollmentStatus(RecordID, 0);
                spCount.InnerText = attendeesSet.Rows.Count + " 户";

                // 获取通知类型列表
                rdblNoticeType.Items.Add(new ListItem("报名邀请", "1"));
                rdblNoticeType.Items.Add(new ListItem("变更通知", "2"));
                rdblNoticeType.Items.Add(new ListItem("报名结果", "4"));
                rdblNoticeType.Items.Add(new ListItem("出席提醒", "3"));
                rdblNoticeType.Items.Add(new ListItem("自定义", "0"));

                // 获取会议详细信息
                var msATRealBo = GetService<IMsRealActivityInfoBO>();
                var msATInfo = msATRealBo.GetSingle(o => o.RecordID == RecordID && o.Status == 1);
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                hidActivityInfo.Value = serializer.Serialize(msATInfo);
                hidActivityPublishInfo.Value = serializer.Serialize(actInfo);
                //hidActivityDateString.Value = msATInfo.StartTime.Value.ToString("yyyy-MM-dd HH:mm") + "到" + msATInfo.FinishTime.Value.ToString("yyyy-MM-dd HH:mm");

                //求出每种通知类型的推送通知户数
                //报名结果推送户数
                var attendeesStatusDt = msAssistantSqlBO.GetEnrollmentStatus(RecordID, 4);
                int enrollmentCount = attendeesStatusDt.AsEnumerable().Select(o => o.Field<string>("DSTNUM")).Distinct().Count();
                int attendCount = attendeesStatusDt.AsEnumerable().Where(o => o.Field<string>("AGREE_ATTEND") == "是").Select(o => o.Field<string>("DSTNUM")).Distinct().Count();
                hidSendCount.Value = "[" + attendeesSet.Rows.Count.ToString() + "," + attendeesSet.Rows.Count.ToString() + "," + attendeesSet.Rows.Count.ToString() + "," + attendCount.ToString() + "," + enrollmentCount.ToString() + "]";
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                var msatActivityInfoBO = GetService<IMsatActivityInfoBO>();
                var actInfo = msatActivityInfoBO.GetSingle(o => o.ID == RecordID && o.Status == 1);
                if (actInfo == null || string.IsNullOrEmpty(actInfo.ID))
                {
                    this.RegisterScriptBlock("fail", "alert('此活动不存在');");
                    return;
                }
                // 只有修改未发布或已发布状态允许推送信息
                if (!(actInfo.PublishStatus == (int)PublishStatus.Modify || actInfo.PublishStatus == (int)PublishStatus.Published))
                {
                    this.RegisterScriptBlock("fail", "alert('只允许状态为修改未发布或已发布的活动推送信息');");
                    return;
                }

                // 获取会议详细信息
                var msATRealBo = GetService<IMsRealActivityInfoBO>();
                var msATInfo = msATRealBo.GetSingle(o => o.RecordID == RecordID && o.Status == 1);

                // 获取名单
                IMsAssistantSqlBO msAssistantSqlBO = ServiceLocator.GetService<IMsAssistantSqlBO>();
                var rs = false;

                using (DataTable attendeesStatusDt = msAssistantSqlBO.GetEnrollmentStatus(RecordID, rdblNoticeType.SelectedValue.ToInt(3)))
                {
                    //WechatMessage message = new WechatMessage();
                    //message.MCTitle = txtSendTitle.Text.Trim();// TO DO : 从界面获取
                    //message.ActivityDate = msATInfo.StartTime.Value.ToString("yyyy-MM-dd HH:mm") + " 到 " + msATInfo.FinishTime.Value.ToString("yyyy-MM-dd HH:mm");
                    //message.ActivityTitle = msATInfo.ActivityName.ToNullString();
                    //message.Remark = txtMsg.Text.Trim();// TO DO : 从界面获取
                    //wechatManager.SendTemplateMessages(adaNumberList, message);

                    AmwayHubManager wechatManager = new AmwayHubManager();
                    var sendTitle = txtSendTitle.Text.Trim();
                    var sendContent = hidSendContent.Value;
                    var sendContentEx = hidSendContentEx.Value;
                    //如果选择的是“报名结果”

                    PushWechatMessageDelegate messageDelegate = new PushWechatMessageDelegate(PushWechatMessage);

                    if (!string.IsNullOrEmpty(sendContentEx))
                    {
                        rs = wechatManager.SendMessage(sendTitle, sendContent, attendeesStatusDt, messageDelegate, sendContentEx, 1, RecordID);

                    }
                    //如果选择的是出席提醒
                    else if (rdblNoticeType.SelectedValue == "3")
                    {

                        rs = wechatManager.SendMessage(sendTitle, sendContent, attendeesStatusDt, messageDelegate, sendContentEx, 2, RecordID);
                    }
                    else
                    {
                        rs = wechatManager.SendMessage(sendTitle, sendContent, attendeesStatusDt, messageDelegate, sendContentEx, 0, RecordID);
                    }
                }

                if (rs)
                {
                    // 推送后更新活动信息
                    actInfo.PushStatus = 1;
                    actInfo.PushTime = DateTime.Now;
                    actInfo.NotificationCnt = actInfo.NotificationCnt + 1;
                    msatActivityInfoBO.Update(actInfo);
                    this.RegisterScriptBlock("success", "alert('通知推送成功');window.opener.location.reload(); window.close();");
                }
                else
                {
                    this.RegisterScriptBlock("failnotice", "alert('通知推送失败');");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("通知推送错误！详细：" + ex.Message + "," + ex.StackTrace);
                this.RegisterScriptBlock("errornotice", "alert('通知推送错误');");
            }
        }

        /// <summary>
        /// 微信推送消息
        /// </summary>
        public bool PushWechatMessage(List<MsgWeChatPushDTO> postDataObjList)
        {
            try
            {
                //读取微信推送消息配置信息
                var weChatAuthorization = BaseDataHelper.GetDataItemList("MSAT_WeChatAuthorization").ToDictionary(o => o.Code, p => p.DisplayName);
                var weChatAppId = weChatAuthorization["AppId"];
                var weChatAppKey = weChatAuthorization["AppKey"];
                var weChatAPostUrl = weChatAuthorization["RouterForWeChat"];
                System.Uri uri = new System.Uri(weChatAPostUrl);

                foreach (var postDataObject in postDataObjList)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Headers.Add("appid", weChatAppId);
                    request.Headers.Add("appkey", weChatAppKey);
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.Method = "POST";
                    ASCIIEncoding encoding = new ASCIIEncoding();
                    var postData = "messageModelCode=activity_apply_mind&messageDataJson=" + Server.UrlPathEncode(JsonConvert.SerializeObject(postDataObject));
                    byte[] arrPostData = encoding.GetBytes(postData);
                    request.ContentLength = arrPostData.Length;
                    request.Timeout = 60000;
                    //发送请求
                    using (System.IO.Stream stream = request.GetRequestStream())
                    {
                        stream.Write(arrPostData, 0, arrPostData.Length);
                    }
                    //获取返回结果
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //
                    StreamReader streamReader = new StreamReader(response.GetResponseStream());
                    string strResult = streamReader.ReadToEnd();

                    var tmpObject = JsonConvert.DeserializeObject(strResult) as JObject;
                    var returnStatus = (string)tmpObject["status"];
                    if (returnStatus == "1")
                    {
                        Logger.Debug("微信推送成功");
                    }
                    else
                    {
                        Logger.Debug("微信推送失败;发送对象：" + postDataObject.ADANUMBER);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("推送微信信息出错；详细：" + ex.Message + ex.StackTrace);
                return false;
            }
            return true;
        }
    }
}
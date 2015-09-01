/// <summary>
/// 消息推送接口
/// </summary>
/// <param name="title"></param>
/// <param name="content"></param>
/// <param name="contentEx">发送内容扩展信息,表示报名不通过模板（只有sendType == 1的时候该参数才起作用）</param>
/// <param name="sendType">1表示报名结果模板，2表示出席提醒，0表示其他模板）</param>
/// <param name="recordID"></param>
/// <returns></returns>
public bool SendMessage(string title, string content, DataTable adaNumberDT, string contentEx = "", int sendType = 0, string recordID = "")
{
    try
    {
        Logger.Debug("调用推送消息：ReocrdID:{0}.", recordID);

        // 读取数码港配置信息
        var amwayHubAuthorization = BaseDataHelper.GetDataItemList("MSAT_MessageSetting").ToDictionary(o => o.Code, p => p.DisplayName);
        var appId = amwayHubAuthorization["AppId"];
        var appKey = amwayHubAuthorization["AppKey"];
        var callerUrl = amwayHubAuthorization["CallerUrl"];
        var msgType = amwayHubAuthorization["Type"];

        // 获取会议详细信息
        var msRealActivityInfoBo = ServiceLocator.GetService<IMsRealActivityInfoBO>();
        var msRealActivityInfo = msRealActivityInfoBo.GetSingle(o => o.RecordID == recordID && o.Status == 1);

        var msgList = new List<AmwayHubService.Message>();
        //by Allen 2015年6月5日
        var msgWechatList = new List<MsgWeChatPushDTO>();
        var enrollmentStatusList = adaNumberDT.AsEnumerable();
        //审批允许出席的表示报名成功，审批不允许出席的表示报名不成功
        foreach (var item in enrollmentStatusList)
        {
            // 生成消息对象
            var msg = new AmwayHubService.Message();

            msg.channel = "MSAT";
            msg.htmlContent = string.Empty;
            msg.receiver = item.Field<string>("DSTNUM");
            msg.sendTime = "";// "yyyy-MM-dd HH:mm:ss",小于或等于当前时间或空，即时发送；指定往后的时间，会在指定的时间发送。
            msg.type = msgType; // 类型（1消息、2短信、3邮件）
            msg.title = title;
            msg.textContent = content.Replace("XXX", item.Field<string>("DSTNAME"));

            //by Allen 2015年6月5日
            var msgWechat = new MsgWeChatPushDTO();
            msgWechat.OPEN_ID = "";
            msgWechat.ADANUMBER = item.Field<string>("DSTNUM");
            msgWechat.FIRST = title;
            msgWechat.KEYWORD1 = msRealActivityInfo.ActivityName;
            var activityTime = (msRealActivityInfo.FinishTime.Value.ToShortDateString() == msRealActivityInfo.StartTime.Value.ToShortDateString()) ?
                msRealActivityInfo.StartTime.Value.ToString("yyyy年MM月dd日 HH:mm") + "-" + msRealActivityInfo.FinishTime.Value.ToShortTimeString() :
                msRealActivityInfo.StartTime.Value.ToString("yyyy年MM月dd日 HH:mm");

            msgWechat.KEYWORD2 = activityTime;
            msgWechat.REMARK = content.Replace("XXX", item.Field<string>("DSTNAME"));

            //设置活动详情页面
            var QRCodeURL = BaseDataHelper.GetDataItemList("MSAT_QRCodeURL ").ToDictionary(o => o.Code, p => p.DisplayName);
            var previewQRCodeLink = QRCodeURL["PublishQRCodeUrl"];
            msgWechat.URL = previewQRCodeLink + msRealActivityInfo.ActivitySn;

            //如果发送的是报名结果模板，名单报名成功，则发送content，否则发送contentEx
            if (sendType == 1 && item.Field<string>("AGREE_ATTEND") == "否")
            {
                msg.textContent = contentEx.Replace("XXX", item.Field<string>("DSTNAME"));
                msgWechat.REMARK = contentEx.Replace("XXX", item.Field<string>("DSTNAME"));
            }

            msgList.Add(msg);
            msgWechatList.Add(msgWechat);
        }
        var paramList = new AmwayHubService.MessageRequest();
        paramList.messageArr = msgList.ToArray();

        //数码港推送
        using (var obj = new AmwayHubService.AmwayMsgServiceService())
        {
            // 需要使用配置的URL
            obj.Url = callerUrl;
            // 需要使用配置的验证
            obj.Credentials = new System.Net.NetworkCredential(appId, appKey);

            Logger.Debug("数码港开始推送消息：ReocrdID:{0}.详细：{1}.", recordID, JsonConvert.SerializeObject(msgList[0]));
            var rs = obj.saveMessage(paramList);
            Logger.Debug("数码港推送消息返回：{0}:{1},ReocrdID:{2}.", rs.errorCode, rs.errorMsg, recordID);

            if (string.IsNullOrWhiteSpace(rs.errorCode))
            {
                Logger.Debug("数码港推送消息成功：ReocrdID:{0}.", recordID);
            }
            else
            {
                Logger.Debug("数码港推送消息失败：{0}:{1},ReocrdID:{2}.详细：{3}.", rs.errorCode, rs.errorMsg, recordID, JsonConvert.SerializeObject(msgList[0]));
            }
        }
        PushWechatMessage(msgWechatList);
        return true;
    }
    catch (System.Exception ex)
    {
        Logger.Error("推送消息失败：{0},ReocrdID:{1}.详细：{2}.", ex.StackTrace, recordID, ex.Message + ex.StackTrace);
        return false;
    }
}
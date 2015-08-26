    public static bool UpdateCardPackAudit(string activityID, string cardId)
    {
        try
        {
            string Url = string.Empty;
            string ParmStr = string.Empty;
            GetUrlAndParmStr(activityID, cardId, out Url, out ParmStr);
            Logger.Debug("开始调用微信卡包更新接口，传送的参数为：activity:{0},carid:{1}，Url:{2},Parmstr:{3}".FormatWith(activityID, string.Empty, Url, ParmStr));
            string responseStr = HttpPOST(Url, ParmStr);
            Logger.Debug("微信卡包更新接口返回值为：" + responseStr);
            var obj = JsonConvert.DeserializeObject(responseStr) as JObject;
            if (obj != null && obj["errcode"] != null && obj["errcode"].ToString() == "0")
            {
                //把cardid保存在MSAD活动表
                var msadMeetinfoBO = ServiceLocator.GetService<IMsadMeetinfoBO>();
                var msadMeetinfo = msadMeetinfoBO.GetById(activityID);
                msadMeetinfo.AuditedCardpack += 1;
                msadMeetinfo.LastAuditTime = DateTime.Now;
                msadMeetinfoBO.Update(msadMeetinfo);
                Logger.Debug("更新卡包（一套）成功，活动ID：{0}".FormatWith(activityID));
                return true;
            }
            else
            {
                Logger.Debug("更新卡包（一套）失败：{0}   ，活动ID：{1}".FormatWith((string)obj["errmsg"], activityID));
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("更新卡包（一套）失败：{0}    ，活动ID：{1}".FormatWith(ex.Message, activityID));
            return false;
        }
    }
    public static bool AddCardPackAudit(string activityID)
    {
        try
        {
            string Url = string.Empty;
            string ParmStr = string.Empty;
            GetUrlAndParmStr(activityID, string.Empty, out Url, out ParmStr);
            Logger.Debug("开始调用微信卡包创建接口，传送的参数为：activity:{0},carid:{1}，Url:{2},Parmstr:{3}".FormatWith(activityID, string.Empty, Url, ParmStr));
            string responseStr = HttpPOST(Url, ParmStr);
            Logger.Debug("微信卡包创建接口返回值为：" + responseStr);
            var obj = JsonConvert.DeserializeObject(responseStr) as JObject;
            if (obj != null && obj["errcode"] != null && obj["errcode"].ToString() == "0")
            {
                //把cardid保存在MSAD活动表
                var msadMeetinfoBO = ServiceLocator.GetService<IMsadMeetinfoBO>();
                var msadMeetinfo = msadMeetinfoBO.GetById(activityID);
                msadMeetinfo.CardpackID = (string)obj["card_id"];
                msadMeetinfo.AuditedCardpack = 1;
                msadMeetinfo.LastAuditTime = DateTime.Now;
                msadMeetinfoBO.Update(msadMeetinfo);
                Logger.Debug("添加卡包（一套）成功，活动ID：{0}".FormatWith(activityID));
                return true;
            }
            else
            {
                Logger.Debug("添加卡包（一套）失败：{0}   ，活动ID：{1}".FormatWith((string)obj["errmsg"], activityID));
                return false;
            }
        }
        catch (Exception ex)
        {
            Logger.Error("添加卡包（一套）失败：{0}    ，活动ID：{1}".FormatWith(ex.Message, activityID));
            return false;
        }
    }

    private static void GetUrlAndParmStr(string activityID, string cardId, out string url, out string parmStr)
    {
        var msatExActivityBO = ServiceLocator.GetService<IMsatExActivityInfoBO>();
        var activityInfo = msatExActivityBO.GetById(activityID);
        StringBuilder parmBuilder = new StringBuilder();
        parmBuilder.Append("card_type=MEETING_TICKET");//卡包类型：MEETING_TICKET为会议门票
        var cardPackConfigList = BaseDataHelper.GetBaseDataList("MSAT_CardPackConfig").ToDictionary(o => o.Code, p => p.DisplayName);
        parmBuilder.Append("&logo_url={0}".FormatWith(cardPackConfigList["LogoUrl"]));//logo地址
        parmBuilder.Append("&brand_name={0}".FormatWith(cardPackConfigList["BrandName"]));//卡券商标名字
        parmBuilder.Append("&code_type=CODE_TYPE_QRCODE");
        parmBuilder.Append("&activity_sn={0}".FormatWith(activityInfo.ActivitySn));//活动编号
        parmBuilder.Append("&title={0}".FormatWith(activityInfo.ActivityName));//卡卷标题
        parmBuilder.Append("&color={0}".FormatWith(cardPackConfigList["CardPackColor"]));//卡券颜色
        parmBuilder.Append("&notice={0}".FormatWith(cardPackConfigList["Notice"]));//卡券使用提示
        parmBuilder.Append("&service_phone={0}".FormatWith(cardPackConfigList["Service_phone"]));//客服使用电话
        parmBuilder.Append("&description={0}".FormatWith(string.IsNullOrEmpty(activityInfo.TicketDescription) ? "无" : activityInfo.TicketDescription));//描述
        parmBuilder.Append("&begin_time={0}".FormatWith(Convert.ToDateTime(activityInfo.StartTime).ToString("yyyyMMddHH")));//会议起始时间
        //获取电子门票过期时间
        var msetDefaultTicketCategory = BaseDataHelper.GetDataItemList("MSET_OverDueTime");
        var ticketOverDueTime = msetDefaultTicketCategory.SingleOrDefault(o => o.Code == "TicketOverdueTime").DisplayName.ToInt(10);
        parmBuilder.Append("&end_time={0}".FormatWith(Convert.ToDateTime(activityInfo.FinishTime.Value.AddHours(ticketOverDueTime)).ToString("yyyyMMddHH")));//会议结束时间
        var msatExCategoryBO = ServiceLocator.GetService<IMsatExTicketCategoryBO>();
        var msatCategory = msatExCategoryBO.GetFilteredList(o => o.RecordID == activityInfo.ID && o.Status == 1);
        parmBuilder.Append("&quantity={0}".FormatWith(msatCategory.Sum(o => o.Quantity)));//卡券数量
        var maxQuantityForPurchaser = BaseDataHelper.GetBaseDataList("MSET_MaxQuantityForPurchaser").ToDictionary(o => o.Code, p => p.DisplayName);
        parmBuilder.Append("&get_limit={0}".FormatWith(maxQuantityForPurchaser["MaxQutity"]));//每人获得的最大限制
        parmBuilder.Append("&can_share=true");//是否可以分享(可空）
        parmBuilder.Append("&use_custom_code=y");
        parmBuilder.Append("&can_give_friend=true");//是否可以赠送(可空）
        parmBuilder.Append("&location_id_list=");//可使用的店，填入商店列表查询接口返回来的poiid(可空）
        parmBuilder.Append("&promotion_url_name=");//推广名称 (可空）
        parmBuilder.Append("&promotion_url=");//推广URL
        parmBuilder.Append("&detail=时间：{0}-{1}\n地点：{2}".FormatWith(Convert.ToDateTime(activityInfo.StartTime).ToString("yyyy.MM.dd"), Convert.ToDateTime(activityInfo.FinishTime.Value.AddHours(ticketOverDueTime)).ToString("yyyy.MM.dd"), activityInfo.HallAddress));//具体信息

        if (!string.IsNullOrWhiteSpace(cardId))//更新还是修改
        {
            parmBuilder.Append("&card_id={0}".FormatWith(cardId));
            url = "{0}updateCardInfo.jhtml".FormatWith(cardPackConfigList["IWeChatUrl"]);
        }
        else
        {
            url = "{0}createCard.jhtml".FormatWith(cardPackConfigList["IWeChatUrl"]);
        }
        parmStr = parmBuilder.ToNullString();
    }

    #region Post 和 Get 函数
    private static string HttpGET(string Url, string postDataStr)
    {
        string retString = string.Empty;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
        request.Method = "GET";
        request.ContentType = "text/html;charset=UTF-8";
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (StreamReader myStreamReader = new StreamReader(response.GetResponseStream()))
        {
            return myStreamReader.ReadToEnd();
        }
    }

    private static string HttpPOST(string url, string postDataStr)
    {
        string retString = string.Empty;
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        byte[] arrPostData = Encoding.UTF8.GetBytes(postDataStr);
        request.KeepAlive = true;
        using (Stream myStream = request.GetRequestStream())
        {
            myStream.Write(arrPostData, 0, arrPostData.Length);
        }
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        using (StreamReader myStreamReader = new StreamReader(response.GetResponseStream()))
        {
            retString = myStreamReader.ReadToEnd();
            return retString;
        }
    }
    #endregion
}
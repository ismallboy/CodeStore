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
        if (string.IsNullOrWhiteSpace(weChatAPostUrl) || weChatAPostUrl == "HTTP")
        {
            return false;
        }
        System.Uri uri = new System.Uri(weChatAPostUrl);
        foreach (var postDataObject in postDataObjList)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Headers.Add("appid", weChatAppId);
            request.Headers.Add("appkey", weChatAppKey);
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = "POST";

            ASCIIEncoding encoding = new ASCIIEncoding();
            var postData = "messageModelCode=activity_apply_mind&channel_id=1&messageDataJson=" +System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(postDataObject));
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
            Logger.Debug("微信推送返回值：" + strResult);

            var tmpObject = JsonConvert.DeserializeObject(strResult) as JObject;
            var returnStatus = (string)tmpObject["status"];
            if (returnStatus == "1")
            {
                Logger.Debug("微信推送成功;发送对象：" + postDataObject.ADANUMBER);
            }
            else
            {
                Logger.Debug("微信推送失败;发送对象：" + postDataObject.ADANUMBER + ";详细：" + (string)tmpObject["errmsg"]);
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
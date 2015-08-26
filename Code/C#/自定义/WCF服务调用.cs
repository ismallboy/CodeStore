
ps:要在web.config里配置相关的终结点；如下：
      <system.serviceModel>
        <bindings>
          <basicHttpBinding>
            <binding name="OrderFacadeServicePortBinding" />
            <binding name="ECardServiceImplPortBinding" />
          </basicHttpBinding>
        </bindings>
        <client>
          <endpoint address="http://gzlnx055:7103/POS_5G_Services_v1_0_0/order/OrderSrv"
              binding="basicHttpBinding" bindingConfiguration="OrderFacadeServicePortBinding"
              contract="Service5GPos.OrderFacadeService" name="OrderFacadeServicePort" />
          <endpoint address="http://gzlnx065:7103/DA_Ecard_Services_v1_0_0/ECardUat/AmwayEcardHubU"
              binding="basicHttpBinding" bindingConfiguration="ECardServiceImplPortBinding"
              contract="ESBECardCUService.ECardService" name="ECardServiceImplPort" />
        </client>
      </system.serviceModel>

/// <summary>
/// 在E-Card创建门票
/// </summary>
/// <param name="ticketDTOList"></param>
/// <returns></returns>
private bool MeetingTicketCreated(List<ESBECardCUService.ActivityTicketDTO> ticketDTOList)
{
    try
    {
        //读取E卡配置信息
        var amwayECardService = BaseDataHelper.GetDataItemList("MSET_ECardService").ToDictionary(o => o.Code, p => p.DisplayName);
        var appId = amwayECardService["AppId"];
        var appKey = amwayECardService["AppKey"];
        var callerUrl = amwayECardService["CallerUrl"];


        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("osbappid", appId);
        headers.Add("osbappkey", appKey);

        var postDataStr = JsonConvert.SerializeObject(ticketDTOList);

        Logger.Debug("开始调用E卡创建接口（postData为空则不调），调用参数为：" + postDataStr);
        if (ticketDTOList != null && ticketDTOList.Count > 0)
        {
            var client = new ESBECardCUService.ECardServiceClient();
            client.Endpoint.Address = new System.ServiceModel.EndpointAddress(callerUrl);


            using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
            {
                //添加header
                foreach (var item in headers)
                {
                    MessageHeader header = MessageHeader.CreateHeader(item.Key, string.Empty, item.Value);
                    OperationContext.Current.OutgoingMessageHeaders.Add(header);
                }

                var result = client.meetingTicketCreated(ticketDTOList.ToArray());

                var resultStr = JsonConvert.SerializeObject(result);
                Logger.Debug("调用E卡创建接口返回结果：" + resultStr);

                if (result.result == 1)
                {
                    Logger.Debug("调用E卡创建接口成功；");
                    return true;
                }
                else
                {
                    Logger.Error("调用E卡创建接口失败；;详情：" + result.messageContent);
                    return false;
                }

            }
        }
        else
        {
            Logger.Error("调用E卡创建接口失败,没有门票信息.");
            return false;
        }
    }
    catch (Exception ex)
    {
        Logger.Error("调用E卡创建接口失败；;详情：" + ex.Message + ex.StackTrace);
        return false;
    }
}
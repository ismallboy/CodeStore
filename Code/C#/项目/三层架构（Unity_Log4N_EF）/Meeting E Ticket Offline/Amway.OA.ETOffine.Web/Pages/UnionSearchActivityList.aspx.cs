using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.Utilities;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.IO;
using Amway.OA.ETOffine.Entities;
using Amway.OA.ETOffine.BLL;
using AutoMapper;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class UnionSearchActivityList : ETOffineBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

            if (!IsPostBack)
            {
                try
                {
                    if (!Utility.IsConnectionNet())
                    {
                        ShowMessage("本机网络已断开，请检查。", "http://localhost/Pages/UnionCheckingActivityList.aspx");
                        return;
                    }
                    var localHost = Utility.GetIP(); // 本机IP                        
                    if (localHost.Substring(0, 8) == "127.0.0.")
                    {
                        ShowMessage("本机IP设置错误，请检查。本机IP为：{0}。".FormatWith(localHost), "http://localhost/Pages/UnionCheckingActivityList.aspx");
                        return;
                    }
                    ScanAllHost(localHost);
                }
                catch (Exception ex)
                {
                    Logger.Error("查找主机错误。详细：" + ex.Message + ex.StackTrace);
                }
            }
        }

        protected void ltnJoinActivity_Click(object sender, EventArgs e)
        {
            var val = lstAcivityList.SelectedValue.Split('$');
            var recordID = val[0];
            var url = "http://{0}/Pages/CheckTicket.aspx?RID={1}&URL=http://localhost/Pages/UnionCheckingActivityList.aspx".FormatWith(val[1], recordID);

            // 搜出来的主机IP当时是可用的（获取时已作判断）
            // 如果主机IP存在
            if (!string.IsNullOrWhiteSpace(val[1]))
            {
                var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
                var activity = activityBO.GetSingle(o => o.RecordID == recordID && o.Status == 1);
                if (activity != null && activity.HostIp == val[1])
                {// 主机为本机IP，直接访问
                    GotoCheckTicket(url);
                }
                else if (activity != null)
                {// 如果在本机，可找到该会议数据，直接设置该会议主机IP为搜索到的IP。
                    activity.HostIp = val[1];
                    activity.UpdateHostTime = DateTime.Now;
                    activityBO.Update(activity);

                    GotoCheckTicket(url);
                }
                else
                { // 本地不存在该活动数据，需要从主机下载下来，并设置为备机
                    var activityTemp = new MsetOfflineActivity();

                    using (var service = new OfflineService.ForRemotingWebService())
                    {
                        service.Url = string.Format("http://{0}/WebService/ForRemotingWebService.asmx", val[1]);

                        #region 被动分发数据

                        // 获取会议信息
                        var remoteActivity = service.GetActivity(recordID);
                        if (remoteActivity != null)
                        {
                            Mapper.DynamicMap(remoteActivity, activityTemp);
                            activityTemp.ID = null;
                            activityTemp.IsHost = 0;
                            activityTemp.SyncDate = DateTime.Now;
                            activityTemp.SyncStatus = 1;
                            activityTemp.UpdateHostTime = DateTime.Now;

                            activityBO.Add(activityTemp);
                        }

                        // 获取会议门票信息
                        var ticketList = service.GetTicketList(recordID);
                        var ticketBO = ServiceLocator.GetService<IMsetOfflineTicketBO>();
                        foreach (var item in ticketList)
                        {
                            var ticket = new MsetOfflineTicket();
                            Mapper.DynamicMap(item, ticket);
                            ticket.ID = null;
                            ticketBO.Add(ticket);
                        }

                        // 获取验票Log信息
                        var ticketLogList = service.GetTicketLogList(recordID);
                        var ticketLogBO = ServiceLocator.GetService<IMsetOfflineCheckingLogBO>();
                        foreach (var item in ticketLogList)
                        {
                            var ticketLog = new MsetOfflineCheckingLog();
                            Mapper.DynamicMap(item, ticketLog);
                            ticketLog.ID = null;
                            ticketLogBO.Add(ticketLog);
                        }
                        #endregion
                    }

                    GotoCheckTicket(url);
                }
            }
        }

        private void GotoCheckTicket(string url)
        {
            var jsBlock = string.Format("<script>window.location.href = '{0}'; </script>", url);
            Response.Write(jsBlock);
            Response.Flush();
        }

        // 进度计数据器
        private int processCounter = 0;

        /// <summary>
        /// 扫描所有活动主机
        /// </summary>
        public void ScanAllHost(string localHost)
        {
            try
            {
                var ipPrefix = localHost.Substring(0, localHost.LastIndexOf('.') + 1); // 前三段IP，包含第三个.

                processCounter = 0;// 重置进度计数器                
                var batchPingCount = 4; // 每个线程每次Ping多少个IP        
                var pingThreadCount = (int)(256 / batchPingCount); // PingIP线程数量        

                ProcessStart(); // 开始构建进度条
                SetProcessBar("正在搜索活动主机", 5); // 设置进度条初始长度为5%

                #region 分线程Ping网段IP是否通过

                // 定义线程安全类型变量, 装载可以Ping通的IP列表
                ConcurrentQueue<string> passIPList = new ConcurrentQueue<string>();

                Parallel.For(0, pingThreadCount, (n) =>
                {
                    var minIP = n * batchPingCount + 1; // n是从0开始,IP从1开始
                    var maxIP = minIP + batchPingCount; // 

                    for (int i = minIP; i < maxIP; i++)
                    {
                        if (i > 255)//if (endSubIPNumber == i || i > 255) 暂时不排除本机
                        {
                            break;
                        }
                        var ip = ipPrefix + i.ToString();
                        Ping pg = new Ping();
                        var rply = pg.Send(ip, 100);
                        if (rply.Status == IPStatus.Success)
                        {
                            passIPList.Enqueue(ip);
                        }
                        processCounter++;
                        if (n == 5)
                        {
                            // 5.1为进度调整参数。只允许其中一个线程执行，不然的话，并发时在请求输出缓冲区会报错。
                            SetProcessBar("正在搜索活动主机", (int)(processCounter / 5.1));
                        }
                    }
                });
                #endregion
                                
                SetProcessBar("正在搜索活动主机", 50); // Ping完IP认为进度达到50%

                var batchRequestCount = 3; // 每个线程每次请求数量
                var requestThreadCount = (int)(passIPList.Count / batchRequestCount) + 1; // 计算请求线程数
                var passIPArr = passIPList.ToArray();
                processCounter = 0; // 重置计数器

                ConcurrentQueue<MsetOfflineActivity> activityList = new ConcurrentQueue<MsetOfflineActivity>();
                if (requestThreadCount > 1)
                {
                    #region 大于batchRequestCount个分线程执行

                    Parallel.For(0, requestThreadCount, (n) =>
                    {
                        var minIndex = n * batchRequestCount; // n是从0开始,Index从0开始
                        var maxIndex = minIndex + batchRequestCount; // 

                        for (int i = minIndex; i < maxIndex; i++)
                        {
                            if (i >= passIPList.Count)
                            {
                                break;
                            }
                            var resposeData = string.Empty;
                            if (RequestWebsite(passIPArr[i], "method=FindAllHost", out resposeData) && !string.IsNullOrWhiteSpace(resposeData))
                            {
                                if (resposeData != "\0")
                                {
                                    var partList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MsetOfflineActivity>>(resposeData);
                                    foreach (var subAT in partList)
                                    {
                                        activityList.Enqueue(subAT);
                                    }
                                }
                            }

                            processCounter++;
                            if (n == 1)
                            {
                                SetProcessBar("正在搜索活动主机", (int)(processCounter * 50.0 / passIPArr.Length) + 50);
                            }
                        }
                    });
                    #endregion
                }
                else
                {
                    #region 小于等于batchRequestCount个按步执行

                    foreach (var item in passIPList)
                    {
                        var resposeData = string.Empty;
                        if (RequestWebsite(item, "method=FindAllHost", out resposeData) && !string.IsNullOrWhiteSpace(resposeData))
                        {
                            if (resposeData != "\0")
                            {
                                var partList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MsetOfflineActivity>>(resposeData);
                                foreach (var subAT in partList)
                                {
                                    activityList.Enqueue(subAT);
                                }
                            }
                        }
                        processCounter++;
                        SetProcessBar("正在搜索活动主机", (int)(processCounter * 50.0 / passIPArr.Length) + 50);
                    }
                    #endregion
                }
                               
                SetProcessBar("正在搜索活动主机", 100); // 完成100%

                var list = activityList.OrderBy(o => o.HostIp).OrderByDescending(o => o.UpdateHostTime).ToList();
                if (list.Count > 0)
                {
                    var activiyDTOList = list.OrderBy(o => o.HostIp).OrderByDescending(o => o.UpdateHostTime)
                        .Select(o => new KeyValuePair<string, string>(o.RecordID + "$" + o.HostIp.ToNullString(), Server.HtmlDecode(o.ActivitySn.CPadLeft(12) + o.ActivityName.CPadLeft(25) + GetActivityStatus(o.CheckingStatus.ToInt(0)) + "&nbsp;&nbsp;&nbsp;&nbsp;" + o.HostIp.ToNullString() + "&nbsp;&nbsp;&nbsp;&nbsp;" + o.UpdateHostTime.Value.ToString("yyyy-MM-dd HH:mm:ss"))));

                    lstAcivityList.DataSource = activiyDTOList;
                    lstAcivityList.DataTextField = "Value";
                    lstAcivityList.DataValueField = "Key";
                    lstAcivityList.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ProcessEnd("搜索完成。");
            }
        }

        private bool RequestWebsite(string ip, string postData, out string result)
        {
            result = string.Empty;
            try
            {
                var url = string.Format("http://{0}/WebService/DataTransferHandler.ashx", ip);

                System.Uri uri = new System.Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] arrPostData = encoding.GetBytes(postData);
                request.ContentLength = arrPostData.Length;
                request.Timeout = 1000;

                //发送请求
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(arrPostData, 0, arrPostData.Length);
                }

                //获取返回结果
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
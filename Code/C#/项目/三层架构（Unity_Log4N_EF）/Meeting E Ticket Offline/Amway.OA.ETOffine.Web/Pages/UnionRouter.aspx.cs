using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Amway.OA.ETOffine.BLL;
using Amway.OA.ETOffine.Web.OfflineService;
using Amway.OA.ETOffine.Utilities;
using System.Net.NetworkInformation;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Amway.OA.ETOffine.Web.Pages
{
    public partial class UnionRouter : ETOffineBasePage
    {
        public string RecordID { get { return Request.QueryString["RID"].ToNullString(); } }
        public string Method { get { return Request.QueryString["t"].ToNullString(); } }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);

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

            if (Method == "check")
            {// 验票
                CheckTicket();
                return;
            }
            else if (Method == "monitor")
            {// 监视
                Monitor();
                return;
            }
            else if (Method == "view")
            {// 查看
                View();
                return;
            }
        }

        private void Monitor()
        {
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var activity = activityBO.GetSingle(o => o.RecordID == RecordID && o.Status == 1);

            if (activity == null)
            {
                GotoUnionChecking("会议不存在, 返回多机验票列表。");
                return;
            }
            if (string.IsNullOrWhiteSpace(activity.HostIp))
            {
                GotoUnionChecking("没有设置主机, 返回多机验票列表。");
                return;
            }

            Ping cpg = new Ping();
            var crply = cpg.Send(activity.HostIp, 200);
            if (crply.Status == IPStatus.Success)
            {
                var message = string.Empty;
                if (RequestWebsite(activity.HostIp, "method=other", out message))
                {
                    GotoMonitor(activity.HostIp);
                }
                else
                {
                    GotoUnionChecking("主机不可用, 返回多机验票列表。");
                    return;
                }
            }
        }

        private void View()
        {
            var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
            var activity = activityBO.GetSingle(o => o.RecordID == RecordID && o.Status == 1);

            if (activity == null)
            {
                GotoUnionChecking("会议不存在, 返回多机验票列表。");
                return;
            }
            if (string.IsNullOrWhiteSpace(activity.HostIp))
            {
                GotoUnionChecking("没有设置主机, 返回多机验票列表。");
                return;
            }

            Ping cpg = new Ping();
            var crply = cpg.Send(activity.HostIp, 200);
            if (crply.Status == IPStatus.Success)
            {
                var message = string.Empty;
                if (RequestWebsite(activity.HostIp, "method=other", out message))
                {
                    GotoView(activity.HostIp);
                }
                else
                {
                    GotoUnionChecking("主机不可用, 返回多机验票列表。");
                    return;
                }
            }
        }

        private void CheckTicket()
        {
            try
            {             
                var localHost = Utility.GetIP();
                var activityBO = ServiceLocator.GetService<IMsetOfflineActivityBO>();
                var activity = activityBO.GetSingle(o => o.RecordID == RecordID && o.Status == 1);

                if (activity == null)
                {
                    GotoUnionChecking("会议不存在, 返回多机验票列表。");
                    return;
                }

                // 无论是否有设置主机，都需要查找是否存在多个主机
                var list = CheckCurActivityAllHost(localHost);

                if (list.Count == 0)
                {// 没有找到其他主机，设置他自己为主机
                    activity.HostIp = localHost;
                    activity.IsHost = 1;
                    activity.UpdateHostTime = DateTime.Now;
                    activityBO.Update(activity);
                }
                else if (list.Count > 0 && list[0].HostIp != localHost)
                {// 如果找到1个或多个主机，设置最新设置为主机的机子为主机
                    activity.HostIp = list[0].HostIp.ToNullString();
                    activity.IsHost = 0;
                    activity.UpdateHostTime = DateTime.Now;
                    activityBO.Update(activity);
                }

                if (!string.IsNullOrWhiteSpace(activity.HostIp))
                {
                    GotoCheckTicket(activity.HostIp);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("进入验票界面异常。详细：" + ex.Message + ex.StackTrace);
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

        // 进度计数器
        private int processCounter = 0;
        /// <summary>
        /// 搜索当前活动的所有主机
        /// </summary>
        /// <param name="localHost"></param>
        /// <returns></returns>
        private List<MsetOfflineActivity> CheckCurActivityAllHost(string localHost)
        {
            var list = new List<MsetOfflineActivity>();

            var preSubIPStr = localHost.Substring(0, localHost.LastIndexOf('.') + 1);
            var endSubIPNumber = localHost.Substring(localHost.LastIndexOf('.') + 1).ToInt(0);
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
                            if (RequestWebsite(passIPArr[i], "method=CheckHost&recordid={0}".FormatWith(RecordID), out resposeData) && !string.IsNullOrWhiteSpace(resposeData))
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
                        if (RequestWebsite(item, "method=CheckHost&recordid={0}".FormatWith(RecordID), out resposeData) && !string.IsNullOrWhiteSpace(resposeData))
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
                list = activityList.OrderBy(o => o.HostIp).OrderByDescending(o => o.UpdateHostTime).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ProcessEnd("检测完成。");
            }

            #region Del
            /* 
            ProcessStart();

            for (int i = 1; i < 256; i++)
            {
                if (i == endSubIPNumber)
                {
                    //continue;
                }
                if (i % 2 == 0)
                {
                    SetProcessBar("正在搜索活动主机", (int)(i / 255.00 * 100));
                }

                var ip = preSubIPStr + i.ToString();
                Ping pg = new Ping();
                var rply = pg.Send(ip, 200);
                if (rply.Status == IPStatus.Success)
                {
                    var result = string.Empty;
                    if (RequestWebsite(ip, "method=CheckHost&recordid={0}".FormatWith(RecordID), out result))
                    {
                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            var partList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MsetOfflineActivity>>(result);
                            list.AddRange(partList);
                        }
                    }
                }
            }
            ProcessEnd("搜索完成。");
            */
            #endregion

            return list;
        }

        private void GotoCheckTicket(string hostIP)
        {
            var url = string.Format("http://{0}/Pages/CheckTicket.aspx?RID={1}&URL=http://localhost/Pages/UnionCheckingActivityList.aspx", hostIP, RecordID);
            var jsBlock = string.Format("<script>window.open('{0}', '_self');</script>", url);
            Response.Write(jsBlock);
            Response.Flush();
        }

        private void GotoMonitor(string hostIP)
        {
            var url = string.Format("http://{0}/Pages/MonitorCheck.aspx?RID={1}&URL=http://localhost/Pages/UnionCheckingActivityList.aspx", hostIP, RecordID);
            Response.Redirect(url, true);
        }

        private void GotoView(string hostIP)
        {
            var url = string.Format("http://{0}/Pages/ActivityInfo.aspx?RID={1}&URL=http://localhost/Pages/UnionCheckingActivityList.aspx", hostIP, RecordID);
            Response.Redirect(url, true);
        }

        private void GotoUnionChecking(string msg)
        {
            var url = "http://localhost/Pages/UnionCheckingActivityList.aspx";
            var jsBlock = string.Format("<script>alert('{0}'); window.open('{1}', '_self');</script>", msg, url);
            Response.Write(jsBlock);
            Response.Flush();
        }
    }
}
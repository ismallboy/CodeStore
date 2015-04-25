using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;

namespace Amway.OA.MSET.Web.WebService
{
    /// <summary>
    /// GetETicketQRCode 的摘要说明
    /// </summary>
    public class GetETicketQRCode : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var code = HttpContext.Current.Request["Code"].ToNullString();
            var codeParams = CodeDescriptor.Init(code, ErrorCorrectionLevel.M, QuietZoneModules.Zero, 8);

            // 编码内容
            if (codeParams == null || !codeParams.TryEncode())
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return;
            }

            // 将转码读到流中输出
            using (var ms = new MemoryStream())
            {
                codeParams.Render(ms);
                context.Response.ContentType = codeParams.ContentType;
                context.Response.OutputStream.Write(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    /// <summary>
    /// 封装生成二维码图像
    /// </summary>
    internal class CodeDescriptor
    {
        public ErrorCorrectionLevel Ecl;
        public string Content;
        /// <summary>
        /// 边距（Marge）
        /// </summary>
        public QuietZoneModules QuietZones;
        /// <summary>
        /// 生成图像大小
        /// </summary>
        public int ModuleSize;
        public BitMatrix Matrix;
        public string ContentType;

        /// <summary>
        /// 初始化生成器参数
        /// </summary>
        /// <param name="code">码内容</param>
        /// <param name="ecl">容错级别</param>
        /// <param name="quietZones">左、上边距</param>
        /// <param name="moduleSize">模型大小</param>
        /// <returns></returns>
        public static CodeDescriptor Init(string code, ErrorCorrectionLevel ecl, QuietZoneModules quietZones, int moduleSize)
        {
            var cp = new CodeDescriptor();

            cp.Ecl = ecl;
            cp.Content = code;
            cp.QuietZones = quietZones;
            cp.ModuleSize = moduleSize;

            return cp;
        }

        /// <summary>
        /// 内容转码，并保存图像
        /// </summary>
        /// <returns></returns>
        public bool TryEncode()
        {
            var encoder = new QrEncoder(Ecl);
            QrCode qr;
            if (!encoder.TryEncode(Content, out qr))
                return false;

            Matrix = qr.Matrix;
            return true;
        }

        /// <summary>
        /// 填充图像，并读到流中
        /// </summary>
        /// <param name="ms"></param>
        internal void Render(MemoryStream ms)
        {
            var render = new GraphicsRenderer(new FixedModuleSize(ModuleSize, QuietZones));
            render.WriteToStream(Matrix, ImageFormat.Png, ms);
            ContentType = "image/png";
        }
    }
}
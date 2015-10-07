using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.ETOffine.Entities
{
    /// <summary>
    /// 是否实名制 0非实名，1实名
    /// </summary>
    public enum IsRealName : int 
    {
        /// <summary>
        /// 非实名
        /// </summary>
        N = 0,
        /// <summary>
        /// 实名
        /// </summary>
        Y = 1
    }

    /// <summary>
    /// 验票状态：1已下载， 5未上传，8已上传 
    /// </summary>
    public enum ActivityCheckingStatus : int 
    {
        /// <summary>
        /// 已下载
        /// </summary>
        Download = 1,
        /// <summary>
        /// 未上传
        /// </summary>
        UnUpload = 5,
        /// <summary>
        /// 已上传
        /// </summary>
        HadUpload = 8
    }

    /// <summary>
    /// 验票状态：1=正常,2=已使用,3=无此票，9=异常
    /// </summary>
    public enum CheckingStatus : int
    {
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        /// <summary>
        /// 已使用
        /// </summary>
        Used = 2,
        /// <summary>
        /// 无此票
        /// </summary>
        None = 3,
        /// <summary>
        /// 异常
        /// </summary>
        Error = 9
    }

    /// <summary>
    /// 门票状态：0 未验票，5已验票，8已取消，9已过期, 
    /// </summary>
    public enum TicketStatus : int
    {
        /// <summary>
        /// 未验票
        /// </summary>
        UnChecked = 0,
        /// <summary>
        /// 已验票
        /// </summary>
        Checked = 5,
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 8,
        /// <summary>
        /// 已过期
        /// </summary>
        Expired = 9
    }

    public enum SyncStatus : int
    {
        /// <summary>
        /// 已同步
        /// </summary>
        Synced=0,

        /// <summary>
        /// 未同步
        /// </summary>
        UnSync=1
    }
}

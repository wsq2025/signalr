using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalR.Common
{
    /// <summary>
    /// 向客户端通讯类
    /// </summary>
    internal class ProgressiveJobInterface
    {
        private static readonly ConcurrentDictionary<string, string> _loginNameConnId = new ConcurrentDictionary<string, string>();

        private ProgressiveJobInterface()
        {

        }

        public static void OnConnected(string loginName, string connId)
        {
            _loginNameConnId.AddOrUpdate(loginName, connId, (key, oldValue) => connId);
        }

        public static void OnDisconnected(string loginName)
        {
            string value;
            _loginNameConnId.TryRemove(loginName, out value);
        }

        /// <summary>
        /// 通知客户端添加或者更新任务
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <param name="progressiveJob"></param>
        public static void AddorUpdate(string userLoginName, ProgressiveJob progressiveJob)
        {
            if (_loginNameConnId.ContainsKey(userLoginName))
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressiveJobHub>();
                hubContext.Clients.Client(_loginNameConnId[userLoginName]).addorUpdate(progressiveJob);
            }
        }

        /// <summary>
        /// 通知客户端任务完成
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <param name="progressiveJob"></param>
        public static void Finish(string userLoginName, ProgressiveJob progressiveJob)
        {
            if (_loginNameConnId.ContainsKey(userLoginName))
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressiveJobHub>();
                hubContext.Clients.Client(_loginNameConnId[userLoginName]).finish(progressiveJob);
            }
        }

        /// <summary>

        /// 通知客户端，服务端发生异常
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <param name="progressiveJob"></param>
        /// <param name="msg"></param>
        /// <param name="stackTrace"></param>
        public static void Exception(string userLoginName, ProgressiveJob progressiveJob, String msg, String stackTrace)
        {
            if (_loginNameConnId.ContainsKey(userLoginName))
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressiveJobHub>();
                hubContext.Clients.Client(_loginNameConnId[userLoginName]).onException(progressiveJob, msg, stackTrace);
            }
        }

        /// <summary>
        /// 初始化客户端任务
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <param name="progressiveJobs"></param>
        public static void Inital(string userLoginName, List<ProgressiveJob> progressiveJobs)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressiveJobHub>();
            hubContext.Clients.Client(_loginNameConnId[userLoginName]).inital(progressiveJobs);
        }

        /// <summary>
        /// 通知客户端任务取消了
        /// </summary>
        /// <param name="userLoginName"></param>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        public static void Cancel(string userLoginName, string id, string msg)
        {
            if (_loginNameConnId.ContainsKey(userLoginName))
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressiveJobHub>();
                hubContext.Clients.Client(_loginNameConnId[userLoginName]).cancel(id, msg);
            }
        }
    }
}
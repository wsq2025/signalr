using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using SignalR.Logic;

namespace SignalR.Common
{
    /// <summary>
    /// 任务消息管道
    /// </summary>
    public class ProgressiveJobHub : Hub
    {
        private string credentials;
        public override Task OnConnected()
        {
            var loginName = HttpContext.Current.Request.Cookies["LastLoginname"].Value; //验证信息
            ProgressiveJobInterface.OnConnected(string.IsNullOrEmpty(credentials) ? loginName : credentials, Context.ConnectionId);   //存储验证信息
            //ProgressiveJobInterface.Inital(loginName, ProgressiveJobManager.progressiveJobs(loginName)); //初始化当前进度任务
            return base.OnConnected();
        }

        //接收客户端发送的消息，执行任务
        public void Send(string name)
        {
            credentials = name;
            var job = new TestProgressveNewJob(name);
            ProgressiveJobManager.Start(job, null);
        }

        //接收客户端发送的消息，继续执行任务
        public void ReSend(string name)
        {
            credentials = name;
            OnConnected();
        }

        //接收客户端发送的消息，重新执行任务
        public void BeSend(string name)
        {
            credentials = name;
            OnConnected();
            Send(name);
        }

        //接收客户端发送的消息，取消执行任务
        public void Stop(string name)
        {
            ProgressiveJobInterface.OnDisconnected(name);
            OnDisconnected();
        }

        //接收客户端发送的消息，取消执行任务
        public void Cancel(string name)
        {
            Stop(name);
            Dispose(true);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected()
        {
            return base.OnDisconnected();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
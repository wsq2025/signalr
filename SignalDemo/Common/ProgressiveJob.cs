using System;
using System.Threading;
using System.Web;

namespace SignalR.Common
{
    public abstract class ProgressiveJob
    {
        /// <summary>
        /// 任务最小粒度，基类
        /// </summary>
        public ProgressiveJob(string name)
        {
            Id = Guid.NewGuid().ToString().ToUpper();
            if (HttpContext.Current != null)
            {
                //SessionId = HttpContext.Current.Session.SessionID;
                UserLoginName = name;
                SessionId = Guid.NewGuid().ToString().ToUpper();
            }
        }

        public string Id { get; set; }

        public String SessionId { get; set; }

        public String UserLoginName { get; set; }

        public abstract String Title { get; }

        public int Progress { get; set; }

        public abstract Unique Unique { get; }

        /// <summary>
        /// 执行任务
        /// </summary>
        public abstract void Execute(object obj);

        /// <summary>
        /// 任务进度说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 更新任务进度
        /// </summary>
        public void UpdateProgress(int progress, string description)
        {
            Progress = progress;
            Description = description;
            ProgressiveJobInterface.AddorUpdate(UserLoginName, this);

            if (progress == 100)
            {
                //任务完成后还显示3秒
                ProgressiveJobInterface.Finish(UserLoginName, this);
                Thread.Sleep(3000);
                ProgressiveJobManager.Remove(this);
            }
        }
    }
}
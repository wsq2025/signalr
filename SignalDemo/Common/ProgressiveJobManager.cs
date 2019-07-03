using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SignalR.Common
{
    public class ProgressiveJobManager
    {
        /// <summary>
        /// 任务进度管理
        /// </summary>
        private static readonly List<ProgressiveJob> _progressiveJobs = new List<ProgressiveJob>();


        /// <summary>
        /// 任务进度加入线程池
        /// </summary>
        public static void Start(ProgressiveJob progressiveJob, Object args)
        {
            if (progressiveJob == null)
                return;

            //if (progressiveJob.Unique == Unique.Global)
            //{
            //    if (_progressiveJobs.Any(job => job.GetType() == progressiveJob.GetType()))
            //    {
            //        throw new Exception("已有一个同一类型的全局任务正在执行。");
            //    }
            //}

            //if (progressiveJob.Unique == Unique.Session)
            //{
            //    if (_progressiveJobs.Any(job => job.GetType() == progressiveJob.GetType() && job.SessionId.Equals(progressiveJob.SessionId)))
            //    {
            //        throw new Exception("你已有一个同一类型的任务正在执行。");
            //    }
            //}
            if (!_progressiveJobs.Contains(progressiveJob))
                _progressiveJobs.Add(progressiveJob);

            //var sessionThreadStarter = new SessionThreadStarter(progressiveJob.Execute) { SessionId = progressiveJob.SessionId };
            var thread = new Thread(progressiveJob.Execute);
            thread.IsBackground = true;
            thread.Start(args);
        }

        public static void Remove(ProgressiveJob progressiveJob)
        {
            _progressiveJobs.Remove(progressiveJob);
        }

        public static List<ProgressiveJob> progressiveJobs(string loginName)
        {
            List<ProgressiveJob> _jobs = new List<ProgressiveJob>();
            foreach (var p in _progressiveJobs)
            {
                if (p.UserLoginName == loginName)
                {
                    _jobs.Add(p);
                }
            }
            return _jobs;
        }
    }

    /// <summary>
    /// 任务类型
    /// </summary>
    public enum Unique
    {
        Global,

        Session,

        Request,

        None
    }
}
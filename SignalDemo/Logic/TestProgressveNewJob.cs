using SignalR.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace SignalR.Logic
{
    public class TestProgressveNewJob : ProgressiveJob
    {
        public TestProgressveNewJob(string name) : base(name)
        {

        }

        public override string Title
        {
            get
            {
                return "测试";
            }
        }

        public override Unique Unique
        {
            get
            {
                return Unique.Global;
            }
        }

        public override void Execute(object obj)
        {
            for (int i = 0; i < 10; i++)
            {
                UpdateProgress(i * 10, $"正在执行任务【{i + 1}】。。。。");
                Thread.Sleep(3000);
                UpdateProgress((i + 1) * 10, $"任务【{i + 1}】执行完毕");
            }
        }
    }
}
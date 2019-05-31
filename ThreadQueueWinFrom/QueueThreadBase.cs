using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

//namespace ThreadQueueWinFrom
//{
//    class QueueThreadBase
//    {
//    }
//}



namespace ThreadQueueWinFrom
{

    public class DownLoadFile
    {
        public string FileName { get; set; }

    }


    public class QueueThreadBase
    {
        #region 变量&属性

        /// 待处理结果

        private class PendingResult
        {
            /// 待处理值
            public DownLoadFile PendingValue { get; set; }

            /// 是否有值
            public bool IsHad { get; set; }
        }

        /// 线程数
        public int ThreadCount
        {
            get { return this.m_ThreadCount; }
            set { this.m_ThreadCount = value; }
        }
        private int m_ThreadCount = 5;

        /// 取消=True
        public bool Cancel { get; set; }

        /// 线程列表
        List<Thread> m_ThreadList;

        /// 完成队列个数
        private volatile int m_CompletedCount = 0;

        /// 队列总数
        private int m_QueueCount = 0;

        /// 全部完成锁
        private object m_AllCompletedLock = new object();

        /// 完成的线程数

        private int m_CompetedCount = 0;

        /// 队列锁

        private object m_PendingQueueLock = new object();
        private Queue<DownLoadFile> m_InnerQueue; //--内部队列..
                                                  //public DownLoadFile Peek()
                                                  //{
                                                  //    return m_InnerQueue.Dequeue();
                                                  //}
                                                  //public void AddQueue(DownLoadFile ff)
                                                  //{
                                                  //    try
                                                  //    {
                                                  //        m_InnerQueue.Enqueue(ff);
                                                  //        //this.m_QueueCount = m_InnerQueue.Count;
                                                  //    }
                                                  //    catch
                                                  //    {
                                                  //        throw;
                                                  //    }
                                                  //}
        #endregion

        #region 事件相关
        //---开始一个任务的事件...
        public event Action<DownLoadFile> OneJobStart;
        private void OnOneJobStart(DownLoadFile pendingValue)
        {
            if (OneJobStart != null)
            {
                try
                {
                    //MessageBox.Show("所有任务完成！");
                    OneJobStart(pendingValue);//--一个任务开始了..
                }
                catch { }
            }

        }



        /// 全部完成事件
        public event Action<CompetedEventArgs> AllCompleted;
        /// 单个完成事件
        public event Action<DownLoadFile, CompetedEventArgs> OneCompleted;
        /// 引发全部完成事件
        private void OnAllCompleted(CompetedEventArgs args)
        {
            if (AllCompleted != null)
            {
                try
                {
                    //MessageBox.Show("所有任务完成！");
                    AllCompleted(args);//全部完成事件
                }
                catch { }
            }
        }

        /// 引发单个完成事件
        private void OnOneCompleted(DownLoadFile pendingValue, CompetedEventArgs args)
        {
            if (OneCompleted != null)
            {
                try
                {
                    //MessageBox.Show("单个任务完成！");
                    OneCompleted(pendingValue, args);
                }
                catch { }
            }
        }
        #endregion

        #region 构造

        //public QueueThreadBase(IEnumerable<T> collection)
        //{
        //    m_InnerQueue = new Queue<T>(collection);
        //    this.m_QueueCount = m_InnerQueue.Count;
        //}

        public QueueThreadBase(IEnumerable<DownLoadFile> collection)
        {
            m_InnerQueue = new Queue<DownLoadFile>(collection);
            this.m_QueueCount = m_InnerQueue.Count;
        }

        //--- 无参数的构造函数，需要向队列中填充元素...
        public QueueThreadBase()
        {
            m_InnerQueue = new Queue<DownLoadFile>();
            this.m_QueueCount = m_InnerQueue.Count;
        }

        #endregion

        #region 主体

        /// 初始化线程
        private void InitThread()
        {
            m_ThreadList = new List<Thread>();

            for (int i = 0; i < 1; i++)
            {
                Thread t = new Thread(new ThreadStart(InnerDoWork));
                m_ThreadList.Add(t);
                t.IsBackground = true;
                t.Start();
            }
        }

        /// 开始
        public void Start()
        {
            InitThread();
        }

        /// 线程工作
        private void InnerDoWork()
        {
            try
            {
                Exception doWorkEx = null;
                DoWorkResult doworkResult = DoWorkResult.ContinueThread;
                var t = CurrentPendingQueue;
                OnOneJobStart(t.PendingValue);

                while (!this.Cancel && t.IsHad)
                {
                    try
                    {
                        doworkResult = DoWork(t.PendingValue);
                    }
                    catch (Exception ex)
                    {
                        doWorkEx = ex;
                    }

                    m_CompletedCount++;
                    int precent = m_CompletedCount * 100 / m_QueueCount;
                    OnOneCompleted(t.PendingValue, new CompetedEventArgs() { CompetedPrecent = precent, InnerException = doWorkEx });

                    if (doworkResult == DoWorkResult.AbortAllThread)
                    {
                        this.Cancel = true;
                        break;
                    }
                    else if (doworkResult == DoWorkResult.AbortCurrentThread)
                    {
                        break;
                    }

                    t = CurrentPendingQueue;
                }

                lock (m_AllCompletedLock)
                {
                    m_CompetedCount++;

                    if (m_CompetedCount == m_ThreadList.Count)
                    {
                        OnAllCompleted(new CompetedEventArgs() { CompetedPrecent = 100 });
                    }
                }
            }
            catch
            {
                throw;
            }
        }


        protected DoWorkResult DoWork(DownLoadFile pendingValue)
        {
            try
            {
                string jna = pendingValue.FileName;
                //MessageBox.Show("正在执行任务" + jna);
                //--- 这里如何通知主界面，任务正在执行...
                //for(int i = 0; i < 5; i++)
                //{
                //    Console.WriteLine("任务名：{0} 正在执行第{1}次", jna, i);
                //}
                //..........多线程处理....
                return DoWorkResult.ContinueThread;//没有异常让线程继续跑..
            }
            catch (Exception)
            {
                return DoWorkResult.AbortCurrentThread;//有异常,可以终止当前线程.当然.也可以继续,
                                                       //return  DoWorkResult.AbortAllThread; //特殊情况下 ,有异常终止所有的线程...
            }
        }

        /// 获取当前结果
        private PendingResult CurrentPendingQueue
        {
            get
            {
                lock (m_PendingQueueLock)
                {
                    PendingResult t = new PendingResult();

                    if (m_InnerQueue.Count != 0)
                    {
                        t.PendingValue = m_InnerQueue.Dequeue();
                        t.IsHad = true;
                    }
                    else
                    {
                        t.PendingValue = default(DownLoadFile);
                        t.IsHad = false;
                    }

                    return t;
                }
            }
        }

        #endregion


    }

    #region 相关类&枚举

    /// dowork结果枚举
    public enum DoWorkResult
    {
        /// 继续运行，默认
        ContinueThread = 0,
        /// 终止当前线程
        AbortCurrentThread = 1,
        /// 终止全部线程
        AbortAllThread = 2
    }

    /// 完成事件数据
    public class CompetedEventArgs : EventArgs
    {
        public CompetedEventArgs()
        {
        }
        /// 完成百分率
        public int CompetedPrecent { get; set; }
        /// 异常信息
        public Exception InnerException { get; set; }
    }
    #endregion

}
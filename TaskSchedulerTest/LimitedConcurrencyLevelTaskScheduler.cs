using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSchedulerTest
{
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        /// <summary>Whether the current thread is processing work items.</summary> 
        /// 判断当前线程是否正在处理work item
        [ThreadStatic]
        private static bool _currentThreadIsProcessingItems;
        /// <summary>待执行的线程队列.</summary> 
        private readonly LinkedList<Task> _tasks = new LinkedList<Task>(); // protected by lock(_tasks) 
        /// <summary>调度允许的最大线程并发数量.</summary> 
        private readonly int _maxDegreeOfParallelism;
        /// <summary>Whether the scheduler is currently processing work items.</summary> 
        private int _delegatesQueuedOrRunning = 0; // protected by lock(_tasks) 

        /// <summary> 
        /// Initializes an instance of the LimitedConcurrencyLevelTaskScheduler class with the 
        /// specified degree of parallelism. 
        /// 使用指定的并发数量初始化limitedConcurrencyLevelTaskScheduler类的实例。
        /// </summary> 
        /// <param name="maxDegreeOfParallelism">此调度程序提供的最大并行度</param> 
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1) 
                throw new ArgumentOutOfRangeException("maxDegreeOfParallelism");
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        /// current executing number;
        /// </summary>
        public int CurrentCount { get; set; }

        /// <summary>System.Threading.Tasks.Task 排队到计划程序中。</summary> 
        /// <param name="task">The task to be queued.</param> 
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed. If there aren't enough 
            // delegates currently queued or running to process tasks, schedule another. 
            //将任务添加到要处理的任务列表中。 如果当前没有足够的代理队列或运行来处理任务，请安排另一个代理。
            lock (_tasks)
            {
                Console.WriteLine("Task Count : {0} ", _tasks.Count);
                _tasks.AddLast(task);
                if (_delegatesQueuedOrRunning < _maxDegreeOfParallelism)
                {
                    ++_delegatesQueuedOrRunning;
                    NotifyThreadPoolOfPendingWork();
                }
            }
        }
        int executingCount = 0;
        private static object executeLock = new object();
        /// <summary> 
        /// Informs the ThreadPool that there's work to be executed for this scheduler. 
        /// 通知ThreadPool要为此调度程序执行工作
        /// </summary> 
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.UnsafeQueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items. 
                // This is necessary to enable inlining of tasks into this thread. 
                // 当前线程现在正在处理工作项。这对于在此线程中启用内联任务是必要的。
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue. 
                    // 循环执行队列中的所有可执行的item。
                    while (true)
                    {
                        Task item;
                        lock (_tasks)
                        {
                            // When there are no more items to be processed, 
                            // note that we're done processing, and get out. 
                            // 当没有其他item需要处理时，提示我们已完成处理，然后退出。
                            if (_tasks.Count == 0)
                            {
                                --_delegatesQueuedOrRunning;

                                break;
                            }

                            // 从线程队列中获取下一个待执行的Task
                            item = _tasks.First.Value;
                            _tasks.RemoveFirst();
                        }


                        // 执行这个从线程队列中取出来的Task 
                        base.TryExecuteTask(item);
                    }
                }
                // We're done processing items on the current thread 
                // 我们在当前线程上完成了执行item
                finally { _currentThreadIsProcessingItems = false; }
            }, null);
        }

        /// <summary>尝试在当前线程上执行指定的任务</summary> 
        /// <param name="task">The task to be executed.</param> 
        /// <param name="taskWasPreviouslyQueued"></param> 
        /// <returns>Whether the task could be executed on the current thread.</returns> 
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {

            // If this thread isn't already processing a task, we don't support inlining 
            if (!_currentThreadIsProcessingItems) 
                return false;

            // If the task was previously queued, remove it from the queue 
            //如果任务先前已排队，将其从队列中删除
            if (taskWasPreviouslyQueued) 
                TryDequeue(task);

            // 尝试执行这个Task
            return base.TryExecuteTask(task);
        }

        /// <summary>
        /// Attempts to remove a previously scheduled task from the scheduler.
        /// 尝试从调度程序中删除以前计划的任务
        /// </summary> 
        /// <param name="task">The task to be removed.</param> 
        /// <returns>Whether the task could be found and removed.</returns> 
        protected sealed override bool TryDequeue(Task task)
        {
            lock (_tasks) 
                return _tasks.Remove(task);
        }

        /// <summary>
        /// Gets the maximum concurrency level supported by this scheduler.
        /// 获取此调度程序支持的最大并发数量。
        /// </summary> 
        public sealed override int MaximumConcurrencyLevel 
        { 
            get 
            { 
                return _maxDegreeOfParallelism; 
            } 
        }

        /// <summary>Gets an enumerable of the tasks currently scheduled on this scheduler.</summary> 
        /// <returns>An enumerable of the tasks currently scheduled.</returns> 
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(_tasks, ref lockTaken);
                if (lockTaken) 
                    return _tasks.ToArray();
                else 
                    throw new NotSupportedException();
            }
            finally
            {
                if (lockTaken) 
                    Monitor.Exit(_tasks);
            }
        }
    }
}

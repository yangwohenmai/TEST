using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper.Net.Patterns.Chainable;

namespace Dapper.Net.Extensions {

    public static class EnumerableExtensions {
        public static DataTable ToDataTable<T>(this IEnumerable<T> items) {
            var tb = new DataTable(typeof (T).Name);
            var props = typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props) {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }
            foreach (var item in items) {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++) {
                    values[i] = props[i].GetValue(item, null);
                }
                tb.Rows.Add(values);
            }
            return tb;
        }


        public static int Count(this IEnumerable source) {
            var col = source as ICollection;
            if (col != null) return col.Count;
            var c = 0;
            var e = source.GetEnumerator();
            DynamicUsing(e, () => { while (e.MoveNext()) c++; });
            return c;
        }

        public static void DynamicUsing(object resource, Action action) {
            try {
                action();
            } finally {
                var d = resource as IDisposable;
                d?.Dispose();
            }
        }

        public static Args ToArgs(this IEnumerable<string> enumerable) => new Args(enumerable);

        public static ArgsMap ToArgsMap(this IEnumerable<KeyValuePair<string, string>> enumerable) {
            return new ArgsMap(enumerable.ToDictionary(k => k.Key, v => v.Value));
        }

        public static void RunTasks(this IEnumerable<Task> tasks, int maxConcurrency, Action<Task> taskComplete = null) {
            if (maxConcurrency <= 0) throw new ArgumentException("maxConcurrency must be more than 0.");
            var taskCount = 0;
            var nextIndex = 0;
            var currentTasks = new Task[maxConcurrency];

            foreach (var task in tasks) {
                currentTasks[nextIndex] = task;
                taskCount++;
                if (taskCount == maxConcurrency) {
                    nextIndex = Task.WaitAny(currentTasks);
                    taskComplete?.Invoke(currentTasks[nextIndex]);
                    currentTasks[nextIndex] = null;
                    taskCount--;
                } else nextIndex++;
            }

            while (taskCount > 0) {
                currentTasks = currentTasks.Where(t => t != null).ToArray();
                nextIndex = Task.WaitAny(currentTasks);
                taskComplete?.Invoke(currentTasks[nextIndex]);
                currentTasks[nextIndex] = null;
                taskCount--;
            }
        }

        public static void RunTasks<T>(this IEnumerable<Task<T>> tasks, int maxConcurrency, Action<Task<T>> taskComplete = null) {
            if (maxConcurrency <= 0) throw new ArgumentException("maxConcurrency must be more than 0.");
            var taskCount = 0;
            var nextIndex = 0;
            var currentTasks = new Task[maxConcurrency];

            foreach (var task in tasks) {
                currentTasks[nextIndex] = task;
                taskCount++;
                if (taskCount == maxConcurrency) {
                    nextIndex = Task.WaitAny(currentTasks);
                    taskComplete?.Invoke((Task<T>) currentTasks[nextIndex]);
                    currentTasks[nextIndex] = null;
                    taskCount--;
                } else nextIndex++;
            }

            while (taskCount > 0) {
                currentTasks = currentTasks.Where(t => t != null).ToArray();
                nextIndex = Task.WaitAny(currentTasks);
                taskComplete?.Invoke((Task<T>) currentTasks[nextIndex]);
                currentTasks[nextIndex] = null;
                taskCount--;
            }
        }
    }

}
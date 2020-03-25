using System;
using System.Collections.Generic;
using System.Text;

namespace Graph
{
    class Graph_3
    {
        /// <summary>
        /// 图的广度遍历
        /// </summary>
        static void Main(string[] args)
        {
            // 初始化图
            SimpleGraph example_graph = new SimpleGraph();

            #region 全连接图
            //char[] A = { 'B','C' };
            //char[] B = { 'A', 'E', 'D' };
            //char[] C = { 'A', 'F', 'G' };
            //char[] D = { 'E', 'B' };
            //char[] E = { 'B','H','D' };
            //char[] F = { 'H', 'C', 'G' };
            //char[] G = { 'F', 'C' };
            //char[] H = { 'E', 'F' };
            #endregion

            #region 有向图 Key->value表示 当前节点 可到达的 相邻节点
            List<string> A = new List<string>{ "B" };
            List<string> B = new List<string>{ };
            List<string> C = new List<string>{ "A", "F", "G" };
            List<string> D = new List<string>{ };
            List<string> E = new List<string>{ "B", "D" };
            List<string> F = new List<string>{ "H" };
            List<string> G = new List<string>{ };
            List<string> H = new List<string>{ "E" };
            #endregion

            example_graph.edges.Add("A", A);
            example_graph.edges.Add("B", B);
            example_graph.edges.Add("C", C);
            example_graph.edges.Add("D", D);
            example_graph.edges.Add("E", E);
            example_graph.edges.Add("F", F);
            example_graph.edges.Add("G", G);
            example_graph.edges.Add("H", H);


            string input = Console.ReadLine();
            breadthFirstSearch(example_graph, input);
            // 防止退出
            Console.ReadKey();
        }

        private static void breadthFirstSearch(SimpleGraph graph, string start)
        {
            // 初始化队列
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(start);
            Dictionary<string, bool> visited = new Dictionary<string, bool>();
            visited[start] = true;

            while (queue.Count != 0)
            {
                string current = queue.Dequeue();
                Console.WriteLine("当前访问节点: " + current);
                foreach (var next in graph.neighbors(current))
                {
                    if (!visited.ContainsKey(next))
                    {
                        queue.Enqueue(next);
                        visited[next] = true;
                    }
                }
            }
        }
    }

    #region 定义图的结构
    class SimpleGraph
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SimpleGraph()
        {
            // 初始化边表
            edges = new SortedDictionary<string, List<string>>();
        }

        public SortedDictionary<string, List<string>> edges;

        /// <summary>
        /// 记录 当前点 与 相邻节点 之间连接关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<string> neighbors(string id)
        {
            return edges[id];
        }
    }
    #endregion

}

using System;
using System.Collections.Generic;
using System.Text;

namespace Graph
{

    class Graph_4
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            int e;
            int n;
            Console.Write("请输入图的类型： 0 为无向图， 1 为有向图  ");
            GraphType t = (GraphType)(int.Parse(Console.ReadLine()));
            Console.Write("请输入图的顶点数 : ");
            e = int.Parse(Console.ReadLine());
            Console.Write("请输入图的边数 : ");
            n = int.Parse(Console.ReadLine());
            ALGraph G = new ALGraph(e, n, t);
            //构建一个有向图结构
            (new GraphOperation()).CreateALGraph(G);
            Console.Write("\n ");
            (new GraphOperation()).DFSTraverse(G);
            Console.Write("\n ");
            (new GraphOperation()).BFSTraverse(G);
            Console.Write("\n ");
            (new GraphOperation()).NonPreFirstTopSort(G);

            Console.ReadLine();

        }
    }

    //边表结点类
    public class EdgeNode
    {
        public int adjvex;
        public EdgeNode next;

        public EdgeNode()
        {
            next = null;
        }
    }

    ////顶点表结点类
    public class VertexNode
    {
        public object vertex;
        public EdgeNode firstedge;

        public VertexNode()
        {
            firstedge = null;
        }
    }

    //图的类型
    public enum GraphType
    {
        UnDiGraph = 0,//无向图 
        DiGraph = 1//有向图
    }

    //图类
    public class ALGraph
    {
        public VertexNode[] AdjList;//顶点表
        public int VertexNodeCount;//顶点数
        public int BordeCount;//边数
        public GraphType theType;//图的类型

        public ALGraph(int n, int e, GraphType cuType)
        {

            VertexNodeCount = n;
            BordeCount = e;
            AdjList = new VertexNode[n];
            theType = cuType;
        }
    }

    //图对象操作类
    public class GraphOperation
    {
        private bool[] visited;
        private StringBuilder sb;

        public GraphOperation()
        {

        }

        /// <summary>
        /// 建全一个有向图对象
        /// (不是建立 ^_^)
        /// </summary>
        /// <param name="G"></param>
        public void CreateALGraph(ALGraph G)
        {
            int i, j, k;
            EdgeNode s;
            for (int vnc = 0; vnc < G.VertexNodeCount; vnc++)
            {
                VertexNode cuVN = new VertexNode();
                Console.Write("请输入顶点的编号 : ");
                cuVN.vertex = Console.ReadLine();
                cuVN.firstedge = null;
                G.AdjList[vnc] = cuVN;
            }

            for (k = 0; k < G.BordeCount; k++)
            {
                Console.Write("请输入边开始顶点的序号 : ");
                i = int.Parse(Console.ReadLine())-1;
                Console.Write("请输入边结束顶点的序号 : ");
                j = int.Parse(Console.ReadLine())-1;
                s = new EdgeNode();
                s.adjvex = j;
                s.next = G.AdjList[i].firstedge;
                //邻接表头插法
                G.AdjList[i].firstedge = s;
                //建立无向图
                if (G.theType == GraphType.UnDiGraph)
                {
                    s = new EdgeNode();
                    s.adjvex = i;
                    s.next = G.AdjList[j].firstedge;
                    G.AdjList[j].firstedge = s;
                }
            }
        }

        /// <summary>
        /// 深度优先遍历以邻接表表示的图G.
        /// </summary>
        /// <param name="G"></param>
        public void DFSTraverse(ALGraph G)
        {
            visited = new bool[G.VertexNodeCount];
            sb = new StringBuilder();
            for (int i = 0; i < G.VertexNodeCount; i++)
            {
                visited[i] = false;

            }
            for (int i = 0; i < G.VertexNodeCount; i++)
            {
                if (!visited[i])
                {
                    DFS(G, i);
                }
            }
            Console.Write("深度优先遍历以邻接表表示的图G 的结果:\n ");
            Console.Write(sb);
        }

        private void DFS(ALGraph G, int i)
        {
            EdgeNode p;
            sb.Append(G.AdjList[i].vertex.ToString() + "--->");

            visited[i] = true;
            p = G.AdjList[i].firstedge;
            while (p != null)
            {
                if (!visited[p.adjvex])
                {
                    DFS(G, p.adjvex);//递归
                }
                p = p.next;
            }
        }

        /// <summary>
        /// 广度优先遍历以邻接表表示的图G
        /// </summary>
        /// <param name="G"></param>
        public void BFSTraverse(ALGraph G)
        {
            visited = new bool[G.VertexNodeCount];
            sb = new StringBuilder();
            for (int i = 0; i < G.VertexNodeCount; i++)
            {
                visited[i] = false;

            }

            for (int i = 0; i < G.VertexNodeCount; i++)
            {
                if (!visited[i])
                {
                    BFS(G, i);
                }
            }
            Console.Write("广度优先遍历以邻接表表示的图G 的结果:\n ");
            Console.Write(sb);
        }
        private void BFS(ALGraph G, int k)
        {
            int i;
            Queue<int> Q;
            EdgeNode p;
            Q = new Queue<int>();
            sb.Append(G.AdjList[k].vertex.ToString() + "--->");
            visited[k] = true;
            Q.Enqueue(k);
            while (Q.Count != 0)
            {
                i = (int)Q.Dequeue();
                p = G.AdjList[i].firstedge;
                while (p != null)
                {
                    if (!visited[p.adjvex])
                    {
                        sb.Append(G.AdjList[p.adjvex].vertex.ToString() + "--->");
                        visited[p.adjvex] = true;
                        Q.Enqueue(p.adjvex);
                    }
                    p = p.next;
                }
            }
        }

        /// <summary>
        /// 有向图无前驱的顶点优先之拓扑排序
        /// </summary>
        /// <param name="G"></param>
        public void NonPreFirstTopSort(ALGraph G)
        {
            if (G.theType == GraphType.UnDiGraph)
            {
                Console.Write("对无向图进行拓扑排序是不对的!");
                return;
            }
            StringBuilder SB = new StringBuilder();
            int[] indegree = new int[G.VertexNodeCount];
            Stack<int> S;
            int i, j, count = 0;
            EdgeNode p;
            for (i = 0; i < G.VertexNodeCount; i++)
            {
                indegree[i] = 0;
            }

            for (i = 0; i < G.VertexNodeCount; i++)
            {
                for (p = G.AdjList[i].firstedge; p != null; p = p.next)
                {
                    indegree[p.adjvex]++;
                }
            }

            S = new Stack<int>();

            for (i = 0; i < G.VertexNodeCount; i++)
            {
                if (indegree[i] == 0)
                {
                    S.Push(i);
                }
            }

            while (S.Count != 0)
            {
                i = (int)S.Pop();
                SB.Append(G.AdjList[i].vertex.ToString() + "--->");
                count++;

                for (p = G.AdjList[i].firstedge; p != null; p = p.next)
                {
                    j = p.adjvex;
                    indegree[j]--;
                    if (indegree[j] == 0)
                    {
                        S.Push(j);
                    }
                }
            }
            if (count < G.VertexNodeCount)
            {
                Console.Write("图中有环,排序失败");
            }
            else
            {
                Console.Write("拓扑排序结果如下 : \n" + SB);
            }
        }
    }
}

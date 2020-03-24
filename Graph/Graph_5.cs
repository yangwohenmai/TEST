using System;
using System.Collections.Generic;
using System.Text;

namespace Graph
{
    /// <summary>
    /// 图的 深度遍历 和 广度遍历
    /// </summary>
    class Graph_5
    {
        static void Main(string[] args)
        {
            #region 显示节点的结构
            //AdjacencyList<char> a = new AdjacencyList<char>();
            ////添加顶点
            //a.AddVertex('A');
            //a.AddVertex('B');
            //a.AddVertex('C');
            //a.AddVertex('D');
            ////添加边
            //a.AddEdge('A', 'B');
            //a.AddEdge('A', 'C');
            //a.AddEdge('A', 'D');
            //a.AddEdge('B', 'D');
            //Console.WriteLine(a.ToString());
            #endregion

            #region 深度优先遍历
            //AdjacencyList<string> D = new AdjacencyList<string>();
            //D.AddVertex("V1");
            //D.AddVertex("V2");
            //D.AddVertex("V3");
            //D.AddVertex("V4");
            //D.AddVertex("V5");
            //D.AddVertex("V6");
            //D.AddVertex("V7");
            //D.AddVertex("V8");
            //D.AddEdge("V1", "V2");
            //D.AddEdge("V1", "V3");
            //D.AddEdge("V2", "V4");
            //D.AddEdge("V2", "V5");
            //D.AddEdge("V3", "V6");
            //D.AddEdge("V3", "V7");
            //D.AddEdge("V4", "V8");
            //D.AddEdge("V5", "V8");
            //D.AddEdge("V6", "V8");
            //D.AddEdge("V7", "V8");
            //D.DFSTraverse();
            #endregion

            #region 广度优先遍历
            AdjacencyList<string> B = new AdjacencyList<string>();
            B.AddVertex("V1");
            B.AddVertex("V2");
            B.AddVertex("V3");
            B.AddVertex("V4");
            B.AddVertex("V5");
            B.AddVertex("V6");
            B.AddVertex("V7");
            B.AddVertex("V8");
            B.AddEdge("V1", "V2");
            B.AddEdge("V1", "V3");
            B.AddEdge("V2", "V4");
            B.AddEdge("V2", "V5");
            B.AddEdge("V3", "V6");
            B.AddEdge("V3", "V7");
            B.AddEdge("V4", "V8");
            B.AddEdge("V5", "V8");
            B.AddEdge("V6", "V8");
            B.AddEdge("V7", "V8");
            B.BFSTraverse();
            #endregion

            Console.ReadLine();
        }


        

    }
    public class AdjacencyList<T>
    {
        List<Vertex<T>> items; //图的顶点集合
        public AdjacencyList() : this(10) { } //构造方法
        public AdjacencyList(int capacity) //指定容量的构造方法
        {
            items = new List<Vertex<T>>(capacity);
        }
        public void AddVertex(T item) //添加一个顶点
        {   //不允许插入重复值
            if (Contains(item))
            {
                throw new ArgumentException("插入了重复顶点！");
            }
            items.Add(new Vertex<T>(item));
        }
        public void AddEdge(T from, T to) //添加无向边
        {
            Vertex<T> fromVer = Find(from); //找到起始顶点
            if (fromVer == null)
            {
                throw new ArgumentException("头顶点并不存在！");
            }
            Vertex<T> toVer = Find(to); //找到结束顶点
            if (toVer == null)
            {
                throw new ArgumentException("尾顶点并不存在！");
            }
            //无向边的两个顶点都需记录边信息
            AddDirectedEdge(fromVer, toVer);
            AddDirectedEdge(toVer, fromVer);
        }
        public bool Contains(T item) //查找图中是否包含某项
        {
            foreach (Vertex<T> v in items)
            {
                if (v.data.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        private Vertex<T> Find(T item) //查找指定项并返回
        {
            foreach (Vertex<T> v in items)
            {
                if (v.data.Equals(item))
                {
                    return v;
                }
            }
            return null;
        }
        //添加有向边
        private void AddDirectedEdge(Vertex<T> fromVer, Vertex<T> toVer)
        {
            if (fromVer.firstEdge == null) //无邻接点时
            {
                fromVer.firstEdge = new Node(toVer);
            }
            else
            {
                Node tmp, node = fromVer.firstEdge;
                do
                {   //检查是否添加了重复边
                    if (node.adjvex.data.Equals(toVer.data))
                    {
                        throw new ArgumentException("添加了重复的边！");
                    }
                    tmp = node;
                    node = node.next;
                } while (node != null);
                tmp.next = new Node(toVer); //添加到链表未尾
            }
        }
        public override string ToString() //仅用于测试
        {   //打印每个节点和它的邻接点
            string s = string.Empty;
            foreach (Vertex<T> v in items)
            {
                s += v.data.ToString() + ":";
                if (v.firstEdge != null)
                {
                    Node tmp = v.firstEdge;
                    while (tmp != null)
                    {
                        s += tmp.adjvex.data.ToString();
                        tmp = tmp.next;
                    }
                }
                s += "\r\n";
            }
            return s;
        }
        //嵌套类，表示链表中的表结点
        public class Node
        {
            public Vertex<T> adjvex; //邻接点域
            public Node next; //下一个邻接点指针域
            public Node(Vertex<T> value)
            {
                adjvex = value;
            }
        }
        //嵌套类，表示存放于数组中的表头结点
        public class Vertex<TValue>
        {
            public TValue data; //数据
            public Node firstEdge; //邻接点链表头指针
            public Boolean visited; //访问标志,遍历时使用
            public Vertex(TValue value) //构造方法
            {
                data = value;
            }
        }


        #region 深度优先遍历算法
        public void DFSTraverse() //深度优先遍历
        {
            InitVisited(); //将visited标志全部置为false
            DFS(items[0]); //从第一个顶点开始遍历
        }
        private void DFS(Vertex<T> v) //使用递归进行深度优先遍历
        {
            v.visited = true; //将访问标志设为true
            Console.Write(v.data + " "); //访问
            Node node = v.firstEdge;
            while (node != null) //访问此顶点的所有邻接点
            {   //如果邻接点未被访问，则递归访问它的边
                if (!node.adjvex.visited)
                {
                    DFS(node.adjvex); //递归
                }
                node = node.next; //访问下一个邻接点
            }
        }

        private void InitVisited() //初始化visited标志
        {
            foreach (Vertex<T> v in items)
            {
                v.visited = false; //全部置为false
            }
        }
        #endregion



        #region 广度优先遍历
        public void BFSTraverse() //广度优先遍历
        {
            InitVisited(); //将visited标志全部置为false
            BFS(items[0]); //从第一个顶点开始遍历
        }
        private void BFS(Vertex<T> v) //使用队列进行广度优先遍历
        {   //创建一个队列
            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            Console.Write(v.data + " "); //访问
            v.visited = true; //设置访问标志
            queue.Enqueue(v); //进队
            while (queue.Count > 0) //只要队不为空就循环
            {
                Vertex<T> w = queue.Dequeue();
                Node node = w.firstEdge;
                while (node != null) //访问此顶点的所有邻接点
                {   //如果邻接点未被访问，则递归访问它的边
                    if (!node.adjvex.visited)
                    {
                        Console.Write(node.adjvex.data + " "); //访问
                        node.adjvex.visited = true; //设置访问标志
                        queue.Enqueue(node.adjvex); //进队
                    }
                    node = node.next; //访问下一个邻接点
                }
            }
        }
        #endregion

        #region 非连通图的遍历
        public void No_DFSTraverse() //深度优先遍历
        {
            InitVisited(); //将visited标志全部置为false
            foreach (Vertex<T> v in items)
            {
                if (!v.visited) //如果未被访问
                {
                    DFS(v); //深度优先遍历
                }
            }
        }
        public void No_BFSTraverse() //广度优先遍历
        {
            InitVisited(); //将visited标志全部置为false
            foreach (Vertex<T> v in items)
            {
                if (!v.visited) //如果未被访问
                {
                    BFS(v); //广度优先遍历
                }
            }
        }
        #endregion
    }
}


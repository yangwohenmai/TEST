using System;
using System.Collections.Generic;
using System.Data;

namespace Graph
{
    class Program
    {
        public static int[] run = new int[9];
        //创建一个GraphLink类型的数组，存放每个节点的“点边关系”数据结构
        public static GraphLink[] head = new GraphLink[200];
        public const int MaxSize = 10;
        public static Queue<int> queue = new Queue<int>();
        public static Dictionary<int, int> dicrun = new Dictionary<int, int>();
        //public static Queue<int> layqueue = new Queue<int>();
        //public static int laycount = 1;
        //public static int pointcount = 0;
        public static void BFS(int current)
        {
            Node temp;
            queue.Enqueue(current);
            dicrun[current] = 1;
            Queue<int> layqueue = new Queue<int>();
            int laycount = 0;
            int pointcount = 0;
            Console.WriteLine("[" + current + "]");
            Console.WriteLine("↑lay:" + laycount);
            while (queue.Count > 0)
            {
                //取出队列中下一个要遍历的节点
                current = queue.Dequeue();

                //获取要遍历节点包含的相邻 点-边 关系
                temp = head[current].first;
                //如果当前节点包含可用的 相邻节点，进行遍历
                while (temp != null)
                {
                    //若当前子节点过去没被遍历过，则进行遍历
                    if (dicrun[temp.item] == 0)
                    {
                        //将当前节点的子节点插入队列中，以便后续使用
                        queue.Enqueue(temp.item);
                        layqueue.Enqueue(temp.item);
                        //将遍历后的节点标记为1
                        dicrun[temp.item] = 1;
                        Console.Write("[" + temp.item + "]");
                    }
                    //游标指向下一个同层子节点
                    temp = temp.next;

                }
                if (pointcount == 0 && layqueue.Count != 0)
                {
                    pointcount = layqueue.Count;
                    layqueue.Clear();
                    laycount++;
                    Console.WriteLine("");
                    Console.WriteLine("↑lay:" + laycount + ",count:" + pointcount);
                }
                pointcount--;

            }
        }
        

        static void Main1(string[] args)
        {
            #region 定义有向图的边
            //定义一个有向图，图中节点从第一个字段指向第二个字段（A->B）
            DataTable dt = new DataTable();
            dt.Columns.Add("A");
            dt.Columns.Add("B");
            dt.Rows.Add("11", "22");
            dt.Rows.Add("33", "11");
            dt.Rows.Add("33", "66");
            dt.Rows.Add("33", "77");
            dt.Rows.Add("55", "22");
            dt.Rows.Add("55", "44");
            dt.Rows.Add("66", "88");
            dt.Rows.Add("88", "55");
            #endregion

            #region 定义无向图的边
            //定义无向图，两个节点直接互相指向即可
            //dt.Rows.Add("11", "22");
            //dt.Rows.Add("22", "11");
            //dt.Rows.Add("11", "33");
            //dt.Rows.Add("33", "11");
            //dt.Rows.Add("22", "44");
            //dt.Rows.Add("44", "22");
            //dt.Rows.Add("22", "55");
            //dt.Rows.Add("55", "22");
            //dt.Rows.Add("33", "66");
            //dt.Rows.Add("66", "33");
            //dt.Rows.Add("33", "77");
            //dt.Rows.Add("77", "33");
            //dt.Rows.Add("44", "55");
            //dt.Rows.Add("55", "44");
            //dt.Rows.Add("66", "77");
            //dt.Rows.Add("77", "66");
            //dt.Rows.Add("55", "88");
            //dt.Rows.Add("88", "55");
            //dt.Rows.Add("66", "88");
            //dt.Rows.Add("88", "66");
            #endregion

            #region 定义图的顶点
            List<int> ll = new List<int>();
            ll.Add(11);
            ll.Add(22);
            ll.Add(33);
            ll.Add(44);
            ll.Add(55);
            ll.Add(66);
            ll.Add(77);
            ll.Add(88);
            #endregion

            //int nbPoint;
            int sonNode;
            //int[,] point_line = new int[20000000, 2];
            for (int p = 0; p < ll.Count; p++)
            {
                dicrun.Add(ll[p], 0);
                head[int.Parse(ll[p].ToString())] = new GraphLink();
                Console.Write("顶点" + ll[p].ToString() + "=>");
                for (int q = 0; q < dt.Rows.Count; q++)
                {
                    if (ll[p].ToString() == dt.Rows[q][0].ToString())
                    {
                        //获取该节点的相邻节点
                        sonNode = int.Parse(dt.Rows[q][1].ToString());
                        //将相邻节点存入该节点的子节点集合
                        head[int.Parse(ll[p].ToString())].Insert(sonNode);
                    }
                }
                head[int.Parse(ll[p].ToString())].Print();
            }

            

            Console.WriteLine("广度优先遍历:");
            int input = int.Parse(Console.ReadLine().ToString());
            BFS(input);
            while (true)
            {
                run = new int[9];
                string a = Console.ReadLine().ToString();
                input = int.Parse(a);
                BFS(input);
            }
        }
        


    }

    #region 定义图的数据结构
    class Node
    {
        public int item;
        public Node next;
        public Node(int a)
        {
            item = a;
            next = null;
        }
    }
    class GraphLink
    {
        public Node first;
        public Node last;
        public GraphLink()
        {
            first = null;
            last = null;
        }
        public void Insert(int a)
        {
            Node newnode = new Node(a);
            if (first == null)
            {
                first = newnode;
                last = newnode;
            }
            else
            {
                last.next = newnode;
                last = newnode;
            }
        }
        public void Print()
        {
            Node current = first;
            while (current != null)
            {
                Console.Write("[" + current.item + "]");
                current = current.next;
            }
            Console.WriteLine();
        }
    }
    #endregion
}

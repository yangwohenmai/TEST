using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataTCR
{
    /// <summary>
    /// 图的广度遍历
    /// </summary>
    class Graph_2
    {
        //创建一个GraphLink类型的数组，存放每个节点的“点边关系”数据结构
        public static GraphLink[] head = new GraphLink[150000000];
        public static Dictionary<int, GraphLink> aidHead = new Dictionary<int, GraphLink>();
        //记录图中包含的所有节点，保证每个节点只被遍历一次
        public static Dictionary<string, string> dicAllPoint = new Dictionary<string, string>();

        public static void BFS(int current)
        {
            Node temp;
            //待遍历节点队列
            Queue<int> queue = new Queue<int>();
            
            queue.Enqueue(current);
            dicAllPoint[current.ToString()] = "1";
            //遍历当前层时，记录下一层要遍历的所有节点
            Queue<int> nextlayqueue = new Queue<int>();
            //记录当前遍历的是第几层
            int laycount = 0;
            //记录当前层中 剩余未遍历节点个数
            int pointcount = 0;
            //string output = "";
            Console.WriteLine("[" + current + "]");
            Console.WriteLine("↑lay:" + laycount);
            while (queue.Count > 0)
            {
                //取出队列中下一个要遍历的节点
                current = queue.Dequeue();

                //
                //如果当前节点没有可达的相邻节点，即不再是父节点，而是是孩子节点
                //则该节点 不存在于父节点集合head中，给null值，不再向下循环遍历，
                //否则从父节点集合中取出该节点，继续向下遍历
                if (head[current] == null)
                    temp = null;
                else
                    temp = head[current].first;

                //如果当前节点属于父节点，包含可用的 相邻节点(孩子)，则进行遍历
                while (temp != null)
                {
                    //若当前节点过去被遍历过，则不再输出
                    if (dicAllPoint[temp.item.ToString()] == "0")
                    {
                        //将 当前节点的孩子节点 加入待遍历队列中
                        queue.Enqueue(temp.item);
                        //用于统计下一层节点总数，本层遍历完成后，记录其数值并清空
                        nextlayqueue.Enqueue(temp.item);
                        //将遍历后的节点标记为1
                        dicAllPoint[temp.item.ToString()] = "1";
                        Console.Write("[" + temp.item + "]");
                        //output += temp.item + ",";
                    }
                    //游标移动到下一个，与当前节点同层的 兄弟节点，进行下一轮遍历
                    temp = temp.next;
                }
                //若“当前层未遍历节点计数器”=0 且 当前层有有节点，说明上一层结束，此时开始遍历新层
                if (pointcount == 0 && nextlayqueue.Count != 0)
                {
                    //将新层的所有节点个数 赋值给“当前层未遍历节点计数器”
                    pointcount = nextlayqueue.Count;
                    //赋值完成后清空，在遍历本层过程中，开始统计下一层包含的节点个数
                    nextlayqueue.Clear();
                    //记录当前层数
                    laycount++;

                    Console.WriteLine("\n↑lay:" + laycount + ",count:" + pointcount);
                }
                pointcount--;
                if (laycount == 5)
                {
                    break;
                }
            }
        }
        


        static void Main1(string[] args)
        {
            Dictionary<string, string> dicFather = new Dictionary<string, string>();
            SortedDictionary<string, List<string>> listFather = new SortedDictionary<string, List<string>>();
            Console.WriteLine("1:" + dicFather.Count);
            //Console.ReadLine();

            #region 测试数据
            //头结点列表
            SortedDictionary<string, List<string>> tempdic = new SortedDictionary<string, List<string>>();
            //子节点列表
            List<string> templist = new List<string>();

            #region 无向全连通图，每两个相邻节点都互相指向，Key->value表示K指向V
            //templist.Add("22");
            //templist.Add("33");
            //tempdic.Add("11", templist);
            //templist = new List<string>();
            //templist.Add("11");
            //templist.Add("44");
            //templist.Add("55");
            //tempdic.Add("22", templist);
            //templist = new List<string>();
            //templist.Add("11");
            //templist.Add("77");
            //templist.Add("66");
            //tempdic.Add("33", templist);
            //templist = new List<string>();
            //templist.Add("22");
            //templist.Add("55");
            //tempdic.Add("44", templist);
            //templist = new List<string>();
            //templist.Add("22");
            //templist.Add("44");
            //templist.Add("88");
            //tempdic.Add("55", templist);
            //templist = new List<string>();
            //templist.Add("77");
            //templist.Add("33");
            //templist.Add("88");
            //tempdic.Add("66", templist);
            //templist = new List<string>();
            //templist.Add("66");
            //templist.Add("33");
            //tempdic.Add("77", templist);
            //templist = new List<string>();
            //templist.Add("55");
            //templist.Add("66");
            //tempdic.Add("88", templist);
            //listFather.Clear();
            //listFather = tempdic;
            #endregion

            #region 定义有向图，Key->value表示 当前节点 可到达的 相邻节点
            templist.Add("22");
            tempdic.Add("11", templist);

            templist = new List<string>();
            templist.Add("11");
            templist.Add("77");
            templist.Add("66");
            tempdic.Add("33", templist);

            templist = new List<string>();
            templist.Add("22");
            templist.Add("44");
            tempdic.Add("55", templist);

            templist = new List<string>();
            templist.Add("88");
            tempdic.Add("66", templist);

            templist = new List<string>();

            templist.Add("55");
            tempdic.Add("88", templist);
            listFather.Clear();
            listFather = tempdic;
            #endregion

            #region 记录图中所有节点，节点被访问过后值变更0->1,避免重复访问
            Dictionary<string, string> templl = new Dictionary<string, string>();
            templl.Add("11", "0");
            templl.Add("22", "0");
            templl.Add("33", "0");
            templl.Add("44", "0");
            templl.Add("55", "0");
            templl.Add("66", "0");
            templl.Add("77", "0");
            templl.Add("88", "0");
            dicAllPoint.Clear();
            dicAllPoint = templl;
            #endregion
            #endregion

            
            List<string> sonNodeList;
            foreach (var item in listFather)
            {
                head[int.Parse(item.Key)] = new GraphLink();
                Console.Write("顶点" + item.Key.ToString() + "=>");
                sonNodeList = listFather[item.Key];
                head[int.Parse(item.Key)].Add(sonNodeList);
                //nbPoint++;
                //if (nbPoint % 10000 == 0)
                //{
                //    Console.WriteLine(nbPoint);
                //}
                head[int.Parse(item.Key)].Print();

            }
            

            Console.WriteLine("广度优先遍历:");
            int input = int.Parse(Console.ReadLine().ToString());
            Console.WriteLine("开始：" + DateTime.Now);
            BFS(input);
            Console.WriteLine("结束：" + DateTime.Now);
            while (true)
            {
                List<string> al = dicAllPoint.Keys.ToList<string>();
                foreach (var item in al)
                {
                    dicAllPoint[item] = "0";
                }


                string a = Console.ReadLine().ToString();
                input = int.Parse(a);
                BFS(input);
            }
        }







    }

    #region 构建 图的数据结构
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

        public void Add(List<string> list)
        {
            foreach (var item in list)
            {
                Node newnode = new Node(int.Parse(item));
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

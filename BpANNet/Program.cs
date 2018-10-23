using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BpANNet
{
    class Program
    {
        static void Main(string[] args)
        {
            //0.1399,0.1467,0.1567,0.1595,0.1588,0.1622,0.1611,0.1615,0.1685,0.1789,0.1790

            //      double [,] p1=new double[,]{{0.05,0.02},{0.09,0.11},{0.12,0.20},{0.15,0.22},{0.20,0.25},{0.75,0.75},{0.80,0.83},{0.82,0.80},{0.90,0.89},{0.95,0.89},{0.09,0.04},{0.1,0.1},{0.14,0.21},{0.18,0.24},{0.22,0.28},{0.77,0.78},{0.79,0.81},{0.84,0.82},{0.94,0.93},{0.98,0.99}};
            //      double [,] t1=new double[,]{{1,0},{1,0},{1,0},{1,0},{1,0},{0,1},{0,1},{0,1},{0,1},{0,1},{1,0},{1,0},{1,0},{1,0},{1,0},{0,1},{0,1},{0,1},{0,1},{0,1}};
            double[,] p1 = new double[,] { { 0.1399, 0.1467, 0.1567, 0.1595, 0.1588, 0.1622 }, { 0.1467, 0.1567, 0.1595, 0.1588, 0.1622, 0.1611 }, { 0.1567, 0.1595, 0.1588, 0.1622, 0.1611, 0.1615 }, { 0.1595, 0.1588, 0.1622, 0.1611, 0.1615, 0.1685 }, { 0.1588, 0.1622, 0.1611, 0.1615, 0.1685, 0.1789 } };
            double[,] t1 = new double[,] { { 0.1622 }, { 0.1611 }, { 0.1615 }, { 0.1685 }, { 0.1789 }, { 0.1790 } };
            BpNet bp = new BpNet(p1, t1);
            int study = 0;
            do
            {
                study++;
                bp.train(p1, t1);
                //       bp.rate=0.95-(0.95-0.3)*study/50000;
                //        Console.Write("第 "+ study+"次学习： ");
                //        Console.WriteLine(" 均方差为 "+bp.e);

            } while (bp.e > 0.001 && study < 50000);
            Console.Write("第 " + study + "次学习： ");
            Console.WriteLine(" 均方差为 " + bp.e);
            bp.saveMatrix(bp.w, "w.txt");
            bp.saveMatrix(bp.v, "v.txt");
            bp.saveMatrix(bp.b1, "b1.txt");
            bp.saveMatrix(bp.b2, "b2.txt");

            //      double [,] p2=new double[,]{{0.05,0.02},{0.09,0.11},{0.12,0.20},{0.15,0.22},{0.20,0.25},{0.75,0.75},{0.80,0.83},{0.82,0.80},{0.90,0.89},{0.95,0.89},{0.09,0.04},{0.1,0.1},{0.14,0.21},{0.18,0.24},{0.22,0.28},{0.77,0.78},{0.79,0.81},{0.84,0.82},{0.94,0.93},{0.98,0.99}};
            double[,] p2 = new double[,] { { 0.1399, 0.1467, 0.1567, 0.1595, 0.1588, 0.1622 }, { 0.1622, 0.1611, 0.1615, 0.1685, 0.1789, 0.1790 } };
            int aa = bp.inNum;
            int bb = bp.outNum;
            int cc = p2.GetLength(0);
            double[] p21 = new double[aa];
            double[] t2 = new double[bb];
            for (int n = 0; n < cc; n++)
            {
                for (int i = 0; i < aa; i++)
                {
                    p21[i] = p2[n, i];
                }
                t2 = bp.sim(p21);

                for (int i = 0; i < t2.Length; i++)
                {
                    Console.WriteLine(t2[i] + " ");
                }

            }

            Console.ReadLine();

        }
    }
}

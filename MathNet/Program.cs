using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathNet
{
    class Program
    {
        static void Main(string[] args)
        {



             //初始化一个矩阵和向量的构建对象
            var mb = Matrix<double>.Build;
            var mb1 = Matrix<double>.Build;
            var vb = Vector<double>.Build;

            DateTime time = new DateTime(2005,12,05);



            double[] AiC =  { 1,2,3};
            double[,] AiC1 = { { 1, 2, 3 }, { 1, 2, 3 }, { 1, 2, 3 } };
            double[] AiR = new double[3];
            DenseMatrix AiMatrix = DenseMatrix.OfArray(new[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 }, { 7.0, 8.0, 9.0 } }); ;//创建一个对角矩阵对象
            VectorBuilder<double> vector = Vector<double>.Build;
            var t = vector.DenseOfArray(AiC);
            //var c = AiMatrix.DenseOfArray(AiC1);
            var r = t * AiMatrix;



            var matrixA = DenseMatrix.OfArray(new[,] { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 }, { 7.0, 8.0, 9.0 } });
            var matrixB = DenseMatrix.OfArray(new[,] { { 1.0, 3.0, 5.0 }, { 2.0, 4.0, 6.0 }, { 3.0, 5.0, 7.0 } });
            var s = matrixA * matrixB;





            string str = "20100531";
            DateTime dtime = DateTime.ParseExact(str, "yyyyMMdd", null);


            DateTime dtime1 = DateTime.Now;

            Dictionary<int, double> dic = new Dictionary<int, double>();
            dic.Add(0, 1);
            dic.Add(1, 2);
            dic.Add(2, 3);
            dic.Add(3, 4);
            //var matrix2 = mb.Dense(2, 3, (i, j) => 3 * i + j);
            var diagMaxtrix = mb.DenseDiagonal(4, 4, (i)=>i);


            //先生成数据集合
            var chiSquare = new ChiSquared(5);
            Console.WriteLine(@"2. Generate 1000 samples of the ChiSquare(5) distribution");
            var data = new double[1000];
            //var data1 = new double[1000];
            //for (var i = 0; i < data.Length; i++)
            //{
            //    data[i] = chiSquare.Sample();
            //}
            List<double> a = new List<double>();
            List<double> b = new List<double>();
            //data1 = new double []{1,2,3,4,5};

            data[0] = 10;
            data[1] = 0;
            data[2] = 0;
            data[3] = 0;
            data[4] = 0;
            data[5] = 0;
            data[6] = 0;
            data[7] = 0;
            data[8] = 0;
            data[9] = -0.1;
            var data1 = new double[] { 1, 2, 3 };
            var data2 = new double[] { 1, 2, 6 };
            data1.Mean();
            data1.Variance();
            data2.Mean();
            var ss = data1.Covariance(data2);
            //使用扩展方法进行相关计算
            Console.WriteLine(@"3.使用扩展方法获取生成数据的基本统计结果");
            Console.WriteLine(@"{0} - 最大值", data.Maximum().ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - 最小值", data.Minimum().ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - 均值", data.Mean().ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - 中间值", data.Median().ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - 有偏方差", data.PopulationVariance().ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - 无偏方差", data.Variance().ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - 标准偏差", data.StandardDeviation().ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - 标准有偏偏差", data.PopulationStandardDeviation().ToString(" #0.00000;-#0.00000"));
            Console.WriteLine();



            Console.WriteLine(@"4. 使用DescriptiveStatistics类进行基本的统计计算");
            var descriptiveStatistics = new DescriptiveStatistics(data);//使用数据进行类型的初始化
            //直接使用属性获取结果
            Console.WriteLine(@"{0} - Kurtosis", descriptiveStatistics.Kurtosis.ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - Largest element", descriptiveStatistics.Maximum.ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - Smallest element", descriptiveStatistics.Minimum.ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - Mean", descriptiveStatistics.Mean.ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - Variance", descriptiveStatistics.Variance.ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - Standard deviation", descriptiveStatistics.StandardDeviation.ToString(" #0.00000;-#0.00000"));
            Console.WriteLine(@"{0} - Skewness", descriptiveStatistics.Skewness.ToString(" #0.00000;-#0.00000"));
            Console.WriteLine();

        }




        private void Run()
        {
            DenseVector vector1 = CreateRandomVector(500);
            DenseVector vector2 = CreateRandomVector(500);
            var matrix1 = CreateRandomMatrix(500, 500);
            var resultV = vector1 * matrix1;
        }

        private DenseMatrix CreateRandomMatrix(int intRow, int intCol)
        {
            DenseMatrix resultMatrix = new DenseMatrix(intRow, intCol);
            var rnd = new Random();
            for (var i = 0; i < resultMatrix.RowCount; i++)
            {
                for (var j = 0; j < resultMatrix.ColumnCount; j++)
                {
                    resultMatrix[i, j] = rnd.Next(50, 1000);
                }
            }
            return resultMatrix;
        }

        private DenseVector CreateRandomVector(int intNum)
        {
            DenseVector resultVector = new DenseVector(intNum);
            var rnd = new Random();
            for (var i = 0; i < resultVector.Count; i++)
            {
                resultVector[i] = rnd.Next(50, 1000);
            }
            return resultVector;
        }







    }
}

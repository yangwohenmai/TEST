using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEST
{
    public class TEST
    {

        static void Main(string[] args)
        {
            double[] arr;
            arr = new double[5] { 2077, 2077, 2078, 2083, 2082 };
            List<double> dd = new List<double>() { 5.853333, 5.516666, 5.513333, 5.533333, 5.58};
            dd = new List<double>() { 5.58, 5.533333, 5.513333, 5.516666, 5.853333 };
            //EMAResult du= ;
            var result = EMA(dd, 3);
            Console.WriteLine("{0}个result的值", result.Values.Count);

            for (int i = 0; i < result.Values.Count; i++)
            {
                Console.WriteLine("第{0}的ema={1}", i, result.Values[i]);
            }
            Console.WriteLine("emaR={0}", result.EmaR);
        }
        /// <summary>
        /// Contains calculation results for EMA indicator
        /// </summary>
        public class EMAResult
        {
            public List<double> Values { get; set; }
            public int StartIndexOffset { get; set; }
            public double EmaR { get; set; }

        }

        //-------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// Calculates Exponential Moving Average (EMA) indicator
        /// </summary>
        /// <param name="input">Input signal</param>
        /// <param name="period">Number of periods</param>
        /// <returns>Object containing operation results</returns>
        public static EMAResult EMA(IEnumerable<double> input, int period)
        {
            var returnValues = new List<double>();

            double multiplier = (2.0 / (period + 1));
            //double initialSMA = input.Take(period).Average();

            //returnValues.Add(initialSMA);

            var copyInputValues = input.ToList();

            int j = 0;
            for (int i = copyInputValues.Count - period; i < copyInputValues.Count; i++)
            {
                if (j < 1)
                {
                    var resultValue = copyInputValues[i];
                    returnValues.Add(resultValue);
                }
                else
                {
                    var resultValue = (copyInputValues[i] * multiplier) + (1 - multiplier) * returnValues.Last();
                    returnValues.Add(resultValue);
                }
                j++;


            }

            var result = new EMAResult()
            {
                EmaR = returnValues.Last(),
                Values = returnValues,
                StartIndexOffset = period - 1
            };

            return result;
        }




    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculate
{
    public class Function
    {
        /// <summary>
        /// Calculates Exponential Moving Average (EMA) indicator
        /// </summary>
        /// <param name="input">Input signal</param>
        /// <param name="period">Number of periods</param>
        /// <returns>Object containing operation results</returns>
        public static List<decimal> EMA(IEnumerable<decimal> input, int period)
        {
            var returnValues = new List<decimal>();

            decimal multiplier = (decimal)(2.0 / (period + 1));
            decimal initialSMA = input.Take(period).Average();

            returnValues.Add(initialSMA);

            var copyInputValues = input.ToList();

            for (int i = period; i < copyInputValues.Count; i++)
            {
                var resultValue = (copyInputValues[i] - returnValues.Last()) * multiplier + returnValues.Last();

                returnValues.Add(resultValue);
            }

            return returnValues;
        }

        /// <summary>
        /// 取前N个周期数据
        /// </summary>
        /// <param name="input"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static decimal REF(List<decimal> input, int period)
        {
            return input[input.Count - period - 1];
        }

        /// <summary>
        /// 求最近N周期均值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public static decimal MA(List<decimal> input, int period)
        {
            List<decimal> list = new List<decimal>();
            if (period <= input.Count)
            {
                for (int i = period; i > 0; i--)
                {
                    list.Add(input[input.Count - i]);
                }
            }
            return list.Average();
        }
    }
}

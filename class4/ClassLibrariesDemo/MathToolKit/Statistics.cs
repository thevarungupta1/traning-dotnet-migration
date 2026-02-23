using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathToolKit
{
    public static class Statistics
    {
        public static double Mean(IEnumerable<double> values)
        {
            ArgumentNullCheck(values);
            var list = values.ToList();
            if (list.Count == 0)
                throw new ArgumentException("Input collection cannot be empty.", nameof(values));

            return list.Sum() / list.Count;
        }

        public static double Median(IEnumerable<double> values)
        {
            ArgumentNullCheck(values);
            var sorted = values.OrderBy(v => v).ToList();
            
            if(sorted.Count == 0)
                throw new ArgumentException("Sequence contains no elements.", nameof(values));

            int mid = sorted.Count / 2;

            if(sorted.Count % 2 == 0)
                return (sorted[mid - 1] + sorted[mid]) / 2.0;
            else
                return sorted[mid];
        }

        private static void ArgumentNullCheck(IEnumerable<double> values)
        {
            if (values == null) 
                throw new ArgumentNullException(nameof(values), "Input collection cannot be null.");
        }
    }
}

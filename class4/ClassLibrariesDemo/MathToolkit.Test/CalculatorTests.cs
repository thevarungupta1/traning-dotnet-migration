using MathToolKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathToolkit.Test
{
    public class CalculatorTests
    {
        [Theory]
        [InlineData(2, 3, 5)]
        [InlineData(-1, 1, 0)]
        [InlineData(0,0,0)]
        public void Add_ReturnsCorrectSum(double a, double b, double expexted)
        {
            Assert.Equal(expexted, Calculator.Add(a, b));
        }

        [Theory]
        [InlineData(10, 4, 6)]
        [InlineData(0, 5, -5)]
        public void Subtract_ReturnsCorrectDifference(double a, double b, double expexted)
        {
            Assert.Equal(expexted, Calculator.Subtract(a, b));
        }
    }
}

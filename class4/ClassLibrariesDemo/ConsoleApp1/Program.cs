using MathToolKit;


Console.WriteLine("=============================================");
Console.WriteLine("Welcome to the Math ToolKit Console App!");
Console.WriteLine("=============================================");

Console.WriteLine("------ Calculator --------");
Console.WriteLine($"5 + 3 = {Calculator.Add(5, 3)}");
Console.WriteLine($"10 - 4 = {Calculator.Subtract(10, 4)}");
Console.WriteLine($"6 * 7 = {Calculator.Multiply(6, 7)}");
Console.WriteLine($"20 / 5 = {Calculator.Divide(20, 5)}");


Console.WriteLine("\n------ Statistics --------");
Console.WriteLine($"Mean of [1, 2, 3, 4, 5] = {Statistics.Mean(new double[] { 1, 2, 3, 4, 5 })}");
Console.WriteLine($"Median of [1, 2, 3, 4, 5] = {Statistics.Median(new double[] { 1, 2, 3, 4, 5 })}");
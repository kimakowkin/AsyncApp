using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        var rand = new Random();
        var numbers = new int[10000000];
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = rand.Next(-10000, 10000);
        }

        for (int threadCount = 1; threadCount <= 8; threadCount *= 2)
        {
            var sw = Stopwatch.StartNew();
            long totalSum = 0;

            var tasks = new Task[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                int start = i * numbers.Length / threadCount;
                int end = (i + 1) * numbers.Length / threadCount;
                tasks[i] = Task.Run(() =>
                {
                    long sum = 0;
                    for (int j = start; j < end; j++)
                    {
                        sum += numbers[j];
                    }
                    return sum;
                }).ContinueWith(t => totalSum += t.Result);
            }

            Task.WaitAll(tasks);
            sw.Stop();

            Console.WriteLine($"Threads: {threadCount}, Time: {sw.ElapsedMilliseconds} ms, Sum: {totalSum}");
        }
    }
}
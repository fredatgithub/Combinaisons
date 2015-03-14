using System;

namespace CH.Combinations.Benchmark
{
  internal class Program
  {
    private static int _seconds = 2;

    private static void DoBenchmarkEnumerator<T>(Combinations<T> combs)
    {
      int count = 0;
      DateTime start = DateTime.UtcNow;
      TimeSpan elapsed;
      do
      {
        foreach (T[] t in combs)
        {
          count++;
          if (count % 10000 == 0)
          {
            elapsed = DateTime.UtcNow - start;
            if (elapsed.Ticks >= _seconds * TimeSpan.TicksPerSecond)
            {
              break;
            }
          }
        }

        elapsed = DateTime.UtcNow - start;
      } while (elapsed.Ticks < _seconds * TimeSpan.TicksPerSecond);

      double eps = ((double)count) / (elapsed.Seconds * 1000 + elapsed.Milliseconds) * 1000.0;
      Console.WriteLine("{0:#,0} e in {1:0.000}s = {2:#,0} e/s", count, elapsed.Seconds + elapsed.Milliseconds / 1000.0, (int)eps);
    }

    private static void DoBenchmarkEnumerator(int n, int r, int percentChanceDuplicate)
    {
      int[] data = new int[n];
      Random rand = new Random();
      for (int i = 0; i < data.Length; i++)
      {
        if (rand.Next(100) < percentChanceDuplicate && i > 0)
        {
          data[i] = data[i - 1];
        }
        else
        {
          data[i] = i;
        }
      }
      Combinations<int> combs = new Combinations<int>(data, r);
      DoBenchmarkEnumerator(combs);
    }

    private static void Shuffle<T>(T[] data)
    {
      int n = data.Length;
      Random r = new Random();
      while (n > 1)
      {
        int idx = r.Next(n--);
        T temp = data[n];
        data[n] = data[idx];
        data[idx] = temp;
      }
    }

    private enum DatasetBenchmarkType
    {
      Sequential, Random
    }

    private static void DoBenchmarkDataset(int n, DatasetBenchmarkType type)
    {
      int[] data = new int[n];
      for (int i = 0; i < data.Length; i++)
      {
        data[i] = i;
      }
      if (type == DatasetBenchmarkType.Random)
      {
        Shuffle(data);
      }
      int count = 0;
      DateTime start = DateTime.UtcNow;
      TimeSpan elapsed;
      do
      {
        new ElementSet<int>(data);
        count++;
        elapsed = DateTime.UtcNow - start;
      } while (elapsed.Ticks < _seconds * TimeSpan.TicksPerSecond);
      double dsps = ((double)count) / (elapsed.Seconds * 1000 + elapsed.Milliseconds) * 1000.0;
      Console.WriteLine("{0:#,0} ds in {1:0.000}s = {2:#,0} ds/s = {3:#,0} e/s", count, elapsed.Seconds + elapsed.Milliseconds / 1000.0, (int)dsps, (int)(dsps * n));
    }

    private static void Main(string[] args)
    {
      if (args.Length == 1)
      {
        _seconds = int.Parse(args[0]);
      }
      Console.WriteLine("Integer enumerator benchmarks (distinct):");
      Console.Write("10C5 integers:\t\t");
      DoBenchmarkEnumerator(10, 5, 0);
      Console.Write("10C7 integers:\t\t");
      DoBenchmarkEnumerator(10, 7, 0);
      Console.Write("10C3 integers:\t\t");
      DoBenchmarkEnumerator(10, 3, 0);
      Console.Write("100C50 integers:\t");
      DoBenchmarkEnumerator(100, 50, 0);
      Console.Write("100C70 integers:\t");
      DoBenchmarkEnumerator(100, 70, 0);
      Console.Write("100C30 integers:\t");
      DoBenchmarkEnumerator(100, 30, 0);
      Console.Write("1000C500 integers:\t");
      DoBenchmarkEnumerator(1000, 500, 0);
      Console.Write("1000C700 integers:\t");
      DoBenchmarkEnumerator(1000, 700, 0);
      Console.Write("1000C300 integers:\t");
      DoBenchmarkEnumerator(1000, 300, 0);

      Console.WriteLine();
      Console.WriteLine("Integer enumerator benchmarks (10% duplicates):");
      Console.Write("10C5 integers:\t\t");
      DoBenchmarkEnumerator(10, 5, 10);
      Console.Write("10C7 integers:\t\t");
      DoBenchmarkEnumerator(10, 7, 10);
      Console.Write("10C3 integers:\t\t");
      DoBenchmarkEnumerator(10, 3, 10);
      Console.Write("100C50 integers:\t");
      DoBenchmarkEnumerator(100, 50, 10);
      Console.Write("100C70 integers:\t");
      DoBenchmarkEnumerator(100, 70, 10);
      Console.Write("100C30 integers:\t");
      DoBenchmarkEnumerator(100, 30, 10);
      Console.Write("1000C500 integers:\t");
      DoBenchmarkEnumerator(1000, 500, 10);
      Console.Write("1000C700 integers:\t");
      DoBenchmarkEnumerator(1000, 700, 10);
      Console.Write("1000C300 integers:\t");
      DoBenchmarkEnumerator(1000, 300, 10);

      Console.WriteLine();
      Console.WriteLine("Integer enumerator benchmarks (25% duplicates):");
      Console.Write("10C5 integers:\t\t");
      DoBenchmarkEnumerator(10, 5, 25);
      Console.Write("10C7 integers:\t\t");
      DoBenchmarkEnumerator(10, 7, 25);
      Console.Write("10C3 integers:\t\t");
      DoBenchmarkEnumerator(10, 3, 25);
      Console.Write("100C50 integers:\t");
      DoBenchmarkEnumerator(100, 50, 25);
      Console.Write("100C70 integers:\t");
      DoBenchmarkEnumerator(100, 70, 25);
      Console.Write("100C30 integers:\t");
      DoBenchmarkEnumerator(100, 30, 25);
      Console.Write("1000C500 integers:\t");
      DoBenchmarkEnumerator(1000, 500, 25);
      Console.Write("1000C700 integers:\t");
      DoBenchmarkEnumerator(1000, 700, 25);
      Console.Write("1000C300 integers:\t");
      DoBenchmarkEnumerator(1000, 300, 25);

      Console.WriteLine();
      Console.WriteLine("Integer enumerator benchmarks (60% duplicates):");
      Console.Write("10C5 integers:\t\t");
      DoBenchmarkEnumerator(10, 5, 60);
      Console.Write("10C7 integers:\t\t");
      DoBenchmarkEnumerator(10, 7, 60);
      Console.Write("10C3 integers:\t\t");
      DoBenchmarkEnumerator(10, 3, 60);
      Console.Write("100C50 integers:\t");
      DoBenchmarkEnumerator(100, 50, 60);
      Console.Write("100C70 integers:\t");
      DoBenchmarkEnumerator(100, 70, 60);
      Console.Write("100C30 integers:\t");
      DoBenchmarkEnumerator(100, 30, 60);
      Console.Write("1000C500 integers:\t");
      DoBenchmarkEnumerator(1000, 500, 60);
      Console.Write("1000C700 integers:\t");
      DoBenchmarkEnumerator(1000, 700, 60);
      Console.Write("1000C300 integers:\t");
      DoBenchmarkEnumerator(1000, 300, 60);

      Console.WriteLine();
      Console.WriteLine("Integer dataset preparation benchmarks (sequential):");
      Console.Write("10 integers:\t");
      DoBenchmarkDataset(10, DatasetBenchmarkType.Sequential);
      Console.Write("100 integers:\t");
      DoBenchmarkDataset(100, DatasetBenchmarkType.Sequential);
      Console.Write("1000 integers:\t");
      DoBenchmarkDataset(1000, DatasetBenchmarkType.Sequential);
      Console.Write("10000 integers:\t");
      DoBenchmarkDataset(10000, DatasetBenchmarkType.Sequential);

      Console.WriteLine();
      Console.WriteLine("Integer dataset preparation benchmarks (random):");
      Console.Write("10 integers:\t");
      DoBenchmarkDataset(10, DatasetBenchmarkType.Random);
      Console.Write("100 integers:\t");
      DoBenchmarkDataset(100, DatasetBenchmarkType.Random);
      Console.Write("1000 integers:\t");
      DoBenchmarkDataset(1000, DatasetBenchmarkType.Random);
      Console.Write("10000 integers:\t");
      DoBenchmarkDataset(10000, DatasetBenchmarkType.Random);
      Console.WriteLine("Press a key to exit:");
      Console.ReadKey();
    }
  }
}
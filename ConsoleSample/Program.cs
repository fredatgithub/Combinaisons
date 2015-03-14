using System;
using System.Linq;
using CH.Combinations;

namespace ConsoleSample
{
  class Program
  {
    static void Main()
    {
      int[] input1 = { 1, 2, 3, 4, 5 };
      Combinations<int> combinations1 = new Combinations<int>(input1, 3);
      foreach (int[] combination in combinations1)
      {
        foreach (var item in combination)
        {
          Console.Write(item + " ");

        }

        Console.WriteLine();
      }

      Console.WriteLine("Press a key to see another sample:");
      Console.ReadKey();

      string[] input = { "alpha", "ALPHA", "beta", "gamma" };
      Combinations<string> combinations = new Combinations<string>(input, StringComparer.CurrentCultureIgnoreCase, 2);
      foreach (string[] combination in combinations)
      {
        foreach (var item in combination)
        {
          Console.Write(item + " ");

        }
        Console.WriteLine();
      }

      Console.WriteLine("Press a key to see another sample:");
      Console.ReadKey();

      int[] input3 = { 1, 2, 3, 4, 5 };
      ElementSet<int> elements = new ElementSet<int>(input3);
      for (int i = 0; i <= 5; i++)
      {
        Combinations<int> combos = new Combinations<int>(elements, i);
        foreach (int[] combo in combos)
        {
          foreach (var item in combo)
          {
            Console.Write(item + " ");

          }
          Console.WriteLine();
        }
      }

      Console.WriteLine("Press a key to see another sample:");
      Console.ReadKey();

      int[] corners = { 1, 2, 3, 4 };
      int[] borders = Enumerable.Range(1, 56).ToArray();
      int[] centers = Enumerable.Range(1, 196).ToArray();

      ElementSet<int> cornerSet = new ElementSet<int>(corners);
      ElementSet<int> borderSet = new ElementSet<int>(borders);
      ElementSet<int> centerSet = new ElementSet<int>(centers);
      Combinations<int> combosCorners = new Combinations<int>(cornerSet, 1);
      Combinations<int> combosBorders = new Combinations<int>(borderSet, 1);
      Combinations<int> combosCenters = new Combinations<int>(centerSet, 1);
      foreach (int[] comboCorner in combosCorners)
      {
        foreach (var item in comboCorner)
        {
          Console.Write(item + " ");

        }
        Console.WriteLine();
      }

      foreach (int[] comboBorder in combosBorders)
      {
        foreach (var item in comboBorder)
        {
          Console.Write(item + " ");

        }
        Console.WriteLine();
      }

      Console.WriteLine("press a key to exit:");
      Console.ReadKey();
    }
  }
}
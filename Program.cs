using System;
using System.Collections.Generic;
using System.Linq;

namespace ChipSecuritySystem
{
    internal class Program
    {
        private static readonly Random rnd = new Random();
        private static readonly int numberOfChips = rnd.Next(4, 10);
        private static readonly List<ColorChip> chips = new List<ColorChip>();
        private static readonly List<ColorChip> assembledChips = new List<ColorChip>();

        static void Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Randomly generated chips.");
            Console.WriteLine("2) Use example chips: [Blue, Yellow] [Red, Green] [Yellow, Red] [Orange, Purple].");
            Console.WriteLine("3) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    GenerateChipsRandomly(numberOfChips);
                    return false;
                case "2":
                    GenerateBicoloredChips();
                    return false;
                case "3":
                    return false;
                default:
                    return true;
            }
        }

        static void GenerateChipsRandomly(int numberOfChips)
        {
            Array colors = Enum.GetValues(typeof(Color));

            for (int i = 0; i < numberOfChips; i++)
            {
                Color startColor = (Color)colors.GetValue(rnd.Next(6));
                Color endColor = (Color)colors.GetValue(rnd.Next(6));
                ColorChip colorChip = new ColorChip(startColor, endColor);
                chips.Add(colorChip);
            }

            Console.WriteLine("\nChips generated");
            foreach (var item in chips)
            {
                Console.WriteLine(item);
            }

            GenerateColorCombination();
        }

        static void GenerateBicoloredChips()
        {
            chips.Add(new ColorChip(Color.Blue, Color.Yellow));
            chips.Add(new ColorChip(Color.Red, Color.Green));
            chips.Add(new ColorChip(Color.Yellow, Color.Red));
            chips.Add(new ColorChip(Color.Orange, Color.Purple));

            Console.WriteLine("\nChips generated");
            foreach (var item in chips)
            {
                Console.WriteLine(item);
            }

            GenerateColorCombination();
        }

        static void GenerateColorCombination()
        {
            bool canUnlock = false;
            foreach (var chip in chips.Where(x => x.StartColor == Color.Blue))
            {
                if (chip.EndColor == Color.Green)
                    continue;

                assembledChips.Add(chip);
                if (FindNextMatch(chip))
                {
                    canUnlock = true;
                    Console.WriteLine("\n---Correct sequence to unlock---");
                    foreach (var chipAdded in assembledChips)
                    {
                        Console.WriteLine(chipAdded.ToString());
                    }
                    Console.WriteLine("\n");
                }
                else
                {
                    assembledChips.Remove(chip);
                }
            }

            if (!canUnlock)
            {
                Console.WriteLine("\n---------------------");
                Console.WriteLine(Constants.ErrorMessage + "\n");
            }
        }

        static bool FindNextMatch(ColorChip currentChip)
        {
            foreach (var nextChip in chips.Where(x => x.StartColor == currentChip.EndColor))
            {
                if (nextChip != null)
                {
                    if (!Exists(nextChip))
                    {
                        assembledChips.Add(nextChip);
                    }
                    else
                    {
                        continue;
                    }

                    if (FindNextMatch(nextChip))
                        return true;

                    if (assembledChips.Count > 1 && assembledChips.LastOrDefault()?.EndColor == Color.Green)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }


            return false;
        }

        static bool Exists(ColorChip nextChip)
        {
            if (assembledChips.Contains(nextChip))
            {
                return true;
            }
            return false;
        }
    }
}

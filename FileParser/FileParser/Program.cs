using System;
using System.IO;
namespace FileParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the file destination:");
            var fileDestination = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(fileDestination))
            {
                Console.WriteLine("Error! Please provide a file destination!");
                return;
            }

            Console.WriteLine("Is this a CSV or TSV file?:");
            var fileType = Console.ReadLine();

            FileExtension fileExt;
            if (!Enum.TryParse(fileType, true, out fileExt))
            {
                Console.WriteLine($"Error! File type: {fileType} is not supported.");
                return;
            }

            Console.WriteLine("How many fields does each row have?:");
            var fieldValue = Console.ReadLine();
            if (!Int32.TryParse(fieldValue, out int numFields))
            {
                Console.WriteLine($"Error! Could not parse '{fieldValue}' to an integer");
                return;
            }


            if (numFields <= 0)
            {
                Console.WriteLine("Error! Please input a number larger than zero!");
                return;
            }

            var pathString = Path.Combine(fileDestination, "Results");
            if (!Directory.Exists(pathString))
            {
                Directory.CreateDirectory(pathString);
            }


            var files = Directory.GetFiles(fileDestination, $"*.{fileExt}");
            if (files.Length == 0)
            {
                Console.WriteLine($"Error! There are not any files with the extension of '{fileExt}' in that directory");
                return;
            }

            var parser = new Parser();
            foreach (var file in files)
            {
                parser.ValidateFile(fileExt, file, pathString, numFields);
            }
        }
    }
}

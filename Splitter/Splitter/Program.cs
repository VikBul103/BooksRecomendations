using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        string sourcePath = "book1-100k.csv";
        string testPath = "test.csv";
        var lines = File.ReadAllLines(sourcePath).ToList();

        string header = lines[0];
        var data = lines.Skip(1).ToList();

        Random rnd = new Random();

        var shuffled = data.OrderBy(x => rnd.Next()).ToList();

        var testData = shuffled.Take(1000).ToList();

        var trainData = shuffled.Skip(1000).ToList();
        File.WriteAllLines(testPath, new[] { header }.Concat(testData));
        File.WriteAllLines(sourcePath, new[] { header }.Concat(trainData));

        Console.WriteLine("File splitted");
    }
}
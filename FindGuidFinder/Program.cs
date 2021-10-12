using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FindGuidFinder
{
    class Program
    {
        private static Dictionary<string,List<string>> guidRefPair = new Dictionary<string, List<string>>();
        private static List<string> guids = new List<string>();
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path you want to be searched.");
            string path = Console.ReadLine();
            while (true)
            {
                Console.WriteLine("Please enter guid of a file you want to find references.");
                string guidtemp= Console.ReadLine();
                if (string.IsNullOrEmpty(guidtemp))
                    continue;
                guids.Add(guidtemp);
                if (Directory.Exists(path))
                {
                    searchPath(new DirectoryInfo(path));
                }
                foreach (var guid in guidRefPair)
                {
                    Console.WriteLine($"Found {guidRefPair.SelectMany(x => x.Value).Count()} matches");
                    Console.WriteLine($"{guid.Key} =======> {String.Join(",  ", guid.Value)} \n");
                }
                guidRefPair.Clear();
            }
        }
        private static void searchPath(DirectoryInfo di)
        {
           
            foreach (var file in di.GetFiles())
            {
                if (file.FullName.EndsWith("prefab") || file.FullName.EndsWith("unity") || file.FullName.EndsWith(".asset"))
                {
                    readFile(file);
                }
            }
            foreach (var dir in di.GetDirectories())
            {
                searchPath(dir);
            }
        }
        private static void readFile(FileInfo fi)
        {
            var lines = File.ReadAllText(fi.FullName);
            foreach(var guid in guids)
            {
                if (lines.Contains(guid))
                {
                    if(guidRefPair.TryGetValue(guid, out List<string> value))
                    {
                        value.Add(fi.FullName);
                    }
                    else
                    {
                        guidRefPair[guid] = new List<string>() { fi.FullName };
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindGuidFinder
{
    class Program
    {
        private static List<string> keys = new List<string>();
        private static string guid;
        static void Main(string[] args)
        {
            Console.WriteLine("Enter path you want to be searched.");
            string path = Console.ReadLine();
            while (true)
            {
                Console.WriteLine("Please enter guid of a file you want to find references.");
                guid = Console.ReadLine();
                if (string.IsNullOrEmpty(guid))
                    continue;
                if (Directory.Exists(path))
                {
                    searchPath(new DirectoryInfo(path));
                }
                Console.WriteLine($"Found {keys.Count} matches");
                foreach (var prefab in keys)
                {
                    Console.WriteLine(prefab);
                }
                keys.Clear();
            }
        }
        private static void searchPath(DirectoryInfo di)
        {
           
            foreach (var file in di.GetFiles())
            {
                if (file.FullName.EndsWith("prefab") || file.FullName.EndsWith("unity")|| file.FullName.EndsWith(".asset"))
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
            var lines = File.ReadAllLines(fi.FullName);
            foreach (var line in lines)
            {
                if (line.Contains(guid))
                {
                    keys.Add(fi.FullName);
                    return;
                }
            }
        }
    }
}

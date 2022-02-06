using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;



namespace StrayToDo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> extensions = new List<string>() { "cpp", "h", "txt", "cs" };
            List<string> files = getAllFiles(location, extensions);
            files = findFilesWithToDoTag(files);
            foreach (var file in files)
                Console.WriteLine(file); 
            Console.ReadLine();
        }
    }
}

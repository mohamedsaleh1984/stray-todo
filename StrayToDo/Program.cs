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
            string location = @"D:\Problem-Solving-Hub";
            List<string> extensions = new List<string>() { "cpp", "h", "txt", "cs" };
            
            Console.WriteLine("fetching all files...");

            List<string> files = getAllFiles(location, extensions);

            Console.WriteLine(@"Searching in " + files.ToArray().Length + " file concurrently...");
            
            Console.WriteLine("Loading...");
            
            TimeSpan timeSpan = new TimeSpan(0,0, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
           
            files = findFilesWithToDoTagThreads(files);
            
            timeSpan=  timeSpan.Subtract(new TimeSpan(0, 0, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond));
            
            Console.WriteLine("Time consumed "+ timeSpan.ToString(@"mm\:ss\.fff"));

            Console.WriteLine("File list..");
            
            foreach (var file in files)
                Console.WriteLine(file); 
            
            Console.ReadLine();
        }

        private static List<string> getAllFiles(string directory, List<string> ext)
        {
            List<string> files = Directory.GetFiles(directory, "", SearchOption.AllDirectories).Where(x => !x.Contains("Debug")).ToList();
            files = filterFiles(files, ext);
            return files;
        }
        private static List<string> filterFiles(List<string> files, List<string> acceptedExtensions)
        {
            List<string> filesToReturn = new List<string>();
            foreach (var ext in acceptedExtensions)
            {
                filesToReturn.AddRange(files.Where(x => x.EndsWith(ext)).ToList());
            }
            return filesToReturn;
        }
        
        private static bool hasToDo(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var lowerCase = line.ToLower();
                if (lowerCase.Contains("todo:"))
                    return true;
            }
            return false;
        }

        private static List<string> findFilesWithToDoTagThreads(List<string> files)
        {
            List<Thread> threadsList = new List<Thread>();
            bool[] threadResult = new bool[files.Count];
            int index = 0;
            foreach (var item in files)
            {
                threadsList.Add(new Thread(() =>
                {
                    threadResult[index] = hasToDo(item);
                    index++;
                }));

            }

            foreach (var thread in threadsList)
            {
                thread.Start();
                thread.Join();
            }

            List<string> filesToReturn = new List<string>();
            index = 0;
            foreach (var item in threadResult)
            {
                if (item)
                {
                    filesToReturn.Add(files[index]);
                }
                index++;
            }
            return filesToReturn;
        }
    }
}

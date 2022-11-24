using Microsoft.Extensions.DependencyInjection;
using StrayToDo.Services.FileService;
using System;
using System.Collections.Generic;
using System.Text;

namespace StrayToDo
{
    public class Runner
    {
        private readonly ServiceProvider _serviceProvider = null;
        public Runner()
        {
            _serviceProvider = new ServiceCollection()
                           .AddSingleton<IFileService, FileService>()
                           .BuildServiceProvider();
        }

        public void Start(string[] args)
        {
            string location = args[0];
            List<string> extensions = new List<string>() { "cpp", "h", "txt", "cs" };

            var _fileService = _serviceProvider.GetService<IFileService>();

            Console.WriteLine("fetching all files...");
            
            List<string> filesPaths = _fileService.GetAllFiles(location);

            filesPaths = _fileService.FilterFiles(filesPaths, extensions);

            Console.WriteLine(@"Searching in " + filesPaths.ToArray().Length + " file concurrently...");

            Console.WriteLine("Loading...");
            
            TimeSpan timeSpan = new TimeSpan(0, 0, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            
            filesPaths = _fileService.FindFilesWithToDoTagThreads(filesPaths);
            
            timeSpan = timeSpan.Subtract(new TimeSpan(0, 0, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond));
            
            Console.WriteLine("Time consumed " + timeSpan.ToString(@"mm\:ss\.fff"));
            
            Console.WriteLine("File list..");
            
            foreach (var file in filesPaths)
                Console.WriteLine(file);

            Console.ReadLine();
        }

    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace StrayToDo.Services.FileService
{
    public class FileService : IFileService
    {
        /// <summary>
        /// Fetch all files in code base directory ignoring Debug folder
        /// </summary>
        /// <param name="directory">Code Base Directory</param>
        /// <returns>File List </returns>
        public List<string> GetAllFiles(string directory)
        {
            List<string> files = Directory.GetFiles(directory, "", SearchOption.AllDirectories).Where(x => !x.Contains("Debug")).ToList();
            return files;
        }

        /// <summary>
        /// Return files list with accepted extensions
        /// </summary>
        /// <param name="files">Files Paths</param>
        /// <param name="acceptedExtensions">Extension List</param>
        /// <returns></returns>
        public List<string> FilterFiles(List<string> files, List<string> acceptedExtensions)
        {
            List<string> filesToReturn = new List<string>();
            foreach (var ext in acceptedExtensions)
            {
                filesToReturn.AddRange(files.Where(x => x.EndsWith(ext)).ToList());
            }
            return filesToReturn;
        }

        /// <summary>
        /// Check if current file conatins ToDo
        /// </summary>
        /// <param name="filePath">File Path</param>
        /// <returns></returns>
        public bool HasToDo(string filePath)
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

        /// <summary>
        /// Return all files contains todo list.
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public List<string> FindFilesWithToDoTagThreads(List<string> files)
        {
            List<Thread> threadsList = new List<Thread>();
            bool[] threadResult = new bool[files.Count];
            int index = 0;
            foreach (var item in files)
            {
                threadsList.Add(new Thread(() =>
                {
                    threadResult[index] = HasToDo(item);
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
        //files = filterFiles(files, ext);
    }
}

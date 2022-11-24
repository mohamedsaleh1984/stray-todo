using System;
using System.Collections.Generic;
using System.Text;

namespace StrayToDo.Services.FileService
{
    public interface IFileService
    {
        List<string> GetAllFiles(string directory);
        List<string> FilterFiles(List<string> files, List<string> acceptedExtensions);
        bool HasToDo(string filePath);
        List<string> FindFilesWithToDoTagThreads(List<string> files);
    }

}

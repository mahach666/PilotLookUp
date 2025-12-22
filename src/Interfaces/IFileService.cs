using Ascon.Pilot.SDK;
using System.Collections.Generic;

namespace PilotLookUp.Interfaces
{
    public interface IFileService
    {
        string SaveFileToTemp(IFile file);
        IReadOnlyList<string> SaveFilesToFolder(IEnumerable<IFile> files, string folderPath);
    }
}


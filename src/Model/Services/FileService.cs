using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class FileService : IFileService
    {
        private readonly IFileProvider _fileProvider;

        public FileService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public string SaveFileToTemp(IFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));

            var tempRoot = Path.Combine(Path.GetTempPath(), "PilotLookUp");
            Directory.CreateDirectory(tempRoot);

            return SaveFileToFolderInternal(file, tempRoot);
        }

        public IReadOnlyList<string> SaveFilesToFolder(IEnumerable<IFile> files, string folderPath)
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            if (string.IsNullOrWhiteSpace(folderPath)) throw new ArgumentException("Folder path is empty.", nameof(folderPath));

            Directory.CreateDirectory(folderPath);

            var savedPaths = new List<string>();
            foreach (var file in files.Where(f => f != null))
            {
                savedPaths.Add(SaveFileToFolderInternal(file, folderPath));
            }

            return savedPaths;
        }

        private string SaveFileToFolderInternal(IFile file, string folderPath)
        {
            if (_fileProvider == null)
                throw new NotSupportedException("IFileProvider не доступен.");

            var fileName = GetSafeFileName(string.IsNullOrWhiteSpace(file.Name) ? file.Id.ToString() : file.Name);
            var destinationPath = GetUniquePath(Path.Combine(folderPath, fileName));

            using (var inputStream = _fileProvider.OpenRead(file))
            using (var outputStream = File.Create(destinationPath))
            {
                inputStream.CopyTo(outputStream);
            }

            return destinationPath;
        }

        private static string GetSafeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var safeName = new string(fileName.Select(ch => invalidChars.Contains(ch) ? '_' : ch).ToArray());
            return string.IsNullOrWhiteSpace(safeName) ? "file" : safeName;
        }

        private static string GetUniquePath(string path)
        {
            if (!File.Exists(path))
                return path;

            var directory = Path.GetDirectoryName(path) ?? "";
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            var extension = Path.GetExtension(path);

            for (var i = 1; i < 10_000; i++)
            {
                var candidate = Path.Combine(directory, $"{fileNameWithoutExtension} ({i}){extension}");
                if (!File.Exists(candidate))
                    return candidate;
            }

            return Path.Combine(directory, $"{fileNameWithoutExtension} ({Guid.NewGuid():N}){extension}");
        }
    }
}

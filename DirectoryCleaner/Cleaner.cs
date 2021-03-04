using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryCleaner
{
    public class Cleaner
    {
        private readonly string _basePath;
        private readonly string _mask;
        private readonly int _searchDay;
        private readonly int _daysBack;
        private readonly bool _testMode;

        public Cleaner(string basePath, string mask, int searchDay, int daysBack, bool testMode)
        {
            _basePath = basePath;
            _mask = mask;
            _searchDay = searchDay;
            _daysBack = daysBack;
            _testMode = testMode;
        }

        public List<FileInfo> CollectFiles()
        {
            List<FileInfo> files = new List<FileInfo>();
            List<FileInfo> filesToDelete = new List<FileInfo>();
            // Attempt to get a list of security permissions from the folder. 
            // This will raise an exception if the path is read only or do not have access to view the permissions.
            var filePaths = Directory.GetFiles(_basePath, _mask, SearchOption.AllDirectories);

            foreach (var filePath in filePaths)
            {
                var info = new FileInfo(filePath);
                files.Add(info);
            }

            var grouped = files.GroupBy(f => f.LastWriteTime.Year.ToString() + f.LastWriteTime.Month.ToString("00"),
                f => f,
                (key, fls) => new {Date = key, Files = fls.ToList()});

            foreach (var item in grouped)
            {
                var toDelete = GetFilesToDelete(item.Files);
                filesToDelete.AddRange(toDelete);
            }

            return filesToDelete;
        }

        public void Clean(IEnumerable<FileInfo> filesToDelete)
        {
            foreach (var file in filesToDelete)
            {
                if (_testMode)
                    Console.WriteLine(file.FullName);
                else
                    file.Delete();
            }
        }

        private IEnumerable<FileInfo> GetFilesToDelete(List<FileInfo> files)
        {
            var filesToDelete = new List<FileInfo>();
            var operationFiles = files.Where(f => f.LastWriteTime < DateTime.Now.Date.AddDays(_daysBack * -1)).ToList();

            if (operationFiles.Any() == false)
                return filesToDelete;

            var match = operationFiles.FirstOrDefault(f => f.LastWriteTime.Day == _searchDay);
            if (match is null)
                match = FindNextDayFile(operationFiles);

            filesToDelete.AddRange(operationFiles.Except(new[] {match}));

            return filesToDelete;
        }

        private FileInfo FindNextDayFile(IEnumerable<FileInfo> files)
        {
            FileInfo match;
            var i = 0;
            bool searchForward = files.Any(f => f.LastWriteTime.Day > _searchDay);
            do
            {
                i = searchForward ? i + 1 : i - 1;
                match = files.FirstOrDefault(f => f.LastWriteTime.Day == _searchDay + i);
            } while (match == null);

            return match;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VkRadio.LowCode.AppGenerator;

public static class FileHelper
{
    // Delete this method, if recursion works for DirectoryInfo.Delete
    public static void Clear(this DirectoryInfo directoryInfo)
    {
        directoryInfo
            .GetFiles()
            .ToList()
            .ForEach(fi => fi.Delete());

        directoryInfo
            .GetDirectories()
            .ToList()
            .ForEach(di =>
            {
                di.Clear();
                di.Delete();
            });
    }

    /// <summary>
    /// Recursive copy of all directory contents to another directory
    /// </summary>
    /// <param name="srcDirectoryInfo">Directory to copy</param>
    /// <param name="destDirectoryInfo">Destination directory</param>
    public static void CopyTo(this DirectoryInfo srcDirectoryInfo, DirectoryInfo destDirectoryInfo)
    {
        if (!destDirectoryInfo.Exists)
            destDirectoryInfo.Create();

        srcDirectoryInfo
            .GetFiles()
            .ToList()
            .ForEach(fi => fi.CopyTo(Path.Combine(destDirectoryInfo.FullName, fi.Name)));

        srcDirectoryInfo
            .GetDirectories()
            .ToList()
            .ForEach(di => di.CopyTo(destDirectoryInfo.CreateSubdirectory(di.Name)));
    }

    /// <summary>
    /// Check if file contents are identical
    /// </summary>
    /// <param name="file1Path">First file to compare</param>
    /// <param name="file2Path">Second file to compare</param>
    /// <returns></returns>
    public static bool FilesMatch(string file1Path, string file2Path)
    {
        var fi1 = new FileInfo(file1Path);
        var fi2 = new FileInfo(file2Path);
        var match = fi1.Length == fi2.Length;
        if (match)
        {
            var bytesFile1 = File.ReadAllBytes(file1Path);
            var bytesFile2 = File.ReadAllBytes(file2Path);
            match = bytesFile1.SequenceEqual(bytesFile2);
        }
        return match;
    }

    /// <summary>
    /// Check if file contents are identical
    /// </summary>
    /// <param name="file1Path">First file to compare</param>
    /// <param name="file2Path">Second file to compare</param>
    /// <returns></returns>
    public static async Task<bool> FilesMatchAsync(string file1Path, string file2Path)
    {
        var fi1 = new FileInfo(file1Path);
        var fi2 = new FileInfo(file2Path);
        var match = fi1.Length == fi2.Length;
        if (match)
        {
            var bytesFile1Task = File.ReadAllBytesAsync(file1Path);
            var bytesFile2Task = File.ReadAllBytesAsync(file2Path);
            var bytesFile1 = await bytesFile1Task;
            var bytesFile2 = await bytesFile2Task;
            match = bytesFile1.SequenceEqual(bytesFile2);
        }
        return match;
    }

    /// <summary>
    /// Transform array of FileInfo to sorted list of file names
    /// </summary>
    /// <param name="fileInfos">Array of FileInfo</param>
    /// <returns></returns>
    static List<string> GetSortedListOfFiles(FileInfo[] fileInfos) => fileInfos
        .Select(fi => fi.Name)
        .OrderBy(name => name)
        .ToList();

    /// <summary>
    /// Transform array of DirectoryInfo to sorted list of directory names
    /// </summary>
    /// <param name="directoryInfos">Array of DirectoryInfo</param>
    /// <returns></returns>
    //static List<string> GetSortedListOfDirectories(DirectoryInfo[] directoryInfos) => directoryInfos
    //    .Select(di => di.Name)
    //    .OrderBy(name => name)
    //    .ToList();

    /// <summary>
    /// Check whether file or directory names are matches
    /// <remarks>ATTENTION: Lists should be pre-sorted! Comparison is case sensitive</remarks>
    /// </summary>
    /// <param name="fileOrDirNames1">File or directory names in the first directory</param>
    /// <param name="fileOrDirNames2">File or directory names in the second directory</param>
    /// <returns></returns>
    public static bool FileOrDirNamesMatchPreSorted(IList<string> fileOrDirNames1, IList<string> fileOrDirNames2)
    {
        // If the number of names doesn't match, the function returns false
        if (fileOrDirNames1.Count != fileOrDirNames2.Count)
            return false;

        // Compare sorted lists
        for (var i = 0; i < fileOrDirNames1.Count; i++)
        {
            if (fileOrDirNames1[i] != fileOrDirNames2[i])
                return false;
        }

        // If got here, then differences are not found
        return true;
    }

    /// <summary>
    /// Byte-by-byte comparison of files in two directories
    /// <remarks>Subdirectories are ignored</remarks>
    /// </summary>
    /// <param name="filesInDir1"></param>
    /// <param name="filesInDir2"></param>
    /// <returns></returns>
    public static bool DirectoriesFilesMatch(FileInfo[] filesInDir1, FileInfo[] filesInDir2)
    {
        // Read and sort file names in both directories
        Task<List<string>>[] sortTasks =
        {
            Task<List<string>>.Factory.StartNew(() => GetSortedListOfFiles(filesInDir1)),
            Task<List<string>>.Factory.StartNew(() => GetSortedListOfFiles(filesInDir2))
        };
        var filesInDir1Sorted = sortTasks[0].Result;
        var filesInDir2Sorted = sortTasks[1].Result;

        // Compare file names in both directories
        bool match = FileOrDirNamesMatchPreSorted(filesInDir1Sorted, filesInDir2Sorted);
        if (match && filesInDir1.Length != 0)
        {
            string dir1Path = filesInDir1[0].DirectoryName;
            string dir2Path = filesInDir2[0].DirectoryName;

            Parallel.For(0, filesInDir1.Length, (i, loopState) =>
            {
                var fileName = filesInDir1[i].Name;
                var localMatch = FilesMatch(Path.Combine(dir1Path, fileName), Path.Combine(dir2Path, fileName));
                if (!localMatch)
                {
                    match = false;
                    loopState.Stop();
                }
            });
        }
        return match;
    }

    /// <summary>
    /// Byte-by-byte comparison of files in two directories
    /// <remarks>Subdirectories are ignored</remarks>
    /// </summary>
    /// <param name="filesInDir1"></param>
    /// <param name="filesInDir2"></param>
    /// <returns></returns>
    public static async Task<bool> DirectoriesFilesMatchAsync(FileInfo[] filesInDir1, FileInfo[] filesInDir2)
    {
        // Read and sort file names in both directories
        var sortTask1 = Task.Factory.StartNew(() => GetSortedListOfFiles(filesInDir1));
        var sortTask2 = Task.Factory.StartNew(() => GetSortedListOfFiles(filesInDir2));
        var filesInDir1Sorted = await sortTask1;
        var filesInDir2Sorted = await sortTask2;

        // Compare file names in both directories
        var match = FileOrDirNamesMatchPreSorted(filesInDir1Sorted, filesInDir2Sorted);
        if (match && filesInDir1.Length != 0)
        {
            var dir1Path = filesInDir1[0].DirectoryName;
            var dir2Path = filesInDir2[0].DirectoryName;

            // Here we cannot use async lamda, because Parallel.For loop actually only launches async operations,
            // but does not await for them. We need to implement complex low-level parallel code with the ability
            // to cancel all execution threads when the first file mismatch found. But it is too labor-expensive
            // for now, so yet just use sync file reads.
            Parallel.For(0, filesInDir1.Length, (i, loopState) =>
            {
                var fileName = filesInDir1[i].Name;
                var localMatch = FilesMatch(Path.Combine(dir1Path, fileName), Path.Combine(dir2Path, fileName));
                if (!localMatch)
                {
                    match = false;
                    loopState.Stop();
                }
            });
        }
        return match;
    }

    //public static bool SubdirectoriesMatch(DirectoryInfo[] subdirs1, DirectoryInfo[] subdirs2)
    //{
    //    // Сортируем имена поддиректорий одинаковым образом в обеих директориях.
    //    Task<List<string>>[] sortTasks =
    //    {
    //        Task<List<string>>.Factory.StartNew(() => GetSortedListOfDirectories(subdirs1)),
    //        Task<List<string>>.Factory.StartNew(() => GetSortedListOfDirectories(subdirs2))
    //    };
    //    List<string> subdirNames1Sorted = sortTasks[0].Result;
    //    List<string> subdirNames2Sorted = sortTasks[1].Result;

    //    // Проверяем, совпадают ли наименования поддиректорий.
    //    bool match = FileOrDirNamesMatchPreSorted(subdirNames1Sorted, subdirNames2Sorted);
    //    if (match)
    //    {
    //        // Сортируем списки DirectoryInfo в соответствии с именами директорий.
    //        Task<List<DirectoryInfo>>[] sortTasksDirs =
    //        {
    //            Task<List<DirectoryInfo>>.Factory.StartNew(() => subdirs1.OrderBy(di => di.Name).ToList()),
    //            Task<List<DirectoryInfo>>.Factory.StartNew(() => subdirs2.OrderBy(di => di.Name).ToList())
    //        };
    //        var subdirs1Sorted = sortTasksDirs[0].Result;
    //        var subdirs2Sorted = sortTasksDirs[1].Result;

    //        // Рекурсивно проверяем совпадение внутренностей каждой поддиректории.
    //        for (var i = 0; i < subdirs1.Length; i++)
    //        {
    //            match = DirectoriesMatch(subdirs1Sorted[i].FullName, subdirs2Sorted[i].FullName);
    //            if (!match)
    //                break;
    //        }
    //    }

    //    return match;
    //}

    //public static bool DirectoriesMatch(string in_dir1Path, string in_dir2Path)
    //{
    //    DirectoryInfo di1 = new DirectoryInfo(in_dir1Path);
    //    DirectoryInfo di2 = new DirectoryInfo(in_dir2Path);

    //    FileInfo[] filesInDir1 = di1.GetFiles();
    //    FileInfo[] filesInDir2 = di2.GetFiles();

    //    bool match = DirectoriesFilesMatch(filesInDir1, filesInDir2);
    //    if (match)
    //    {
    //        DirectoryInfo[] subdirs1 = di1.GetDirectories();
    //        DirectoryInfo[] subdirs2 = di2.GetDirectories();

    //        match = SubdirectoriesMatch(subdirs1, subdirs2);
    //    }
    //    return match;
    //}

    /// <summary>
    /// Getting path to file or folder that is relative to some other &quot;root&quot; path
    /// </summary>
    /// <param name="rootFullPath">Full &quot;root&quot; path</param>
    /// <param name="targetFullPath">Path to file or folder to that we need to get the relative path</param>
    /// <returns>Relative path in DOS/Windows notation (using \ as a dividers)</returns>
    public static string GetRelativePath(string rootFullPath, string targetFullPath)
    {
        var thisUri = new Uri(rootFullPath);
        var theirUri = new Uri(targetFullPath);
        var theirUriRel = thisUri.MakeRelativeUri(theirUri);
        return theirUriRel.ToString().Replace('/', '\\');
    }
}

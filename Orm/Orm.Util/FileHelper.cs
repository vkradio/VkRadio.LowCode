namespace VkRadio.Orm.Util;

public static class FileHelper
{
    public static void Clear(this DirectoryInfo thisDir)
    {
        var files = thisDir.GetFiles();

        foreach (var fi in files)
        {
            fi.Delete();
        }

        var subdirs = thisDir.GetDirectories();

        foreach (var di in subdirs)
        {
            di.Clear();
            di.Delete();
        }
    }

    /// <summary>
    /// Recursive copy all directory contents
    /// </summary>
    /// <param name="thisDir">Source directory</param>
    /// <param name="destination">Destination directory</param>
    public static void CopyTo(this DirectoryInfo thisDir, DirectoryInfo destination)
    {
        if (!destination.Exists)
        {
            destination.Create();
        }

        foreach (var fi in thisDir.GetFiles())
        {
            fi.CopyTo(Path.Combine(destination.FullName, fi.Name));
        }

        foreach (var di in thisDir.GetDirectories())
        {
            var destDi = destination.CreateSubdirectory(di.Name);
            di.CopyTo(destDi);
        }
    }

    /// <summary>
    /// Check file contents indentity
    /// </summary>
    /// <param name="file1Path">First file to compare</param>
    /// <param name="file2Path">Second file to compare Второй сравниваемый файл</param>
    /// <returns>true, if contents is identical in both files</returns>
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
    /// Convert a list of files in FileInfo array to an ordered list of file names
    /// </summary>
    /// <param name="filesInfos">File infos</param>
    /// <returns></returns>
    static List<string> GetSortedListOfFiles(FileInfo[] filesInfos) => filesInfos
        .Select(x => x.Name)
        .OrderBy(x => x)
        .ToList();

    /// <summary>
    /// Convert a list of directories in DirectoryInfo array to a sorted list of directory names
    /// </summary>
    /// <param name="directoryInfos">Directory infos</param>
    /// <returns></returns>
    static List<string> GetSortedListOfDirectories(DirectoryInfo[] directoryInfos) => directoryInfos
        .Select(x => x.Name)
        .OrderBy(x => x)
        .ToList();

    /// <summary>
    /// Sort DirectoryInfos by name
    /// </summary>
    /// <param name="directoryInfos"></param>
    /// <returns></returns>
    static List<DirectoryInfo> SortDirectoryInfos(DirectoryInfo[] directoryInfos) => directoryInfos
        .OrderBy(x => x.Name)
        .ToList();

    /// <summary>
    /// Are file or subderectory names the same
    /// <remarks>WARNING: Lists should be pre-sorted! Comparison is case-sensitive</remarks>
    /// </summary>
    /// <param name="fileOrDirNames1">File or subdirectory names in 1st directory</param>
    /// <param name="fileOrDirNames2">File or subdirectory names in 2nd directory</param>
    /// <returns>true, names are identical in both directories</returns>
    public static bool FileOrDirNamesMatchPreSorted(List<string> fileOrDirNames1, List<string> fileOrDirNames2)
    {
        // If number of names differs, return false
        if (fileOrDirNames1.Count != fileOrDirNames2.Count)
        {
            return false;
        }

        // Compare sorted lists
        for (var i = 0; i < fileOrDirNames1.Count; i++)
        {
            if (fileOrDirNames1[i] != fileOrDirNames2[i])
            {
                return false;
            }
        }

        // If went here, sequences are the same
        return true;
    }

    /// <summary>
    /// Byte-by-byte comparison of files in directories
    /// <remarks>Subdirectories are not compared</remarks>
    /// </summary>
    /// <param name="fileInfosInDir1"></param>
    /// <param name="fileInfosInDir2"></param>
    /// <returns></returns>
    public async static Task<bool> DirectoriesFilesMatch(FileInfo[] fileInfosInDir1, FileInfo[] fileInfosInDir2)
    {
        // Sort file names
        Task<List<string>>[] sortTasks =
        {
            Task<List<string>>.Factory.StartNew(() => GetSortedListOfFiles(fileInfosInDir1)),
            Task<List<string>>.Factory.StartNew(() => GetSortedListOfFiles(fileInfosInDir2))
        };

        var sortResults = await Task.WhenAll(sortTasks);

        var filesInDir1Sorted = sortResults[0];
        var filesInDir2Sorted = sortResults[1];

        // Compare file names
        var match = FileOrDirNamesMatchPreSorted(filesInDir1Sorted, filesInDir2Sorted);

        if (match)
        {
            if (fileInfosInDir1.Length != 0)
            {
                var dir1Path = fileInfosInDir1[0].DirectoryName;
                var dir2Path = fileInfosInDir2[0].DirectoryName;

                // Non-parallel variant of cycle
                //for (long i = 0; i < in_filesInDir1.LongLength; i++)
                //{
                //    string fileName = in_filesInDir1[i].Name;
                //    match = FilesMatch(Path.Combine(dir1Path, fileName), Path.Combine(dir2Path, fileName));
                //    if (!match)
                //        break;
                //}

                // See https://msdn.microsoft.com/en-us/library/dd460721.aspx
                Parallel.For(0, fileInfosInDir1.Length, (i, loopState) =>
                {
                    var fileName = fileInfosInDir1[i].Name;

                    var localMatch = FilesMatch(Path.Combine(dir1Path!, fileName), Path.Combine(dir2Path!, fileName));

                    if (!localMatch)
                    {
                        match = false;
                        loopState.Stop();
                    }
                });

                return match;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public async static Task<bool> SubdirectoriesMatch(DirectoryInfo[] subdirs1, DirectoryInfo[] subdirs2)
    {
        // Sort subdirectory names
        Task<List<string>>[] sortTasks =
        {
            Task<List<string>>.Factory.StartNew(() => GetSortedListOfDirectories(subdirs1)),
            Task<List<string>>.Factory.StartNew(() => GetSortedListOfDirectories(subdirs2))
        };

        var sortResults = await Task.WhenAll(sortTasks);

        var subdirNames1Sorted = sortResults[0];
        var subdirNames2Sorted = sortResults[1];

        // Check whether subdirectory names match
        var match = FileOrDirNamesMatchPreSorted(subdirNames1Sorted, subdirNames2Sorted);

        if (match)
        {
            // Сортируем списки DirectoryInfo в соответствии с именами директорий.
            Task<List<DirectoryInfo>>[] sortTasksDirs =
            {
                Task<List<DirectoryInfo>>.Factory.StartNew(() => SortDirectoryInfos(subdirs1)),
                Task<List<DirectoryInfo>>.Factory.StartNew(() => SortDirectoryInfos(subdirs2))
            };

            var sortDirResults = await Task.WhenAll(sortTasksDirs);

            var subdirs1Sorted = sortDirResults[0];
            var subdirs2Sorted = sortDirResults[1];

            // Recursively check match inside of each subdirectory
            for (var i = 0; i < subdirs1.Length; i++)
            {
                match = await DirectoriesMatch(subdirs1Sorted[i].FullName, subdirs2Sorted[i].FullName);

                if (!match)
                {
                    break;
                }
            }
        }

        return match;
    }

    public async static Task<bool> DirectoriesMatch(string dir1Path, string dir2Path)
    {
        var di1 = new DirectoryInfo(dir1Path);
        var di2 = new DirectoryInfo(dir2Path);

        var filesInDir1 = di1.GetFiles();
        var filesInDir2 = di2.GetFiles();

        bool match = await DirectoriesFilesMatch(filesInDir1, filesInDir2);

        if (match)
        {
            var subdirs1 = di1.GetDirectories();
            var subdirs2 = di2.GetDirectories();

            match = await SubdirectoriesMatch(subdirs1, subdirs2);
        }

        return match;
    }

    /// <summary>
    /// Get a path to a file or directory, relative to some other path (root)
    /// </summary>
    /// <param name="thisFullPath">Full path to a root, relative to that need to construct a relative path</param>
    /// <param name="theirFullPath">Full path to a file or directory to which need a relative path</param>
    /// <returns>Relative path in DOS/Windows form (with backslashes as path parts dividers)</returns>
    public static string GetRelativePath(string thisFullPath, string theirFullPath)
    {
        var thisUri = new Uri(thisFullPath);
        var theirUri = new Uri(theirFullPath);
        var theirUriRel = thisUri.MakeRelativeUri(theirUri);
        return theirUriRel.ToString().Replace('/', '\\');
    }
}

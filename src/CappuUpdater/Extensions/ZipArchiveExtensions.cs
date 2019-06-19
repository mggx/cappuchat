using System;
using System.IO;
using System.IO.Compression;

namespace CappuUpdater.Extensions
{
    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite = false)
        {
            if (archive == null)
                throw new ArgumentNullException(nameof(archive));

            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }

            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.Combine(destinationDirectoryName, file.FullName);
                if (file.Name?.Length == 0)
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                // create dirs
                var dirToCreate = destinationDirectoryName;
                for (var i = 0; i < file.FullName.Split('/').Length - 1; i++)
                {
                    var s = file.FullName.Split('/')[i];
                    dirToCreate = Path.Combine(dirToCreate, s);
                    if (!Directory.Exists(dirToCreate))
                        Directory.CreateDirectory(dirToCreate);
                }

                file.ExtractToFile(completeFileName, true);
            }
        }
    }
}

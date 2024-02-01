using SharpCompress.Archives;
using SharpCompress.Common;
using System.IO.Compression;

namespace UnityPackageExtractor
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // Check if a Unity package file is provided as a command-line argument
            if (args.Length == 0)
            {
                Console.WriteLine("Please drag the Unity package file onto this executable.");
                Thread.Sleep(10000); // Pause for 10 seconds before exiting
                return;
            }
            Console.WriteLine("This Could Take a While Please wait ");
            // Create a working directory
            Directory.CreateDirectory(".working");

            // Extract the Unity package file
            byte[] tarFileBytes = ExtractGzipFileAndReturnBytes(args[0]);
            ExtractTarFile(tarFileBytes, ".working");

            // Start the main loop to copy the files
            try
            {
                string directoryName = Path.GetFileNameWithoutExtension(args[0]);
                DoLoopOfDFile(directoryName);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in main loop: {e}");
            }

            // Clean up: delete the working directory
            Directory.Delete(".working", true);

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static void DoLoopOfDFile(string savepath)
        {
            // Ensure the save path exists, create it if not
            if (!Directory.Exists(savepath))
                Directory.CreateDirectory(savepath);

            // Loop through subdirectories in the working directory
            foreach (string subDirectory in Directory.GetDirectories(".working"))
            {
              

                // Get file path from the 'pathname' file in the current subdirectory
                string file_path = File.ReadAllText(Path.Combine(subDirectory, "pathname"));

                // Ensure the directory for the file exists in the save path
                string fileDirectory = Path.Combine(savepath, Path.GetDirectoryName(file_path));
                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);

                // Copy the asset file to the save path if it exists
                string assetFilePath = Path.Combine(subDirectory, "asset");
                if (File.Exists(assetFilePath))
                {
                    File.Copy(assetFilePath, Path.Combine(savepath, file_path));
                }
                else
                {
                    Console.WriteLine($"Failed to find the file associated with {file_path} note may have already been copyed");
                }
            }
        }

        static byte[] ExtractGzipFileAndReturnBytes(string gzipFilePath)
        {
            using (FileStream fileStream = new FileStream(gzipFilePath, FileMode.Open, FileAccess.Read))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
                {
                    // Extract the contents of the gzip file into a MemoryStream
                    gzipStream.CopyTo(memoryStream);
                }

                // Return the extracted content as a byte array
                return memoryStream.ToArray();
            }
        }


        static void ExtractTarFile(byte[] tarFileBytes, string extractFolderPath)
        {
            using (var stream = new MemoryStream(tarFileBytes))
            using (var archive = ArchiveFactory.Open(stream))
            {
                // Ensure the extract folder exists
                if (!Directory.Exists(extractFolderPath))
                {
                    Directory.CreateDirectory(extractFolderPath);
                }

                // Extract each entry in the tar archive
                foreach (var entry in archive.Entries.Where(e => !e.IsDirectory))
                {
                    entry.WriteToDirectory(extractFolderPath, new ExtractionOptions { ExtractFullPath = true, Overwrite = true });
                }
            }
        }
    }
}

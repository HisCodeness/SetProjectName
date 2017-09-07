using System;
using System.IO;
using System.Linq;

namespace SetProjectName
{
    class Program
    {
        static string target;
        static string starter;
        static string baseDir;

        static int countFile = 0;
        static int countFileContent = 0;
        static int countDir = 0;

        /// <summary>
        /// Excluded extension, to complete if required
        /// </summary>
        static string[] excludedExtension = { ".dll", ".jpg ", ".jpeg ", ".png", ".gif", ".zip", ".pdb", ".cache", ".exe", ".jar", ".mov", ".mp3", ".pfx", ".docx", ".xlsx", ".suo", ".ide" };

        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Get settings
            Console.WriteLine("Give the project name to replace :");
            starter = Console.ReadLine();
            Console.WriteLine("Give the project base folder to replace :");
            baseDir = Console.ReadLine();
            Console.WriteLine("Give the target project name :");
            target = Console.ReadLine();

            try
            {
                // Update
                UpdateFilesContent(baseDir);

                UpdatePath(baseDir);

                Console.WriteLine("{0} files with updated content", countFileContent);
                Console.WriteLine("{0} file names updated", countFile);
                Console.WriteLine("{0} directories updated", countDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something goes wrong while updating project name : {0}", ex.Message);
            }

            // That's all
            Console.ReadLine();
        }

        /// <summary>
        /// Update the content of the files
        /// </summary>
        /// <param name="currentDir"></param>
        static void UpdateFilesContent(string currentDir)
        {
            foreach (var file in Directory.EnumerateFiles(currentDir, "*", SearchOption.AllDirectories))
            {
                try
                {
                    if (!excludedExtension.Contains(Path.GetExtension(file)))
                    {
                        var content = File.ReadAllText(file);
                        content = content.Replace(starter, target);
                        File.WriteAllText(file, content);
                        countFileContent++;
                    }
                    else
                    {

                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Can't update file {0}", file);
                }
            }
        }

        /// <summary>
        /// Update files and directories name
        /// </summary>
        /// <param name="currentDir"></param>
        static void UpdatePath(string currentDir)
        {
            var updatedDir = currentDir.Replace(starter, target);

            if (currentDir.Contains(starter))
            {
                Directory.Move(currentDir, updatedDir);
                countDir++;
            }

            // Files
            foreach (var file in Directory.EnumerateFiles(updatedDir))
            {
                if (Path.GetFileName(file).Contains(starter))
                {
                    File.Move(file, file.Replace(starter, target));
                    countFile++;
                }
            }

            // Directories
            foreach (var dir in Directory.EnumerateDirectories(updatedDir))
            {
                if (dir.Contains(starter))
                {
                    updatedDir = dir.Replace(starter, target);
                    Directory.Move(dir, updatedDir);
                    countDir++;

                    UpdatePath(updatedDir);
                }
                else
                {
                    UpdatePath(dir);
                }
            }
        }
    }
}

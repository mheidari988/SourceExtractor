using System.IO;
using System.Text;

namespace SourceExtractor.Services
{
    public class DirectoryProcessor
    {
        public void WriteDirectoryTree(string directoryPath, StreamWriter writer)
        {
            StringBuilder structureBuilder = new StringBuilder();

            // Collect folder structure
            AppendDirectoryTree(directoryPath, structureBuilder, "");

            // Write as a single comment block
            writer.WriteLine("/* Directory Structure");
            writer.WriteLine(structureBuilder.ToString().TrimEnd()); // Trim trailing whitespace
            writer.WriteLine("*/");
            writer.WriteLine(); // Add an empty line for separation
        }

        private void AppendDirectoryTree(string directoryPath, StringBuilder builder, string indent)
        {
            // Add all files in the current directory
            foreach (var file in Directory.GetFiles(directoryPath, "*.cs"))
            {
                builder.AppendLine($"{indent}{Path.GetFileName(file)}");
            }

            // Add all subdirectories recursively
            foreach (var subDir in Directory.GetDirectories(directoryPath))
            {
                if (IsExcludedFolder(subDir)) continue;

                builder.AppendLine($"{indent}{Path.GetFileName(subDir)}/");
                AppendDirectoryTree(subDir, builder, indent + "    ");
            }
        }

        public bool ProcessDirectory(string directoryPath, StreamWriter writer)
        {
            bool hasFiles = false;

            // Process all .cs files in the current directory
            foreach (var file in Directory.GetFiles(directoryPath, "*.cs"))
            {
                writer.WriteLine($"// File: {file}");
                writer.WriteLine(File.ReadAllText(file));
                writer.WriteLine();
                hasFiles = true;
            }

            // Process all subdirectories recursively
            foreach (var subDir in Directory.GetDirectories(directoryPath))
            {
                if (IsExcludedFolder(subDir)) continue;

                if (ProcessDirectory(subDir, writer))
                {
                    hasFiles = true;
                }
            }

            return hasFiles;
        }

        private bool IsExcludedFolder(string path)
        {
            string folderName = Path.GetFileName(path).ToLowerInvariant();
            return folderName == "bin" || folderName == "obj";
        }
    }
}

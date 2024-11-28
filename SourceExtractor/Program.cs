using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string projectPath = string.Empty;
        string outputPath = string.Empty;

        // Parse arguments
        foreach (var arg in args)
        {
            if (arg.StartsWith("--output-path="))
            {
                outputPath = arg.Substring("--output-path=".Length);
            }
            else
            {
                projectPath = arg;
            }
        }

        // Default project path to the current directory if not specified
        if (string.IsNullOrEmpty(projectPath))
        {
            projectPath = Directory.GetCurrentDirectory();
        }

        if (!Directory.Exists(projectPath))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: The path '{projectPath}' does not exist.");
            Console.ResetColor();
            return;
        }

        string directoryName = new DirectoryInfo(projectPath).Name;
        string outputFileName = $"{directoryName}.txt";

        // Set the output file path
        string outputFilePath = !string.IsNullOrEmpty(outputPath)
            ? Path.Combine(outputPath, outputFileName)
            : Path.Combine(projectPath, outputFileName);

        try
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Process: Starting to combine .cs files into a single file...");
                Console.ResetColor();

                // Write the folder structure
                AppendDirectoryStructure(projectPath, writer);

                bool hasFiles = ProcessDirectory(projectPath, writer);

                if (!hasFiles)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning: No .cs files found. Skipping output file creation.");
                    Console.ResetColor();
                    writer.Close(); // Ensure file is not locked before deleting
                    File.Delete(outputFilePath);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"All .cs files have been combined into: {outputFilePath}");
                    Console.ResetColor();
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.ResetColor();
        }
        finally
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static void AppendDirectoryStructure(string baseDirectory, StreamWriter writer)
    {
        // Start with a comment block for the directory structure
        writer.WriteLine("/*");
        WriteDirectoryTree(baseDirectory, writer, "");
        writer.WriteLine("*/");
        writer.WriteLine(); // Add an empty line after the structure
    }

    static void WriteDirectoryTree(string directoryPath, StreamWriter writer, string indent)
    {
        // Skip bin and obj folders
        if (IsExcludedFolder(directoryPath)) return;

        // Get all files in the current directory
        var files = Directory.GetFiles(directoryPath, "*.cs");
        foreach (var file in files)
        {
            writer.WriteLine($"{indent}{Path.GetFileName(file)}");
        }

        // Get all subdirectories and recursively process them
        var subdirectories = Directory.GetDirectories(directoryPath);
        foreach (var subdirectory in subdirectories)
        {
            if (IsExcludedFolder(subdirectory)) continue;

            string folderName = Path.GetFileName(subdirectory);
            writer.WriteLine($"{indent}{folderName}/");
            WriteDirectoryTree(subdirectory, writer, indent + "    ");
        }
    }

    static bool ProcessDirectory(string directoryPath, StreamWriter writer)
    {
        // Skip bin and obj folders
        if (IsExcludedFolder(directoryPath)) return false;

        bool hasFiles = false;

        // Get all .cs files in the current directory
        string[] csFiles = Directory.GetFiles(directoryPath, "*.cs");
        foreach (var file in csFiles)
        {
            AppendFileContent(file, writer);
            hasFiles = true;
        }

        // Recursively process subdirectories
        string[] subDirectories = Directory.GetDirectories(directoryPath);
        foreach (var subDirectory in subDirectories)
        {
            if (IsExcludedFolder(subDirectory)) continue;

            if (ProcessDirectory(subDirectory, writer))
            {
                hasFiles = true;
            }
        }

        return hasFiles;
    }

    static void AppendFileContent(string filePath, StreamWriter writer)
    {
        // Get the base directory of the project
        string baseDirectory = Directory.GetCurrentDirectory();

        // Calculate the relative path
        string relativePath = Path.GetRelativePath(baseDirectory, filePath)
                              .Replace(Path.DirectorySeparatorChar, '/'); // Normalize to "/"

        // Write the relative file path with the normalized separator
        writer.WriteLine($"// File: {relativePath}");
        writer.WriteLine(File.ReadAllText(filePath));
        writer.WriteLine(); // Add an empty line between files
    }

    static bool IsExcludedFolder(string path)
    {
        string folderName = Path.GetFileName(path).ToLowerInvariant();
        return folderName == "bin" || folderName == "obj";
    }
}

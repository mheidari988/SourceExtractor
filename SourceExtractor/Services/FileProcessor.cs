using SourceExtractor.Models;
using SourceExtractor.Utils;
using System.IO;

namespace SourceExtractor.Services;
public class FileProcessor : IFileProcessor
{
    private readonly DirectoryProcessor _directoryProcessor;

    public FileProcessor()
    {
        _directoryProcessor = new DirectoryProcessor();
    }

    public void Process(ProjectOptions options)
    {
        string projectPath = options.ProjectPath ?? Directory.GetCurrentDirectory();
        if (!Directory.Exists(projectPath))
        {
            ConsoleHelper.WriteError($"Error: The path '{projectPath}' does not exist.");
            return;
        }

        string directoryName = new DirectoryInfo(projectPath).Name;
        string outputFilePath = Path.Combine(
            options.OutputPath ?? projectPath,
            $"{directoryName}.txt"
        );

        try
        {
            using StreamWriter writer = new StreamWriter(outputFilePath);
            ConsoleHelper.WriteInfo("Starting to combine .cs files...");

            // Write folder structure as a single block
            _directoryProcessor.WriteDirectoryTree(projectPath, writer);

            if (!options.DirectoryOnly)
            {
                bool hasFiles = _directoryProcessor.ProcessDirectory(projectPath, writer);

                if (!hasFiles)
                {
                    ConsoleHelper.WriteWarning("No .cs files found. Skipping output file creation.");
                    writer.Flush(); // Ensure all writes are flushed
                    writer.Close(); // Explicitly close the writer before deletion
                    File.Delete(outputFilePath);
                    return;
                }
            }

            ConsoleHelper.WriteSuccess($"Output file created: {outputFilePath}");
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"An error occurred: {ex.Message}");
        }
    }
}

using SourceExtractor.Models;
using SourceExtractor.Services;

class Program
{
    static void Main(string[] args)
    {
        ProjectOptions options = ParseArguments(args);

        IFileProcessor fileProcessor = new FileProcessor();
        fileProcessor.Process(options);

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
    private static ProjectOptions ParseArguments(string[] args)
    {
        string projectPath = null!;
        string outputPath = null!;
        bool dirOnly = false;

        foreach (var arg in args)
        {
            if (arg.StartsWith("--output-path="))
            {
                outputPath = arg.Substring("--output-path=".Length);
            }
            else if (arg == "--dir-only")
            {
                dirOnly = true;
            }
            else
            {
                projectPath = arg;
            }
        }

        return new ProjectOptions
        {
            ProjectPath = projectPath,
            OutputPath = outputPath,
            DirectoryOnly = dirOnly
        };
    }

}

using SourceExtractor.Models;

namespace SourceExtractor.Services;
public interface IFileProcessor
{
    void Process(ProjectOptions options);
}

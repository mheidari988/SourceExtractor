Here's a comprehensive `README.md` file for your project:

---

# srcx: Source Extractor for C# Projects

`srcx` is a lightweight command-line tool designed to extract and combine all `.cs` files from a given directory (and its subdirectories) into a single `.txt` file. The tool also provides a directory structure overview at the beginning of the output file for better context.

## Features

- Recursively scans a directory for `.cs` files.
- Combines all `.cs` file contents into a single `.txt` file.
- Includes a directory structure overview at the top of the output.
- Supports optional custom output paths via `--output-path`.
- Cross-platform support (Windows, Linux, macOS).
- Native AOT (Ahead-of-Time) compilation for fast execution and standalone binaries.

---
## Download

You can download the latest version of `srcx` from the link below:

[Download srcx.exe](https://mheidari988.github.io/downloads/srcx.exe)

---

## Installation

### Prerequisites

1. [.NET 9 SDK](https://dotnet.microsoft.com/) or lower (if not using AOT or features exclusive to .NET 9).
2. Windows, Linux, or macOS environment.

---

### Build the Project

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/srcx.git
   cd srcx
   ```

2. Publish the project with Native AOT (optional):
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained -p:PublishAot=true -o ./publish
   ```

3. Move the `srcx` binary to a directory included in your system's `PATH` for global usage:
   - On **Windows**:
     ```bash
     move .\publish\srcx.exe C:\Path\To\Global\Bin\
     ```
   - On **Linux/Mac**:
     ```bash
     mv ./publish/srcx /usr/local/bin/
     chmod +x /usr/local/bin/srcx
     ```

---

## Usage

### Basic Usage

Navigate to the directory containing your C# project and run:
```bash
srcx
```

This creates a `.txt` file named after the root directory, containing all `.cs` files and the directory structure.

### Specify a Custom Directory

Provide a specific directory as an argument:
```bash
srcx /path/to/project
```

### Specify a Custom Output Path

Use the `--output-path` option to specify where the `.txt` file should be saved:
```bash
srcx /path/to/project --output-path=/path/to/output
```

---

## Example

### Input Directory Structure
```
MyApp.Domain/
    Entities/
        Common/
            AuditableEntity.cs
    Services/
        UserService.cs
```

### Command
```bash
srcx /path/to/MyApp.Domain --output-path=/path/to/output
```

### Output File (`MyApp.Domain.txt`)
```txt
/*
MyApp.Domain/
    Entities/
        Common/
            AuditableEntity.cs
    Services/
        UserService.cs
*/

// File: Entities/Common/AuditableEntity.cs
<Contents of AuditableEntity.cs>

// File: Services/UserService.cs
<Contents of UserService.cs>
```

---

## Development

### Prerequisites
- Install the [.NET 9 SDK](https://dotnet.microsoft.com/).

### Running the Project Locally
1. Build and run the project:
   ```bash
   dotnet run
   ```

2. Test with a sample directory:
   ```bash
   dotnet run -- /path/to/sample/project --output-path=/path/to/output
   ```

---

## Publishing

### Native AOT Compilation
To generate a self-contained native binary:
```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishAot=true -o ./publish
```

### Cross-Platform Binaries
Replace `win-x64` with the appropriate runtime identifier:
- **Linux**: `linux-x64`
- **macOS**: `osx-x64`

For example:
```bash
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishAot=true -o ./publish
```

---

## Contributing

Contributions are welcome! Feel free to:
- Submit a bug report or feature request via [GitHub Issues](https://github.com/yourusername/srcx/issues).
- Open a pull request for improvements or new features.

---

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for more details.

---

## Acknowledgments

Special thanks to the .NET community for building a robust and versatile framework that powers tools like this.

---

### Notes for Contributors
- **Ensure Cross-Platform Compatibility**: Test on Windows, Linux, and macOS.
- **Follow the Coding Style**: Align with the existing conventions and patterns in the project.
- **Write Tests**: Include unit tests for any new functionality.

---

Feel free to customize links and placeholders like `yourusername` with your specific project details. This `README.md` provides comprehensive documentation for installation, usage, and contribution.
using System.Management.Automation;

namespace LogixShell;

public static class Error
{
    public static ErrorRecord FileNotFound(string filePath)
    {
        var exception = new FileNotFoundException($"Project file not found at path: {filePath}");
        return new ErrorRecord(exception, "FileNotFound", ErrorCategory.ObjectNotFound, filePath);
    }
}
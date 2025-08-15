using System.IO;

namespace Lapidary;

public static class OnModuleLoad
{
    public static void EnsureGemBuilderFilesExist(string currentDirectory)
    {
        var gcitsPath = Path.Combine(currentDirectory, Definitions.Version.GciTsDLL);
        if (!File.Exists(gcitsPath))
        {
            // Always required
            throw new InvalidOperationException($"Required GCITS file not found: {gcitsPath}");
        }

        var sslPath = Path.Combine(currentDirectory, Definitions.Version.SslDLL);
        if (!File.Exists(sslPath))
        {
            // Required for X509
            throw new InvalidOperationException($"Required SSL file not found: {sslPath}");
        }
    }
}

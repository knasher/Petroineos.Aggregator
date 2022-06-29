namespace Petroineos.Aggregator.Service.Utils;

public static class FileUtils
{
    public static string AddPath(this string filename, string? path)
    {
        if (filename == null) throw new ArgumentNullException(nameof(filename));
        
        if (string.IsNullOrWhiteSpace(path))
            return filename;

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        return Path.Combine(path, filename);
    }

    public static string GenerateFilename(this string format, DateTime dateTime)
    {
        if (format == null) throw new ArgumentNullException(nameof(format));
        
        return string.Format(format, dateTime.ToString("yyyyMMdd_HHmm"));
    }
}
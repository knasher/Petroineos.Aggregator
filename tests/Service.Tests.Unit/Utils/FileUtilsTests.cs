using Petroineos.Aggregator.Service.Utils;
#pragma warning disable CS8604

namespace Petroineos.Aggregator.Service.Tests.Unit.Utils;

public class FileUtilsTests
{
    [Fact]
    public void AddPath_WithNullFilename_ShouldThrowArgumentNullException()
    {
        string? filename = null;

        Action act = () => _ = filename.AddPath("foo");
        
        act.Should().Throw<ArgumentNullException>().WithMessage($"Value cannot be null. (Parameter '{nameof(filename)}')");
    }

    [Fact]
    public void AddPath_WithFilenameAndNoPath_ShouldReturnSameFilename()
    {
        const string filename = "foo.bar";

        filename.AddPath(null).Should().Be(filename);
    }
    
    [Fact]
    public void AddPath_WithFilenameAndPath_ShouldReturnFileAndPath()
    {
        const string filename = "foo.bar";
        var path = Path.GetTempPath();

        filename.AddPath(path).Should().Be(Path.Combine(path, filename));
    }
    
    [Fact]
    public void AddPath_WithFilenameAndPath_ShouldCreateDirectory()
    {
        const string filename = "foo.bar";
        var path = Path.Combine(Path.GetTempPath(), $"foo_{DateTime.Now.ToString("yyyyMMddHHmmss")}");

        Directory.Exists(path).Should().Be(false);
        
        filename.AddPath(path);

        Directory.Exists(path).Should().Be(true);
        Directory.Delete(path);
    }

    [Fact]
    public void GenerateFilename_WithNullFormat_ShouldThrowArgumentNullException()
    {
        string? format = null;

        Action act = () => _ = format.GenerateFilename(DateTime.Now);
        
        act.Should().Throw<ArgumentNullException>().WithMessage($"Value cannot be null. (Parameter '{nameof(format)}')");
    }
    
    [Fact]
    public void GenerateFilename_WithFormat_ShouldReturnFilename()
    {
        var format = "File_{0}.csv";
        var dateTime = DateTime.Now;

        format.GenerateFilename(dateTime).Should().Be(string.Format(format, dateTime.ToString("yyyyMMdd_HHmm")));
    }
}
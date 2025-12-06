static void LogToFile<T>(string filePath, T content)
{
    using var writer = new StreamWriter(filePath, append: true);
    writer.WriteLine(content?.ToString() ?? "null");
}

static void LogToFile<T>(T content)
{
    var timestamp = DateTime.Now.ToString("yyyyMMdd");
    var filePath = Path.Combine("logs", $"log_{timestamp}.txt");
    Directory.CreateDirectory("logs");
    LogToFile(filePath, content);
}
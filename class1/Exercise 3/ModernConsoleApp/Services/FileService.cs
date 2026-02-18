using System.Configuration;
using System.IO;

public class FileService
{
    public void Write(string content)
    {
        var path = ConfigurationManager.AppSettings["OutputFilePath"];
        File.AppendAllText(path, content + "\n");
    }
}

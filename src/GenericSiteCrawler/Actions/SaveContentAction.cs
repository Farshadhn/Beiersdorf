using GenericSiteCrawler.Core.Interfaces;

namespace GenericSiteCrawler.Actions;

public class SaveContentAction : IAction
{
    public async Task Invoke(string pageName, string content)
    {
        pageName = ((IAction)this).SantizeName(pageName);
        await SavePageToFile(pageName, content);

    }

    static async Task SavePageToFile(string fileName, string content)
    {
        var filePath = Path.Combine("Pages", fileName + ".txt");
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        await File.WriteAllTextAsync(filePath, content);
        Console.WriteLine("Page saved: " + fileName);
    }
}

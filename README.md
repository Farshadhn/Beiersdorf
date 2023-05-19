# Crawler Library

The Crawler Library is a generic site crawler that provides basic services to traverse a complete site tree. Given a start link, it visits each page of the site that is reachable via one or more hops from the start page within the same domain. The library allows the consumer to execute a custom action on each of the pages found.

## Features

- Crawls a website by visiting each page within the same domain
- Executes a custom action on each page found
- Handles relative and absolute URLs to ensure proper traversal

## Usage

1. Create an instance of the `ISiteCrawler` class by using DI, providing the start link and an action to be executed on each page:

   ```csharp
   crawler.SetPageAddress("https://www.beiersdorf.com/"); 


2. Define your custom action method that will be executed on each page. The method should take two parameters: pageName and its HTML content. For example:
   ```csharp
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
3. Add Your Action To ActionType Enum
   ```csharp
   public enum ActionType
   {
    SaveContent
   }

4. Set Action
   ```csharp
   crawler.SetAction(ActionType.SaveContent);
5. Crawl!!
   ```csharp
   await crawler.CrawlAsync();  

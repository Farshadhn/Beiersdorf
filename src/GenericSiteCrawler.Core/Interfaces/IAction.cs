namespace GenericSiteCrawler.Core.Interfaces;

public interface IAction
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageName">To Save in a file if needed</param>
    /// <param name="content">Page Content</param>
    /// <returns></returns>
    Task Invoke(string pageName, string content);
    string SantizeName(string pageName) => pageName.Replace("https://", "").Replace("http://", "").Replace("/", "_").Replace(".", "_");
}

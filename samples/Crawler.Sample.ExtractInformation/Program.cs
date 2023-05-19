using GenericSiteCrawler.Core.Constants;
using GenericSiteCrawler.Core.Interfaces;
using GenericSiteCrawler.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddGenericSiteCrawler(); })
    .Build();


var crawler = host.Services.GetRequiredService<ISiteCrawler>();

crawler.SetPageAddress("https://www.beiersdorf.com/"); 
crawler.SetAction(ActionType.SaveContent);
await crawler.CrawlAsync(); 

Console.WriteLine("Crawling completed!");


 
 
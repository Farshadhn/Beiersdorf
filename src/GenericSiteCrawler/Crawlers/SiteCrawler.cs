using GenericSiteCrawler.Actions;
using GenericSiteCrawler.Core.Constants;
using GenericSiteCrawler.Core.Interfaces;
using HtmlAgilityPack;
using System.Net.Mail;

namespace GenericSiteCrawler.Crawlers
{
    public class SiteCrawler : ISiteCrawler
    {
        #region ... Functions ...
        public void SetPageAddress(string pageAddress)
        {
            PageAddress = pageAddress;
            domain = new Uri(PageAddress).Host;
            httpClient = HttpClientFactory.CreateClient();
        }

        public void SetAction(ActionType actionType)
        {
            crawlAction = actionType switch
            {
                ActionType.SaveContent => new SaveContentAction(),
                _ => throw new NotImplementedException()
            };
        }

        private bool CheckPrerequisite()
        => !string.IsNullOrEmpty(PageAddress) && crawlAction != default;



        #endregion

        public SiteCrawler(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }

        #region ... Properties ...
        public string PageAddress { get; private set; }
        public IAction crawlAction { get; private set; }
        private HashSet<string> VisitedLinks { get; set; }
        private string domain { get; set; } // If PageAddress is absolute, we need to find all other pages from its main address
        public IHttpClientFactory HttpClientFactory { get; init; }
        private HttpClient httpClient { get; set; }
        #endregion


        /// <summary>
        /// This is called from OutSide
        /// </summary>
        /// <returns></returns>
        public async Task CrawlAsync()
        {
            var IsItEligibleToCrawl = CheckPrerequisite();

            if (!IsItEligibleToCrawl)
                return;



            VisitedLinks = new HashSet<string>();
            await CrawlPageAsyncRecursively(PageAddress);
        }


        /// <summary>
        /// This Will be called recursively.
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        private async Task CrawlPageAsyncRecursively(string link)
        {
            if (VisitedLinks.Contains(link))
                return;

            VisitedLinks.Add(link);
            Console.WriteLine("Visiting: " + link);

            try
            {
                var response = await httpClient.GetAsync(link);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error: " + link);
                    return;
                }


                var pageContent = await response.Content.ReadAsStringAsync();
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(pageContent);

                await crawlAction.Invoke(link, pageContent);

                var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]");
                if (links is null)
                    return;

                foreach (var anchor in links)
                {
                    var href = anchor.GetAttributeValue("href", ""); 
                    var absoluteUrl = GetAbsoluteUrl(link, href);
                    var isItInThisSite = new Uri(absoluteUrl).Host.Equals(domain);
                    var isItEligibleUrl = !string.IsNullOrEmpty(absoluteUrl) && isItInThisSite;
                    if (isItEligibleUrl)
                        await CrawlPageAsyncRecursively(absoluteUrl);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        private string GetAbsoluteUrl(string baseUrl, string relativeUrl)
        {
            var pureUrl = relativeUrl.Split("#")[0];
            if (Uri.TryCreate(new Uri(baseUrl), pureUrl, out var absoluteUri))
                return absoluteUri.AbsoluteUri;

            return string.Empty;
        }


    }
}
using GenericSiteCrawler.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericSiteCrawler.Core.Interfaces;

public interface ISiteCrawler
{
    void SetPageAddress(string link);
    void SetAction(ActionType actionType);
    Task CrawlAsync();
}

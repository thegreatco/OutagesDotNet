using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using HtmlAgilityPack;

using Newtonsoft.Json;

namespace OutagesDotNet.Peco
{
    /// <summary>
    /// The PECO Energy provider.
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Gets the current outage data from the electrical supplier.
        /// </summary>
        /// <param name="sa"> The service area. </param>
        /// <returns> An await-able <see cref="Task"/> containing the outage results. </returns>
        public async Task<IEnumerable<Outage>> GetCurrentData(ServiceArea sa)
        {
            var client = new HttpClient();
            const string Url = "https://www.peco.com/CustomerService/OutageCenter/OutageMap/Pages/CountyDetails.aspx?County={0}";
            var uri = new Uri(string.Format(Url, sa));
            var htmlStream = await client.GetStreamAsync(uri);
            return ParseResults(htmlStream, sa);
        }

        private static IEnumerable<Outage> ParseResults(Stream htmlStream, ServiceArea sa)
        {
            var outages = new List<Outage>();
            var doc = new HtmlDocument();
            doc.Load(htmlStream);
            foreach (var tables in doc.DocumentNode.SelectNodes("//div[contains(@id,'outageMapTownshipInformation')]//table").Where(x => x.SelectNodes("thead") != null))
            {
                // if (tables.SelectNodes("thead") == null) continue;
                foreach (var node in tables.SelectNodes("tbody/tr"))
                {
                    var subNodes = node.SelectNodes("td");
                    int custsOut;
                    int.TryParse(subNodes[1].InnerText, out custsOut);
                    var outage = new Outage
                        {
                            City = subNodes[0].InnerText,
                            State = "Pennsylvania",
                            County = sa.ToString(),
                            Out = custsOut,
                            Served = null
                        };
                    outages.Add(outage);
                }
            }
            return outages;
        }
    }
}

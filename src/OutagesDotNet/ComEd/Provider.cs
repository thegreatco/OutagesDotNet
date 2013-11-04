using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ServiceStack.Text;

namespace OutagesDotNet.ComEd
{
    /// <summary>
    /// The provider for the Commonwealth Edison Utility in Illinois.
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Gets the current outage data from the electrical supplier.
        /// </summary>
        /// <param name="sa"> The service area. </param>
        /// <returns> An await-able <see cref="Task"/> containing the outage results. </returns>
        public async Task<IEnumerable<ProviderStatus>> GetCurrentData(ServiceArea sa)
        {
            var regex = new Regex(@"\d{4}_\d{2}_\d{2}_\d{2}_\d{2}_\d{2}\w{0,4}(?=<)");
            var xmlUri = new Uri(string.Format("https://s3.amazonaws.com/stormcenter.comed.com/data/alerts/metadata.xml"));
            var client = new HttpClient();
            var xmlString = await client.GetStringAsync(xmlUri);
            var lastUpdate = regex.Match(xmlString).Value;
            const string Url = "https://s3.amazonaws.com/stormcenter.comed.com/data/interval_generation_data/{0}/report2.js?timestamp={1}";
            var uri = new Uri(string.Format(Url, lastUpdate, SecondsSinceEpoch()));
            var json = await Get<dynamic>(uri);
            return ParseResults(json);
        }

        private static IEnumerable<ProviderStatus> ParseResults(dynamic results)
        {
            var states = results.file_data.curr_custs_aff;
            var outages = new List<ProviderStatus>();
            foreach (var area in states.areas)
            {
                foreach (var city in area.areas)
                {
                    var outage = new ProviderStatus
                    {
                        State = "Illinois",
                        City = city.area_name,
                        County = "Unknown",
                        Served = city.total_custs,
                        Out = city.custs_out,
                        Provider = "ComEd"
                    };
                    outages.Add(outage);
                }
            }

            return outages;
        }

        private static string SecondsSinceEpoch()
        {
            var diff = Math.Round((DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds, 0);
            return diff.ToString(CultureInfo.InvariantCulture);
        }

        private static async Task<T> Get<T>(Uri requestUri)
        {
            using (var httpClient = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (var response = await httpClient.SendAsync(request))
            {
                if (response.IsSuccessStatusCode) return (await response.Content.ReadAsStringAsync()).FromJson<T>();
                else throw new Exception("Error deserializing from provider");
            }
        }
    }
}

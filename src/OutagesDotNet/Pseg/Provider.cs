using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using ServiceStack.Text;

namespace OutagesDotNet.Pseg
{
    /// <summary>
    /// The PSE G provider.
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Gets the current outage data from the electrical supplier.
        /// </summary>
        /// <param name="sa"> The service area. Not used by all providers. </param>
        /// <returns> An await-able <see cref="Task"/> containing the outage results.  </returns>
        public async Task<IEnumerable<ProviderStatus>> GetCurrentData(ServiceArea sa)
        {
            const string Url = "http://www.pseg.com/outagemap/Customer%20Outage%20Application/Web%20Pages/GML/State.gml?randomVar={0}";
            var uri = new Uri(string.Format(Url, RandomVar()));
            var json = await Get<dynamic>(uri);
            return ParseResults(json);
        }

        private static IEnumerable<ProviderStatus> ParseResults(dynamic results)
        {
            var states = results.file_data.curr_custs_aff;
            var outages = new List<ProviderStatus>();
            foreach (var state in states.areas)
            {
                foreach (var county in state.areas)
                {
                    foreach (var city in county.areas)
                    {
                        var outage = new ProviderStatus
                        {
                            State = state.area_name,
                            City = city.area_name,
                            County = county.area_name,
                            Served = city.total_custs,
                            Out = city.custs_out,
                            Provider = "PSE&G"
                        };
                        outages.Add(outage);
                    }
                }
            }

            return outages;
        }

        private static string RandomVar()
        {
            var diff = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
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

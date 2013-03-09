using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace OutagesDotNet
{
    /// <summary>
    /// The PSE G provider.
    /// </summary>
    public class PsegProvider
    {
        /// <summary>
        /// Gets the current outage data from the electrical supplier.
        /// </summary>
        /// <returns> An await-able <see cref="Task"/> containing the outage results. </returns>
        public async Task<IEnumerable<Outage>> GetCurrentData()
        {
            const string Url = "http://www.pseg.com/outagemap/Customer%20Outage%20Application/Web%20Pages/GML/State.gml?randomVar={0}";
            var uri = new Uri(string.Format(Url, RandomVar()));
            var client = new HttpClient();
            var httpClientStream = await client.GetStreamAsync(uri);
            var json = new JsonSerializer().Deserialize<dynamic>(new JsonTextReader(new StreamReader(httpClientStream)));
            return ParseResults(json);
        }

        private static IEnumerable<Outage> ParseResults(dynamic results)
        {
            var states = results.file_data.curr_custs_aff;
            var outages = new List<Outage>();
            foreach (var state in states.areas)
            {
                foreach (var county in state.areas)
                {
                    foreach (var city in county.areas)
                    {
                        var outage = new Outage
                        {
                            State = state.area_name,
                            City = city.area_name,
                            County = county.area_name,
                            Served = city.total_custs,
                            Out = city.custs_out
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
    }
}

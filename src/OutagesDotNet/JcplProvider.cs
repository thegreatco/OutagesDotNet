using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace OutagesDotNet
{
    /// <summary>
    /// The jcpl provider.
    /// </summary>
    public class JcplProvider
    {
        /// <summary>
        /// Gets the current outage data from the electrical supplier.
        /// </summary>
        /// <returns> An await-able <see cref="Task"/> containing the outage results. </returns>
        public async Task<IEnumerable<Outage>> GetCurrentData()
        {
            const string Url = "http://outages.firstenergycorp.com/data/interval_generation_data/{0}NJ/report.js?timestamp={1}";
            var uri = new Uri(string.Format(Url, "2013_03_08_19_04_31", SecondsSinceEpoch()));
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

        private static string CurrentDate()
        {
            var date = DateTime.Now;
            var month = date.Month < 10 ? "0" + date.Month.ToString(CultureInfo.InvariantCulture) : date.Month.ToString(CultureInfo.InvariantCulture);
            var day = date.Day < 10 ? "0" + date.Day.ToString(CultureInfo.InvariantCulture) : date.Day.ToString(CultureInfo.InvariantCulture);
            var hour = date.Hour < 10 ? "0" + date.Hour.ToString(CultureInfo.InvariantCulture) : date.Hour.ToString(CultureInfo.InvariantCulture);
            var minute = date.Minute < 10 ? "0" + date.Minute.ToString(CultureInfo.InvariantCulture) : date.Minute.ToString(CultureInfo.InvariantCulture);
            var second = date.Second < 10 ? "0" + date.Second.ToString(CultureInfo.InvariantCulture) : date.Second.ToString(CultureInfo.InvariantCulture);
            var str = string.Format("{0}_{1}_{2}_{3}_{4}_{5}", date.Year, month, day, hour, minute, second);
            return str;
        }

        private static string SecondsSinceEpoch()
        {
            var diff = (DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
            return diff.ToString(CultureInfo.InvariantCulture);
        }
    }
}

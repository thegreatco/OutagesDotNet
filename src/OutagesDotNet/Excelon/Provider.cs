using System.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ServiceStack.Text;

namespace OutagesDotNet.Excelon
{
    /// <summary>
    /// The jcpl provider.
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Gets the current outage data from the electrical supplier.
        /// </summary>
        /// <param name="sa"> The service area. </param>
        /// <returns> An await-able <see cref="Task"/> containing the outage results.   </returns>
        public async Task<IEnumerable<ProviderStatus>> GetCurrentData(ServiceArea sa)
        {
            var regex = new Regex(@"\d{4}_\d{2}_\d{2}_\d{2}_\d{2}_\d{2}\w{2,4}(?=<)");
            var xmlUri = new Uri(string.Format("http://outages.firstenergycorp.com/data/alerts/metadata{0}.xml", sa));
            var client = new HttpClient();
            var xmlString = await client.GetStringAsync(xmlUri);
            var lastUpdate = regex.Match(xmlString).Value;
            const string Url = "http://outages.firstenergycorp.com/data/interval_generation_data/{0}/report.js?timestamp={1}";
            var uri = new Uri(string.Format(Url, lastUpdate, SecondsSinceEpoch()));
            var json = await Get<DTO.Outage>(uri);
            return ParseResults(json);
        }

        private static IEnumerable<ProviderStatus> ParseResults(DTO.Outage results)
        {
            var states = results.FileData.CurrCustsAff;

            return from state in states.Areas
                   from county in state.Areas
                   from city in county.Areas
                   select new ProviderStatus
                   {
                       State = state.AreaName,
                       City = city.AreaName,
                       County = county.AreaName,
                       Served = city.TotalCusts,
                       Out = city.CustsOut,
                       Provider = "Excelon"
                   };
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

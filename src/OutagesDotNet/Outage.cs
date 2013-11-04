using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutagesDotNet
{
    /// <summary>
    /// The outages.
    /// </summary>
    public class ProviderStatus
    {
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the county.
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// Gets or sets the served.
        /// </summary>
        public int? Served { get; set; }

        /// <summary>
        /// Gets or sets the out.
        /// </summary>
        public int? Out { get; set; }

        /// <summary>
        /// Gets or sets the provider.
        /// </summary>
        public string Provider { get; set; }

        /// <summary>
        /// The to string.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}, {3} of {4}", Provider, City ?? County, State, Out, Served);
        }

        /// <summary>
        /// Write the object to comma separated values.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string ToCsv()
        {
            return string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", Provider, City, County, State, Out, Served, Served != null && Out != null ? Math.Round(((double)Out / (double)Served) * 100, 2).ToString(CultureInfo.InvariantCulture) : string.Empty);
        }

        /// <summary>
        /// The comma separated value headers.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string[] CsvHeaders()
        {
            return new[] { DateTime.Now.ToShortDateString(), string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}", "Provider", "City", "County", "State", "Out", "Served", "Percentage Out") };
        }
    }
}

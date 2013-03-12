using System;
using System.Linq;

using OutagesDotNet;

using NUnit.Framework;

namespace JCPL.Tests
{
    /// <summary>
    /// The tests for the JSPL Outage Provider.
    /// </summary>
    [TestFixture]
    public class JcplTests
    {
        /// <summary>
        /// Outputs just the currently listed outages.
        /// </summary>
        [Test]
        public async void OutagesOnly()
        {
            // http://outages.firstenergycorp.com/data/interval_generation_data/2013_03_12_12_32_23NJ/report.js?timestamp=1363106470621
            var stuff = new JcplProvider();
            var outages = await stuff.GetCurrentData();
            foreach (var outage in outages.Where(x => x.Out > 0))
            {
                Console.WriteLine("{0}: {1}", outage.City, outage.Out);
            }
        }

        /// <summary>
        /// Outputs all the data returned by the provider.
        /// </summary>
        [Test]
        public async void AllData()
        {
            // http://outages.firstenergycorp.com/data/interval_generation_data/2013_03_12_12_32_23NJ/report.js?timestamp=1363106470621
            var stuff = new JcplProvider();
            var outages = await stuff.GetCurrentData();
            foreach (var outage in outages)
            {
                if (outage.Served > 0)
                {
                    double percentOut = ((double) outage.Out / (double) outage.Served) * 100;
                    Console.WriteLine("{4}{0}: {1} of {2} ({3}%)", outage.City, outage.Out, outage.Served, percentOut, percentOut > 0d ? "----> " : string.Empty);
                }
                else Console.WriteLine("{0}: {1} of {2}", outage.City, outage.Out, outage.Served);
            }
        }
    }
}

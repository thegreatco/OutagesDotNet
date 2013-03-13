using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

using NUnit.Framework;

using OutagesDotNet.ComEd;

namespace OutagesDotNet.Tests
{
    /// <summary>
    /// The tests for the ComEd Outage Provider.
    /// </summary>
    [TestFixture]
    public class ComEdTests
    {
        /// <summary>
        /// Outputs just the currently listed outages.
        /// </summary>
        [Test]
        public async void OutagesOnly()
        {
            var stuff = new Provider();
            var outages = await stuff.GetCurrentData(ServiceArea.IL);
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
            var stuff = new Provider();
            var outages = await stuff.GetCurrentData(ServiceArea.IL);
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

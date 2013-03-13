using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using OutagesDotNet.Peco;

namespace OutagesDotNet.Tests
{
    /// <summary>
    /// The peco tests.
    /// </summary>
    [TestFixture]
    class PecoTests
    {
        /// <summary>
        /// Outputs just the currently listed outages.
        /// </summary>
        [Test]
        public async void OutagesOnlyBucks()
        {
            var stuff = new Provider();
            var outages = await stuff.GetCurrentData(ServiceArea.Bucks);
            foreach (var outage in outages.Where(x => x.Out > 0))
            {
                Console.WriteLine("{0}: {1}", outage.City, outage.Out);
            }
        }

        /// <summary>
        /// Outputs just the currently listed outages.
        /// </summary>
        [Test]
        public async void OutagesAll()
        {
            var stuff = new Provider();
            foreach (ServiceArea state in Enum.GetValues(typeof(ServiceArea)))
            {
                var outages = await stuff.GetCurrentData(state);
                foreach (var outage in outages.Where(x => x.Out > 0))
                {
                    Console.WriteLine("{0}: {1}", outage.City, outage.Out);
                }
            }
        }

        /// <summary>
        /// Outputs all the data returned by the provider.
        /// </summary>
        [Test]
        public async void AllDataBucks()
        {
            var stuff = new Provider();
            var outages = await stuff.GetCurrentData(ServiceArea.Bucks);
            foreach (var outage in outages)
            {
                if (outage.Served > 0)
                {
                    double percentOut = ((double)outage.Out / (double)outage.Served) * 100;
                    Console.WriteLine("{4}{0}: {1} of {2} ({3}%)", outage.City, outage.Out, outage.Served, percentOut, percentOut > 0d ? "----> " : string.Empty);
                }
                else Console.WriteLine("{0}: {1} of {2}", outage.City, outage.Out, outage.Served);
            }
        }

        /// <summary>
        /// Outputs all the data returned by the provider.
        /// </summary>
        [Test]
        public async void AllDataAll()
        {
            var stuff = new Provider();
            foreach (ServiceArea state in Enum.GetValues(typeof(ServiceArea)))
            {
                Console.WriteLine("Getting Information for {0}", state);
                var outages = await stuff.GetCurrentData(state);
                foreach (var outage in outages)
                {
                    if (outage.Served > 0)
                    {
                        double percentOut = ((double)outage.Out / (double)outage.Served) * 100;
                        Console.WriteLine("{4}{0}: {1} of {2} ({3}%)", outage.City, outage.Out, outage.Served, percentOut, percentOut > 0d ? "----> " : string.Empty);
                    }
                    else Console.WriteLine("{0}: {1} of {2}", outage.City, outage.Out, outage.Served);
                }
                Console.WriteLine(string.Empty);
                Console.WriteLine(string.Empty);
            }
        }
    }
}

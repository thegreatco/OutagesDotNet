using System;
using System.Linq;

using NUnit.Framework;

using OutagesDotNet.Excelon;

namespace OutagesDotNet.Tests
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
        public async void OutagesOnlyNj()
        {
            var stuff = new Provider();
            var outages = await stuff.GetCurrentData(ServiceArea.NJ);
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
        public async void AllDataNj()
        {
            var stuff = new Provider();
            var outages = await stuff.GetCurrentData(ServiceArea.NJ);
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

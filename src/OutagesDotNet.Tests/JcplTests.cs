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
                Console.WriteLine("{0}, {1}: {2}", outage.City, outage.State, outage.Out);
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
                    Console.WriteLine("{0}, {1}: {2}", outage.City, outage.State, outage.Out);
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
                    if (outage.Out != null && outage.Served != null)
                    {
                        var percentOut = Math.Round(((double)outage.Out / (double)outage.Served) * 100, 2);
                        Console.WriteLine("{5}{0}, {1}: {2} of {3} ({4}%)", outage.City, outage.State, outage.Out, outage.Served, percentOut, percentOut > 0d ? "----> " : string.Empty);
                    }
                }
                else Console.WriteLine("{0}, {1}: {2} of {3}", outage.City, outage.State, outage.Out, outage.Served);
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
                        if (outage.Out != null && outage.Served != null)
                        {
                            var percentOut = Math.Round(((double)outage.Out / (double)outage.Served) * 100, 2);
                            Console.WriteLine("{5}{0}, {1}: {2} of {3} ({4}%)", outage.City, outage.State, outage.Out, outage.Served, percentOut, percentOut > 0d ? "----> " : string.Empty);
                        }
                    }
                    else Console.WriteLine("{0}, {1}: {2} of {3}", outage.City, outage.State, outage.Out, outage.Served);
                }

                Console.WriteLine(string.Empty);
                Console.WriteLine(string.Empty);
            }
        }
    }
}

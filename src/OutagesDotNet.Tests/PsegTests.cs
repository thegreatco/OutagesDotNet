using System;
using System.Linq;

using NUnit.Framework;

using OutagesDotNet.Pseg;

namespace OutagesDotNet.Tests
{
    /// <summary>
    /// The tests for the PSE G Outage Provider.
    /// </summary>
    [TestFixture]
    public class PsegTests
    {
        /// <summary>
        /// The test method 1.
        /// </summary>
        [Test]
        public async void TestMethod1()
        {
            var stuff = new Provider();
            var outages = await stuff.GetCurrentData(ServiceArea.NJ);
            foreach (var outage in outages.Where(x => x.Out > 0))
            {
                Console.WriteLine("{0}: {1}", outage.City, outage.Out);
            }
        }
    }
}

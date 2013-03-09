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
        /// The test method 1.
        /// </summary>
        [Test]
        public async void TestMethod1()
        {
            var stuff = new JcplProvider();
            var outages = await stuff.GetCurrentData();
            foreach (var outage in outages.Where(x => x.Out > 0))
            {
                Console.WriteLine("{0}: {1}", outage.City, outage.Out);
            }
        }
    }
}

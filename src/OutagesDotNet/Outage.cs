using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutagesDotNet
{
    /// <summary>
    /// The outages.
    /// </summary>
    public class Outage
    {
        public string State { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public int? Served { get; set; }
        public int? Out { get; set; }
    }
}

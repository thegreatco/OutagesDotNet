using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OutagesDotNet.Peco
{
    /// <summary>
    /// The PECO Energy provider.
    /// </summary>
    public class Provider
    {
        /// <summary>
        /// Gets the current outage data from the electrical supplier.
        /// </summary>
        /// <param name="sa"> The service area. </param>
        /// <returns> An await-able <see cref="Task"/> containing the outage results. </returns>
        public Task<IEnumerable<Outage>> GetCurrentData(ServiceArea sa)
        {
            // https://www.peco.com/CustomerService/OutageCenter/OutageMap/Pages/CountyDetails.aspx?County=Bucks
            return null;
        }
    }
}

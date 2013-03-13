using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutagesDotNet
{
    /// <summary>
    /// The EnergyProvider interface.
    /// </summary>
    public interface IEnergyProvider
    {
        /// <summary>
        /// Gets the current outage data from the electrical supplier.
        /// </summary>
        /// <param name="sa"> The service area. </param>
        /// <returns> An await-able <see cref="Task"/> containing the outage results.   </returns>
        Task<IEnumerable<Outage>> GetCurrentData(ServiceArea sa);
    }
}

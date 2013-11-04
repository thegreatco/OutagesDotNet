using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OutagesDotNet.Excelon.DTO
{
    /// <summary>
    /// The outage.
    /// </summary>
    [DataContract]
    public class Outage
    {
        /// <summary>
        /// Gets or sets the file title.
        /// </summary>
        [DataMember(Name = "file_title")]
        public string FileTitle { get; set; }

        /// <summary>
        /// Gets or sets the file data.
        /// </summary>
        [DataMember(Name = "file_data")]
        public FileDataEntry FileData { get; set; }

        /// <summary>
        /// The area entry.
        /// </summary>
        [DataContract]
        public class AreaEntry
        {
            /// <summary>
            /// Gets or sets the area name.
            /// </summary>
            [DataMember(Name = "area_name")]
            public string AreaName { get; set; }

            /// <summary>
            /// Gets or sets the areas.
            /// </summary>
            [DataMember(Name = "areas")]
            public IEnumerable<AreaEntry> Areas { get; set; }

            /// <summary>
            /// Gets or sets the customers without power.
            /// </summary>
            [DataMember(Name = "custs_out")]
            public int CustsOut { get; set; }

            /// <summary>
            /// Gets or sets the total customers served in the area.
            /// </summary>
            [DataMember(Name = "total_custs")]
            public int TotalCusts { get; set; }
        }

        /// <summary>
        /// The current customers affected entry.
        /// </summary>
        [DataContract]
        public class CurrCustsAffEntry
        {
            /// <summary>
            /// Gets or sets the areas.
            /// </summary>
            [DataMember(Name = "areas")]
            public IEnumerable<AreaEntry> Areas { get; set; }
        }

        /// <summary>
        /// The file data entry.
        /// </summary>
        [DataContract]
        public class FileDataEntry
        {
            /// <summary>
            /// Gets or sets the current customers affected entry.
            /// </summary>
            [DataMember(Name = "curr_custs_aff")]
            public CurrCustsAffEntry CurrCustsAff { get; set; }
        }
    }
}

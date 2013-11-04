using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutagesDotNet.Console
{
    /// <summary>
    /// The outage collector.
    /// </summary>
    public class OutageCollector
    {
        private static string _outputFile;
        
        /// <summary>
        /// The run.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        public static void Run(string[] args)
        {
            if (args.Length != 0)
                _outputFile = args.First();

            var outages = new List<ProviderStatus>();

            try
            {
                var excelonProvider = new Excelon.Provider();
                foreach (Excelon.ServiceArea serviceArea in Enum.GetValues(typeof(Excelon.ServiceArea)))
                {
                    outages.AddRange(excelonProvider.GetCurrentData(serviceArea).Result);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var psegProvider = new Pseg.Provider();
                foreach (Pseg.ServiceArea serviceArea in Enum.GetValues(typeof(Pseg.ServiceArea)))
                {
                    outages.AddRange(psegProvider.GetCurrentData(serviceArea).Result);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var pecoProvider = new Peco.Provider();
                foreach (Peco.ServiceArea serviceArea in Enum.GetValues(typeof(Peco.ServiceArea)))
                {
                    outages.AddRange(pecoProvider.GetCurrentData(serviceArea).Result);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                var comEdProvider = new ComEd.Provider();
                foreach (ComEd.ServiceArea serviceArea in Enum.GetValues(typeof(ComEd.ServiceArea)))
                {
                    outages.AddRange(comEdProvider.GetCurrentData(serviceArea).Result);
                }
            }
            catch (Exception)
            {
            }

            if (string.IsNullOrWhiteSpace(_outputFile)) foreach (var outage in outages) System.Console.WriteLine(outage.ToString());
            else
            {
                var outStr = new List<string>();
                outStr.AddRange(ProviderStatus.CsvHeaders());
                outStr.AddRange(outages.Select(outage => outage.ToCsv()));

                File.WriteAllLines(_outputFile, outStr);
            }
        }
    }
}

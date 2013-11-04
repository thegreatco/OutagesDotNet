using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OutagesDotNet.Console
{
    /// <summary>
    /// The program.
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            OutageCollector.Run(args);
        }
    }
}

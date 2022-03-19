using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umi.BetterOccupancyDemo
{
    internal class BetterOccupancyProjectSettings
    {
        public Dictionary<string, double> BetterOccupantDensitiesByTemplate { get; set; } = new Dictionary<string, double>();
    }
}

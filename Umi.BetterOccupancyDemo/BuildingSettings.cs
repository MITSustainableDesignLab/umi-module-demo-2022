using Umi.Core;

namespace Umi.BetterOccupancyDemo
{
    public class BuildingSettings : SettingsObject
    {
        public override string? Id { get; set; }

        public double? OccupantDensityOverride { get; set; }
    }
}

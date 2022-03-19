using Rhino;
using Rhino.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Umi.Core;
using Umi.RhinoServices;
using Umi.RhinoServices.Context;

namespace Umi.BetterOccupancyDemo.RhinoCommands
{
    [Guid("432ae1df-d477-40ce-bb9d-f7b29b676b79")]
    public class UmiCalculateBetterOccupancy : UmiCommand
    {
        public override string EnglishName => nameof(UmiCalculateBetterOccupancy);

        public override Result Run(RhinoDoc doc, UmiContext context, RunMode mode)
        {
            var databaseObjects = new List<IUmiObject>();

            var projectSettings = context.ModuleProjectSettings.Get<BetterOccupancyProjectSettings>();

            var buildingSettings = context.GetSettings<BuildingSettings>();

            foreach (var building in context.Buildings.All)
            {
                if (building.TemplateName == null)
                {
                    continue;
                }

                var grossFloorArea = building.GrossFloorArea;

                projectSettings.BetterOccupantDensitiesByTemplate.TryGetValue(building.TemplateName, out var occupantDensity);

                var settingsForBuilding = buildingSettings.TryGet(building.Id);

                if (settingsForBuilding != null && settingsForBuilding.OccupantDensityOverride.HasValue)
                {
                    occupantDensity = settingsForBuilding.OccupantDensityOverride.Value;
                }

                var occupancy = grossFloorArea * occupantDensity;

                RhinoApp.WriteLine($"{building.Name}: {occupancy}");

                var databaseSeries = new UmiDataSeries();
                databaseSeries.Name = "better occupancy";
                databaseSeries.Units = "persons";
                databaseSeries.Data = new List<double>() { occupancy ?? 0 };

                var databaseObject = UmiObject.Create(building.Name, building.Id.ToString(), databaseSeries);

                databaseObjects.Add(databaseObject);

                Module.Instance.SetBetterOccupancy(building.Id, occupancy ?? 0);
            }

            context.StoreObjects(databaseObjects);

            return Result.Success;
        }
    }
}

using Rhino;
using Rhino.Commands;
using Rhino.Input;
using System.Runtime.InteropServices;
using Umi.RhinoServices;
using Umi.RhinoServices.Context;

namespace Umi.BetterOccupancyDemo.RhinoCommands
{
    [Guid("81b3df3f-a704-46c9-8b6a-b076a1a73554")]
    public class UmiSetOccupantDensityOverride : UmiCommand
    {
        public override string EnglishName => nameof(UmiSetOccupantDensityOverride);

        public override Result Run(RhinoDoc doc, UmiContext context, RunMode mode)
        {
            var occupantDensityOverride = 0.0;

            var getResult = RhinoGet.GetNumber("Enter building occupant density override", false, ref occupantDensityOverride);

            if (getResult != Result.Success)
            {
                return getResult;
            }

            var selectedRhinoObjects = doc.Objects.GetSelectedObjects(false, false);

            var selectedUmiBuildings = context.Buildings.ForObjects(selectedRhinoObjects);

            var settingsForAllBuildings = context.GetSettings<BuildingSettings>();

            foreach (var selectedUmiBuilding in selectedUmiBuildings)
            {
                var thisBuildingSettings = settingsForAllBuildings.GetOrCreate(selectedUmiBuilding.Id);

                thisBuildingSettings.OccupantDensityOverride = occupantDensityOverride;
            }

            return Result.Success;
        }
    }
}

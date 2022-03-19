using Rhino;
using Rhino.Commands;
using Rhino.Input;
using Rhino.Input.Custom;
using System.Linq;
using System.Runtime.InteropServices;
using Umi.RhinoServices;
using Umi.RhinoServices.Context;

namespace Umi.BetterOccupancyDemo.RhinoCommands
{
    [Guid("74bd09e1-336d-4622-8738-3a52037567c8")]
    public class UmiSetBetterOccupantDensityForTemplate : UmiCommand
    {
        public override string EnglishName => nameof(UmiSetBetterOccupantDensityForTemplate);

        public override Result Run(RhinoDoc doc, UmiContext context, RunMode mode)
        {
            if (context.TemplateLibrary == null)
            {
                return Result.Nothing;
            }

            var templateNames = context.TemplateLibrary.AvailableTemplateNames().ToList();

            var templateNameGetter = new GetOption();
            templateNameGetter.SetCommandPrompt("Pick a template");
            templateNameGetter.AddOptionList("Template", templateNames, 0);
            var getTemplateResult = templateNameGetter.Get();

            if (getTemplateResult == GetResult.Cancel)
            {
                return Result.Cancel;
            }

            var selectedIndex = templateNameGetter.Option().CurrentListOptionIndex;
            var selectedTemplateName = templateNames[selectedIndex];

            RhinoApp.WriteLine($"User picked template: {selectedTemplateName}");

            var projectSettings = context.ModuleProjectSettings.Get<BetterOccupancyProjectSettings>();

            projectSettings.BetterOccupantDensitiesByTemplate.TryGetValue(selectedTemplateName, out var occupantDensity);

            var getOccupantDensityResult = RhinoGet.GetNumber("Specify occupant density", false, ref occupantDensity);

            if (getOccupantDensityResult != Result.Success)
            {
                return getOccupantDensityResult;
            }

            projectSettings.BetterOccupantDensitiesByTemplate[selectedTemplateName] = occupantDensity;

            context.ModuleProjectSettings.Save<BetterOccupancyProjectSettings>();

            return Result.Success;
        }
    }
}

using Rhino;
using System.ComponentModel;
using System.Windows.Input;

namespace Umi.BetterOccupancyDemo.Panel
{
    public class PanelViewModel : INotifyPropertyChanged
    {
        private int totalSelectedBuildingOccupants;

        public PanelViewModel()
        {
            RunExampleCommand = new RelayCommand(RunExampleRhinoCommand);

            PropertyChanged += (s, e) => { };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand RunExampleCommand { get; }

        public int TotalSelectedBuildingOccupants
        {
            get => totalSelectedBuildingOccupants;
            set
            {
                totalSelectedBuildingOccupants = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TotalSelectedBuildingOccupants)));
            }
        }

        private void RunExampleRhinoCommand()
        {
            RhinoApp.RunScript("UmiCalculateBetterOccupancy", echo: true);
        }
    }
}

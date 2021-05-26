using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using StepProgressBench.Annotations;
using StepProgressBench.Controls;
using Color = Windows.UI.Color;
using Rectangle = Windows.UI.Xaml.Shapes.Rectangle;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace StepProgressBench
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private List<StepProgress> _steps;

        public List<StepProgress> Steps
        {
            get => _steps;
            set => Set(ref _steps, value);
        }

        public MainPage()
        {
            this.InitializeComponent();
            Steps = new List<StepProgress>()
            {
                new StepProgress()
                {
                    StepState = StepState.Done,
                    Text = "ToolBox Talk Form"
                },
                new StepProgress()
                {
                    StepState = StepState.Done,
                    Text = "Job Hazard Analysis"
                },
                new StepProgress()
                {
                    StepState = StepState.Active,
                    Text = "Project Safety"
                },
                new StepProgress()
                {
                    StepState = StepState.New,
                    Text = "Photos"
                },
                new StepProgress()
                {
                    StepState = StepState.New,
                    Text = "Complete"
                }
            };
        }

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return;

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
    }
}

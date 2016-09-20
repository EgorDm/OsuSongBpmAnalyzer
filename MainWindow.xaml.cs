using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using ManagedBass;
using ManagedBass.Fx;
using Microsoft.Win32;
using OsuBeatToolbox.Annotations;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace OsuBeatToolbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {

        private readonly OpenFileDialog _ofd;

        ~MainWindow()
        {
            Bass.Free();
        }


        public MainWindow()
        {
            _ofd = new OpenFileDialog
            {
                Filter =
                    "Playable files|*.mo3; *.xm; *.mod; *.s3m; *.it; *.mtm; *.mp3; *.mp2; *.mp1; *.ogg; *.wav; *.aif|All files|*.*"
            };

            CheckFrequency = 6.0;

            BpmPlot = new PlotModel {Title = "BPM Graph"};
            var serie = new LineSeries {TrackerFormatString = "seconds={2:0.0} BPM={4:0.0}" };
            BpmPlot.Series.Add(serie);
            BpmPlot.Axes.Add(new LinearAxis(){ Position = AxisPosition.Left, Minimum = 0, Maximum = 300 });
            OpenCommand = new DelegateCommand(OpenSong);
        }

        private void OpenSong()
        {
            var showDialog = _ofd.ShowDialog();
            if (showDialog != null && !showDialog.Value)
                return;

            var analyze = new TimingAnalyzer();
            var data = analyze.AnalyzeSong(_ofd.FileName, CheckFrequency);
            (BpmPlot.Series[0] as LineSeries).Points.Clear();
            foreach (var dataPoint in data.GetBpmData())
            {
                (BpmPlot.Series[0] as LineSeries).Points.Add(dataPoint);
            }
            BpmPlot.InvalidatePlot(true);
            OsuText = data.OsuOut;
        }

        #region UIBindings

        public PlotModel BpmPlot { get; set; }

        public PlotModel BeatOffsetPlotModel { get; set; }

        public ICommand OpenCommand { get; }

        public double CheckFrequency  { get; set; }

        public string _osuText = "";
        public string OsuText {
            get { return _osuText; }
            set
            {
                _osuText = value;

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
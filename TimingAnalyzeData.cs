using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;

namespace OsuBeatToolbox
{
    public class TimingPoint
    {
        public double Offset { get; set; }
        public double BPM { get; set; }

        public TimingPoint(double offset, double bpm)
        {
            Offset = offset;
            BPM = bpm;
        }

        public override string ToString()
        {
            return  $"{Offset * 1000},{((60.0*1000.0)/BPM):N2},4,1,0,10,1,0";
        }
    }

    public class TimingAnalyzeData
    {
        private List<TimingPoint> _timings;
        public string OsuOut = "";

        public TimingAnalyzeData()
        {
            _timings = new List<TimingPoint>();
        }

        public void AddBpm(TimingPoint timing)
        {
            _timings.Add(timing);
        }

        public List<DataPoint> GetBpmData()
        {
            var ret = new List<DataPoint>();
            foreach (var d in _timings)
            {
                OsuOut += d.ToString() + "\n";
                ret.Add(new DataPoint(d.Offset, d.BPM));
            }
            return ret;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedBass;
using ManagedBass.Fx;

namespace OsuBeatToolbox
{
    public class TimingAnalyzer
    {
        #region Constants
        
        #endregion

        #region Variables

        private int _channel;

        #endregion

        public TimingAnalyzer()
        {
            Bass.Init();
        }

        public TimingAnalyzeData AnalyzeSong(string songPath, double blockLength)
        {
            _channel = Bass.CreateStream(songPath, 0, 0, BassFlags.Decode);
            if (_channel == 0)
                _channel = Bass.MusicLoad(songPath, 0, 0, BassFlags.Decode | BassFlags.Prescan, 0);

            var songLength = Bass.ChannelBytes2Seconds(_channel, Bass.ChannelGetLength(_channel));
            var ret = new TimingAnalyzeData();

            for (var p = 0.0; (p + blockLength) < songLength; p += blockLength)
            {
                var bpm = AnalyzeBpm(p, p + blockLength);
                while (bpm > 360 | bpm < 60)
                {
                    if (bpm > 360)
                    {
                        bpm /= 2;
                    } else if (bpm < 60)
                    {
                        bpm *= 2;
                    }
                }
                ret.AddBpm(new TimingPoint(p, bpm));
            }
            BassFx.BPMFree(_channel);
            Bass.MusicFree(_channel);
            Bass.StreamFree(_channel);

            return ret;
        }

        private double AnalyzeBpm(double from, double to)
        {
            if (_channel == 0) return -1.0;
            return BassFx.BPMDecodeGet(_channel, from, to, 0,
                    BassFlags.FxBpmBackground | BassFlags.FXBpmMult2 | BassFlags.FxFreeSource, GetBPM_ProgressCallback);
        }

        private void GetBPM_ProgressCallback(int Channel, float Percent, IntPtr User)
        {
        }
    }
}
// Developed by Pigeon Studios
// Released with the EasyState-2020
using System.Diagnostics;
using System.Timers;

namespace EasyState.Core
{
    public class PausableTimer : Timer
    {
        public double RemainingAfterPause { get; private set; }

        private readonly double _initialInterval;

        private readonly Stopwatch _stopwatch;

        private bool _resumed;

        public PausableTimer(double interval) : base(interval)
        {
            _initialInterval = interval;
            Elapsed += OnElapsed;
            _stopwatch = new Stopwatch();
        }

        public void Pause()
        {
            Stop();
            _stopwatch.Stop();
            RemainingAfterPause = Interval - _stopwatch.Elapsed.TotalMilliseconds;
        }

        public void Resume()
        {
            _resumed = true;
            Interval = RemainingAfterPause <= 0? .001f: RemainingAfterPause;
            RemainingAfterPause = 0;
            Start();
        }

        public new void Start()
        {
            ResetStopwatch();
            base.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_resumed)
            {
                _resumed = false;
                Stop();
                Interval = _initialInterval;
                Start();
            }

            ResetStopwatch();
        }

        private void ResetStopwatch()
        {
            _stopwatch.Reset();
            _stopwatch.Start();
        }
    }
}
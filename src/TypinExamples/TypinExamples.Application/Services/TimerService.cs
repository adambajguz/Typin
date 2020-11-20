namespace TypinExamples.Application.Services
{
    using System;
    using System.Timers;

    public class TimerService : IDisposable
    {
        private Timer? _timer = new Timer();

        public bool IsRunning => _timer?.Enabled ?? false;

        public event Action? Elapsed;

        public TimerService()
        {

        }

        public void Set(int interval, bool autoReset = false)
        {
            Stop();

            _timer ??= new Timer(interval);
            _timer.AutoReset = autoReset;
            _timer.Elapsed += NotifyTimerElapsed;
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }

        private void NotifyTimerElapsed(Object source, ElapsedEventArgs e)
        {
            Elapsed?.Invoke();
            if (_timer?.AutoReset == false)
            {
                _timer?.Stop();
                _timer?.Dispose();
                _timer = null;
            }
        }

        public void Dispose()
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
        }
    }
}

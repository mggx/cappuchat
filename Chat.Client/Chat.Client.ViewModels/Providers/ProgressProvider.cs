using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chat.Client.ViewModels.Providers
{
    public class ProgressProvider : INotifyPropertyChanged
    {
        private IProgressScope _progressScope;
        public IProgressScope ProgressScope
        {
            get { return _progressScope; }
            set { _progressScope = value; OnPropertyChanged(); }
        }

        public IProgressScope StartProgress()
        {
            return ProgressScope = new ProgressScope();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ProgressScope : IProgressScope, INotifyPropertyChanged
    {
        private bool _inProgress;
        public bool InProgress
        {
            get { return _inProgress; }
            set { _inProgress = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ProgressScope()
        {
            StartProgress();
        }

        private void StartProgress()
        {
            InProgress = true;
        }

        public void Dispose()
        {
            InProgress = false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IProgressScope : IDisposable
    {
        bool InProgress { get; set; }
    }
}

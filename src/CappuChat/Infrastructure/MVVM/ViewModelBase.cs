using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chat.Client.Framework
{
    public class ViewModelBase : Disposable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

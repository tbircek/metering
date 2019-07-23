using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace metering.viewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged //, IDisposable
    {        
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(T field, T newValue, [CallerMemberName]string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                Debug.WriteLine($"SetProperty (propertyName: {propertyName}) processed successfull.");
                //Log += $"SetProperty (propertyName: {propertyName}) processed successfull.";
                return true;
            }
            Debug.WriteLine($"SetProperty (propertyName: {propertyName}) processed are same values.");
            return false;
        }
    }
}

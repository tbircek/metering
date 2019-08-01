using System.ComponentModel;
using System.Diagnostics;
using PropertyChanged;

namespace metering
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// the event when any property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// call this to fire <see cref="PropertyChanged"/> event
        /// </summary>
        /// <param name="name"></param>
        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
            Debug.WriteLine($"SetProperty: (name: {name}) processed successfull.");
        }
        //{
        //    Debug.WriteLine($"SetProperty: (sender: {sender.GetType().Name} (propertyName: {e.PropertyName}) processed successfull.");
        //};

        //protected bool SetProperty<T>(T field, T newValue, [CallerMemberName]string propertyName = null)
        //{
        //    if (!EqualityComparer<T>.Default.Equals(field, newValue))
        //    {
        //        field = newValue;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //        Debug.WriteLine($"SetProperty (propertyName: {propertyName}) processed successfull.");
        //        //Log += $"SetProperty (propertyName: {propertyName}) processed successfull.";
        //        return true;
        //    }
        //    Debug.WriteLine($"SetProperty (propertyName: {propertyName}) processed are same values.");
        //    return false;
        //}
    }
}

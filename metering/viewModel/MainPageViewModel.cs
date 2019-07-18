using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using metering.model;

namespace metering.ViewModel
{
    public class MainPageViewModel: INotifyPropertyChanged
    {
       
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {

            Debug.WriteLine(string.Format("OnPropertyChanged ( propertyName: {0} ) just processed.", propertyName));

            this.VerifyPropertyName(propertyName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Throws a 'InvalidProperty' exception.
        /// It will not exists in 'Release' version.
        /// Allows me see if a property named incorrectly.
        /// </summary>
        /// <param name="propertyName">Property name to verify.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                Debug.WriteLine($"Invalid property: {propertyName}");
            }
        }
    }
}

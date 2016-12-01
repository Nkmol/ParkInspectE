using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PersoneelViewModel : ViewModelBase
    {

        public ObservableCollection<Object> PersoneelCollection { get; set; }

        /// <summary>
        /// Initializes a new instance of the PersoneelViewModel class.
        /// </summary>
        public PersoneelViewModel()
        {

        }
    }
}
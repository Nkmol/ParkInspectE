using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class CRUDViewModel<T> : ViewModelBase
    {
        public T SelectedItem { get; set; }

        public RelayCommand SaveCommand { get; set; }

        /// <summary>
        /// Initializes a new instance of the CRUDViewModel class.
        /// </summary>
        public CRUDViewModel()
        {
        }
    }
}
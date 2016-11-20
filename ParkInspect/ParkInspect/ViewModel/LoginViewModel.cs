using GalaSoft.MvvmLight;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Linq;

namespace ParkInspect.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoginViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the LoginViewModel class.
        /// </summary>
        public LoginViewModel()
        {

        }

        public int login(string username, string password)
        {
            using (var context = new ParkInspectEntities())
            {
                List<Employee> list = context.Employee.ToList();
                foreach (Employee u in list)
                {
                    if (u.email.Equals(username) && u.password.Equals(password))
                    {
                        return 1;
                    }
                }
            }
            return 0;
        }
    }
}
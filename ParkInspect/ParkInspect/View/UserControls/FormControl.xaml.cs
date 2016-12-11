using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParkInspect.ViewModel;
using ParkInspect;
using System.Diagnostics;

namespace ParkInspect.View.UserControls
{
    public partial class FormControl : UserControl
    {
        List<CachedFormField> fields;
        public FormControl()
        {
            InitializeComponent();
            FormViewModel viewmodel = (FormViewModel) DataContext;
            fields = new List<CachedFormField>();
            viewmodel.View = this;
        }

        public void clear()
        {
            foreach (Control element in FormGrid.Children) {
                if (element.Name.IndexOf("FormElement") > 0)
                {
                    FormGrid.Children.Remove(element);
                }
            }
        }

        public void addFormField(CachedFormField field,int count)
        {
            Control element = null;
            Debug.WriteLine(field.datatype);
            switch (field.datatype){
                case "Boolean":
                    element = new CheckBox();
                    BindingOperations.SetBinding(element, CheckBox.IsCheckedProperty, new Binding("fields[" + count + "].boolvalue"));
                    break;
                case "Date":
                    element = new DatePicker();
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("fields[" + count + "].stringvalue"));
                    break;
                case "Double":
                    element = new TextBox();
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("fields[" + count + "].doublevalue"));
                    break;
                case "Integer":
                    element = new TextBox();
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("fields[" + count + "].intvalue"));
                    break;
                case "String":
                    element = new TextBox();
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("fields[" + count + "].stringvalue"));
                    break;
                case "Time":
                    element = new TextBox();
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("fields[" + count + "].stringvalue"));
                    break;
                default:
                    element = new TextBox();
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("fields[" + count + "].stringvalue"));
                    break;
            }
            Label textLabel = new Label();
            textLabel.Content = field.field_title;

            textLabel.Margin = new Thickness(50, 50 + fields.Count * 50, 250, 850 - (50 + fields.Count * 50 + 40));
            element.Margin = new Thickness(250, 50 + fields.Count * 50, 250, 850 - (50 + fields.Count * 50 + 40));
            element.Name = "FormElement" + count;

            fields.Add(field);
            FormGrid.Children.Add(textLabel);
            FormGrid.Children.Add(element);
        }
    }
}

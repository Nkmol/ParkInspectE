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
using System.Timers;

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
            Timer timer = new Timer(1000);
            timer.Elapsed += async (sender, e) => await HandleTimer();
            timer.Start();
        }

        public Task HandleTimer()
        {
            changeColor();
            return new Task(changeColor);
        }

        public void changeColor()
        {
            Random random = new Random();
            int R = random.Next(0, 255);
            int G = random.Next(0, 255);
            int B = random.Next(0, 255);
            FormGrid.Background = new SolidColorBrush(Color.FromArgb(255, (byte)R, (byte)G, (byte)B));
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

        public void addFormField(CachedFormField field,int count, bool isReadonly)
        {
            if (isReadonly)
            {
                saveButton.Visibility = Visibility.Hidden;
            } else
            {
                saveButton.Visibility = Visibility.Visible;
            }
            Control element = null;
            switch (field.datatype){
                case "Boolean":
                    element = new CheckBox();
                    ((CheckBox)element).IsHitTestVisible = !isReadonly;
                    BindingOperations.SetBinding(element, CheckBox.IsCheckedProperty, new Binding("cachedForm.fields[" + count + "].value.boolvalue"));
                    break;
                case "Date":
                    element = new DatePicker();
                    ((DatePicker)element).Focusable = !isReadonly;
                    ((DatePicker)element).IsHitTestVisible = !isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("cachedForm.fields[" + count + "].value.stringvalue"));
                    break;
                case "Double":
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("cachedForm.fields[" + count + "].value.doublevalue"));
                    break;
                case "Integer":
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("cachedForm.fields[" + count + "].value.intvalue"));
                    break;
                case "String":
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("cachedForm.fields[" + count + "].value.stringvalue"));
                    break;
                case "Time":
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("cachedForm.fields[" + count + "].value.stringvalue"));
                    break;
                default:
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("cachedForm.fields[" + count + "].value.stringvalue"));
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

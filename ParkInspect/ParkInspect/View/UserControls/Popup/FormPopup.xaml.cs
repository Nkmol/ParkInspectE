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
using ParkInspect.Model.ValidationRules;

namespace ParkInspect.View.UserControls.Popup
{
    /// <summary>
    /// Interaction logic for FormControl.xaml
    /// </summary>
    public partial class FormPopup : UserControl
    {
        List<CachedFormField> fields;
        public FormPopup()
        {
            InitializeComponent();
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            FormViewModel viewmodel = (FormViewModel)DataContext;
            fields = new List<CachedFormField>();
            viewmodel.View = this;
        }

        public void clear()
        {
            List<Control> toRemoveElements = new List<Control>();
            foreach (Control element in FormGrid.Children)
            {
                if (element.Name.IndexOf("ormElement") > 0)
                {
                    toRemoveElements.Add(element);
                }
            }
            foreach (Control element in toRemoveElements)
            {
                FormGrid.Children.Remove(element);
            }
        }

        public void addFormField(CachedFormField field, int count, bool isReadonly)
        {
            if (isReadonly)
            {
                saveButton.Visibility = Visibility.Hidden;
                addAttachmentButton.Visibility = Visibility.Hidden;
            }
            else
            {
                saveButton.Visibility = Visibility.Visible;
                addAttachmentButton.Visibility = Visibility.Visible;
            }
            Control element = null;
            switch (field.datatype)
            {
                case "Boolean":
                    element = new CheckBox();
                    ((CheckBox)element).IsHitTestVisible = !isReadonly;
                    Binding binding = new Binding("cachedForm.fields[" + count + "].value.boolvalue");
                    BindingOperations.SetBinding(element, CheckBox.IsCheckedProperty, binding);
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
                    Binding doubleBinding = new Binding("cachedForm.fields[" + count + "].value.doublevalue");
                    doubleBinding.ValidationRules.Add(new StringToIntValidationRule());
                    BindingOperations.SetBinding(element, TextBox.TextProperty, doubleBinding);
                    break;
                case "Integer":
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    Binding intBinding = new Binding("cachedForm.fields[" + count + "].value.intvalue");
                    intBinding.ValidationRules.Add(new StringToIntValidationRule());
                    BindingOperations.SetBinding(element, TextBox.TextProperty, intBinding);
                    break;
                case "String":
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    Binding stringbinding = new Binding("cachedForm.fields[" + count + "].value.stringvalue");
                    stringbinding.ValidationRules.Add(new IsNotEmptyValidationRule());
                    BindingOperations.SetBinding(element, TextBox.TextProperty, stringbinding);
                    break;
                case "Time":
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty, new Binding("cachedForm.fields[" + count + "].value.stringvalue"));
                    break;
                default:
                    element = new TextBox();
                    ((TextBox)element).IsReadOnly = isReadonly;
                    Binding defaultbinding = new Binding("cachedForm.fields[" + count + "].value.stringvalue");
                    defaultbinding.ValidationRules.Add(new IsNotEmptyValidationRule());
                    BindingOperations.SetBinding(element, TextBox.TextProperty, defaultbinding);
                    break;
            }
            Label textLabel = new Label();
            textLabel.Content = field.field_title;
            textLabel.Name = "formElement" + count * 2;

            textLabel.Margin = new Thickness(50, 50 + count * 50, 250, 850 - (50 + count * 50 + 40));
            element.Margin = new Thickness(250, 50 + count * 50, 250, 850 - (50 + count * 50 + 40));
            element.Name = "FormElement" + (count * 2 + 1);


            fields.Add(field);
            FormGrid.Children.Add(textLabel);
            FormGrid.Children.Add(element);
        }
    }
}

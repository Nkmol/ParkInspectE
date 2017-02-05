using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using ParkInspect.Model.ValidationRules;
using ParkInspect.ViewModel;
using Syncfusion.Linq;

namespace ParkInspect.View.UserControls.Popup
{
    /// <summary>
    ///     Interaction logic for FormControl.xaml
    /// </summary>
    public partial class FormPopup : UserControl
    {
        private readonly List<CachedFormField> fields;

        public FormPopup()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            var viewmodel = (FormViewModel) DataContext;
            fields = new List<CachedFormField>();
            viewmodel.View = this;
        }

        public void clear()
        {
            var toRemoveElements = new List<Control>();
            foreach (
                Control element in
                FormGrid.Children.ToList<UIElement>()
                    .Where(x => x is Control)
                    .Where(x => ((Control) x).Name.IndexOf("formElement") > 0))
                toRemoveElements.Add(element);

            foreach (var element in toRemoveElements)
                FormGrid.Children.Remove(element);
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
            switch (field.Datatype)
            {
                case "Boolean":
                    element = new CheckBox();
                    ((CheckBox) element).IsHitTestVisible = !isReadonly;
                    var binding = new Binding("cachedForm.fields[" + count + "].value.boolvalue");
                    BindingOperations.SetBinding(element, ToggleButton.IsCheckedProperty, binding);
                    break;
                case "Date":
                    element = new DatePicker();
                    ((DatePicker) element).Focusable = !isReadonly;
                    ((DatePicker) element).IsHitTestVisible = !isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty,
                        new Binding("cachedForm.fields[" + count + "].value.stringvalue"));
                    break;
                case "Double":
                    element = new TextBox();
                    ((TextBox) element).IsReadOnly = isReadonly;
                    var doubleBinding = new Binding("cachedForm.fields[" + count + "].value.doublevalue");
                    doubleBinding.ValidationRules.Add(new StringToIntValidationRule());
                    BindingOperations.SetBinding(element, TextBox.TextProperty, doubleBinding);
                    break;
                case "Integer":
                    element = new TextBox();
                    ((TextBox) element).IsReadOnly = isReadonly;
                    var intBinding = new Binding("cachedForm.fields[" + count + "].value.intvalue");
                    intBinding.ValidationRules.Add(new StringToIntValidationRule());
                    BindingOperations.SetBinding(element, TextBox.TextProperty, intBinding);
                    break;
                case "String":
                    element = new TextBox();
                    ((TextBox) element).IsReadOnly = isReadonly;
                    var stringbinding = new Binding("cachedForm.fields[" + count + "].value.stringvalue");
                    stringbinding.ValidationRules.Add(new IsNotEmptyValidationRule());
                    BindingOperations.SetBinding(element, TextBox.TextProperty, stringbinding);
                    break;
                case "Time":
                    element = new TextBox();
                    ((TextBox) element).IsReadOnly = isReadonly;
                    BindingOperations.SetBinding(element, TextBox.TextProperty,
                        new Binding("cachedForm.fields[" + count + "].value.stringvalue"));
                    break;
                default:
                    element = new TextBox();
                    ((TextBox) element).IsReadOnly = isReadonly;
                    var defaultbinding = new Binding("cachedForm.fields[" + count + "].value.stringvalue");
                    defaultbinding.ValidationRules.Add(new IsNotEmptyValidationRule());
                    BindingOperations.SetBinding(element, TextBox.TextProperty, defaultbinding);
                    break;
            }
            var stackPanel = new UniformGrid {Rows = 1};

            var textLabel = new Label();
            textLabel.Content = field.FieldTitle;
            textLabel.Name = "formElement" + count * 2;
            textLabel.Margin = new Thickness(0, 5, 0, 0);

            //textLabel.Margin = new Thickness(50, 50 + count * 50, 250, 850 - (50 + count * 50 + 40));
            //element.Margin = new Thickness(250, 50 + count * 50, 250, 850 - (50 + count * 50 + 40));
            element.Name = "formElement" + (count * 2 + 1);

            stackPanel.Children.Add(textLabel);
            stackPanel.Children.Add(element);

            fields.Add(field);
            FormGrid.Children.Add(stackPanel);
        }
    }
}
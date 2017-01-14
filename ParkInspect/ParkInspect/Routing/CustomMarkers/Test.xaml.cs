using System.Windows.Controls;

namespace GMAPWPF.CustomMarkers
{
   /// <summary>
   /// Interaction logic for Test.xaml
   /// </summary>
   public partial class Test : UserControl
   {
      public Test(string txt)
      {
         InitializeComponent();

         text.Text = txt;
      }
   }
}

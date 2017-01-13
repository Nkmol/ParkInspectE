using System.Windows.Controls;
using GMap.NET;
using GMAPWPF;

namespace GMAPWPF.Controls
{
   /// <summary>
   /// Interaction logic for TrolleyTooltip.xaml
   /// </summary>
   public partial class TrolleyTooltip : UserControl
   {
      public TrolleyTooltip()
      {
         InitializeComponent();
      }

      public void SetValues(string placeName)
      {
            label.Content = "Plaats: " + placeName;
      }
   }
}

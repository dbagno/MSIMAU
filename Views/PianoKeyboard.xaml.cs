using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSIMAU.Views;
public static class Constants
{
    public const int Columns = 52;
    public const double RowHeight = 0.5;
    public const int RowSpan = 2;
    public const double ColumnWidth = 1.923;
}
public partial class PianoKeyboard : ContentView
{
    public PianoKeyboard()
    {
        InitializeComponent();
   
        for (int i = 0; i < Constants.Columns; i++)
        {
            PianoGrid.ColumnDefinitions.Add(new ColumnDefinition
                { Width = new GridLength(Constants.ColumnWidth, GridUnitType.Auto) });
         
            PianoGrid.Add(view:new PianoKey(){HeightRequest = 74},i,row:1);
        }

        // Adds buttons or other dynamic logic if necessary.
    }
}
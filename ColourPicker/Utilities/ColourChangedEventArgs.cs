using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColourPicker.Utilities
{
    public class ColourChangedEventArgs
    {
        public Color OldColour { get; set; }
        public Color NewColour { get; set; }

        public ColourChangedEventArgs() { }
        public ColourChangedEventArgs(Color oldColour, Color newColour)
        {
            OldColour = oldColour;
            NewColour = newColour;
        }
    }
}

using ColourPicker.Helpers;
using ColourPicker.Utilities;
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

namespace ColourPicker.Controls
{
    /// <summary>
    /// Interaction logic for ColourSlider.xaml
    /// </summary>
    public partial class ColourSlider : UserControl
    {
        public delegate void ColourChangedArgs(object sender, ColourChangedEventArgs e);
        public event ColourChangedArgs OnColourChanged;

        public static readonly DependencyProperty SelectedColourProperty =
            DependencyProperty.Register(
                nameof(SelectedColour),
                typeof(Color),
                typeof(ColourSlider),
                new PropertyMetadata(
                    Colors.Black,
                    new PropertyChangedCallback(
                        SelectedColourChangedCallBack)));
        private static void SelectedColourChangedCallBack(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            if (property is ColourSlider colourSlider)
            {
                if (args.OldValue is Color oldVal && args.NewValue is Color newVal)
                {
                    ColourChangedEventArgs ccea =
                        new ColourChangedEventArgs(oldVal, newVal);

                    colourSlider.OnColourChanged?.Invoke(colourSlider, ccea);
                }
            }
        }

        public Color SelectedColour
        {
            get { return (Color)GetValue(SelectedColourProperty); }
            set { SetValue(SelectedColourProperty, value); }
        }

        public ColourSlider()
        {
            InitializeComponent();
        }

        private void ColourSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (BrightnessSliderGradient.GradientStops != null)
            {
                try
                {
                    Color newColour =
                        GradientPositionColours.GetRelativeColor(
                            BrightnessSliderGradient.GradientStops,
                            1 - (e.NewValue / 1000));
                    
                    SelectedColour = newColour;
                }
                catch { }
            }
        }
    }
}

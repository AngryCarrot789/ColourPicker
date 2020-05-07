using ColourPicker.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace ColourPicker.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private byte _red;
        private byte _green;
        private byte _blue;
        private byte _alpha;
        private Brush _finalColour;

        #region Active Colour Fields

        public byte Red
        {
            get => _red; 
            set => RaisePropertyChanged(ref _red, value, UpdateFullColour);
        }
        public byte Green
        {
            get => _green; 
            set => RaisePropertyChanged(ref _green, value, UpdateFullColour);
        }
        public byte Blue
        {
            get => _blue; 
            set => RaisePropertyChanged(ref _blue, value, UpdateFullColour);
        }

        public byte Alpha
        {
            get => _alpha;
            set => RaisePropertyChanged(ref _alpha, value, UpdateFullColour);
        }

        public Brush FinalColour
        {
            get => _finalColour;
            set => RaisePropertyChanged(ref _finalColour, value);
        }

        #endregion

        public ICommand UpdateColourManuallyCommand { get; private set; }

        public Action<Color> SetPickerMainColourCallback { get; set; }
        public Action<Color> SetSliderColourCallback { get; set; }

        public MainViewModel()
        {
            UpdateColourManuallyCommand = new Command(UpdateColourManually);
        }

        public void UpdateColourManually()
        {
            Color newColour = new Color();

            if (Red   <= 255) newColour.R = Red;
            if (Green <= 255) newColour.G = Green;
            if (Blue  <= 255) newColour.B = Blue;
            if (Alpha <= 255) newColour.A = Alpha;

            SetSliderColourCallback?.Invoke(newColour);
            UpdateFullColour();
        }

        public void ColourPickerValueChanged(Color newColour)
        {
            Red = newColour.R;
            Green = newColour.G;
            Blue = newColour.B;
            Alpha = newColour.A;
        }

        public void ColourSliderValueChanged(Color newColour)
        {
            SetPickerMainColourCallback?.Invoke(newColour);
        }

        public void UpdateFullColour()
        {
            FinalColour =
                new SolidColorBrush(
                    new Color()
                    {
                        R = Red,
                        G = Green,
                        B = Blue,
                        A = Alpha
                    });

        }
    }
}

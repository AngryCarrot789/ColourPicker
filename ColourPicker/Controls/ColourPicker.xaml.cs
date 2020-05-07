using ColourPicker.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for ColourPicker.xaml
    /// </summary>
    public partial class ColourPicker : UserControl
    {
        public delegate void ColourChangedArgs(object sender, ColourChangedEventArgs e);
        public event ColourChangedArgs OnColourChanged;

        public static readonly DependencyProperty SelectedColourProperty =
            DependencyProperty.Register(
                nameof(SelectedColour),
                typeof(Color),
                typeof(ColourPicker),
                new PropertyMetadata(
                    Colors.Black, 
                    new PropertyChangedCallback(
                        SelectedColourChangedCallBack)));
        private static void SelectedColourChangedCallBack(DependencyObject property, DependencyPropertyChangedEventArgs args)
        {
            if (property is ColourPicker colourPicker)
            {
                if(args.OldValue is Color oldVal && args.NewValue is Color newVal)
                {
                    ColourChangedEventArgs ccea = 
                        new ColourChangedEventArgs(oldVal, newVal);

                    colourPicker.OnColourChanged?.Invoke(colourPicker, ccea);
                }
            }
        }

        public Color SelectedColour
        {
            get { return (Color)this.GetValue(SelectedColourProperty); }
            set { this.SetValue(SelectedColourProperty, value); }
        }

        public TranslateTransform PickerTransform { get; set; }
        public ColourPicker()
        {
            InitializeComponent();
            PickerTransform = new TranslateTransform();

            PickerMarker.RenderTransform = PickerTransform;
            PickerMarker.RenderTransformOrigin = new Point(1,1);
        }

        public void SetMainColour(Color colour)
        {
            MainColour.Color = colour;
            UpdatePaletteColour();
        }

        /// <summary>
        /// Moves the picker marker to the mouse's position within the palette
        /// </summary>
        public void UpdateMarkerPosition()
        {
            double x = Mouse.GetPosition(ColourPaletteBase).X - (ColourPaletteBase.ActualWidth / 2);
            double y = Mouse.GetPosition(ColourPaletteBase).Y - (ColourPaletteBase.ActualHeight / 2);
            double widthMax = ColourPaletteBase.ActualWidth / 2;
            double heightMax = ColourPaletteBase.ActualHeight / 2;
            if (x > (-widthMax) && x < widthMax)
                PickerTransform.X = x;
            if (y > (-heightMax) && y < heightMax)
                PickerTransform.Y = y;

            UpdatePaletteColour();
            //Console.WriteLine($"MaxMinWidth: {ColourPaletteBase.ActualWidth / 2}");
            //Console.WriteLine($"MaxMinHeight: {ColourPaletteBase.ActualHeight / 2}");
            //Console.WriteLine($"Mouse X: {x}");
            //Console.WriteLine($"Mouse Y: {y}");
        }

        public Color GetPaletteMarketLocationColour()
        {
            Point markerPosition = PickerMarker.TransformToAncestor(ColourPaletteBase).Transform(new Point(0.5, 0.5));
            //Point markerPosition = Mouse.GetPosition(ColourPalette);
            RenderTargetBitmap renderTargetBitmap =
                new RenderTargetBitmap(
                    (int)ColourPalette.ActualWidth,
                    (int)ColourPalette.ActualHeight,
                    96, 96, PixelFormats.Default);
            
            renderTargetBitmap.Render(ColourPalette);

            // Make sure that the point is within the dimensions of the image.
            if (markerPosition.X <= renderTargetBitmap.PixelWidth && markerPosition.Y <= renderTargetBitmap.PixelHeight)
            {
                try
                {
                    // Create a cropped image at the supplied point coordinates.
                    CroppedBitmap croppedBitmap =
                        new CroppedBitmap(
                            renderTargetBitmap,
                            new Int32Rect(
                                (int)markerPosition.X,
                                (int)markerPosition.Y, 1, 1));

                    // Copy the sampled pixel to a byte array.
                    byte[] pixels = new byte[4];
                    croppedBitmap.CopyPixels(pixels, 4, 0);

                    // Assign the sampled color to a SolidColorBrush and return as conversion.
                    return Color.FromArgb(255, pixels[2], pixels[1], pixels[0]);
                }
                catch
                {
                    return Colors.Black;

                }
            }
            return Colors.Black;
        }

        public void UpdatePaletteColour()
        {
            SelectedColour = GetPaletteMarketLocationColour();
        }

        private void ColourPaletteLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                UpdateMarkerPosition();
            }
            catch { }
        }

        private void ColourPaletteMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    UpdateMarkerPosition();
                }
                catch { }
            }
        }
    }
}

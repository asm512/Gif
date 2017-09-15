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
using Microsoft.Win32;
using System.IO;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gif2png
{
    /// <summary>
    /// Interaction logic for mainControl.xaml
    /// </summary>
    public partial class mainControl : UserControl
    {
        public mainControl()
        {
            InitializeComponent();
        }

        static double ConvertByteToMegabyte(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        private void selectGIF_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openGif = new OpenFileDialog();
            openGif.Filter = "GIF (*.gif)|*.gif";
            openGif.Title = "Select GIF";
            if (openGif.ShowDialog() == true)
            {
                Int64 fileSizeInBytes = new FileInfo(openGif.FileName).Length;
                gifSizeattrib.Text = Math.Round(ConvertByteToMegabyte(fileSizeInBytes), 2) + " Mb";
                gifNameattrib.Text = openGif.SafeFileName;
            }
        }
    }
}

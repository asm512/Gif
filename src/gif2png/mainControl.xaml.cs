using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing.Imaging;

namespace gif2png
{
    /// <summary>
    /// Interaction logic for mainControl.xaml
    /// </summary>
    public partial class mainControl : System.Windows.Controls.UserControl
    {
        public mainControl()
        {
            InitializeComponent();

            pathCombobox.Items.Add("SELECT EXTRACTION PATH");
            pathCombobox.Items.Add(@"Default (\extractedFrames)");
            pathCombobox.Items.Add("Custom");





        }

        static double ConvertByteToMegabyte(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        private void selectGIF_Click(object sender, RoutedEventArgs e)
        {
            var tempINI = new IniFile("temp.ini");
            Microsoft.Win32.OpenFileDialog openGif = new Microsoft.Win32.OpenFileDialog();
            openGif.Filter = "GIF (*.gif)|*.gif";
            openGif.Title = "Select GIF";
            if (openGif.ShowDialog() == true)
            {
                System.Drawing.Image gifImg = System.Drawing.Image.FromFile(openGif.FileName);

                FrameDimension dimension = new FrameDimension(gifImg.FrameDimensionsList[0]);
                int frames = gifImg.GetFrameCount(dimension);

                if (frames <= 1)
                {
                    System.Windows.Forms.MessageBox.Show("Image is not animated (contains 1 or less frames)");
                    gifSizeattrib.Text = string.Empty;
                    gifNameattrib.Text = string.Empty;
                    gifFramesattrib.Text = string.Empty;
                }
                else
                {
                    Int64 fileSizeInBytes = new FileInfo(openGif.FileName).Length;
                    gifSizeattrib.Text = Math.Round(ConvertByteToMegabyte(fileSizeInBytes), 3) + " Mb";
                    gifNameattrib.Text = openGif.SafeFileName;
                    string comboBoxItem = openGif.SafeFileName.Remove(openGif.SafeFileName.Length - 4);
                    pathCombobox.Items.Add(@"\" + comboBoxItem);
                    prefixInput.Text = openGif.SafeFileName.Remove(openGif.SafeFileName.Length - 4);
                    tempINI.Write("gifName", openGif.SafeFileName);
                    tempINI.Write("gifPath", openGif.FileName);
                    gifFramesattrib.Text = frames.ToString();
                }
            }
        }

        private void extractFrames_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainWindowContainer.Children.Clear();
            extraction extraction = new extraction();
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainWindowContainer.Children.Add(extraction);
        }

        private void formatCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tempINI = new IniFile("temp.ini");
            tempINI.Write("imageFormat", formatCombobox.SelectedIndex.ToString());
        }

        private void pathCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tempINI = new IniFile("temp.ini");
            if (pathCombobox.SelectedItem.ToString() == "Custom")
            {
                using (var selectextractPath = new FolderBrowserDialog())
                {
                    DialogResult result = selectextractPath.ShowDialog();

                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(selectextractPath.SelectedPath))
                    {
                        tempINI.Write("extractPath", selectextractPath.SelectedPath);   
                    }
                    else
                    {
                        tempINI.Write("extractPath", "1");
                        pathCombobox.SelectedIndex = 1;
                    }
                }
            }
            else
            {
                tempINI.Write("extractPath", pathCombobox.SelectedIndex.ToString());
            }
        }

        private void prefixInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tempINI = new IniFile("temp.ini");
            tempINI.Write("prefix", prefixInput.Text);
        }
    }
}

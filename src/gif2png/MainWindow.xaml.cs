using System;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using System.Windows.Forms;
using System.Collections.ObjectModel;

namespace gif2png
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Gif gif;
        private ObservableCollection<string> extractionFolders = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
            pathCombobox.ItemsSource = extractionFolders;
            extractionFolders.Add("SELECT EXTRACTION PATH");
            extractionFolders.Add(@"Default (\extractedFrames)");
            extractionFolders.Add("Custom");
        }

        private void selectGIF_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openGif = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "GIF (*.gif)|*.gif",
                Title = "Select GIF"
            };
            if (openGif.ShowDialog() == true)
            {
                gif = new Gif(openGif.FileName);

                gifNameattrib.Text = gif.ToString();
                gifSizeattrib.Text = $"{gif.Size().ToString()}Mb";
                gifFramesattrib.Text = gif.FrameCount().ToString();
            }
        }

        private void extractFrames_Click(object sender, RoutedEventArgs e)
        {
            var format = Gif.OutputFormat.BMP;
            switch (formatCombobox.Text)
            {
                case "BMP":
                    format = Gif.OutputFormat.BMP;
                    break;
                case "ICO":
                    format = Gif.OutputFormat.ICO;
                    break;
                case "JPG":
                    format = Gif.OutputFormat.JPG;
                    break;
                case "PNG":
                    format = Gif.OutputFormat.PNG;
                    break;
                default:
                    System.Windows.MessageBox.Show("Invalid format selected");
                    return;
            }
            if (String.IsNullOrWhiteSpace(prefixInput.Text)) { gif.Extract(format, prefixInput.Text); }
            else { gif.Extract(format, prefixInput.Text); }
            
        }

        private void pathCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pathCombobox.SelectedItem.ToString() == "Custom")
            {
                using (var selectextractPath = new FolderBrowserDialog())
                {
                    DialogResult result = selectextractPath.ShowDialog();

                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(selectextractPath.SelectedPath))
                    {
                        gif.SetExtractPath(selectextractPath.SelectedPath);
                    }
                }
            }
            else
            {
                // :/ i know.
                if (pathCombobox.SelectedItem.ToString().Contains("Default"))
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "extractedFrames"));
                    gif.SetExtractPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"extractedFrames"));
                }
            }
        }
    }
}

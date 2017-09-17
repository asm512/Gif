using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Windows.Input;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gif2png
{
    /// <summary>
    /// Interaction logic for extraction.xaml
    /// </summary>
    public partial class extraction : System.Windows.Controls.UserControl
    {
        private void returntoSettings()
        {
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainWindowContainer.Children.Clear();
            mainControl mainControl = new mainControl();
            ((MainWindow)System.Windows.Application.Current.MainWindow).mainWindowContainer.Children.Add(mainControl);
        }

        private void returnButtonVisibility()
        {
            progressBar.Visibility = Visibility.Hidden;
            returnButton.Visibility = Visibility.Visible;
        }

        private void finishedExtraction()
        {
            progressBar.Visibility = Visibility.Hidden;
            finishedButton.Visibility = Visibility.Visible;
        }
        
        ////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathtoGif"></param>
        /// <param name="extractPath"></param>
        /// <param name="Format"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public void SplitGif(string pathtoGif, string extractPath, string Format, string prefix)
        {
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            //JPG
            if (Format == "JPG")
            {
                //JPG and JPEG files are indentical :)
                System.Drawing.Image gifImg = System.Drawing.Image.FromFile(pathtoGif);
                int frames = gifImg.GetFrameCount(FrameDimension.Time);
                if (frames <= 1) System.Windows.MessageBox.Show("no frames");
                for (int i = 0; i < frames; i++)
                {
                    gifImg.SelectActiveFrame(FrameDimension.Time, i);
                    var outputFile = extractPath + prefix + i + ".jpeg";
                    gifImg.Save(outputFile, ImageFormat.Jpeg);
                }

                System.Windows.MessageBox.Show("done to jpg");
            }

            //JPG
            else if (Format == "PNG")
            {
                System.Drawing.Image gifImg = System.Drawing.Image.FromFile(pathtoGif);
                int frames = gifImg.GetFrameCount(FrameDimension.Time);
                if (frames <= 1) System.Windows.MessageBox.Show("no frames");
                for (int i = 0; i < frames; i++)
                {
                    gifImg.SelectActiveFrame(FrameDimension.Time, i);
                    var outputFile = extractPath + prefix + i + ".png";
                    gifImg.Save(outputFile, ImageFormat.Png);
                }

                System.Windows.MessageBox.Show("done to png");
            }

            else
            {
                System.Windows.MessageBox.Show("unable to resolve format");
            }
        }
        ////////////////////////////////////////////////////////////////////////////////////////////////

        public extraction()
        {
            InitializeComponent();

            int x = 0;
            int y = 0;
            string trueFormat;
            string truePath;

            var tempINI = new IniFile("temp.ini");

            string gifPath = tempINI.Read("gifPath");

            //Image format
            int formatIndex = Convert.ToInt32(tempINI.Read("imageFormat"));

            if(formatIndex == 0)
            {
                formatAttrib.Text = "No format selected";
            }
            else if(formatIndex == 1)
            {
                formatAttrib.Text = "JPG";
                x++;
                trueFormat = "JPG";
                File.WriteAllText("temp", trueFormat);
            }
            else if (formatIndex == 2)
            {
                formatAttrib.Text = "PNG";
                x++;
                trueFormat = "PNG";
                File.WriteAllText("temp", trueFormat);
            }
            else
            {
                formatAttrib.Text = "Unable to resolve format";
                returnButtonVisibility();
            }


            //Extract path
            string extractpathIndex = tempINI.Read("extractPath");

            if (extractpathIndex == "0")
            {
                extractpathAttrib.Text = @"No extract path selected";
            }
            else if (extractpathIndex == "1")
            {
                extractpathAttrib.Text = @"\extracted";
                File.WriteAllText("extractPath", System.Windows.Forms.Application.StartupPath + @"\extracted\");
                x++;
            }
            else if (Directory.Exists(extractpathIndex))
            {
                extractpathAttrib.Text = extractpathIndex;
                x++;
            }
            else if (extractpathIndex == "3")
            {
                extractpathAttrib.Text = extractpathIndex;
                x++;
            }
            else
            {
                extractpathAttrib.Text = "Unable to resolve extract path";
                returnButtonVisibility();
            }


            //Prefix 

            prefixAttrib.Text = tempINI.Read("prefix");
            x++;


            

            if(x == 3)
            {
                ////Timer so that the metro ring is actually visible before extraction
                //System.Windows.Forms.Timer progressRIng = new System.Windows.Forms.Timer { Interval = 750 };
                //progressRIng.Start();
                //progressRIng.Tick += (o, args) =>
                //{
                //    if (y < 2)
                //    {
                //        y++;
                //    }
                //    else
                //    {
                //        SplitGif(gifPath, File.ReadAllText("extractPath"), File.ReadAllText("temp"), tempINI.Read("prefix"));
                //        System.Windows.MessageBox.Show("FORMAT :" + File.ReadAllText("temp"));
                //        System.Windows.MessageBox.Show("EXTRACTPATH :" + File.ReadAllText("extractPath"));
                //        finishedExtraction();
                //        //(gifPath, extractpathAttrib, formatAttrib, prefixAttrib);
                //        progressRIng.Stop();
                //        returntoSettings();
                //    }
                //};


                SplitGif(gifPath, File.ReadAllText("extractPath"), File.ReadAllText("temp"), tempINI.Read("prefix"));
                System.Windows.MessageBox.Show("FORMAT :" + File.ReadAllText("temp"));
                System.Windows.MessageBox.Show("EXTRACTPATH :" + File.ReadAllText("extractPath"));
                finishedExtraction();
            }

            else
            {
                returntoSettings();
            }

        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            returntoSettings();
        }

        private void finishedButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(File.ReadAllText("extractPath"));
        }
    }
}

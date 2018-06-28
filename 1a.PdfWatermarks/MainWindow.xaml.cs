using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
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

namespace _1a.PdfWatermarks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public string filename;

        private void chooseButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension

            dlg.DefaultExt = ".pdf";

            dlg.Filter = "PDF documents (.pdf)|*.pdf";

            // Display OpenFileDialog by calling ShowDialog method

            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox

            if (result == true)

            {
                // Open document

                filename = dlg.FileName;

                textBox.Text = filename;

            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void addWatermark_Click(object sender, RoutedEventArgs e)
        {
            // Take existing pdf
            PdfDocument document = PdfReader.Open(filename, PdfDocumentOpenMode.Modify);

            var pagesCount = document.Pages.Count;



            string watermark = "Kowal";

            //Set Font

            XFont font = new XFont("Times New Roman", 100, XFontStyle.Bold);

            int n = 0;

            while (n < pagesCount)
            {


                var page = document.Pages[n];
                // Get an XGraphics object for drawing beneath the existing content
                XGraphics gfx = XGraphics.FromPdfPage(page, XGraphicsPdfPageOptions.Prepend);

                // Get the size (in point) of the text
                XSize size = gfx.MeasureString(watermark, font);

                // Define a rotation transformation at the center of the page
                gfx.TranslateTransform(page.Width / 2, page.Height / 2);
                gfx.RotateTransform(-Math.Atan(page.Height / page.Width) * 180 / Math.PI);
                gfx.TranslateTransform(-page.Width / 2, -page.Height / 2);

                // Create a string format
                XStringFormat format = new XStringFormat();
                format.Alignment = XStringAlignment.Near;
                format.LineAlignment = XLineAlignment.Near;

                // Create a dimmed red brush
                XBrush brush = new XSolidBrush(XColor.FromArgb(128, 255, 0, 0));


                // Draw the string
                gfx.DrawString(watermark, font, brush,
                new XPoint((page.Width - size.Width) / 2, (page.Height - size.Height) / 2), format);
                n++;
            }


            //Save Document
            document.Save(filename);

        }
    }
}

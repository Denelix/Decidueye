/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Decidueye
{
    class Reader
    {
        //x540 y1045   x500 y1030
        public static string Read(int x, int y, int width, int height)
        {
            Variables.isReading = true;
            Bitmap grayBmp = null;

            Rectangle sourceRect = new Rectangle(x, y, width, height);
            using (Bitmap bmp = new Bitmap(width, height))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(sourceRect.Location, Point.Empty, sourceRect.Size);
                grayBmp = ToGrayScale(bmp);
            }
            try
            {
                grayBmp.Save("C:\\Users\\Denny\\AppData\\Roaming\\zDecidueye\\screenshot.png");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving image: " + ex.ToString());
            }
            using (TesseractEngine engine = new TesseractEngine(@"C:\Users\Denny\source\repos\Decidueye\Decidueye\tessdata", "eng", EngineMode.Default))
            {
                // Set the page segmentation mode to auto
                engine.DefaultPageSegMode = PageSegMode.Auto;
                using (var ms = new MemoryStream())
                {

                    using (grayBmp)
                    {
                        grayBmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // save the grayscale bitmap to the memory stream in PNG format
                    }

                    byte[] bitmapToMemory = ms.ToArray(); // extract the byte array from the memory stream

                    using (var page = engine.Process(Pix.LoadFromMemory(bitmapToMemory)))
                    {
                        string text = page.GetText();
                        Variables.isReading = false;
                        return text;
                    }
                }
            }
        }
        public static double TextToDouble(string text)
        {
            if (double.TryParse(text, out double result))
            {
                return result;
            }
            return 1;
        }

        private static Bitmap ToGrayScale(Bitmap bmp)
        {
            // Create a new grayscale bitmap with the same dimensions as the input image
            Bitmap grayBmp = new Bitmap(bmp.Width, bmp.Height);

            // Iterate through the pixels of the input image
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    // Get the pixel color from the input image
                    Color pixelColor = bmp.GetPixel(x, y);

                    // Calculate the grayscale value using the average method
                    int grayValue = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                    // Create the grayscale color
                    Color grayColor = Color.FromArgb(pixelColor.A, grayValue, grayValue, grayValue);

                    // Set the grayscale color in the new bitmap
                    grayBmp.SetPixel(x, y, grayColor);
                }
            }

            // Return the grayscale bitmap
            return grayBmp;
        }
    }
}
*/
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RosettaJuliaSet
{
    class Program
    {
        static void Main(string[] args)
        {
            int frames = 360;
            double num = 0;
            for (int i=0; i<frames; i++)
            {
                //Function to use for circular set. (Starting at 0 + i)
                double degree = System.Convert.ToDouble(i) * (System.Math.PI / 180);
                //MessageBox.Show(System.Math.Cos(degree).ToString() + " / " + System.Math.Sin(degree).ToString());
                generateImage(System.Math.Cos(degree), System.Math.Sin(degree), i); //Function to use for circular set.

                //Function to use for linear set (0 + 0i => 1 + 1i)
                //num = System.Convert.ToDouble(i) / frames;
                //generateImage(num, num, i);
            }


        }

        public static void generateImage(double num, double imaginary, int frame)
        {
            int w = 1200; //Dimensions of image in pixels.
            int h = 800;
            double zoom = 0.8;
            int maxiter = 255; //Maximum number of iterations to test in each equation.
            int moveX = 0;
            int moveY = 0;
            double cX = num; //Rational number.
            double cY = imaginary; //Imaginary coefficient.
            double zx, zy, tmp; //Placeholders, to be used in future calculations.
            int i;

            var colors = (from c in Enumerable.Range(0, 256)
                          select Color.FromArgb((c >> 5) * 36, (c >> 3 & 7) * 36, (c & 3) * 85)).ToArray(); //Color scheme inherited from RosettaCode.

            
            var julia_img = new Bitmap(w, h); //Generate placeholder bitmap file to write colors to.
            for (int x = 0; x < w; x++) //Parent loop through horizontal pixels.
            {
                for (int y = 0; y < h; y++) //Child loop through vertical pixels.
                {
                    zx = 1.5 * (x - w / 2) / (0.5 * zoom * w) + moveX; //Image scaling calculations.
                    zy = 1.0 * (y - h / 2) / (0.5 * zoom * h) + moveY;
                    i = maxiter;
                    while (zx * zx + zy * zy < 4 && i > 1) //Test for number of iterations before reaching infinity.
                    {
                        tmp = zx * zx - zy * zy + cX; //Abstracted julia set equation. (Using programmed variables.)
                        zy = 2.0 * zx * zy + cY;
                        zx = tmp;
                        i -= 1; //For each successful iteration, subtract from max iteration (to be used in color determination).
                    }

                    double value = (System.Convert.ToDouble(i) / maxiter) * 255; //Determine gray value based on number of iterations.
                    int graycolor = System.Convert.ToInt32(System.Math.Round(value));

                    System.Drawing.Color juliaShade = Color.FromArgb(graycolor, graycolor, graycolor);

                    julia_img.SetPixel(x, y, juliaShade); //Set value in drawing, pixel by pixel.
                }
            }
            julia_img.Save("D" + frame.ToString().PadLeft(3, '0') + ".png"); //Save image as one frame of animation.
            julia_img.Dispose();
        }
    }
}
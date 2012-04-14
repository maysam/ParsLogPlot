using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace myImage
{
    public static class imageutilities
    {
        #region utilities
        public static Bitmap tile(Bitmap bmp, int width, int height)
        {
            if ((width < 1) | (height < 1))
                return bmp;
            Bitmap test = new Bitmap(width, height);
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    test.SetPixel(i, j, bmp.GetPixel(i % bmp.Width, j % bmp.Height));
            return test;
        }
        public static Bitmap recolor(Bitmap bitmap, Color[] src, Color[] dest)
        {
            Bitmap bmp = (Bitmap)bitmap.Clone();
            if (src.Length != dest.Length)
                throw new Exception("RECOLOR: Source and Destination must be of the same size!");
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            IntPtr ptr = bmpData.Scan0;
            int bytes = bmp.Width * bmp.Height * 4;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            for (int counter = 0; counter < rgbValues.Length; counter += 4)
                for (int i = 0; i < src.Length; i++)
                    if ((rgbValues[counter + 0] == src[i].B) && (rgbValues[counter + 1] == src[i].G) && (rgbValues[counter + 2] == src[i].R) && (rgbValues[counter + 3] == src[i].A))
                    {
                        rgbValues[counter + 0] = dest[i].B;
                        rgbValues[counter + 1] = dest[i].G;
                        rgbValues[counter + 2] = dest[i].R;
                        rgbValues[counter + 3] = dest[i].A;
                    }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        public static bool isin(int a, int b, int c)
        {
            return ((a >= b) & (a <= c));
        }
        public static bool bit(ref string s, int i)
        {
            s = s.PadRight(i + 1, '0');
            return (s[i] == '1');

        }
        public static void setbit(ref string s, int i, string val)
        {
            s = s.PadRight(i+1, '0');
            char c = s[i];
            s = s.Remove(i, 1);
            s = s.Insert(i, val);
        }
        public static Bitmap resize(Bitmap bmp, float zoom)
        {
            Bitmap test = new Bitmap((int)Math.Ceiling(bmp.Width * zoom), (int)Math.Ceiling(bmp.Height * zoom));
                for (int i = 0; i < bmp.Width; i++)
                    for (int j = 0; j < bmp.Height; j++)
                        for (int k = 0; k < zoom; k++)
                            for (int h = 0; h < zoom; h++)
                                try
                                {
                                    test.SetPixel((int)(i * zoom + k), (int)(j * zoom + h), bmp.GetPixel(i, j));
                                }
                                catch
                                {
                                    MessageBox.Show(i.ToString());
                                }
            return test;
        }
        #endregion utilities
    }
}

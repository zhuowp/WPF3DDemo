﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace WPF3DDemo.Helpers
{
    public class TextureMapHelper
    {
        public DiffuseMaterial m_material;
        private bool m_bPseudoColor = false;

        public TextureMapHelper()
        {
            SetRGBMaping();
        }

        //public void SetRGBMaping()
        //{
        //    WriteableBitmap writeableBitmap = new WriteableBitmap(64, 64, 96, 96, PixelFormats.Bgr24, null);
        //    writeableBitmap.Lock();

        //    unsafe
        //    {
        //        // Get a pointer to the back buffer.
        //        byte* pStart = (byte*)(void*)writeableBitmap.BackBuffer;
        //        int nL = writeableBitmap.BackBufferStride;

        //        for (int r = 0; r < 16; r++)
        //        {
        //            for (int g = 0; g < 16; g++)
        //            {
        //                for (int b = 0; b < 16; b++)
        //                {
        //                    int nX = (g % 4) * 16 + b;
        //                    int nY = r * 4 + (int)(g / 4);

        //                    *(pStart + nY * nL + nX * 3 + 0) = (byte)(b * 17);
        //                    *(pStart + nY * nL + nX * 3 + 1) = (byte)(g * 17);
        //                    *(pStart + nY * nL + nX * 3 + 2) = (byte)(r * 17);
        //                }
        //            }
        //        }

        //    }
        //    writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 64, 64));

        //    // Release the back buffer and make it available for display.
        //    writeableBitmap.Unlock();

        //    ImageBrush imageBrush = new ImageBrush(writeableBitmap);
        //    //ImageBrush imageBrush = new ImageBrush(imSrc);
        //    imageBrush.ViewportUnits = BrushMappingMode.Absolute;
        //    m_material = new DiffuseMaterial();
        //    m_material.Brush = imageBrush;

        //    m_bPseudoColor = false;
        //}
        public void SetRGBMaping()
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(64, 64, 96, 96, PixelFormats.Bgra32, null);
            writeableBitmap.Lock();

            unsafe
            {
                // Get a pointer to the back buffer.
                byte* pStart = (byte*)(void*)writeableBitmap.BackBuffer;
                int nL = writeableBitmap.BackBufferStride;

                for (int r = 0; r < 16; r++)
                {
                    for (int g = 0; g < 16; g++)
                    {
                        for (int b = 0; b < 16; b++)
                        {
                            int nX = (g % 4) * 16 + b;
                            int nY = r * 4 + (int)(g / 4);

                            //*(pStart + nY * nL + nX * 3 + 0) = (byte)(b * 17);
                            //*(pStart + nY * nL + nX * 3 + 1) = (byte)(g * 17);
                            *(pStart + nY * nL + nX * 3 + 2) = (byte)(r * 17);
                            *(pStart + nY * nL + nX * 3 + 3) = (byte)(r * 17);
                        }
                    }
                }

            }
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 64, 64));

            // Release the back buffer and make it available for display.
            writeableBitmap.Unlock();

            ImageBrush imageBrush = new ImageBrush(writeableBitmap);
            //ImageBrush imageBrush = new ImageBrush(imSrc);
            imageBrush.ViewportUnits = BrushMappingMode.Absolute;
            m_material = new DiffuseMaterial();
            m_material.Brush = imageBrush;

            m_bPseudoColor = false;
        }

        public void SetPseudoMaping()
        {
            WriteableBitmap writeableBitmap = new WriteableBitmap(64, 64, 96, 96, PixelFormats.Bgr24, null);
            writeableBitmap.Lock();

            unsafe
            {
                // Get a pointer to the back buffer.
                byte* pStart = (byte*)(void*)writeableBitmap.BackBuffer;
                int nL = writeableBitmap.BackBufferStride;

                for (int nY = 0; nY < 64; nY++)
                {
                    for (int nX = 0; nX < 64; nX++)
                    {
                        int nI = nY * 64 + nX;
                        double k = ((double)nI) / 4095;

                        Color color = PseudoColor(k);

                        *(pStart + nY * nL + nX * 3 + 0) = (byte)(color.B);
                        *(pStart + nY * nL + nX * 3 + 1) = (byte)(color.G);
                        *(pStart + nY * nL + nX * 3 + 2) = (byte)(color.R);
                    }
                }

            }
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 64, 64));

            // Release the back buffer and make it available for display.
            writeableBitmap.Unlock();

            ImageBrush imageBrush = new ImageBrush(writeableBitmap);
            //ImageBrush imageBrush = new ImageBrush(imSrc);
            imageBrush.ViewportUnits = BrushMappingMode.Absolute;
            m_material = new DiffuseMaterial();
            m_material.Brush = imageBrush;

            m_bPseudoColor = true;
        }

        public Point GetMappingPosition(Color color)
        {
            return GetMappingPosition(color, m_bPseudoColor);
        }

        public static Point GetMappingPosition(Color color, bool bPseudoColor)
        {
            if (bPseudoColor)
            {
                double r = ((double)color.R) / 255;
                double g = ((double)color.G) / 255;
                double b = ((double)color.B) / 255;

                double k = 0;
                if ((b >= g) && (b > r))
                {
                    k = 0.25 * g;
                }
                else if ((g > b) && (b >= r))
                {
                    k = 0.25 + 0.25 * (1 - b);
                }
                else if ((g >= r) && (r > b))
                {
                    k = 0.5 + 0.25 * r;
                }
                else
                {
                    k = 0.75 + 0.25 * (1 - g);
                }
                int nI = (int)(k * 4095);
                if (nI < 0) nI = 0;
                if (nI > 4095) nI = 4095;

                int nY = nI / 64;
                int nX = nI % 64;

                double x1 = (double)nX;
                double y1 = (double)nY;
                return new Point(x1 / 64, y1 / 64);
            }
            else
            {
                int nR = (color.R) / 17;
                int nG = (color.G) / 17;
                int nB = (color.B) / 17;

                int nX = (nG % 4) * 16 + nB;
                int nY = nR * 4 + (int)(nG / 4);

                double x1 = (double)nX;
                double y1 = (double)nY;
                return new Point(x1 / 63, y1 / 63);
            }
        }

        // color according to the z value
        static public Color PseudoColor(double k)
        {
            if (k < 0) k = 0;
            if (k > 1) k = 1;

            double r, g, b;
            r = b = g = 0;
            if (k < 0.25)
            {
                r = 0;
                g = 4 * k;
                b = 1;
            }
            else if (k < 0.5)
            {
                r = 0;
                g = 1;
                b = 1 - 4 * (k - 0.25);
            }
            else if (k < 0.75)
            {
                r = 4 * (k - 0.5);
                g = 1;
                b = 0;
            }
            else
            {
                r = 1;
                g = 1 - 4 * (k - 0.75);
                b = 0;
            }

            byte R = (byte)(r * 255 + 0.0);
            byte G = (byte)(g * 255 + 0.0);
            byte B = (byte)(b * 255 + 0.0);

            return Color.FromRgb(R, G, B);
        }
    }
}

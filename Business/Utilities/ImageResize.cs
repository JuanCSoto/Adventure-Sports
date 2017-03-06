// <copyright file="ImageResize.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business
{
    using System;
    using System.Configuration;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;

    /// <summary>
    /// resize images
    /// </summary>
    public class ImageResize
    {
        /// <summary>
        /// path of application
        /// </summary>
        private string pathServer;

        /// <summary>
        /// if the operation is successful
        /// </summary>
        private bool isSuccessful = true;

        /// <summary>
        /// width of image
        /// </summary>
        private int width = Convert.ToInt32(ConfigurationManager.AppSettings["WidthThumb"]);

        /// <summary>
        /// height of image
        /// </summary>
        private int height = Convert.ToInt32(ConfigurationManager.AppSettings["HeightThumb"]);

        /// <summary>
        /// prefix of image
        /// </summary>
        private string prefix = "thumb_";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageResize"/> class
        /// </summary>
        /// <param name="pathServer">server path</param>
        public ImageResize(string pathServer)
        {
            this.pathServer = pathServer;
        }

        /// <summary>
        /// resize type
        /// </summary>
        public enum TypeResize
        {
            /// <summary>
            /// <para></para>
            /// </summary>
            Proportional = 0,

            /// <summary>
            /// <para></para>
            /// </summary>
            Tight,

            /// <summary>
            /// <para></para>
            /// </summary>
            BackgroundProportional,

            /// <summary>
            /// <para></para>
            /// </summary>
            PartialProportional,

            /// <summary>
            /// <para></para>
            /// </summary>
            CropProportional
        }

        /// <summary>
        /// Gets or sets the new image width
        /// </summary>
        public int Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        /// <summary>
        /// Gets or sets the new image height
        /// </summary>
        public int Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        /// <summary>
        /// Gets or sets the image prefix
        /// </summary>
        public string Prefix
        {
            get { return this.prefix; }
            set { this.prefix = value; }
        }

        /// <summary>
        /// resize image according to the new dimensions
        /// </summary>
        /// <param name="pathimage">path of image</param>
        /// <param name="type">resize type</param>
        /// <returns>returns true if the operation is success</returns>
        public bool Resize(string pathimage, TypeResize type)
        {
            string imagen = Path.Combine(this.pathServer, pathimage);
            string pathImage = Path.GetDirectoryName(imagen);
            string nameimg = Path.GetFileName(imagen);

            switch (type)
            {
                case TypeResize.Proportional:
                    this.Proportional(imagen, pathImage, nameimg);
                    break;
                case TypeResize.Tight:
                    this.Tight(imagen, pathImage, nameimg);
                    break;
                case TypeResize.BackgroundProportional:
                    this.BackgroundProportional(imagen, pathImage, nameimg);
                    break;
                case TypeResize.PartialProportional:
                    this.PartialProportional(imagen, pathImage, nameimg);
                    break;
                case TypeResize.CropProportional:
                    this.CropProportional(imagen, pathImage, nameimg);
                    break;
            }

            return this.isSuccessful;
        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="imagen">path of image</param>
        /// <param name="pathImage">path where you will save the image</param>
        /// <param name="nameimg">name of new image</param>
        private void CropProportional(string imagen, string pathImage, string nameimg)
        {
            Image original = null;
            Image imgPhoto = null;
            Bitmap bitmapPhoto = null;
            Graphics graphicsPhoto = null;
            int pX = 0, pY = 0, oX = 0, oY = 0;

            try
            {
                original = Image.FromFile(imagen);
                imgPhoto = Image.FromFile(imagen);

                int widthO = original.Width, heigthO = original.Height;

                if (widthO > this.Width)
                {
                    pX = Convert.ToInt32(Math.Abs(widthO - this.Width) / 2);
                }

                if (heigthO > this.Height)
                {
                    pY = Convert.ToInt32(Math.Abs(heigthO - this.Height) / 2);
                }

                SolidBrush background = new SolidBrush(Color.White);
                bitmapPhoto = new Bitmap(this.Width, this.Height, PixelFormat.Format32bppRgb);
                graphicsPhoto = Graphics.FromImage(bitmapPhoto);
                graphicsPhoto.FillRectangle(background, 0, 0, float.Parse(this.Width.ToString()), float.Parse(this.Height.ToString()));
                graphicsPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                graphicsPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsPhoto.DrawImage(imgPhoto, new Rectangle(oX, oY, widthO, heigthO), new Rectangle(pX, pY, original.Width, original.Height), GraphicsUnit.Pixel);
                bitmapPhoto.SetResolution(original.HorizontalResolution, original.VerticalResolution);
                string newimage = Path.Combine(pathImage, this.Prefix + nameimg);
                bitmapPhoto.Save(newimage, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception)
            {
                this.isSuccessful = false;
            }
            finally
            {
                if (graphicsPhoto != null)
                {
                    graphicsPhoto.Dispose();
                }

                if (bitmapPhoto != null)
                {
                    bitmapPhoto.Dispose();
                }

                if (original != null)
                {
                    original.Dispose();
                }

                if (imgPhoto != null)
                {
                    imgPhoto.Dispose();
                }
            }
        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="imagen">path of image</param>
        /// <param name="pathImage">path where you will save the image</param>
        /// <param name="nameimg">name of new image</param>
        private void BackgroundProportional(string imagen, string pathImage, string nameimg)
        {
            Image original = null;
            Image imgagePhoto = null;
            Bitmap bitmapPhoto = null;
            Graphics graphicsPhoto = null;

            try
            {
                original = Image.FromFile(imagen);
                imgagePhoto = Image.FromFile(imagen);

                int heigthO = original.Height, widthO = original.Width;
                int pX = 0, pY = 0;

                if (widthO > this.width)
                {
                    widthO = this.width;
                    heigthO = Convert.ToInt32((Convert.ToDouble(heigthO) * Convert.ToDouble(this.width)) / Convert.ToDouble(original.Width));
                }

                if (heigthO > this.height)
                {
                    int alturao = heigthO;
                    heigthO = this.height;
                    widthO = Convert.ToInt32((Convert.ToDouble(widthO) * Convert.ToDouble(this.height)) / Convert.ToDouble(alturao));
                }

                if (widthO < this.width)
                {
                    pX = Convert.ToInt32((Convert.ToDouble(this.width) - Convert.ToDouble(widthO)) / 2);
                }

                if (heigthO < this.height)
                {
                    pY = Convert.ToInt32((Convert.ToDouble(this.height) - Convert.ToDouble(heigthO)) / 2);
                }

                bitmapPhoto = new Bitmap(this.width, this.height);
                graphicsPhoto = Graphics.FromImage(bitmapPhoto);
                SolidBrush background = new SolidBrush(Color.White);

                graphicsPhoto.FillRectangle(background, 0, 0, float.Parse(this.width.ToString()), float.Parse(this.height.ToString()));
                graphicsPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                graphicsPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsPhoto.DrawImage(imgagePhoto, new Rectangle(pX, pY, widthO, heigthO), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
                bitmapPhoto.SetResolution(original.HorizontalResolution, original.VerticalResolution);
                string newimage = Path.Combine(pathImage, this.prefix + nameimg);
                bitmapPhoto.Save(newimage, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception)
            {
                this.isSuccessful = false;
            }
            finally
            {
                if (graphicsPhoto != null)
                {
                    graphicsPhoto.Dispose();
                }

                if (bitmapPhoto != null)
                {
                    bitmapPhoto.Dispose();
                }

                if (original != null)
                {
                    original.Dispose();
                }

                if (imgagePhoto != null)
                {
                    imgagePhoto.Dispose();
                }
            }
        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="imagen">path of image</param>
        /// <param name="pathImage">path where you will save the image</param>
        /// <param name="nameimg">name of new image</param>
        private void Tight(string imagen, string pathImage, string nameimg)
        {
            Image original = null;
            Image imgagePhoto = null;
            Bitmap bitmapPhoto = null;
            Graphics graphicsPhoto = null;

            try
            {
                original = Image.FromFile(imagen);
                imgagePhoto = Image.FromFile(imagen);

                this.height = this.height <= original.Height ? this.height : original.Height;
                this.width = this.width <= original.Width ? this.width : original.Width;
                bitmapPhoto = new Bitmap(this.width, this.height, PixelFormat.Format24bppRgb);
                graphicsPhoto = Graphics.FromImage(bitmapPhoto);

                graphicsPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                graphicsPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsPhoto.DrawImage(imgagePhoto, new Rectangle(0, 0, this.width, this.height), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);

                string newimage = Path.Combine(pathImage, this.prefix + nameimg);
                bitmapPhoto.Save(newimage, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception)
            {
                this.isSuccessful = false;
            }
            finally
            {
                if (graphicsPhoto != null)
                {
                    graphicsPhoto.Dispose();
                }

                if (bitmapPhoto != null)
                {
                    bitmapPhoto.Dispose();
                }

                if (original != null)
                {
                    original.Dispose();
                }

                if (imgagePhoto != null)
                {
                    imgagePhoto.Dispose();
                }
            }
        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="imagen">path of image</param>
        /// <param name="pathImage">path where you will save the image</param>
        /// <param name="nameimg">name of new image</param>
        private void Proportional(string imagen, string pathImage, string nameimg)
        {
            Image original = null;
            Image imgagePhoto = null;
            Bitmap bitmapPhoto = null;
            Graphics graphicsPhoto = null;

            try
            {
                original = Image.FromFile(imagen);
                imgagePhoto = Image.FromFile(imagen);

                int heigthO = original.Height, widthO = original.Width;

                if (widthO > this.width)
                {
                    widthO = this.width;
                    heigthO = Convert.ToInt32((Convert.ToDouble(heigthO) * Convert.ToDouble(this.width)) / Convert.ToDouble(original.Width));
                }

                if (heigthO > this.height)
                {
                    int alturao = heigthO;
                    heigthO = this.height;
                    widthO = Convert.ToInt32((Convert.ToDouble(widthO) * Convert.ToDouble(this.height)) / Convert.ToDouble(alturao));
                }

                bitmapPhoto = new Bitmap(widthO, heigthO, PixelFormat.Format32bppRgb);
                graphicsPhoto = Graphics.FromImage(bitmapPhoto);

                graphicsPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                graphicsPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

                graphicsPhoto.DrawImage(imgagePhoto, new Rectangle(0, 0, widthO, heigthO), new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
                bitmapPhoto.SetResolution(original.HorizontalResolution, original.VerticalResolution);
                string newimage = Path.Combine(pathImage, this.prefix + nameimg);
                bitmapPhoto.Save(newimage, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception)
            {
                this.isSuccessful = false;
            }
            finally
            {
                if (graphicsPhoto != null)
                {
                    graphicsPhoto.Dispose();
                }

                if (bitmapPhoto != null)
                {
                    bitmapPhoto.Dispose();
                }

                if (original != null)
                {
                    original.Dispose();
                }

                if (imgagePhoto != null)
                {
                    imgagePhoto.Dispose();
                }
            }
        }

        /// <summary>
        /// <para></para>
        /// </summary>
        /// <param name="imagen">path of image</param>
        /// <param name="pathImage">path where you will save the image</param>
        /// <param name="nameimg">name of new image</param>
        private void PartialProportional(string imagen, string pathImage, string nameimg)
        {
            Image original = null;
            Image imgagePhoto = null;
            Bitmap bitmapPhoto = null;
            Graphics graphicsPhoto = null;
            int pX = 0, pY = 0, oX = 0, oY = 0;

            try
            {
                original = Image.FromFile(imagen);
                imgagePhoto = Image.FromFile(imagen);

                int heigthO = original.Height, widthO = original.Width;

                if (widthO > this.width)
                {
                    widthO = this.width;
                    heigthO = Convert.ToInt32((Convert.ToDouble(heigthO) * Convert.ToDouble(this.width)) / Convert.ToDouble(original.Width));
                    pY = Convert.ToInt32(Math.Abs(heigthO - this.height) / 2);

                    if (this.height > heigthO)
                    {
                        oY = Convert.ToInt32(Math.Abs(this.height - heigthO) / 2);
                    }
                }
                else if (heigthO > this.height)
                {
                    int alturao = heigthO;
                    heigthO = this.height;
                    widthO = Convert.ToInt32((Convert.ToDouble(widthO) * Convert.ToDouble(this.height)) / Convert.ToDouble(alturao));

                    if (this.width > widthO)
                    {
                        oX = Convert.ToInt32(Math.Abs(this.width - widthO) / 2);
                    }
                }

                SolidBrush background = new SolidBrush(Color.FromArgb(245, 245, 245));
                bitmapPhoto = new Bitmap(this.width, this.height, PixelFormat.Format32bppRgb);
                graphicsPhoto = Graphics.FromImage(bitmapPhoto);
                graphicsPhoto.FillRectangle(background, 0, 0, float.Parse(this.width.ToString()), float.Parse(this.height.ToString()));
                graphicsPhoto.SmoothingMode = SmoothingMode.AntiAlias;
                graphicsPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphicsPhoto.DrawImage(imgagePhoto, new Rectangle(oX, oY, widthO, heigthO), new Rectangle(pX, pY, original.Width, original.Height), GraphicsUnit.Pixel);
                bitmapPhoto.SetResolution(original.HorizontalResolution, original.VerticalResolution);
                string newimage = Path.Combine(pathImage, this.prefix + nameimg);
                bitmapPhoto.Save(newimage, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception)
            {
                this.isSuccessful = false;
            }
            finally
            {
                if (graphicsPhoto != null)
                {
                    graphicsPhoto.Dispose();
                }

                if (bitmapPhoto != null)
                {
                    bitmapPhoto.Dispose();
                }

                if (original != null)
                {
                    original.Dispose();
                }

                if (imgagePhoto != null)
                {
                    imgagePhoto.Dispose();
                }
            }
        }
    }
}

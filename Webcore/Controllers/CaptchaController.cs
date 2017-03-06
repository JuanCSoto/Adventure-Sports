// ---------------------------------------------------------------------
// <copyright file="CaptchaController.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno</author>
// ---------------------------------------------------------------------

namespace Webcore.Controllers
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;

    /// <summary>
    /// Controller used for handling all the captcha requests
    /// </summary>
    public class CaptchaController : Controller
    {
        /// <summary>
        /// Height of the captcha image
        /// </summary>
        private const int Height = 30;

        /// <summary>
        /// Width of the captcha image
        /// </summary>
        private const int Width = 80;

        /// <summary>
        /// Length of the captcha text
        /// </summary>
        private const int Length = 4;

        /// <summary>
        /// Chars allowed to be used in the captcha
        /// </summary>
        private const string Chars = "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789";

        /// <summary>
        /// Validates if the value of the captcha is valid or not
        /// </summary>
        /// <param name="captchaValue">Value of the captcha</param>
        /// <returns>Boolean indicating if it's valid or not</returns>
        public static bool IsValidCaptchaValue(string captchaValue)
        {
            var expectedHash = System.Web.HttpContext.Current.Session["CaptchaHash"];
            var toCheck = captchaValue.ToUpper() + GetSalt();
            var hash = ComputeMd5Hash(toCheck);
            return hash.Equals(expectedHash);
        }

        /// <summary>
        /// Creates a new captcha image
        /// </summary>
        /// <returns>Image of the captcha</returns>
        public ActionResult Show()
        {
            var randomText = GenerateRandomText(Length);
            var hash = ComputeMd5Hash(randomText + GetSalt());
            this.Session["CaptchaHash"] = hash;

            var rnd = new Random();
            var fonts = new[] { "Verdana", "Times New Roman" };
            float orientationAngle = rnd.Next(0, 359);

            var index0 = rnd.Next(0, fonts.Length);
            var familyName = fonts[index0];

            using (var bmpOut = new Bitmap(Width, Height))
            {
                var g = Graphics.FromImage(bmpOut);
                var gradientBrush = new LinearGradientBrush(
                    new Rectangle(0, 0, Width, Height),
                    Color.White,
                    Color.DarkGray,
                    orientationAngle);
                g.FillRectangle(gradientBrush, 0, 0, Width, Height);
                DrawRandomLines(ref g, Width, Height);
                g.DrawString(randomText, new Font(familyName, 18), new SolidBrush(Color.Gray), 0, 2);
                var ms = new MemoryStream();
                bmpOut.Save(ms, ImageFormat.Png);
                var bmpBytes = ms.GetBuffer();
                bmpOut.Dispose();
                ms.Close();

                return new FileContentResult(bmpBytes, "image/png");
            }
        }

        /// <summary>
        /// Validates if the captcha is valid
        /// </summary>
        /// <param name="captchaValue">Value introduced by the user</param>
        /// <returns>JSON indicating if the captcha was valid or not</returns>
        public JsonResult ValidateCaptcha(string captchaValue)
        {
            bool b = IsValidCaptchaValue(captchaValue.ToUpper());
            if (!b)
            {
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Validates if the captcha is valid
        /// </summary>
        /// <param name="captchaValue">Value introduced by the user</param>
        /// <returns>JSON indicating if the captcha was valid or not</returns>
        public JsonResult ValidateInvisibleCaptcha(string captchaValue)
        {
            bool b = captchaValue == string.Empty;
            if (!b)
            {
                return this.Json(string.Empty, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Draws random lines on a graphic
        /// </summary>
        /// <param name="g">Graphic to draw the line on</param>
        /// <param name="width">Width of the graphic</param>
        /// <param name="height">Height of the graphic</param>
        private static void DrawRandomLines(ref Graphics g, int width, int height)
        {
            var rnd = new Random();
            var pen = new Pen(Color.Gray);
            for (var i = 0; i < 10; i++)
            {
                g.DrawLine(
                    pen,
                    rnd.Next(0, width),
                    rnd.Next(0, height),
                    rnd.Next(0, width),
                    rnd.Next(0, height));
            }
        }

        /// <summary>
        /// Gets the type of the name of the assemble of the controller
        /// </summary>
        /// <returns>Type of the name of the assemble of the controller</returns>
        private static string GetSalt()
        {
            return typeof(CaptchaController).Assembly.FullName;
        }

        /// <summary>
        /// Encodes a string into an MD5
        /// </summary>
        /// <param name="input">String to encode</param>
        /// <returns>Encoded string</returns>
        private static string ComputeMd5Hash(string input)
        {
            var encoding = new ASCIIEncoding();
            var bytes = encoding.GetBytes(input);
            HashAlgorithm md5Hasher = MD5.Create();
            return BitConverter.ToString(md5Hasher.ComputeHash(bytes));
        }

        /// <summary>
        /// Generates a random text
        /// </summary>
        /// <param name="textLength">Length of the text</param>
        /// <returns>Generated random text</returns>
        private static string GenerateRandomText(int textLength)
        {
            var random = new Random();
            var result = new string(Enumerable.Repeat(Chars, textLength)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
    }
}

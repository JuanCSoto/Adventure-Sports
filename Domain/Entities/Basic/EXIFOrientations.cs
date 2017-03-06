// -----------------------------------------------------------------------
// <copyright file="EXIFOrientations.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno</author>
// ----------------------------------------------------------------------
namespace Business.Entities
{
    /// <summary>
    /// Class defining the EXIF orientation property
    /// </summary>
    public class EXIFOrientations
    {
        /// <summary>
        /// ID of the property
        /// </summary>
        public const int OrientationID = 0x0112;

        /// <summary>
        /// Enumerator defining the different orientation values of the images
        /// </summary>
        public enum Orientations : byte
        {
            /// <summary>
            /// Unknown orientation
            /// </summary>
            Unknown = 0,

            /// <summary>
            /// Normal orientation
            /// </summary>
            TopLeft = 1,

            /// <summary>
            /// Normal mirrored orientation
            /// </summary>
            TopRight = 2,

            /// <summary>
            /// 180° Rotated orientation
            /// </summary>
            BottomRight = 3,

            /// <summary>
            /// 180° Rotated mirrored orientation
            /// </summary>
            BottomLeft = 4,

            /// <summary>
            /// 90° Rotated mirrored orientation
            /// </summary>
            LeftTop = 5,

            /// <summary>
            /// 270° Rotated orientation
            /// </summary>
            RightTop = 6,

            /// <summary>
            /// 270° Rotated mirrored orientation
            /// </summary>
            RightBottom = 7,

            /// <summary>
            /// 90° Rotated orientation
            /// </summary>
            LeftBottom = 8
        }
    }
}

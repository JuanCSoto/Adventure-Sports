// <copyright file="PaginInfo.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Domain.Entities
{
    using System;
    
    /// <summary>
    /// represent a basic information to build a pager
    /// </summary>
    public class PaginInfo
    {
        /// <summary>
        /// initial size to page
        /// </summary>
        private int size = 8;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginInfo"/> class
        /// </summary>
        public PaginInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginInfo"/> class
        /// </summary>
        /// <param name="totalcount">total fields</param>
        /// <param name="pageindex">a page index</param>
        public PaginInfo(int totalcount, int pageindex)
        {
            this.TotalCount = totalcount;
            this.PageIndex = pageindex;
        }

        /// <summary>
        /// Gets or sets a size to page
        /// </summary>
        public int Size
        {
            get { return this.size; }
            set { this.size = value; }
        }

        /// <summary>
        /// Gets a number of pages
        /// </summary>
        public int PagesCount
        {
            get
            {
                return ((int)Math.Ceiling((double)this.TotalCount / (double)this.size)) < 0 ? 1 : (int)Math.Ceiling((double)this.TotalCount / (double)this.size);
            }
        }

        /// <summary>
        /// Gets the actual position row
        /// </summary>
        public int ActualRow
        {
            get
            {
                return (this.size * this.PageIndex) - (this.size - 1);
            }
        }

        /// <summary>
        /// Gets or sets a total fields
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets a index page
        /// </summary>
        public int PageIndex { get; set; }
    }
}
// <copyright file="ConvertListExcel.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Web;
    
    /// <summary>
    /// generates a excel file
    /// </summary>
    /// <typeparam name="T">type of object</typeparam>
    public class ConvertListExcel<T>
    {
        /// <summary>
        /// HTTP context
        /// </summary>
        private HttpContextBase context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertListExcel{T}"/> class
        /// </summary>
        /// <param name="context">HTTP context</param>
        public ConvertListExcel(HttpContextBase context) 
        {
            this.context = context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertListExcel{T}"/> class
        /// </summary>
        /// <param name="collList">object list</param>
        /// <param name="context">HTTP context</param>
        public ConvertListExcel(List<T> collList, HttpContextBase context) 
        {
            this.CollList = collList;
            this.context = context;
        }

        /// <summary>
        /// Gets or sets a object list
        /// </summary>
        public List<T> CollList { get; set; }

        /// <summary>
        /// Gets or sets a color of cells
        /// </summary>
        public string CellColor { get; set; }

        /// <summary>
        /// Gets or sets a sheet name
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// Gets or sets a file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Export a list to excel file
        /// </summary>
        public void ConvertToExcel()
        {
            StringBuilder strbCells = new StringBuilder();
            string filebase = Path.Combine(this.context.Server.MapPath("~"), @"resources\templates\toexcel.xml");
            string baseExcel = Utils.GetContentFile(filebase);
            PropertyInfo[] col = typeof(T).GetProperties();

            strbCells.AppendLine("<ss:Row>");
            foreach (PropertyInfo prop in col)
            {
                strbCells.AppendLine("<ss:Cell ss:StyleID=\"s1\">");
                strbCells.AppendLine("<ss:Data ss:Type=\"String\">" + prop.Name + "</ss:Data>");
                strbCells.AppendLine("</ss:Cell>");
            }

            strbCells.AppendLine("</ss:Row>");

            foreach (T item in this.CollList)
            {
                strbCells.AppendLine("<ss:Row>");
                foreach (PropertyInfo prop in col)
                {
                    object val = prop.GetValue(item, null);
                    strbCells.AppendLine("<ss:Cell ss:StyleID=\"s2\">");
                    strbCells.AppendLine("<ss:Data ss:Type=\"String\">" + (val != null ? HttpUtility.HtmlEncode(val.ToString()) : string.Empty) + "</ss:Data>");
                    strbCells.AppendLine("</ss:Cell>");
                }

                strbCells.AppendLine("</ss:Row>");
            }

            string result = baseExcel.Replace("@CONTENT", strbCells.ToString())
                .Replace("@NAMESHEET", this.SheetName ?? "Hoja 1")
                .Replace("@CELLCOLOR", this.CellColor ?? "#99CCFF");

            this.context.Response.Clear();
            this.context.Response.Buffer = true;
            this.context.Response.AddHeader("content-disposition", "attachment;filename=" + (this.FileName != null ? this.FileName : DateTime.Now.ToString("ddMMyyyyhhmmss")) + ".xls");
            this.context.Response.Charset = "UTF-8";
            this.context.Response.ContentEncoding = System.Text.Encoding.Default;
            this.context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.context.Response.ContentType = "application/vnd.ms-excel";
            this.context.Response.Write(result);
            this.context.Response.End();
        }
    }
}

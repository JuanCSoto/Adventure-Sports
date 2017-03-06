// <copyright file="Paginator.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business
{
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using Domain.Entities;
    
    /// <summary>
    /// builds a <c>paginator</c> for object list
    /// </summary>
    public class Paginator
    {
        /// <summary>
        /// HTTP context
        /// </summary>
        private HttpContext context;

        /// <summary>
        /// object <c>PaginInfo</c>
        /// </summary>
        private PaginInfo pagininfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="Paginator"/> class
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <param name="paginInfo">object <c>PaginInfo</c></param>
        public Paginator(HttpContext context, PaginInfo paginInfo)
        {
            this.context = context;
            this.pagininfo = paginInfo;
        }

        /// <summary>
        /// render a <c>paginator</c>
        /// </summary>
        /// <returns>returns a string render</returns>
        public HtmlString GetPaginator()
        {
            if (this.pagininfo.TotalCount > this.pagininfo.Size)
            {
                string htmlContent = "<div id=\"dvpaging\">{0}{1}<br /><span>{2} P&aacute;gina(s) de {3}</span><br /><span>{4} registro(s)</span></div>";

                string strBack = this.pagininfo.PageIndex > 1 ? "<img onclick=\"window.location.href='" + this.GetPreviosLink(this.pagininfo.PageIndex - 1) + "'\" src=\"/resources/images/35back.gif\" />" : string.Empty;
                string strNext = this.pagininfo.PageIndex < this.pagininfo.PagesCount ? "<img onclick=\"window.location.href='" + this.GetPreviosLink(this.pagininfo.PageIndex + 1) + "'\" src=\"/resources/images/35next.gif\" />" : string.Empty;

                return new HtmlString(
                    string.Format(
                    htmlContent, 
                    strBack, 
                    strNext,
                    this.pagininfo.PageIndex,
                    this.pagininfo.PagesCount,
                    this.pagininfo.TotalCount));
            }
            else
            {
                return new HtmlString(string.Empty);
            }
        }

        /// <summary>
        /// builds a <c>URL</c> references
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <returns>returns a string <c>URL</c></returns>
        private string GetPreviosLink(int pageIndex)
        {
            string queryString;
            string rootPath;

            StringBuilder builder = new StringBuilder();
            Regex regEx = new Regex("[\\<>\"'()]");

            foreach (string key in this.context.Request.QueryString.Keys)
            {
                if (key == "page")
                {
                    continue;
                }

                foreach (var value in this.context.Request.QueryString.GetValues(key))
                {
                    var safeValue = value;
                    if (regEx.IsMatch(value))
                    {
                        safeValue = regEx.Replace(value, string.Empty);
                    }

                    builder.AppendFormat("&amp;{0}={1}", key, safeValue);
                }
            }

            queryString = builder.ToString();

            if (this.context.Request.RawUrl.Contains("?"))
            {
                rootPath = this.context.Request.RawUrl.Substring(0, this.context.Request.RawUrl.IndexOf("?"));
            }
            else
            {
                rootPath = this.context.Request.RawUrl;
            }

            return rootPath + "?page=" + pageIndex + queryString;
        }
    }
}

// <copyright file="Helpers.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Xml;
    using Domain.Entities;
    
    /// <summary>
    /// provides html helpers
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// render item from module menu
        /// </summary>
        /// <param name="helper">Represents support for rendering HTML controls in a view.</param>
        /// <param name="mod"><c>Modul</c> object</param>
        /// <param name="modulId">Identifier of <c>Modul</c></param>
        /// <param name="collModul">List of object <c>Modul</c></param>
        /// <param name="modul">name of the module</param>
        /// <returns>returns the string for render</returns>
        public static HtmlString GetItemModul(this HtmlHelper helper, Domain.Entities.Modul mod, int? modulId, IEnumerable<Domain.Entities.Modul> collModul, string modul)
        {
            UrlHelper url = new UrlHelper(helper.ViewContext.RequestContext);

            IEnumerable<Domain.Entities.Modul> col = collModul.Where(t => t.ParentId == mod.ModulId);

            if (col.Count() > 0)
            {
                return new HtmlString("class='mod" + mod.ModulId + (modulId != null && mod.ModulId == modulId ? " liactive" : string.Empty) + "' data-num=" + col.Count() + " onclick='fade(" + Json.Encode(col) + ", " + modul + ", this)'");
            }
            else
            {
                if (!mod.IsContent.Value)
                {
                    return new HtmlString("class='mod" + mod.ModulId + (modulId != null && mod.ModulId == modulId ? " liactive" : string.Empty) + "' onclick=window.location.href='" + url.Action("Index", mod.Controller, new { Area = "Admin" }) + "'");
                }
                else
                {
                    return new HtmlString("class='mod" + mod.ModulId + (modulId != null && mod.ModulId == modulId ? " liactive" : string.Empty) + "' onclick=window.location.href='" + url.Action("Index", "Content", new { Area = "Admin", mod = mod.ModulId }) + "'");
                }
            }
        }

        /// <summary>
        /// Gets the URL of the entity
        /// </summary>
        /// <param name="helper">Represents support for rendering HTML controls in a view.</param>
        /// <param name="language">object language</param>
        /// <param name="key">path of url</param>
        /// <returns>returns the path of entity</returns>
        public static string GetUrl(this HtmlHelper helper, Language language, string key)
        {
            return VirtualPathUtility.ToAbsolute("~/" + (!language.IsDefault.Value ? language.Culturename + "/" : string.Empty) + key);
        }

        /// <summary>
        /// Gets the URL image content
        /// </summary>
        /// <param name="helper">Represents support for rendering HTML controls in a view.</param>
        /// <param name="image">name of the image</param>
        /// <param name="contentId">identifier of content</param>
        /// <param name="w">width of image</param>
        /// <param name="h">height of image</param>
        /// <returns>returns the path of the image</returns>
        public static string GetImageContent(this HtmlHelper helper, string image, int contentId, int? w, int? h)
        {
            if (!string.IsNullOrEmpty(image))
            {
                if (w != null && h != null)
                {
                    string serverpath = HttpContext.Current.Server.MapPath("~");
                    string prefix = h + "X" + w;

                    if (!File.Exists(Path.Combine(serverpath, @"files\" + contentId + @"\" + prefix + image)))
                    {
                        ImageResize objimg = new ImageResize(serverpath)
                        {
                            Height = h.Value,
                            Width = w.Value,
                            Prefix = prefix
                        };

                        objimg.Resize(@"files\" + contentId + @"\" + image, ImageResize.TypeResize.PartialProportional);
                    }

                    return VirtualPathUtility.ToAbsolute("~/files/" + contentId + "/" + prefix + image);
                }
                else
                {
                    return VirtualPathUtility.ToAbsolute("~/files/" + contentId + "/" + image);
                }
            }
            else
            {
                return VirtualPathUtility.ToAbsolute("~/Resources/Images/Default.jpg");
            }
        }

        /// <summary>
        /// Gets the information from the xml node
        /// </summary>
        /// <param name="helper">Represents support for rendering HTML controls in a view.</param>
        /// <param name="xmlDocument"><c>XmlDocument</c> object</param>
        /// <param name="key">key of the search item</param>
        /// <returns>returns the string from value of xml node</returns>
        public static string GetInfoNode(this HtmlHelper helper, XmlDocument xmlDocument, string key)
        {
            if (xmlDocument != null)
            {
                XmlNode node = xmlDocument.SelectSingleNode("/content/node[@id='" + key + "']");
                if (null != node)
                {
                    return node.InnerText;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}

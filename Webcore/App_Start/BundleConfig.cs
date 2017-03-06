// <copyright file="BundleConfig.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore
{
    using System.Web;
    using System.Web.Optimization;

    /// <summary>
    /// set the Bundling information
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// register the Bundling information
        /// </summary>
        /// <param name="bundles">bundles list</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Resources/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquerycl").Include(
                "~/Resources/Scripts/jquery.cleditor*",
                        "~/Resources/Scripts/jquery.cleditorext.extimages.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Resources/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Resources/Scripts/jquery.admin.utils*",
                        "~/Resources/Scripts/jquery.unobtrusive*",
                        "~/Resources/Scripts/jquery.validate*"));

            bundles.Add(new StyleBundle("~/resources/css/").Include(
                        "~/Resources/Css/admin.styles.css",
                        "~/Resources/Css/jquery.cleditor.css",
                        "~/Resources/Css/jquery-ui-{version}.css"));

            bundles.Add(new StyleBundle("~/Content/frontend").Include(
                        "~/Resources/Css/frontend.styles.css"));
        }
    }
}
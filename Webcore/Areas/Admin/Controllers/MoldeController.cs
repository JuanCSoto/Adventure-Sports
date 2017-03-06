// <copyright file="MoldeController.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml;
    using Business.Services;
    using Domain.Concrete;
    using Domain.Entities;
    using Webcore.Areas.Admin.Models;
    
    /// <summary>
    /// Controller for mold module
    /// </summary>
    [ModulAuthorize]
    public class MoldeController : AdminController
    {
        /// <summary>
        /// gets the home of mold module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            MoldRepository objMold = new MoldRepository(SessionCustom);

            return this.View(new Molde()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                CollMold = objMold.GetAll(),
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage
            });
        }

        /// <summary>
        /// obtains a detail of mold
        /// </summary>
        /// <param name="id">identifier of mold</param>
        /// <returns>returns the result to action</returns>
        public ActionResult Detail(int? id)
        {
            MoldRepository objMold = new MoldRepository(SessionCustom);
            Mold mold = null;
            XmlDocument xmlDoc = null;

            if (id != null)
            {
                objMold.Entity.MoldId = id;
                objMold.Load();
                mold = objMold.Entity;
                ViewBag.id = id;
                xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(objMold.Entity.Xmlcontent);
            }

            return this.View(new Molde()
            {
                UserPrincipal = CustomUser,
                Module = this.Module,
                Mold = mold,
                ColModul = CustomMemberShipProvider.GetModuls(CustomUser.UserId, SessionCustom, HttpContext),
                CurrentLanguage = CurrentLanguage,
                Xmldocument = xmlDoc
            });
        }

        /// <summary>
        /// inserts or updates a mold module
        /// </summary>
        /// <param name="id">identifier of mold</param>
        /// <param name="model">information to mold</param>
        /// <param name="values">form values</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult Detail(int? id, Molde model, FormCollection values)
        {
            MoldRepository objMold = new MoldRepository(SessionCustom);
            objMold.Entity = model.Mold;
            objMold.Entity.Xmlcontent = this.GetXmlDocument(values).InnerXml;

            if (id != null)
            {
                objMold.Entity.MoldId = id;
                objMold.Update();
                this.InsertAudit("Update", this.Module.Name + " -> " + objMold.Entity.Name);
            }
            else
            {
                objMold.Insert();
                this.InsertAudit("Insert", this.Module.Name + " -> " + objMold.Entity.Name);
            }

            return this.RedirectToAction("Index");
        }

        /// <summary>
        /// build a <c>XML</c> document from form values
        /// </summary>
        /// <param name="values">form values</param>
        /// <returns>returns the result to action</returns>
        private XmlDocument GetXmlDocument(FormCollection values)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            xmlDoc.AppendChild(docNode);
            XmlNode firstnode = xmlDoc.CreateElement("content");
            xmlDoc.AppendChild(firstnode);

            foreach (KeyValuePair<string, string> item in values.AllKeys
                .Where(k => k.StartsWith("_hf_"))
                .ToDictionary(k => k, k => values[k]))
            {
                XmlNode secundarynode = xmlDoc.CreateElement("node");
                secundarynode.Attributes.Append(this.GetAttribute("control", item.Value.Split('-')[0], xmlDoc));
                secundarynode.Attributes.Append(this.GetAttribute("type", item.Value.Split('-')[1], xmlDoc));
                secundarynode.Attributes.Append(this.GetAttribute("id", item.Value.Split('-')[2], xmlDoc));
                firstnode.AppendChild(secundarynode);
            }

            return xmlDoc;
        }

        /// <summary>
        /// get a <c>XmlAttribute</c> object according to parameters
        /// </summary>
        /// <param name="name">name of node</param>
        /// <param name="value">value of node</param>
        /// <param name="xmlDoc"><c>XmlDocument</c> container</param>
        /// <returns>returns the result to action</returns>
        private XmlAttribute GetAttribute(string name, string value, XmlDocument xmlDoc)
        {
            XmlAttribute xmlAttr = xmlDoc.CreateAttribute(name);
            xmlAttr.Value = value;
            return xmlAttr;
        }
    }
}

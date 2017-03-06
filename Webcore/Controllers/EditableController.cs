// <copyright file="EditableController.cs" company="Dasigno">
//  Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>

namespace Webcore.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Business;
    using Business.Services;
    using Domain.Concrete;

    /// <summary>
    /// controller for the editable front end action
    /// </summary>
    public class EditableController : FrontEndController
    {
        /// <summary>
        /// gets a form of front end editable values
        /// </summary>
        /// <param name="view">the view to return</param>
        /// <returns>returns the result to action</returns>
        [Authorize]
        public ActionResult Index(string view)
        {
            SetLabel();
            if (((CustomPrincipal)User).IsFrontEndAdmin)
            {
                return this.View(view);
            }

            return null;
        }

        /// <summary>
        /// Receive a form of front end editable values
        /// </summary>
        /// <param name="json">A JSON string with all the editable values</param>
        /// <returns>A JSON object indicating if the process was successful or not</returns>
        public JsonResult SaveChanges(string json)
        {
            bool result = false;

            if (((CustomPrincipal)User).IsFrontEndAdmin)
            {
                if (!string.IsNullOrEmpty(json))
                {
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    List<Domain.Entities.Basic.ForntEndEditable> editables = serializer.Deserialize<List<Domain.Entities.Basic.ForntEndEditable>>(json);

                    FrontEndEditableRepository repository = new FrontEndEditableRepository(SessionCustom);
                    foreach (Domain.Entities.Basic.ForntEndEditable editable in editables)
                    {
                        repository.Entity.EditableId = editable.TypeID;

                        if (editable.ValueType == "image")
                        {
                            repository.LoadByKey();
                            editable.Value = System.IO.Path.GetFileName(editable.Value);
                            if (System.IO.Path.GetFileName(repository.Entity.CurrentValue) != editable.Value)
                            {
                                string serverMap = Server.MapPath("~");
                                string origin = serverMap + @"\resources\temporal\" + editable.Value;
                                string destination = serverMap + @"\files\systemimages\" + editable.Value;
                                if (System.IO.File.Exists(origin))
                                {
                                    if (!System.IO.Directory.Exists(serverMap + @"\files\systemimages\"))
                                    {
                                        System.IO.Directory.CreateDirectory(serverMap + @"\files\systemimages\");
                                    }

                                    System.IO.File.Move(origin, serverMap + @"\files\systemimages\" + editable.Value);

                                    editable.Value = "/files/systemimages/" + editable.Value;
                                }
                                else if (System.IO.File.Exists(destination))
                                {
                                    editable.Value = repository.Entity.CurrentValue;
                                }
                                else
                                {
                                    editable.Value = string.Empty;
                                }

                                repository.Entity.CurrentValue = editable.Value;
                                repository.Update();
                            }
                        }
                        else
                        {
                            repository.Entity.CurrentValue = editable.Value;
                            repository.Update();
                        }
                    }

                    result = true;
                }
            }

            return this.Json(new { result = result });
        }

        /// <summary>
        /// show a form to upload an editable value
        /// </summary>
        /// <param name="id">editable id</param>
        /// <returns>returns the result to action</returns>
        public ActionResult UploadImage(int? id)
        {
            if (((CustomPrincipal)User).IsFrontEndAdmin)
            {
                ViewBag.EditableID = id;
                return this.View();
            }

            return null;
        }

        /// <summary>
        /// receive a form to upload an editable value
        /// </summary>
        /// <param name="editableFile">image file</param>
        /// <param name="id">editable id</param>
        /// <returns>returns the result to action</returns>
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase editableFile, int? id)
        {
            if (((CustomPrincipal)User).IsFrontEndAdmin)
            {
                ViewBag.EditableID = id;
                string serverMap = Server.MapPath("~");
                string fileName = Business.Utils.UploadFile(editableFile, serverMap, @"resources\temporal\", string.Empty);

                return this.View((object)fileName);
            }

            return null;
        }

        /// <summary>
        /// Set lenguage label
        /// </summary>
        private void SetLabel()
        {
            LabelManagement objlabel = new LabelManagement(SessionCustom, HttpContext);
            ViewBag.TXTTEXNEW = objlabel.GetLabelByName("TXTTEXNEW", CurrentLanguage.LanguageId.Value);
            ViewBag.SINGULAR = objlabel.GetLabelByName("SINGULAR", CurrentLanguage.LanguageId.Value);
            ViewBag.PLURAL = objlabel.GetLabelByName("PLURAL", CurrentLanguage.LanguageId.Value);
            ViewBag.SAVE = objlabel.GetLabelByName("SAVE", CurrentLanguage.LanguageId.Value);
            ViewBag.SINGULAR = objlabel.GetLabelByName("SINGULAR", CurrentLanguage.LanguageId.Value);
            ViewBag.PLURAL = objlabel.GetLabelByName("PLURAL", CurrentLanguage.LanguageId.Value);
            ViewBag.DESCRIPTION = objlabel.GetLabelByName("DESCRIPTION", CurrentLanguage.LanguageId.Value);
            ViewBag.RETOS = objlabel.GetLabelByName("RETOS", CurrentLanguage.LanguageId.Value);
            ViewBag.DESCRIPTION = objlabel.GetLabelByName("DESCRIPTION", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTITPART = objlabel.GetLabelByName("TXTTITPART", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTPIETEXT = objlabel.GetLabelByName("TXTPIETEXT", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTEXTPOLI = objlabel.GetLabelByName("TXTTEXTPOLI", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTCONTACT = objlabel.GetLabelByName("TXTCONTACT", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTLOGLEAD = objlabel.GetLabelByName("TXTLOGLEAD", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTSELIMG = objlabel.GetLabelByName("TXTSELIMG", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTEXTLEADS = objlabel.GetLabelByName("TXTTEXTLEADS", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTLINKLEAD = objlabel.GetLabelByName("TXTLINKLEAD", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTLOGOSPON = objlabel.GetLabelByName("TXTLOGOSPON", CurrentLanguage.LanguageId.Value);
            ViewBag.TEXT = objlabel.GetLabelByName("TEXT", CurrentLanguage.LanguageId.Value);
            ViewBag.GENERAL = objlabel.GetLabelByName("GENERAL", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTBACKCOLOR = objlabel.GetLabelByName("TXTBACKCOLOR", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTBTNJOIN = objlabel.GetLabelByName("TXTBTNJOIN", CurrentLanguage.LanguageId.Value);
            ViewBag.HEADBOARD = objlabel.GetLabelByName("HEADBOARD", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTCOLORHEAD = objlabel.GetLabelByName("TXTCOLORHEAD", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTESTPROG = objlabel.GetLabelByName("TXTTESTPROG", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTEXTIDEAS = objlabel.GetLabelByName("TXTTEXTIDEAS", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTTEXTFOLL = objlabel.GetLabelByName("TXTTEXTFOLL", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTEDITLOGO = objlabel.GetLabelByName("TXTEDITLOGO", CurrentLanguage.LanguageId.Value);
            ViewBag.TXTEDITMEN = objlabel.GetLabelByName("TXTEDITMEN", CurrentLanguage.LanguageId.Value);
            ViewBag.HOME = objlabel.GetLabelByName("HOME", CurrentLanguage.LanguageId.Value);
            ViewBag.USERS = objlabel.GetLabelByName("USERS", CurrentLanguage.LanguageId.Value);
            ViewBag.BLOQ = objlabel.GetLabelByName("BLOQ", CurrentLanguage.LanguageId.Value);
            ViewBag.STATISTICS = objlabel.GetLabelByName("STATISTICS", CurrentLanguage.LanguageId.Value);
            ViewBag.ALLIES = objlabel.GetLabelByName("ALLIES", CurrentLanguage.LanguageId.Value);
            ViewBag.QUESTIONS = objlabel.GetLabelByName("QUESTIONS", CurrentLanguage.LanguageId.Value);
            
        }
    }
}

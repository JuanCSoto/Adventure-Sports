// <copyright file="SectionManagement.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using Domain.Abstract;
    using Domain.Concrete;
    using Domain.Entities;
    
    /// <summary>
    /// Represents a management of section
    /// </summary>
    public class SectionManagement
    {
        /// <summary>
        /// Represents a mutable string of characters
        /// </summary>
        private StringBuilder strb = new StringBuilder();

        /// <summary>
        /// Represents a list of sections
        /// </summary>
        private List<Section> sections;

        /// <summary>
        /// framework that establishes communication between the application and the database
        /// </summary>
        private ISession session;

        /// <summary>
        /// HTTP context
        /// </summary>
        private HttpContextBase context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectionManagement"/> class
        /// </summary>
        /// <param name="session">framework that establishes communication between the application and the database</param>
        /// <param name="context">HTTP context</param>
        public SectionManagement(ISession session, HttpContextBase context)
        {
            this.session = session;
            this.context = context;
        }

        /// <summary>
        /// Gets or sets a tree of sections
        /// </summary>
        public string Tree { get; set; }

        /// <summary>
        /// obtains a list of section with parent null
        /// </summary>
        /// <param name="languageId">identifier language</param>
        /// <returns>returns a list of sections</returns>
        public IEnumerable<Section> GetSectionsParentNull(int languageId)
        {
            try
            {
                SectionRepository objsection = new SectionRepository(this.session);
                objsection.Entity.LanguageId = languageId;
                return objsection.GetAll().Where(t => t.ParentId == null);
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session, 
                    "Load Sections", 
                    ex.Message + " - " + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// obtains a section according the identifier
        /// </summary>
        /// <param name="sectionId">identifier section</param>
        /// <returns>returns a object section</returns>
        public Section GetSection(int sectionId)
        {
            try
            {
                SectionRepository objsection = new SectionRepository(this.session);
                objsection.Entity.SectionId = sectionId;
                objsection.Load();

                objsection.Entity.MaxValue = objsection.GetMaxOrder();

                return objsection.Entity;
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session, 
                    "Load Section", 
                    ex.Message + " - " + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// inserts or updates a section object
        /// </summary>
        /// <param name="objSection">object section</param>
        /// <returns>returns true if operation successful</returns>
        public bool SaveSection(Section objSection)
        {
            try
            {
                SectionRepository sectionRepository = new SectionRepository(this.session);

                string section = objSection.Name;
                
                sectionRepository.Entity = objSection;
                if (sectionRepository.Entity.SectionId != null)
                {
                    if (null != objSection.OldOrder && objSection.Sectionorder != null && objSection.Sectionorder != objSection.OldOrder)
                    {
                        sectionRepository.ChangeOrder(sectionRepository.Entity.Sectionorder.Value, objSection.OldOrder.Value);
                    }

                    sectionRepository.Update();

                    FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);
                    friendlyrepo.Entity.Id = sectionRepository.Entity.SectionId;
                    friendlyrepo.Entity.Friendlyurlid = sectionRepository.Entity.Friendlyname;
                    friendlyrepo.Entity.Type = Friendlyurl.FriendlyType.Section;
                    friendlyrepo.Update();

                    InfoCache<List<Section>> cache = new InfoCache<List<Section>>(this.context) { TimeOut = 120 };
                    sectionRepository.Entity = new Section();
                    cache.SetCache("sections", sectionRepository.GetAll());
                    
                    Utils.InsertAudit(
                        this.session, 
                        new Audit()
                        {
                            Auditaction = "Update",
                            Description = "Section -> " + section,
                            Joindate = DateTime.Now,
                            Username = (this.context.User as CustomPrincipal).UserId
                        });
                }
                else
                {
                    sectionRepository.Entity.Sectionorder = sectionRepository.GetMaxOrder();
                    sectionRepository.Entity.Friendlyname = Utils.GetFindFrienlyName(
                        this.session, 
                        sectionRepository.Entity.Name, 
                        sectionRepository.Entity.Sectionorder.Value);
                    sectionRepository.Entity.SectionId = Convert.ToInt32(sectionRepository.Insert());

                    FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);
                    friendlyrepo.Entity.Id = sectionRepository.Entity.SectionId;
                    friendlyrepo.Entity.Friendlyurlid = sectionRepository.Entity.Friendlyname;
                    friendlyrepo.Entity.Type = Friendlyurl.FriendlyType.Section;
                    friendlyrepo.Entity.LanguageId = sectionRepository.Entity.LanguageId;
                    friendlyrepo.Insert();

                    Utils.InsertAudit(
                        this.session, 
                        new Audit()
                        {
                            Auditaction = "Insert",
                            Description = "Section -> " + section,
                            Joindate = DateTime.Now,
                            Username = (this.context.User as CustomPrincipal).UserId
                        });
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session, 
                    "Insert Section", 
                    ex.Message + " - " + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// delete a section
        /// </summary>
        /// <param name="sectionId">identifier of section</param>
        /// <returns>returns true if operation successful</returns>
        public bool DeleteSection(int sectionId)
        {
            try
            {
                FriendlyurlRepository friendlyrepo = new FriendlyurlRepository(this.session);
                SectionRepository objsection = new SectionRepository(this.session);
                objsection.Entity.SectionId = sectionId;
                objsection.Delete();

                friendlyrepo.Entity.Id = sectionId;
                friendlyrepo.Entity.Type = Friendlyurl.FriendlyType.Section;
                friendlyrepo.Delete();

                Utils.InsertAudit(
                    this.session, 
                    new Audit()
                    {
                        Auditaction = "Delete",
                        Description = "Section -> " + sectionId.ToString(),
                        Joindate = DateTime.Now,
                        Username = (this.context.User as CustomPrincipal).UserId
                    });

                return true;
            }
            catch (Exception ex)
            {
                Utils.InsertLog(
                    this.session, 
                    "Delete Section",
                    ex.Message + " - " + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// create a tree sections
        /// </summary>
        /// <param name="sectionId">identifier section</param>
        /// <param name="collSection">list of section</param>
        public void CreateTreeView(int? sectionId, List<Section> collSection)
        {
            this.sections = collSection;
            this.BindSection(sectionId, null);
        }

        /// <summary>
        /// obtains a list of layouts
        /// </summary>
        /// <returns>returns a list of strings</returns>
        public IEnumerable<string> GetLayouts()
        {
            string directoryPath = Path.Combine(
                this.context.Server.MapPath("~"),
                @"Views\Layouts\");

            string[] filesnames = Directory.GetFiles(directoryPath, "*.cshtml");
            foreach (string item in filesnames)
            {
                yield return Path.GetFileNameWithoutExtension(item);
            }
        }

        /// <summary>
        /// obtains a list of templates
        /// </summary>
        /// <returns>returns a list of strings</returns>
        public IEnumerable<string> GetTemplates()
        {
            TemplateRepository template = new TemplateRepository(this.session);
            template.Entity.Type = 1;
            return template.GetAll().Select(t => t.TemplateId);
        }

        /// <summary>
        /// create a tree check sections
        /// </summary>
        /// <param name="collSection">list of sections</param>
        /// <param name="listContent">list of <c>Bannersection</c> object</param>
        public void CreateTreeViewCheck(List<Section> collSection, List<Bannersection> listContent)
        {
            this.sections = collSection;
            List<Section> parents = this.sections.FindAll(t => t.ParentId == null);
            this.strb.AppendLine("<ul>");
            bool isFind = listContent != null;

            foreach (Section item in parents)
            {
                string check = isFind ? (listContent.Exists(t => t.SectionId == item.SectionId) ? "checked='checked'" : string.Empty) : string.Empty;
                this.strb.AppendLine("<li><nobr><input value='" + item.SectionId + "'" + check + " id='" + item.SectionId + "' type='checkbox' /><label for='" + item.SectionId + "'>" + item.Name + "</label></nobr>");
                this.Recursive(item.SectionId.Value, listContent);
                this.strb.AppendLine("</li>");
            }

            this.strb.AppendLine("</ul>");

            this.Tree = this.strb.ToString();
        }

        /// <summary>
        /// recursive function
        /// </summary>
        /// <param name="parentId">parent identifier</param>
        /// <param name="listContent">list of sections</param>
        private void Recursive(int parentId, List<Bannersection> listContent)
        {
            List<Section> parents = this.sections.FindAll(t => t.ParentId == parentId);
            bool isFind = listContent != null;

            if (parents.Count() > 0)
            {
                this.strb.AppendLine("<ul>");
                foreach (Section item in parents)
                {
                    string check = isFind ? (listContent.Exists(t => t.SectionId == item.SectionId) ? "checked='checked'" : string.Empty) : string.Empty;
                    this.strb.AppendLine("<li><nobr><input value='" + item.SectionId + "' " + check + " id='" + item.SectionId + "' type='checkbox' /><label for='" + item.SectionId + "'>" + item.Name + "</label></nobr>");
                    this.Recursive(item.SectionId.Value, listContent);
                    this.strb.AppendLine("</li>");
                }

                this.strb.AppendLine("</ul>");
            }
        }

        /// <summary>
        /// fills a section information
        /// </summary>
        /// <param name="id">identifier of section</param>
        /// <param name="node">name of section</param>
        private void BindSection(int? id, string node)
        {
            StringBuilder strbind = new StringBuilder();
            Section sec = this.sections.Find(t => t.SectionId == id);

            if (sec != null && sec.ParentId != null)
            {
                List<Section> collSections = this.sections.FindAll(t => t.ParentId == sec.ParentId);
                if (collSections.Count > 0)
                {
                    strbind.AppendLine("<ul>");

                    foreach (Section cursor in collSections)
                    {
                        if (id != cursor.SectionId)
                        {
                            strbind.AppendLine("<li><div id='" + cursor.SectionId + "'><nobr><img onclick=\"expand(this, " + cursor.SectionId + ")\" height=\"15px\" width=\"15px\" src=\"/resources/images/25add.gif\" />");
                            strbind.AppendLine("<span onclick=\"ctnback.binddetail(" + cursor.SectionId + ")\">" + cursor.Name + "</span></nobr></div></li>");
                        }
                        else
                        {
                            strbind.AppendLine("<li><div id='" + cursor.SectionId + "'><nobr><img onclick=\"expand(this, " + cursor.SectionId + ")\" height=\"15px\" width=\"15px\" src=\"/resources/images/25add.gif\" />");
                            strbind.AppendLine("<span onclick=\"ctnback.binddetail(" + cursor.SectionId + ")\">" + cursor.Name + "</span></nobr></div>" + node + "</li>");
                        }
                    }

                    strbind.AppendLine("</ul>");
                }

                this.BindSection(sec.ParentId.Value, strbind.ToString());
            }
            else
            {
                List<Section> collSections = this.sections.FindAll(t => t.ParentId == null);
                strbind.AppendLine("<span class=\"spntitle\">Secciones</span><ul><li><div id=\"0\"><img height=\"15px\" width=\"15px\" src=\"/resources/images/15add.gif\" />");
                strbind.AppendLine("<span onclick=\"$('#conttree').find('.activ').removeClass('activ');$('#0').find('span').addClass('activ');$('#SectionId').val(null);ctnback.newsection(true)\">Raiz</span></div>");

                strbind.Append("<ul>");

                foreach (Section cursor in collSections)
                {
                    if (id != cursor.SectionId)
                    {
                        strbind.AppendLine("<li><div id='" + cursor.SectionId + "'><nobr><img onclick=\"expand(this, " + cursor.SectionId + ")\" height=\"15px\" width=\"15px\" src=\"/resources/images/25add.gif\" />");
                        strbind.AppendLine("<span onclick=\"ctnback.binddetail(" + cursor.SectionId + ")\">" + cursor.Name + "</span></nobr></div></li>");
                    }
                    else
                    {
                        strbind.AppendLine("<li><div id='" + cursor.SectionId + "'><nobr><img onclick=\"expand(this, " + cursor.SectionId + ")\" height=\"15px\" width=\"15px\" src=\"/resources/images/25add.gif\" />");
                        strbind.AppendLine("<span onclick=\"ctnback.binddetail(" + cursor.SectionId + ")\">" + cursor.Name + "</span></nobr></div>" + node + "</li>");
                    }
                }

                strbind.Append("</ul></li></ul>");

                this.Tree = strbind.ToString();
            }
        }
    }
}

// <copyright file="ReportController.cs" company="Dasigno">
//     Copyright (c) 2015 All Rights Reserved
// </copyright>
// <author>Dasigno SAS</author>
namespace Webcore.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;
    using Business;
    using Business.Services;
    using Domain.Concrete;
    using OfficeOpenXml;
    using Webcore.Areas.Admin.Models;

    /// <summary>
    /// Controller for Ally module
    /// </summary>
    [ModulAuthorize]
    public class ReportController : AdminController
    {
        /// <summary>
        /// gets the home of report module
        /// </summary>
        /// <returns>returns the result to action</returns>
        public ActionResult Index()
        {
            ContentManagement objcontentman = new ContentManagement(this.SessionCustom, HttpContext);
            SectionRepository objsection = new SectionRepository(this.SessionCustom);
            TemplateRepository objtemplate = new TemplateRepository(this.SessionCustom);
            objtemplate.Entity.Type = 0;

            ContentRepository content = new ContentRepository(SessionCustom);
            DataTable pulsesTable = content.ReportPulses();
            List<SelectListItem> pulses = new List<SelectListItem>();
            foreach (DataRow dr in pulsesTable.Rows)
            {
                pulses.Add(new SelectListItem() { Value = dr["ContentId"].ToString(), Text = dr["Nombre"].ToString() });
            }

            pulsesTable.Dispose();
            ViewBag.Pulses = pulses;

            return this.View(new ReportModel()
            {
                UserPrincipal = this.CustomUser,
                Module = this.Module,
                ColModul = CustomMemberShipProvider.GetModuls(this.CustomUser.UserId, this.SessionCustom, HttpContext),
                Templates = objtemplate.GetAll().Select(t => t.TemplateId),
                
                CurrentLanguage = this.CurrentLanguage
            });
        }
         
        /// <summary>
        /// return an excel file with the pulses report
        /// </summary>
        public void PulsesReport()
        {
            ContentRepository content = new ContentRepository(SessionCustom);
            DataTable pulses = content.ReportPulses();
            pulses.Columns.Remove("ContentId");

            using (ExcelPackage pck = new ExcelPackage())
            {
                string fileName = string.Concat("pulsos ", DateTime.Now.ToString("yyyyMMdd-HHmmss"));

                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Pulsos");
                worksheet.Cells.Style.Font.SetFromFont(new Font("Arial", 10));

                worksheet.Cells[1, 1, 1, 11].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Value = string.Concat("LISTADO DE PULSOS ", DateTime.Now.ToString("yyyy-MM-dd"));

                worksheet.Cells[3, 1, 3, pulses.Columns.Count].Style.Font.Bold = true;
                worksheet.Cells[3, 1].LoadFromDataTable(pulses, true);

                worksheet.Cells[4, 4, 4 + pulses.Rows.Count, 4].Style.Numberformat.Format = "yyyy-MM-dd";
                worksheet.Cells[4, 5, 4 + pulses.Rows.Count, 5].Style.Numberformat.Format = "yyyy-MM-dd";

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Concat("attachment;  filename=", fileName, ".xls"));
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        /// <summary>
        /// return an excel file with the users report
        /// </summary>
        public void UsersReport()
        {
            UserRepository user = new UserRepository(SessionCustom);
            DataTable users = user.ReportUsers();

            using (ExcelPackage pck = new ExcelPackage())
            {
                string fileName = string.Concat("usuarios ", DateTime.Now.ToString("yyyyMMdd-HHmmss"));

                ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Usuarios");
                worksheet.Cells.Style.Font.SetFromFont(new Font("Arial", 10));

                worksheet.Cells[1, 1, 1, 16].Merge = true;
                worksheet.Cells[1, 1].Style.Font.Bold = true;
                worksheet.Cells[1, 1].Value = string.Concat("LISTADO DE Usuarios ", DateTime.Now.ToString("yyyy-MM-dd"));

                worksheet.Cells[3, 1, 3, users.Columns.Count].Style.Font.Bold = true;
                worksheet.Cells[3, 1].LoadFromDataTable(users, true);

                worksheet.Cells[4, 3, 4 + users.Rows.Count, 3].Style.Numberformat.Format = "yyyy-MM-dd";                

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", string.Concat("attachment;  filename=", fileName, ".xls"));
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        /// <summary>
        /// return an excel file with a pulse report
        /// </summary>
        /// <param name="id">pulse id</param>
        public void PulseReportDetail(string id)
        {
            ContentRepository content = new ContentRepository(SessionCustom);
            DataSet pulse = content.PulseReportDetail(id);

            if (pulse.Tables.Count == 4)
            {
                using (ExcelPackage pck = new ExcelPackage())
                {
                    string fileName = string.Concat("detalle pulso ", DateTime.Now.ToString("yyyyMMdd-HHmmss"));

                    ExcelWorksheet worksheet = pck.Workbook.Worksheets.Add("Detalle Pulso");
                    worksheet.Cells.Style.Font.SetFromFont(new Font("Arial", 10));

                    worksheet.Cells[1, 1, 1, 11].Merge = true;
                    worksheet.Cells[1, 1].Style.Font.Bold = true;
                    worksheet.Cells[1, 1].Value = string.Concat("DETALLE DEL PULSO ", DateTime.Now.ToString("yyyy-MM-dd"));

                    worksheet.Cells[3, 1, 3, pulse.Tables[0].Columns.Count].Style.Font.Bold = true;
                    worksheet.Cells[3, 1].LoadFromDataTable(pulse.Tables[0], true);

                    worksheet.Cells[4, 4, 4 + pulse.Tables[0].Rows.Count, 4].Style.Numberformat.Format = "yyyy-MM-dd";
                    worksheet.Cells[4, 5, 4 + pulse.Tables[0].Rows.Count, 5].Style.Numberformat.Format = "yyyy-MM-dd";

                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    ExcelWorksheet worksheet2 = pck.Workbook.Worksheets.Add("Ideas Pulso");
                    worksheet2.Cells.Style.Font.SetFromFont(new Font("Arial", 10));

                    worksheet2.Cells[1, 1, 1, 11].Merge = true;
                    worksheet2.Cells[1, 1].Style.Font.Bold = true;
                    worksheet2.Cells[1, 1].Value = string.Concat("IDEAS DEL PULSO ", DateTime.Now.ToString("yyyy-MM-dd"));

                    worksheet2.Cells[3, 1, 3, pulse.Tables[1].Columns.Count].Style.Font.Bold = true;
                    worksheet2.Cells[3, 1].LoadFromDataTable(pulse.Tables[1], true);
                    
                    worksheet2.Cells[4, 5, 4 + pulse.Tables[1].Rows.Count, 5].Style.Numberformat.Format = "yyyy-MM-dd";

                    worksheet2.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    ExcelWorksheet worksheet3 = pck.Workbook.Worksheets.Add("Comentarios Idea");
                    worksheet3.Cells.Style.Font.SetFromFont(new Font("Arial", 10));

                    worksheet3.Cells[1, 1, 1, 11].Merge = true;
                    worksheet3.Cells[1, 1].Style.Font.Bold = true;
                    worksheet3.Cells[1, 1].Value = string.Concat("COMENTARIOS DE LAS IDEAS ", DateTime.Now.ToString("yyyy-MM-dd"));

                    worksheet3.Cells[3, 1, 3, pulse.Tables[2].Columns.Count].Style.Font.Bold = true;
                    worksheet3.Cells[3, 1].LoadFromDataTable(pulse.Tables[2], true);

                    worksheet3.Cells[4, 5, 4 + pulse.Tables[2].Rows.Count, 5].Style.Numberformat.Format = "yyyy-MM-dd";

                    worksheet3.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    ExcelWorksheet worksheet4 = pck.Workbook.Worksheets.Add("Votos Usuarios");
                    worksheet4.Cells.Style.Font.SetFromFont(new Font("Arial", 10));

                    worksheet4.Cells[1, 1, 1, 11].Merge = true;
                    worksheet4.Cells[1, 1].Style.Font.Bold = true;
                    worksheet4.Cells[1, 1].Value = string.Concat("VOTOS USUARIOS ", DateTime.Now.ToString("yyyy-MM-dd"));

                    worksheet4.Cells[3, 1, 3, pulse.Tables[3].Columns.Count].Style.Font.Bold = true;
                    worksheet4.Cells[3, 1].LoadFromDataTable(pulse.Tables[3], true);

                    worksheet4.Cells[4, 5, 4 + pulse.Tables[3].Rows.Count, 5].Style.Numberformat.Format = "yyyy-MM-dd";

                    worksheet4.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Write it back to the client
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", string.Concat("attachment;  filename=", fileName, ".xls"));
                    Response.BinaryWrite(pck.GetAsByteArray());
                }
            }
        }
    }
}

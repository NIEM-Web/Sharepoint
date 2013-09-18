using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using Microsoft.Office.Server.UserProfiles;
using System.Data;
using OfficeOpenXml;
namespace lmd.NIEM.FarmSolution
{
    public partial class ExportUsers : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (SPContext.Current.Web.CurrentUser.IsSiteAdmin)
                {
                    string format = Request.QueryString["f"];
                    if (string.IsNullOrEmpty(format) || string.Compare(format, "excel", true) == 0)
                    {
                        ExportUsersInExcel();
                    }
                    else if (string.Compare(format, "csv", true) == 0)
                    {
                        ExportUsersToCsv();

                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void ExportUsersToCsv()
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.Charset = "";
            Page.EnableViewState = false;
            Response.AddHeader("Content-Disposition", "inline;filename=report.csv");
            System.IO.StringWriter tw = new System.IO.StringWriter();
            List<UserDetail> userDetails = Utility.GetUserDetails();
            tw.WriteLine("Name, First Name, Last Name, Email, Login Name");
            foreach (var userDetail in userDetails)
            {
                tw.WriteLine(userDetail.Name + ", " + userDetail.FirstName + ", " + userDetail.LastName + ", " + userDetail.Email +
                ", " + userDetail.LoginName);
            }
            Response.Write(tw.ToString());
            Response.End();
        }

        void GenerateWorkbook(DataTable tbl)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Users");

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(tbl, true);

                //Write it back to the client
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=report.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }
           
        }

        private void ExportUsersInExcel()
        {
            DataTable tbl = new DataTable("Users");
            tbl.Columns.Add("Name");
            tbl.Columns.Add("First Name");
            tbl.Columns.Add("Last Name");
            tbl.Columns.Add("Email");
            tbl.Columns.Add("Login Name");
            List<UserDetail> userDetails = Utility.GetUserDetails();
            foreach (var userDetail in userDetails)
            {
                DataRow drow = tbl.NewRow();
                drow[0] = userDetail.Name;
                drow[1] = userDetail.FirstName;
                drow[2] = userDetail.LastName; ;
                drow[3] = userDetail.Email;
                string[] namepart = userDetail.LoginName.Split('|');
                drow[4] =namepart[namepart.Length -1] ;
                tbl.Rows.Add(drow);
            }
            GenerateWorkbook(tbl);
            //Response.Clear();
            //Response.ContentType = "application/vnd.ms-excel";
            //Response.Charset = "";
            //Page.EnableViewState = false;
            //Response.AddHeader("Content-Disposition", "inline;filename=report.xls");
            //System.IO.StringWriter tw = new System.IO.StringWriter();
            
            //tw.WriteLine("<table><tr><th>Name</th><th>First Name</th><th>Last Name</th><th>Email</th><th>Login Name</th></tr>");
            //foreach (var userDetail in userDetails)
            //{
            //    tw.WriteLine("<tr><td>" + userDetail.Name + "</td><td>" + userDetail.FirstName + "</td><td>" + userDetail.LastName + "</td><td>" + userDetail.Email +
            //    "</td><td>" + userDetail.LoginName + "</td></tr>");
            //}
            //tw.WriteLine("</table>");
            //Response.Write(tw.ToString());
            //Response.End();
        }
    }
}

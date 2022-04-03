using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using EmployeeInfo.Class;

namespace EmployeeInfo.Webforms
{
    public partial class WebViewEmployee : System.Web.UI.Page
    {
        Cls_MstEmployee vc_emp = new Cls_MstEmployee();
        DataTable dt = new DataTable();
        bool proc_res;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    GetAllEmployee();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            } 
        }
        private void GetAllEmployee()
        {
            try
            {
                
                vc_emp.GetAllEmployee(ref dt);
                if (dt.Rows.Count > 0)
                {
                    ViewState["sort_dt"] = dt;
                    GrdEmployee.DataSource = dt;
                    GrdEmployee.DataBind();
                }
                else
                {
                    GrdEmployee.DataSource = null;
                    GrdEmployee.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }

        protected void GrdEmployee_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dt = new DataTable();
            try
            {
                dt = (DataTable)ViewState["sort_dt"];
                GrdEmployee.PageIndex = e.NewPageIndex;
                GrdEmployee.DataSource = dt;
                GrdEmployee.DataBind();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }


        protected void GrdEmployee_DataBound(object sender, EventArgs e)
        {
            try
            {
                for (int i = GrdEmployee.Rows.Count - 1; i > 0; i--)
                {
                    GridViewRow row = GrdEmployee.Rows[i];
                    GridViewRow previousRow = GrdEmployee.Rows[i - 1];
                    for (int j = 0; j < row.Cells.Count; j++)
                    {                        
                        if (row.Cells[j].Text == previousRow.Cells[j].Text && row.Cells[0].Text == previousRow.Cells[0].Text)
                        {
                            if (previousRow.Cells[j].RowSpan == 0)
                            {
                                if (row.Cells[j].RowSpan == 0)
                                {
                                    previousRow.Cells[j].RowSpan += 2;
                                }
                                else
                                {
                                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                                }
                                row.Cells[j].Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }
    }
}
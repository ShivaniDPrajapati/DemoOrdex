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
    public partial class WebMstEmployee : System.Web.UI.Page
    {
        Cls_Common vc_common = new Cls_Common();
        DataTable dt = new DataTable();
        bool proc_res;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    vc_common.SetConnection();
                    Clear();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }

        public void Clear()
        {
            try
            {
                GetMaxCode();
                txtName.Text = "";
                txtAge.Text = "";
                ViewState["dt_item"] = null;
                FillEmptyGrid();
                FillEmptyGrid_Footer();
                txtName.Focus();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }

        public void GetMaxCode()
        {
            try
            {
                Cls_MstEmployee vc_emp = new Cls_MstEmployee();
                vc_emp.GetMaxEmpid(ref dt);
                if (dt.Rows.Count > 0)
                {
                    txtId.Text = (dt.Rows[0].IsNull("EmpId") == true ? "" : dt.Rows[0]["EmpId"]).ToString();
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }





        public Boolean saveValidate()
        {
            try
            {
                if (txtName.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Please Enter Employee Name');", true);
                    txtName.Focus();
                    return false;
                }

                if (txtAge.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Please Enter Employee Age');", true);
                    txtAge.Focus();
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
                return false;
            }
        }

        public int retInt(string strValue)
        {
            try
            {
                int intValue = 0;

                bool isNumerical = int.TryParse(strValue, out intValue);
                if (isNumerical == true)
                {
                    intValue = int.Parse(strValue);
                }
                return intValue;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
                return 0;
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            Cls_MstEmployee vc_emp = new Cls_MstEmployee();
            try
            {
                if (saveValidate() == true)
                {
                    vc_emp.begin_trans();
                    proc_res = vc_emp.InsertEmployee(retInt(txtId.Text), txtName.Text, retInt(txtAge.Text), DateTime.Now);

                    if (proc_res == true)
                    {
                        GridViewRow gr = grd_I_Add.Rows[0];
                        TextBox txt_IAddress = (TextBox)gr.FindControl("txt_I_Address");

                        if (grd_I_Add.Rows.Count > 0 && txt_IAddress.Text != "")
                        {
                            for (int i = 0; i <= grd_I_Add.Rows.Count - 1; i++)
                            {
                                TextBox txt_I_Address = (TextBox)grd_I_Add.Rows[i].FindControl("txt_I_Address");

                                if (txt_I_Address.Text == "")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Alert", "alert('Please Enter Address.');", true);
                                    txt_I_Address.Focus();
                                    return;
                                }

                                proc_res = vc_emp.InsertEmployeeAddress(retInt(txtId.Text), i + 1, txt_I_Address.Text, DateTime.Now);
                            }
                        }
                    }

                    if (proc_res == true)
                    {
                        vc_emp.commit_trans();
                        Clear();
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Alert", "alert('Save Successfully.');", true);
                       

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "Alert", "alert('Save Failed.');", true);
                        vc_emp.roll_back();
                    }
                }
            }
            catch (Exception ex)
            {

                vc_emp.rollback_trans();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }

        protected void btn_Cancel_Click(object sender, EventArgs e)
        {
            try { Clear(); }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }

        protected void FillEmptyGrid()
        {
            try
            {
                if (ViewState["dt_item"] == null)
                {
                    DataTable dtGrid = new DataTable();
                    dtGrid.Columns.Add("Address");


                    DataRow dr;
                    dr = dtGrid.NewRow();
                    dr["Address"] = "";
                    dtGrid.Rows.Add(dr);

                    ViewState["dt_item"] = dtGrid;
                    grd_I_Add.DataSource = dtGrid;
                    grd_I_Add.DataBind();
                    grd_I_Add.Rows[0].Visible = false;

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }
        protected void FillEmptyGrid_Footer()
        {
            try
            {
                DataTable dtGrid = new DataTable();
                dtGrid.Columns.Add("Address");

                DataRow dr;
                dr = dtGrid.NewRow();
                dr["Address"] = "";
                dtGrid.Rows.Add(dr);

                ViewState["dt_Footer"] = dtGrid;
                grd_F_Add.DataSource = dtGrid;
                grd_F_Add.DataBind();

                grd_F_Add.Rows[0].Visible = false;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }

        public void RemoveBlankRow()
        {
            try
            {
                if (ViewState["dt_item"] != null)
                {
                    DataTable dtItem = (DataTable)ViewState["dt_item"];
                    for (int i = 0; i <= dtItem.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j <= dtItem.Rows.Count - 1; j++)
                        {
                            DataRow dr = dtItem.Rows[j];
                            if (dr["Address"].ToString() == "")
                            {
                                dtItem.Rows.RemoveAt(j);
                                break;
                            }
                        }
                    }
                    ViewState["dt_item"] = dtItem;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }
        protected void Set_DG_For_Data_Bind()
        {
            try
            {
                DataTable dtGrid = new DataTable();
                Int32 i = 0;

                dtGrid.Columns.Add("Address");

                for (i = 0; i <= grd_I_Add.Rows.Count - 1; i++)
                {
                    GridViewRow gr;
                    gr = grd_I_Add.Rows[i];

                    TextBox txt_I_Address = (TextBox)gr.FindControl("txt_I_Address");

                    DataRow dr;
                    dr = dtGrid.NewRow();
                    dr["Address"] = txt_I_Address.Text;
                    dtGrid.Rows.Add(dr);
                }
                ViewState["dt_item"] = dtGrid;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt_item = new DataTable();

                TextBox txt_F_Address = (TextBox)grd_F_Add.FooterRow.FindControl("txt_F_Address");

                if (txt_F_Address.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('Please Enter Address');", true);
                    txt_F_Address.Focus();
                    return;
                }


                Set_DG_For_Data_Bind();
                dt_item = (DataTable)ViewState["dt_item"];

                Int32 r;
                r = dt_item.Rows.Count;
                dt_item.Rows.Add(r);

                DataRow dr = dt_item.NewRow();
                dt_item.Rows[r]["Address"] = txt_F_Address.Text;
                ViewState["dt_item"] = dt_item;
                RemoveBlankRow();

                grd_I_Add.DataSource = dt_item;
                grd_I_Add.DataBind();

                FillEmptyGrid_Footer();
                txt_F_Address.Focus();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }

        protected void grd_I_Add_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DataTable dtGrid = new DataTable();
                dtGrid = (DataTable)ViewState["dt_item"];
                if (dtGrid.Rows.Count > 0)
                {
                    dtGrid.Rows.RemoveAt(e.RowIndex);
                    if (dtGrid.Rows.Count == 0)
                    {
                        ViewState["dt_item"] = null;
                        FillEmptyGrid();
                        dtGrid = (DataTable)ViewState["dt_item"];
                    }
                }
                grd_I_Add.DataSource = dtGrid;
                grd_I_Add.DataBind();
                ViewState["dt_item"] = dtGrid;
                if (dtGrid.Rows[0]["Address"].ToString() == "")
                {
                    grd_I_Add.Rows[0].Visible = false;
                }
                FillEmptyGrid();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }
    }
}
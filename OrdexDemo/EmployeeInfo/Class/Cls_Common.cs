using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace EmployeeInfo.Class
{
    public class Cls_Common
    {
        string V_Server;
        string V_DB;
        string v_UID;
        string v_Pass;

        public void SetConnection()
        {
            try
            {
                V_Server = "";
                V_DB = "";
                v_UID = "";
                v_Pass = "";

                ConfigurationManager.AppSettings.Set("V_Server", V_Server);
                ConfigurationManager.AppSettings.Set("V_DB", V_DB);
                ConfigurationManager.AppSettings.Set("V_UID", v_UID);
                ConfigurationManager.AppSettings.Set("V_Pass", v_Pass);

                dynamic StreamReader1 = new System.IO.StreamReader(HttpContext.Current.Server.MapPath(@"~/V_SERVER.txt"));
                V_Server = StreamReader1.ReadToEnd();
                StreamReader1.Close();

                dynamic StreamReader2 = new System.IO.StreamReader(HttpContext.Current.Server.MapPath(@"~/V_DB.txt"));
                V_DB = StreamReader2.ReadToEnd();
                StreamReader2.Close();

                dynamic StreamReader3 = new System.IO.StreamReader(HttpContext.Current.Server.MapPath(@"~/V_USER.txt"));
                v_UID = StreamReader3.ReadToEnd();
                StreamReader3.Close();

                dynamic StreamReader4 = new System.IO.StreamReader(HttpContext.Current.Server.MapPath(@"~/V_PASS.txt"));
                v_Pass = StreamReader4.ReadToEnd();
                StreamReader4.Close();

                ConfigurationManager.AppSettings.Set("V_Server", V_Server);
                ConfigurationManager.AppSettings.Set("V_DB", V_DB);
                ConfigurationManager.AppSettings.Set("V_UID", v_UID);
                ConfigurationManager.AppSettings.Set("V_PASS", v_Pass);

            }
            catch (Exception ex)
            {
                throw ex;
                //ScriptManager.RegisterStartupScript(Page , this.Page.GetType(), "alert", "alert('" + ex.Message.Replace(Environment.NewLine, " ").Replace("'", " ") + "');", true);
            }
        }
    }
}
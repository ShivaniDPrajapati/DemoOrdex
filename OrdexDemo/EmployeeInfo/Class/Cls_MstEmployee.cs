using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using NetAccessConnection.ganesh.Dataccess;

namespace EmployeeInfo.Class
{
    public class Cls_MstEmployee
    {
        Cls_DataAccess cnn = new Cls_DataAccess();
        DataTable[] dt;
        Boolean proc_res;
        string[] str_sql = null;

        public Cls_MstEmployee()
        {
            try
            {
                cnn.prop_IsOLEConn = false;
                cnn.prop_datasource = ConfigurationManager.AppSettings.Get("V_Server");
                cnn.prop_iniCatlog = ConfigurationManager.AppSettings.Get("V_DB");
                cnn.prop_user_Id = ConfigurationManager.AppSettings.Get("v_UID");
                cnn.prop_Password = ConfigurationManager.AppSettings.Get("v_Pass");
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        public bool begin_trans()
        {
            try
            {
                proc_res = cnn.Sql_begin_trans();
                return proc_res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool commit_trans()
        {
            try
            {
                proc_res = cnn.Sql_commit_trans();
                return proc_res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool roll_back()
        {
            try
            {
                proc_res = cnn.Sql_rollback_trans();
                return proc_res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool is_begintras()
        {
            return cnn.prop_IsSqlBeginTrans;
        }

        public bool rollback_trans()
        {
            try
            {
                proc_res = cnn.Sql_rollback_trans();
                return proc_res;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }

        }

        public Boolean GetMaxEmpid(ref DataTable v_dt)
        {
            try
            {
                str_sql = new string[1];
                str_sql[0] = "[SPGetMaxEmpid]";
                proc_res = cnn.GetDataTable_on_Sp(str_sql, ref dt);
                v_dt = dt[0];
                return proc_res;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public Boolean GetAllEmployee(ref DataTable v_dt)
        {
            try
            {
                str_sql = new string[1];
                str_sql[0] = "[SPGetAllEmployee]";
                proc_res = cnn.GetDataTable_on_Sp(str_sql, ref dt);
                v_dt = dt[0];
                return proc_res;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public Boolean InsertEmployee(int EmpId,string EmpName,int EmpAge,  DateTime CreatedOn)
        {
            try
            {
                SqlParameter[] sqlparam = new SqlParameter[4];

                sqlparam[0] = new SqlParameter();
                sqlparam[0].ParameterName = "@EmpId";
                sqlparam[0].SqlDbType = SqlDbType.Int;
                sqlparam[0].Value = EmpId;

                sqlparam[1] = new SqlParameter();
                sqlparam[1].ParameterName = "@EmpName";
                sqlparam[1].SqlDbType = SqlDbType.VarChar;
                sqlparam[1].Size = 50;
                sqlparam[1].Value = EmpName;

                sqlparam[2] = new SqlParameter();
                sqlparam[2].ParameterName = "@EmpAge";
                sqlparam[2].SqlDbType = SqlDbType.Int;
                sqlparam[2].Value = EmpAge;

                sqlparam[3] = new SqlParameter();
                sqlparam[3].ParameterName = "@CreatedOn";
                sqlparam[3].SqlDbType = SqlDbType.DateTime;
                sqlparam[3].Value = CreatedOn;

                str_sql = new string[1];
                str_sql[0] = "[SPInsertEmployee]";
                proc_res = cnn.UpdateInfo_On_Sp(str_sql, sqlparam);
                return proc_res;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }

        public Boolean InsertEmployeeAddress(int EmpId,int EmpAddId, string Address, DateTime CreatedOn)
        {
            try
            {
                SqlParameter[] sqlparam = new SqlParameter[4];

                sqlparam[0] = new SqlParameter();
                sqlparam[0].ParameterName = "@EmpId";
                sqlparam[0].SqlDbType = SqlDbType.Int;
                sqlparam[0].Value = EmpId;

                sqlparam[1] = new SqlParameter();
                sqlparam[1].ParameterName = "@EmpAddId";
                sqlparam[1].SqlDbType = SqlDbType.Int;
                sqlparam[1].Value = EmpAddId;

                sqlparam[2] = new SqlParameter();
                sqlparam[2].ParameterName = "@Address";
                sqlparam[2].SqlDbType = SqlDbType.VarChar;
                sqlparam[2].Value = Address;

                sqlparam[3] = new SqlParameter();
                sqlparam[3].ParameterName = "@CreatedOn";
                sqlparam[3].SqlDbType = SqlDbType.DateTime;
                sqlparam[3].Value = CreatedOn;

                str_sql = new string[1];
                str_sql[0] = "[SPInsertEmployeeAddress]";
                proc_res = cnn.UpdateInfo_On_Sp(str_sql, sqlparam);
                return proc_res;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }
    }
}
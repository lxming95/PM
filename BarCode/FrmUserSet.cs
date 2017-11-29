using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace BarCode
{
    public partial class FrmUserSet : XtraForm
    {
        public FrmUserSet()
        {
            InitializeComponent();
        }

        private void FrmUserSet_Load(object sender, EventArgs e)
        {
            start();
        }

        private void gvUser_ShowingEditor(object sender, CancelEventArgs e)
        {
            switch (gvUser.FocusedColumn.Name)
            {
                case "colUserCode":
                    if (Convert.ToString(gvUser.GetFocusedRowCellValue(colUserName)) == "系统管理员")
                    {
                        e.Cancel = true;
                    }
                    break;
                case "colUserName":
                    if (Convert.ToString(gvUser.GetFocusedRowCellValue(colUserName)) == "系统管理员")
                    {
                        e.Cancel = true;
                    }
                    break;
            }
        }

        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gvUser.RowCount == 0)
            {
                return;
            }
            if (Convert.ToString(gvUser.GetFocusedRowCellValue(colUserName)) == "系统管理员")
            {
                XtraMessageBox.Show("系统管理员不能删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (XtraMessageBox.Show("是否要删除当前用户信息？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //删除数据行
                string sawid = gvUser.GetFocusedDataRow()["awid"].ToString();
                if (!string.IsNullOrEmpty(sawid))
                {
                    int iawid = Convert.ToInt32(sawid);
                    MySQLHelper.ExecuteNonQuery("DELETE FROM ry_user WHERE awid='"+iawid+"'");
                }
                gvUser.GetFocusedDataRow().Delete();
                (gcUser.DataSource as DataTable).AcceptChanges();
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //结束表格编辑状态
            gvUser.CloseEditor();
            gvUser.UpdateCurrentRow();
            //保存
            //BarCode.BLL.ry_user ryuserBll = new BLL.ry_user();
            //判断是新增保存还是修改保存
            //BarCode.Model.ry_user ryuserModel = new Model.ry_user();
            foreach (DataRow drow in (gcUser.DataSource as DataTable).Rows)
            {
                drow.EndEdit();
                //判断行的状态 
                if (drow.RowState == DataRowState.Modified)
                {

                    List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("user_code", drow["user_code"].ToString()),
                    new MySqlParameter("user_pass", drow["user_name"].ToString()),
                    new MySqlParameter("user_name", drow["user_pass"].ToString()),
                    new MySqlParameter("awid", Convert.ToInt32( drow["awid"])),
                    };
                    MySqlParameter[] param = ilistStr.ToArray();
                    MySQLHelper.ExecuteNonQuery("UPDATE ry_user SET user_code =@user_code, user_name=@user_name,user_pass=@user_pass WHERE awid = @awid", param);
                }
                //如果是新增
                if (drow.RowState == DataRowState.Added && !drow["awid"].ToString().Equals(""))
                {
                    List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("user_code", drow["user_code"].ToString()),
                    new MySqlParameter("user_pass", drow["user_name"].ToString()),
                    new MySqlParameter("user_name", drow["user_pass"].ToString()),
                    new MySqlParameter("awid", Convert.ToInt32( drow["awid"])),
                    };
                    MySqlParameter[] param = ilistStr.ToArray();
                    MySQLHelper.ExecuteNonQuery("INSERT INTO ry_user(user_code,user_pass,user_name) VALUES(@user_code, @user_pass,@user_name)", param);
                }
                drow.AcceptChanges();
            }
            XtraMessageBox.Show("保存成功");
        }

        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            start();
        }
        private void start()
        {
            if (Pub.PubValue.UserName == "系统管理员")
            {
                gcUser.DataSource = MySQLHelper.table("SELECT * FROM ry_user");
            }
            else
            {
                gcUser.DataSource = MySQLHelper.table("SELECT * FROM ry_user where user_code = '" + Pub.PubValue.UserCode + "'");
            }
        }
    }
}

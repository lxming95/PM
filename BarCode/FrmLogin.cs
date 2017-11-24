using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BarCode
{
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        public static string shop_code = null,DBName=null,suserName=null;
        frmMain main;
        string[] password;
        Boolean DLorGB = false;
        public FrmLogin(frmMain frmMain)
        {
            //初始化登陆界面
            InitializeComponent();
            //设置当前的服务器，读取app.config配置文件
            String constr = ConfigurationManager.AppSettings["ConnectionString"];
            //截取服务器地址信息
            string[] sLocalHost = constr.Split(new string[] { "Data Source=", ";Initial Catalog=" }, StringSplitOptions.RemoveEmptyEntries);
            if (sLocalHost.Length == 2)
            {
                txtServer.EditValue = sLocalHost[0];
                password = sLocalHost[1].Split(new string[] { "gr_uf_jiekou;Persist Security Info=True;User ID=sa;Password=", "" }, StringSplitOptions.RemoveEmptyEntries);
                main = frmMain;

                
            }
            
        }

        //关闭窗体
        private void FrmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(DLorGB==false)
            {
                Application.Exit();
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPassword.Focus();
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtServer.Text))
            {
                XtraMessageBox.Show("服务器不能为空！！");
                return;
            }
            
            suserName = this.txtUser.Text;
            string suserPassword = this.txtPassword.Text;
            List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("user_code", txtUser.Text),
                    new MySqlParameter("user_pass", suserPassword),};
            MySqlParameter[] param = ilistStr.ToArray();
            DataTable dt = MySQLHelper.table("SELECT * FROM ry_user WHERE user_code=@user_code  AND user_pass=@user_pass ", param);
            
            if (dt!=null&&dt.Rows.Count>0)
            {
                if (suserPassword.Equals(dt.Rows[0]["user_pass"]))
                {
                    this.Close();

                    DateTime LoginDate = DateTime.Today;
                    Pub.PubValue.ComputerName = Environment.MachineName;
                    //赋值执行程序目录
                    Pub.PubValue.ExePath = Environment.CurrentDirectory.ToString();
                    Pub.PubValue.LoginDate = LoginDate;
                    Pub.PubValue.SAPass = suserPassword;
                    Pub.PubValue.Server = txtServer.Text;
                    Pub.PubValue.UserID = Convert.ToInt32( dt.Rows[0]["awid"]);
                    Pub.PubValue.UserCode = dt.Rows[0]["user_code"].ToString();
                    Pub.PubValue.UserName = dt.Rows[0]["user_name"].ToString();
                    Pub.PubValue.ServerName = ConfigurationManager.AppSettings["ServerName"];
                    Pub.PubValue.DBName = ConfigurationManager.AppSettings["DBName"];
                    Pub.PubValue.SA = ConfigurationManager.AppSettings["SA"];
                    Pub.PubValue.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
                    Pub.PubValue.ConnectionStringUFData = ConfigurationManager.AppSettings["UFConnectionString"];
                    Pub.PubValue.ConnectionStringUFSystem = ConfigurationManager.AppSettings["UFSystemConnectionString"];
                    DLorGB = true;

                }
                else
                {
                    XtraMessageBox.Show("密码错误！！");
                }
            }
            else
            {
                XtraMessageBox.Show("用户名错误！！");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.txtUser.Text = null;
            this.txtPassword.Text = null;
            this.Close();
        }
    }
}

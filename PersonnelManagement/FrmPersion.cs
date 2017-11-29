using BarCode;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PersonnelManagement;
using DevExpress.XtraTab;

namespace PersonnelManagement
{
    public partial class FrmPersion : DevExpress.XtraEditors.XtraForm
    {
        public FrmPersion()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            start();
        }

        private void btnQuerry_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //DateTime d = Convert.ToDateTime("1995.06");
            //MessageBox.Show(d.ToString("yyMM"));
            #region 构造查询条件
            List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("cName",txtName.Text),
                    new MySqlParameter("cSex", txtSex.Text),
                    new MySqlParameter("cNation", txtNation.Text),
                    new MySqlParameter("cFull_timeEducation", txtcFull_timeEducation.Text),
                    new MySqlParameter("cIn_serviceEducation", txtcIn_serviceEducation.Text),
                    new MySqlParameter("cCurrentJob", txtCurrentJob.Text),
                    new MySqlParameter("cNativePlace", txtNativePlace.Text),
                    new MySqlParameter("Experience", txtExperience.Text),
            };
            MySqlParameter[] param = ilistStr.ToArray();

            StringBuilder MyStringBuilder = new StringBuilder(" ");
            //姓名
            if (txtName.Text != "")
            {
                if (isSafe(txtName.Text))
                {
                    MyStringBuilder.Append(" and cName like '%" + txtName.Text + "%'");
                }
            }
            //性别
            if (txtSex.Text != "")
                MyStringBuilder.Append(" and cSex =@cSex ");
            //民族
            if (txtNation.Text != "")
                if (isSafe(txtNation.Text))
                {
                    //MyStringBuilder.Append(" and cNation =@cNation ");
                    MyStringBuilder.Append(" and cNation like '%" + txtNation.Text + "%'");
                }

            //籍贯
            if (txtNativePlace.Text != "")
                if (isSafe(txtNativePlace.Text))
                {
                    //MyStringBuilder.Append(" and cNativePlace =@cNativePlace ");
                    MyStringBuilder.Append(" and cNativePlace like '%" + txtNativePlace.Text + "%'");
                }
            //是否本地人
            if (cbIsNative.Checked)
            {
                MyStringBuilder.Append(" and bIsNative ='1' ");
            }
            else
            {
                MyStringBuilder.Append(" and bIsNative ='0' ");
            }

            //年龄
            int year;

            Regex r = new Regex(@"^\d+$");//构造表达式
            Match m = r.Match(txtEage.Text);//匹配源文本
            Match m2 = r.Match(txtEage2.Text);//匹配源文本
            DateTime date1 = DateTime.Now;
            if (dDate.Text!="基准时间")
            {
                date1 = Convert.ToDateTime(dDate.Text);
            }
            if (m.Success&&m2.Success)
            {
                //MessageBox.Show("bigo");
                
                year = date1.Year - Convert.ToInt32(txtEage.Text);
                string da1 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                year = date1.Year - Convert.ToInt32(txtEage2.Text);
                string da2 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                MyStringBuilder.Append(" and dBirth_date > '" + da2 + "' and  dBirth_date < '"+da1+"'");
            }
            //党龄
            m = r.Match(txtJoinEage1.Text);//匹配源文本
            m2 = r.Match(txtJoinEage2.Text);//匹配源文本
            date1 = DateTime.Now;
            if (dDate.Text != "基准时间")
            {
                date1 = Convert.ToDateTime(dDate.Text);
            }
            if (m.Success && m2.Success)
            {
                //MessageBox.Show("bigo");

                year = date1.Year - Convert.ToInt32(txtJoinEage1.Text);
                string da1 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                year = date1.Year - Convert.ToInt32(txtJoinEage2.Text);
                string da2 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                MyStringBuilder.Append(" and dJoin_date > '" + da2 + "' and  dJoin_date < '" + da1 + "'");
            }

            //工龄
            m = r.Match(txtWorkEage1.Text);//匹配源文本
            m2 = r.Match(txtWorkEage2.Text);//匹配源文本
            date1 = DateTime.Now;
            if (dDate.Text != "基准时间")
            {
                date1 = Convert.ToDateTime(dDate.Text);
            }
            if (m.Success && m2.Success)
            {
                //MessageBox.Show("bigo");

                year = date1.Year - Convert.ToInt32(txtWorkEage1.Text);
                string da1 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                year = date1.Year - Convert.ToInt32(txtWorkEage2.Text);
                string da2 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                MyStringBuilder.Append(" and dWorkDate > '" + da2 + "' and  dWorkDate < '" + da1 + "'");
            }

            //全日制学历
            if (txtcFull_timeEducation.Text != "")
                MyStringBuilder.Append(" and cFull_timeEducation =@cFull_timeEducation ");
            //在职学历
            if (txtcIn_serviceEducation.Text != "")
                MyStringBuilder.Append(" and cIn_serviceEducation =@cIn_serviceEducation ");
            //现任职位
            if (txtCurrentJob.Text != "")
                MyStringBuilder.Append(" and cCurrentJob =@cCurrentJob ");
            //是否党政正职
            if (cbbIsOfficialPosition.Checked)
            {
                MyStringBuilder.Append(" and bIsOfficialPosition ='1' ");
            }
            else
            {
                MyStringBuilder.Append(" and bIsOfficialPosition ='0' ");
            }

            //简历起始时间
            if (dStartDate.Text != ""&& dStartDate.Text != "始")
                MyStringBuilder.Append(" and dStartDate like '"+Convert.ToDateTime(dStartDate.Text).ToString("yyyy-MM")+"%%'");
            //简历结束时间
            if (dDeadLine.Text != ""&& dDeadLine.Text != "止")
                MyStringBuilder.Append(" and dDeadline like '" + Convert.ToDateTime(dDeadLine.Text).ToString("yyyy-MM") + "%%'");

            //学习或者工作单位
            if (txtExperience.Text != "")
                if (isSafe(txtExperience.Text))
                {
                    //MyStringBuilder.Append(" and cExperience =@Experience ");
                    MyStringBuilder.Append(" and cExperience like '%" + txtExperience.Text + "%'");
                }

            #endregion
            DataTable dt= MySQLHelper.table("select * from data_persion where do_flag =1 " + MyStringBuilder, param);
            gcType.DataSource = FormatDT(dt, date1);
            txtNum.Text = (gcType.DataSource as DataTable).Rows.Count.ToString();
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow row = gvType.GetDataRow(gvType.FocusedRowHandle);
            if (row==null||row["pid"].Equals(""))                           //判断是否为空行
            {
                XtraMessageBox.Show("请不要选择空行");
                return;
            }
            FrmInfMain frm = new FrmInfMain(row["pid"].ToString());
            frm.Show();
        }

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow row = gvType.GetDataRow(gvType.FocusedRowHandle);
            if (row == null || row["pid"].Equals(""))
            {
                XtraMessageBox.Show("请不要选择空行");
                return;
            }
            string name = row["cName"].ToString() + "-";
            SaveFile.ExportLrm(row.Table,"");
            //XtraMessageBox.Show(@"""");
            //FrmInf frm = new FrmInf(row["pid"].ToString());
            //frm.Show();
        }

        /// <summary>
        /// 清空按钮点击时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //基本信息
            txtName.Text = "";
            txtSex.Text = "";
            txtNation.Text = "";
            txtcFull_timeEducation.Text = "";
            txtcIn_serviceEducation.Text = "";
            txtCurrentJob.Text = "";
            txtNativePlace.Text = "";
            dDate.Text = "";
            txtEage.Text = "";
            txtEage2.Text = "";
            txtJoinEage1.Text = "";
            txtJoinEage2.Text = "";
            txtWorkEage1.Text = "";
            txtWorkEage2.Text = "";
            cbbIsOfficialPosition.Checked = false;
            //经历
            dStartDate.Text = "";
            dDeadLine.Text = "";
            txtExperience.Text = "";
        }

        /// <summary>
        /// 添加按钮的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmInf form = new FrmInf();
            form.Show();
            //tcMmain = Pub.PubValue.frmmain;
            string frmname = "人员信息新增";
            //if (tabControlCheckHave(Pub.PubValue.frmmain., frmname))
            //{
            //    return;
            //}
            //else
            //{
            //    XtraTabPage tp = new XtraTabPage();
            //    tp.Text = frmname;
            //    tp.Resize += new System.EventHandler(this.tabPage_Resize);
            //    tcMain.TabPages.Add(tp);
            //    tcMain.SelectedTabPageIndex = tcMain.TabPages.Count - 1;
            //    tcMain.SelectedTabPage.Controls.Clear();
            //    tcMain.SelectedTabPage.Controls.Add(form);
            //    form.Show();
            //}
        }

        /// <summary>
        /// 删除按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DialogResult.Yes == XtraMessageBox.Show("是否删除？", "提示", MessageBoxButtons.YesNo))
            {
                DataRow row = gvType.GetDataRow(gvType.FocusedRowHandle);
                if (row["pid"].Equals(""))
                {
                    XtraMessageBox.Show("该条数据不存在，重新查询后重试");
                    return;
                }
                
                List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("pid",row["pid"].ToString()),
                };
                MySqlParameter[] param = ilistStr.ToArray();
                DataTable dt = MySQLHelper.table("select * from data_persion where do_flag=1 and pid =@pid", param);
                if (dt==null||dt.Rows.Count<=0)
                {
                    XtraMessageBox.Show("该条数据不存在，重新查询后重试");
                    return;
                }
                MySQLHelper.ExecuteNonQuery("UPDATE data_persion SET do_flag=2 WHERE pid=@pid",param);
                gcType.DataSource = MySQLHelper.table("select * from data_persion where do_flag =1");
            }
            

        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void start()
        {
            gcType.DataSource = FormatDT( MySQLHelper.table("select * from data_persion where do_flag =1"), DateTime.Now);

            //设置基准年龄选择框样式
            var formatString = "yyyy.MM";
            //var dateEdit = new DateEdit();
            //格式化显示字符串
            dDate.Properties.DisplayFormat.FormatString = formatString;
            //格式化编辑字符串
            dDate.Properties.Mask.EditMask = formatString;
            //设置选择样式
            dDate.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            dDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;

            dDate.Text = DateTime.Now.ToString("yyyy.MM");

        }
        /// <summary>
        /// 输入字符的安全检测
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private bool isSafe(string s)
        {
            if (Regex.IsMatch(s, @"[-|;|,|\/||||\}|\{|%|@|\*|!|\']"""))
            {
                XtraMessageBox.Show("请不要输入特殊字符");
                return false;
            }
            else { return true; }
        }

        private void gcType_DoubleClick(object sender, EventArgs e)
        {
            DataRow row = gvType.GetDataRow(gvType.FocusedRowHandle);
            if (row == null || row["pid"].Equals(""))                           //判断是否为空行
            {
                XtraMessageBox.Show("请不要选择空行");
                return;
            }
            FrmInfMain frm = new FrmInfMain(row["pid"].ToString(),false);
            frm.Show();
        }

        private DataTable FormatDT(DataTable dt,DateTime date)
        {
            if (!dt.Columns.Contains("cAge"))
                dt.Columns.Add("cAge", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["cAge"] = GetAgeByBirthdate(Convert.ToDateTime(dt.Rows[i]["dBirth_date"] + "/01"), date);
                if (dt.Rows[i]["bIsOfficialPosition"].ToString() == "1")
                {
                    dt.Rows[i]["bIsOfficialPosition"] = "是";
                }
                else
                {
                    dt.Rows[i]["bIsOfficialPosition"] = "否";
                }
            }

            return dt;
        }
        public int GetAgeByBirthdate(DateTime birthdate, DateTime date)
        {
            int age = date.Year - birthdate.Year;
            //if (date.Month < birthdate.Month || (date.Month == birthdate.Month && date.Day < birthdate.Day))
            if (date.Month < birthdate.Month)
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }
    }
}

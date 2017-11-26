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
    public partial class FrmPersionMain : DevExpress.XtraEditors.XtraForm
    {
        public FrmPersionMain()
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
                    new MySqlParameter("cIn_serviceEducation", txtEducation.Text),
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
                    MyStringBuilder.Append(" and cNation like '" + txtNation.Text + "%'");
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

            //学历
            if (txtEducation.Text != "")
                MyStringBuilder.Append(" and cIn_serviceEducation =@cIn_serviceEducation ");
            //身份
            if (txtCurrentJob.Text != "")
                MyStringBuilder.Append(" and cCurrentJob =@cCurrentJob ");
            //籍贯
            if (txtNativePlace.Text != "")
                if (isSafe(txtNativePlace.Text))
                {
                    //MyStringBuilder.Append(" and cNativePlace =@cNativePlace ");
                    MyStringBuilder.Append(" and cNativePlace like '" + txtNativePlace.Text + "%'");
                }

            //简历起始时间
            if (dStartDate.Text != "")
                MyStringBuilder.Append(" and dStartDate like '"+Convert.ToDateTime(dStartDate.Text).ToString("yyyy-MM")+"%%'");
            //简历结束时间
            if (dDeadLine.Text != "")
                MyStringBuilder.Append(" and dDeadline like '" + Convert.ToDateTime(dDeadLine.Text).ToString("yyyy-MM") + "%%'");

            //学习或者工作单位
            if (txtExperience.Text != "")
                if (isSafe(txtExperience.Text))
                {
                    //MyStringBuilder.Append(" and cExperience =@Experience ");
                    MyStringBuilder.Append(" and cExperience like '" + txtExperience.Text + "%'");
                }
            
            #endregion

            gcType.DataSource = MySQLHelper.table("select * from data_persion where do_flag =1 " + MyStringBuilder);
        }

        private void btnEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRow row = gvType.GetDataRow(gvType.FocusedRowHandle);
            if (row==null||row["pid"].Equals(""))                           //判断是否为空行
            {
                XtraMessageBox.Show("请不要选择空行");
                return;
            }
            FrmInf frm = new FrmInf(row["pid"].ToString());
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
            SaveFile.ExportLrm(row.Table, "");
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
            txtEducation.Text = "";
            txtCurrentJob.Text = "";
            txtNativePlace.Text = "";
            dDate.Text = "";
            txtEage.Text = "";
            txtEage2.Text = "";
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
                    new MySqlParameter("pid", "'"+row["pid"].ToString()+""),
                };
                MySqlParameter[] param = ilistStr.ToArray();
                DataTable dt = MySQLHelper.table("select * from data_persion where do_flag=1 and pid =@pid", param);
                if (dt==null||dt.Rows.Count<=0)
                {
                    XtraMessageBox.Show("该条数据不存在，重新查询后重试");
                    return;
                }
                MySQLHelper.ExecuteNonQuery("UPDATE data_persion SET do_flag=2 WHERE pid=@pid",param);

            }
            

        }
        /// <summary>
        /// 初始化，数据绑定
        /// </summary>
        private void start()
        {
            gcType.DataSource = MySQLHelper.table("select * from data_persion where do_flag =1");

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
            //性别下拉框绑定
            DataTable dt = new DataTable();
            dt.Columns.Add("sex",typeof(string));
            dt.Rows.Add();
            dt.Rows.Add();
            dt.Rows[0]["sex"] = "男";
            dt.Rows[1]["sex"] = "女";
            //txtSex.Properties.valuemember = "实际要用的字段";   //相当于editvalue
            //txtSex.Properties.DisplayMember = "要显示的字段";    //相当于text
            txtSex.Properties.DataSource = dt;

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

        /// <summary>
        /// 检查标签页是否被打开
        /// </summary>
        /// <param name="tab">窗体空间</param>
        /// <param name="tabName">要打开的功能的标签的名称</param>
        /// <returns></returns>
        public bool tabControlCheckHave(XtraTabControl tab, String tabName)
        {
            for (int i = 0; i < tab.TabPages.Count; i++)
            {
                if (tab.TabPages[i].Text == tabName)
                {
                    tab.SelectedTabPageIndex = i;
                    return true;
                }
            }
            return false;
        }
    }
}

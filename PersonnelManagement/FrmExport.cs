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

namespace PersonnelManagement
{

    public partial class FrmExport : DevExpress.XtraEditors.XtraForm
    {
        public bool ExportCheckBox = true;
        public FrmExport()
        {
            InitializeComponent();
            fromatDateEdit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //显示CheckBox列初始化界面
            if (ExportCheckBox)
            {
                gvType.OptionsSelection.MultiSelect = true;
                gvType.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            }
            //不显示CheckBox列初始化界面
            else
            {
                gvType.OptionsSelection.MultiSelect = false;
                gvType.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            }
            start();
        }

        private void btnQuerry_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //MessageBox.Show(dCheckYear.Text);
            #region 构造查询条件 主表信息查询
            List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("cName",txtName.Text),
                    new MySqlParameter("cSex", txtSex.Text),
                    new MySqlParameter("cNation", txtNation.Text),
                    new MySqlParameter("cFull_timeEducation", txtcFull_timeEducation.Text),
                    new MySqlParameter("cFull_timeDegree", txtcFull_timeDegree.Text),
                    new MySqlParameter("cFull_timeMajor", txtcFull_timeMajor.Text),
                    new MySqlParameter("cIn_serviceEducation", txtcIn_serviceEducation.Text),
                    new MySqlParameter("cIn_serviceDegree", txtcIn_serviceDegree.Text),
                    new MySqlParameter("cIn_serviceMajor", txtcIn_serviceMajor.Text),
                    new MySqlParameter("cCurrentJob", txtCurrentJob.Text),
                    new MySqlParameter("cIdentityCategory", txtcIdentityCategory.Text),
                    new MySqlParameter("cRank", txtcRank.Text),
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

            Regex r = new Regex(@"^\d+$");      //构造表达式
            Match m = r.Match(txtEage.Text);    //匹配源文本
            Match m2 = r.Match(txtEage2.Text);  //匹配源文本
            DateTime date1 = DateTime.Now;
            if (dDate.Text != "基准时间")
            {
                date1 = Convert.ToDateTime(dDate.Text);
            }
            if (m.Success)
            {
                //MessageBox.Show("bigo");

                year = date1.Year - Convert.ToInt32(txtEage.Text);
                string da1 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                
                MyStringBuilder.Append(" and  dBirth_date < '" + da1 + "' ");
            }
            if (m2.Success)
            {
                year = date1.Year - Convert.ToInt32(txtEage2.Text);
                string da2 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                MyStringBuilder.Append(" and dBirth_date > '" + da2 + "' ");
            }

            //党龄
            m = r.Match(txtJoinDate1.Text);
            m2 = r.Match(txtJoinDate2.Text);
            if (m.Success)
            {
                //MessageBox.Show("bigo");

                year = date1.Year - Convert.ToInt32(txtJoinDate1.Text);
                string da1 = year + date1.ToString("yyyy-MM-dd").Substring(4);

                MyStringBuilder.Append(" and  dJoin_date < '" + da1 + "' ");
            }
            if (m2.Success)
            {
                year = date1.Year - Convert.ToInt32(txtJoinDate2.Text);
                string da2 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                MyStringBuilder.Append(" and dJoin_date > '" + da2 + "' ");
            }
            //工龄
            m = r.Match(txtWorkDate1.Text);
            m2 = r.Match(txtWorkDate2.Text);
            if (m.Success)
            {
                //MessageBox.Show("bigo");

                year = date1.Year - Convert.ToInt32(txtWorkDate1.Text);
                string da1 = year + date1.ToString("yyyy-MM-dd").Substring(4);

                MyStringBuilder.Append(" and  dJoin_date < '" + da1 + "' ");
            }
            if (m2.Success)
            {
                year = date1.Year - Convert.ToInt32(txtWorkDate2.Text);
                string da2 = year + date1.ToString("yyyy-MM-dd").Substring(4);
                MyStringBuilder.Append(" and dJoin_date > '" + da2 + "' ");
            }
            //全日制
            //学历
            if (txtcFull_timeEducation.Text != "")
                MyStringBuilder.Append(" and cFull_timeEducation =@cFull_timeEducation ");
            //学位
            if (txtcFull_timeDegree.Text != "")
                MyStringBuilder.Append(" and cFull_timeDegree =@cFull_timeDegree ");
            //专业
            if (txtcFull_timeMajor.Text != "")
                MyStringBuilder.Append(" and cFull_timeMajor =@cFull_timeMajor ");

            //在职
            //学历
            if (txtcIn_serviceEducation.Text != "")
                MyStringBuilder.Append(" and cIn_serviceEducation =@cIn_serviceEducation ");
            //学位
            if (txtcIn_serviceDegree.Text != "")
                MyStringBuilder.Append(" and cIn_serviceDegree =@cIn_serviceDegree ");
            //专业
            if (txtcIn_serviceMajor.Text != "")
                MyStringBuilder.Append(" and cIn_serviceMajor =@cIn_serviceMajor ");

            //身份
            if (txtCurrentJob.Text != "")
                MyStringBuilder.Append(" and cCurrentJob =@cCurrentJob ");
            //籍贯
            if (txtNativePlace.Text != "")
                if (isSafe(txtNativePlace.Text))
                {
                    //MyStringBuilder.Append(" and cNativePlace =@cNativePlace ");
                    MyStringBuilder.Append(" and cNativePlace like '%" + txtNativePlace.Text + "%'");
                }
            if (txtIsNative.Text!="")
            {
                if(txtIsNative.Text.Equals("本地"))
                    MyStringBuilder.Append(" and bIsNative = 1");
                if (txtIsNative.Text.Equals("外地"))
                    MyStringBuilder.Append(" and bIsNative = 0");
            }
            if (txtCurrentJob.Text!="")
            {
                MyStringBuilder.Append(" and cCurrentJob like '%" + txtCurrentJob.Text + "%'");
            }
            if (txtcIdentityCategory.Text != "")
            {
                MyStringBuilder.Append(" and cIdentityCategory like '%" + txtcIdentityCategory.Text + "%'");
            }
            if (txtcRank.Text != "")
            {
                MyStringBuilder.Append(" and cRank like '%" + txtcRank.Text + "%'");
            }
            #endregion


            #region 从表信息查询
            StringBuilder other = new StringBuilder(" ");
            DataTable dt = new DataTable();
            #region 工作经历
            //简历起始时间
            if (dStartDate.Text != "")
                other.Append(" and dStartDate like '" + Convert.ToDateTime(dStartDate.DateTime).ToString("yyyy-MM") + "%'");
            //简历结束时间
            if (dDeadLine.Text != "")
                other.Append(" and dDeadline like '" + Convert.ToDateTime(dDeadLine.DateTime).ToString("yyyy-MM") + "%'");

            //学习或者工作单位
            if (txtExperience.Text != "")
                if (isSafe(txtExperience.Text))
                {
                    //MyStringBuilder.Append(" and cExperience =@Experience ");
                    other.Append(" and cExperience like '%" + txtExperience.Text + "%'");
                }
            dt= MySQLHelper.table("SELECT DISTINCT(PersionID) from data_resume where PersionID IS NOT NULL " + other);
            if(dStartDate.Text != ""&& dDeadLine.Text != ""&& txtExperience.Text != "")
                MyStringBuilder.Append(" and pid in ("+GetPidFromDatetable(dt)+")");
            other.Clear();
            dt.Clear();
            #endregion

            #region 奖惩信息
            if (dRewardData.Text != "")
                other.Append(" and dData like '%" + Convert.ToDateTime(dRewardData.DateTime).ToString("yyyy-MM") + "%'");
            if (txtcCategory.Text != "")
                other.Append(" and cCategory like '%" + txtcCategory.Text+ "%'");
            if (txtcLevel.Text != "")
                other.Append(" and cLevel like '%" + txtcLevel.Text+ "%'");
            if (txtcDetailed.Text != "")
                other.Append(" and cDetailed like '%" + txtcDetailed.Text + "%'");
            dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_rewards_punishments where PersionID IS NOT NULL " + other);
            if(dRewardData.Text != ""&& txtcCategory.Text != ""&& txtcLevel.Text != ""&& txtcDetailed.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            other.Clear();
            dt.Clear();
            #endregion 
            #region 考核结果
            if (dCheckYear.Text!="")
                other.Append(" and dcrYear like '%" + Convert.ToDateTime(dRewardData.DateTime).ToString("yyyy") + "%'");
            if(txtcrChechResult.Text!="")
                other.Append(" and crChechResult like '%" + txtcrChechResult.Text + "%'");
            dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_checkresult where PersionID IS NOT NULL " + other);
            if(dCheckYear.Text != ""&& txtcrChechResult.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            other.Clear();
            dt.Clear();
            #endregion
            #region 干部身份取得
            if (dGetCadresDate.Text!="")
                other.Append(" and gcData like '%" + Convert.ToDateTime(dGetCadresDate.DateTime).ToString("yyyy-MM") + "%'");
            if(txtgcDocumentBasis.Text!="")
                other.Append(" and gcDocumentBasis like '%" + txtgcDocumentBasis.Text + "%'");
            if(txtgcApprovingAuthority.Text!="")
                other.Append(" and gcApprovingAuthority like '%" + txtgcApprovingAuthority.Text + "%'");
            if(txtgcWay.Text!="")
                other.Append(" and gcWay like '%" + txtgcWay.Text + "%'");
            dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_getcadres where PersionID IS NOT NULL " + other);
            if(dGetCadresDate.Text != ""&& txtgcDocumentBasis.Text != ""&& txtgcApprovingAuthority.Text != ""&& txtgcWay.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            other.Clear();
            dt.Clear();
            #endregion
            #region  后备干部时间
            if(dReservecadreDate.Text!="")
                other.Append(" and rcYear like '%" + Convert.ToDateTime(dRewardData.DateTime).ToString("yyyy") + "%'");
            if(txtrcLevel.Text!="")
                other.Append(" and rcLevel like '%" + txtrcLevel.Text + "%'");
            dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_reservecadre where PersionID IS NOT NULL " + other);
            if(dReservecadreDate.Text != ""&& txtrcLevel.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            other.Clear();
            dt.Clear();
            #endregion
            #region 现实表现
            if(dPerformanceDate.Text!="")
                other.Append(" and dData like '%" + Convert.ToDateTime(dRewardData.DateTime).ToString("yyyy") + "%'");
            if(txtcSelfEvaluation.Text!="")
                other.Append(" and cSelfEvaluation like '%" + txtcSelfEvaluation.Text + "%'");
            if (txtcUnitEvaluation.Text != "")
                other.Append(" and cUnitEvaluation like '%" + txtcUnitEvaluation.Text + "%'");
            if (txtcOrganizationEvaluation.Text != "")
                other.Append(" and cOrganizationEvaluation like '%" + txtcOrganizationEvaluation.Text + "%'");
            dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_performance where PersionID IS NOT NULL  " + other);
            if(dPerformanceDate.Text != ""&& txtcSelfEvaluation.Text != ""&& txtcOrganizationEvaluation.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            other.Clear();
            dt.Clear();
            #endregion
            #endregion


            gcType.DataSource = MySQLHelper.table("select * from data_persion where do_flag =1 " + MyStringBuilder);
            txtNum.Text = (gcType.DataSource as DataTable).Rows.Count.ToString();

        }


        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //带CheckBox传值
            if (ExportCheckBox)
            {
                //返回传值
                foreach (int drid in gvType.GetSelectedRows())
                {
                    //ReturnDT.Rows.Add(gvType.GetDataRow(drid).ItemArray);
                    SaveFile.Export(gvType.GetFocusedDataRow().Table, gvType.GetFocusedDataRow()["cName"].ToString()+"-");
                }
            }
            //不带CheckBox传值
            else
            {
                //判断当前行是否为null
                if (gvType.RowCount == 0)
                {
                    MessageBox.Show("数据为空！");
                    return;
                }

                //ReturnDT.Rows.Add(gvType.GetFocusedDataRow().ItemArray);
                SaveFile.Export(gvType.GetFocusedDataRow().Table, gvType.GetFocusedDataRow()["cName"].ToString() + "-");
            }


            //DataRow row = gvType.GetDataRow(gvType.FocusedRowHandle);
            //if (row == null || row["pid"].Equals(""))
            //{
            //    XtraMessageBox.Show("请不要选择空行");
            //    return;
            //}

            //SaveFile.saveFile(SaveFile.formString(row.Table),"");
            ////XtraMessageBox.Show(@"""");
            //FrmInf frm = new FrmInf(row["pid"].ToString());
            //frm.Show();
        }

        /// <summary>
        /// 清空按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //基本信息
            txtName.Text = "";
            txtSex.Text = "";
            txtNation.Text = "";
            txtCurrentJob.Text = "";
            txtNativePlace.Text = "";
            dDate.Text = "";
            txtEage.Text = "";
            txtEage2.Text = "";
            txtJoinDate1.Text = "";
            txtJoinDate2.Text = "";
            txtWorkDate1.Text = "";
            txtWorkDate2.Text = "";

            txtcFull_timeEducation.Text = "";
            txtcFull_timeDegree.Text = "";
            txtcFull_timeMajor.Text = "";
            txtcIn_serviceEducation.Text = "";
            txtcIn_serviceDegree.Text = "";
            txtcIn_serviceMajor.Text = "";


            //经历
            dStartDate.Text = "";
            dDeadLine.Text = "";
            txtExperience.Text = "";
            txtcIdentityCategory.Text = "";
            txtcRank.Text = "";
            dRewardData.Text = "";
            txtcCategory.Text = "";
            txtcLevel.Text = "";
            txtcDetailed.Text = "";
            dCheckYear.Text = "";
            txtcrChechResult.Text = "";
            dGetCadresDate.Text = "";
            txtgcDocumentBasis.Text = "";
            txtgcApprovingAuthority.Text = "";
            txtgcWay.Text = "";
            dReservecadreDate.Text = "";
            txtrcLevel.Text = "";
            dPerformanceDate.Text = "";

            txtcSelfEvaluation.Text = "";
            txtcUnitEvaluation.Text = "";
            txtcOrganizationEvaluation.Text = "";

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
            //dDate.Properties.DisplayFormat.FormatString = formatString;
            //格式化编辑字符串
            dDate.Properties.Mask.EditMask = formatString;
            //使用编辑的格式作为显示的格式
            dDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            //设置选择样式
            dDate.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            dDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            //性别下拉框绑定
            DataTable dt = new DataTable();
            dt.Columns.Add("sex", typeof(string));
            dt.Rows.Add();
            dt.Rows.Add();
            dt.Rows.Add();
            dt.Rows[0]["sex"] = "";
            dt.Rows[1]["sex"] = "男";
            dt.Rows[2]["sex"] = "女";

            txtSex.Properties.ValueMember = "sex";   //相当于editvalue
            txtSex.Properties.DisplayMember = "sex";
            //txtSex.Properties.valuemember = "实际要用的字段";   //相当于editvalue
            //txtSex.Properties.DisplayMember = "要显示的字段";    //相当于text
            txtSex.Properties.DataSource = dt;
            DataTable dtNative=new DataTable();
            dtNative.Columns.Add("Native", typeof(string));
            dtNative.Rows.Add();
            dtNative.Rows.Add();
            dtNative.Rows.Add();
            dtNative.Rows[0]["Native"] = "";
            dtNative.Rows[1]["Native"] = "本地";
            dtNative.Rows[2]["Native"] = "外地";
            txtIsNative.Properties.DataSource = dtNative;
            txtIsNative.Properties.ValueMember = "Native";
            txtIsNative.Properties.DisplayMember = "Native";
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
        /// 获得datatable中PersionID的组合字符串，用于查询
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetPidFromDatetable(DataTable dt)
        {
            string s = "";
            if (dt!=null&&dt.Rows.Count>0&&dt.Columns.Contains("PersionID"))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    s += dt.Rows[i]["PersionID"].ToString()+",";
                }
                s = s.Substring(0, s.Length - 1);
            }

            return s;
        }

        private void fromatDateEdit()
        {
            var formatString = "yyyy";
            dCheckYear.Properties.Mask.EditMask= formatString;
            //dDate2.DisplayFormat.FormatString = formatString;
            dCheckYear.Properties.Mask.UseMaskAsDisplayFormat = true;
            dCheckYear.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dCheckYear.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;
        }
    }
}

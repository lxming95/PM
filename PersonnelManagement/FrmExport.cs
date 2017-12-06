﻿using BarCode;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PersonnelManagement;
using System.Data.OleDb;
using System.IO;

namespace PersonnelManagement
{

    public partial class FrmExport : DevExpress.XtraEditors.XtraForm
    {
        DateTime date1 = DateTime.Now;          //基准时间
        public bool ExportCheckBox = true;      //是否带复选框
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
                //设置列宽
                gvType.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
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

            if (dDate.Text != "基准时间"&& dDate.Text != "")
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
            //本地人
            if (cbIsNative.Checked==true)
            {
                MyStringBuilder.Append(" and bIsNative = 1");
                //if(txtIsNative.Text.Equals("本地"))
                //    MyStringBuilder.Append(" and bIsNative = 1");
                //if (txtIsNative.Text.Equals("外地"))
                //    MyStringBuilder.Append(" and bIsNative = 0");
            }
            //党政正职
            if (cbIsOfficialPosition.Checked==true)
            {
                MyStringBuilder.Append(" and bIsOfficialPosition = 1");
            }
            //现任职务
            if (txtCurrentJob.Text!="")
            {
                MyStringBuilder.Append(" and cCurrentJob like '%" + txtCurrentJob.Text + "%'");
            }
            //身份类别
            if (txtcIdentityCategory.Text != "")
            {
                MyStringBuilder.Append(" and cIdentityCategory like '%" + txtcIdentityCategory.Text + "%'");
            }
            //个人所属职级
            if (txtcRank.Text != "")
            {
                MyStringBuilder.Append(" and cRank like '%" + txtcRank.Text + "%'");
            }
            //干部身份取得
            if (dGetCadresDate.Text != "" && dGetCadresDate.Text != "起")
                MyStringBuilder.Append(" and dGetCadresDate like '%" + Convert.ToDateTime(dGetCadresDate.DateTime).ToString("yyyy-MM") + "%'");
            if (txtgcDocumentBasis.Text != "")
                MyStringBuilder.Append(" and cDocumentBasis like '%" + txtgcDocumentBasis.Text + "%'");
            if (txtgcApprovingAuthority.Text != "")
                MyStringBuilder.Append(" and cApprovingAuthority like '%" + txtgcApprovingAuthority.Text + "%'");
            if (txtgcWay.Text != "")
                MyStringBuilder.Append(" and cWay like '%" + txtgcWay.Text + "%'");

            #endregion


            #region 从表信息查询
            StringBuilder other = new StringBuilder(" ");
            DataTable dt = new DataTable();
            #region 工作经历
            //简历起始时间
            if (dStartDate.Text != ""&& dStartDate.Text != "起")
                other.Append(" and dStartDate like '" + Convert.ToDateTime(dStartDate.DateTime).ToString("yyyy-MM") + "%'");
            //简历结束时间
            if (dDeadLine.Text != ""&& dDeadLine.Text != "止")
                other.Append(" and dDeadline like '" + Convert.ToDateTime(dDeadLine.DateTime).ToString("yyyy-MM") + "%'");

            //学习或者工作单位
            if (txtExperience.Text != "")
                if (isSafe(txtExperience.Text))
                {
                    //MyStringBuilder.Append(" and cExperience =@Experience ");
                    other.Append(" and cExperience like '%" + txtExperience.Text + "%'");
                }
            if (!other.Equals(""))
            {
                dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_resume where PersionID IS NOT NULL " + other);
                //if(dStartDate.Text != "" && dStartDate.Text != "起" && dDeadLine.Text != "" && dDeadLine.Text != "止" && txtExperience.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            }
            other.Clear();
            dt.Clear();
            #endregion

            #region 奖惩信息
            if (dRewardData.Text != ""&& dRewardData.Text != "起")
                other.Append(" and dData like '%" + Convert.ToDateTime(dRewardData.DateTime).ToString("yyyy-MM") + "%'");
            if (txtcCategory.Text != "")
                other.Append(" and cCategory like '%" + txtcCategory.Text+ "%'");
            if (txtcLevel.Text != "")
                other.Append(" and cLevel like '%" + txtcLevel.Text+ "%'");
            if (txtcDetailed.Text != "")
                other.Append(" and cDetailed like '%" + txtcDetailed.Text + "%'");
            if (!other.Equals(""))
            {
                dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_rewards_punishments where PersionID IS NOT NULL " + other);
                //if(dRewardData.Text != "" && dRewardData.Text != "起" && txtcCategory.Text != ""&& txtcLevel.Text != ""&& txtcDetailed.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            }
            other.Clear();
            dt.Clear();
            #endregion 
            #region 考核结果
            if (dCheckYear.Text!=""&& dCheckYear.Text != "起")
                other.Append(" and dcrYear like '%" + Convert.ToDateTime(dCheckYear.DateTime).ToString("yyyy") + "%'");
            if(txtcrChechResult.Text!="")
                other.Append(" and crChechResult like '%" + txtcrChechResult.Text + "%'");
            if (!other.Equals(""))
            {
                dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_checkresult where PersionID IS NOT NULL " + other);
                //if(dCheckYear.Text != "" && dCheckYear.Text != "起" && txtcrChechResult.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            }
            other.Clear();
            dt.Clear();
            #endregion
            #region 干部身份取得
            //if (dGetCadresDate.Text!=""&& dGetCadresDate.Text != "起")
            //    other.Append(" and gcData like '%" + Convert.ToDateTime(dGetCadresDate.DateTime).ToString("yyyy-MM") + "%'");
            //if(txtgcDocumentBasis.Text!="")
            //    other.Append(" and gcDocumentBasis like '%" + txtgcDocumentBasis.Text + "%'");
            //if(txtgcApprovingAuthority.Text!="")
            //    other.Append(" and gcApprovingAuthority like '%" + txtgcApprovingAuthority.Text + "%'");
            //if(txtgcWay.Text!="")
            //    other.Append(" and gcWay like '%" + txtgcWay.Text + "%'");
            //if (!other.Equals(""))
            //{
            //    dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_getcadres where PersionID IS NOT NULL " + other);
            //    //if(dGetCadresDate.Text != "" && dGetCadresDate.Text != "起" && txtgcDocumentBasis.Text != ""&& txtgcApprovingAuthority.Text != ""&& txtgcWay.Text != "")
            //    MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            //}
            other.Clear();
            dt.Clear();
            #endregion
            #region  后备干部时间
            if(dReservecadreDate.Text!=""&& dReservecadreDate.Text != "起")
                other.Append(" and rcYear like '%" + Convert.ToDateTime(dReservecadreDate.DateTime).ToString("yyyy") + "%'");
            if(txtrcLevel.Text!="")
                other.Append(" and rcLevel like '%" + txtrcLevel.Text + "%'");
            if (!other.Equals(""))
            {
                dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_reservecadre where PersionID IS NOT NULL " + other);
                //if(dReservecadreDate.Text != "" && dReservecadreDate.Text != "起" && txtrcLevel.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            }
            other.Clear();
            dt.Clear();
            #endregion
            #region 现实表现
            if(dPerformanceDate.Text!=""&& dPerformanceDate.Text != "起")
                other.Append(" and rpYear like '%" + Convert.ToDateTime(dPerformanceDate.DateTime).ToString("yyyy") + "%'");
            if(txtcSelfEvaluation.Text!="")
                other.Append(" and cSelfEvaluation like '%" + txtcSelfEvaluation.Text + "%'");
            if (txtcUnitEvaluation.Text != "")
                other.Append(" and cUnitEvaluation like '%" + txtcUnitEvaluation.Text + "%'");
            if (txtcOrganizationEvaluation.Text != "")
                other.Append(" and cOrganizationEvaluation like '%" + txtcOrganizationEvaluation.Text + "%'");
            if (!other.Equals(""))
            {
                dt = MySQLHelper.table("SELECT DISTINCT(PersionID) from data_performance where PersionID IS NOT NULL  " + other);
                //if(dPerformanceDate.Text != "" && dPerformanceDate.Text != "起" && txtcSelfEvaluation.Text != ""&& txtcOrganizationEvaluation.Text != "")
                MyStringBuilder.Append(" and pid in (" + GetPidFromDatetable(dt) + ")");
            }
            other.Clear();
            dt.Clear();
            #endregion
            #endregion


            gcType.DataSource = FormatDT(MySQLHelper.table("select * from data_persion where do_flag =1 " + MyStringBuilder), date1);
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
                    SaveFile.ExportLrm(gvType.GetDataRow(drid).Table, gvType.GetDataRow(drid)["cName"].ToString()+"-");
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
                SaveFile.ExportLrm(gvType.GetFocusedDataRow().Table, gvType.GetFocusedDataRow()["cName"].ToString() + "-");
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
            gcType.DataSource = FormatDT(MySQLHelper.table("select * from data_persion where do_flag =1"),DateTime.Now);
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
                //s = s.Substring(0, s.Length - 1);
            }
            s += @"''";
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

            dPerformanceDate.Properties.Mask.EditMask = formatString;
            dPerformanceDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            dPerformanceDate.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dPerformanceDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;

            dReservecadreDate.Properties.Mask.EditMask = formatString;
            dReservecadreDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            dReservecadreDate.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dReservecadreDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;

            dCheckYear.Properties.Mask.EditMask = formatString;
            dCheckYear.Properties.Mask.UseMaskAsDisplayFormat = true;
            dCheckYear.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dCheckYear.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;

            //设置基准年龄选择框样式
            formatString = "yyyy.MM";
            //var dateEdit = new DateEdit();
            //格式化显示字符串
            //dDate.Properties.DisplayFormat.FormatString = formatString;
            //格式化编辑字符串
            dDate.Properties.Mask.EditMask = formatString;
            //使用编辑的格式作为显示的格式
            dDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            //设置选择样式
            dDate.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            dDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.Default;

            dRewardData.Properties.Mask.EditMask = formatString;
            dRewardData.Properties.Mask.UseMaskAsDisplayFormat = true;
            dRewardData.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            dRewardData.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.Default;

            dGetCadresDate.Properties.Mask.EditMask = formatString;
            dGetCadresDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            dGetCadresDate.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            dGetCadresDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.Default;
        }

        private void btnExportPersionList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //string name= gvType.GetFocusedDataRow()["cName"].ToString() + "-";
            //SaveFile.ExportExcel(name);

            DataTable ReturnDT = (gcType.DataSource as DataTable).Clone();
            ReturnDT.Clear();
            //带CheckBox传值
            if (ExportCheckBox)
            {
                //返回传值
                foreach (int drid in gvType.GetSelectedRows())
                {
                    ReturnDT.Rows.Add(gvType.GetDataRow(drid).ItemArray);
                    //SaveFile.ExportLrm(gvType.GetDataRow(drid).Table, gvType.GetDataRow(drid)["cName"].ToString() + "-");
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

                ReturnDT.Rows.Add(gvType.GetFocusedDataRow().ItemArray);
                //SaveFile.ExportLrm(gvType.GetFocusedDataRow().Table, gvType.GetFocusedDataRow()["cName"].ToString() + "-");
            }
            SaveFile.ExportExcel(ReturnDT);
        }
        /// <summary>
        /// 添加年龄，添加党政正职
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private DataTable FormatDT(DataTable dt, DateTime date)
        {
            if (!dt.Columns.Contains("cAge"))
                dt.Columns.Add("cAge", typeof(string));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["cAge"] = SaveFile.GetAgeByBirthdate(Convert.ToDateTime(dt.Rows[i]["dBirth_date"] + ".01"), date);
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
        /// <summary>
        /// 导出word任免审批表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //带CheckBox传值
            if (ExportCheckBox)
            {
                //返回传值
                foreach (int drid in gvType.GetSelectedRows())
                {
                    //ReturnDT.Rows.Add(gvType.GetDataRow(drid).ItemArray);

                    SaveFile.ExportWord(date1, gvType.GetDataRow(drid).Table, "干部任免审批表-" + gvType.GetDataRow(drid)["cName"].ToString() + gvType.GetDataRow(drid)["dBirth_date"].ToString());
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
                SaveFile.ExportWord(date1, gvType.GetFocusedDataRow().Table, "干部任免审批表-"+gvType.GetFocusedDataRow()["cName"].ToString()+ gvType.GetFocusedDataRow()["dBirth_date"].ToString());
            }
        }
        /// <summary>
        /// 导出excel任免审批表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExportExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //带CheckBox传值
            if (ExportCheckBox)
            {
                //返回传值
                foreach (int drid in gvType.GetSelectedRows())
                {
                    //ReturnDT.Rows.Add(gvType.GetDataRow(drid).ItemArray);

                    SaveFile.seveExcel(date1,gvType.GetDataRow(drid), "干部任免审批表-" + gvType.GetDataRow(drid)["cName"].ToString() + gvType.GetDataRow(drid)["dBirth_date"].ToString());
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
                SaveFile.seveExcel(date1, gvType.GetFocusedDataRow(), "干部任免审批表-" + gvType.GetFocusedDataRow()["cName"].ToString() + gvType.GetFocusedDataRow()["dBirth_date"].ToString());
            }
        }
    }
}

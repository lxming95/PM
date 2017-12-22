using BarCode;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PersonnelManagement
{

    public partial class FrmInport : DevExpress.XtraEditors.XtraForm
    {
        public bool ExportCheckBox = false;
        public FrmInport()
        {
            InitializeComponent();
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
        /// <summary>
        /// 初始化，数据绑定
        /// </summary>
        private void start()
        {
            //gcType.DataSource = MySQLHelper.table("select * from data_persion where do_flag =1");

            //设置基准年龄选择框样式
            //var formatString = "yyyy.MM";
            //var dateEdit = new DateEdit();
            //dDate.Properties.Mask.EditMask = formatString;
            //dDate.Properties.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            //dDate.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;

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

        private void btnInPort_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            inport();
            
        }
        private void inport()
        {
            string Path = "";
            int tatal = 0;
            int success = 0;
            OpenFileDialog fileDialog1 = new OpenFileDialog();
            fileDialog1.InitialDirectory = "C:\\";//默认打开C：
            fileDialog1.Filter = "Excel 97-2013 工作簿(*.xls)|*.xls";
            fileDialog1.FilterIndex = 1;//如果您设置 FilterIndex 属性，则当显示对话框时，将选择该筛选器。
            fileDialog1.RestoreDirectory = true;//取得或设定值，指出对话方块是否在关闭前还原目前的目录。
            if (fileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Bitmap bitmap = new Bitmap(fileDialog1.FileName);// fileDialog1.FileName显示选中文件的路径
                Path = fileDialog1.FileName;
                //picGPS.Image = bitmap;
            }
            else
            {
                //MessageBox.Show("");
                return;

            }
            if (Path == "")
                return;

            DataTable dt = new DataTable();
            try
            {
                string strConn = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Path + ";" + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
                string tableName = schemaTable.Rows[0][2].ToString().Trim();
                string strExcel = "";
                OleDbDataAdapter myCommand = null;
                DataSet ds = null;
                //strExcel = "select * from [sheet1$]";
                //strExcel = "select * from [" + tableName + "]";
                #region 主要信息
                strExcel = "select * from [基本信息$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt = ds.Tables[0];
                dt.Columns[12].ColumnName = "cFull_timeSchool";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        if (dt.Rows[i][3].ToString() != "")
                        {
                            List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                            new MySqlParameter("cIDnumber", dt.Rows[i][1].ToString()),              //身份证号码
                            new MySqlParameter("cUnit", dt.Rows[i][2].ToString()),                  //单位
                            new MySqlParameter("cName", dt.Rows[i][3].ToString()),                  //姓名
                            new MySqlParameter("cCurrentJob", dt.Rows[i][4].ToString()),            //现任职务
                            new MySqlParameter("cSex", dt.Rows[i][5].ToString()),                   //性别
                            new MySqlParameter("cNation", dt.Rows[i][6].ToString()),                //民族
                            new MySqlParameter("cNativePlace", dt.Rows[i][7].ToString()),           //籍贯
                            new MySqlParameter("dBirth_date", fromDate(dt.Rows[i][8].ToString(),'.')),  //出生年月
                            new MySqlParameter("dJoin_date", fromDate(dt.Rows[i][9].ToString(),'.')),   //入党时间
                            new MySqlParameter("dWorkDate", fromDate(dt.Rows[i][10].ToString(),'.')),    //工作时间
                            new MySqlParameter("cFull_timeEducation", dt.Rows[i][11].ToString()),   //全日制学历
                            new MySqlParameter("cFull_timeDegree", dt.Rows[i][12].ToString()),      //全日制学位
                            new MySqlParameter("cFull_timeSchool", dt.Rows[i][13].ToString()),      //全日制毕业院校
                            new MySqlParameter("cFull_timeMajor", dt.Rows[i][14].ToString()),       //全日制专业
                            new MySqlParameter("cIn_serviceEducation", dt.Rows[i][15].ToString()),  //在职学历
                            new MySqlParameter("cIn_serviceDegree", dt.Rows[i][16].ToString()),     //在职学位
                            new MySqlParameter("cIn_serviceSchool", dt.Rows[i][17].ToString()),     //在职毕业院校
                            new MySqlParameter("cIn_serviceMajor", dt.Rows[i][18].ToString()),      //在职专业
                            new MySqlParameter("cResume", dt.Rows[i][19].ToString()),               //历任职务/工作经历
                            new MySqlParameter("dInOffice", fromDate(dt.Rows[i][20].ToString(),'.')),   //任现职时间
                            new MySqlParameter("dSameOffic", fromDate(dt.Rows[i][21].ToString(),'.')),  //任同等职位时间
                            new MySqlParameter("cRank", dt.Rows[i][22].ToString()),                 //个人所属职级
                            //new MySqlParameter("bIsOfficialPosition", dt.Rows[i][23].ToString()),   //是否党政正职
                            new MySqlParameter("cBirthPlace", dt.Rows[i][24].ToString()),           //出生地
                            //new MySqlParameter("bIsNative", dt.Rows[i][25].ToString()),             //是否本地人
                            new MySqlParameter("cDismissalReason", dt.Rows[i][26].ToString()),      //任免理由
                            new MySqlParameter("cHealthStatus", dt.Rows[i][27].ToString()),         //健康状况
                            new MySqlParameter("cDuties", dt.Rows[i][28].ToString()),               //专业技术职务
                            new MySqlParameter("cSkill", dt.Rows[i][29].ToString()),                //熟悉专业和特长
                            new MySqlParameter("cIdentityCategory", dt.Rows[i][30].ToString()),     //身份类别
                            new MySqlParameter("dGetCadresDate", dt.Rows[i][31].ToString()),        //干部身份取得时间
                            new MySqlParameter("cWay", dt.Rows[i][32].ToString()),                  //干部身份取得途径
                            new MySqlParameter("cDocumentBasis", dt.Rows[i][33].ToString()),        //干部身份取得文件依据
                            new MySqlParameter("cApprovingAuthority", dt.Rows[i][34].ToString()),   //干部身份取得审批机关
                            new MySqlParameter("cRemarks", dt.Rows[i][35].ToString()),              //备注

                    };
                            //XtraMessageBox.Show(dt.Rows[i][1].ToString());
                            //MySQLHelper.ExecuteNonQuery(" INSERT INTO data_persion(cUnit) VALUES('" + dt.Rows[i][1].ToString()+"') ");
                            if (dt.Rows[i][23].ToString().Equals("是"))
                                ilistStr.Add(new MySqlParameter("bIsOfficialPosition", "1"));
                            else
                                ilistStr.Add(new MySqlParameter("bIsOfficialPosition", "0"));

                            if (dt.Rows[i][25].ToString().Equals("是"))
                                ilistStr.Add(new MySqlParameter("bIsNative", "1"));
                            else
                                ilistStr.Add(new MySqlParameter("bIsNative", "0"));
                            MySqlParameter[] param = ilistStr.ToArray();
                            DataTable dt_hasPersion = MySQLHelper.table("SELECT * FROM data_persion WHERE cName=@cName AND dBirth_date=@dBirth_date AND do_flag='1'",param);
                            if (dt_hasPersion != null && dt_hasPersion.Rows.Count > 0)
                            {
                                ilistStr.Add(new MySqlParameter("pid", dt_hasPersion.Rows[0]["pid"]));
                                param = ilistStr.ToArray();
                                MySQLHelper.ExecuteNonQuery("DELETE FROM data_persion WHERE pid=@pid ",param);
                                MySQLHelper.ExecuteNonQuery("INSERT INTO data_persion(pid,cIDnumber,cUnit,cName,cCurrentJob,cSex,"
                                    + "cNation, cNativePlace, dBirth_date, dJoin_date, dWorkDate, cFull_timeEducation,cFull_timeDegree,cFull_timeSchool,cFull_timeMajor,"
                                    + "cIn_serviceEducation,cIn_serviceDegree, cIn_serviceSchool,cIn_serviceMajor, cResume, dInOffice, dSameOffic, cRemarks,"
                                    + " cRank,bIsOfficialPosition,cBirthPlace,bIsNative,cDismissalReason,cHealthStatus,cDuties,cSkill,cIdentityCategory,dGetCadresDate,cWay,cDocumentBasis,cApprovingAuthority)"
                                    + "VALUES(@pid,@cIDnumber, @cUnit, @cName, @cCurrentJob, @cSex,"
                                    + "@cNation, @cNativePlace, @dBirth_date, @dJoin_date, @dWorkDate, @cFull_timeEducation,@cFull_timeDegree,@cFull_timeSchool,@cFull_timeMajor,"
                                    + "@cIn_serviceEducation,@cIn_serviceDegree, @cIn_serviceSchool,@cIn_serviceMajor, @cResume, @dInOffice, @dSameOffic, @cRemarks,"
                                    + " @cRank,@bIsOfficialPosition,@cBirthPlace,@bIsNative,@cDismissalReason,@cHealthStatus,@cDuties,@cSkill,@cIdentityCategory,@dGetCadresDate,@cWay,@cDocumentBasis,@cApprovingAuthority)", param);
                            }
                            else
                            {
                                MySQLHelper.ExecuteNonQuery("INSERT INTO data_persion(cIDnumber,cUnit,cName,cCurrentJob,cSex,"
                                    + "cNation, cNativePlace, dBirth_date, dJoin_date, dWorkDate, cFull_timeEducation,cFull_timeDegree,cFull_timeSchool,cFull_timeMajor,"
                                    + "cIn_serviceEducation,cIn_serviceDegree, cIn_serviceSchool,cIn_serviceMajor, cResume, dInOffice, dSameOffic, cRemarks,"
                                    + " cRank,bIsOfficialPosition,cBirthPlace,bIsNative,cDismissalReason,cHealthStatus,cDuties,cSkill,cIdentityCategory,dGetCadresDate,cWay,cDocumentBasis,cApprovingAuthority)"
                                    + "VALUES(@cIDnumber, @cUnit, @cName, @cCurrentJob, @cSex,"
                                    + "@cNation, @cNativePlace, @dBirth_date, @dJoin_date, @dWorkDate, @cFull_timeEducation,@cFull_timeDegree,@cFull_timeSchool,@cFull_timeMajor,"
                                    + "@cIn_serviceEducation,@cIn_serviceDegree, @cIn_serviceSchool,@cIn_serviceMajor, @cResume, @dInOffice, @dSameOffic, @cRemarks,"
                                    + " @cRank,@bIsOfficialPosition,@cBirthPlace,@bIsNative,@cDismissalReason,@cHealthStatus,@cDuties,@cSkill,@cIdentityCategory,@dGetCadresDate,@cWay,@cDocumentBasis,@cApprovingAuthority)", param);

                            }

                        }
                        success++;
                    }
                    catch (Exception e)
                    {
                        XtraMessageBox.Show("第" + i + "条插入错误，错误信息:" + e.Message + ".");
                        // savefile.savelog("第"+i+"条插入错误，错误信息:"+e.Message+".");
                    }
                }

                #endregion

                #region 学习或工作简历 data_resume
                strExcel = "SELECT 本人姓名,出生年月 FROM [学习及工作简历$] WHERE len(本人姓名)>0  GROUP BY 本人姓名,出生年月";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                DataTable dt_cl = ds.Tables[0];
                foreach (DataRow r in dt_cl.Rows)
                {
                    DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName='"+r["本人姓名"] +"' AND dBirth_date LIKE '%" + r["出生年月"].ToString() + "%'");
                    if (dt_persion == null || dt_persion.Rows.Count <= 0)
                    {
                        XtraMessageBox.Show("请确保数据库中存在" + r["本人姓名"].ToString() + "的信息，然后再插入相关的学习及工作简历");
                        return;
                        //break;
                    }
                    MySQLHelper.ExecuteNonQuery("DELETE FROM data_resume WHERE PersionID='" + dt_persion.Rows[0]["pid"].ToString()+"' ");
                }
                strExcel = "select * from [学习及工作简历$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                DataTable dt_re = ds.Tables[0];
                for (int i = 0; i < dt_re.Rows.Count; i++)
                {
                    if (dt_re.Rows[i][1].ToString() != "")
                    {
                        List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                            new MySqlParameter("cName", dt_re.Rows[i][1].ToString()),          //姓名
                            new MySqlParameter("dBirth_date", dt_re.Rows[i][2].ToString()),    //出生日期
                            //new MySqlParameter("cIDnumber", dt_re.Rows[i][2].ToString()),    //身份证号码
                            new MySqlParameter("dStartDate", dt_re.Rows[i][3].ToString()),     //开始时间
                            new MySqlParameter("dDeadline", dt_re.Rows[i][4].ToString()),      //截至时间
                            new MySqlParameter("rLevel", dt_re.Rows[i][5].ToString()),         //工作单位经历
                            new MySqlParameter("cExperience", dt_re.Rows[i][6].ToString()),    //工作单位经历
                            new MySqlParameter("rRemarks", dt_re.Rows[i][7].ToString()),       //备注
                        };
                        MySqlParameter[] param = ilistStr.ToArray();
                        DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName=@cName AND dBirth_date LIKE '%" + dt_re.Rows[i][2].ToString() + "%'", param);
                        //DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cIDnumber=@cIDnumber", param);
                        if (dt_persion == null || dt_persion.Rows.Count <= 0)
                        {
                            XtraMessageBox.Show("请确保数据库中存在"+ dt_re.Rows[i][1].ToString() + "的信息，然后再插入相关的学习及工作简历");
                            return;
                            //break;
                        }
                        ilistStr.Add(new MySqlParameter("PersionID", dt_persion.Rows[0]["pid"].ToString()));
                        ilistStr.Add(new MySqlParameter("cIDnumber", dt_persion.Rows[0]["cIDnumber"].ToString()));
                        param = ilistStr.ToArray();
                        MySQLHelper.ExecuteNonQuery("INSERT INTO data_resume(PersionID,dStartDate,dDeadline,rLevel,cExperience,cIDnumber,rRemarks) VALUES(@PersionID,@dStartDate,@dDeadline,@rLevel,@cExperience,@cIDnumber,@rRemarks)", param);
                    }
                }

                #endregion

                #region 后备干部 data_reservecadre
                strExcel = "SELECT 本人姓名,出生年月 FROM [学习及工作简历$] WHERE len(本人姓名)>0  GROUP BY 本人姓名,出生年月";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_cl = ds.Tables[0];
                foreach (DataRow r in dt_cl.Rows)
                {
                    DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName='" + r["本人姓名"] + "' AND dBirth_date LIKE '%" + r["出生年月"].ToString() + "%'");
                    if (dt_persion == null || dt_persion.Rows.Count <= 0)
                    {
                        XtraMessageBox.Show("请确保数据库中存在" + r["本人姓名"].ToString() + "的信息，然后再插入相关的学习及工作简历");
                        return;
                        //break;
                    }
                    MySQLHelper.ExecuteNonQuery("DELETE FROM data_reservecadre WHERE PersionID='" + dt_persion.Rows[0]["pid"].ToString() + "' ");
                }
                strExcel = "select * from [后备干部$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_re = ds.Tables[0];
                for (int i = 0; i < dt_re.Rows.Count; i++)
                {
                    if (dt_re.Rows[i][1].ToString() != "")
                    {
                        List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                            new MySqlParameter("cName", dt_re.Rows[i][1].ToString()),          //姓名
                            new MySqlParameter("dBirth_date", dt_re.Rows[i][2].ToString()),    //身份证号码
                            //new MySqlParameter("cIDnumber", dt_re.Rows[i][2].ToString()),      //身份证号码
                            new MySqlParameter("rcYear", dt_re.Rows[i][3].ToString()),         //年度
                            new MySqlParameter("rcLevel", dt_re.Rows[i][4].ToString()),        //级别
                            new MySqlParameter("rcRemarks", dt_re.Rows[i][5].ToString()),      //备注
                        };
                        MySqlParameter[] param = ilistStr.ToArray();
                        DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName=@cName AND dBirth_date LIKE '%" + dt_re.Rows[i][2].ToString() + "%'", param);
                        //DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND  cIDnumber=@cIDnumber", param);
                        if (dt_persion == null || dt_persion.Rows.Count <= 0)
                        {
                            XtraMessageBox.Show("请确保数据库中存在" + dt_re.Rows[i][1].ToString() + "的信息，然后再插入相关的后备干部数据");
                            return;
                            //break;
                        }
                        ilistStr.Add(new MySqlParameter("PersionID", dt_persion.Rows[0]["pid"].ToString()));
                        ilistStr.Add(new MySqlParameter("cIDnumber", dt_persion.Rows[0]["cIDnumber"].ToString()));
                        param = ilistStr.ToArray();
                        MySQLHelper.ExecuteNonQuery("INSERT INTO data_reservecadre(PersionID,rcYear,rcLevel,cIDnumber,rcRemarks) VALUES(@PersionID,@rcYear,@rcLevel,@cIDnumber,@rcRemarks)", param);
                    }
                }
                #endregion
                #region 家庭主要成员 data_familymembers
                strExcel = "SELECT 本人姓名,出生年月 FROM [学习及工作简历$] WHERE len(本人姓名)>0  GROUP BY 本人姓名,出生年月";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_cl = ds.Tables[0];
                foreach (DataRow r in dt_cl.Rows)
                {
                    DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName='" + r["本人姓名"] + "' AND dBirth_date LIKE '%" + r["出生年月"].ToString() + "%'");
                    if (dt_persion == null || dt_persion.Rows.Count <= 0)
                    {
                        XtraMessageBox.Show("请确保数据库中存在" + r["本人姓名"].ToString() + "的信息，然后再插入相关的学习及工作简历");
                        return;
                        //break;
                    }
                    MySQLHelper.ExecuteNonQuery("DELETE FROM data_familymembers WHERE PersionID='" + dt_persion.Rows[0]["pid"].ToString() + "' ");
                }

                strExcel = "select * from [家庭主要成员$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_re = ds.Tables[0];
                for (int i = 0; i < dt_re.Rows.Count; i++)
                {
                    if (dt_re.Rows[i][1].ToString() != "")
                    {
                        List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                            new MySqlParameter("cName", dt_re.Rows[i][1].ToString()),               //姓名
                            new MySqlParameter("dBirth_date", dt_re.Rows[i][2].ToString()),         //出生日期
                            //new MySqlParameter("cIDnumber", dt_re.Rows[i][2].ToString()),           //身份证号码
                            new MySqlParameter("cfCalled", dt_re.Rows[i][3].ToString()),            //称谓
                            new MySqlParameter("cfName", dt_re.Rows[i][4].ToString()),              //名字
                            new MySqlParameter("dfBirthDate", dt_re.Rows[i][5].ToString()),         //出生年份
                            new MySqlParameter("cfPoliticalStatus", dt_re.Rows[i][6].ToString()),   //政治面貌
                            new MySqlParameter("cfDuties", dt_re.Rows[i][7].ToString()),            //工作单位及职务
                            
                        };
                        MySqlParameter[] param = ilistStr.ToArray();
                        DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName=@cName AND dBirth_date LIKE '%" + dt_re.Rows[i][2].ToString() + "%'", param);
                        //DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND  cIDnumber=@cIDnumber", param);
                        if (dt_persion == null || dt_persion.Rows.Count <= 0)
                        {
                            XtraMessageBox.Show("请确保数据库中存在" + dt_re.Rows[i][1].ToString() + "的信息，然后再插入相关的家庭主要成员数据");
                            return;
                            //break;
                        }
                        ilistStr.Add(new MySqlParameter("PersionID", dt_persion.Rows[0]["pid"].ToString()));
                        ilistStr.Add(new MySqlParameter("cIDnumber", dt_persion.Rows[0]["cIDnumber"].ToString()));
                        param = ilistStr.ToArray();
                        MySQLHelper.ExecuteNonQuery("INSERT INTO data_familymembers(PersionID,cfCalled,cfName,dfBirthDate,cfPoliticalStatus,cfDuties,cIDnumber) VALUES(@PersionID,@cfCalled,@cfName,@dfBirthDate,@cfPoliticalStatus,@cfDuties,@cIDnumber)", param);
                    }
                }
                #endregion
                #region 奖惩情况 data_rewards_punishments
                strExcel = "SELECT 本人姓名,出生年月 FROM [学习及工作简历$] WHERE len(本人姓名)>0  GROUP BY 本人姓名,出生年月";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_cl = ds.Tables[0];
                foreach (DataRow r in dt_cl.Rows)
                {
                    DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName='" + r["本人姓名"] + "' AND dBirth_date LIKE '%" + r["出生年月"].ToString() + "%'");
                    if (dt_persion == null || dt_persion.Rows.Count <= 0)
                    {
                        XtraMessageBox.Show("请确保数据库中存在" + r["本人姓名"].ToString() + "的信息，然后再插入相关的学习及工作简历");
                        return;
                        //break;
                    }
                    MySQLHelper.ExecuteNonQuery("DELETE FROM data_rewards_punishments WHERE PersionID='" + dt_persion.Rows[0]["pid"].ToString() + "' ");
                }

                strExcel = "select * from [奖惩情况$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_re = ds.Tables[0];
                for (int i = 0; i < dt_re.Rows.Count; i++)
                {
                    if (dt_re.Rows[i][1].ToString() != "")
                    {
                        List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                            new MySqlParameter("cName", dt_re.Rows[i][1].ToString()),               //姓名
                            new MySqlParameter("dBirth_date", dt_re.Rows[i][2].ToString()),         //出生日期
                            //new MySqlParameter("cIDnumber", dt_re.Rows[i][2].ToString()),           //身份证号码
                            new MySqlParameter("dData", dt_re.Rows[i][4].ToString()),               //时间
                            new MySqlParameter("cCategory", dt_re.Rows[i][3].ToString()),           //类别
                            new MySqlParameter("cLevel", dt_re.Rows[i][5].ToString()),              //级别
                            new MySqlParameter("cDetailed", dt_re.Rows[i][6].ToString()),           //奖惩内容
                            new MySqlParameter("rpRemarks", dt_re.Rows[i][7].ToString()),           //备注
                        };
                        MySqlParameter[] param = ilistStr.ToArray();
                        DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName=@cName AND dBirth_date LIKE '%" + dt_re.Rows[i][2].ToString() + "%'", param);
                        //DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND  cIDnumber=@cIDnumber", param);
                        if (dt_persion == null || dt_persion.Rows.Count <= 0)
                        {
                            XtraMessageBox.Show("请确保数据库中存在" + dt_re.Rows[i][1].ToString() + "的信息，然后再插入相关的奖惩情况数据");
                            return;
                            //break;
                        }
                        ilistStr.Add(new MySqlParameter("PersionID", dt_persion.Rows[0]["pid"].ToString()));
                        ilistStr.Add(new MySqlParameter("cIDnumber", dt_persion.Rows[0]["cIDnumber"].ToString()));
                        param = ilistStr.ToArray();
                        MySQLHelper.ExecuteNonQuery("INSERT INTO data_rewards_punishments(PersionID,dData,cCategory,cLevel,cDetailed,cIDnumber,rpRemarks) VALUES(@PersionID,@dData,@cCategory,@cLevel,@cDetailed,@cIDnumber,@rpRemarks)", param);
                    }
                }
                #endregion
                #region 年度考核 data_checkresult
                strExcel = "SELECT 本人姓名,出生年月 FROM [学习及工作简历$] WHERE len(本人姓名)>0  GROUP BY 本人姓名,出生年月";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_cl = ds.Tables[0];
                foreach (DataRow r in dt_cl.Rows)
                {
                    DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName='" + r["本人姓名"] + "' AND dBirth_date LIKE '%" + r["出生年月"].ToString() + "%'");
                    if (dt_persion == null || dt_persion.Rows.Count <= 0)
                    {
                        XtraMessageBox.Show("请确保数据库中存在" + r["本人姓名"].ToString() + "的信息，然后再插入相关的学习及工作简历");
                        return;
                        //break;
                    }
                    MySQLHelper.ExecuteNonQuery("DELETE FROM data_checkresult WHERE PersionID='" + dt_persion.Rows[0]["pid"].ToString() + "' ");
                }

                strExcel = "select * from [年度考核$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_re = ds.Tables[0];
                for (int i = 0; i < dt_re.Rows.Count; i++)
                {
                    if (dt_re.Rows[i][1].ToString() != "")
                    {
                        List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                            new MySqlParameter("cName", dt_re.Rows[i][1].ToString()),               //姓名
                            new MySqlParameter("dBirth_date", dt_re.Rows[i][2].ToString()),         //出生日期
                            //new MySqlParameter("cIDnumber", dt_re.Rows[i][2].ToString()),           //身份证号码
                            new MySqlParameter("dcrYear", dt_re.Rows[i][3].ToString()),             //考核年度
                            new MySqlParameter("crChechResult", dt_re.Rows[i][4].ToString()),       //类别
                            new MySqlParameter("crRemarks", dt_re.Rows[i][5].ToString()),           //类别
                        };
                        MySqlParameter[] param = ilistStr.ToArray();
                        DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName=@cName AND dBirth_date LIKE '%" + dt_re.Rows[i][2].ToString() + "%'", param);
                        //DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1  AND  cIDnumber=@cIDnumber", param);
                        if (dt_persion == null || dt_persion.Rows.Count <= 0)
                        {
                            XtraMessageBox.Show("请确保数据库中存在" + dt_re.Rows[i][1].ToString() + "的信息，然后再插入相关的年度考核数据");
                            return;
                            //break;
                        }
                        ilistStr.Add(new MySqlParameter("PersionID", dt_persion.Rows[0]["pid"].ToString()));
                        ilistStr.Add(new MySqlParameter("cIDnumber", dt_persion.Rows[0]["cIDnumber"].ToString()));
                        param = ilistStr.ToArray();
                        MySQLHelper.ExecuteNonQuery("INSERT INTO data_checkresult(PersionID,dcrYear,crChechResult,cIDnumber,crRemarks) VALUES(@PersionID,@dcrYear,@crChechResult,@cIDnumber,@crRemarks)", param);
                    }
                }
                #endregion
                #region 现实表现 data_performance
                strExcel = "SELECT 本人姓名,出生年月 FROM [学习及工作简历$] WHERE len(本人姓名)>0  GROUP BY 本人姓名,出生年月";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_cl = ds.Tables[0];
                foreach (DataRow r in dt_cl.Rows)
                {
                    DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName='" + r["本人姓名"] + "' AND dBirth_date LIKE '%" + r["出生年月"].ToString() + "%'");
                    if (dt_persion == null || dt_persion.Rows.Count <= 0)
                    {
                        XtraMessageBox.Show("请确保数据库中存在" + r["本人姓名"].ToString() + "的信息，然后再插入相关的学习及工作简历");
                        return;
                        //break;
                    }
                    MySQLHelper.ExecuteNonQuery("DELETE FROM data_performance WHERE PersionID='" + dt_persion.Rows[0]["pid"].ToString() + "' ");
                }

                strExcel = "select * from [现实表现$]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                dt_re = ds.Tables[0];
                for (int i = 0; i < dt_re.Rows.Count; i++)
                {
                    if (dt_re.Rows[i][1].ToString() != "")
                    {
                        List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                            new MySqlParameter("cName", dt_re.Rows[i][1].ToString()),                   //姓名
                            new MySqlParameter("dBirth_date", dt_re.Rows[i][2].ToString()),             //出生日期
                            //new MySqlParameter("cIDnumber", dt_re.Rows[i][2].ToString()),               //身份证号码
                            new MySqlParameter("rpYear", dt_re.Rows[i][3].ToString()),                  //年度
                            new MySqlParameter("cSelfEvaluation", dt_re.Rows[i][4].ToString()),         //自我评价
                            new MySqlParameter("cUnitEvaluation", dt_re.Rows[i][5].ToString()),         //一把手评价
                            new MySqlParameter("cOrganizationEvaluation", dt_re.Rows[i][6].ToString()), //组织部评价
                            new MySqlParameter("prRemarks", dt_re.Rows[i][7].ToString()), //组织部评价
                        };
                        MySqlParameter[] param = ilistStr.ToArray();
                        DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND cName=@cName AND dBirth_date LIKE '%" + dt_re.Rows[i][2].ToString() + "%'", param);
                        //DataTable dt_persion = MySQLHelper.table("SELECT * FROM data_persion WHERE do_flag=1 AND  cIDnumber=@cIDnumber", param);
                        if (dt_persion == null || dt_persion.Rows.Count <= 0)
                        {
                            XtraMessageBox.Show("请确保数据库中存在" + dt_re.Rows[i][1].ToString() + "的信息，然后再插入相关的现实表现数据");
                            return;
                            //break;
                        }
                        ilistStr.Add(new MySqlParameter("PersionID", dt_persion.Rows[0]["pid"].ToString()));
                        ilistStr.Add(new MySqlParameter("cIDnumber", dt_persion.Rows[0]["cIDnumber"].ToString()));
                        param = ilistStr.ToArray();
                        MySQLHelper.ExecuteNonQuery("INSERT INTO data_performance(PersionID,rpYear,cSelfEvaluation,cUnitEvaluation,cOrganizationEvaluation,cIDnumber,prRemarks) VALUES(@PersionID,@rpYear,@cSelfEvaluation,@cUnitEvaluation,@cOrganizationEvaluation,@cIDnumber,@prRemarks)", param);
                    }
                }
                #endregion

                this.gcType.DataSource = fromDTcolumns(dt);
                tatal= dt.Rows.Count;
                //success = (gcType.DataSource as DataTable).Rows.Count;
                txtTotal.Text = tatal.ToString();
                txtSuccess.Text = success.ToString();
                txtfail.Text = (tatal- success).ToString();
                return;
            }
            catch (Exception err)
            {
                XtraMessageBox.Show(err.Message);
            }
        }
        /// <summary>
        /// 格式化年月
        /// </summary>
        /// <param name="s">输入年月字符</param>
        /// <param name="c">输入分隔符</param>
        /// <returns>返回为yyyy.MM</returns>
        private string fromDate(string s,char c,string f= "yyyy.MM")
        {
            if (s == "")
                return "";
            if (s.Split(c).Length > 1)
            {
                string date = "";
                if (s.Split(c)[0].Length >= 4)
                {
                    date = s;
                    if (date.Split(c)[1].Length <= 1)
                        date += "0";
                    return date;
                }
                else if (s.Split(c)[0].Length <= 1)
                {
                    date = "0" + s.Split(c)[0];
                }
                else
                {
                    date = s.Split(c)[0];
                }
                
                if (Convert.ToDecimal("20" + date) > DateTime.Now.Year)
                    date = "19" + date + c + s.Split(c)[1];
                else
                    date = "20" + date + c + s.Split(c)[1];


                if (date.Split(c)[1].Length <= 1)
                    date += "0";

                return Convert.ToDateTime(date).ToString(f);
            }
            else { XtraMessageBox.Show("时间格式错误，请填写yyyy"+c+"MM格式或者yy"+c+"MM格式的时间");return ""; }

        }

        private DataTable fromDTcolumns(DataTable dt)
        {
            dt.Columns[2].ColumnName = "cUnit";
            dt.Columns[3].ColumnName = "cName";
            dt.Columns[4].ColumnName = "cCurrentJob";
            dt.Columns[5].ColumnName = "cSex";
            dt.Columns[6].ColumnName = "cNation";
            dt.Columns[7].ColumnName = "cNativePlace";
            dt.Columns[8].ColumnName = "dBirth_date";
            dt.Columns[9].ColumnName = "dJoin_date";
            dt.Columns[10].ColumnName = "dWorkDate";
            dt.Columns[11].ColumnName = "cFull_timeEducation";
            //dt.Columns[12].ColumnName = "cFull_timeSchool";
            dt.Columns[15].ColumnName = "cIn_serviceEducation";
            dt.Columns[17].ColumnName = "cIn_serviceSchool";
            dt.Columns[19].ColumnName = "cResume";
            dt.Columns[20].ColumnName = "dInOffice";
            dt.Columns[21].ColumnName = "dSameOffic";
            dt.Columns[27].ColumnName = "cHealthStatus";
            dt.Columns[28].ColumnName = "cDuties";
            dt.Columns[30].ColumnName = "cIdentityCategory";
            dt.Columns[35].ColumnName = "cRemarks";

            return dt;
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SaveFileDialog sfExcel = new SaveFileDialog();
            sfExcel.Filter = "Excel 97-2013 工作簿(*.xls)|*.xls";
            sfExcel.FileName = "模版";
            if (sfExcel.ShowDialog()==DialogResult.OK)
            {
                if (sfExcel.FileName.Trim().Length > 0)
                {
                    byte[] excel = Properties.Resources.模版;
                    FileStream stream = new FileStream(sfExcel.FileName,FileMode.Create);
                    stream.Write(excel, 0, excel.Length);
                    stream.Close();
                    stream.Dispose();
                    XtraMessageBox.Show("保存成功","提示");
                }
            }
        }

        private DataTable readXlsExcel()
        {
            string Path = "";
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = schemaTable.Rows[0][2].ToString().Trim();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            //strExcel = "select * from [sheet1$]";
            strExcel = "select * from [" + tableName + "]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            DataTable dt = ds.Tables[0];
            return dt;
        }

        private void gvType_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            ShowCount.DrawRowIndicator(gvType,30);
        }
   
    }
}

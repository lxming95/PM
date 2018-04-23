using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using MySql.Data.MySqlClient;
using BarCode;

namespace PersonnelManagement
{
    public partial class FrmInfMain : DevExpress.XtraEditors.XtraForm
    {
        //人员的主表信息
        public DataTable gl_dt = new DataTable();
        private bool isAdd = false;
        private bool issave = false;
        private bool isEnableEdit = true;
        public FrmInfMain()
        {
            InitializeComponent();
            formatDateEdit();
            start();
            isAdd = true;
        }
        public FrmInfMain(string id,bool flag=true)
        {
            InitializeComponent();
            formatDateEdit();
            isAdd = false;
            formatDateEdit();
            start(id);
            isEnableEdit = flag;
            enableEdit(isEnableEdit);

        }
        private void FrmInfMain_Load(object sender, EventArgs e)
        {
            formatDateEdit();
            //txtcSex.Text = "男";
        }
        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DialogResult.Yes == XtraMessageBox.Show("是否退出,未保存的数据将会丢失", "提示", MessageBoxButtons.YesNo))
            {
                //关闭窗体
                this.Close();

            }
        }
        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bool su=saveIteam();
            if (su)
            {
                issave = true;
                if (this.Parent != null)
                {
                    if (DialogResult.Yes == XtraMessageBox.Show("是否继续添加？", "提示", MessageBoxButtons.YesNo) && isAdd)
                    {
                        clear();
                    }
                }
                else
                {
                    if (DialogResult.Yes == XtraMessageBox.Show("保存成功！是否关闭本页面？", "提示", MessageBoxButtons.YesNo))
                    {
                        this.Close();
                    }
                    else
                    {
                        issave = false;
                    }
                }
            }

        }
        /// <summary>
        /// 导出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnSave_ItemClick(sender, e);
            List<MySqlParameter> ilistStr = new List<MySqlParameter> {
            new MySqlParameter("cName", txtcName.Text),
            new MySqlParameter("dBirth_date", dBirth_date.Text),
            };
            MySqlParameter[] param = ilistStr.ToArray();
            DataTable dt = MySQLHelper.table("SELECT * FROM data_persion WHERE cName=@cName AND dBirth_date=@dBirth_date ", param);
            SaveFile.ExportLrm(dt);
        }
        /// <summary>
        /// 清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //saveIteam();
            //isAdd = true;
            clear();

        }
        /// <summary>
        /// 不传值的初始化函数
        /// </summary>
        private void start()
        {
            gcResume.DataSource= MySQLHelper.table("SELECT * FROM data_resume  where rid in ('')");
            gcRewards.DataSource = MySQLHelper.table("SELECT * FROM data_rewards_punishments  where rpid in ('')");
            gcFamilymembers.DataSource = MySQLHelper.table("SELECT * FROM data_familymembers  where fid in ('')");
            gcCheckResult.DataSource= MySQLHelper.table("SELECT * FROM data_checkresult  where crid in ('')");
            gcReserveCadre.DataSource = MySQLHelper.table("SELECT * FROM data_reservecadre  where rcid in ('')");
            //gcGetCadres.DataSource = MySQLHelper.table("SELECT * FROM data_getcadres  where gcid in ('')");
            gcEmployment.DataSource = MySQLHelper.table("SELECT * FROM data_employment  where eid in ('')");
            gcPerformance.DataSource = MySQLHelper.table("SELECT * FROM data_performance  where prid in ('')");

            #region 显示数据
            dMakeDate.Text = DateTime.Now.ToString("yyyy.MM.dd");       //制表时间
            txtcMaker.Text = Pub.PubValue.UserName;                     //制表人
            #endregion

        }
        /// <summary>
        /// 传值的初始化函数
        /// </summary>
        /// <param name="id"></param>
        private void start(string id)
        {
            List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("pid", id),
            };
            MySqlParameter[] parm = ilistStr.ToArray();
            DataTable dt = MySQLHelper.table("select * from data_persion where pid= @pid", parm);

            if (dt == null || dt.Rows.Count <= 0)
            {
                XtraMessageBox.Show("无该数据，请关闭后，刷新列表重试");
                return;
            }
            gl_dt = dt;
            //gcresume.DataSource = dt;

            #region   显示数据
            gcResume.DataSource = MySQLHelper.table("SELECT * FROM data_resume where PersionID= @pid", parm);
            gcRewards.DataSource = MySQLHelper.table("select * from data_rewards_punishments where PersionID= @pid", parm);
            gcFamilymembers.DataSource = MySQLHelper.table("select * from data_familymembers where PersionID= @pid", parm);

            gcCheckResult.DataSource = MySQLHelper.table("SELECT * FROM data_checkresult  where PersionID= @pid", parm);
            gcReserveCadre.DataSource = MySQLHelper.table("SELECT * FROM data_reservecadre  where PersionID= @pid", parm);
            //gcGetCadres.DataSource = MySQLHelper.table("SELECT * FROM data_getcadres  where PersionID= @pid", parm);
            gcEmployment.DataSource = MySQLHelper.table("SELECT * FROM data_employment  where PersionID= @pid", parm);
            gcPerformance.DataSource = MySQLHelper.table("SELECT * FROM data_performance  where PersionID= @pid", parm);

            txtcName.Text = gl_dt.Rows[0]["cName"].ToString();                          //姓名
            //txtcSex.EditValue = gl_dt.Rows[0]["cSex"];                                //
            txtcSex.Text = gl_dt.Rows[0]["cSex"].ToString();                            //性别
            dBirth_date.Text = gl_dt.Rows[0]["dBirth_date"].ToString();                 //出生年月
            txtcNation.Text = gl_dt.Rows[0]["cNation"].ToString();                      //民族
            txtcNativePlace.Text = gl_dt.Rows[0]["cNativePlace"].ToString();            //籍贯
            txtcIDnumber.Text= gl_dt.Rows[0]["cIDnumber"].ToString();                   //身份证号码
            dJoin_date.Text = gl_dt.Rows[0]["dJoin_date"].ToString();                   //入党时间
            txtcHealthStatus.Text = gl_dt.Rows[0]["cHealthStatus"].ToString();          //健康状况
            txtcBirthPlace.Text = gl_dt.Rows[0]["cBirthPlace"].ToString();              //出生地
            dWorkDate.Text = gl_dt.Rows[0]["dWorkDate"].ToString();                     //参加工作时间
            txtcDuties.Text = gl_dt.Rows[0]["cDuties"].ToString();                      //专业技术职务
            txtcSkill.Text = gl_dt.Rows[0]["cSkill"].ToString();                        //专业特长
            txtcFull_timeEducation.Text = gl_dt.Rows[0]["cFull_timeEducation"].ToString();//全日制教育学历
            txtcFull_timeDegree.Text = gl_dt.Rows[0]["cFull_timeDegree"].ToString();    //全日制学位
            txtcFull_timeMajor.Text = gl_dt.Rows[0]["cFull_timeMajor"].ToString();      //全日制教育专业
            txtcFull_timeSchool.Text = gl_dt.Rows[0]["cFull_timeSchool"].ToString();    //全日制毕业院校
            txtcIn_serviceEducation.Text = gl_dt.Rows[0]["cIn_serviceEducation"].ToString();//在职教育学历
            txtcIn_serviceDegree.Text = gl_dt.Rows[0]["cIn_serviceDegree"].ToString();  //在职教育学位
            txtcIn_serviceMajor.Text = gl_dt.Rows[0]["cIn_serviceMajor"].ToString();    //在职教育专业
            txtcIn_serviceSchool.Text = gl_dt.Rows[0]["cIn_serviceSchool"].ToString();  //在职教育毕业院校
            txtcCurrentJob.Text = gl_dt.Rows[0]["cCurrentJob"].ToString();              //现任职务
            txtcProposedJob.Text = gl_dt.Rows[0]["cProposedJob"].ToString();            //拟任职务
            txtcRemoveJob.Text = gl_dt.Rows[0]["cRemoveJob"].ToString();                //拟免职务
            //txtResume.Text = gl_dt.Rows[0]["cResume"].ToString();                       //工作经历
            //txtcChech_Result.Text = gl_dt.Rows[0]["cChech_Result"].ToString();        //审核结果
            txtcDismissalReason.Text = gl_dt.Rows[0]["cDismissalReason"].ToString();    //任免理由
            txtcReporting_Unit.Text = gl_dt.Rows[0]["cReporting_Unit"].ToString();      //呈报单位
            dEageDate.Text = gl_dt.Rows[0]["dEageDate"].ToString();                     //基准时间
            txtcMaker.Text = gl_dt.Rows[0]["cMaker"].ToString();                        //制表人
            dMakeDate.Text = gl_dt.Rows[0]["dMakeDate"].ToString();                     //制表时间
            txtIdentityCategory.Text = gl_dt.Rows[0]["cIdentityCategory"].ToString();   //身份类别
            txtRank.Text = gl_dt.Rows[0]["cRank"].ToString();                           //个人所属职级
            dDate4.Text= gl_dt.Rows[0]["dGetCadresDate"].ToString();                    //干部身份取得时间
            txtcDocumentBasis.Text = gl_dt.Rows[0]["cDocumentBasis"].ToString();        //干部身份取得文件依据
            txtcApprovingAuthority.Text = gl_dt.Rows[0]["cApprovingAuthority"].ToString();//干部身份取得审批机关
            txtcWay.Text = gl_dt.Rows[0]["cWay"].ToString();                            //干部身份取得方式


            if (gl_dt.Rows[0]["bIsNative"].Equals("1"))
            {
                cbIsNative.Checked = true;
            }
            else
            {
                cbIsNative.Checked = false;
            }
            if (gl_dt.Rows[0]["bIsOfficialPosition"].Equals("1"))
            {
                cbIsOfficialPosition.Checked = true;
            }
            else
            {
                cbIsOfficialPosition.Checked = false;
            }

            #endregion


        }
        /// <summary>
        /// 清空界面
        /// </summary>
        private void clear()
        {
            #region clear balnk
            txtcName.Text = "";
            txtcSex.Text = "";
            dBirth_date.Text = "";
            txtcNation.Text = "";
            txtcNativePlace.Text = "";
            txtcIDnumber.Text = "";
            dJoin_date.Text = "";
            txtcHealthStatus.Text = "";
            txtcBirthPlace.Text = "";
            dWorkDate.Text = "";
            txtcDuties.Text = "";
            txtcSkill.Text = "";
            txtcFull_timeEducation.Text = "";
            txtcFull_timeDegree.Text = "";
            txtcFull_timeSchool.Text = "";
            txtcFull_timeMajor.Text = "";
            txtcIn_serviceEducation.Text = "";
            txtcIn_serviceDegree.Text = "";
            txtcIn_serviceSchool.Text = "";
            txtcIn_serviceMajor.Text = "";
            txtcCurrentJob.Text = "";
            txtcProposedJob.Text = "";
            txtcRemoveJob.Text = "";
            //txtcChech_Result.Text = "";
            txtcDismissalReason.Text = "";
            txtcReporting_Unit.Text = "";

            dEageDate.Text = "";
            txtcMaker.Text = Pub.PubValue.UserName;
            dMakeDate.Text = "";

            txtIdentityCategory.Text = "";
            txtRank.Text = "";

            dDate4.Text = "";
            txtcDocumentBasis.Text = "";
            txtcApprovingAuthority.Text = "";
            txtcWay.Text = "";
            cbIsNative.Checked = false;
            cbIsOfficialPosition.Checked = false;
            #endregion
            #region clear all excel
            DataTable dt_clear = new DataTable();
            if (!dt_clear.Columns.Contains("PersionID"))
                dt_clear.Columns.Add("PersionID", typeof(string));
            if (!dt_clear.Columns.Contains("dStartDate"))
                dt_clear.Columns.Add("dStartDate", typeof(string));
            if (!dt_clear.Columns.Contains("dStartDate"))
                dt_clear.Columns.Add("dStartDate", typeof(string));
            if (!dt_clear.Columns.Contains("dDeadline"))
                dt_clear.Columns.Add("dDeadline", typeof(string));
            if (!dt_clear.Columns.Contains("cExperience"))
                dt_clear.Columns.Add("cExperience", typeof(string));
            gcResume.DataSource = dt_clear;

            if (!dt_clear.Columns.Contains("dData"))
                dt_clear.Columns.Add("dData", typeof(string));
            if (!dt_clear.Columns.Contains("cCategory"))
                dt_clear.Columns.Add("cCategory", typeof(string));
            if (!dt_clear.Columns.Contains("cLevel"))
                dt_clear.Columns.Add("cLevel", typeof(string));
            if (!dt_clear.Columns.Contains("cDetailed"))
                dt_clear.Columns.Add("cDetailed", typeof(string));
            gcRewards.DataSource = dt_clear;

            if (!dt_clear.Columns.Contains("cfCalled"))
                dt_clear.Columns.Add("cfCalled", typeof(string));
            if (!dt_clear.Columns.Contains("cfName"))
                dt_clear.Columns.Add("cfName", typeof(string));
            if (!dt_clear.Columns.Contains("dfBirthDate"))
                dt_clear.Columns.Add("dfBirthDate", typeof(string));
            if (!dt_clear.Columns.Contains("cfPoliticalStatus"))
                dt_clear.Columns.Add("cfPoliticalStatus", typeof(string));
            if (!dt_clear.Columns.Contains("cfDuties"))
                dt_clear.Columns.Add("cfDuties", typeof(string));
            gcFamilymembers.DataSource = dt_clear;

            if (!dt_clear.Columns.Contains("dcrYear"))
                dt_clear.Columns.Add("dcrYear", typeof(string));
            if (!dt_clear.Columns.Contains("crChechResult"))
                dt_clear.Columns.Add("crChechResult", typeof(string));
            gcCheckResult.DataSource = dt_clear;

            if (!dt_clear.Columns.Contains("rcYear"))
                dt_clear.Columns.Add("rcYear", typeof(string));
            if (!dt_clear.Columns.Contains("rcLevel"))
                dt_clear.Columns.Add("rcLevel", typeof(string));
            gcReserveCadre.DataSource = dt_clear;

            //if (!dt_clear.Columns.Contains("gcData"))
            //    dt_clear.Columns.Add("gcData", typeof(string));
            //if (!dt_clear.Columns.Contains("gcDocumentBasis"))
            //    dt_clear.Columns.Add("gcDocumentBasis", typeof(string));
            //if (!dt_clear.Columns.Contains("gcApprovingAuthority"))
            //    dt_clear.Columns.Add("gcApprovingAuthority", typeof(string));
            //if (!dt_clear.Columns.Contains("gcWay"))
            //    dt_clear.Columns.Add("gcWay", typeof(string));
            //gcGetCadres.DataSource = dt_clear;

            if (!dt_clear.Columns.Contains("eDate"))
                dt_clear.Columns.Add("eDate", typeof(string));
            if (!dt_clear.Columns.Contains("eWay"))
                dt_clear.Columns.Add("eWay", typeof(string));
            gcEmployment.DataSource = dt_clear;

            if (!dt_clear.Columns.Contains("rpYear"))
                dt_clear.Columns.Add("rpYear", typeof(string));
            if (!dt_clear.Columns.Contains("cSelfEvaluation"))
                dt_clear.Columns.Add("cSelfEvaluation", typeof(string));
            if (!dt_clear.Columns.Contains("cUnitEvaluation"))
                dt_clear.Columns.Add("cUnitEvaluation", typeof(string));
            if (!dt_clear.Columns.Contains("cOrganizationEvaluation"))
                dt_clear.Columns.Add("cOrganizationEvaluation", typeof(string));
            gcPerformance.DataSource = dt_clear;

            #endregion

        }
        
        /// <summary>
        ///  保存数据
        /// </summary>
        private bool saveIteam()
        {
            bool su = false;
            if (txtcName.Text != ""&&dBirth_date.Text!="")
            {
                #region  制造数据集合
                List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("cName", txtcName.Text),
                    new MySqlParameter("cSex", txtcSex.Text),
                    new MySqlParameter("dBirth_date", dBirth_date.Text),
                    new MySqlParameter("cNation", txtcNation.Text),
                    new MySqlParameter("cNativePlace", txtcNativePlace.Text),
                    new MySqlParameter("cIDnumber", txtcIDnumber.Text),
                    new MySqlParameter("dJoin_date", dJoin_date.Text),
                    new MySqlParameter("cHealthStatus", txtcHealthStatus.Text),
                    new MySqlParameter("cBirthPlace", txtcBirthPlace.Text),
                    new MySqlParameter("dWorkDate", dWorkDate.Text),
                    new MySqlParameter("cDuties", txtcDuties.Text),
                    new MySqlParameter("cSkill", txtcSkill.Text),
                    new MySqlParameter("cFull_timeEducation", txtcFull_timeEducation.Text),
                    new MySqlParameter("cFull_timeSchool", txtcFull_timeSchool.Text),
                    new MySqlParameter("cIn_serviceEducation", txtcIn_serviceEducation.Text),
                    new MySqlParameter("cIn_serviceSchool", txtcIn_serviceSchool.Text),
                    new MySqlParameter("cCurrentJob", txtcCurrentJob.Text),
                    new MySqlParameter("cProposedJob", txtcProposedJob.Text),
                    new MySqlParameter("cRemoveJob", txtcRemoveJob.Text),
                    //new MySqlParameter("cResume", txtResume.Text),

                    new MySqlParameter("cIdentityCategory", txtIdentityCategory.Text),
                    new MySqlParameter("cRank", txtRank.Text),

                    //new MySqlParameter("cName", gcRewards.DataSource = dt;
                    //new MySqlParameter("cName", gcFamilymembers.DataSource = dt;
                    //new MySqlParameter("cChech_Result", txtcChech_Result.Text),
                    new MySqlParameter("cDismissalReason", txtcDismissalReason.Text),
                    new MySqlParameter("cReporting_Unit", txtcReporting_Unit.Text),
                    new MySqlParameter("dEageDate", txtcReporting_Unit.Text),
                    new MySqlParameter("cMaker", txtcMaker.Text),
                    new MySqlParameter("dMakeDate", dMakeDate.Text),
                    new MySqlParameter("do_flag", "1"),
                    //11-15 reedit

                    new MySqlParameter("cFull_timeDegree", txtcFull_timeDegree.Text),   //全日制学位
                    new MySqlParameter("cFull_timeMajor", txtcFull_timeMajor.Text),     //全日制专业
                    new MySqlParameter("cIn_serviceDegree", txtcIn_serviceDegree.Text), //在职学位
                    new MySqlParameter("cIn_serviceMajor", txtcIn_serviceMajor.Text),   //在职专业

                    new MySqlParameter("dGetCadresDate", dDate4.Text),   //干部身份取得时间
                    new MySqlParameter("cDocumentBasis", txtcDocumentBasis.Text),     //文件依据
                    new MySqlParameter("cApprovingAuthority", txtcApprovingAuthority.Text), //审核单位
                    new MySqlParameter("cWay", txtcWay.Text),   //途径
                    };
                if (cbIsNative.Checked == true)
                {
                    ilistStr.Add(new MySqlParameter("bIsNative", "1"));
                }
                else
                {
                    ilistStr.Add(new MySqlParameter("bIsNative", "0"));
                }
                if (cbIsOfficialPosition.Checked == true)
                {
                    ilistStr.Add(new MySqlParameter("bIsOfficialPosition", "1"));
                }
                else
                {
                    ilistStr.Add(new MySqlParameter("bIsOfficialPosition", "0"));
                }
                gvResume.CloseEditor();
                gvResume.UpdateCurrentRow();
                gvRewards.CloseEditor();
                gvRewards.UpdateCurrentRow();
                gvFamilymembers.CloseEditor();
                gvFamilymembers.UpdateCurrentRow();
                gvCheckResult.CloseEditor();
                gvCheckResult.UpdateCurrentRow();
                gvReserveCadre.CloseEditor();
                gvReserveCadre.UpdateCurrentRow();
                //gvGetCadres.CloseEditor();
                //gvGetCadres.UpdateCurrentRow();
                gvEmployment.CloseEditor();
                gvEmployment.UpdateCurrentRow();
                gvPerformance.CloseEditor();
                gvPerformance.UpdateCurrentRow();


                DataTable dt_Resume = gcResume.DataSource as DataTable;             //工作经历
                DataTable dt_Rewards = gcRewards.DataSource as DataTable;           //奖惩
                DataTable dt_Family = gcFamilymembers.DataSource as DataTable;      //家庭成员
                DataTable dt_CheckResult = gcCheckResult.DataSource as DataTable;   //年度审核
                DataTable dt_ReserveCadre = gcReserveCadre.DataSource as DataTable; //后备干部
                //DataTable dt_GetCadres = gcGetCadres.DataSource as DataTable;       //干部身份取得
                DataTable dt_Employment = gcEmployment.DataSource as DataTable;     //录用情况
                DataTable dt_Performance = gcPerformance.DataSource as DataTable;   //评价
                MySqlParameter[] param = ilistStr.ToArray();
                #endregion
                #region base data
                if (isAdd)
                {

                    //MySQLHelper.ExecuteNonQuery(" INSERT INTO data_persion(cName,cSex,dWorkDate) VALUES(@cName,@cSex,@dWorkDate)", param);
                    MySQLHelper.ExecuteNonQuery(" INSERT INTO data_persion( cIdentityCategory,cRank,cName,cSex,dBirth_date,cNation,cNativePlace,cIDnumber,dJoin_date,cHealthStatus, cBirthPlace, dWorkDate, cDuties, cSkill, cFull_timeEducation, cFull_timeSchool, cIn_serviceEducation, cIn_serviceSchool,cCurrentJob, cProposedJob, cRemoveJob, cDismissalReason, cReporting_Unit, dEageDate, dMakeDate, cMaker, do_flag,bIsNative,bIsOfficialPosition,cFull_timeDegree,cFull_timeMajor,cIn_serviceDegree,cIn_serviceMajor,dGetCadresDate,cDocumentBasis,cApprovingAuthority,cWay ) VALUES( @cIdentityCategory,@cRank, @cName , @cSex , @dBirth_date , @cNation,@cNativePlace,@cIDnumber,@dJoin_date,@cHealthStatus, @cBirthPlace, @dWorkDate, @cDuties, @cSkill, @cFull_timeEducation, @cFull_timeSchool, @cIn_serviceEducation, @cIn_serviceSchool,@cCurrentJob, @cProposedJob, @cRemoveJob, @cDismissalReason, @cReporting_Unit, @dEageDate, @dMakeDate, @cMaker, @do_flag, @bIsNative,@bIsOfficialPosition,@cFull_timeDegree,@cFull_timeMajor,@cIn_serviceDegree,@cIn_serviceMajor,@dGetCadresDate,@cDocumentBasis,@cApprovingAuthority,@cWay )", param);
                }
                else
                {

                    if (gl_dt != null && gl_dt.Rows.Count > 0)
                    {
                        ilistStr.Add(new MySqlParameter("pid", gl_dt.Rows[0]["pid"].ToString()));
                        param = ilistStr.ToArray();
                        MySQLHelper.ExecuteNonQuery(" UPDATE data_persion SET cIDnumber=@cIDnumber,cIdentityCategory=@cIdentityCategory,cRank=@cRank,cName=@cName,cSex=@cSex,dBirth_date=@dBirth_date,cNation=@cNation,cNativePlace=@cNativePlace,dJoin_date=@dJoin_date,cHealthStatus=@cHealthStatus,cBirthPlace=@cBirthPlace,dWorkDate=@dWorkDate,cDuties=@cDuties,cSkill=@cSkill,cFull_timeEducation=@cFull_timeEducation,cFull_timeSchool=@cFull_timeSchool,cIn_serviceEducation=@cIn_serviceEducation,cIn_serviceSchool=@cIn_serviceSchool, cCurrentJob=@cCurrentJob,cProposedJob=@cProposedJob,cRemoveJob=@cRemoveJob,cDismissalReason=@cDismissalReason,cReporting_Unit=@cReporting_Unit,dEageDate=@dEageDate,dMakeDate=@dMakeDate,cMaker=@cMaker,do_flag=@do_flag,bIsNative=@bIsNative,bIsOfficialPosition=@bIsOfficialPosition,cFull_timeDegree=@cFull_timeDegree,cFull_timeMajor=@cFull_timeMajor,cIn_serviceDegree=@cIn_serviceDegree,cIn_serviceMajor=@cIn_serviceMajor,dGetCadresDate=@dGetCadresDate,cDocumentBasis=@cDocumentBasis,cApprovingAuthority=@cApprovingAuthority,cWay=@cWay WHERE pid=@pid", param);
                    }
                    else
                    {
                        XtraMessageBox.Show("没有查询到数据，请刷新重试");
                    }
                }
                DataTable dt_pid = new DataTable();
                if (dBirth_date.Text == "")
                {
                    dt_pid = MySQLHelper.table(" SELECT pid FROM data_persion WHERE cName = @cName", param);
                }
                else
                { dt_pid = MySQLHelper.table(" SELECT pid FROM data_persion WHERE cName = @cName AND dBirth_date LIKE '%" + Convert.ToDateTime(dBirth_date.Text).ToString("yy.MM") + "%'", param); }
                #endregion

                #region table Resume
                if (dt_Resume.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Resume.Rows.Count; i++)
                    {
                        if (dt_Resume.Columns.Contains("PersionID") && dt_Resume.Rows[i]["PersionID"].ToString() != "" && dt_Resume.Columns.Contains("rid"))
                        {
                            //UPDATE Rewards
                                List<MySqlParameter> tableStr = new List<MySqlParameter> {
                                    new MySqlParameter("dStartDate", dt_Resume.Rows[i]["dStartDate"].ToString()),
                                    new MySqlParameter("dDeadline", dt_Resume.Rows[i]["dDeadline"].ToString()),
                                    new MySqlParameter("rLevel", dt_Resume.Rows[i]["rLevel"].ToString()),
                                    new MySqlParameter("cExperience", dt_Resume.Rows[i]["cExperience"].ToString()),
                                    new MySqlParameter("rid", dt_Resume.Rows[i]["rid"].ToString()),
                                    };
                                MySqlParameter[] tableParam = tableStr.ToArray();
                                MySQLHelper.ExecuteNonQuery("UPDATE data_resume SET dStartDate=@dStartDate ,dDeadline=@dDeadline,rLevel=@rLevel ,cExperience=@cExperience WHERE rid=@rid ", tableParam);
                            }
                            else
                            {
                                //INSERT Rewards
                                MySqlParameter[] tableParam = new MySqlParameter[]  {
                                    new MySqlParameter("dStartDate", dt_Resume.Rows[i]["dStartDate"].ToString()),
                                    new MySqlParameter("dDeadline", dt_Resume.Rows[i]["dDeadline"].ToString()),
                                    new MySqlParameter("rLevel", dt_Resume.Rows[i]["cExperience"].ToString()),
                                    new MySqlParameter("cExperience", dt_Resume.Rows[i]["cExperience"].ToString()),
                                    new MySqlParameter("pid", dt_pid.Rows[0]["pid"].ToString()),
                                    };
                                MySQLHelper.ExecuteNonQuery("INSERT INTO data_resume (PersionID,dStartDate,dDeadline,rLevel,cExperience) VALUES(@pid,@dStartDate,@dDeadline,@rLevel,@cExperience )", tableParam);

                            }
                        }
                        
                }

                #endregion

                // 11-11 re-edit
                #region table Rewards

                if (dt_Rewards.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Rewards.Rows.Count; i++)
                    {
                        if (dt_Rewards.Columns.Contains("PersionID") && dt_Rewards.Rows[i]["PersionID"].ToString() != "" && dt_Rewards.Columns.Contains("rpid"))
                        {
                            //UPDATE Rewards
                            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                                new MySqlParameter("dData", dt_Rewards.Rows[i]["dData"].ToString()),
                                new MySqlParameter("cDetailed", dt_Rewards.Rows[i]["cDetailed"].ToString()),
                                new MySqlParameter("rpid", dt_Rewards.Rows[i]["rpid"].ToString()),
                                new MySqlParameter("cCategory", dt_Rewards.Rows[i]["cCategory"].ToString()),
                                new MySqlParameter("cLevel", dt_Rewards.Rows[i]["cLevel"].ToString()),
                                };
                            MySqlParameter[] tableParam = tableStr.ToArray();
                            MySQLHelper.ExecuteNonQuery("UPDATE data_rewards_punishments SET dData=@dData ,cDetailed=@cDetailed ,cCategory=@cCategory ,cLevel=@cLevel WHERE rpid=@rpid ", tableParam);
                        }

                        else
                        {
                            //INSERT Rewards
                            MySqlParameter[] tableParam = new MySqlParameter[]  {
                                new MySqlParameter("dData", Convert.ToDateTime(dt_Rewards.Rows[i]["dData"]).ToString("yyyy.MM")),
                                new MySqlParameter("cDetailed", Convert.ToDateTime(dt_Rewards.Rows[i]["cDetailed"]).ToString("yyyy.MM")),
                                new MySqlParameter("cCategory", dt_Rewards.Rows[i]["cCategory"].ToString()),
                                new MySqlParameter("cLevel", dt_Rewards.Rows[i]["cLevel"].ToString()),
                                new MySqlParameter("pid", dt_pid.Rows[0]["pid"].ToString()),
                                };
                                MySQLHelper.ExecuteNonQuery("INSERT INTO data_rewards_punishments (PersionID,dData,cCategory,cLevel,cDetailed) VALUES(@pid,@dData,@cCategory,@cLevel,@cDetailed )", tableParam);
                            }
                        }
                    }
                #endregion

                #region  table Family
                if (dt_Family.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Family.Rows.Count; i++)
                    {
                        if (dt_Family.Columns.Contains("PersionID") &&dt_Family.Rows[i]["PersionID"].ToString() != "" && dt_Family.Columns.Contains("fid"))
                    {
                        // Update family
                        
                        MySqlParameter[] tableParam = new MySqlParameter[] {
                            new MySqlParameter("cfCalled", dt_Family.Rows[i]["cfCalled"].ToString()),
                            new MySqlParameter("cfName", dt_Family.Rows[i]["cfName"].ToString()),
                            new MySqlParameter("dfBirthDate", Convert.ToDateTime(dt_Family.Rows[i]["dfBirthDate"]).ToString("yyyy.MM")),
                            new MySqlParameter("cfPoliticalStatus", dt_Family.Rows[i]["cfPoliticalStatus"].ToString()),
                            new MySqlParameter("cfDuties", dt_Family.Rows[i]["cfDuties"].ToString()),
                            new MySqlParameter("fid", dt_Family.Rows[i]["fid"].ToString()),
                            };
                        MySQLHelper.ExecuteNonQuery("UPDATE  data_familymembers SET cfCalled=@cfCalled,cfName=@cfName,dfBirthDate=@dfBirthDate,cfPoliticalStatus=@cfPoliticalStatus,cfDuties=@cfDuties "
                            + "WHERE fid='" + dt_Family.Rows[i]["fid"].ToString() + "' ", tableParam);

                        }
                        else
                        {
                            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                                new MySqlParameter("cfCalled", dt_Family.Rows[i]["cfCalled"].ToString()),
                                new MySqlParameter("cfName", dt_Family.Rows[i]["cfName"].ToString()),
                                new MySqlParameter("dfBirthDate", dt_Family.Rows[i]["dfBirthDate"].ToString()),
                                new MySqlParameter("cfPoliticalStatus", dt_Family.Rows[i]["cfPoliticalStatus"].ToString()),
                                new MySqlParameter("cfDuties", dt_Family.Rows[i]["cfDuties"].ToString()),

                                new MySqlParameter("pid", dt_pid.Rows[0]["pid"].ToString()),
                                };
                            MySqlParameter[] tableParam = tableStr.ToArray();
                            MySQLHelper.ExecuteNonQuery("INSERT INTO data_familymembers (PersionID,cfCalled,cfName,dfBirthDate,cfPoliticalStatus,cfDuties) VALUES(@pid,@cfCalled,@cfName,@dfBirthDate,@cfPoliticalStatus,@cfDuties)", tableParam);
                        }
                    }
                    
                }
                #endregion

                #region  table CheckResult
                if (dt_CheckResult.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_CheckResult.Rows.Count; i++)
                    {
                        if (dt_CheckResult.Columns.Contains("PersionID") && dt_CheckResult.Rows[i]["PersionID"].ToString() != "" && dt_CheckResult.Columns.Contains("crid"))
                        {
                            // Update

                            MySqlParameter[] tableParam = new MySqlParameter[] {
                            new MySqlParameter("dcrYear", dt_CheckResult.Rows[i]["dcrYear"].ToString().Substring(0,4)),
                            new MySqlParameter("crChechResult", dt_CheckResult.Rows[i]["crChechResult"].ToString()),
                            new MySqlParameter("crid", dt_CheckResult.Rows[i]["crid"].ToString()),
                            };
                            MySQLHelper.ExecuteNonQuery("UPDATE  data_checkresult SET dcrYear=@dcrYear,crChechResult=@crChechResult WHERE crid=@crid ", tableParam);

                        }
                        else
                        {
                            //Insert
                            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                                new MySqlParameter("@dcrYear", dt_CheckResult.Rows[i]["dcrYear"].ToString().Substring(0,4)),
                                new MySqlParameter("@crChechResult", dt_CheckResult.Rows[i]["crChechResult"].ToString()),

                                new MySqlParameter("@pid", dt_pid.Rows[0]["pid"].ToString()),
                                };
                            MySqlParameter[] tableParam = tableStr.ToArray();
                            MySQLHelper.ExecuteNonQuery("INSERT INTO data_checkresult( PersionID,dcrYear,crChechResult) VALUES(@pid,@dcrYear,@crChechResult)", tableParam);
                        }
                    }

                }
                #endregion

                #region  table ReserveCadre
                if (dt_ReserveCadre.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_ReserveCadre.Rows.Count; i++)
                    {
                        if (dt_ReserveCadre.Columns.Contains("PersionID") && dt_ReserveCadre.Rows[i]["PersionID"].ToString() != "" && dt_ReserveCadre.Columns.Contains("rcid"))
                        {
                            // Update family

                            MySqlParameter[] tableParam = new MySqlParameter[] {
                            new MySqlParameter("rcYear", dt_ReserveCadre.Rows[i]["rcYear"].ToString()),
                            new MySqlParameter("rcLevel", dt_ReserveCadre.Rows[i]["rcLevel"].ToString()),
                            new MySqlParameter("rcid", dt_ReserveCadre.Rows[i]["rcid"].ToString()),
                            };
                            MySQLHelper.ExecuteNonQuery("UPDATE  data_reservecadre SET rcYear=@rcYear ,rcLevel=@rcLevel WHERE rcid=@rcid ", tableParam);

                        }
                        else
                        {
                            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                                new MySqlParameter("rcYear", dt_ReserveCadre.Rows[i]["rcYear"].ToString()),
                                new MySqlParameter("rcLevel", dt_ReserveCadre.Rows[i]["rcLevel"].ToString()),

                                new MySqlParameter("pid", dt_pid.Rows[0]["pid"].ToString()),
                                };
                            MySqlParameter[] tableParam = tableStr.ToArray();
                            MySQLHelper.ExecuteNonQuery("INSERT INTO data_reservecadre (PersionID,rcYear,rcLevel) VALUES(@pid,@rcYear,@rcLevel)", tableParam);
                        }
                    }

                }
                #endregion

                #region  table GetCadres
                //if (dt_GetCadres.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt_GetCadres.Rows.Count; i++)
                //    {
                //        if (dt_GetCadres.Columns.Contains("PersionID") && dt_GetCadres.Rows[i]["PersionID"].ToString() != "" && dt_GetCadres.Columns.Contains("gcid"))
                //        {
                //            // Update family

                //            MySqlParameter[] tableParam = new MySqlParameter[] {
                //            new MySqlParameter("gcData", dt_GetCadres.Rows[i]["gcData"].ToString()),
                //            new MySqlParameter("gcDocumentBasis", dt_GetCadres.Rows[i]["gcDocumentBasis"].ToString()),
                //            new MySqlParameter("gcApprovingAuthority", dt_GetCadres.Rows[i]["gcApprovingAuthority"].ToString()),
                //            new MySqlParameter("gcWay", dt_GetCadres.Rows[i]["gcWay"].ToString()),
                //            new MySqlParameter("gcid", dt_GetCadres.Rows[i]["gcid"].ToString()),
                //            };
                //            MySQLHelper.ExecuteNonQuery("UPDATE  data_getcadres SET gcData=@gcData ,gcDocumentBasis=@gcDocumentBasis ,gcApprovingAuthority=@gcApprovingAuthority ,gcWay=@gcWay WHERE gcid=@gcid ", tableParam);

                //        }
                //        else
                //        {
                //            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                //                new MySqlParameter("gcData", dt_GetCadres.Rows[i]["gcData"].ToString()),
                //                new MySqlParameter("gcDocumentBasis", dt_GetCadres.Rows[i]["gcDocumentBasis"].ToString()),
                //                new MySqlParameter("gcApprovingAuthority", dt_GetCadres.Rows[i]["gcApprovingAuthority"].ToString()),
                //                new MySqlParameter("gcWay", dt_GetCadres.Rows[i]["gcWay"].ToString()),

                //                new MySqlParameter("pid", dt_pid.Rows[0]["pid"].ToString()),
                //                };
                //            MySqlParameter[] tableParam = tableStr.ToArray();
                //            MySQLHelper.ExecuteNonQuery("INSERT INTO data_getcadres (PersionID,gcData,gcDocumentBasis,gcApprovingAuthority,gcWay) VALUES(@pid,@gcData,@gcDocumentBasis,@gcApprovingAuthority,@gcWay)", tableParam);
                //        }
                //    }

                //}
                #endregion

                #region  table Employment
                if (dt_Employment.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Employment.Rows.Count; i++)
                    {
                        if (dt_Employment.Columns.Contains("PersionID") && dt_Employment.Rows[i]["PersionID"].ToString() != "" && dt_Employment.Columns.Contains("eid"))
                        {
                            // Update family

                            MySqlParameter[] tableParam = new MySqlParameter[] {
                            new MySqlParameter("eDate", Convert.ToDateTime(dt_Employment.Rows[i]["eDate"]).ToString("yyyy.MM")),
                            new MySqlParameter("eWay", dt_Employment.Rows[i]["eWay"].ToString()),
                            new MySqlParameter("eid", dt_Employment.Rows[i]["eid"].ToString()),
                            };
                            MySQLHelper.ExecuteNonQuery("UPDATE  data_employment SET eDate=@eDate ,eWay=@eWay WHERE eid=@eid ", tableParam);

                        }
                        else
                        {
                            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                                new MySqlParameter("eDate", dt_Employment.Rows[i]["eDate"].ToString()),
                                new MySqlParameter("eWay", dt_Employment.Rows[i]["eWay"].ToString()),

                                new MySqlParameter("pid", dt_pid.Rows[0]["pid"].ToString()),
                                };
                            MySqlParameter[] tableParam = tableStr.ToArray();
                            MySQLHelper.ExecuteNonQuery("INSERT INTO data_employment (PersionID,eDate,eWay) VALUES(@pid,@eDate,@eDate)", tableParam);
                        }
                    }

                }
                #endregion

                #region  table Performance
                if (dt_Performance.Rows.Count > 0)
                {
                    for (int i = 0; i < dt_Performance.Rows.Count; i++)
                    {
                        if (dt_Performance.Columns.Contains("PersionID") && dt_Performance.Rows[i]["PersionID"].ToString() != "" && dt_Performance.Columns.Contains("prid"))
                        {
                            // Update family

                            MySqlParameter[] tableParam = new MySqlParameter[] {
                            new MySqlParameter("rpYear", dt_Performance.Rows[i]["rpYear"].ToString()),
                            new MySqlParameter("cSelfEvaluation", dt_Performance.Rows[i]["cSelfEvaluation"].ToString()),
                            new MySqlParameter("cUnitEvaluation", dt_Performance.Rows[i]["cUnitEvaluation"].ToString()),
                            new MySqlParameter("cOrganizationEvaluation", dt_Performance.Rows[i]["cOrganizationEvaluation"].ToString()),
                             new MySqlParameter("prid",dt_Performance.Rows[i]["prid"].ToString()),
                            };
                            MySQLHelper.ExecuteNonQuery("UPDATE  data_performance SET rpYear=@rpYear ,cSelfEvaluation=@cSelfEvaluation,cUnitEvaluation=@cUnitEvaluation,cOrganizationEvaluation=@cOrganizationEvaluation WHERE prid=@prid ", tableParam);

                        }
                        else
                        {
                            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                                new MySqlParameter("rpYear", dt_Performance.Rows[i]["rpYear"].ToString()),
                                new MySqlParameter("cSelfEvaluation", dt_Performance.Rows[i]["cSelfEvaluation"].ToString()),
                                new MySqlParameter("cUnitEvaluation", dt_Performance.Rows[i]["cUnitEvaluation"].ToString()),
                                new MySqlParameter("cOrganizationEvaluation", dt_Performance.Rows[i]["cOrganizationEvaluation"].ToString()),

                                new MySqlParameter("pid", dt_pid.Rows[0]["pid"].ToString()),
                                };
                            MySqlParameter[] tableParam = tableStr.ToArray();
                            MySQLHelper.ExecuteNonQuery("INSERT INTO data_performance (PersionID,rpYear,cSelfEvaluation,cUnitEvaluation,cOrganizationEvaluation) VALUES(@pid,@rpYear,@cSelfEvaluation,@cUnitEvaluation,@cOrganizationEvaluation)", tableParam);
                        }
                    }

                }
                #endregion
                su = true;
                return su ;
            }
            else {
                su = false;

                XtraMessageBox.Show("请填写姓名和出生年月");
                return su;
            }
        }
        /// <summary>
        /// formatDateEdit,更改时间选择格式
        /// </summary>
        private void formatDateEdit()
        {
            var formatString = "yyyy.MM";
            //var dateEdit = new DateEdit();
            //dfBirthDateDateEdit.DisplayFormat.FormatString = formatString;
            dfBirthDateDateEdit.Mask.EditMask = formatString;
            dfBirthDateDateEdit.Mask.UseMaskAsDisplayFormat = true;
            dfBirthDateDateEdit.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
            dfBirthDateDateEdit.VistaCalendarViewStyle = VistaCalendarViewStyle.Default;

            dStratDateEdit.Mask.EditMask = formatString;
            dStratDateEdit.Mask.UseMaskAsDisplayFormat = true;
            dStratDateEdit.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
            dStratDateEdit.VistaCalendarViewStyle = VistaCalendarViewStyle.Default;

            dBirth_date.Properties.Mask.EditMask = formatString;
            dBirth_date.Properties.Mask.UseMaskAsDisplayFormat = true;
            dBirth_date.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
            dBirth_date.Properties.VistaCalendarViewStyle = VistaCalendarViewStyle.Default;

            dJoin_date.Properties.Mask.EditMask = formatString;
            dJoin_date.Properties.Mask.UseMaskAsDisplayFormat = true;
            dJoin_date.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
            dJoin_date.Properties.VistaCalendarViewStyle = VistaCalendarViewStyle.Default;

            dWorkDate.Properties.Mask.EditMask = formatString;
            dWorkDate.Properties.Mask.UseMaskAsDisplayFormat = true;
            dWorkDate.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
            dWorkDate.Properties.VistaCalendarViewStyle = VistaCalendarViewStyle.Default;

            dDate4.Properties.Mask.EditMask = formatString;
            dDate4.Properties.Mask.UseMaskAsDisplayFormat = true;
            dDate4.Properties.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
            dDate4.Properties.VistaCalendarViewStyle = VistaCalendarViewStyle.Default;

            formatString = "yyyy.MM.dd";
            dDate5.Mask.EditMask = formatString;
            dDate5.Mask.UseMaskAsDisplayFormat = true;

            dMakeDate.Properties.Mask.EditMask = formatString;
            dMakeDate.Properties.Mask.UseMaskAsDisplayFormat = true;

            formatString = "yyyy";
            dDate2.Mask.EditMask = formatString;
            dDate2.Mask.UseMaskAsDisplayFormat = true;
            dDate2.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dDate2.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;

            dDate3.Mask.EditMask = formatString;
            //dDate3.DisplayFormat.FormatString = formatString;
            dDate3.Mask.UseMaskAsDisplayFormat = true;
            dDate3.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dDate3.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;

            dDate6.Mask.EditMask = formatString;
            //dDate6.DisplayFormat.FormatString = formatString;
            dDate6.Mask.UseMaskAsDisplayFormat = true;
            dDate6.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearsGroupView;
            dDate6.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearsGroupView;
        }
        /// <summary>
        /// 是否激活编辑状态
        /// </summary>
        /// <param name="flag">true表示激活，false不激活</param>
        private void enableEdit(bool flag)
        {
            if (!flag)
            {
                #region disable balnk
                txtcName.Enabled = false;
                txtcName.ForeColor = Color.Black;
                txtcName.BackColor = Color.White;
                
                txtcSex.Enabled = false;
                txtcSex.ForeColor = Color.Black;
                txtcSex.BackColor = Color.White;

                dBirth_date.Enabled = false;
                dBirth_date.ForeColor = Color.Black;
                dBirth_date.BackColor = Color.White;

                txtcNation.Enabled = false;
                txtcNation.ForeColor = Color.Black;
                txtcNation.BackColor = Color.White;

                txtcNativePlace.Enabled = false;
                txtcNativePlace.ForeColor = Color.Black;
                txtcNativePlace.BackColor = Color.White;

                txtcIDnumber.Enabled = false;
                txtcIDnumber.ForeColor = Color.Black;
                txtcIDnumber.BackColor = Color.White;

                dJoin_date.Enabled = false;
                dJoin_date.ForeColor = Color.Black;
                dJoin_date.BackColor = Color.White;

                txtcHealthStatus.Enabled = false;
                txtcHealthStatus.ForeColor = Color.Black;
                txtcHealthStatus.BackColor = Color.White;

                txtcBirthPlace.Enabled = false;
                txtcBirthPlace.ForeColor = Color.Black;
                txtcBirthPlace.BackColor = Color.White;

                dWorkDate.Enabled = false;
                dWorkDate.ForeColor = Color.Black;
                dWorkDate.BackColor = Color.White;

                txtcDuties.Enabled = false;
                txtcDuties.ForeColor = Color.Black;
                txtcDuties.BackColor = Color.White;

                txtcSkill.Enabled = false;
                txtcSkill.ForeColor = Color.Black;
                txtcSkill.BackColor = Color.White;

                txtcFull_timeEducation.Enabled = false;
                txtcFull_timeEducation.ForeColor = Color.Black;
                txtcFull_timeEducation.BackColor = Color.White;

                txtcFull_timeDegree.Enabled = false;
                txtcFull_timeDegree.ForeColor = Color.Black;
                txtcFull_timeDegree.BackColor = Color.White;

                txtcFull_timeSchool.Enabled = false;
                txtcFull_timeSchool.ForeColor = Color.Black;
                txtcFull_timeSchool.BackColor = Color.White;

                txtcFull_timeMajor.Enabled = false;
                txtcFull_timeMajor.ForeColor = Color.Black;
                txtcFull_timeMajor.BackColor = Color.White;

                txtcIn_serviceEducation.Enabled = false;
                txtcIn_serviceEducation.ForeColor = Color.Black;
                txtcIn_serviceEducation.BackColor = Color.White;

                txtcIn_serviceDegree.Enabled = false;
                txtcIn_serviceDegree.ForeColor = Color.Black;
                txtcIn_serviceDegree.BackColor = Color.White;

                txtcIn_serviceSchool.Enabled = false;
                txtcIn_serviceSchool.ForeColor = Color.Black;
                txtcIn_serviceSchool.BackColor = Color.White;

                txtcIn_serviceMajor.Enabled = false;
                txtcIn_serviceMajor.ForeColor = Color.Black;
                txtcIn_serviceMajor.BackColor = Color.White;

                txtcCurrentJob.Enabled = false;
                txtcCurrentJob.ForeColor = Color.Black;
                txtcCurrentJob.BackColor = Color.White;

                txtcProposedJob.Enabled = false;
                txtcProposedJob.ForeColor = Color.Black;
                txtcProposedJob.BackColor = Color.White;

                txtcRemoveJob.Enabled = false;
                txtcRemoveJob.ForeColor = Color.Black;
                txtcRemoveJob.BackColor = Color.White;

                txtcDismissalReason.Enabled = false;
                txtcDismissalReason.ForeColor = Color.Black;
                txtcDismissalReason.BackColor = Color.White;

                txtcReporting_Unit.Enabled = false;
                txtcReporting_Unit.ForeColor = Color.Black;
                txtcReporting_Unit.BackColor = Color.White;

                dEageDate.Enabled = false;
                dEageDate.ForeColor = Color.Black;
                dEageDate.BackColor = Color.White;

                txtcMaker.Enabled = false;
                txtcMaker.ForeColor = Color.Black;
                txtcMaker.BackColor = Color.White;

                dMakeDate.Enabled = false;
                dMakeDate.ForeColor = Color.Black;
                dMakeDate.BackColor = Color.White;

                txtIdentityCategory.Enabled = false;
                txtIdentityCategory.ForeColor = Color.Black;
                txtIdentityCategory.BackColor = Color.White;

                txtRank.Enabled = false;
                txtRank.ForeColor = Color.Black;
                txtRank.BackColor = Color.White;

                dDate4.Enabled = false;
                dDate4.ForeColor = Color.Black;
                dDate4.BackColor = Color.White;

                txtcDocumentBasis.Enabled = false;
                txtcDocumentBasis.ForeColor = Color.Black;
                txtcDocumentBasis.BackColor = Color.White;

                txtcApprovingAuthority.Enabled = false;
                txtcApprovingAuthority.ForeColor = Color.Black;
                txtcApprovingAuthority.BackColor = Color.White;

                txtcWay.Enabled = false;
                txtcWay.ForeColor = Color.Black;
                txtcWay.BackColor = Color.White;

                cbIsNative.Enabled = false;
                cbIsNative.ForeColor = Color.Black;
                cbIsNative.BackColor = Color.White;

                cbIsOfficialPosition.Enabled = false;
                cbIsOfficialPosition.ForeColor = Color.Black;
                cbIsOfficialPosition.BackColor = Color.White;

                #endregion
                #region disable all excel
                gvResume.OptionsBehavior.Editable= false;
                gvRewards.OptionsBehavior.Editable = false;
                gvFamilymembers.OptionsBehavior.Editable = false;
                gvCheckResult.OptionsBehavior.Editable = false;
                gvReserveCadre.OptionsBehavior.Editable = false;
                gvEmployment.OptionsBehavior.Editable = false;
                gvPerformance.OptionsBehavior.Editable = false;

                #endregion
                #region disable button
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                btnOut.Enabled = false;
                btnExit.Enabled = false;
                #endregion
            }
        }
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmInfMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Parent == null && isEnableEdit && !issave)
            {
                if (XtraMessageBox.Show("是否关闭，未保存的数据将会丢失", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }
    }
}
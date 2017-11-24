using BarCode;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace PersonnelManagement
{
    public partial class FrmInf : DevExpress.XtraEditors.XtraForm
    {
        public  DataTable gl_dt = new DataTable();
        private bool isAdd = false;

        public FrmInf()
        {
            InitializeComponent();
            //初始化填表人
            txtcMaker.Text = Pub.PubValue.UserName;
            start();
            isAdd = true;
        }
        public FrmInf(string id)
        {
            InitializeComponent();
            start(id);
            isAdd = false;
        }


        private void btnQuit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (DialogResult.Yes == XtraMessageBox.Show("是否退出,未保存的数据将会丢失", "提示", MessageBoxButtons.YesNo))
            {
                //关闭窗体
                this.Close();
                
            }
        }

        private void btnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            saveIteam();
            if (DialogResult.Yes == XtraMessageBox.Show("是否继续添加？", "提示", MessageBoxButtons.YesNo)&&isAdd)
            {
                clear();
            }
        }
       

        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            btnSave_ItemClick(sender,e);
            List<MySqlParameter> ilistStr = new List<MySqlParameter> {
            new MySqlParameter("cName", txtcName.Text),
            new MySqlParameter("cName", txtcName.Text),
            };
            MySqlParameter[] param = ilistStr.ToArray();
            DataTable dt= MySQLHelper.table("SELECT * FROM data_persion WHERE cName=@cName AND dBirth_date=@dBirth_date ", param);
            SaveFile.Export(dt);
        }

        /// <summary>
        /// 初始化操作
        /// </summary>
        /// <param name="id"></param>
        private void btnAddIteam_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //saveIteam();
            //isAdd = true;
            clear();
            
        }
        private void start()
        {
            
            DataTable dt_Rewards = MySQLHelper.table("SELECT * FROM data_rewards_punishments  where rpid in ('')");
            DataTable dt_Family = MySQLHelper.table("SELECT * FROM data_familymembers  where fid in ('')");

            #region   显示数据
            gcRewards.DataSource = dt_Rewards;
            gcFamilymembers.DataSource = dt_Family;
            #endregion

            var formatString = "yyyy.MM";
            //var dateEdit = new DateEdit();
            dfBirthDateDateEdit.Mask.EditMask = formatString;
            dfBirthDateDateEdit.DisplayFormat.FormatString = formatString;
            dfBirthDateDateEdit.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            dfBirthDateDateEdit.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
        }
        private void start(string id)
        {
            List<MySqlParameter> ilistStr = new List<MySqlParameter> {
                    new MySqlParameter("pid", id),
            };
            MySqlParameter[] parm = ilistStr.ToArray();
            DataTable dt = MySQLHelper.table("select * from data_all where pid= @pid", parm);
            DataTable dt_Rewards = MySQLHelper.table("select * from data_rewards_punishments where PersionID= @pid", parm);
            DataTable dt_Family = MySQLHelper.table("select * from data_familymembers where PersionID= @pid", parm);
            if (dt == null || dt.Rows.Count <= 0)
            {
                XtraMessageBox.Show("无该数据，请关闭后，刷新列表重试");
                return;
            }
            gl_dt = dt;
            //gcresume.DataSource = dt;

            #region   显示数据
            txtcName.Text = gl_dt.Rows[0]["cName"].ToString();
            txtcSex.Text = gl_dt.Rows[0]["cSex"].ToString();
            dBirth_date.Text = gl_dt.Rows[0]["dBirth_date"].ToString();
            txtcNation.Text = gl_dt.Rows[0]["cNation"].ToString();
            txtcNativePlace.Text = gl_dt.Rows[0]["cNativePlace"].ToString();
            dJoin_date.Text = gl_dt.Rows[0]["dJoin_date"].ToString();
            txtcHealthStatus.Text = gl_dt.Rows[0]["cHealthStatus"].ToString();
            txtcBirthPlace.Text = gl_dt.Rows[0]["cBirthPlace"].ToString();
            dWorkDate.Text = gl_dt.Rows[0]["dWorkDate"].ToString();
            txtcDuties.Text = gl_dt.Rows[0]["cDuties"].ToString();
            txtcSkill.Text = gl_dt.Rows[0]["cSkill"].ToString();
            txtcFull_timeEducation.Text = gl_dt.Rows[0]["cFull_timeEducation"].ToString();
            txtcFull_timeSchool.Text = gl_dt.Rows[0]["cFull_timeSchool"].ToString();
            txtcIn_serviceEducation.Text = gl_dt.Rows[0]["cIn_serviceEducation"].ToString();
            txtcIn_serviceSchool.Text = gl_dt.Rows[0]["cIn_serviceSchool"].ToString();
            txtcCurrentJob.Text = gl_dt.Rows[0]["cCurrentJob"].ToString();
            txtcProposedJob.Text = gl_dt.Rows[0]["cProposedJob"].ToString();
            txtcRemoveJob.Text = gl_dt.Rows[0]["cRemoveJob"].ToString();

            txtResume.Text= gl_dt.Rows[0]["cResume"].ToString();
            gcRewards.DataSource = dt_Rewards;
            gcFamilymembers.DataSource = dt_Family;
            txtcChech_Result.Text = gl_dt.Rows[0]["cChech_Result"].ToString();
            txtcDismissalReason.Text = gl_dt.Rows[0]["cDismissalReason"].ToString();
            txtcReporting_Unit.Text = gl_dt.Rows[0]["cReporting_Unit"].ToString();

            dEageDate.Text= gl_dt.Rows[0]["dEageDate"].ToString();
            txtcMaker.Text = gl_dt.Rows[0]["cMaker"].ToString();
            dMakeDate.Text = gl_dt.Rows[0]["dMakeDate"].ToString();
            #endregion

            var formatString = "yyyy.MM";
            //var dateEdit = new DateEdit();
            dfBirthDateDateEdit.Mask.EditMask = formatString;
            dfBirthDateDateEdit.DisplayFormat.FormatString = formatString;
            dfBirthDateDateEdit.VistaCalendarInitialViewStyle = DevExpress.XtraEditors.VistaCalendarInitialViewStyle.YearView;
            dfBirthDateDateEdit.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
        }

        private void clear()
        {
            
            txtcName.Text = "";
            txtcSex.Text = "";
            dBirth_date.Text = "";
            txtcNation.Text = "";
            txtcNativePlace.Text = "";
            dJoin_date.Text = "";
            txtcHealthStatus.Text = "";
            txtcBirthPlace.Text = "";
            dWorkDate.Text = "";
            txtcDuties.Text = "";
            txtcSkill.Text = "";
            txtcFull_timeEducation.Text = "";
            txtcFull_timeSchool.Text = "";
            txtcIn_serviceEducation.Text = "";
            txtcIn_serviceSchool.Text = "";
            txtcCurrentJob.Text = "";
            txtcProposedJob.Text = "";
            txtcRemoveJob.Text = "";
            DataTable dt_Rewards = new DataTable();
            gcRewards.DataSource = dt_Rewards;
            DataTable dt_Family = new DataTable();
            gcFamilymembers.DataSource = dt_Family;
            txtcChech_Result.Text = "";
            txtcDismissalReason.Text = "";
            txtcReporting_Unit.Text = "";

            dEageDate.Text = "";
            txtcMaker.Text = Pub.PubValue.UserName;
            dMakeDate.Text = "";
        }
        /// <summary>
        ///  保存数据
        /// </summary>
        private void saveIteam()
        {
            if (txtcName.Text != "")
            {
                #region  制造数据集合
                List<MySqlParameter> ilistStr = new List<MySqlParameter> {
            new MySqlParameter("cName", txtcName.Text),
            new MySqlParameter("cSex", txtcSex.Text),
            new MySqlParameter("dBirth_date", dBirth_date.Text),
            new MySqlParameter("cNation", txtcNation.Text),
            new MySqlParameter("cNativePlace", txtcNativePlace.Text),
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
            new MySqlParameter("cResume", txtResume.Text),

            //new MySqlParameter("cName", gcRewards.DataSource = dt;
            //new MySqlParameter("cName", gcFamilymembers.DataSource = dt;
            new MySqlParameter("cChech_Result", txtcChech_Result.Text),
            new MySqlParameter("cDismissalReason", txtcDismissalReason.Text),
            new MySqlParameter("cReporting_Unit", txtcReporting_Unit.Text),
            new MySqlParameter("dEageDate", txtcReporting_Unit.Text),
            new MySqlParameter("cMaker", txtcMaker.Text),
            new MySqlParameter("dMakeDate", dMakeDate.Text),
            new MySqlParameter("do_flag", "1"),
            };
                DataTable dt_Rewards = gcRewards.DataSource as DataTable;
                DataTable dt_Family = gcFamilymembers.DataSource as DataTable;
                MySqlParameter[] param = ilistStr.ToArray();
                #endregion

                if (isAdd)
                {

                    //MySQLHelper.ExecuteNonQuery(" INSERT INTO data_persion(cName,cSex,dWorkDate) VALUES(@cName,@cSex,@dWorkDate)", param);
                    MySQLHelper.ExecuteNonQuery(" INSERT INTO data_persion( cName,cSex,dBirth_date,cNation,cNativePlace,dJoin_date,cHealthStatus, cBirthPlace, dWorkDate, cDuties, cSkill, cFull_timeEducation, cFull_timeSchool, cIn_serviceEducation, cIn_serviceSchool,cCurrentJob, cProposedJob, cRemoveJob,cResume,cChech_Result, cDismissalReason, cReporting_Unit, dEageDate, dMakeDate, cMaker, do_flag ) VALUES( @cName , @cSex , @dBirth_date , @cNation,@cNativePlace,@dJoin_date,@cHealthStatus, @cBirthPlace, @dWorkDate, @cDuties, @cSkill, @cFull_timeEducation, @cFull_timeSchool, @cIn_serviceEducation, @cIn_serviceSchool,@cCurrentJob, @cProposedJob, @cRemoveJob,@cResume,@cChech_Result, @cDismissalReason, @cReporting_Unit, @dEageDate, @dMakeDate, @cMaker, @do_flag )", param);
                }
                else
                {

                    if (gl_dt != null && gl_dt.Rows.Count > 0)
                    {
                        ilistStr.Add(new MySqlParameter("pid", gl_dt.Rows[0]["pid"].ToString()));
                        param = ilistStr.ToArray();
                        MySQLHelper.ExecuteNonQuery(" UPDATE data_persion SET cName=@cName,cSex=@cSex,dBirth_date=@dBirth_date,cNation=@cNation,cNativePlace=@cNativePlace,dJoin_date=@dJoin_date,cHealthStatus=@cHealthStatus,cBirthPlace=@cBirthPlace,dWorkDate=@dWorkDate,cDuties=@cDuties,cSkill=@cSkill,cFull_timeEducation=@cFull_timeEducation,cFull_timeSchool=@cFull_timeSchool,cIn_serviceEducation=@cIn_serviceEducation,cIn_serviceSchool=@cIn_serviceSchool, cCurrentJob=@cCurrentJob,cProposedJob=@cProposedJob,cRemoveJob=@cRemoveJob,cResume=@cResume,cChech_Result=@cChech_Result,cDismissalReason=@cDismissalReason,cReporting_Unit=@cReporting_Unit,dEageDate=@dEageDate,dMakeDate=@dMakeDate,cMaker=@cMaker,do_flag=@do_flag WHERE pid=@pid", param);
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

                // 11-11 re-edit
                #region table Rewards

                if (dt_Rewards.Rows.Count > 0)
                {
                    if (dt_Rewards.Rows[0]["PersionID"].ToString() != "" && dt_Rewards.Columns.Contains("rpid"))
                    {
                        //UPDATE Rewards
                        for (int i = 0; i < dt_Rewards.Rows.Count; i++)
                        {
                            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                            new MySqlParameter("dData", dt_Rewards.Rows[i]["dData"].ToString()),
                            new MySqlParameter("cDetailed", dt_Rewards.Rows[i]["cDetailed"].ToString()),
                            new MySqlParameter("rpid", dt_Rewards.Rows[i]["rpid"].ToString()),
                            new MySqlParameter("cCategory", dt_Rewards.Rows[i]["cCategory"].ToString()),
                            new MySqlParameter("cLevel", dt_Rewards.Rows[i]["cLevel"].ToString()),
                            };
                            MySqlParameter[] tableParam = ilistStr.ToArray();
                            MySQLHelper.ExecuteNonQuery("UPDATE data_rewards_punishments SET dData='" + dt_Rewards.Rows[i]["dData"].ToString() 
                                + "' ,cDetailed='" + dt_Rewards.Rows[i]["cDetailed"].ToString()
                                + "' ,cCategory='" + dt_Rewards.Rows[i]["cCategory"].ToString()
                                + "' ,cLevel='" + dt_Rewards.Rows[i]["cLevel"].ToString()
                                + "' WHERE rpid='" + dt_Rewards.Rows[i]["rpid"].ToString() + "' ", tableParam);
                        }
                    }
                    else
                    {
                        //INSERT Rewards
                        for (int i = 0; i < dt_Rewards.Rows.Count; i++)
                        {
                            MySqlParameter[] tableParam = new MySqlParameter[]  {
                            new MySqlParameter("dData", dt_Rewards.Rows[i]["dData"].ToString()),
                            new MySqlParameter("cDetailed", dt_Rewards.Rows[i]["cDetailed"].ToString()),
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
                    if (dt_Family.Rows[0]["PersionID"].ToString() != "" && dt_Family.Columns.Contains("fid"))
                    {
                        // Update family
                        for (int i = 0; i < dt_Family.Rows.Count; i++)
                        {
                            MySqlParameter[] tableParam = new MySqlParameter[] {
                        new MySqlParameter("cfCalled", dt_Family.Rows[i]["cfCalled"].ToString()),
                        new MySqlParameter("cfName", dt_Family.Rows[i]["cfName"].ToString()),
                        new MySqlParameter("dfBirthDate", dt_Family.Rows[i]["dfBirthDate"].ToString()),
                        new MySqlParameter("cfPoliticalStatus", dt_Family.Rows[i]["cfPoliticalStatus"].ToString()),
                        new MySqlParameter("cfDuties", dt_Family.Rows[i]["cfDuties"].ToString()),
                        new MySqlParameter("fid", dt_Family.Rows[i]["fid"].ToString()),
                    };
                            MySQLHelper.ExecuteNonQuery("UPDATE  data_familymembers SET cfCalled='" + dt_Family.Rows[i]["cfCalled"].ToString()
                                + "',cfName='" + dt_Family.Rows[i]["cfName"].ToString() + "',dfBirthDate='" + dt_Family.Rows[i]["dfBirthDate"].ToString()
                                + "',cfPoliticalStatus='" + dt_Family.Rows[i]["cfPoliticalStatus"].ToString() + "',cfDuties='" + dt_Family.Rows[i]["cfDuties"].ToString() + "' "
                                + "WHERE fid='" + dt_Family.Rows[i]["fid"].ToString() + "' ", tableParam);

                        }
                    }
                    else
                    {
                        for (int i = 0; i < dt_Family.Rows.Count; i++)
                        {
                            List<MySqlParameter> tableStr = new List<MySqlParameter> {
                        new MySqlParameter("cfCalled", dt_Family.Rows[i]["cfCalled"].ToString()),
                        new MySqlParameter("cfName", dt_Family.Rows[i]["cfName"].ToString()),
                        new MySqlParameter("dfBirthDate", dt_Family.Rows[i]["dfBirthDate"].ToString()),
                        new MySqlParameter("cfPoliticalStatus", dt_Family.Rows[i]["cfPoliticalStatus"].ToString()),
                        new MySqlParameter("cfDuties", dt_Family.Rows[i]["cfDuties"].ToString()),

                        new MySqlParameter("pid", dt_pid.Rows[0]["pid"].ToString()),
                    };
                            MySqlParameter[] tableParam = ilistStr.ToArray();
                            MySQLHelper.ExecuteNonQuery("INSERT INTO data_familymembers (PersionID,cfCalled,cfName,dfBirthDate,cfPoliticalStatus,cfDuties) VALUES(@PersionID,@cfCalled,@cfName,@dfBirthDate,@cfPoliticalStatus,@cfDuties)", tableParam);

                        }
                    }
                }
                #endregion
            }
            else { XtraMessageBox.Show("请填写关键数据"); }
        }


        private void FrmInf_Load(object sender, EventArgs e)
        {

        }
    }
}
using BarCode;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PersonnelManagement
{
    public class SaveFile
    {
        private static DateTime date1 = DateTime.Now;

        /// <summary>
        /// 保存对话框
        /// </summary>
        /// <param name="s"></param>
        /// <param name="name"></param>
        private static string saveFile(string s, string name)
        {
            string path = "";
            using (SaveFileDialog saveDialog = new SaveFileDialog())    //离开后销毁对话框
            {
                name += DateTime.Now.ToString("yyyyMMdd");
                saveDialog.FileName = name;


                saveDialog.Filter = "任免表(.lrm)|*.lrm";
                Start: if (saveDialog.ShowDialog() != DialogResult.Cancel)          //用户点击保存按钮
                {
                    
                    string exportFilePath = saveDialog.FileName;                    //定义文件路径              
                    string fileExtenstion = new FileInfo(exportFilePath).Extension; //文件扩展名
                    path = saveDialog.FileName.Replace(name+ fileExtenstion, "");
                    //创建文件
                    try
                    {
                        switch (fileExtenstion)                             //判断文件类型
                        {
                            case ".lrm":
                                //gcQurry.ExportToXls(exportFilePath);      //.lrm类型
                                //使用“保存”对话框中输入的文件名实例化StreamWriter对象
                                StreamWriter sw = new StreamWriter(exportFilePath, true);
                                //向创建的文件中写入内容
                                sw.WriteLine(s);
                                //关闭当前文件写入流
                                sw.Close();
                                break;
                            default:
                                break;
                        }
                        path= saveDialog.FileName.Replace(name, "");
                        XtraMessageBox.Show("文件保存成功");
                        //return saveDialog.FileName.Replace(name,"");
                    }
                    catch
                    {
                        //文件无法打开 消息字符串 提示语+/n/n+path:+路径
                        String msg = "文件无法被创建或者修改，正在使用或者没有权限,请检查是否同名文件被打开，请尝试关闭打开文件后重试" + Environment.NewLine + Environment.NewLine + "Path: " + exportFilePath;
                        XtraMessageBox.Show(msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //转到执行保存事件
                        //btnOutput_Click(sender,e);  待测试替换goto
                        goto Start;

                    }
                }
                return path;
            }

            //if (s == string.Empty)
            //   {
            //       MessageBox.Show("要写入的文件内容不能为空");
            //   }
            //   else
            //   {
            //       //设置保存文件的格式
            //       saveFileDialog1.Filter = "文本文件(*.txt)|*.txt";
            //       if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //       {
            //           //使用“另存为”对话框中输入的文件名实例化StreamWriter对象
            //           StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, true);
            //           //向创建的文件中写入内容
            //           sw.WriteLine(textBox1.Text);
            //           //关闭当前文件写入流
            //           sw.Close();
            //           textBox1.Text = string.Empty;
            //       }
            //   }
        }

        /// <summary>
        /// 格式化数据
        /// </summary>
        /// <param name="dt">基础数据表，需含有pid列(人员主键列)</param>
        /// <returns>返回lrm字符串</returns>
        private static string formString(DataTable dt)
        {
            StringBuilder s = new StringBuilder("");
            if (dt == null || dt.Rows.Count < 0)
            {
                XtraMessageBox.Show("没有数据");
            }
            s.Append(@"""" + dt.Rows[0]["cName"].ToString() + @""",");
            s.Append(@"""" + dt.Rows[0]["cSex"].ToString() + @""",");
            s.Append(@"""" + dt.Rows[0]["dBirth_date"].ToString() + @""",");
            s.Append(@"""" + dt.Rows[0]["cNation"].ToString() + @""",");
            s.Append(@"""" + dt.Rows[0]["cNativePlace"].ToString() + @""",");
            s.Append(@"""" + dt.Rows[0]["dJoin_date"].ToString() + @""",");
            s.Append(@"""" + dt.Rows[0]["cHealthStatus"].ToString() + @""",");
            s.Append(@"""" + dt.Rows[0]["cBirthPlace"].ToString() + @""",");
            s.Append(@"""" + dt.Rows[0]["dWorkDate"].ToString() + @""",");


            s.Append(@"""" + dt.Rows[0]["cFull_timeEducation"].ToString() + "#@" + dt.Rows[0]["cIn_serviceEducation"].ToString() + @"#"",");
            s.Append(@"""" + dt.Rows[0]["cFull_timeSchool"].ToString() + "#@" + dt.Rows[0]["cIn_serviceSchool"].ToString() + @"#"",");

            s.Append(@"""" + dt.Rows[0]["cDuties"].ToString() + @""",");
            s.Append(@"""" + " " + @""",");
            s.Append(@"""" + " " + @""",");
            s.Append(@"""" + " " + @""",");
            s.Append(@"""" + " " + @""",");

            s.Append(@"""" + dt.Rows[0]["cSkill"].ToString() + @""",");

            //简历
            DataTable dt_resume = MySQLHelper.table("SELECT *FROM data_resume WHERE PersionID='" + dt.Rows[0]["pid"] + "'");
            if (dt_resume != null && dt_resume.Rows.Count > 0)
            {
                for (int i = 0; i < dt_resume.Rows.Count - 1; i++)
                {
                    s.Append(@"""" + dt_resume.Rows[i]["dStartDate"]+ "-"
                        + dt_resume.Rows[i]["dDeadline"] + "       "
                        + dt_resume.Rows[i]["cExperience"].ToString() + "\r\n");
                }
                s.Append(@"""" + dt_resume.Rows[dt_resume.Rows.Count - 1]["dStartDate"] + "-"
                        + dt_resume.Rows[dt_resume.Rows.Count - 1]["dDeadline"]+ "       "
                        + dt_resume.Rows[dt_resume.Rows.Count - 1]["cExperience"].ToString() + @""",");
            }
            dt_resume.Dispose();

            //奖惩情况
            DataTable dt_rewards = MySQLHelper.table("SELECT *FROM data_rewards_punishments WHERE PersionID='" + dt.Rows[0]["pid"] + "'");
            if (dt_rewards != null && dt_rewards.Rows.Count > 0)
            {
                for (int i = 0; i < dt_rewards.Rows.Count - 1; i++)
                {
                    //s.Append(@"""" + Convert.ToDateTime(dt_rewards.Rows[i]["dData"]).ToString("yyyy.MM") + "       "
                    //       + dt_rewards.Rows[i]["cDetailed"].ToString() + "\r\n");
                    s.Append(@"""" + dt_rewards.Rows[i]["dData"].ToString() + "       "
                           + dt_rewards.Rows[i]["cDetailed"].ToString() + "\r\n");
                }
                //s.Append(@"""" + Convert.ToDateTime(dt_rewards.Rows[dt_rewards.Rows.Count - 1]["dData"]).ToString("yyyy.MM") + "       "
                //           + dt_rewards.Rows[dt_rewards.Rows.Count - 1]["cDetailed"].ToString() + "\r\n");
                s.Append(@"""" + dt_rewards.Rows[dt_rewards.Rows.Count - 1]["dData"].ToString() + "       "
                           + dt_rewards.Rows[dt_rewards.Rows.Count - 1]["cDetailed"].ToString() + "\r\n");
            }
            dt_rewards.Dispose();

            //年度考核结果
            s.Append(@"""" + dt.Rows[0]["cChech_Result"].ToString() + @""",");

            //家庭成员

            DataTable dt_family = MySQLHelper.table("SELECT *FROM data_familymembers WHERE PersionID='" + dt.Rows[0]["pid"] + "'");
            if (dt_family != null && dt_family.Rows.Count > 0)
            {
                string[] name = new string[] { "cfCalled", "cfName", "dfBirthDate", "cfPoliticalStatus", "cfDuties" };
                foreach (string str in name)
                {
                    s.Append(@"""");
                    for (int j = 0; j < 12; j++)
                    {
                        for (int i = 0; i < dt_family.Rows.Count; i++, j++)
                        {
                            s.Append(dt_family.Rows[i][str].ToString());

                        }
                    }
                    s.Append(@""",");
                }
            }
            dt_family.Dispose();

            //现任职务
            s.Append(@"""" + dt.Rows[0]["cCurrentJob"].ToString() + @""",");
            //拟任职务
            s.Append(@"""" + dt.Rows[0]["cProposedJob"].ToString() + @""",");
            //拟免职务
            s.Append(@"""" + dt.Rows[0]["cRemoveJob"].ToString() + @""",");
            //任免理由
            s.Append(@"""" + dt.Rows[0]["cDismissalReason"].ToString() + @""",");
            //呈报单位
            s.Append(@"""" + dt.Rows[0]["cReporting_Unit"].ToString() + @""",");
            //计算年龄时间
            s.Append(@"""" + dt.Rows[0]["dEageDate"].ToString() + @""",");
            //填表人
            s.Append(@"""" + dt.Rows[0]["cMaker"].ToString() + @""",");
            //填表时间
            s.Append(@"""" + dt.Rows[0]["dMakeDate"].ToString() + @""",");

            return s.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt">基础数据表，需含有pid列(人员主键列)</param>
        /// <param name="name">文件名</param>
        public static void ExportLrm(DataTable dt, string name = "")
        {
            saveFile(formString(dt), name);
        }
        /// <summary>
        /// 导出人名单
        /// </summary>
        /// <param name="dt">基础数据表，需含有pid列(人员主键列)</param>
        /// <param name="name">文件名</param>
        public static void ExportExcel(DataTable dt, string name="")
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())    //离开后销毁对话框
            {
                //string name = gvType.GetFocusedDataRow()["cName"].ToString() + "-";
                saveDialog.FileName = name + DateTime.Now.ToString("yyyyMMdd");
                saveDialog.Filter = "Excel 97-2013 工作簿(*.xls)|*.xls";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)  //用户点击保存按钮
                {
                    string exportFilePath = saveDialog.FileName;        //定义文件路径              
                    string fileExtenstion = new FileInfo(exportFilePath).Extension; //创建文件
                    if (saveDialog.FileName.Trim().Length > 0)
                    {
                        byte[] excel = Properties.Resources.名册0;

                        try
                        {
                            FileStream stream = new FileStream(saveDialog.FileName, FileMode.Create);
                            stream.Write(excel, 0, excel.Length);
                            stream.Close();
                            stream.Dispose();
                            //XtraMessageBox.Show("保存成功", "提示");

                            string Path = exportFilePath;
                            if (Path == "")
                                return;

                            //Office 07以下版本
                            string sConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Path + ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=0;'"; 
                            // Office 07及以上版本 不能出现多余的空格 而且分号注意
                            //string sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=0;'";
                            OleDbConnection cn = new OleDbConnection(sConnectionString);
                            //string c = "INSERT INTO [在职$] (本人姓名,出生年月,年度,级别,备注) VALUES('lxm','1995/07','1995','1','2')";



                            OleDbCommand cmd = new OleDbCommand("", cn);
                            //创建Excel文件
                            cn.Open();
                            //foreach (DataRow r in dt.Rows)
                            for(int i=0;i<dt.Rows.Count;i++)
                            {
                                //cmd.CommandText = "INSERT INTO [在职$] (入党时间) VALUES('1995.02')";
                                cmd.CommandText = "INSERT INTO [在职$] (序号,单位,姓名,职务,性别,民族,籍贯,"
                                    + "出生年月,入党时间,参加工作时间,全日制学历,毕业院校及专业,在职教育,毕业学校及专业,历任职务,"
                                    + "任现职时间,任同级职务时间,备注) "
                                    + "VALUES('" + (i+1).ToString() + "','" + dt.Rows[i]["cUnit"] + "','" + dt.Rows[i]["cName"] + "','" + dt.Rows[i]["cCurrentJob"] + "','" + dt.Rows[i]["cSex"] + "','" + dt.Rows[i]["cNation"] + "','" + dt.Rows[i]["cNativePlace"] + "','" + dt.Rows[i]["dBirth_date"] + "','" + dt.Rows[i]["dJoin_date"] + "',"
                                    + "'" + dt.Rows[i]["dWorkDate"] + "','" + dt.Rows[i]["cFull_timeEducation"] + "','" + dt.Rows[i]["cFull_timeSchool"] + dt.Rows[i]["cFull_timeMajor"] + "','" + dt.Rows[i]["cIn_serviceEducation"] + "','" + dt.Rows[i]["cIn_serviceSchool"] + dt.Rows[i]["cIn_serviceMajor"] + "','" + getResumeBypid(dt.Rows[i]["pid"].ToString()) + "','" + dt.Rows[i]["dInOffice"] + "','" + dt.Rows[i]["dSameOffic"] + "','" + dt.Rows[i]["cRemarks"] + "')";

                                //添加数据
                                cmd.ExecuteNonQuery();
                            }
                            //关闭连接
                            cn.Close();
                            XtraMessageBox.Show("保存成功！");

                        }
                        catch (Exception err)
                        {
                            XtraMessageBox.Show(err.Message);
                        }
                    }
                }
            };
        }
        /// <summary>
        /// 通过pid获得人员的主要经历，
        /// </summary>
        /// <param name="pid">人员唯一标识，pid</param>
        /// <param name="wrap">是否一条记录之后换行</param>
        /// <returns></returns>
        public static string getResumeBypid(string pid,bool wrap=false)
        {
            StringBuilder resume = new StringBuilder("");
            DataTable dt_resume = MySQLHelper.table("SELECT *FROM data_resume WHERE PersionID='" + pid + "'");
            if (dt_resume != null && dt_resume.Rows.Count > 0)
            {
                foreach (DataRow r in dt_resume.Rows)
                {
                    resume.Append(r["dStartDate"]+ "-"+ r["dDeadline"]+","+ r["cExperience"]+"；");
                    if (wrap)
                        resume.Append("\r\n");
                }
                
            }
            return resume.ToString();
        }
        /// <summary>
        /// 通过pid获得人员的奖惩情况
        /// </summary>
        /// <param name="pid">人员唯一标识</param>
        /// <param name="wrap">是否一条记录之后换行</param>
        /// <returns></returns>
        public static string getRewardsBypid(string pid, bool wrap = false)
        {
            StringBuilder rewards = new StringBuilder("");
            DataTable dt_rewards = MySQLHelper.table("SELECT *FROM data_rewards_punishments WHERE PersionID='" + pid + "'");
            if (dt_rewards != null && dt_rewards.Rows.Count > 0)
            {
                foreach (DataRow r in dt_rewards.Rows)
                {
                    rewards.Append(r["dData"] + "," + r["cDetailed"] + "；");
                    if (wrap)
                        rewards.Append("\r\n");
                }
            }
            return rewards.ToString();

        }
        /// <summary>
        /// 通过pid获得人员的年度审核结果
        /// </summary>
        /// <param name="pid">人员唯一标识</param>
        /// <param name="wrap">是否一条记录之后换行</param>
        /// <returns></returns>
        public static string getCheckresultBypid(string pid, bool wrap = false)
        {
            StringBuilder checkresult = new StringBuilder("");
            DataTable dt_checkresult = MySQLHelper.table("SELECT *FROM data_checkresult WHERE PersionID='" + pid + "'");
            if (dt_checkresult != null && dt_checkresult.Rows.Count > 0)
            {
                foreach (DataRow r in dt_checkresult.Rows)
                {
                    checkresult.Append(r["dcrYear"] + "年度考核为：" + r["crChechResult"] + "；");
                    if (wrap)
                        checkresult.Append("\r\n");
                }

            }
            return checkresult.ToString();

        }
        public static void ExportExcel1(string name)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())    //离开后销毁对话框
            {
                //string name = gvType.GetFocusedDataRow()["cName"].ToString() + "-";
                saveDialog.FileName = name + DateTime.Now.ToString("yyyyMMdd");
                saveDialog.Filter = "Excel 97-2013 工作簿(*.xls)|*.xls";
                if (saveDialog.ShowDialog() != DialogResult.Cancel)  //用户点击保存按钮
                {
                    string exportFilePath = saveDialog.FileName;        //定义文件路径              
                    string fileExtenstion = new FileInfo(exportFilePath).Extension; //创建文件
                    if (saveDialog.FileName.Trim().Length > 0)
                    {
                        byte[] excel = Properties.Resources.模版;
                        FileStream stream = new FileStream(saveDialog.FileName, FileMode.Create);
                        stream.Write(excel, 0, excel.Length);
                        stream.Close();
                        stream.Dispose();
                        //XtraMessageBox.Show("保存成功", "提示");

                        string Path = exportFilePath;
                        if (Path == "")
                            return;
                        try
                        {
                            string sConnectionString = "Provider=Microsoft.JET.OLEDB.4.0;Data Source=" + Path + ";" + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=0;'";
                            OleDbConnection cn = new OleDbConnection(sConnectionString);
                            string c = "INSERT INTO [后备干部$] (本人姓名,出生年月,年度,级别,备注) VALUES('lxm','1995/07','1995','1','2')";
                            OleDbCommand cmd = new OleDbCommand(c, cn);
                            //创建Excel文件
                            cn.Open();
                            //添加数据
                            cmd.ExecuteNonQuery();
                            //关闭连接
                            cn.Close();
                        }
                        catch (OleDbException err)
                        {
                            XtraMessageBox.Show(err.Message);
                        }
                    }
                }
            };
        }

        public static void ExportWord(DateTime date, DataTable dt, string name)
        {
            try
            {
                object oMissing = System.Reflection.Missing.Value;
                //创建一个Word应用程序实例  
                Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
                //设置为不可见  
                oWord.Visible = false;
                //模板文件地址，这里假设在X盘根目录  
                object oTemplate = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "model//word.doc";
                //以模板为基础生成文档  
                Microsoft.Office.Interop.Word._Document oDoc = oWord.Documents.Add(ref oTemplate, ref oMissing, ref oMissing, ref oMissing);

                #region //声明书签数组  
                object[] oBookMark = new object[63];
                //赋值书签名  
                oBookMark[0] = "cName";
                oBookMark[1] = "cSex";
                oBookMark[2] = "dBirth_date";
                oBookMark[3] = "cEage";
                oBookMark[4] = "cNation";
                oBookMark[5] = "cNativePlace";
                oBookMark[6] = "cBirthPlace";
                oBookMark[7] = "dJoin_date";
                oBookMark[8] = "dWorkDate";
                oBookMark[9] = "cHealthStatus";
                oBookMark[10] = "cDuties";
                oBookMark[11] = "cSkill";
                oBookMark[12] = "cFull_timeEducation";
                oBookMark[13] = "cFull_timeDegree";
                oBookMark[14] = "cFull_timeSchool";
                oBookMark[15] = "cFull_timeMajor";
                oBookMark[16] = "cIn_serviceEducation";
                oBookMark[17] = "cIn_serviceDegree";
                oBookMark[18] = "cIn_serviceSchool";
                oBookMark[19] = "cIn_serviceMajor";
                oBookMark[20] = "cCurrentJob";
                oBookMark[21] = "cProposedJob";
                oBookMark[22] = "cRemoveJob";


                //赋值任意数据到书签的位置  
                oDoc.Bookmarks.get_Item(ref oBookMark[0]).Range.Text = dt.Rows[0]["cName"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[1]).Range.Text = dt.Rows[0]["cSex"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[2]).Range.Text = dt.Rows[0]["dBirth_date"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[3]).Range.Text = dt.Rows[0]["cAge"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[4]).Range.Text = dt.Rows[0]["cNation"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[5]).Range.Text = dt.Rows[0]["cNativePlace"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[6]).Range.Text = dt.Rows[0]["cBirthPlace"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[7]).Range.Text = dt.Rows[0]["dJoin_date"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[8]).Range.Text = dt.Rows[0]["dWorkDate"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[9]).Range.Text = dt.Rows[0]["cHealthStatus"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[10]).Range.Text = dt.Rows[0]["cDuties"].ToString();

                oDoc.Bookmarks.get_Item(ref oBookMark[11]).Range.Text = dt.Rows[0]["cSkill"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[12]).Range.Text = dt.Rows[0]["cFull_timeEducation"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[13]).Range.Text = dt.Rows[0]["cFull_timeDegree"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[14]).Range.Text = dt.Rows[0]["cFull_timeSchool"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[15]).Range.Text = dt.Rows[0]["cFull_timeMajor"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[16]).Range.Text = dt.Rows[0]["cIn_serviceEducation"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[17]).Range.Text = dt.Rows[0]["cIn_serviceDegree"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[18]).Range.Text = dt.Rows[0]["cIn_serviceSchool"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[19]).Range.Text = dt.Rows[0]["cIn_serviceMajor"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[20]).Range.Text = dt.Rows[0]["cCurrentJob"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[21]).Range.Text = dt.Rows[0]["cProposedJob"].ToString();
                oDoc.Bookmarks.get_Item(ref oBookMark[22]).Range.Text = dt.Rows[0]["cRemoveJob"].ToString();

                #endregion
                using (SaveFileDialog sfd = new SaveFileDialog())    //离开后销毁对话框
                {
                    //string name = gvType.GetFocusedDataRow()["cName"].ToString() + "-";
                    sfd.FileName = name;
                    sfd.Filter = "Word Document(*.doc)|*.doc";
                    if (sfd.ShowDialog() != DialogResult.Cancel)  //用户点击保存按钮
                    {
                        //string exportFilePath = sfd.FileName;        //定义文件路径              
                        //string fileExtenstion = new FileInfo(exportFilePath).Extension; //创建文件
                        if (sfd.FileName.Trim().Length > 0)
                        {
                            object filename = sfd.FileName;

                            oDoc.SaveAs(ref filename, ref oMissing, ref oMissing, ref oMissing,
                            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                            ref oMissing, ref oMissing);
                            oDoc.Close(ref oMissing, ref oMissing, ref oMissing);
                            //关闭word  
                            oWord.Quit(ref oMissing, ref oMissing, ref oMissing);
                        }
                        XtraMessageBox.Show("保存成功", "提示");
                    }
                }
            }
            catch (Exception err)
            {
                XtraMessageBox.Show(err.Message);
            }
        }
        /// <summary>
        /// seve log in app's path
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static string savelog(string log)
        {
            string path= AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log//"+DateTime.Now.ToString("yyyyMMddHHmmss") +".txt";
            StreamWriter sw = new StreamWriter(path, true);
            //向创建的文件中写入内容
            sw.WriteLine(log);
            //关闭当前文件写入流
            sw.Close();
            return path;

        }

        /// <summary>
        /// 往excel指定位置插入值
        /// </summary>
        /// <param name="ws">工作表对象</param>
        /// <param name="x">x轴坐标</param>
        /// <param name="y">y轴坐标</param>
        /// <param name="value"></param>
        private static void SetCellValue (Microsoft.Office.Interop.Excel.Worksheet ws, int x, int y, object value)
        {
            ws.Cells[x, y] = value;
        }

        private static void insertIntoExcel(Microsoft.Office.Interop.Excel.Worksheet ws, DataRow dr)
        {
            SetCellValue(ws, 5, 4, dr["cName"].ToString());
            SetCellValue(ws, 5, 8, dr["cSex"].ToString());
            SetCellValue(ws, 5, 13, dr["dBirth_date"].ToString() + "\n" + "（" + dr["cAge"].ToString() + "岁）");
            SetCellValue(ws, 6, 4, dr["cNation"].ToString());
            SetCellValue(ws, 6, 8, dr["cNativePlace"].ToString());
            SetCellValue(ws, 6, 13, dr["cBirthPlace"].ToString());
            SetCellValue(ws, 7, 4, dr["dJoin_date"].ToString());
            SetCellValue(ws, 7, 8, dr["dWorkDate"].ToString());
            SetCellValue(ws, 7, 13, dr["cHealthStatus"].ToString());

            SetCellValue(ws, 8, 4, dr["cDuties"].ToString());
            SetCellValue(ws, 8, 10, dr["cSkill"].ToString());

            SetCellValue(ws, 9, 6, dr["cFull_timeEducation"].ToString());
            SetCellValue(ws, 9, 13, dr["cFull_timeSchool"].ToString() + dr["cFull_timeMajor"].ToString());

            SetCellValue(ws, 10, 6, dr["cIn_serviceEducation"].ToString());
            SetCellValue(ws, 10, 13, dr["cIn_serviceSchool"].ToString() + dr["cIn_serviceMajor"].ToString());

            SetCellValue(ws, 11, 6, dr["cCurrentJob"].ToString());
            SetCellValue(ws, 12, 6, dr["cProposedJob"].ToString());
            SetCellValue(ws, 13, 6, dr["cRemoveJob"].ToString());

            SetCellValue(ws, 15, 3, getResumeBypid(dr["pid"].ToString(),wrap:true));

            SetCellValue(ws, 51, 4, getRewardsBypid(dr["pid"].ToString()));
            SetCellValue(ws, 55, 4, getCheckresultBypid(dr["pid"].ToString()));
            SetCellValue(ws, 58, 4, dr["cDismissalReason"].ToString());

            DataTable dt_familymembers = MySQLHelper.table("SELECT *FROM data_familymembers WHERE PersionID='" + dr["pid"].ToString() + "'");
            for (int i = 0; i < 7 && i < dt_familymembers.Rows.Count; i++)
            {
                SetCellValue(ws, 63 + i, 3, dt_familymembers.Rows[i]["cfCalled"].ToString());
                SetCellValue(ws, 63 + i, 5, dt_familymembers.Rows[i]["cfName"].ToString());
                SetCellValue(ws, 63 + i, 7, GetAgeByBirthdate(Convert.ToDateTime(dt_familymembers.Rows[i]["dfBirthDate"].ToString() + ".01"), date1));
                SetCellValue(ws, 63 + i, 9, dt_familymembers.Rows[i]["cfPoliticalStatus"].ToString());
                SetCellValue(ws, 63 + i, 11, dt_familymembers.Rows[i]["cfDuties"].ToString());
            }


        }
        /// <summary>
        /// 导出任免审批表excel文件
        /// </summary>
        /// <param name="dt">人员信息主表，需要包含pid列，标识唯一人员</param>
        /// <param name="name">文件名</param>
        /// <param name="SheetName">excel工作表名，默认为Sheet1</param>
        /// <returns></returns>
        public static bool seveExcel(DateTime date,DataRow dr,string name, int SheetNum=1)
        {
            date1 = date;
            bool success = false;
            //创建一个Word应用程序实例  
            Microsoft.Office.Interop.Excel._Application oExcel = new Microsoft.Office.Interop.Excel.Application();
            //设置为不可见  
            oExcel.Visible = false;
            //审批表模版路径
            string path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "model//excel.xls";
            //Microsoft.Office.Interop.Excel.Workbooks wbs = oExcel.Workbooks;


            try
            {
                Microsoft.Office.Interop.Excel.Workbook wb = oExcel.Workbooks._Open(path,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                Microsoft.Office.Interop.Excel.Worksheet ws = wb.Worksheets[SheetNum];
                insertIntoExcel(ws, dr);

                using (SaveFileDialog sfd = new SaveFileDialog())    //离开后销毁对话框
                {
                    //string name = gvType.GetFocusedDataRow()["cName"].ToString() + "-";
                    sfd.FileName = name;
                    sfd.Filter = "Excel 97-2013 工作簿(*.xls)|*.xls";
                    if (sfd.ShowDialog() != DialogResult.Cancel)  //用户点击保存按钮
                    {
                        //string exportFilePath = sfd.FileName;        //定义文件路径              
                        //string fileExtenstion = new FileInfo(exportFilePath).Extension; //创建文件
                        if (sfd.FileName.Trim().Length > 0)
                        {
                            object filename = sfd.FileName;
                            wb.SaveAs(filename, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                            wb.Close(Type.Missing, Type.Missing, Type.Missing);
                            //关闭word  
                            oExcel.Quit();
                            XtraMessageBox.Show("保存成功", "提示");
                            success= true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                KillExcel(oExcel);
            }
            return success;
        }

        [DllImport("User32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);
        private static void KillExcel(Microsoft.Office.Interop.Excel._Application theApp)
        {
            int id = 0;
            IntPtr intptr = new IntPtr(theApp.Hwnd);
            System.Diagnostics.Process p = null;
            try
            {
                GetWindowThreadProcessId(intptr, out id);
                p = System.Diagnostics.Process.GetProcessById(id);
                if (p != null)
                {
                    p.Kill();
                    p.Dispose();
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 获得年龄
        /// </summary>
        /// <param name="birthdate"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetAgeByBirthdate(DateTime birthdate, DateTime date)
        {
            int age = date.Year - birthdate.Year;
            //if (date.Month < birthdate.Month || (date.Month == birthdate.Month && date.Day < birthdate.Day))
            if (date.Month < birthdate.Month)
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        public static string FormatMajor(DataTable dt)
        {
            StringBuilder s = new StringBuilder("(");
            foreach (DataRow r in dt.Rows)
            {
                s.Append("'"+r["cSubject"] +"',");
            }
            s.Append("' ')");
            return s.ToString();
        }
    }
}

using BarCode;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PersonnelManagement
{
    public class SaveFile
    {
        /// <summary>
        /// 保存对话框
        /// </summary>
        /// <param name="s"></param>
        /// <param name="name"></param>
        private static void saveFile(string s, string name)
        {

            using (SaveFileDialog saveDialog = new SaveFileDialog())    //离开后销毁对话框
            {
                saveDialog.FileName = name + DateTime.Now.ToString("yyyyMMdd");


                saveDialog.Filter = "任免表(.lrm)|*.lrm";
                Start: if (saveDialog.ShowDialog() != DialogResult.Cancel)  //用户点击保存按钮
                {
                    string exportFilePath = saveDialog.FileName;        //定义文件路径              
                    string fileExtenstion = new FileInfo(exportFilePath).Extension; //创建文件

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
        /// <param name="dt"></param>
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

        public static void ExportLrm(DataTable dt, string name = "")
        {
            saveFile(formString(dt), name);
        }
        public static void ExportExcel(string name)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())    //离开后销毁对话框
            {
                //string name = gvType.GetFocusedDataRow()["cName"].ToString() + "-";
                saveDialog.FileName = name + DateTime.Now.ToString("yyyyMMdd");
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
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
                            string sConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";" + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=0;'";
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

        public static void ExportWord(DataTable dt, string name)
        {
            try
            {
                object oMissing = System.Reflection.Missing.Value;
                //创建一个Word应用程序实例  
                Microsoft.Office.Interop.Word._Application oWord = new Microsoft.Office.Interop.Word.Application();
                //设置为不可见  
                oWord.Visible = false;
                //模板文件地址，这里假设在X盘根目录  
                object oTemplate = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "model//word.doc";
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
    }
}

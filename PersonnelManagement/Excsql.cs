using BarCode;
using DevExpress.XtraEditors;
using System;
using System.Configuration;
using System.IO;

namespace PersonnelManagement
{
    public class Excsql
    {
        public static bool isUpdate()
        {
            bool b = false;
            if (ConfigurationManager.AppSettings["Update"] == "0")
            {
                Excsql.ExecuteSqlScript("");
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//获取Configuration对象
                config.AppSettings.Settings["Update"].Value = "1";//key的值设置为1
                config.Save(ConfigurationSaveMode.Modified);    //保存
                ConfigurationManager.RefreshSection("appSettings");//刷新
                b = true;
            }
            return b;
        }
        public static int ExecuteSqlScript(string sqlFile)
        {
            if(sqlFile.Length==0)
                sqlFile = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "sql//data_majorinf.sql";
            int returnValue = -1;
            int sqlCount = 0, errorCount = 0;
            if (!File.Exists(sqlFile))
            {
                //Log.WriteLog(string.Format("sql file not exists!", sqlFile));
                XtraMessageBox.Show("文件不存在");
                return -1;
            }
            using (StreamReader sr = new StreamReader(sqlFile))
            {
                string line = string.Empty;
                char spaceChar = ' ';
                string newLIne = "\r\n", semicolon = ";";
                string sprit = "/", whiffletree = "-";
                string sql = string.Empty;
                do
                {
                    line = sr.ReadLine();
                    // 文件结束
                    if (line == null) break;
                    // 跳过注释行
                    if (line.StartsWith(sprit) || line.StartsWith(whiffletree)) continue;
                    // 去除右边空格
                    line = line.TrimEnd(spaceChar);
                    sql += line;
                    // 以分号(;)结尾，则执行SQL
                    if (sql.EndsWith(semicolon))
                    {
                        try
                        {
                            sqlCount++;
                            MySQLHelper.ExecuteNonQuery(sql, null);
                        }
                        catch (Exception ex)
                        {
                            errorCount++;
                            //Log.WriteLog(sql + newLIne + ex.Message);
                        }
                        sql = string.Empty;
                    }
                    else
                    {
                        // 添加换行符
                        if (sql.Length > 0) sql += newLIne;
                    }
                } while (true);
            }
            if (sqlCount > 0 && errorCount == 0)
                returnValue = 1;
            if (sqlCount == 0 && errorCount == 0)
                returnValue = 0;
            else if (sqlCount > errorCount && errorCount > 0)
                returnValue = -1;
            else if (sqlCount == errorCount)
                returnValue = -2;
            return returnValue;
        }
    }
}

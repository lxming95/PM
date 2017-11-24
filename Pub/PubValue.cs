using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using System.Runtime.InteropServices;

namespace Pub
{
    static public class PubValue
    {
        /// <summary>
        /// 执行程序目录
        /// </summary>
        static public string ExePath = "";

        /// <summary>
        /// 登录日期
        /// </summary>
        static public DateTime LoginDate;

        /// <summary>
        /// 登录的计算机名
        /// </summary>
        static public string ComputerName;

        /// <summary>
        /// 用户ID
        /// </summary>
        static public int UserID;

        /// <summary>
        /// 用户号
        /// </summary>
        static public string UserCode = "";

        /// <summary>
        /// 用户名
        /// </summary>
        static public string UserName = "";

        /// <summary>
        /// 服务器
        /// </summary>
        static public string Server;

        /// <summary>
        /// SA密码
        /// </summary>
        static public string SAPass;

        /// <summary>
        /// 主窗体
        /// </summary>
        static public Form frmmain;

        /// <summary>
        /// 系统字段：制单人
        /// </summary>
        static public string sysMaker
        {
            get { return UserName; }
        }

        /// <summary>
        /// 系统字段：制单日期
        /// </summary>
        static public DateTime sysMakerDate
        {
            get { return DateTime.Today; }
        }

        /// <summary>
        /// 系统字段：审核人
        /// </summary>
        static public string sysVerifier
        {
            get { return UserName; }
        }

        /// <summary>
        /// 系统字段：审核日期
        /// </summary>
        static public DateTime sysVerifierDate
        {
            get { return DateTime.Today; }
        }

        /// <summary>
        /// 许可数HashTable
        /// </summary>
        static public Hashtable htLicense;

        /// <summary>
        /// 条码系统数据库链接字符串
        /// </summary>
        public static string ConnectionString { get; set; }
        /// <summary>
        /// 用友数据库链接字符串
        /// </summary>
        public static string ConnectionStringUFData { get; set; }
        /// <summary>
        /// 用友系统数据库链接字符串
        /// </summary>
        public static string ConnectionStringUFSystem { get; set; }

        /// <summary>
        /// 系统数据库名
        /// </summary>
        public static string DBName { get; set; }

        /// <summary>
        /// 系统服务器名称
        /// </summary>
        public static string ServerName { get; set; }

        /// <summary>
        /// SA密码
        /// </summary>
        public static string SA { get; set; }
    }

    
}

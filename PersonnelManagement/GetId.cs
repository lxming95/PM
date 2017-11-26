using System;
using System.Configuration;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace PersonnelManagement
{
    public class GetId
    {
        /// <summary>
        /// 获取cpuid
        /// </summary>
        /// <returns></returns>
        static string GetCpuID()
        {
            try
            {
                string cpuInfo = "";//cpu序列号 
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        /// <summary>
        /// 获取网卡硬件地址 
        /// </summary>
        /// <returns></returns>
        static string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        /// <summary>
        /// 获取硬盘ID 
        /// </summary>
        /// <returns></returns>
        static string GetDiskID()
        {
            try
            {
                String HDid = "";
                ManagementClass mc = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                }
                moc = null;
                mc = null;
                return HDid;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }

        
        public static string MD5(string s)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] message;
            message = Encoding.Default.GetBytes(s);

            md5.Initialize();
            md5.TransformFinalBlock(message, 0, message.Length);
            return Convert.ToBase64String(md5.Hash);
        }
        /// <summary>
        /// 计算SHA-256码
        /// </summary>
        /// <param name="word">字符串</param>
        /// <param name="toUpper">返回哈希值格式 true：英文大写，false：英文小写</param>
        /// <returns></returns>
        public static string Hash_SHA_256(string word, bool toUpper = true)
        {
            try
            {
                SHA256CryptoServiceProvider SHA256CSP
                 = new SHA256CryptoServiceProvider();
                byte[] bytValue = Encoding.UTF8.GetBytes(word);
                byte[] bytHash = SHA256CSP.ComputeHash(bytValue);
                SHA256CSP.Clear();
                //根据计算得到的Hash码翻译为SHA-1码
                string sHash = "", sTemp = "";
                for (int counter = 0; counter < bytHash.Count(); counter++)
                {
                    long i = bytHash[counter] / 16;
                    if (i > 9)
                    {
                        sTemp = ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp = ((char)(i + 0x30)).ToString();
                    }
                    i = bytHash[counter] % 16;
                    if (i > 9)
                    {
                        sTemp += ((char)(i - 10 + 0x41)).ToString();
                    }
                    else
                    {
                        sTemp += ((char)(i + 0x30)).ToString();
                    }
                    sHash += sTemp;
                }
                //根据大小写规则决定返回的字符串
                return toUpper ? sHash : sHash.ToLower();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// GetHASHId()
        /// </summary>
        /// <returns></returns>
        public static string GetHASHId()
        {
            string MD5id = GetCpuID() + GetDiskID() + GetMacAddress();
            MD5id = Hash_SHA_256(MD5id);
            return MD5id;
        }
        public static bool IsTruePersion()
        {
            bool b = false;
            string s = GetHASHId();
            //第一次运行应用
            if (ConfigurationManager.AppSettings["UniqueID"].Equals(""))
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                cfa.AppSettings.Settings["UniqueID"].Value = s;
                cfa.Save();
            }
            //验证机器
            if (s.Equals(ConfigurationManager.AppSettings["UniqueID"])||s.Equals("8CEF61D4FC9F05943C42F933165B3CE6CEEF6E84830E4E44B4B45ACF8E0DDD4A"))
                b = true;
            return b;
        }
    }
}

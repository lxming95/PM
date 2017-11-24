using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace PersonnelManagement
{
    static class awQurry
    {
        /// <summary>
        /// 查询不直接传字符串
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        static public DataTable Qurry(string sql)
        {
            SqlConnection con = new SqlConnection();         //获取Configuration对象
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();//根据Key读取<add>元素的Value
            con.ConnectionString = ConnectionString;         //配置连接属性
            con.Open();                                      //打开连接
            SqlCommand com = new SqlCommand();               //获取SqlCommand对象
            com.Connection = con;                            //设置连接对象
            com.CommandType = CommandType.Text;              //设置sql语句类型
            com.CommandText = sql;                           //添加sql语句
            SqlDataAdapter adapter = new SqlDataAdapter(com);//执行SQL语句
            DataTable dt = new DataTable();                  //创建返回datatable表
            adapter.Fill(dt);                                //填充datatable表
            con.Close();                                     //关闭数据库
            con.Dispose();                                   //释放连接
            return dt;                                       //返回datatable表
        }
        /// <summary>
        /// 查询，传入SqlParameter
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sp">SqlParameter类型的一个参数或者参数数组</param>
        /// <returns></returns>
        static public DataTable Qurry(string sql, SqlParameter[] sp)
        {
            SqlConnection con = new SqlConnection();         //获取Configuration对象
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();//根据Key读取<add>元素的Value
            con.ConnectionString = ConnectionString;         //配置连接属性
            con.Open();                                      //打开连接
            SqlCommand com = new SqlCommand();               //获取SqlCommand对象
            com.Connection = con;                            //设置连接对象
            com.CommandType = CommandType.Text;              //设置sql语句类型
            com.CommandText = sql;                           //添加sql语句
            com.Parameters.AddRange(sp);                     //添加 参数数组
            SqlDataAdapter adapter = new SqlDataAdapter(com);//执行SQL语句
            DataTable dt = new DataTable();                  //创建返回datatable表
            adapter.Fill(dt);                                //填充datatable表
            con.Close();                                     //关闭数据库
            con.Dispose();                                   //释放连接
            return dt;                                       //返回datatable表
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">string sql语句 </param>
        /// <returns></returns>
        static public int ExSql(string sql)
        {
            string MyUpdate = sql;                      //定义sql语句
            SqlConnection con = new SqlConnection();    //获取Configuration对象
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();//根据Key读取<add>元素的Value
            con.ConnectionString = ConnectionString;    //配置连接属性
            con.Open();                                 //打开连接
            SqlCommand com = new SqlCommand();          //获取SqlCommand对象
            com.Connection = con;                       //设置连接对象
            com.CommandType = CommandType.Text;         //设置sql语句类型
            com.CommandText = sql;                      //添加sql语句
            int x = com.ExecuteNonQuery();              //执行sql语句并返回影响的行数
            con.Close();                                //关闭数据库
            con.Dispose();                              //释放数据库连接
            return x;                                   //返回影响的行数
        }
        /// <summary>
        /// 查询一个时间段的统计信息
        /// </summary>
        /// <param name="date1">传入一个起始时间，字符串类型</param>
        /// <param name="date2">传入一个终止时间，字符串类型</param>
        /// <param name="line">传入一个线路，字符串类型</param>
        /// <param name="site">传入一个站点，字符串类型</param>
        /// <returns></returns>
        static public DataTable QurryTotal(string date1, string date2,string line, string site)
        {
            DataTable dt = new DataTable();             //创建datatable
            if (date1 == "")                            //起始时间为空则设置为1970年
                date1 = "1970/1/1 00:00:00";
            if (date2 == "")                            //终止时间为空则设置为现在时间
                date2 = DateTime.Now.ToString();
            string where="";                            //初始化where字段，where字段是根据不同传入参数的情况来拼接不同的where语句
            if (line != "" && site == "")               //线路不为空，站点为空的情况
                where = " where t.kh_name in (select cStationName from AA_Station where cStationLine= @Line )";
            else if (line == "" && site != "")          //线路为空，站点不为空的情况
                where = " where t.kh_name = @Site ";
            else if (line != "" && site != "")          //线路不为空，站点不为空的情况
                where = " where t.kh_name= @Site and t.kh_name in (select cStationName from AA_Station where cStationLine= @Line )";
            else                                        //线路，站点都为空的情况
                where = ""; 
            string sql = "select cShipmentType as type,iPayMoney as pay,iTotalMoney as total from data_ShipmentType";
           
            DataTable dtType = Qurry(sql);               //获得所有快件类型

            //添加站点，线路的SqlParameter
            SqlParameter[] paras = new SqlParameter[]{ new SqlParameter("@Line", line),new SqlParameter("@Site", site)};

            // sql语句
            sql = "select t.kh_name as site,t.kh_name as site2,t.g_stockspec as type,remainSum,outSum from"
            + "(select distinct kh_name,g_stockspec from data_printlog)as t"
            + " left join"
            + " (select COUNT(do_flag) as remainSum,g_stockspec,kh_name from data_printlog where do_flag=2 and print_date >= '"
            + date1 + "' and print_date <= '" + date2 + "' group by g_stockspec,kh_name)as t1"
            + " on t.g_stockspec=t1.g_stockspec and t.kh_name=t1.kh_name"
            + " left join"
            + " (select COUNT(do_flag) as outSum,g_stockspec,kh_name from data_printlog where do_flag=3 and out_date >= '"
            + date1 + "' and out_date <= '" + date2 + "' group by g_stockspec,kh_name)as t2"
            + " on t.kh_name=t2.kh_name and t.g_stockspec=t2.g_stockspec "+ where + " order by site,type";

            dt = Qurry(sql, paras);                       //获得查询结果
            dt.Columns.Add("TotalSum", typeof(decimal));  //添加TotalSum金额合计行
            dt.Columns.Add("TotalMoney", typeof(decimal));//添加TotalMoney总金额行
            dt.Columns.Add("EarnMoney", typeof(decimal)); //添加EarnMoney盈利金额行
            dt.Columns.Add("PayMoney", typeof(decimal));  //添加PayMoney支付金额行
            for (int r = 0; r < dt.Rows.Count; r++)       //填充新增行的各行数据
            {
                for (int c = 0; c < dtType.Rows.Count; c++)
                {
                    //运用三目运算符，避免转换decimal失败，计算总计快件数
                    dt.Rows[r]["TotalSum"] = (dt.Rows[r]["remainSum"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[r]["remainSum"])) + (dt.Rows[r]["outSum"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[r]["outSum"]));
                    if (dt.Rows[r]["type"].Equals(dtType.Rows[c]["type"]))//类型相同计算金额
                    {
                        //运用三目运算符，避免转换decimal失败，计算总金额
                        dt.Rows[r]["TotalMoney"] = dtType.Rows[c]["total"] == DBNull.Value ? 0 : Convert.ToDecimal(dtType.Rows[c]["total"]) * (dt.Rows[r]["outSum"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[r]["outSum"]));
                        //运用三目运算符，避免转换decimal失败，计算支付金额
                        dt.Rows[r]["PayMoney"] = dtType.Rows[c]["pay"] == DBNull.Value ? 0 : Convert.ToDecimal(dtType.Rows[c]["pay"]) * (dt.Rows[r]["outSum"] == DBNull.Value ? 0 : Convert.ToDecimal(dt.Rows[r]["outSum"]));
                        //运用三目运算符，避免转换decimal失败，计算盈利金额
                        dt.Rows[r]["EarnMoney"] = Convert.ToDecimal(dt.Rows[r]["TotalMoney"]) - Convert.ToDecimal(dt.Rows[r]["PayMoney"]);

                    }
                }
            }
            return dt;
        }
        /// <summary>
        /// 查询一个时间段内的清单
        /// </summary>
        /// <param name="date1">传入一个起始时间，字符串类型</param>
        /// <param name="date2">传入一个终止时间，字符串类型</param>
        /// <param name="line">传入一个线路，字符串类型</param>
        /// <param name="site">传入一个站点，字符串类型</param>
        /// <returns></returns>
        static public DataTable QurryList(string date1, string date2,string line,string site)
        {
            DataTable dtList = new DataTable();     //创建datatable
            if (date1 == "")
                date1 = "1970/1/1 00:00:00";        //起始时间为空则设置为1970年
            if (date2 == "")
                date2 = DateTime.Now.ToString();    //终止时间为空则设置为现在时间
            string sql = "";
            //添加站点，线路的SqlParameter
            SqlParameter[] paras = new SqlParameter[] { new SqlParameter("@Line", line), new SqlParameter("@Site", site) };
            string where = "";                      //初始化where字段，where字段是根据不同传入参数的情况来拼接不同的where语句
            if (line != "" && site == "")           //线路不为空，站点为空的情况
                where = " and t1.kh_name in (select cStationName from AA_Station where cStationLine= @Line ) ";
            else if (line == "" && site != "")      //线路为空，站点不为空的情况
                where = " and t1.kh_name  = @Site ";
            else if (line != "" && site != "")      //线路不为空，站点不为空的情况
                where = " and t1.kh_name  = @Site and t1.kh_name in (select cStationName from AA_Station where cStationLine= @Line ) ";
            else                                    //线路站点都为空的情况
                where = "";
            //sql语句
            sql = "select g_sz as name,g_jd as phone,g_ccdcode as code,g_zxname as expressName,kh_name as site,kh_name as site2,g_stockspec as type,iTotalMoney,iPayMoney,iEarnMoney,out_date as date from data_printlog t1 "
                + "left join data_ShipmentType t2 "
                + "on t1.g_stockspec=t2.cShipmentType "
                + "where do_flag=3 and out_date >='" + date1 + "' and out_date <= '" + date2 + "' "+where+"  order by site,out_date,type";
            dtList = Qurry(sql, paras);             //获得查询结果
            return dtList;                          //返回查询结果
        }

    }
}

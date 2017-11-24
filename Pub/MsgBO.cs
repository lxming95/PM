using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pub
{
    /// <summary>
    /// 消息传递类
    /// </summary>
    public class MsgBO
    {
        /// <summary>
        /// 成功True或失败False
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Msg { get; set; }
    }
}

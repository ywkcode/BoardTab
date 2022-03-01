using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardTab.Common
{
    public class PageResponse
    {
        /// <summary>
        /// 返回消息 成功或者错误的信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// HTTP状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Result
        /// </summary>
        public dynamic Result { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// PageResponse
        /// </summary>
        public PageResponse()
        {

            Message = "操作成功";
            Code = 200;
            Status = true;
        }
    }
}

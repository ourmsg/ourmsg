using System;
using System.Collections.Generic;
using System.Text;

namespace IMLibrary3.Protocol 
{
    /// <summary>
    /// 协议类型
    /// </summary>
    public enum type
    {
        /// <summary>
        /// 无操作
        /// </summary>
        None,
        /// <summary>
        /// 新增
        /// </summary>
        New,
        /// <summary>
        /// 获取
        /// </summary>
        get,
        /// <summary>
        /// 设置
        /// </summary>
        set,
        /// <summary>
        /// 回复结果
        /// </summary>
        result,
        /// <summary>
        /// 删除
        /// </summary>
        delete,
        /// <summary>
        /// 取消
        /// </summary>
        cancel,
        /// <summary>
        /// 错误
        /// </summary>
        error,
        /// <summary>
        /// 其他
        /// </summary>
        Else,
        /// <summary>
        /// 结束
        /// </summary>
        over,
    }

    
}

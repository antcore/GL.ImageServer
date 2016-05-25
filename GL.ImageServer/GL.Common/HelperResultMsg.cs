namespace GL.Common
{
    /// <summary>
    /// 操作通知消息 
    /// ResultMsg msg = new ResultMsg();  
    /// </summary> 
    public class HelperResultMsg
    {
        public HelperResultMsg()
        {
            success = false;
            codeState = 0;
            message = "操作失败";
            resultInfo = string.Empty;
        }

        /// <summary>
        /// 操作成功与否标志 true成功 false失败 默认false
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 操作状态编码 默认0
        /// </summary>
        public int codeState { get; set; }

        /// <summary>
        /// 操作提示消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 备用返回字段 有需要使用 默认空
        /// </summary>
        public object resultInfo { get; set; }
    }
}
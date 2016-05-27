namespace GL.DBOptions.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GL_CloudSetting
    {
        [Key]
        [StringLength(32)]
        public string sId { get; set; }

        /// <summary>
        /// 服务器名称
        /// </summary>
        [Required]
        [StringLength(30)]
        public string sServerName { get; set; }
        /// <summary>
        /// 服务器地址  http://img.gl.cn  http://127.0.0.1
        /// </summary>
        [Required]
        [StringLength(50)]
        public string sServerUriDomain { get; set; }
        /// <summary>
        /// 文件存储于服务器盘符  默认 C:
        /// </summary>
        [Required]
        [StringLength(20)]
        public string sSaveDisc { get; set; }
        /// <summary>
        /// 文件存储文件服务器路径   默认 /UpLoad 以/开头
        /// </summary>
        [Required]
        [StringLength(50)]
        public string sSavePath { get; set; }

        /// <summary>
        /// 服务器  唯一标识码  用以区分不同服务器信息 
        /// </summary>
        [Required]
        [StringLength(32)]
        public string sServerCode { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        [StringLength(250)]
        public string sRemark { get; set; }
        /// <summary>
        /// 默认服务 默认[0]否 1是
        /// </summary>
        public bool bIsDefault { get; set; }
        /// <summary>
        /// 数据可用状态 默认[1]有效  0 无效
        /// </summary>
        public bool bState { get; set; }
        /// <summary>
        /// 逻辑删除标识 默认[0]未删除 1已删除
        /// </summary>
        public bool bIsDelete { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime dCreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? dUpdateTime { get; set; }
    }
}

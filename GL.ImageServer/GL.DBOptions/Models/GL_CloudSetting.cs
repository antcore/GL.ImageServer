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

        [Required]
        [StringLength(30)]
        public string sServerName { get; set; }

        [Required]
        [StringLength(50)]
        public string sServerUriDomain { get; set; }

        [Required]
        [StringLength(20)]
        public string sSaveDisc { get; set; }

        [Required]
        [StringLength(50)]
        public string sSavePath { get; set; }

        [Required]
        [StringLength(32)]
        public string sServerCode { get; set; }

        [StringLength(250)]
        public string sRemark { get; set; }

        public bool bIsDefault { get; set; }

        public bool bState { get; set; }

        public bool bIsDelete { get; set; }

        public DateTime dCreateTime { get; set; }

        public DateTime? dUpdateTime { get; set; }
    }
}

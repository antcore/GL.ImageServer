namespace GL.DBOptions.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GL_Directory
    {
        [Key]
        [StringLength(32)]
        public string sId { get; set; }

        [Required]
        [StringLength(32)]
        public string sAppId { get; set; }

        [Required]
        [StringLength(32)]
        public string sPId { get; set; }

        [Required]
        [StringLength(30)]
        public string sDirName { get; set; }

        [StringLength(250)]
        public string sDirExplain { get; set; }

        public bool bIsDefault { get; set; }

        public bool bState { get; set; }

        public bool bIsDelete { get; set; }

        public DateTime dCreateTime { get; set; }

        public DateTime? dUpdateTime { get; set; }
    }
}

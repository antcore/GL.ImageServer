namespace GL.DBOptions.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GL_APP_Authorized
    {
        [Key]
        [StringLength(32)]
        public string sId { get; set; }

        [Required]
        [StringLength(32)]
        public string sMemberId { get; set; }

        [Required]
        [StringLength(50)]
        public string sAppName { get; set; }
         
        [Required]
        [StringLength(32)]
        public string sAppKey { get; set; }

        [Required]
        [StringLength(16)]
        public string sAppSecret { get; set; }

        public bool bState { get; set; }

        public bool bIsDelete { get; set; }

        public DateTime dCreateTime { get; set; }

        public DateTime? dUpdateTime { get; set; }
    }
}

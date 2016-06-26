namespace GL.DBOptions.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

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
        [StringLength(16)]
        public string sAppId { get; set; }

        [Required]
        [StringLength(16)]
        public string sAppKey { get; set; }

        [Required]
        [StringLength(32)]
        public string sAppSecret { get; set; }

        public bool bState { get; set; }

        public bool bIsDelete { get; set; }

        public DateTime dCreateTime { get; set; }

        public DateTime? dUpdateTime { get; set; }
    }
}
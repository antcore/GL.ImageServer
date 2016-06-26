namespace GL.DBOptions.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class GL_Member
    {
        [Key]
        [StringLength(32)]
        public string sId { get; set; }

        public string sUserName { get; set; }

        [StringLength(30)]
        public string sUserEmail { get; set; }

        public string sUserPhone { get; set; }

        [Required]
        [StringLength(32)]
        public string sUserPwd { get; set; }

        public bool bState { get; set; }

        public bool bIsDelete { get; set; }

        public DateTime dCreateTime { get; set; }

        public DateTime? dUpdateTime { get; set; }
    }
}
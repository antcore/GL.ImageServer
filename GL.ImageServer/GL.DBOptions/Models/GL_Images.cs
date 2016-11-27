namespace GL.DBOptions.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class GL_Images
    {
        [Key]
        [StringLength(32)]
        public string sId { get; set; }

        [StringLength(32)]
        public string sDirId { get; set; }

        [Required]
        [StringLength(32)]
        public string sAppId { get; set; }

        [Required]
        [StringLength(100)]
        public string sUriDomain { get; set; }

        [Required]
        [StringLength(100)]
        public string sUriPath { get; set; }

        /// <summary>
        /// ÅäÖÃÐéÄâÄ¿Â¼ ÏÂÔØÂ·¾¶
        /// </summary>
        [Required]
        [StringLength(100)]
        public string sVirtual_DirectoryPath { get; set; }

        public int iSource { get; set; }

        [StringLength(100)]
        public string sFileName { get; set; }

        [Required]
        [StringLength(10)]
        public string sFileSiffix { get; set; }

        public long iFileSize { get; set; }

        [StringLength(150)]
        public string sFilePath { get; set; }

        public bool bState { get; set; }

        public bool bIsDelete { get; set; }

        public DateTime dCreateTime { get; set; }

        public DateTime? dUpdateTime { get; set; }
    }
}
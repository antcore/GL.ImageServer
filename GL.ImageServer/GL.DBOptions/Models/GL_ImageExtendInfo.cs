namespace GL.DBOptions.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class GL_ImageExtendInfo
    {
        [Key]
        [StringLength(32)]
        public string sId { get; set; }

        public double? fImgSizeHight { get; set; }

        public double? fImgSizeWidth { get; set; }

        [StringLength(128)]
        public string sProductNum { get; set; }

        public DateTime? dPhotoCreatime { get; set; }

        [StringLength(20)]
        public string sLng { get; set; }

        [StringLength(20)]
        public string sLat { get; set; }
    }
}
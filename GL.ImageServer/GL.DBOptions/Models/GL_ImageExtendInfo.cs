namespace GL.DBOptions.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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

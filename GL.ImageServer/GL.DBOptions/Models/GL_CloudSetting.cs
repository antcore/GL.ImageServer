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
        /// ����������
        /// </summary>
        [Required]
        [StringLength(30)]
        public string sServerName { get; set; }
        /// <summary>
        /// ��������ַ  http://img.gl.cn  http://127.0.0.1
        /// </summary>
        [Required]
        [StringLength(50)]
        public string sServerUriDomain { get; set; }
        /// <summary>
        /// �ļ��洢�ڷ������̷�  Ĭ�� C:
        /// </summary>
        [Required]
        [StringLength(20)]
        public string sSaveDisc { get; set; }
        /// <summary>
        /// �ļ��洢�ļ�������·��   Ĭ�� /UpLoad ��/��ͷ
        /// </summary>
        [Required]
        [StringLength(50)]
        public string sSavePath { get; set; }

        /// <summary>
        /// ������  Ψһ��ʶ��  �������ֲ�ͬ��������Ϣ 
        /// </summary>
        [Required]
        [StringLength(32)]
        public string sServerCode { get; set; }
        /// <summary>
        /// ��ע��Ϣ
        /// </summary>
        [StringLength(250)]
        public string sRemark { get; set; }
        /// <summary>
        /// Ĭ�Ϸ��� Ĭ��[0]�� 1��
        /// </summary>
        public bool bIsDefault { get; set; }
        /// <summary>
        /// ���ݿ���״̬ Ĭ��[1]��Ч  0 ��Ч
        /// </summary>
        public bool bState { get; set; }
        /// <summary>
        /// �߼�ɾ����ʶ Ĭ��[0]δɾ�� 1��ɾ��
        /// </summary>
        public bool bIsDelete { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime dCreateTime { get; set; }
        /// <summary>
        /// �޸�ʱ��
        /// </summary>
        public DateTime? dUpdateTime { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// ʵ���ࣺ��������Ϣ
    /// </summary>
    public partial class ParticipantInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ���ν�ɫ����
        /// eg: ["����", "����", "����"]
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(100)]
        public string RoleNames { get; set; }

        /// <summary>
        /// ��������
        /// <para>�ڴ˴���������ʲô</para>
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// ɾ��ʱ�䣺Ϊnull����δɾ��
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// �Ƿ�ɾ��
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}

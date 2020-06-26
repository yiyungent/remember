using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class Comment : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(5000)]
        public string Content { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// �������ʱ��
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// �޵�����
        /// </summary>
        public int LikeNum { get; set; }

        /// <summary>
        /// �ȵ�����
        /// </summary>
        public int DislikeNum { get; set; }

        /// <summary>
        /// ɾ��ʱ�䣺Ϊnull����δɾ��
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// �Ƿ�ɾ��
        /// </summary>
        public bool IsDeleted { get; set; }

        #region Relationships

        /// <summary>
        /// ����
        /// </summary>
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual UserInfo Author { get; set; }

        /// <summary>
        /// �������ۻظ���˭
        /// </summary>
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Comment Parent { get; set; }

        /// <summary>
        /// ����Щ���ۻظ��˴�������
        /// </summary>
        [InverseProperty("Parent")]
        public virtual ICollection<Comment> Children { get; set; }

        #endregion
    }
}

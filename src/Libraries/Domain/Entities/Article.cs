using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class Article : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [StringLength(30)]
        public string Title { get; set; }

        /// <summary>
        /// ����/ժҪ
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// ����ͼ
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(1000)]
        public string PicUrl { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(8000)]
        public string Content { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// �������
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// �Զ���Url
        /// </summary>
        [StringLength(1000)]
        [Column(TypeName = "text")]
        public string CustomUrl { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int LikeNum { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int DislikeNum { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public int ShareNum { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public int CommentNum { get; set; }

        /// <summary>
        /// �ղ���Ŀ
        /// </summary>
        public int FavNum { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public AStatus ArticleStatus { get; set; }

        /// <summary>
        /// ����״̬
        /// </summary>
        public CStatus CommentStatus { get; set; }

        /// <summary>
        /// �ɼ��̶�
        /// </summary>
        public OStatus OpenStatus { get; set; }

        #region Relationships

        /// <summary>
        /// ����
        /// </summary>
        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual UserInfo Author { get; set; }

        /// <summary>
        /// ɾ��ʱ�䣺Ϊnull����δɾ��
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// �Ƿ�ɾ��
        /// </summary>
        public bool IsDeleted { get; set; }

        #endregion

        public enum AStatus
        {
            /// <summary>
            /// ������
            /// </summary>
            Publish = 0,

            /// <summary>
            /// �༭�У��ݸ�״̬��
            /// </summary>
            Draft = 1,
        }

        public enum CStatus
        {
            /// <summary>
            /// ��������
            /// </summary>
            Open,

            /// <summary>
            /// �ر�����
            /// </summary>
            Closed,
        }

        public enum OStatus
        {
            /// <summary>
            /// �����˿ɼ�
            /// </summary>
            All,

            /// <summary>
            /// ���Լ��ɼ�
            /// </summary>
            Self,
            /// <summary>
            /// �Լ��ͷ�˿�ɼ�
            /// </summary>
            Fans
        }
    }
}

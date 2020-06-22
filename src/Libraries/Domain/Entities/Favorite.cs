namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    /// <summary>
    /// �ղؼ�
    /// </summary>
    public partial class Favorite : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// �ղؼ���
        /// </summary>
        [StringLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// �ղؼ�����
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// �Ƿ񹫿�
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }

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
        /// �ղؼеĴ�����
        /// </summary>
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual UserInfo Creator { get; set; }

        /// <summary>
        /// �ղص������б�
        /// </summary>
        public virtual ICollection<Favorite_Article> Favorite_Articles { get; set; }

        #endregion

        #region Helpers

        [NotMapped]
        public IList<Article> Articles
        {
            get
            {
                IList<Article> bookInfos = new List<Article>();
                if (this.Favorite_Articles != null && this.Favorite_Articles.Count >= 1)
                {
                    bookInfos = this.Favorite_Articles.Select(m => m.Article)?.ToList();
                }

                return bookInfos;
            }
        }

        #endregion
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// ׷����A-��׷��/��ע��B
    /// <para>A��עB -> A��Ϊ��B�ķ�˿</para>
    /// </summary>
    public partial class Follower_Followed : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ����/��עʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }

        #region Relationships

        /// <summary>
        /// ׷����A
        /// </summary>
        [ForeignKey("Follower")]
        public int FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public virtual UserInfo Follower { get; set; }

        /// <summary>
        /// ����ע/׷�����B
        /// </summary>
        [ForeignKey("Followed")]
        public int FollowedId { get; set; } 
        [ForeignKey("FollowedId")]
        public virtual UserInfo Followed { get; set; }

        #endregion
    }
}

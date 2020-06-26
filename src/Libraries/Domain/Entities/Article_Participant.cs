using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// ʵ���ࣺ����-������
    /// </summary>
    public partial class Article_Participant : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// �Ƿ񱻲�����ͬ��
        /// <para>
        /// Ͷ���߷���Ͷ��ʱ��Ĭ��Ϊ0,����ͬ�⣬Ȼ�󵱲�����ͬ���Ϊ1�����������ڴ�������ʾ�˲�����
        /// </para>
        /// </summary>
        public bool IsAgreed { get; set; }

        /// <summary>
        /// ������ͬ����� ʱ��
        /// </summary>
        public DateTime? AgreeTime { get; set; }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }

        #region Relationships

        /// <summary>
        /// ����
        /// </summary>
        [ForeignKey("Article")]
        public int ArticleId { get; set; }
        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [ForeignKey("Participant")]
        public int ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public virtual UserInfo Participant { get; set; }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        [ForeignKey("ParticipantInfo")]
        public int ParticipantInfoId { get; set; }
        [ForeignKey("ParticipantInfoId")]
        public virtual ParticipantInfo ParticipantInfo { get; set; }

        #endregion
    }
}

namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

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
        /// Ͷ���߷���Ͷ��ʱ��Ĭ��Ϊ0,����ͬ�⣬Ȼ�󵱲�����ͬ���Ϊ1�����������ڴ˿γ���ʾ�˲�����
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

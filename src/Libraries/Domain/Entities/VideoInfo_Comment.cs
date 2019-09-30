namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VideoInfo_Comment : BaseEntity
    {
        public int ID { get; set; }

        #region Relationships

        [ForeignKey("VideoInfo")]
        public int? VideoInfoId { get; set; }
        [ForeignKey("VideoInfoId")]
        public virtual VideoInfo VideoInfo { get; set; }

        [ForeignKey("Comment")]
        public int? CommentId { get; set; }
        [ForeignKey("CommentId")]
        public virtual Comment Comment { get; set; }

        #endregion
    }
}

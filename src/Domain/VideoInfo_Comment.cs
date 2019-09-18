using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain
{
    /// <summary>
    /// 实体类：课件-评论
    /// </summary>
    [ActiveRecord]
    [Serializable]
    public class VideoInfo_Comment : BaseEntity<VideoInfo_Comment>
    {
        #region Relationships

        [BelongsTo(Column = "VideoInfoId")]
        public VideoInfo VideoInfo { get; set; }

        [BelongsTo(Column = "CommentId")]
        public Comment Comment { get; set; }

        #endregion
    }
}

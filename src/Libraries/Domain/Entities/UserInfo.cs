namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 用户名(唯一，可改，可作为登录使用)
        /// </summary>
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [StringLength(60)]
        public string Password { get; set; }

        /// <summary>
        /// 刷新Toke
        /// </summary>
        // TODO: 实际已经无用,等以后实现 OAuth2.0
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 选择的主体模板
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 用户头像Url地址
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string Avatar { get; set; }

        /// <summary>
        /// 邮箱(唯一，可改，可作为登录使用)
        /// </summary>
        [StringLength(30)]
        public string Email { get; set; }

        /// <summary>
        /// 手机号(唯一，可改，可作为登录使用)
        /// </summary>
        [StringLength(30)]
        public string Phone { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 硬币数
        /// </summary>
        public long? Coin { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(30)]
        public string Remark { get; set; }

        #region Relationships

        /// <summary>
        /// 角色-用户
        /// </summary>
        public virtual ICollection<Role_User> Role_Users { get; set; }

        #endregion
    }
}

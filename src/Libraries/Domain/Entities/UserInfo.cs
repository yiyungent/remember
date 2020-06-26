using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain.Entities
{
    public partial class UserInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// �û���(Ψһ���ɸģ�����Ϊ��¼ʹ��)
        /// </summary>
        [Required]
        [StringLength(30)]
        public string UserName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Required]
        [StringLength(60)]
        public string Password { get; set; }

        /// <summary>
        /// ����¼ʱ��
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// ����¼Ip
        /// </summary>
        [StringLength(60)]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// ����¼��ַ
        /// </summary>
        [StringLength(60)]
        public string LastLoginAddress { get; set; }

        /// <summary>
        /// ѡ�������ģ��
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string TemplateName { get; set; }

        /// <summary>
        /// �û�ͷ��Url��ַ
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string Avatar { get; set; }

        /// <summary>
        /// ����(Ψһ���ɸģ�����Ϊ��¼ʹ��)
        /// </summary>
        [StringLength(30)]
        public string Email { get; set; }

        /// <summary>
        /// �ֻ���(Ψһ���ɸģ�����Ϊ��¼ʹ��)
        /// </summary>
        [StringLength(30)]
        public string Phone { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Credit { get; set; }

        /// <summary>
        /// ע��ʱ��
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(30)]
        public string Remark { get; set; }

        /// <summary>
        /// �û�״̬
        /// </summary>
        public UStatus UserStatus { get; set; }

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
        /// ��ɫ-�û�
        /// </summary>
        public virtual ICollection<Role_User> Role_Users { get; set; }

        #endregion

        #region Helpers

        [NotMapped]
        public IList<RoleInfo> RoleInfos
        {
            get
            {
                IList<RoleInfo> roleInfos = new List<RoleInfo>();
                if (this.Role_Users != null && this.Role_Users.Count >= 1)
                {
                    roleInfos = this.Role_Users.Select(m => m.RoleInfo).ToList();
                }

                return roleInfos;
            }
        }

        public enum UStatus
        {
            /// <summary>
            /// ����
            /// </summary>
            Normal = 0,

            /// <summary>
            /// ����
            /// </summary>
            Frozen = 1,

            /// <summary>
            /// ���Ʒ���
            /// </summary>
            Limited = 2
        }

        #endregion

    }
}

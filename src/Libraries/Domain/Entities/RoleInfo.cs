using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Domain.Entities
{
    /// <summary>
    /// ʵ����: ��ɫ
    /// </summary>
    public partial class RoleInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ��ɫ��
        /// </summary>
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        [StringLength(500)]
        public string Remark { get; set; }

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
        [InverseProperty("RoleInfo")]
        public virtual ICollection<Role_User> Role_Users { get; set; }

        /// <summary>
        /// ��ɫ-�˵�
        /// </summary>
        [InverseProperty("RoleInfo")]
        public virtual ICollection<Role_Menu> Role_Menus { get; set; }

        /// <summary>
        /// ��ɫ-Ȩ��
        /// </summary>
        [InverseProperty("RoleInfo")]
        public virtual ICollection<Role_Function> Role_Functions { get; set; }

        #endregion

        #region Helpers

        [NotMapped]
        public ICollection<UserInfo> UserInfos
        {
            get
            {
                ICollection<UserInfo> userInfos = new List<UserInfo>();
                if (this.Role_Users != null && this.Role_Users.Count >= 1)
                {
                    userInfos = this.Role_Users.Select(m => m.UserInfo).ToList();
                }

                return userInfos;
            }
        }

        [NotMapped]
        public ICollection<Sys_Menu> Sys_Menus
        {
            get
            {
                ICollection<Sys_Menu> sys_Menus = new List<Sys_Menu>();
                if (this.Role_Menus != null && this.Role_Menus.Count >= 1)
                {
                    sys_Menus = this.Role_Menus.Select(m => m.Sys_Menu).ToList();
                }

                return sys_Menus;
            }
        }

        [NotMapped]
        public ICollection<FunctionInfo> FunctionInfos
        {
            get
            {
                ICollection<FunctionInfo> functionInfos = new List<FunctionInfo>();
                if (this.Role_Functions != null && this.Role_Functions.Count >= 1)
                {
                    functionInfos = this.Role_Functions.Select(m => m.FunctionInfo).ToList();
                }

                return functionInfos;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class FunctionInfo : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// Ȩ�޼���Ψһ��ʶ��
        /// </summary>
        [Required]
        [StringLength(50)]
        public string AuthKey { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(50)]
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
        /// ϵͳ�˵�
        ///     ���һ��ϵ
        /// </summary>
        [ForeignKey("Sys_Menu")]
        public int? Sys_MenuId { get; set; }
        [ForeignKey("Sys_MenuId")]
        public virtual Sys_Menu Sys_Menu { get; set; }

        /// <summary>
        /// ��ɫ-Ȩ��
        /// </summary>
        public virtual ICollection<Role_Function> Role_Functions { get; set; }

        #endregion
    }
}

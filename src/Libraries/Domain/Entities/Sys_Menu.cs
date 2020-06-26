using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class Sys_Menu : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// �˵���
        /// </summary>
        [Required]
        [StringLength(500)]
        [Column(TypeName = "text")]
        public string Name { get; set; }

        /// <summary>
        /// �˵�����
        /// </summary>
        [StringLength(500)]
        [Column(TypeName = "text")]
        public string Description { get; set; }

        /// <summary>
        /// ͼ��
        /// </summary>
        [StringLength(500)]
        [Column(TypeName = "text")]
        public string Icon { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        [StringLength(500)]
        [Column(TypeName = "text")]
        public string ControllerName { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [StringLength(500)]
        [Column(TypeName = "text")]
        public string ActionName { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [StringLength(500)]
        [Column(TypeName = "text")]
        public string AreaName { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public int SortCode { get; set; }

        #region Relationships

        /// <summary>
        /// �ϼ��˵�
        ///     ���һ��ϵ
        /// </summary>
        [ForeignKey("Parent")]
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Sys_Menu Parent { get; set; }

        /// <summary>
        /// �Ӳ˵��б�
        ///     һ�Զ��ϵ
        /// </summary>
        [InverseProperty("Parent")]
        public virtual ICollection<Sys_Menu> Children { get; set; }

        /// <summary>
        /// ��ɫ-�˵�
        /// </summary>
        [InverseProperty("Sys_Menu")]
        public virtual ICollection<Role_Menu> Role_Menus { get; set; }

        #endregion
    }
}

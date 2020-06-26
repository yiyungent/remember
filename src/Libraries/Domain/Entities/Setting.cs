using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class Setting : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ��
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string SetKey { get; set; }

        /// <summary>
        /// ֵ
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(500)]
        public string SetValue { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string Name { get; set; }

        /// <summary>
        /// ��ע
        /// </summary>
        [Column(TypeName = "text")]
        [StringLength(1000)]
        public string Remark { get; set; }

    }
}

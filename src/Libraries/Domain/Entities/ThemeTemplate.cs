using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public partial class ThemeTemplate : BaseEntity
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// ģ����(��ʶ��Ψһ)
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string TemplateName { get; set; }

        /// <summary>
        /// ģ����⣨չʾ����
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column(TypeName = "text")]
        public string Title { get; set; }

        /// <summary>
        /// ״̬
        ///     0: ����
        ///     1: ����
        /// </summary>
        public int IsOpen { get; set; }
    }
}

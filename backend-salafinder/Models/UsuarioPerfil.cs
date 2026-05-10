using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_salafinder.Models
{
    public class UsuarioPerfil
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        [Required]
        public string identity_user_id { get; set; } = string.Empty;

        [ForeignKey("identity_user_id")]
        public IdentityUser? identity_user { get; set; }

        [Required]
        [MaxLength(100)]
        public string nombre_completo { get; set; } = string.Empty;

        public int no_shows { get; set; } = 0;

        public DateTime? bloqueado_hasta { get; set; } = null;

        public DateTime creado_en { get; set; } = DateTime.UtcNow;
    }
}

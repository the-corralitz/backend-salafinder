using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_salafinder.Models {
    public class Reserva {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public DateOnly fecha { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public TimeOnly hora_inicio { get; set; }
        public TimeOnly hora_fin { get; set; }
        public string proposito { get; set; }
        public int asistentes { get; set; } = 0;
        public DateTime creado_en { get; set; } = DateTime.UtcNow;
        public DateTime ultima_vez_modificado { get; set; } = DateTime.UtcNow;
        public string estado { get; set; } = "Pendiente";
        public Guid id_espacio { get; set; }
        [ForeignKey("id_espacio")]
        public Espacio espacio { get; set; }
    }
}

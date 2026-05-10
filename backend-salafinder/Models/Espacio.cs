using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace backend_salafinder.Models {
    public class Espacio {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string nombre { get; set; }
        public string tipo { get; set; }
        public int capacidad { get; set; }
        public string edificio { get; set; }
        public string descripcion { get; set; }
        public string[] recursos { get; set; }
        public string[] programas_prioritarios { get; set; }
        public bool requiere_aprobacion { get; set; } = false;
        public DateTime creado_en { get; set; } = DateTime.UtcNow;
        public DateTime ultima_vez_modificado { get; set; } = DateTime.UtcNow;
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace backend_salafinder.Models.DTO {
    public class ReservaDTO_Create {
        public DateOnly fecha { get; set; }
        public TimeOnly hora_inicio { get; set; }
        public TimeOnly hora_fin { get; set; }
        public string proposito { get; set; }
        public int asistentes { get; set; }
        public Guid id_espacio { get; set; }
        public Guid id_usuario { get; set; }
    }
}

namespace backend_salafinder.Models {
    public class Espacio {
        public string nombre_evento { get; set; }
        public string tipo { get; set; }
        public int capacidad { get; set; }
        public string edificio { get; set; }
        public string descripcion { get; set; }
        public string[] recursos { get; set; }
        public string[] programas_prioritarios { get; set; }
        public bool requiere_aprobacion { get; set; } = false;
    }
}

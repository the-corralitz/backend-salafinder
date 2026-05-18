namespace backend_salafinder.Models.DTO {
    public class UsuarioPerfilDTO {
        public Guid id { get; set; }
        public string nombre_completo { get; set; }
        public string email { get; set; }
        public string rol { get; set; }
        public int no_shows { get; set; }
        public DateTime? bloqueado_hasta { get; set; }
        public bool esta_bloqueado { get; set; }
    }
}

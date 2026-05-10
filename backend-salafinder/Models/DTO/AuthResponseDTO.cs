namespace backend_salafinder.Models.DTO
{
    public class AuthResponseDTO
    {
        public string token { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string nombre_completo { get; set; } = string.Empty;
        public string rol { get; set; } = string.Empty;
        public Guid usuario_perfil_id { get; set; }
        public DateTime expira_en { get; set; }
    }
}

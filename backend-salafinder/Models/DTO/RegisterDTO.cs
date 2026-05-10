namespace backend_salafinder.Models.DTO
{
    public class RegisterDTO
    {
        public string nombre_completo { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}

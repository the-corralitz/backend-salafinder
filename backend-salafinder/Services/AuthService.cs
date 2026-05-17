using backend_salafinder.Interfaces;
using backend_salafinder.Models;
using backend_salafinder.Models.DTO;
using backend_salafinder.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend_salafinder.Services {
    public class AuthService : IAuthService {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public AuthService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            ApplicationDbContext context) {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        public async Task<AuthResponseDTO> Register(RegisterDTO dto) {
            var existingUser = await _userManager.FindByEmailAsync(dto.email);
            if (existingUser != null)
                throw new Exception("Ya existe una cuenta con ese correo.");

            var identityUser = new IdentityUser {
                UserName = dto.email,
                Email = dto.email,
            };

            var result = await _userManager.CreateAsync(identityUser, dto.password);
            if (!result.Succeeded) {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            foreach (var rol in new[] { "Student", "Staff", "Admin" })
                if (!await _roleManager.RoleExistsAsync(rol))
                    await _roleManager.CreateAsync(new IdentityRole(rol));

            await _userManager.AddToRoleAsync(identityUser, "Student");

            var perfil = new UsuarioPerfil {
                identity_user_id = identityUser.Id,
                nombre_completo = dto.nombre_completo,
                no_shows = 0,
                bloqueado_hasta = null,
                creado_en = DateTime.UtcNow,
            };
            _context.UsuarioPerfil.Add(perfil);
            await _context.SaveChangesAsync();

            return GenerarAuthResponse(identityUser, perfil, "Student");
        }

        public async Task<AuthResponseDTO> Login(LoginDTO dto) {
            var identityUser = await _userManager.FindByEmailAsync(dto.email);
            if (identityUser == null)
                throw new Exception("Correo o contraseña incorrectos.");

            var isValid = await _userManager.CheckPasswordAsync(identityUser, dto.password);
            if (!isValid)
                throw new Exception("Correo o contraseña incorrectos.");

            var perfil = _context.UsuarioPerfil
                .FirstOrDefault(u => u.identity_user_id == identityUser.Id);
            if (perfil == null)
                throw new Exception("Perfil de usuario no encontrado.");

            var roles = await _userManager.GetRolesAsync(identityUser);
            var rol = roles.FirstOrDefault() ?? "Student";

            return GenerarAuthResponse(identityUser, perfil, rol);
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private AuthResponseDTO GenerarAuthResponse(
            IdentityUser identityUser,
            UsuarioPerfil perfil,
            string rol) {
            var expira = DateTime.UtcNow.AddHours(8);
            var token = GenerarJwtToken(identityUser, perfil, rol, expira);

            return new AuthResponseDTO {
                token = token,
                email = identityUser.Email!,
                nombre_completo = perfil.nombre_completo,
                rol = rol,
                usuario_perfil_id = perfil.id,
                expira_en = expira,
            };
        }

        private string GenerarJwtToken(
            IdentityUser identityUser,
            UsuarioPerfil perfil,
            string rol,
            DateTime expira) {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,   identityUser.Id),
                new Claim(JwtRegisteredClaimNames.Email, identityUser.Email!),
                new Claim(ClaimTypes.Role,               rol),
                new Claim("usuario_perfil_id",           perfil.id.ToString()),
                new Claim("nombre_completo",             perfil.nombre_completo),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expira,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPIAutores1.DTOs;

namespace WebAPIAutores1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<IdentityUser> signInManager;

        public CuentasController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }
        [HttpPost("registrar")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            var usuario = new IdentityUser {  UserName = credencialesUsuarioDTO.Email, Email = credencialesUsuarioDTO.Email };
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuarioDTO.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuarioDTO);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuarioDTO.Email, credencialesUsuarioDTO.Password,
                                                                        isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(credencialesUsuarioDTO);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }
        [HttpGet("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RespuestaAutenticacionDTO>> Renovar()
        {
            var email = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault().Value;
            var credencialesUsuario = new CredencialesUsuarioDTO()
            {
                Email = email,
            };
            return await ConstruirToken(credencialesUsuario);
        }
        private async Task<RespuestaAutenticacionDTO> ConstruirToken(CredencialesUsuarioDTO credencialesUsuarioDTO)
        {
            var claims = new List<Claim>() //claims: valores del usuario en los que confío, no son secretos
            {
                new Claim("email", credencialesUsuarioDTO.Email)
            };

            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioDTO.Email);
            var claimsDB = await userManager.GetClaimsAsync(usuario);
            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Llavejwt"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddMinutes(30);

            var securityToken = new JwtSecurityToken(issuer:null, audience:null, claims:claims, expires:expiracion, signingCredentials:creds);
            return new RespuestaAutenticacionDTO()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiracion = expiracion
            };
        }

        [HttpPost("HacerAdmin")]
        public async Task<ActionResult> HacerAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.AddClaimAsync(usuario, new Claim("EsAdmin", "1"));
            return NoContent();
        }
        [HttpPost("RemoverAdmin")]
        public async Task<ActionResult> RemoverAdmin(EditarAdminDTO editarAdminDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarAdminDTO.Email);
            await userManager.RemoveClaimAsync(usuario, new Claim("EsAdmin", "1"));
            return NoContent();
        }
    }
}

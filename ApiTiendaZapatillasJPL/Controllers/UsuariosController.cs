using ApiTiendaZapatillasJPL.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuggetTiendaZapatillasJPL.Models;
using System.Security.Claims;

namespace ApiTiendaZapatillasJPL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private RepositoryUsuarios repo;

        public UsuariosController(RepositoryUsuarios repo)
        {
            this.repo = repo;
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(string email, string password)
        {
            Usuario usuario = await this.repo.ExisteUsuarioAsync(email, password);
            if (usuario != null)
            {
                ClaimsIdentity identity =
               new ClaimsIdentity
               (CookieAuthenticationDefaults.AuthenticationScheme
               , ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim
                   (new Claim(ClaimTypes.Name, usuario.Email));
                identity.AddClaim
                    (new Claim(ClaimTypes.NameIdentifier, usuario.Password.ToString()));

                ClaimsPrincipal user = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme
                    , user);


                return Ok();
            }
            else
            {
                
                return Ok();
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Register
            (string nombre, string dni, string direccion, string telefono, string email, string password)
        {
            Usuario user = new Usuario();
            string fileName = user.IdUsuario.ToString();

            await this.repo.RegistrarUsuarioAsync(nombre, dni, direccion, telefono, email, password);
            //ViewData["MENSAJE"] = "Usuario regristado correctamente";
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Usuario>> PerfilUsuario()
        {
            //DEBEMOS BUSCAR EL CLAIM DEL EMPLEADO
            Claim claim = HttpContext.User.Claims
                .SingleOrDefault(x => x.Type == "UserData");
            string jsonUsuario =
                claim.Value;
            Usuario user = JsonConvert.DeserializeObject<Usuario>
                (jsonUsuario);
            return user;
        }
    }
}

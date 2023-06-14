using Auto_Keep.Helpers;
using Auto_Keep.Models.JWT.Usuarios;
using Auto_Keep.Services.ServiceAuthorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auto_Keep.Controllers.Authorization
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController() { }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] Usuarios usuarios)
        {
            Usuarios credentials = CredentialsRepository.GetCredential(usuarios.User, usuarios.Password, usuarios.Role);

            if(credentials is null) return Unauthorized(new AppException("Acesso negado, as credenciais passadas não são válidas!"));

            var token = TokenService.GenerateToken(credentials);
            var dataHoraValidado = DateTime.Now;
            var dataExpira = new TimeSpan(0, 30, 0);
            DateTime dt = dataHoraValidado + dataExpira;
            TimeSpan duracaoToken = dt - dataHoraValidado;
            credentials.Password = "";

            return new
            {
                credential = credentials,
                tokenJWT = token,
                dataHoraValidado = dataHoraValidado.ToString(),
                dataHoraExpira = dt.ToString(),
                duracaoToken = duracaoToken.TotalMinutes
            };
        }
    }
}

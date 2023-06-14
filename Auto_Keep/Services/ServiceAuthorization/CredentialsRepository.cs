using Auto_Keep.Models.JWT.Usuarios;

namespace Auto_Keep.Services.ServiceAuthorization
{
    public static class CredentialsRepository
    {
        public static Usuarios GetCredential(string user, string password, string role)
        {
            var usuarios = new List<Usuarios>
            {
                new Usuarios { Id = 1, User = "AdminAutoKeep", Password = "f43f5f345f34gybva!d$frf5", Role = "Administrador" },
                new Usuarios { Id = 2, User = "ClienteAutoKeep", Password = "51w6w4ff364gh4k$cc!ntm7", Role = "Cliente" }
            };
            return usuarios.Where(x => x.User.ToLower() == user.ToLower() && x.Password.ToLower() == password.ToLower() && x.Role.ToLower() == role.ToLower()).FirstOrDefault();
        }
    }
}

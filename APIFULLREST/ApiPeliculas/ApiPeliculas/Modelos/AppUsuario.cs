using Microsoft.AspNetCore.Identity;

namespace ApiPeliculas.Modelos
{
    public class AppUsuario : IdentityUser
    {

        //Anadir campos personalizados

        public string Nombre { get; set; }


    }
}

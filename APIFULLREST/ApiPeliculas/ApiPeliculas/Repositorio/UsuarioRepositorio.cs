using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;

using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;


namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {

        private readonly ApplicationDbContext _bd;
        private string claveSecreta;
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        private readonly IMapper _mapper;

        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config, IMapper mapper, RoleManager<IdentityRole> roleManager, UserManager<AppUsuario> userManager)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
            _mapper = mapper;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public AppUsuario GetUsuario(string usuarioId)
        {
            return _bd.AppUsuario.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _bd.AppUsuario.OrderBy(c => c.UserName).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var usuariobd = _bd.AppUsuario.FirstOrDefault(u => u.UserName == usuario);
            if (usuariobd == null)
            {
                return true;
            }
            return false;
        }



        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
           // var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail= usuarioRegistroDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegistroDto.Nombre
            };


            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);

            if (result.Succeeded)
            {
                if(!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }

                await _userManager.AddToRoleAsync(usuario, "registrado");
                var usuarioRetornado = _bd.AppUsuario.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);
                //opcion 1

                //return new UsuarioDatosDto()
                //{
                //    Id = usuarioRetornado.Id,
                //    Username = usuarioRetornado.UserName,
                //    Nombre = usuarioRetornado.Nombre
                //};
                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);


            }


            //_bd.Add(usuario);
            //await _bd.SaveChangesAsync();
            //usuario.Password = passwordEncriptado;
            //return new UsuarioDatosDto();
            return new UsuarioDatosDto();
        }


        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            //Encriptar password a md5  antes de enviar la consulta
            //var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);

            var usuario = _bd.AppUsuario.FirstOrDefault
                (u => u.UserName.ToLower() == usuarioLoginDto.NombreUsuario.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(usuario, usuarioLoginDto.Password);

            //Validamos si el usuario no existe con la combinación de usuario y contraseña correcta
            if (usuario == null || isValid == false)
            {
                //return null;
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null
                };
            }

            //Aquí existe el usuario entonces podemos procesar el login
            var roles = await _userManager.GetRolesAsync(usuario);

            var manejadorToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),

            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var token = manejadorToken.CreateToken(tokenDescriptor);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new UsuarioLoginRespuestaDto()
            {
                Token = manejadorToken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario),
            };

            return usuarioLoginRespuestaDto;


        }


        //metodo para encriptar contrasena md5

        //public static string obtenermd5(string valor)
        //{
        //    MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
        //    byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        //    data = x.ComputeHash(data);
        //    string resp = "";
        //    for (int i = 0; i < data.Length; i++)
        //        resp += data[i].ToString("x2").ToLower();
        //    return resp;
        //}


    }
}

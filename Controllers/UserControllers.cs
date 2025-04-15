using Microsoft.AspNetCore.Mvc;
using PermissivityProject.Models;
using PermissivityProject.Services;

namespace PermissivityProject.Controllers
{
    [ApiController]
    [Route("[controller]")] // agora o endpoint será /user
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        // Injeção de dependência via construtor
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add")]
        public IActionResult AddUser([FromBody] User user)
        {
            _userService.AddUser(user);
            return Ok("Usuário criado com sucesso!");
        }
    }
}

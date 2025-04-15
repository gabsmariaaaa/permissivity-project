using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissivityProject.Models;
using PermissivityProject.Services;

namespace PermissivityProject.Controllers
{
    [ApiController]
    [Route("[controller]")] 
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        
        [HttpPost("add")]
        public IActionResult AddUser([FromBody] User user)
        {
            var allowedRoles = new List<string> { "Agente", "Gerente" }; 

            
            if (string.IsNullOrWhiteSpace(user.Role) || !allowedRoles.Contains(user.Role))
            {
                return BadRequest("Papel de usuário inválido ou não permitido.");
            }

           
            _userService.AddUser(user);
            return Ok("Usuário criado com sucesso!");
        }

        
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = new List<string> { "Agente", "Gerente", "Administrador" };
            return Ok(roles);
        }

        
        [HttpGet("user/{id}")]
        [Authorize(Roles = "Gerente, Administrador")] 
        public IActionResult GetUser(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }
            return Ok(user);


        }
    }
}

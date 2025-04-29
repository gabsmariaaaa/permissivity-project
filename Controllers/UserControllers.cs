using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissivityProject.Models;
using PermissivityProject.Services;
using System.Collections.Generic;

namespace PermissivityProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AuthService _authService; 

        
        public UserController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService; 
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

        
        [HttpGet("dashboard")]
        [Authorize] 
        public IActionResult GetDashboard()
        {
            
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == "Agente")
            {
                return Ok(new { mensagem = "Visualização de Agente", dados = "Informações básicas" });
            }
            else if (role == "Gerente")
            {
                return Ok(new { mensagem = "Visualização de Gerente", dados = "Relatórios e estatísticas" });
            }
            else if (role == "Administrador")
            {
                return Ok(new { mensagem = "Visualização de Administrador", dados = "Configurações e gerenciamento de usuários" });
            }

            return Unauthorized("Papel de usuário não autorizado para acessar esta visualização.");
        }

        
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel login)
        {
            if (login == null || !ModelState.IsValid)
            {
                return BadRequest("Dados inválidos");
            }

            var user = _userService.Authenticate(login.Email, login.Password);
            if (user == null)
                return Unauthorized("Credenciais inválidas.");

            
            var token = _authService.GenerateJwtToken(user);
            return Ok(new
            {
                token = token,
                userId = user.Id
            });
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Dados inválidos.");

            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                PasswordHash = model.Password, 
                Role = model.Role
            };
            _userService.AddUser(user);

            return Ok(new
            {
                message = "Usuário registrado com sucesso.",
                userId = user.Id 
            });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Gerente, Administrador")] 
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();

            
            var result = users.Select(u => new
            {
                id = u.Id,
                name = u.Name,
                email = u.Email,
                role = u.Role
            });

            return Ok(result);
        }

    }
}

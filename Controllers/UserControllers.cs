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
        private readonly AuthService _authService; // Adicionando o AuthService

        // Injeção das dependências no construtor
        public UserController(UserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService; // Inicializando o AuthService
        }

        // Endpoint para adicionar usuário
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

        // Endpoint para listar os papéis disponíveis
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = new List<string> { "Agente", "Gerente", "Administrador" };
            return Ok(roles);
        }

        // Endpoint para buscar um usuário pelo ID (somente para Gerentes ou Administradores)
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

        // Endpoint para dashboard com visualização diferente para cada papel
        [HttpGet("dashboard")]
        [Authorize] // Todos precisam estar autenticados
        public IActionResult GetDashboard()
        {
            // Obter o papel do usuário atual
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            // Verificar o papel do usuário e retornar dados específicos
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

        // Endpoint para login e geração de token JWT
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

            // Gerando o token JWT
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
                PasswordHash = model.Password, // Em produção, aplique hash aqui!
                Role = model.Role
            };
            _userService.AddUser(user);

            return Ok(new
            {
                message = "Usuário registrado com sucesso.",
                userId = user.Id // Retornando o ID gerado
            });
        }

        [HttpGet("all")]
        [Authorize(Roles = "Gerente, Administrador")] // Apenas Gerente e Admin podem ver a lista
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();

            // Vamos retornar só informações básicas (não a senha/hash)
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

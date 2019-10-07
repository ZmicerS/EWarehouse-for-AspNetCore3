using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EWarehouse.Web.Services.Authorization;
using EWarehouse.Web.Models.Account;
using EWarehouse.Services.Contracts;
using EWarehouse.Services.Entities.AccountModels;

namespace EWarehouse.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AccountController> _logger;
        private readonly IJwtTokenService _jwtTokenService;
        public AccountController(IMapper mapper, 
                                 IAccountService accountService, 
                                 IAuthorizationService authorizationService,
                                 ILogger<AccountController> logger,
                                 IJwtTokenService jwtTokenService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _logger = logger;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        [Route("account/register")]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            _logger.LogInformation("Register");

            var registerServiceModel = _mapper.Map<RegisterServiceModel>(model);
            var accountServiceModel = _mapper.Map<AccountServiceModel>(model);

            var hasUser = await _accountService.FindUserAsync(accountServiceModel);
            if (hasUser == true)
            {
                ModelState.AddModelError("user", "user exist");

                var Errors = ModelState.SelectMany(x => x.Value.Errors)
                 .Select(x => x.ErrorMessage).ToArray();
                _logger.LogError("Register : user exist");
                return BadRequest(Errors);
            }

            await _accountService.RegisterUserAsync(registerServiceModel);

            return Ok();
        }

        [HttpPost("/token")]
        public IActionResult Token([FromBody] LoginViewModel model)
        {
            _logger.LogInformation("Login");
            var loginServiceModel = _mapper.Map<LoginServiceModel>(model);

            var response = _jwtTokenService.CreateJwtToken(loginServiceModel);

            if (response == null)
            {
                Response.StatusCode = 400;
                return BadRequest("Invalid username or password.");
            }
            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [Route("account/getusers")]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("GetUsers");
            var users = await _accountService.GetUsersAsync();
            var userCollection = _mapper.Map<List<UserViewModel>>(users);

            return Ok(userCollection);
        }

        [HttpGet]
        [Route("account/getuser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            _logger.LogInformation("GetUser");
            var user = await _accountService.GetUserAsync(id);
            var userModel = _mapper.Map<UserViewModel>(user);
            return Ok(userModel);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [Route("account/getroles")]
        public async Task<IActionResult> GetRoles()
        {
            _logger.LogInformation("GetRoles");
            var roles = await _accountService.GetRolesAsync();
            var roleCollection = _mapper.Map<List<RoleViewModel>>(roles);
            return Ok(roleCollection);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [Route("account/assignroles")]
        public async Task<IActionResult> AssignRoles([FromBody] UserViewModel viewModel)
        {
            _logger.LogInformation("AssignRoles");
            var userModel = _mapper.Map<UserServiceModel>(viewModel);
            await _accountService.AssignRoleToUserAsync(userModel);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "SuperAdmin")]
        [Route("account/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Delete");
            await _accountService.DeleteUserAsync(id);
            return Ok();
        }

        [HttpGet]
        [Route("account/getpermission")]
        public async Task<IActionResult> GetPermission()
        {
            var roles = new List<string> { "SuperAdmin" };

            var result = await _authorizationService.AuthorizeAsync(HttpContext.User, null, new RolesRequirement(roles));
            if (result.Succeeded)
            {
                return Ok(new { result = true });
            }
            return Ok(new { result = false });
        }

    }
}

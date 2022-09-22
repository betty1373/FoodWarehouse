using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using FW.Identity.ViewModels;
using FW.Identity.Data.Models;

namespace FW.Identity.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interactionService) =>
            (_signInManager, _userManager, _interactionService) =
            (signInManager, userManager, interactionService);

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser
            {
                UserName = viewModel.Username,
                Email = viewModel.Email,
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "user");
                await _signInManager.SignInAsync(user, false);
                return Ok();
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userManager.FindByNameAsync(viewModel.Username);
            if (user == null)
                return StatusCode(StatusCodes.Status401Unauthorized, "Incorrect username or password");

            var result = await _signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, false, false);

            if (result.Succeeded)
                return Ok();
  
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("[action]")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            await _interactionService.GetLogoutContextAsync(logoutId);
            return Ok();
        }
    }
}

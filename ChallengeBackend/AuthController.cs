using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace ChallengeBackend
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SendGridClient _sendGridClient;
        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, SendGridClient sendGridClient)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _sendGridClient = sendGridClient;
        }

        [HttpGet]
        public ActionResult Authorize()
        {
            HttpContext.SignInAsync(User);
            return new EmptyResult();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _signInManager.PasswordSignInAsync(loginModel.Email,
                           loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);

                if (user.Succeeded)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public bool RememberMe { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = registerModel.Email, Email = registerModel.Email };
                var result = await _userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    await SendWelcomeEmail(registerModel.Email);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Ok();
        }

        public class RegisterModel
        {
            [EmailAddress]
            public string Email { get; set; }
            [MinLength(6)]
            public string Password { get; set; }
        }

        private async Task SendWelcomeEmail(string email)
        {
            var from = new EmailAddress("disneyworld@disneyworld.com", "Disney World");
            var subject = "Welcome to Disney World!";
            var to = new EmailAddress(email);
            var plainTextContent = "Welcome to Disney World!";
            var htmlContent = "<strong>Welcome to Disney World!</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await _sendGridClient.SendEmailAsync(msg);
        }
    }
}

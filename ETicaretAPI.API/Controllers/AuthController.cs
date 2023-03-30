using ETicaretAPI.Application.Features.Commands.AppUser.ConfirmEmail;
using ETicaretAPI.Application.Features.Commands.AppUser.FacebookLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.GoogleLogin;
using ETicaretAPI.Application.Features.Commands.AppUser.LoginUser;
using ETicaretAPI.Application.Features.Commands.AppUser.PasswordReset;
using ETicaretAPI.Application.Features.Commands.AppUser.RefreshToken;
using ETicaretAPI.Application.Features.Commands.AppUser.VerifyEmailConfirmToken;
using ETicaretAPI.Application.Features.Commands.AppUser.VerifyResetToken;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommandRequest request)
        {
            LoginUserCommandResponse response = await _mediator.Send(request);
            return Ok(response);

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommandRequest request)
        {
            RefreshTokenCommandResponse response = await _mediator.Send(request);
            return Ok(response);

        }


        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginCommandRequest request)
        {
            GoogleLoginCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        //TODO fix, If the email of an account previously logged in with facebook is the same as the email of an account later logged in with google, it does not log in.

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin([FromBody] FacebookLoginCommandRequest request)
        {
            FacebookLoginCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetCommandRequest request)
        {
            PasswordResetCommandResponse response = await _mediator.Send(request);
            return Ok();
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyResetTokenCommandRequest request)
        {
            VerifyResetTokenCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("email-confirm")]
        public async Task<IActionResult> EmailConfirm([FromBody] ConfirmEmailCommandRequest request)
        {
            ConfirmEmailCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPost("verify-confirm-token")]
        public async Task<IActionResult> VerifyEmailConfirmToken([FromBody] VerifyEmailConfirmTokenCommandRequest request)
        {
            VerifyEmailConfirmTokenCommandResponse response = await _mediator.Send(request);
            return Ok(response);
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using VpnHood.AppLogic.Api;
using VpnHood.AppLogic.Api.SwaggerHost.Exceptions;
using SignInResult = VpnHood.AppLogic.Api.Accounts.SignInResult;
using VpnHood.AppLogic.Api.Accounts;

namespace VpnHood.AppLogic.Api.SwaggerHost.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase, IAccountApi
{
    [HttpGet]
    public Task<Account?> Get(CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpPost("refresh")]
    public Task Refresh(CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }


    [HttpPost("sign-in")]
    public Task<SignInResult> SignIn(SignInOptions signInOptions, CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpPost("sign-out")]
    public Task SignOut(CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpDelete]
    public Task Delete(CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpGet("password-reset-url")]
    public Task<Uri?> GetPasswordResetUrl(CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

}
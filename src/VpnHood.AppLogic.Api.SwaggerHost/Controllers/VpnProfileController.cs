using Microsoft.AspNetCore.Mvc;
using VpnHood.AppLogic.Api;
using VpnHood.AppLogic.Api.SwaggerHost.Exceptions;
using VpnHood.AppLogic.Api.VpnProfiles;
using VpnHood.AppLogic.Api.Premium;

namespace VpnHood.AppLogic.Api.SwaggerHost.Controllers;

[ApiController]
[Route("api/vpn-profiles")]
public class VpnProfileController : ControllerBase, IVpnProfilesApi
{
    [HttpPut("access-keys")]
    public Task<VpnProfileInfo> AddByAccessKey(string accessKey, CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpGet("{vpnProfileId}")]
    public Task<VpnProfileInfo> Get(Guid vpnProfileId, CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpGet("{vpnProfileId}/access-code")]
    public Task<string> GetAccessCode(Guid vpnProfileId, CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpPatch("{vpnProfileId}")]
    public Task<VpnProfileInfo> Update(Guid vpnProfileId, VpnProfileUpdateParams updateParams,
        CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpDelete("{vpnProfileId}")]
    public Task Delete(Guid vpnProfileId, CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpGet("{vpnProfileId}/purchase-options")]
    public Task<AppPurchaseOptions> GetPurchaseOptions(Guid vpnProfileId, CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }
}
using Microsoft.AspNetCore.Mvc;
using VpnHood.AppLogic.Api;
using VpnHood.AppLogic.Api.SwaggerHost.Exceptions;
using VpnHood.AppLogic.Api.Billing;
using VpnHood.AppLogic.Api.Premium;

namespace VpnHood.AppLogic.Api.SwaggerHost.Controllers;

[ApiController]
[Route("api/billing")]
public class BillingController : ControllerBase, IBillingApi
{
    [HttpGet("subscription-plans")]
    public Task<IReadOnlyList<SubscriptionPlan>> GetSubscriptionPlans(CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpPost("purchase")]
    public Task Purchase(PurchaseParams purchaseParams, CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpPost("restore-purchase")]
    public Task<bool> RestorePurchase(CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpPost("subscription-management")]
    public Task OpenSubscriptionManagement(CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }

    [HttpGet("purchase-options")]
    public Task<AppPurchaseOptions> GetPurchaseOptions(Guid vpnProfileId, CancellationToken cancellationToken)
    {
        throw new SwaggerOnlyException();
    }
}
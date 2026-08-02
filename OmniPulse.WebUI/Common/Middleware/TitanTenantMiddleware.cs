using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OmniPulse.WebUI.Common.Middleware;

public class TitanTenantMiddleware
{
    private readonly RequestDelegate _next;

    public TitanTenantMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var tenantId = context.Request.Headers["X-TITAN-TENANT-ID"].ToString();

        // [HIGH_MULTITENANT_VALIDATION_LOGIC_START]

        if (string.IsNullOrEmpty(tenantId))
        {
            context.Items["TenantContext"] = "GLOBAL_SHARED";
        }
        else
        {
            context.Items["TenantContext"] = tenantId;
        }

        // [HIGH_MULTITENANT_VALIDATION_LOGIC_END]

        await _next(context);
    }
}
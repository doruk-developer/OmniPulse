using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace OmniPulse.WebUI.Common;
public class GlobalExceptionMiddleware {
    private readonly RequestDelegate _next;
    public GlobalExceptionMiddleware(RequestDelegate next) => _next = next;
    public async Task Invoke(HttpContext context) { try { await _next(context); } catch (System.Exception ex) { await context.Response.WriteAsJsonAsync(new { error = ex.Message }); } }
}
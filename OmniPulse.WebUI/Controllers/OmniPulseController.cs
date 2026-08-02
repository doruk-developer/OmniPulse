using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OmniPulse.Entities.Common;
using OmniPulse.Business.Services;

namespace OmniPulse.WebUI.Controllers;

public partial class OmniPulseController : Controller
{
    private readonly IOmniPulseService _projectService;
    private readonly ILogger<OmniPulseController> _logger;

public OmniPulseController(
        IOmniPulseService projectService, 
        ILogger<OmniPulseController> logger
        /* [PROJECT_CONTROLLER_CONSTRUCTOR_PARAM_INJECTION_POINT] */)
    {
        _projectService = projectService;
        _logger = logger;
        
    }

    [HttpGet]
    
    public async Task<IActionResult> Index()
    {
        try
        {

await InitializeModuleContextAsync();

ViewBag.EcosystemIdentity = "OmniPulse";
            ViewBag.Timestamp = DateTime.UtcNow;

            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Critical failure in OmniPulse main execution node.");
            return View("Error");
        }
    }

    [HttpGet("details")]
    
    public async Task<IActionResult> Details()
    {
        try
        {

await InitializeModuleContextAsync();

return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Detailed analysis node for OmniPulse is currently unreachable.");
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpGet("node-status")]
    
    public IActionResult GetNodeStatus()
    {

return Ok(new 
        { 
            node = "OmniPulse", 
            status = "Operational", 
            sync_time = DateTime.UtcNow,
            security_level = "Verified"
        });
    }

    private async Task InitializeModuleContextAsync()
    {
        var lifecycleMethods = this.GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(m => m.Name.StartsWith("Activate_Mod_"));

        foreach (var method in lifecycleMethods)
        {
            try
            {
                if (method.ReturnType == typeof(Task))
                {
                    await (Task)method.Invoke(this, null);
                }
                else
                {
                    method.Invoke(this, null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, $"Module sequence failed: {method.Name}");
            }
        }
    }

}
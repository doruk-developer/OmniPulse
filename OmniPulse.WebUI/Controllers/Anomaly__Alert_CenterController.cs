using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OmniPulse.Business.Services;
namespace OmniPulse.WebUI.Controllers;

public class Anomaly__Alert_CenterController : Controller
{
    private readonly IOmniPulseService _projectService;
    public Anomaly__Alert_CenterController(IOmniPulseService projectService) { _projectService = projectService; }

    public async Task<IActionResult> Index() 
    {
        ViewBag.ThreatLevel = 0.5;
        return await Task.FromResult(View());
    }
}
